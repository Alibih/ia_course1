namespace FaceApplication
{
    partial class FaceApplicationMainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rightCenterContainer = new System.Windows.Forms.SplitContainer();
            this.rightContainer = new System.Windows.Forms.SplitContainer();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.departureListBox = new System.Windows.Forms.ListBox();
            this.arrivalListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.departureStationLabel = new System.Windows.Forms.Label();
            this.arrivalStationLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.leftCenterContainer = new System.Windows.Forms.SplitContainer();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightCenterContainer)).BeginInit();
            this.rightCenterContainer.Panel1.SuspendLayout();
            this.rightCenterContainer.Panel2.SuspendLayout();
            this.rightCenterContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rightContainer)).BeginInit();
            this.rightContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leftCenterContainer)).BeginInit();
            this.leftCenterContainer.Panel1.SuspendLayout();
            this.leftCenterContainer.Panel2.SuspendLayout();
            this.leftCenterContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1086, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // rightCenterContainer
            // 
            this.rightCenterContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightCenterContainer.Location = new System.Drawing.Point(0, 24);
            this.rightCenterContainer.Name = "rightCenterContainer";
            // 
            // rightCenterContainer.Panel1
            // 
            this.rightCenterContainer.Panel1.Controls.Add(this.leftCenterContainer);
            // 
            // rightCenterContainer.Panel2
            // 
            this.rightCenterContainer.Panel2.Controls.Add(this.rightContainer);
            this.rightCenterContainer.Size = new System.Drawing.Size(1086, 548);
            this.rightCenterContainer.SplitterDistance = 767;
            this.rightCenterContainer.TabIndex = 1;
            // 
            // rightContainer
            // 
            this.rightContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightContainer.Location = new System.Drawing.Point(0, 0);
            this.rightContainer.Name = "rightContainer";
            this.rightContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.rightContainer.Size = new System.Drawing.Size(315, 548);
            this.rightContainer.SplitterDistance = 244;
            this.rightContainer.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(508, 548);
            this.webBrowser1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // departureListBox
            // 
            this.departureListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departureListBox.FormattingEnabled = true;
            this.departureListBox.ItemHeight = 16;
            this.departureListBox.Location = new System.Drawing.Point(4, 336);
            this.departureListBox.Name = "departureListBox";
            this.departureListBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.departureListBox.Size = new System.Drawing.Size(100, 212);
            this.departureListBox.TabIndex = 3;
            // 
            // arrivalListBox
            // 
            this.arrivalListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arrivalListBox.FormattingEnabled = true;
            this.arrivalListBox.ItemHeight = 16;
            this.arrivalListBox.Location = new System.Drawing.Point(153, 336);
            this.arrivalListBox.Name = "arrivalListBox";
            this.arrivalListBox.Size = new System.Drawing.Size(100, 212);
            this.arrivalListBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1, 270);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "[Depart Station]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(138, 270);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "[Arrival Station]";
            // 
            // departureStationLabel
            // 
            this.departureStationLabel.AutoSize = true;
            this.departureStationLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.departureStationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.departureStationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departureStationLabel.Location = new System.Drawing.Point(4, 286);
            this.departureStationLabel.MinimumSize = new System.Drawing.Size(110, 2);
            this.departureStationLabel.Name = "departureStationLabel";
            this.departureStationLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.departureStationLabel.Size = new System.Drawing.Size(110, 18);
            this.departureStationLabel.TabIndex = 9;
            this.departureStationLabel.Text = "_";
            this.departureStationLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // arrivalStationLabel
            // 
            this.arrivalStationLabel.AutoSize = true;
            this.arrivalStationLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.arrivalStationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arrivalStationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arrivalStationLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.arrivalStationLabel.Location = new System.Drawing.Point(141, 286);
            this.arrivalStationLabel.MinimumSize = new System.Drawing.Size(110, 2);
            this.arrivalStationLabel.Name = "arrivalStationLabel";
            this.arrivalStationLabel.Size = new System.Drawing.Size(110, 18);
            this.arrivalStationLabel.TabIndex = 10;
            this.arrivalStationLabel.Text = "_";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(78, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "[Time Table]";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // leftCenterContainer
            // 
            this.leftCenterContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftCenterContainer.Location = new System.Drawing.Point(0, 0);
            this.leftCenterContainer.Name = "leftCenterContainer";
            // 
            // leftCenterContainer.Panel1
            // 
            this.leftCenterContainer.Panel1.Controls.Add(this.label3);
            this.leftCenterContainer.Panel1.Controls.Add(this.arrivalStationLabel);
            this.leftCenterContainer.Panel1.Controls.Add(this.departureStationLabel);
            this.leftCenterContainer.Panel1.Controls.Add(this.label1);
            this.leftCenterContainer.Panel1.Controls.Add(this.label2);
            this.leftCenterContainer.Panel1.Controls.Add(this.arrivalListBox);
            this.leftCenterContainer.Panel1.Controls.Add(this.departureListBox);
            this.leftCenterContainer.Panel1.Controls.Add(this.button2);
            this.leftCenterContainer.Panel1.Controls.Add(this.button1);
            this.leftCenterContainer.Panel1.Controls.Add(this.textBox1);
            // 
            // leftCenterContainer.Panel2
            // 
            this.leftCenterContainer.Panel2.Controls.Add(this.webBrowser1);
            this.leftCenterContainer.Size = new System.Drawing.Size(767, 548);
            this.leftCenterContainer.SplitterDistance = 255;
            this.leftCenterContainer.TabIndex = 0;
            // 
            // FaceApplicationMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 572);
            this.Controls.Add(this.rightCenterContainer);
            this.Controls.Add(this.menuStrip1);
            this.Name = "FaceApplicationMainForm";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.rightCenterContainer.Panel1.ResumeLayout(false);
            this.rightCenterContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rightCenterContainer)).EndInit();
            this.rightCenterContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rightContainer)).EndInit();
            this.rightContainer.ResumeLayout(false);
            this.leftCenterContainer.Panel1.ResumeLayout(false);
            this.leftCenterContainer.Panel1.PerformLayout();
            this.leftCenterContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftCenterContainer)).EndInit();
            this.leftCenterContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.SplitContainer rightCenterContainer;
        private System.Windows.Forms.SplitContainer rightContainer;
        private System.Windows.Forms.SplitContainer leftCenterContainer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label arrivalStationLabel;
        private System.Windows.Forms.Label departureStationLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox arrivalListBox;
        private System.Windows.Forms.ListBox departureListBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.WebBrowser webBrowser1;
    }
}