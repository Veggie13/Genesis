namespace Genesis.Ambience.Controls
{
    partial class AEventControl
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
            this._contents = new System.Windows.Forms.Panel();
            this._btnColor = new System.Windows.Forms.Button();
            this._txtName = new System.Windows.Forms.TextBox();
            this._nameLabel = new System.Windows.Forms.Label();
            this._btnApply = new System.Windows.Forms.Button();
            this._btnUndo = new System.Windows.Forms.Button();
            this._tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this._tableLayoutPanel1.SuspendLayout();
            this._tableLayoutPanel2.SuspendLayout();
            this._tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // _contents
            // 
            this._contents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._contents.Dock = System.Windows.Forms.DockStyle.Fill;
            this._contents.Location = new System.Drawing.Point(3, 32);
            this._contents.Name = "_contents";
            this._contents.Size = new System.Drawing.Size(215, 166);
            this._contents.TabIndex = 7;
            // 
            // _btnColor
            // 
            this._btnColor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._btnColor.Location = new System.Drawing.Point(195, 3);
            this._btnColor.Name = "_btnColor";
            this._btnColor.Size = new System.Drawing.Size(23, 23);
            this._btnColor.TabIndex = 6;
            this._btnColor.UseVisualStyleBackColor = false;
            // 
            // _txtName
            // 
            this._txtName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._txtName.Location = new System.Drawing.Point(47, 4);
            this._txtName.Name = "_txtName";
            this._txtName.Size = new System.Drawing.Size(142, 20);
            this._txtName.TabIndex = 5;
            this._txtName.TextChanged += new System.EventHandler(this._txtName_TextChanged);
            // 
            // _nameLabel
            // 
            this._nameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._nameLabel.AutoSize = true;
            this._nameLabel.Location = new System.Drawing.Point(3, 8);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new System.Drawing.Size(38, 13);
            this._nameLabel.TabIndex = 4;
            this._nameLabel.Text = "Name:";
            this._nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _btnApply
            // 
            this._btnApply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._btnApply.Enabled = false;
            this._btnApply.Location = new System.Drawing.Point(17, 3);
            this._btnApply.Name = "_btnApply";
            this._btnApply.Size = new System.Drawing.Size(75, 23);
            this._btnApply.TabIndex = 8;
            this._btnApply.Text = "Apply";
            this._btnApply.UseVisualStyleBackColor = true;
            this._btnApply.Click += new System.EventHandler(this._btnApply_Click);
            // 
            // _btnUndo
            // 
            this._btnUndo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._btnUndo.Enabled = false;
            this._btnUndo.Location = new System.Drawing.Point(128, 3);
            this._btnUndo.Name = "_btnUndo";
            this._btnUndo.Size = new System.Drawing.Size(75, 23);
            this._btnUndo.TabIndex = 9;
            this._btnUndo.Text = "Undo";
            this._btnUndo.UseVisualStyleBackColor = true;
            this._btnUndo.Click += new System.EventHandler(this._btnUndo_Click);
            // 
            // _tableLayoutPanel1
            // 
            this._tableLayoutPanel1.ColumnCount = 1;
            this._tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel1.Controls.Add(this._tableLayoutPanel2, 0, 0);
            this._tableLayoutPanel1.Controls.Add(this._tableLayoutPanel3, 0, 2);
            this._tableLayoutPanel1.Controls.Add(this._contents, 0, 1);
            this._tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel1.Name = "_tableLayoutPanel1";
            this._tableLayoutPanel1.RowCount = 3;
            this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel1.Size = new System.Drawing.Size(221, 230);
            this._tableLayoutPanel1.TabIndex = 10;
            // 
            // _tableLayoutPanel2
            // 
            this._tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayoutPanel2.AutoSize = true;
            this._tableLayoutPanel2.ColumnCount = 3;
            this._tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel2.Controls.Add(this._nameLabel, 0, 0);
            this._tableLayoutPanel2.Controls.Add(this._txtName, 1, 0);
            this._tableLayoutPanel2.Controls.Add(this._btnColor, 2, 0);
            this._tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this._tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel2.Name = "_tableLayoutPanel2";
            this._tableLayoutPanel2.RowCount = 1;
            this._tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel2.Size = new System.Drawing.Size(221, 29);
            this._tableLayoutPanel2.TabIndex = 0;
            // 
            // _tableLayoutPanel3
            // 
            this._tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayoutPanel3.AutoSize = true;
            this._tableLayoutPanel3.ColumnCount = 2;
            this._tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel3.Controls.Add(this._btnApply, 0, 0);
            this._tableLayoutPanel3.Controls.Add(this._btnUndo, 1, 0);
            this._tableLayoutPanel3.Location = new System.Drawing.Point(0, 201);
            this._tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this._tableLayoutPanel3.Name = "_tableLayoutPanel3";
            this._tableLayoutPanel3.RowCount = 1;
            this._tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tableLayoutPanel3.Size = new System.Drawing.Size(221, 29);
            this._tableLayoutPanel3.TabIndex = 1;
            // 
            // AEventControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._tableLayoutPanel1);
            this.Name = "AEventControl";
            this.Size = new System.Drawing.Size(221, 230);
            this._tableLayoutPanel1.ResumeLayout(false);
            this._tableLayoutPanel1.PerformLayout();
            this._tableLayoutPanel2.ResumeLayout(false);
            this._tableLayoutPanel2.PerformLayout();
            this._tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel _contents;
        private System.Windows.Forms.Button _btnColor;
        private System.Windows.Forms.TextBox _txtName;
        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.Button _btnApply;
        private System.Windows.Forms.Button _btnUndo;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel3;
    }
}
