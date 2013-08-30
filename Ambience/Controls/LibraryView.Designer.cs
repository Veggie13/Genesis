namespace Genesis.Ambience.Controls
{
    partial class LibraryView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Aga.Controls.Tree.TreeColumn treeColumn1 = new Aga.Controls.Tree.TreeColumn();
            Aga.Controls.Tree.TreeColumn treeColumn2 = new Aga.Controls.Tree.TreeColumn();
            Aga.Controls.Tree.TreeColumn treeColumn3 = new Aga.Controls.Tree.TreeColumn();
            Aga.Controls.Tree.TreeColumn treeColumn4 = new Aga.Controls.Tree.TreeColumn();
            this._tree = new Aga.Controls.Tree.TreeViewAdv();
            this._name = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this._format = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this._length = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this._path = new Aga.Controls.Tree.NodeControls.NodeTextBox();
            this._icon = new Aga.Controls.Tree.NodeControls.NodeStateIcon();
            this.SuspendLayout();
            // 
            // _tree
            // 
            this._tree.BackColor = System.Drawing.SystemColors.Window;
            treeColumn1.Header = "Name";
            treeColumn2.Header = "Format";
            treeColumn2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            treeColumn3.Header = "Length";
            treeColumn4.Header = "Path";
            this._tree.Columns.Add(treeColumn1);
            this._tree.Columns.Add(treeColumn2);
            this._tree.Columns.Add(treeColumn3);
            this._tree.Columns.Add(treeColumn4);
            this._tree.Cursor = System.Windows.Forms.Cursors.Default;
            this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tree.DragDropMarkColor = System.Drawing.Color.Black;
            this._tree.FullRowSelect = true;
            this._tree.LineColor = System.Drawing.SystemColors.ControlDark;
            this._tree.Location = new System.Drawing.Point(0, 0);
            this._tree.Model = null;
            this._tree.Name = "_tree";
            this._tree.NodeControls.Add(this._icon);
            this._tree.NodeControls.Add(this._name);
            this._tree.NodeControls.Add(this._format);
            this._tree.NodeControls.Add(this._length);
            this._tree.NodeControls.Add(this._path);
            this._tree.SelectedNode = null;
            this._tree.Size = new System.Drawing.Size(216, 150);
            this._tree.TabIndex = 0;
            this._tree.Text = "treeViewAdv1";
            this._tree.UseColumns = true;
            // 
            // _name
            // 
            this._name.DataPropertyName = "Name";
            // 
            // _format
            // 
            this._format.Column = 1;
            this._format.DataPropertyName = "Format";
            // 
            // _length
            // 
            this._length.Column = 2;
            this._length.DataPropertyName = "Length";
            // 
            // _path
            // 
            this._path.Column = 3;
            this._path.DataPropertyName = "Path";
            this._path.Trimming = System.Drawing.StringTrimming.EllipsisPath;
            // 
            // LibraryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tree);
            this.Name = "LibraryView";
            this.Size = new System.Drawing.Size(216, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv _tree;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _name;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _format;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _length;
        private Aga.Controls.Tree.NodeControls.NodeTextBox _path;
        private Aga.Controls.Tree.NodeControls.NodeStateIcon _icon;
    }
}
