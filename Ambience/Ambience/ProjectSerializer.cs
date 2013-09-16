using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Genesis.Ambience.Scheduler;
using Genesis.Ambience.DataModel;
using Genesis.Common.Tools;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace Genesis.Ambience
{
    class ProjectSerializer
    {
        #region Helper Classes
        private class EventElementFactory : IEventProviderVisitor
        {
            private Dictionary<IEventProvider, AEventElement> _models = new Dictionary<IEventProvider, AEventElement>();

            public AEventElement GetElement(IEventProvider provider)
            {
                if (_models.ContainsKey(provider))
                {
                    return _models[provider];
                }

                this.TryVisit(provider);

                _models[provider] = Element;
                return Element;
            }

            public Dictionary<IEventProvider, AEventElement> GetModels()
            {
                return new Dictionary<IEventProvider, AEventElement>(_models);
            }

            private AEventElement Element { get; set; }

            public void Visit(DelayEventProvider provider)
            {
                Element = new DelayEventElement()
                {
                    Name = provider.Name,
                    BaseColor = Color.Gray,
                    Delay = provider.Delay,
                    Subordinate = GetElement(provider.Subordinate)
                };
            }

            public void Visit(PeriodicEventProvider provider)
            {
                Element = new PeriodicEventElement()
                {
                    Name = provider.Name,
                    BaseColor = Color.Gray,
                    Period = provider.Period,
                    Variance = provider.Variance,
                    Subordinate = GetElement(provider.Subordinate)
                };
            }

            public void Visit(RandomEventSelector provider)
            {
                var selection = provider.Selection
                    .Select(prov => (SubordinateElement)GetElement(prov))
                    .ToList();
                Element = new RandomEventElement()
                {
                    Name = provider.Name,
                    BaseColor = Color.Gray,
                    Selection = selection
                };
            }

            public void Visit(SequentialEventSelector provider)
            {
                var sequence = provider.Sequence
                    .Select(prov => (SubordinateElement)GetElement(prov))
                    .ToList();
                Element = new SequentialEventElement()
                {
                    Name = provider.Name,
                    BaseColor = Color.Gray,
                    Sequence = sequence
                };
            }

            public void Visit(SimultaneousEventProvider provider)
            {
                var group = provider.Group
                    .Select(prov => (SubordinateElement)GetElement(prov))
                    .ToList();
                Element = new SimultaneousEventElement()
                {
                    Name = provider.Name,
                    BaseColor = Color.Gray,
                    Group = group
                };
            }

            public void Visit(SoundEvent.Provider provider)
            {
                Element = new SoundEventElement()
                {
                    Name = provider.Name,
                    BaseColor = Color.Gray,
                    Resource = provider.ResourceName
                };
            }

            public void Visit(IVisitable<IEventProviderVisitor, IEventProvider> item)
            {
                throw new NotImplementedException();
            }
        }

        private class EventProviderFactory : IEventElementVisitor
        {
            private ProjectInstance _inst;
            private Dictionary<Guid, IEventProvider> _providers = new Dictionary<Guid, IEventProvider>();

            public EventProviderFactory(ProjectInstance inst)
            {
                _inst = inst;
            }

            public IEventProvider GetProvider(AEventElement element)
            {
                if (_providers.ContainsKey(element.ID))
                    return _providers[element.ID];

                this.TryVisit(element);

                _providers[element.ID] = Provider;
                return Provider;
            }

            public Dictionary<Guid, IEventProvider> GetProviders()
            {
                return new Dictionary<Guid, IEventProvider>(_providers);
            }

            private IEventProvider Provider { get; set; }

            public void Visit(DelayEventElement element)
            {
                Provider = new DelayEventProvider(element.Name)
                {
                    Delay = element.Delay,
                    Subordinate = GetProvider(element.Subordinate.Subordinate)
                };
            }

            public void Visit(PeriodicEventElement element)
            {
                Provider = new PeriodicEventProvider(element.Name)
                {
                    Period = element.Period,
                    Variance = element.Variance,
                    Subordinate = GetProvider(element.Subordinate.Subordinate)
                };
            }

            public void Visit(RandomEventElement element)
            {
                var provider = new RandomEventSelector(element.Name);
                provider.Selection.AddRange(element.Selection
                    .Select(e => GetProvider(e.Subordinate)));
                Provider = provider;
            }

            public void Visit(SequentialEventElement element)
            {
                var provider = new SequentialEventSelector(element.Name);
                provider.Sequence.AddRange(element.Sequence
                    .Select(e => GetProvider(e.Subordinate)));
                Provider = provider;
            }

            public void Visit(SimultaneousEventElement element)
            {
                var provider = new SimultaneousEventProvider(element.Name);
                provider.Group.AddRange(element.Group
                    .Select(e => GetProvider(e.Subordinate)));
                Provider = provider;
            }

            public void Visit(SoundEventElement element)
            {
                Provider = new SoundEvent.Provider(element.Name, _inst.Resources, element.Resource);
            }

            public void Visit(IVisitable<IEventElementVisitor, AEventElement> item)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Class Members
        private ProjectInstance _inst;
        #endregion

        public ProjectSerializer(ProjectInstance inst)
        {
            _inst = inst;
        }

        #region Public Operations
        public void Serialize(Stream stream)
        {
            Project proj = new Project();

            proj.Libraries = modelLibraries();
            var models = modelEvents();
            proj.Events = models.Values.ToList();
            proj.SoundBoard = modelSoundBoard(models);

            using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.ASCII) { Formatting = Formatting.Indented })
            {
                XmlSerializer serializer = Project.GetSerializer();
                serializer.Serialize(writer, proj);
            }
        }

        public void Deserialize(Stream stream)
        {
            XmlSerializer serializer = Project.GetSerializer();
            Project proj = (Project)serializer.Deserialize(stream);

            loadLibraries(proj);
            var providers = loadEvents(proj);
            loadSoundBoard(proj, providers);
        }
        #endregion

        #region Private Helpers
        #region Serialization
        private List<ALibraryElement> modelLibraries()
        {
            return _inst.Resources.Libraries
                .Select(lib => new FolderLibraryElement() { Path = lib.Path } as ALibraryElement)
                .ToList();
        }

        private Dictionary<IEventProvider, AEventElement> modelEvents()
        {
            EventElementFactory elementFactory = new EventElementFactory();
            foreach (var prov in _inst.Events)
            {
                elementFactory.GetElement(prov);
            }

            return elementFactory.GetModels();
        }

        private List<SoundBoardElement> modelSoundBoard(Dictionary<IEventProvider, AEventElement> models)
        {
            List<SoundBoardElement> result = new List<SoundBoardElement>();
            
            for (int row = 0; row < _inst.RowCount; row++)
            {
                for (int col = 0; col < _inst.ColumnCount; col++)
                {
                    if (_inst[row, col] != null)
                    {
                        result.Add(models[_inst[row, col]].ToSoundBoard(row, col));
                    }
                }
            }

            return result;
        }
        #endregion

        #region Deserialization
        private void loadLibraries(Project proj)
        {
            foreach (var lib in proj.Libraries)
            {
                _inst.Resources.LoadLibrary(lib.Path);
            }
        }

        private Dictionary<Guid, IEventProvider> loadEvents(Project proj)
        {
            EventProviderFactory factory = new EventProviderFactory(_inst);
            foreach (var evt in proj.Events)
            {
                _inst.Events.Add(factory.GetProvider(evt));
            }

            return factory.GetProviders();
        }

        private void loadSoundBoard(Project proj, Dictionary<Guid, IEventProvider> providers)
        {
            foreach (var button in proj.SoundBoard)
            {
                _inst[button.Row, button.Col] = providers[button.ID];
            }
        }
        #endregion
        #endregion
    }
}
