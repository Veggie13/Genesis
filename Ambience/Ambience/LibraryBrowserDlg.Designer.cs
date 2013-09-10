namespace Genesis.Ambience
{
    partial class LibraryBrowserDlg
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
            this._libView = new Genesis.Ambience.Controls.LibraryView();
            this._btnNewEvent = new System.Windows.Forms.Button();
            this._btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _libView
            // 
            this._libView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._libView.Location = new System.Drawing.Point(3, 3);
            this._libView.Name = "_libView";
            this._libView.Resources = null;
            this.tableLayoutPanel1.SetRowSpan(this._libView, 3);
            this._libView.Size = new System.Drawing.Size(306, 399);
            this._libView.TabIndex = 0;
            // 
            // _btnNewEvent
            // 
            this._btnNewEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._btnNewEvent.AutoSize = true;
            this._btnNewEvent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnNewEvent.Enabled = false;
            this._btnNewEvent.Location = new System.Drawing.Point(315, 3);
            this._btnNewEvent.Name = "_btnNewEvent";
            this._btnNewEvent.Size = new System.Drawing.Size(79, 23);
            this._btnNewEvent.TabIndex = 1;
            this._btnNewEvent.Text = "Create Event";
            this._btnNewEvent.UseVisualStyleBackColor = true;
            this._btnNewEvent.Click += new System.EventHandler(this._btnNewEvent_Click);
            // 
            // _btnClose
            // 
            this._btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._btnClose.AutoSize = true;
            this._btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnClose.Location = new System.Drawing.Point(315, 32);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(79, 23);
            this._btnClose.TabIndex = 2;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._libView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._btnClose, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._btnNewEvent, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(397, 405);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // LibraryBrowserDlg
            // 
            this.AcceptButton = this._btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnClose;
            this.ClientSize = new System.Drawing.Size(397, 405);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LibraryBrowserDlg";
            this.ShowIcon = false;
            this.Text = "Library Browser";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.LibraryView _libView;
        private System.Windows.Forms.Button _btnNewEvent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button _btnClose;
    }
}