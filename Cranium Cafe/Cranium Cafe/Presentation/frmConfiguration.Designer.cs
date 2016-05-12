namespace CraniumCafeConfig
{
    partial class frmConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfiguration));
            this.tabConfiguration = new System.Windows.Forms.TabControl();
            this.General = new System.Windows.Forms.TabPage();
            this.btnTestRest = new System.Windows.Forms.Button();
            this.btnSaveGeneral = new System.Windows.Forms.Button();
            this.grpExchange = new System.Windows.Forms.GroupBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAgentPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAgentUsername = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.grpCranium = new System.Windows.Forms.GroupBox();
            this.txtSecret = new System.Windows.Forms.TextBox();
            this.lblsecret = new System.Windows.Forms.Label();
            this.lblkey = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.UserCredentials = new System.Windows.Forms.TabPage();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddnewuser = new System.Windows.Forms.Button();
            this.dgvUserCredential = new System.Windows.Forms.DataGridView();
            this.btnTestExchange = new System.Windows.Forms.Button();
            this.tabConfiguration.SuspendLayout();
            this.General.SuspendLayout();
            this.grpExchange.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpCranium.SuspendLayout();
            this.UserCredentials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserCredential)).BeginInit();
            this.SuspendLayout();
            // 
            // tabConfiguration
            // 
            this.tabConfiguration.Controls.Add(this.General);
            this.tabConfiguration.Controls.Add(this.UserCredentials);
            this.tabConfiguration.Location = new System.Drawing.Point(0, 0);
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.SelectedIndex = 0;
            this.tabConfiguration.Size = new System.Drawing.Size(644, 533);
            this.tabConfiguration.TabIndex = 7;
            // 
            // General
            // 
            this.General.Controls.Add(this.btnSaveGeneral);
            this.General.Controls.Add(this.grpExchange);
            this.General.Controls.Add(this.groupBox3);
            this.General.Controls.Add(this.grpCranium);
            this.General.Location = new System.Drawing.Point(4, 22);
            this.General.Name = "General";
            this.General.Size = new System.Drawing.Size(636, 507);
            this.General.TabIndex = 2;
            this.General.Text = "General";
            this.General.UseVisualStyleBackColor = true;
            // 
            // btnTestRest
            // 
            this.btnTestRest.Location = new System.Drawing.Point(131, 77);
            this.btnTestRest.Name = "btnTestRest";
            this.btnTestRest.Size = new System.Drawing.Size(75, 23);
            this.btnTestRest.TabIndex = 8;
            this.btnTestRest.Text = "Test API";
            this.btnTestRest.UseVisualStyleBackColor = true;
            this.btnTestRest.Click += new System.EventHandler(this.btnTestRest_Click);
            // 
            // btnSaveGeneral
            // 
            this.btnSaveGeneral.Location = new System.Drawing.Point(558, 476);
            this.btnSaveGeneral.Name = "btnSaveGeneral";
            this.btnSaveGeneral.Size = new System.Drawing.Size(75, 23);
            this.btnSaveGeneral.TabIndex = 6;
            this.btnSaveGeneral.Text = "Save";
            this.btnSaveGeneral.UseVisualStyleBackColor = true;
            this.btnSaveGeneral.Click += new System.EventHandler(this.button1_Click);
            // 
            // grpExchange
            // 
            this.grpExchange.Controls.Add(this.btnTestExchange);
            this.grpExchange.Controls.Add(this.txtServerName);
            this.grpExchange.Controls.Add(this.label5);
            this.grpExchange.Controls.Add(this.txtAgentPassword);
            this.grpExchange.Controls.Add(this.label2);
            this.grpExchange.Controls.Add(this.label1);
            this.grpExchange.Controls.Add(this.txtAgentUsername);
            this.grpExchange.Location = new System.Drawing.Point(3, 124);
            this.grpExchange.Name = "grpExchange";
            this.grpExchange.Size = new System.Drawing.Size(625, 144);
            this.grpExchange.TabIndex = 0;
            this.grpExchange.TabStop = false;
            this.grpExchange.Text = "Exchange Credential";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(131, 78);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(478, 20);
            this.txtServerName.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Exchange Server Name";
            // 
            // txtAgentPassword
            // 
            this.txtAgentPassword.Enabled = false;
            this.txtAgentPassword.Location = new System.Drawing.Point(131, 50);
            this.txtAgentPassword.Name = "txtAgentPassword";
            this.txtAgentPassword.Size = new System.Drawing.Size(478, 20);
            this.txtAgentPassword.TabIndex = 4;
            this.txtAgentPassword.Text = "Windows authentication will be used";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Agent Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Agent Username";
            // 
            // txtAgentUsername
            // 
            this.txtAgentUsername.Location = new System.Drawing.Point(131, 24);
            this.txtAgentUsername.Name = "txtAgentUsername";
            this.txtAgentUsername.Size = new System.Drawing.Size(478, 20);
            this.txtAgentUsername.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtInterval);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(3, 270);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(625, 70);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Interval";
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(94, 30);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(52, 20);
            this.txtInterval.TabIndex = 5;
            this.txtInterval.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Polling Interval";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "(mins)";
            // 
            // grpCranium
            // 
            this.grpCranium.Controls.Add(this.txtSecret);
            this.grpCranium.Controls.Add(this.btnTestRest);
            this.grpCranium.Controls.Add(this.lblsecret);
            this.grpCranium.Controls.Add(this.lblkey);
            this.grpCranium.Controls.Add(this.txtKey);
            this.grpCranium.Location = new System.Drawing.Point(3, 8);
            this.grpCranium.Name = "grpCranium";
            this.grpCranium.Size = new System.Drawing.Size(625, 111);
            this.grpCranium.TabIndex = 22;
            this.grpCranium.TabStop = false;
            this.grpCranium.Text = "Cranium Credential";
            // 
            // txtSecret
            // 
            this.txtSecret.Location = new System.Drawing.Point(131, 51);
            this.txtSecret.Name = "txtSecret";
            this.txtSecret.Size = new System.Drawing.Size(478, 20);
            this.txtSecret.TabIndex = 2;
            // 
            // lblsecret
            // 
            this.lblsecret.AutoSize = true;
            this.lblsecret.Location = new System.Drawing.Point(4, 51);
            this.lblsecret.Name = "lblsecret";
            this.lblsecret.Size = new System.Drawing.Size(90, 13);
            this.lblsecret.TabIndex = 18;
            this.lblsecret.Text = "REST API Secret";
            // 
            // lblkey
            // 
            this.lblkey.AutoSize = true;
            this.lblkey.Location = new System.Drawing.Point(4, 25);
            this.lblkey.Name = "lblkey";
            this.lblkey.Size = new System.Drawing.Size(77, 13);
            this.lblkey.TabIndex = 19;
            this.lblkey.Text = "REST API Key";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(131, 25);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(478, 20);
            this.txtKey.TabIndex = 1;
            // 
            // UserCredentials
            // 
            this.UserCredentials.Controls.Add(this.btnSave);
            this.UserCredentials.Controls.Add(this.btnAddnewuser);
            this.UserCredentials.Controls.Add(this.dgvUserCredential);
            this.UserCredentials.Location = new System.Drawing.Point(4, 22);
            this.UserCredentials.Name = "UserCredentials";
            this.UserCredentials.Padding = new System.Windows.Forms.Padding(3);
            this.UserCredentials.Size = new System.Drawing.Size(636, 507);
            this.UserCredentials.TabIndex = 3;
            this.UserCredentials.Text = "User Credentials";
            this.UserCredentials.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(555, 476);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddnewuser
            // 
            this.btnAddnewuser.Location = new System.Drawing.Point(6, 478);
            this.btnAddnewuser.Name = "btnAddnewuser";
            this.btnAddnewuser.Size = new System.Drawing.Size(95, 23);
            this.btnAddnewuser.TabIndex = 10;
            this.btnAddnewuser.Text = "Add New User";
            this.btnAddnewuser.UseVisualStyleBackColor = true;
            this.btnAddnewuser.Click += new System.EventHandler(this.btnAddnewuser_Click);
            // 
            // dgvUserCredential
            // 
            this.dgvUserCredential.AllowUserToAddRows = false;
            this.dgvUserCredential.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUserCredential.Location = new System.Drawing.Point(0, 0);
            this.dgvUserCredential.Name = "dgvUserCredential";
            this.dgvUserCredential.ReadOnly = true;
            this.dgvUserCredential.Size = new System.Drawing.Size(633, 470);
            this.dgvUserCredential.TabIndex = 9;
            this.dgvUserCredential.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUserCredential_CellClick);
            // 
            // btnTestExchange
            // 
            this.btnTestExchange.Location = new System.Drawing.Point(131, 108);
            this.btnTestExchange.Name = "btnTestExchange";
            this.btnTestExchange.Size = new System.Drawing.Size(91, 23);
            this.btnTestExchange.TabIndex = 23;
            this.btnTestExchange.Text = "Test Exchange";
            this.btnTestExchange.UseVisualStyleBackColor = true;
            this.btnTestExchange.Click += new System.EventHandler(this.btnTestExchange_Click);
            // 
            // frmConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 533);
            this.Controls.Add(this.tabConfiguration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmConfiguration";
            this.Text = "Configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConfiguration_FormClosed);
            this.Load += new System.EventHandler(this.frmConfiguration_Load);
            this.tabConfiguration.ResumeLayout(false);
            this.General.ResumeLayout(false);
            this.grpExchange.ResumeLayout(false);
            this.grpExchange.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.grpCranium.ResumeLayout(false);
            this.grpCranium.PerformLayout();
            this.UserCredentials.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUserCredential)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabConfiguration;
        private System.Windows.Forms.TabPage General;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtSecret;
        private System.Windows.Forms.Label lblkey;
        private System.Windows.Forms.Label lblsecret;
        private System.Windows.Forms.TextBox txtAgentUsername;
        private System.Windows.Forms.TextBox txtAgentPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpExchange;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox grpCranium;
        private System.Windows.Forms.TabPage UserCredentials;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddnewuser;
        private System.Windows.Forms.DataGridView dgvUserCredential;
        private System.Windows.Forms.Button btnSaveGeneral;
        private System.Windows.Forms.Button btnTestRest;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnTestExchange;
    }
}

