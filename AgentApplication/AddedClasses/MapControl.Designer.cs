namespace AgentApplication.AddedClasses
{
    partial class MapControl
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
            this.components = new System.ComponentModel.Container();
            this.fullLoadTimer = new System.Windows.Forms.Timer(this.components);
            this.jsTimer = new System.Windows.Forms.Timer(this.components);
            this.leftCenterContainer = new System.Windows.Forms.SplitContainer();
            this.arrivalStationLabel = new System.Windows.Forms.Label();
            this.departureStationLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.webBrowser2 = new System.Windows.Forms.WebBrowser();
            this.label3 = new System.Windows.Forms.Label();
            this.arrivalListBox = new System.Windows.Forms.ListBox();
            this.departureListBox = new System.Windows.Forms.ListBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.leftCenterContainer)).BeginInit();
            this.leftCenterContainer.Panel1.SuspendLayout();
            this.leftCenterContainer.Panel2.SuspendLayout();
            this.leftCenterContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fullLoadTimer
            // 
            this.fullLoadTimer.Interval = 1500;
            this.fullLoadTimer.Tick += new System.EventHandler(this.fullLoadTimer_Tick);
            // 
            // jsTimer
            // 
            this.jsTimer.Enabled = true;
            this.jsTimer.Interval = 1500;
            this.jsTimer.Tick += new System.EventHandler(this.jsTimer_Tick);
            // 
            // leftCenterContainer
            // 
            this.leftCenterContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.leftCenterContainer.Location = new System.Drawing.Point(8, 8);
            this.leftCenterContainer.Name = "leftCenterContainer";
            // 
            // leftCenterContainer.Panel1
            // 
            this.leftCenterContainer.Panel1.Controls.Add(this.arrivalStationLabel);
            this.leftCenterContainer.Panel1.Controls.Add(this.departureStationLabel);
            this.leftCenterContainer.Panel1.Controls.Add(this.label1);
            this.leftCenterContainer.Panel1.Controls.Add(this.label2);
            this.leftCenterContainer.Panel1.Controls.Add(this.panel1);
            this.leftCenterContainer.Panel1.Controls.Add(this.label3);
            this.leftCenterContainer.Panel1.Controls.Add(this.arrivalListBox);
            this.leftCenterContainer.Panel1.Controls.Add(this.departureListBox);
            // 
            // leftCenterContainer.Panel2
            // 
            this.leftCenterContainer.Panel2.Controls.Add(this.webBrowser1);
            this.leftCenterContainer.Size = new System.Drawing.Size(850, 548);
            this.leftCenterContainer.SplitterDistance = 245;
            this.leftCenterContainer.TabIndex = 3;
            // 
            // arrivalStationLabel
            // 
            this.arrivalStationLabel.AutoEllipsis = true;
            this.arrivalStationLabel.AutoSize = true;
            this.arrivalStationLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.arrivalStationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.arrivalStationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arrivalStationLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.arrivalStationLabel.Location = new System.Drawing.Point(133, 30);
            this.arrivalStationLabel.MinimumSize = new System.Drawing.Size(110, 40);
            this.arrivalStationLabel.Name = "arrivalStationLabel";
            this.arrivalStationLabel.Size = new System.Drawing.Size(110, 40);
            this.arrivalStationLabel.TabIndex = 10;
            // 
            // departureStationLabel
            // 
            this.departureStationLabel.AutoEllipsis = true;
            this.departureStationLabel.AutoSize = true;
            this.departureStationLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.departureStationLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.departureStationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departureStationLabel.Location = new System.Drawing.Point(4, 30);
            this.departureStationLabel.MinimumSize = new System.Drawing.Size(110, 40);
            this.departureStationLabel.Name = "departureStationLabel";
            this.departureStationLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.departureStationLabel.Size = new System.Drawing.Size(110, 40);
            this.departureStationLabel.TabIndex = 9;
            this.departureStationLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(124, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "[Arrival Station]";
            // 
            // label2
            // 
            this.label2.AutoEllipsis = true;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "[Depart Station]";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.webBrowser2);
            this.panel1.Location = new System.Drawing.Point(4, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(238, 471);
            this.panel1.TabIndex = 12;
            // 
            // webBrowser2
            // 
            this.webBrowser2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser2.Location = new System.Drawing.Point(0, -101);
            this.webBrowser2.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser2.Name = "webBrowser2";
            this.webBrowser2.Size = new System.Drawing.Size(239, 571);
            this.webBrowser2.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(78, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "[Time Table]";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Visible = false;
            // 
            // arrivalListBox
            // 
            this.arrivalListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.arrivalListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.arrivalListBox.FormattingEnabled = true;
            this.arrivalListBox.ItemHeight = 16;
            this.arrivalListBox.Location = new System.Drawing.Point(140, 337);
            this.arrivalListBox.Name = "arrivalListBox";
            this.arrivalListBox.Size = new System.Drawing.Size(100, 212);
            this.arrivalListBox.TabIndex = 4;
            this.arrivalListBox.Visible = false;
            // 
            // departureListBox
            // 
            this.departureListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.departureListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.departureListBox.FormattingEnabled = true;
            this.departureListBox.ItemHeight = 16;
            this.departureListBox.Location = new System.Drawing.Point(4, 339);
            this.departureListBox.Name = "departureListBox";
            this.departureListBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.departureListBox.Size = new System.Drawing.Size(100, 212);
            this.departureListBox.TabIndex = 3;
            this.departureListBox.Visible = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(0, -51);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(601, 599);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // MapControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.leftCenterContainer);
            this.Name = "MapControl";
            this.Size = new System.Drawing.Size(861, 559);
            this.leftCenterContainer.Panel1.ResumeLayout(false);
            this.leftCenterContainer.Panel1.PerformLayout();
            this.leftCenterContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.leftCenterContainer)).EndInit();
            this.leftCenterContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer fullLoadTimer;
        private System.Windows.Forms.Timer jsTimer;
        private System.Windows.Forms.SplitContainer leftCenterContainer;
        private System.Windows.Forms.Label arrivalStationLabel;
        private System.Windows.Forms.Label departureStationLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser webBrowser2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox arrivalListBox;
        private System.Windows.Forms.ListBox departureListBox;
        public System.Windows.Forms.WebBrowser webBrowser1;
    }
}
