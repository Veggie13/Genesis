namespace Genesis.Ambience.Controls
{
    partial class EventProviderEditorDlg
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._btnClose = new System.Windows.Forms.Button();
            this._editorPanel = new System.Windows.Forms.Panel();
            this._btnCancel = new System.Windows.Forms.Button();
            this._itemList = new Genesis.Ambience.Controls.ProviderTokenList();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._btnClose, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._editorPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._btnCancel, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._itemList, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(457, 293);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _btnClose
            // 
            this._btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnClose.Location = new System.Drawing.Point(64, 267);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(75, 23);
            this._btnClose.TabIndex = 0;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            // 
            // _editorPanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._editorPanel, 4);
            this._editorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editorPanel.Location = new System.Drawing.Point(3, 3);
            this._editorPanel.Name = "_editorPanel";
            this._editorPanel.Size = new System.Drawing.Size(278, 258);
            this._editorPanel.TabIndex = 1;
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Enabled = false;
            this._btnCancel.Location = new System.Drawing.Point(145, 267);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Visible = false;
            // 
            // _itemList
            // 
            this._itemList.AllowDragArrange = false;
            this._itemList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._itemList.ColorProvider = null;
            this._itemList.Location = new System.Drawing.Point(287, 3);
            this._itemList.Name = "_itemList";
            this.tableLayoutPanel1.SetRowSpan(this._itemList, 2);
            this._itemList.Size = new System.Drawing.Size(166, 287);
            this._itemList.TabIndex = 3;
            this._itemList.UseHoverScroll = true;
            // 
            // EventProviderEditorDlg
            // 
            this.AcceptButton = this._btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(457, 293);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EventProviderEditorDlg";
            this.ShowIcon = false;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button _btnClose;
        private System.Windows.Forms.Panel _editorPanel;
        private System.Windows.Forms.Button _btnCancel;
        private ProviderTokenList _itemList;
    }
}