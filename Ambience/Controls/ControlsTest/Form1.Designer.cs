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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.providerTokenList1 = new Genesis.Ambience.Controls.ProviderTokenList();
            this.eventTokenTile2 = new Genesis.Ambience.Controls.ProviderTokenTile();
            this.eventTokenTile1 = new Genesis.Ambience.Controls.ProviderTokenTile();
            this.scheduleView1 = new Genesis.Ambience.Controls.ScheduleView();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(173, 171);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(254, 171);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(621, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(548, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(621, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(621, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "label4";
            // 
            // providerTokenList1
            // 
            this.providerTokenList1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.providerTokenList1.ColorProvider = null;
            this.providerTokenList1.Location = new System.Drawing.Point(56, 332);
            this.providerTokenList1.Name = "providerTokenList1";
            this.providerTokenList1.Size = new System.Drawing.Size(715, 94);
            this.providerTokenList1.TabIndex = 9;
            this.providerTokenList1.UseHoverScroll = true;
            this.providerTokenList1.ViewOrientation = Genesis.Ambience.Controls.ProviderTokenList.Orientation.Horizontal;
            // 
            // eventTokenTile2
            // 
            this.eventTokenTile2.AllowDrop = true;
            this.eventTokenTile2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.eventTokenTile2.Location = new System.Drawing.Point(456, 171);
            this.eventTokenTile2.Name = "eventTokenTile2";
            this.eventTokenTile2.Padding = new System.Windows.Forms.Padding(2);
            this.eventTokenTile2.Size = new System.Drawing.Size(137, 88);
            this.eventTokenTile2.TabIndex = 8;
            this.eventTokenTile2.Token = null;
            this.eventTokenTile2.TokenFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            // 
            // eventTokenTile1
            // 
            this.eventTokenTile1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.eventTokenTile1.Location = new System.Drawing.Point(335, 171);
            this.eventTokenTile1.Name = "eventTokenTile1";
            this.eventTokenTile1.Padding = new System.Windows.Forms.Padding(2);
            this.eventTokenTile1.Size = new System.Drawing.Size(115, 59);
            this.eventTokenTile1.TabIndex = 7;
            this.eventTokenTile1.Token = null;
            this.eventTokenTile1.TokenFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            // 
            // scheduleView1
            // 
            this.scheduleView1.ColorProvider = null;
            this.scheduleView1.Location = new System.Drawing.Point(12, 12);
            this.scheduleView1.Name = "scheduleView1";
            this.scheduleView1.RowHeight = 35;
            this.scheduleView1.Schedule = null;
            this.scheduleView1.ShowScale = true;
            this.scheduleView1.Size = new System.Drawing.Size(529, 153);
            this.scheduleView1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 471);
            this.Controls.Add(this.providerTokenList1);
            this.Controls.Add(this.eventTokenTile2);
            this.Controls.Add(this.eventTokenTile1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.scheduleView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Genesis.Ambience.Controls.ScheduleView scheduleView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Genesis.Ambience.Controls.ProviderTokenTile eventTokenTile1;
        private Genesis.Ambience.Controls.ProviderTokenTile eventTokenTile2;
        private Genesis.Ambience.Controls.ProviderTokenList providerTokenList1;
    }
}

