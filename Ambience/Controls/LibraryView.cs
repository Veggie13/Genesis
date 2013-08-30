using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Genesis.Ambience.Audio;
using Genesis.Common.Tools;
using Aga.Controls.Tree;
using System.Collections;

namespace Genesis.Ambience.Controls
{
    public partial class LibraryView : UserControl
    {
        #region Model Classes
        private class BaseNode
        {
            public string Name { get; protected set; }
            public string Format { get; protected set; }
            public string Length { get; protected set; }
            public string Path { get; protected set; }
        }

        private class LibraryNode : BaseNode
        {
            public LibraryNode(ILibrary lib)
            {
                Name = lib.Name;
                Format = "";
                Length = "";
                Path = lib.Path;
                Library = lib;
            }

            public ILibrary Library { get; private set; }
        }

        private class ItemNode : BaseNode
        {
            public ItemNode(ILibrary lib, string name)
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(name);
                Format = lib.FileFormat(name).ToString();
                Length = "";
                Path = name;
            }
        }

        private class LibraryTreeModel : ITreeModel
        {
            public LibraryTreeModel()
            {
                _libs.ItemsAdded += new SignalList<ILibrary>.ItemsEvent(_libs_ItemsAdded);
                _libs.ItemsRemoved += new SignalList<ILibrary>.ItemsEvent(_libs_ItemsRemoved);
            }

            #region Properties
            #region Libraries
            private SignalList<ILibrary> _libs = new SignalList<ILibrary>();
            public ICollection<ILibrary> Libraries
            {
                get { return _libs; }
            }
            #endregion
            #endregion

            #region Event Handlers
            private void _libs_ItemsRemoved(IEnumerable<Tuple<int, ILibrary>> items)
            {
                NodesRemoved(this, new TreeModelEventArgs(
                    new TreePath(),
                    items.Select(t => t.Item1).ToArray(),
                    items.Select(t => new LibraryNode(t.Item2) as object).ToArray()));
            }

            private void _libs_ItemsAdded(IEnumerable<Tuple<int, ILibrary>> items)
            {
                NodesInserted(this, new TreeModelEventArgs(
                    new TreePath(),
                    items.Select(t => t.Item1).ToArray(),
                    items.Select(t => new LibraryNode(t.Item2) as object).ToArray()));
            }
            #endregion

            #region ITreeModel
            public IEnumerable GetChildren(TreePath treePath)
            {
                if (treePath.IsEmpty())
                {
                    foreach (var lib in _libs)
                    {
                        LibraryNode item = new LibraryNode(lib);
                        yield return item;
                    }
                }
                else
                {
                    LibraryNode parent = treePath.LastNode as LibraryNode;
                    if (parent != null)
                    {
                        foreach (string name in parent.Library.Sounds.OrderBy(n => n))
                        {
                            yield return new ItemNode(parent.Library, name);
                        }
                    }
                    else
                        yield break;
                }
            }

            public bool IsLeaf(TreePath treePath)
            {
                return treePath.LastNode is ItemNode;
            }

            public event EventHandler<TreeModelEventArgs> NodesChanged = (o, e) => { };
            public event EventHandler<TreeModelEventArgs> NodesInserted = (o, e) => { };
            public event EventHandler<TreeModelEventArgs> NodesRemoved = (o, e) => { };
            public event EventHandler<TreePathEventArgs> StructureChanged = (o, e) => { };
            #endregion
        }
        #endregion

        #region Class Members
        private LibraryTreeModel _model = new LibraryTreeModel();
        #endregion

        public LibraryView()
        {
            InitializeComponent();

            _tree.Model = _model;
        }

        #region Properties
        public ICollection<ILibrary> Libraries
        {
            get { return _model.Libraries; }
        }
        #endregion
    }
}
