namespace Genesis.Common.Controls
{
    partial class ClientConnectionDlg
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
            this._btnConnect = new System.Windows.Forms.Button();
            this._btnCancel = new System.Windows.Forms.Button();
            this._details = new Genesis.Common.Controls.ClientConnectionDetailControl();
            this.SuspendLayout();
            // 
            // _btnConnect
            // 
            this._btnConnect.Location = new System.Drawing.Point(115, 114);
            this._btnConnect.Name = "_btnConnect";
            this._btnConnect.Size = new System.Drawing.Size(75, 23);
            this._btnConnect.TabIndex = 1;
            this._btnConnect.Text = "Connect";
            this._btnConnect.UseVisualStyleBackColor = true;
            this._btnConnect.Click += new System.EventHandler(this._btnConnect_Click);
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(196, 114);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 2;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            // 
            // _details
            // 
            this._details.Location = new System.Drawing.Point(12, 12);
            this._details.Name = "_details";
            this._details.Size = new System.Drawing.Size(259, 83);
            this._details.TabIndex = 0;
            // 
            // ClientConnectionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 149);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnConnect);
            this.Controls.Add(this._details);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientConnectionDlg";
            this.ShowIcon = false;
            this.Text = "Genesis Client Connection";
            this.ResumeLayout(false);

        }

        #endregion

        private ClientConnectionDetailControl _details;
        private System.Windows.Forms.Button _btnConnect;
        private System.Windows.Forms.Button _btnCancel;
    }
}