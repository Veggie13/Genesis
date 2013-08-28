namespace Genesis.Ambience
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._providerList = new Genesis.Ambience.Controls.ProviderTokenList();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._spnRowCount = new System.Windows.Forms.NumericUpDown();
            this._spnColCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this._soundBoardPanel = new System.Windows.Forms.Panel();
            this._soundBoard = new Genesis.Ambience.Controls.TokenBoard();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._schedView = new Genesis.Ambience.Controls.ScheduleView();
            this._btnPlay = new System.Windows.Forms.Button();
            this._btnStop = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this._fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._fileNameItem = new System.Windows.Forms.ToolStripMenuItem();
            this._libMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._libImportFromFileItem = new System.Windows.Forms.ToolStripMenuItem();
            this._libImportFolderItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._spnRowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._spnColCount)).BeginInit();
            this._soundBoardPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this._providerList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(938, 694);
            this.splitContainer1.SplitterDistance = 179;
            this.splitContainer1.TabIndex = 0;
            // 
            // _providerList
            // 
            this._providerList.AllowDragArrange = false;
            this._providerList.ColorProvider = null;
            this._providerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._providerList.Location = new System.Drawing.Point(0, 0);
            this._providerList.Name = "_providerList";
            this._providerList.Size = new System.Drawing.Size(179, 694);
            this._providerList.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer2.Size = new System.Drawing.Size(755, 694);
            this.splitContainer2.SplitterDistance = 497;
            this.splitContainer2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this._spnRowCount, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._spnColCount, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._soundBoardPanel, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(755, 497);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _spnRowCount
            // 
            this._spnRowCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._spnRowCount.Location = new System.Drawing.Point(3, 474);
            this._spnRowCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._spnRowCount.Name = "_spnRowCount";
            this._spnRowCount.Size = new System.Drawing.Size(50, 20);
            this._spnRowCount.TabIndex = 0;
            this._spnRowCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _spnColCount
            // 
            this._spnColCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._spnColCount.Location = new System.Drawing.Point(79, 474);
            this._spnColCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._spnColCount.Name = "_spnColCount";
            this._spnColCount.Size = new System.Drawing.Size(50, 20);
            this._spnColCount.TabIndex = 1;
            this._spnColCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 477);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "X";
            // 
            // _soundBoardPanel
            // 
            this._soundBoardPanel.AutoScroll = true;
            this.tableLayoutPanel1.SetColumnSpan(this._soundBoardPanel, 4);
            this._soundBoardPanel.Controls.Add(this._soundBoard);
            this._soundBoardPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._soundBoardPanel.Location = new System.Drawing.Point(0, 0);
            this._soundBoardPanel.Margin = new System.Windows.Forms.Padding(0);
            this._soundBoardPanel.Name = "_soundBoardPanel";
            this._soundBoardPanel.Size = new System.Drawing.Size(755, 471);
            this._soundBoardPanel.TabIndex = 5;
            // 
            // _soundBoard
            // 
            this._soundBoard.AutoSize = true;
            this._soundBoard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._soundBoard.Location = new System.Drawing.Point(4, 4);
            this._soundBoard.Margin = new System.Windows.Forms.Padding(0);
            this._soundBoard.MinimumSize = new System.Drawing.Size(70, 50);
            this._soundBoard.Name = "_soundBoard";
            this._soundBoard.Size = new System.Drawing.Size(70, 50);
            this._soundBoard.TabIndex = 0;
            this._soundBoard.TokenBoardProvider = null;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this._schedView, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this._btnPlay, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this._btnStop, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(755, 193);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // _schedView
            // 
            this._schedView.ColorProvider = null;
            this._schedView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._schedView.Location = new System.Drawing.Point(84, 3);
            this._schedView.Name = "_schedView";
            this.tableLayoutPanel2.SetRowSpan(this._schedView, 4);
            this._schedView.Schedule = null;
            this._schedView.ShowScale = true;
            this._schedView.Size = new System.Drawing.Size(668, 187);
            this._schedView.TabIndex = 0;
            // 
            // _btnPlay
            // 
            this._btnPlay.Location = new System.Drawing.Point(3, 70);
            this._btnPlay.Name = "_btnPlay";
            this._btnPlay.Size = new System.Drawing.Size(75, 23);
            this._btnPlay.TabIndex = 1;
            this._btnPlay.Text = "Play";
            this._btnPlay.UseVisualStyleBackColor = true;
            this._btnPlay.Click += new System.EventHandler(this._btnPlay_Click);
            // 
            // _btnStop
            // 
            this._btnStop.Location = new System.Drawing.Point(3, 99);
            this._btnStop.Name = "_btnStop";
            this._btnStop.Size = new System.Drawing.Size(75, 23);
            this._btnStop.TabIndex = 2;
            this._btnStop.Text = "Stop";
            this._btnStop.UseVisualStyleBackColor = true;
            this._btnStop.Click += new System.EventHandler(this._btnStop_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileMenu,
            this._libMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(938, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // _fileMenu
            // 
            this._fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileNameItem});
            this._fileMenu.Name = "_fileMenu";
            this._fileMenu.Size = new System.Drawing.Size(37, 20);
            this._fileMenu.Text = "&File";
            // 
            // _fileNameItem
            // 
            this._fileNameItem.Name = "_fileNameItem";
            this._fileNameItem.Size = new System.Drawing.Size(107, 22);
            this._fileNameItem.Text = "New...";
            this._fileNameItem.Click += new System.EventHandler(this._fileNameItem_Click);
            // 
            // _libMenu
            // 
            this._libMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._libImportFromFileItem,
            this._libImportFolderItem});
            this._libMenu.Name = "_libMenu";
            this._libMenu.Size = new System.Drawing.Size(55, 20);
            this._libMenu.Text = "&Library";
            // 
            // _libImportFromFileItem
            // 
            this._libImportFromFileItem.Name = "_libImportFromFileItem";
            this._libImportFromFileItem.Size = new System.Drawing.Size(169, 22);
            this._libImportFromFileItem.Text = "Import from File...";
            this._libImportFromFileItem.Click += new System.EventHandler(this._libImportFromFileItem_Click);
            // 
            // _libImportFolderItem
            // 
            this._libImportFolderItem.Name = "_libImportFolderItem";
            this._libImportFolderItem.Size = new System.Drawing.Size(169, 22);
            this._libImportFolderItem.Text = "Import Folder...";
            this._libImportFolderItem.Click += new System.EventHandler(this._libImportFolderItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 718);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._spnRowCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._spnColCount)).EndInit();
            this._soundBoardPanel.ResumeLayout(false);
            this._soundBoardPanel.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem _fileMenu;
        private System.Windows.Forms.ToolStripMenuItem _libMenu;
        private System.Windows.Forms.ToolStripMenuItem _libImportFromFileItem;
        private System.Windows.Forms.ToolStripMenuItem _fileNameItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown _spnRowCount;
        private System.Windows.Forms.NumericUpDown _spnColCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel _soundBoardPanel;
        private Controls.TokenBoard _soundBoard;
        private Controls.ProviderTokenList _providerList;
        private Controls.ScheduleView _schedView;
        private System.Windows.Forms.ToolStripMenuItem _libImportFolderItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button _btnPlay;
        private System.Windows.Forms.Button _btnStop;
    }
}

