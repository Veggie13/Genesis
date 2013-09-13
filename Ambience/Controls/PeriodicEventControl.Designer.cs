namespace Genesis.Ambience.Controls
{
    partial class PeriodicEventControl
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
            this._spnPeriod = new System.Windows.Forms.NumericUpDown();
            this._element = new Genesis.Ambience.Controls.ProviderTokenTile();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._spnVariance = new System.Windows.Forms.NumericUpDown();
            this._contents.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._spnPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._spnVariance)).BeginInit();
            this.SuspendLayout();
            // 
            // _contents
            // 
            this._contents.Controls.Add(this.tableLayoutPanel1);
            this._contents.Size = new System.Drawing.Size(135, 103);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this._spnPeriod, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._element, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._spnVariance, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(133, 101);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // _spnPeriod
            // 
            this._spnPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._spnPeriod.Location = new System.Drawing.Point(61, 3);
            this._spnPeriod.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this._spnPeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._spnPeriod.Name = "_spnPeriod";
            this._spnPeriod.Size = new System.Drawing.Size(69, 20);
            this._spnPeriod.TabIndex = 1;
            this._spnPeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._spnPeriod.ValueChanged += new System.EventHandler(this._spnPeriod_ValueChanged);
            // 
            // _element
            // 
            this._element.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._element.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this._element, 2);
            this._element.Location = new System.Drawing.Point(3, 55);
            this._element.Name = "_element";
            this._element.Size = new System.Drawing.Size(127, 43);
            this._element.TabIndex = 1;
            this._element.Token = null;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Variance:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Period:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _spnVariance
            // 
            this._spnVariance.Location = new System.Drawing.Point(61, 29);
            this._spnVariance.Name = "_spnVariance";
            this._spnVariance.Size = new System.Drawing.Size(69, 20);
            this._spnVariance.TabIndex = 3;
            this._spnVariance.ValueChanged += new System.EventHandler(this._spnVariance_ValueChanged);
            // 
            // PeriodicEventControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PeriodicEventControl";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(147, 173);
            this._contents.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._spnPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._spnVariance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown _spnPeriod;
        private ProviderTokenTile _element;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _spnVariance;




    }
}
