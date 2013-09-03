namespace Genesis.Ambience.Controls
{
    partial class ResourceSelectionDlg
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
            this._view = new Genesis.Ambience.Controls.LibraryView();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._txtSelectedItem = new System.Windows.Forms.TextBox();
            this._btnOK = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _view
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._view, 4);
            this._view.Dock = System.Windows.Forms.DockStyle.Fill;
            this._view.Location = new System.Drawing.Point(3, 3);
            this._view.Name = "_view";
            this._view.Resources = null;
            this._view.Size = new System.Drawing.Size(512, 367);
            this._view.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 381);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selected Resource:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._view, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._txtSelectedItem, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._btnOK, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._btnCancel, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(518, 402);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // _txtSelectedItem
            // 
            this._txtSelectedItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._txtSelectedItem.Location = new System.Drawing.Point(110, 377);
            this._txtSelectedItem.Name = "_txtSelectedItem";
            this._txtSelectedItem.ReadOnly = true;
            this._txtSelectedItem.Size = new System.Drawing.Size(243, 20);
            this._txtSelectedItem.TabIndex = 2;
            // 
            // _btnOK
            // 
            this._btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOK.Location = new System.Drawing.Point(359, 376);
            this._btnOK.Name = "_btnOK";
            this._btnOK.Size = new System.Drawing.Size(75, 23);
            this._btnOK.TabIndex = 3;
            this._btnOK.Text = "OK";
            this._btnOK.UseVisualStyleBackColor = true;
            this._btnOK.Click += new System.EventHandler(this._btnOK_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(440, 376);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 4;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // ResourceSelectionDlg
            // 
            this.AcceptButton = this._btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(518, 402);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResourceSelectionDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Select a Resource";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LibraryView _view;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _txtSelectedItem;
        private System.Windows.Forms.Button _btnOK;
        private System.Windows.Forms.Button _btnCancel;
    }
}