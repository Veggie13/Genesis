namespace Genesis.Applications.Core
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._btnState = new System.Windows.Forms.Button();
            this._chkTCP = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this._chkPipes = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this._treeTriggers = new System.Windows.Forms.TreeView();
            this._btnKick = new System.Windows.Forms.Button();
            this._spnTCPPort = new System.Windows.Forms.NumericUpDown();
            this._spnPipePort = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._spnTCPPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._spnPipePort)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(398, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // _btnState
            // 
            this._btnState.Enabled = false;
            this._btnState.Location = new System.Drawing.Point(13, 28);
            this._btnState.Name = "_btnState";
            this._btnState.Size = new System.Drawing.Size(47, 23);
            this._btnState.TabIndex = 1;
            this._btnState.Text = "Start";
            this._btnState.UseVisualStyleBackColor = true;
            this._btnState.Click += new System.EventHandler(this._btnState_Click);
            // 
            // _chkTCP
            // 
            this._chkTCP.AutoSize = true;
            this._chkTCP.Location = new System.Drawing.Point(113, 33);
            this._chkTCP.Name = "_chkTCP";
            this._chkTCP.Size = new System.Drawing.Size(84, 17);
            this._chkTCP.TabIndex = 2;
            this._chkTCP.Text = "Use TCP/IP";
            this._chkTCP.UseVisualStyleBackColor = true;
            this._chkTCP.CheckedChanged += new System.EventHandler(this._chkTCP_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Port";
            // 
            // _chkPipes
            // 
            this._chkPipes.AutoSize = true;
            this._chkPipes.Location = new System.Drawing.Point(276, 33);
            this._chkPipes.Name = "_chkPipes";
            this._chkPipes.Size = new System.Drawing.Size(111, 17);
            this._chkPipes.TabIndex = 5;
            this._chkPipes.Text = "Use Named Pipes";
            this._chkPipes.UseVisualStyleBackColor = true;
            this._chkPipes.CheckedChanged += new System.EventHandler(this._chkPipes_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Port";
            // 
            // _treeTriggers
            // 
            this._treeTriggers.Enabled = false;
            this._treeTriggers.Location = new System.Drawing.Point(12, 82);
            this._treeTriggers.Name = "_treeTriggers";
            this._treeTriggers.Size = new System.Drawing.Size(374, 227);
            this._treeTriggers.TabIndex = 8;
            this._treeTriggers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._treeTriggers_AfterSelect);
            // 
            // _btnKick
            // 
            this._btnKick.Enabled = false;
            this._btnKick.Location = new System.Drawing.Point(311, 315);
            this._btnKick.Name = "_btnKick";
            this._btnKick.Size = new System.Drawing.Size(75, 23);
            this._btnKick.TabIndex = 9;
            this._btnKick.Text = "Kick";
            this._btnKick.UseVisualStyleBackColor = true;
            this._btnKick.Click += new System.EventHandler(this._btnKick_Click);
            // 
            // _spnTCPPort
            // 
            this._spnTCPPort.Location = new System.Drawing.Point(163, 56);
            this._spnTCPPort.Maximum = new decimal(new int[] {
            64000,
            0,
            0,
            0});
            this._spnTCPPort.Name = "_spnTCPPort";
            this._spnTCPPort.Size = new System.Drawing.Size(63, 20);
            this._spnTCPPort.TabIndex = 10;
            this._spnTCPPort.ValueChanged += new System.EventHandler(this._spnTCPPort_ValueChanged);
            // 
            // _spnPipePort
            // 
            this._spnPipePort.Location = new System.Drawing.Point(323, 56);
            this._spnPipePort.Maximum = new decimal(new int[] {
            64000,
            0,
            0,
            0});
            this._spnPipePort.Name = "_spnPipePort";
            this._spnPipePort.Size = new System.Drawing.Size(63, 20);
            this._spnPipePort.TabIndex = 11;
            this._spnPipePort.ValueChanged += new System.EventHandler(this._spnPipePort_ValueChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 350);
            this.Controls.Add(this._spnPipePort);
            this.Controls.Add(this._spnTCPPort);
            this.Controls.Add(this._btnKick);
            this.Controls.Add(this._treeTriggers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._chkPipes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._chkTCP);
            this.Controls.Add(this._btnState);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Core";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._spnTCPPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._spnPipePort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button _btnState;
        private System.Windows.Forms.CheckBox _chkTCP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox _chkPipes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView _treeTriggers;
        private System.Windows.Forms.Button _btnKick;
        private System.Windows.Forms.NumericUpDown _spnTCPPort;
        private System.Windows.Forms.NumericUpDown _spnPipePort;
    }
}

