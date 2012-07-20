namespace Genesis.Ambience.Controls
{
    partial class ScheduleView
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
            this._hScroll = new System.Windows.Forms.HScrollBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._view = new System.Windows.Forms.Panel();
            this._vScroll = new System.Windows.Forms.VScrollBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _hScroll
            // 
            this._hScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._hScroll.Location = new System.Drawing.Point(0, 134);
            this._hScroll.Name = "_hScroll";
            this._hScroll.Size = new System.Drawing.Size(436, 16);
            this._hScroll.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this._hScroll, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._view, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._vScroll, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(452, 150);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _view
            // 
            this._view.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._view.Dock = System.Windows.Forms.DockStyle.Fill;
            this._view.Location = new System.Drawing.Point(0, 0);
            this._view.Margin = new System.Windows.Forms.Padding(0);
            this._view.Name = "_view";
            this._view.Size = new System.Drawing.Size(436, 134);
            this._view.TabIndex = 1;
            // 
            // _vScroll
            // 
            this._vScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this._vScroll.Location = new System.Drawing.Point(436, 0);
            this._vScroll.Name = "_vScroll";
            this._vScroll.Size = new System.Drawing.Size(16, 134);
            this._vScroll.TabIndex = 2;
            // 
            // ScheduleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScheduleView";
            this.Size = new System.Drawing.Size(452, 150);
            this.Load += new System.EventHandler(this.ScheduleView_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HScrollBar _hScroll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel _view;
        private System.Windows.Forms.VScrollBar _vScroll;
    }
}
