namespace MyCloneRoyale
{
    partial class ApplicationWindow
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
            this.buttonOpenServer = new System.Windows.Forms.Button();
            this.buttonJoinServer = new System.Windows.Forms.Button();
            this.textBoxServerIp = new System.Windows.Forms.TextBox();
            this.textBoxChatInput = new System.Windows.Forms.TextBox();
            this.textBoxChat = new System.Windows.Forms.TextBox();
            this.Ready = new System.Windows.Forms.Button();
            this.QLabel = new System.Windows.Forms.Label();
            this.WLabel = new System.Windows.Forms.Label();
            this.ELabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SelectedTroopLabel = new System.Windows.Forms.Label();
            this.energyLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ResetGame = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOpenServer
            // 
            this.buttonOpenServer.Location = new System.Drawing.Point(12, 12);
            this.buttonOpenServer.Name = "buttonOpenServer";
            this.buttonOpenServer.Size = new System.Drawing.Size(114, 23);
            this.buttonOpenServer.TabIndex = 0;
            this.buttonOpenServer.Text = "Open Server";
            this.buttonOpenServer.UseVisualStyleBackColor = true;
            this.buttonOpenServer.Click += new System.EventHandler(this.buttonOpenServer_Click);
            // 
            // buttonJoinServer
            // 
            this.buttonJoinServer.Location = new System.Drawing.Point(12, 41);
            this.buttonJoinServer.Name = "buttonJoinServer";
            this.buttonJoinServer.Size = new System.Drawing.Size(114, 23);
            this.buttonJoinServer.TabIndex = 1;
            this.buttonJoinServer.Text = "Join Server";
            this.buttonJoinServer.UseVisualStyleBackColor = true;
            this.buttonJoinServer.Click += new System.EventHandler(this.buttonJoinServer_Click);
            // 
            // textBoxServerIp
            // 
            this.textBoxServerIp.Location = new System.Drawing.Point(132, 43);
            this.textBoxServerIp.Name = "textBoxServerIp";
            this.textBoxServerIp.ReadOnly = true;
            this.textBoxServerIp.Size = new System.Drawing.Size(160, 20);
            this.textBoxServerIp.TabIndex = 2;
            this.textBoxServerIp.Text = "127.0.0.1";
            // 
            // textBoxChatInput
            // 
            this.textBoxChatInput.Location = new System.Drawing.Point(12, 580);
            this.textBoxChatInput.Name = "textBoxChatInput";
            this.textBoxChatInput.Size = new System.Drawing.Size(783, 20);
            this.textBoxChatInput.TabIndex = 3;
            this.textBoxChatInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxChatInput_KeyDown);
            // 
            // textBoxChat
            // 
            this.textBoxChat.Location = new System.Drawing.Point(12, 424);
            this.textBoxChat.Multiline = true;
            this.textBoxChat.Name = "textBoxChat";
            this.textBoxChat.ReadOnly = true;
            this.textBoxChat.Size = new System.Drawing.Size(783, 150);
            this.textBoxChat.TabIndex = 4;
            // 
            // Ready
            // 
            this.Ready.Location = new System.Drawing.Point(635, 43);
            this.Ready.Name = "Ready";
            this.Ready.Size = new System.Drawing.Size(114, 23);
            this.Ready.TabIndex = 5;
            this.Ready.Text = "Ready";
            this.Ready.UseVisualStyleBackColor = true;
            this.Ready.Click += new System.EventHandler(this.Ready_Click);
            // 
            // QLabel
            // 
            this.QLabel.AutoSize = true;
            this.QLabel.Font = new System.Drawing.Font("Papyrus", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.QLabel.Location = new System.Drawing.Point(344, 31);
            this.QLabel.Name = "QLabel";
            this.QLabel.Size = new System.Drawing.Size(48, 42);
            this.QLabel.TabIndex = 6;
            this.QLabel.Text = "Q";
            // 
            // WLabel
            // 
            this.WLabel.AutoSize = true;
            this.WLabel.Font = new System.Drawing.Font("Papyrus", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WLabel.Location = new System.Drawing.Point(398, 31);
            this.WLabel.Name = "WLabel";
            this.WLabel.Size = new System.Drawing.Size(47, 42);
            this.WLabel.TabIndex = 7;
            this.WLabel.Text = "W";
            // 
            // ELabel
            // 
            this.ELabel.AutoSize = true;
            this.ELabel.Font = new System.Drawing.Font("Papyrus", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ELabel.Location = new System.Drawing.Point(451, 31);
            this.ELabel.Name = "ELabel";
            this.ELabel.Size = new System.Drawing.Size(45, 42);
            this.ELabel.TabIndex = 8;
            this.ELabel.Text = "E";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Papyrus", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(595, 396);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 25);
            this.label4.TabIndex = 9;
            this.label4.Text = "Selected Troop:";
            // 
            // SelectedTroopLabel
            // 
            this.SelectedTroopLabel.AutoSize = true;
            this.SelectedTroopLabel.Font = new System.Drawing.Font("Papyrus", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectedTroopLabel.Location = new System.Drawing.Point(732, 396);
            this.SelectedTroopLabel.Name = "SelectedTroopLabel";
            this.SelectedTroopLabel.Size = new System.Drawing.Size(63, 25);
            this.SelectedTroopLabel.TabIndex = 10;
            this.SelectedTroopLabel.Text = "Normal";
            // 
            // energyLabel
            // 
            this.energyLabel.AutoSize = true;
            this.energyLabel.Font = new System.Drawing.Font("Papyrus", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.energyLabel.Location = new System.Drawing.Point(86, 396);
            this.energyLabel.Name = "energyLabel";
            this.energyLabel.Size = new System.Drawing.Size(32, 25);
            this.energyLabel.TabIndex = 12;
            this.energyLabel.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Papyrus", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 396);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 25);
            this.label2.TabIndex = 11;
            this.label2.Text = "Energy:";
            // 
            // ResetGame
            // 
            this.ResetGame.Location = new System.Drawing.Point(635, 12);
            this.ResetGame.Name = "ResetGame";
            this.ResetGame.Size = new System.Drawing.Size(114, 23);
            this.ResetGame.TabIndex = 13;
            this.ResetGame.Text = "Reset Game";
            this.ResetGame.UseVisualStyleBackColor = true;
            this.ResetGame.Click += new System.EventHandler(this.ResetGame_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Location = new System.Drawing.Point(515, 43);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(114, 23);
            this.PauseButton.TabIndex = 14;
            this.PauseButton.Text = "Pause Game";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(132, 12);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(114, 23);
            this.disconnectButton.TabIndex = 15;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // ApplicationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 609);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.ResetGame);
            this.Controls.Add(this.energyLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SelectedTroopLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ELabel);
            this.Controls.Add(this.WLabel);
            this.Controls.Add(this.QLabel);
            this.Controls.Add(this.Ready);
            this.Controls.Add(this.textBoxChat);
            this.Controls.Add(this.textBoxChatInput);
            this.Controls.Add(this.textBoxServerIp);
            this.Controls.Add(this.buttonJoinServer);
            this.Controls.Add(this.buttonOpenServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ApplicationWindow";
            this.Text = "CloneRoyale";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicationWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ApplicationWindow_FormClosed);
            this.Load += new System.EventHandler(this.ApplicationWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenServer;
        private System.Windows.Forms.Button buttonJoinServer;
        private System.Windows.Forms.TextBox textBoxServerIp;
        private System.Windows.Forms.TextBox textBoxChatInput;
        private System.Windows.Forms.TextBox textBoxChat;
        private System.Windows.Forms.Button Ready;
        private System.Windows.Forms.Label QLabel;
        private System.Windows.Forms.Label WLabel;
        private System.Windows.Forms.Label ELabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label SelectedTroopLabel;
        private System.Windows.Forms.Label energyLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ResetGame;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Button disconnectButton;
    }
}

