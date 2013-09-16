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

            public IEnumerable<KeyValuePair<IEventProvider, AEventElement>> Models
            {
                get { return _models; }
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
        #endregion

        #region Private Helpers
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

            return elementFactory.Models.ToDictionary(p => p.Key, p => p.Value);
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
    }
}
