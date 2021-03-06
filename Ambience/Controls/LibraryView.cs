﻿using System;
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
using Genesis.Ambience.Scheduler;

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
            public LibraryNode(string name)
            {
                Name = name;
                Format = "";
                Length = "";
                Path = "";
            }
        }

        private class ItemNode : BaseNode
        {
            public ItemNode(string name, SoundEvent.IResource res)
            {
                Name = name;
                Format = "";
                Length = TimeSpan.FromSeconds(res.Length).Format(0, "M:ss");
                Path = "";
                Resource = res;
            }

            public SoundEvent.IResource Resource
            {
                get;
                private set;
            }
        }

        private class LibraryTreeModel : ITreeModel
        {
            private Dictionary<string, List<string>> _items = new Dictionary<string, List<string>>();

            public LibraryTreeModel()
            {
            }

            #region Properties
            #region Resources
            private SoundEvent.IResourceProvider _resMgr;
            public SoundEvent.IResourceProvider Resources
            {
                get { return _resMgr; }
                set
                {
                    _resMgr = value;
                    setupItems();
                    this.StructureChanged(this, new TreePathEventArgs());
                }
            }
            #endregion
            #endregion

            #region ITreeModel
            public IEnumerable GetChildren(TreePath treePath)
            {
                if (treePath.IsEmpty())
                {
                    foreach (string libName in _items.Keys.OrderBy(n => n))
                    {
                        LibraryNode item = new LibraryNode(libName);
                        yield return item;
                    }
                }
                else
                {
                    LibraryNode parent = treePath.LastNode as LibraryNode;
                    if (parent != null && _items.ContainsKey(parent.Name))
                    {
                        foreach (string name in _items[parent.Name].OrderBy(n => n))
                        {
                            yield return new ItemNode(name, _resMgr.GetResource(parent.Name + "::" + name));
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

            #region Private Helpers
            private void setupItems()
            {
                _items.Clear();
                if (_resMgr == null)
                    return;

                foreach (var item in _resMgr.GetAllSounds())
                {
                    string[] tokens = item.Split(new string[] { "::" }, StringSplitOptions.None);
                    if (!_items.ContainsKey(tokens[0]))
                        _items[tokens[0]] = new List<string>();
                    _items[tokens[0]].Add(tokens[1]);
                }
            }
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
            _tree.SelectionChanged += new EventHandler(_tree_SelectionChanged);
        }

        #region Public Operations
        public void SetSelectedResource(string resName)
        {
            var found = _tree.AllNodes.Where(n => n.IsLeaf && fullName(n).Equals(resName));
            if (found.Any())
                _tree.SelectedNode = found.First();
            else
                _tree.SelectedNode = null;
        }
        #endregion

        #region Properties
        [Browsable(false)]
        public SoundEvent.IResourceProvider Resources
        {
            get { return _model.Resources; }
            set { _model.Resources = value; }
        }

        [Browsable(false)]
        public SoundEvent.IResource SelectedResource
        {
            get
            {
                if (_tree.SelectedNode != null && _tree.SelectedNode.IsLeaf)
                    return ((ItemNode)_tree.SelectedNode.Tag).Resource;
                return null;
            }
        }
        #endregion

        #region Events
        public delegate void ResourceEventHandler(SoundEvent.IResource res);
        public event ResourceEventHandler SelectionChanged = (r) => { };
        #endregion

        #region Event Handlers
        private void _tree_SelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged(SelectedResource);
        }
        #endregion

        #region Private Helpers
        private string fullName(TreeNodeAdv node)
        {
            if (node.IsLeaf)
            {
                ItemNode item = node.Tag as ItemNode;
                return fullName(node.Parent) + "::" + item.Name;
            }
            else
            {
                LibraryNode libNode = node.Tag as LibraryNode;
                return libNode.Name;
            }
        }
        #endregion
    }
}
