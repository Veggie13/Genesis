namespace Genesis.Ambience.Controls
{
    partial class ProviderTokenTile
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
            this._panel = new MainPanel();
            this._label = new System.Windows.Forms.Label();
            this._panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panel
            // 
            this._panel.AllowDrop = true;
            this._panel.ColumnCount = 3;
            this._panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._panel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._panel.Controls.Add(this._label, 1, 1);
            this._panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panel.Location = new System.Drawing.Point(0, 0);
            this._panel.Name = "_panel";
            this._panel.RowCount = 3;
            this._panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._panel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._panel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._panel.Size = new System.Drawing.Size(184, 160);
            this._panel.TabIndex = 1;
            // 
            // _label
            // 
            this._label.AllowDrop = true;
            this._label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._label.AutoSize = true;
            this._label.Location = new System.Drawing.Point(45, 67);
            this._label.Name = "_label";
            this._label.Size = new System.Drawing.Size(94, 26);
            this._label.TabIndex = 1;
            this._label.Text = "Drag element here\r\nor right-click";
            this._label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProviderTokenTile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._panel);
            this.Name = "ProviderTokenTile";
            this.Size = new System.Drawing.Size(184, 160);
            this._panel.ResumeLayout(false);
            this._panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MainPanel _panel;
        private System.Windows.Forms.Label _label;

    }
}
