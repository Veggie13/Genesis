namespace Genesis.Ambience.Controls
{
    partial class ProviderTokenList
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._scrollRight = new System.Windows.Forms.VScrollBar();
            this._scrollLeft = new System.Windows.Forms.VScrollBar();
            this._scrollBottom = new System.Windows.Forms.HScrollBar();
            this._view = new System.Windows.Forms.Panel();
            this._flow = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this._view.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._scrollRight, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this._scrollLeft, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._scrollBottom, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._view, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(251, 228);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _scrollRight
            // 
            this._scrollRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._scrollRight.Location = new System.Drawing.Point(234, 0);
            this._scrollRight.Name = "_scrollRight";
            this._scrollRight.Size = new System.Drawing.Size(17, 211);
            this._scrollRight.TabIndex = 1;
            // 
            // _scrollLeft
            // 
            this._scrollLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._scrollLeft.Location = new System.Drawing.Point(0, 0);
            this._scrollLeft.Name = "_scrollLeft";
            this._scrollLeft.Size = new System.Drawing.Size(17, 211);
            this._scrollLeft.TabIndex = 2;
            // 
            // _scrollBottom
            // 
            this._scrollBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._scrollBottom.Location = new System.Drawing.Point(17, 211);
            this._scrollBottom.Name = "_scrollBottom";
            this._scrollBottom.Size = new System.Drawing.Size(217, 17);
            this._scrollBottom.TabIndex = 0;
            // 
            // _view
            // 
            this._view.Controls.Add(this._flow);
            this._view.Dock = System.Windows.Forms.DockStyle.Fill;
            this._view.Location = new System.Drawing.Point(17, 0);
            this._view.Margin = new System.Windows.Forms.Padding(0);
            this._view.Name = "_view";
            this._view.Size = new System.Drawing.Size(217, 211);
            this._view.TabIndex = 3;
            // 
            // _flow
            // 
            this._flow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._flow.AutoSize = true;
            this._flow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._flow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._flow.Location = new System.Drawing.Point(0, 0);
            this._flow.Margin = new System.Windows.Forms.Padding(0);
            this._flow.Name = "_flow";
            this._flow.Size = new System.Drawing.Size(2, 2);
            this._flow.TabIndex = 0;
            this._flow.WrapContents = false;
            // 
            // ProviderTokenList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ProviderTokenList";
            this.Size = new System.Drawing.Size(251, 228);
            this.tableLayoutPanel1.ResumeLayout(false);
            this._view.ResumeLayout(false);
            this._view.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.VScrollBar _scrollRight;
        private System.Windows.Forms.VScrollBar _scrollLeft;
        private System.Windows.Forms.HScrollBar _scrollBottom;
        private System.Windows.Forms.Panel _view;
        private System.Windows.Forms.FlowLayoutPanel _flow;
    }
}
