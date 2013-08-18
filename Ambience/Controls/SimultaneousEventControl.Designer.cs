namespace Genesis.Ambience.Controls
{
    partial class SimultaneousEventControl
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
            this._itemList = new Genesis.Ambience.Controls.ProviderTokenList();
            this.SuspendLayout();
            // 
            // _itemList
            // 
            this._itemList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._itemList.AutoSize = true;
            this._itemList.ColorProvider = null;
            this._itemList.Location = new System.Drawing.Point(0, 0);
            this._itemList.Margin = new System.Windows.Forms.Padding(0);
            this._itemList.Name = "_itemList";
            this._itemList.Size = new System.Drawing.Size(100, 100);
            this._itemList.TabIndex = 0;
            // 
            // RandomEventControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._itemList);
            this.Name = "RandomEventControl";
            this.Size = new System.Drawing.Size(100, 100);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ProviderTokenList _itemList;
    }
}
