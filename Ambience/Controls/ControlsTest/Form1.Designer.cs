namespace ControlsTest
{
    partial class Form1
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
            this.scheduleView1 = new Genesis.Ambience.Controls.ScheduleView();
            this.SuspendLayout();
            // 
            // scheduleView1
            // 
            this.scheduleView1.Background = System.Drawing.Color.White;
            this.scheduleView1.BorderColor = System.Drawing.Color.LightGray;
            this.scheduleView1.BorderThickness = 1F;
            this.scheduleView1.ColorProvider = null;
            this.scheduleView1.ColumnWidth = 50;
            this.scheduleView1.LeftColumn = 0;
            this.scheduleView1.Location = new System.Drawing.Point(12, 12);
            this.scheduleView1.Name = "scheduleView1";
            this.scheduleView1.RowHeight = 15;
            this.scheduleView1.ScaleBackground = System.Drawing.Color.LightGray;
            this.scheduleView1.ScaleHeight = 15;
            this.scheduleView1.Schedule = null;
            this.scheduleView1.ShowScale = true;
            this.scheduleView1.Size = new System.Drawing.Size(529, 85);
            this.scheduleView1.TabIndex = 0;
            this.scheduleView1.TokenFont = new System.Drawing.Font("Arial", 8F);
            this.scheduleView1.TokenFontColor = System.Drawing.Color.Black;
            this.scheduleView1.TopRow = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 273);
            this.Controls.Add(this.scheduleView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Genesis.Ambience.Controls.ScheduleView scheduleView1;
    }
}

