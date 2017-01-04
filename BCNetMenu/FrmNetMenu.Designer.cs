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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(181, 225);
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
            this.button2.Location = new System.Drawing.Point(90, 225);
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
            this.chkAutoStart.Location = new System.Drawing.Point(11, 20);
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
            this.groupBox1.Location = new System.Drawing.Point(11, 45);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(254, 103);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connections";
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
            this.cboMenuAppearance.Location = new System.Drawing.Point(16, 185);
            this.cboMenuAppearance.Name = "cboMenuAppearance";
            this.cboMenuAppearance.Size = new System.Drawing.Size(158, 24);
            this.cboMenuAppearance.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Appearance";
            // 
            // frmNetMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 265);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboMenuAppearance);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkAutoStart);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
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
    }
}

