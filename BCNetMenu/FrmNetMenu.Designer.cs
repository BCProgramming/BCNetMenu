namespace BCNetMenu
{
    partial class frmNetMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNetMenu));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chkAutoStart = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radBoth = new System.Windows.Forms.RadioButton();
            this.radWireless = new System.Windows.Forms.RadioButton();
            this.radVPN = new System.Windows.Forms.RadioButton();
            this.cboMenuAppearance = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCurrentFont = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.chkDWMBlur = new System.Windows.Forms.CheckBox();
            this.chkSystemAccent = new System.Windows.Forms.CheckBox();
            this.cmdAccentColor = new System.Windows.Forms.Button();
            this.tBarIntensity = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.chkConnectionNotifications = new System.Windows.Forms.CheckBox();
            this.chkLogConnection = new System.Windows.Forms.CheckBox();
            this.btnOpenLog = new System.Windows.Forms.Button();
            this.NotificationIconSplit = new BCNetMenu.DropSplitButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBarIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(303, 590);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(212, 590);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(84, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chkAutoStart
            // 
            this.chkAutoStart.AutoSize = true;
            this.chkAutoStart.Location = new System.Drawing.Point(11, 23);
            this.chkAutoStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkAutoStart.Name = "chkAutoStart";
            this.chkAutoStart.Size = new System.Drawing.Size(148, 21);
            this.chkAutoStart.TabIndex = 2;
            this.chkAutoStart.Text = "Start with Windows";
            this.chkAutoStart.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radBoth);
            this.groupBox1.Controls.Add(this.radWireless);
            this.groupBox1.Controls.Add(this.radVPN);
            this.groupBox1.Location = new System.Drawing.Point(11, 120);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(367, 103);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show Connections";
            // 
            // radBoth
            // 
            this.radBoth.AutoSize = true;
            this.radBoth.Location = new System.Drawing.Point(5, 68);
            this.radBoth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radBoth.Name = "radBoth";
            this.radBoth.Size = new System.Drawing.Size(58, 21);
            this.radBoth.TabIndex = 6;
            this.radBoth.TabStop = true;
            this.radBoth.Text = "&Both";
            this.radBoth.UseVisualStyleBackColor = true;
            // 
            // radWireless
            // 
            this.radWireless.AutoSize = true;
            this.radWireless.Location = new System.Drawing.Point(5, 44);
            this.radWireless.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radWireless.Name = "radWireless";
            this.radWireless.Size = new System.Drawing.Size(83, 21);
            this.radWireless.TabIndex = 5;
            this.radWireless.TabStop = true;
            this.radWireless.Text = "Wireless";
            this.radWireless.UseVisualStyleBackColor = true;
            // 
            // radVPN
            // 
            this.radVPN.AutoSize = true;
            this.radVPN.Location = new System.Drawing.Point(5, 20);
            this.radVPN.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radVPN.Name = "radVPN";
            this.radVPN.Size = new System.Drawing.Size(57, 21);
            this.radVPN.TabIndex = 4;
            this.radVPN.TabStop = true;
            this.radVPN.Text = "VPN";
            this.radVPN.UseVisualStyleBackColor = true;
            // 
            // cboMenuAppearance
            // 
            this.cboMenuAppearance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMenuAppearance.FormattingEnabled = true;
            this.cboMenuAppearance.Location = new System.Drawing.Point(107, 229);
            this.cboMenuAppearance.Name = "cboMenuAppearance";
            this.cboMenuAppearance.Size = new System.Drawing.Size(179, 24);
            this.cboMenuAppearance.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Style:";
            // 
            // lblCurrentFont
            // 
            this.lblCurrentFont.AutoSize = true;
            this.lblCurrentFont.Location = new System.Drawing.Point(14, 31);
            this.lblCurrentFont.Name = "lblCurrentFont";
            this.lblCurrentFont.Size = new System.Drawing.Size(134, 17);
            this.lblCurrentFont.TabIndex = 7;
            this.lblCurrentFont.Text = "12pt. Verdana, Bold";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.lblCurrentFont);
            this.groupBox2.Location = new System.Drawing.Point(16, 478);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(376, 101);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "&Font";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(292, 58);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 30);
            this.button3.TabIndex = 8;
            this.button3.Text = "&Change...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // chkDWMBlur
            // 
            this.chkDWMBlur.AutoSize = true;
            this.chkDWMBlur.Location = new System.Drawing.Point(28, 263);
            this.chkDWMBlur.Name = "chkDWMBlur";
            this.chkDWMBlur.Size = new System.Drawing.Size(93, 21);
            this.chkDWMBlur.TabIndex = 9;
            this.chkDWMBlur.Text = "DWM Blur";
            this.chkDWMBlur.UseVisualStyleBackColor = true;
            this.chkDWMBlur.CheckedChanged += new System.EventHandler(this.chkDWMBlur_CheckedChanged);
            // 
            // chkSystemAccent
            // 
            this.chkSystemAccent.AutoSize = true;
            this.chkSystemAccent.Location = new System.Drawing.Point(28, 296);
            this.chkSystemAccent.Name = "chkSystemAccent";
            this.chkSystemAccent.Size = new System.Drawing.Size(189, 21);
            this.chkSystemAccent.TabIndex = 10;
            this.chkSystemAccent.Text = "Use System Accent Color";
            this.chkSystemAccent.UseVisualStyleBackColor = true;
            this.chkSystemAccent.CheckedChanged += new System.EventHandler(this.chkSystemAccent_CheckedChanged);
            // 
            // cmdAccentColor
            // 
            this.cmdAccentColor.Location = new System.Drawing.Point(28, 323);
            this.cmdAccentColor.Name = "cmdAccentColor";
            this.cmdAccentColor.Size = new System.Drawing.Size(202, 34);
            this.cmdAccentColor.TabIndex = 11;
            this.cmdAccentColor.Text = "Custom Accent Color...";
            this.cmdAccentColor.UseVisualStyleBackColor = true;
            this.cmdAccentColor.Click += new System.EventHandler(this.cmdAccentColor_Click);
            // 
            // tBarIntensity
            // 
            this.tBarIntensity.LargeChange = 25;
            this.tBarIntensity.Location = new System.Drawing.Point(100, 367);
            this.tBarIntensity.Maximum = 255;
            this.tBarIntensity.Name = "tBarIntensity";
            this.tBarIntensity.Size = new System.Drawing.Size(212, 56);
            this.tBarIntensity.SmallChange = 5;
            this.tBarIntensity.TabIndex = 12;
            this.tBarIntensity.TickFrequency = 5;
            this.tBarIntensity.Value = 25;
            this.tBarIntensity.ValueChanged += new System.EventHandler(this.tBarIntensity_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 374);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Intensity:";
            // 
            // chkConnectionNotifications
            // 
            this.chkConnectionNotifications.AutoSize = true;
            this.chkConnectionNotifications.Location = new System.Drawing.Point(11, 48);
            this.chkConnectionNotifications.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkConnectionNotifications.Name = "chkConnectionNotifications";
            this.chkConnectionNotifications.Size = new System.Drawing.Size(220, 21);
            this.chkConnectionNotifications.TabIndex = 14;
            this.chkConnectionNotifications.Text = "Show Connection Notifications";
            this.chkConnectionNotifications.UseVisualStyleBackColor = true;
            // 
            // chkLogConnection
            // 
            this.chkLogConnection.AutoSize = true;
            this.chkLogConnection.Location = new System.Drawing.Point(11, 74);
            this.chkLogConnection.Name = "chkLogConnection";
            this.chkLogConnection.Size = new System.Drawing.Size(189, 21);
            this.chkLogConnection.TabIndex = 17;
            this.chkLogConnection.Text = "Log Connection Changes";
            this.chkLogConnection.UseVisualStyleBackColor = true;
            // 
            // btnOpenLog
            // 
            this.btnOpenLog.Location = new System.Drawing.Point(275, 74);
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.Size = new System.Drawing.Size(103, 32);
            this.btnOpenLog.TabIndex = 18;
            this.btnOpenLog.Text = "Open Log";
            this.btnOpenLog.UseVisualStyleBackColor = true;
            this.btnOpenLog.Click += new System.EventHandler(this.btnOpenLog_Click);
            // 
            // NotificationIconSplit
            // 
            this.NotificationIconSplit.AutoSize = true;
            this.NotificationIconSplit.DropDownImage = null;
            this.NotificationIconSplit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NotificationIconSplit.Location = new System.Drawing.Point(21, 429);
            this.NotificationIconSplit.Name = "NotificationIconSplit";
            this.NotificationIconSplit.Size = new System.Drawing.Size(123, 27);
            this.NotificationIconSplit.TabIndex = 16;
            this.NotificationIconSplit.Text = "Notification Icon";
            this.NotificationIconSplit.UseVisualStyleBackColor = true;
            // 
            // frmNetMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 630);
            this.Controls.Add(this.btnOpenLog);
            this.Controls.Add(this.chkLogConnection);
            this.Controls.Add(this.NotificationIconSplit);
            this.Controls.Add(this.chkConnectionNotifications);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBarIntensity);
            this.Controls.Add(this.cmdAccentColor);
            this.Controls.Add(this.chkSystemAccent);
            this.Controls.Add(this.chkDWMBlur);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboMenuAppearance);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkAutoStart);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNetMenu";
            this.Text = "Network Menu Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNetMenu_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.frmNetMenu_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBarIntensity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chkAutoStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radBoth;
        private System.Windows.Forms.RadioButton radWireless;
        private System.Windows.Forms.RadioButton radVPN;
        private System.Windows.Forms.ComboBox cboMenuAppearance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCurrentFont;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox chkDWMBlur;
        private System.Windows.Forms.CheckBox chkSystemAccent;
        private System.Windows.Forms.Button cmdAccentColor;
        private System.Windows.Forms.TrackBar tBarIntensity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkConnectionNotifications;
        private DropSplitButton NotificationIconSplit;
        private System.Windows.Forms.CheckBox chkLogConnection;
        private System.Windows.Forms.Button btnOpenLog;
    }
}

