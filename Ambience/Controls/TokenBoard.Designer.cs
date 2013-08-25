namespace Genesis.Ambience.Controls
{
    partial class TokenBoard
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
            this._content = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // _content
            // 
            this._content.ColumnCount = 1;
            this._content.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._content.Dock = System.Windows.Forms.DockStyle.Fill;
            this._content.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this._content.Location = new System.Drawing.Point(0, 0);
            this._content.Margin = new System.Windows.Forms.Padding(0);
            this._content.Name = "_content";
            this._content.RowCount = 1;
            this._content.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._content.Size = new System.Drawing.Size(217, 213);
            this._content.TabIndex = 0;
            // 
            // TokenBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._content);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TokenBoard";
            this.Size = new System.Drawing.Size(217, 213);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _content;
    }
}
