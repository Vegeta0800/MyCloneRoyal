using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MyCloneRoyale.Messages;
using System.Runtime.InteropServices;
using Message = MyCloneRoyale.Messages.Message;

namespace MyCloneRoyale
{

    public partial class ApplicationWindow : Form
    {
        private Thread serverThread;
        private volatile bool keepServerThreadRunning;
        const int SERVERPORT = 4000;

        enum State
        {
            NotConnected,
            Server,
            Client
        }



        private State state;
        private Graphics g;
        private OpenConnection openConnection;
        private Game game = new Game();

        public ApplicationWindow()
        {
            InitializeComponent();

            var canvas = new Canvas(game);

            canvas.Size = new Size(700, 300);
            canvas.Location = new Point(10, 85);
            this.Controls.Add(canvas);
        }

        private void buttonOpenServer_Click(object sender, EventArgs e)
        {
            this.buttonJoinServer.Enabled = false;
            this.buttonOpenServer.Enabled = false;

            StartServerThread();

            AddChatLine("Server", "Server started!");
        }

        private void StartServerThread()
        {
            this.keepServerThreadRunning = true;

            this.serverThread = new Thread(this.ServerThreadProc);
            this.serverThread.Start();
        }

        private void StopServerThread()
        {
            // this.serverThread.Abort();       // not nice!

            if (this.serverThread != null)
            {
                this.keepServerThreadRunning = false;
                this.serverThread.Join();
            }
        }

        private void ServerThreadProc()
        {
            // this is my server thread!
            var tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), SERVERPORT));
            tcpListener.Start();

            while (this.keepServerThreadRunning)
            {
                if (this.state == State.NotConnected)
                {
                    if (tcpListener.Pending())
                    {
                        var client = tcpListener.AcceptTcpClient();

                        this.openConnection = new OpenConnection(client);
                        this.openConnection.MessageComplete += OpenConnectionOnMessageComplete;
                        this.state = State.Server;

                        this.game.Start(0, this.openConnection);

                        //AddChatLine(this.state.ToString(), "is ready!");

                        //if (this.openConnection != null)
                        //{
                        //    this.openConnection.Send(new ChatMessage()
                        //    {
                        //        Name = this.state.ToString(),
                        //        Text = "is ready!"
                        //    });
                        //}
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void buttonJoinServer_Click(object sender, EventArgs e)
        {
            if (this.state != State.NotConnected)
                return;

            try
            {
                var client = new TcpClient();
                client.Connect(this.textBoxServerIp.Text, SERVERPORT);

                this.openConnection = new OpenConnection(client);
                this.openConnection.MessageComplete += OpenConnectionOnMessageComplete;
                this.state = State.Client;

                this.buttonJoinServer.Enabled = false;
                this.buttonOpenServer.Enabled = false;

                this.game.Start(1, this.openConnection);

                //AddChatLine(this.state.ToString(), "Connected");

                //if (this.openConnection != null)
                //{
                //    this.openConnection.Send(new ChatMessage()
                //    {
                //        Name = this.state.ToString(),
                //        Text = "Connected"
                //    });
                //}
            }
            catch (SocketException exception)
            {
                MessageBox.Show("Failed to connect.");
            }
        }

        private void OpenConnectionOnMessageComplete(Message message)
        {
            // callback is coming from connection thread; needs to be pushed into UI thread
            this.BeginInvoke(new Action(() =>
            {
                if (message is ChatMessage)
                {
                    var m = (ChatMessage)message;
                    AddChatLine(m.Name, m.Text);
                }
                else if (message is SelectTypeMessage)
                {
                    this.game.OnNetworkTypeMessage((SelectTypeMessage)message);
                }
                else if (message is GameInputMessage)
                {
                    this.game.OnNetworkInputMessage((GameInputMessage)message);
                }
                else if (message is ReadyMessage)
                {
                    this.game.OnNetworkReadyMessage((ReadyMessage)message);
                }
                else if (message is DamageMessage)
                {
                    this.game.OnNetworkDamageMessage((DamageMessage)message);
                }
                else if (message is TimeoutMessage)
                {
                    this.game.OnNetworkTimeoutMessage((TimeoutMessage)message);

                    AddChatLine(this.state.ToString(), "has timeouted!");

                    if (this.openConnection != null)
                    {
                        this.openConnection.Send(new ChatMessage()
                        {
                            Name = this.state.ToString(),
                            Text = "has timeouted!"
                        });
                    }
                }
                else if (message is EndMessage)
                {
                    this.game.OnNetworkEndMessage((EndMessage)message);

                    if (this.game.victory)
                        AddChatLine("Server", " you have won!");
                    else
                        AddChatLine("Server", "you have lost!");
                }
                else if (message is ResetMessage)
                {
                    this.game.OnNetworkResetMessage((ResetMessage)message);

                    AddChatLine("Server", "reseted the game!");

                    if (this.openConnection != null)
                    {
                        this.openConnection.Send(new ChatMessage()
                        {
                            Name = "Server",
                            Text = "reseted the game!"
                        });
                    }
                }
                else if (message is PauseMessage)
                {
                    this.game.OnNetworkPausedMessage((PauseMessage)message);

                    AddChatLine("Server", "paused the game!");

                    if (this.openConnection != null)
                    {
                        this.openConnection.Send(new ChatMessage()
                        {
                            Name = "Server",
                            Text = "paused the game!"
                        });
                    }
                }
            }));
        }

        private void ApplicationWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.game.Stop();

            StopServerThread();

            if (this.openConnection != null)
            {
                this.openConnection.Dispose();
            }
        }

        private void ApplicationWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void AddChatLine(string sSender, string sText)
        {
            this.textBoxChat.Text = this.textBoxChat.Text + string.Format("{0}: {1}\r\n", sSender, sText);
        }

        private void textBoxChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if(this.textBoxChatInput.Text == "")
                    return;

                AddChatLine(this.state.ToString(), this.textBoxChatInput.Text);

                if (this.openConnection != null)
                {
                    this.openConnection.Send(new ChatMessage()
                    {
                        Name = this.state.ToString(),
                        Text = this.textBoxChatInput.Text
                    });
                }
            }
        }

        private void Ready_Click(object sender, EventArgs e)
        {
            if (this.openConnection != null)
            {
                this.game.Ready();

                bool readyState = this.game.isReady();
                bool runningState = this.game.isGameRunning();
                string readyText;

                if (readyState)
                {
                    readyText = "is ready";
                    this.Ready.BackColor = Color.IndianRed;
                }
                else
                {
                    readyText = "is not ready";
                    this.Ready.BackColor = SystemColors.Control;
                }

                AddChatLine(this.state.ToString(), readyText);

                this.openConnection.Send(new ChatMessage()
                    {
                        Name = this.state.ToString(),
                        Text = readyText
                    });


                if (runningState)
                {
                    AddChatLine("Server", "Both players are ready! Game is starting now!");

                    this.openConnection.Send(new ChatMessage()
                    {
                        Name = "Server",
                        Text = "Both players are ready! Game is starting now!"
                    });

                    this.PauseButton.Enabled = true;
                    this.disconnectButton.Enabled = true;
                }

            }
            else
            {
                AddChatLine(this.state.ToString(), "NOT CONNECTED");
            }
        }

        private void ApplicationWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.KeyCode)
            {
                case Keys.Q:
                    this.QLabel.ForeColor = Color.IndianRed;
                    this.WLabel.ForeColor = Color.Black;
                    this.ELabel.ForeColor = Color.Black;
                    this.SelectedTroopLabel.Text = "Normal";
                    this.game.selectedType = Game.EntityTypes.One;
                    this.openConnection.Send(new SelectTypeMessage()
                    {
                        selectedType = 1
                    });
                    break;
                case Keys.W:
                    this.QLabel.ForeColor = Color.Black;
                    this.WLabel.ForeColor = Color.IndianRed;
                    this.ELabel.ForeColor = Color.Black;
                    this.SelectedTroopLabel.Text = "Sniper";
                    this.game.selectedType = Game.EntityTypes.Two;
                    this.openConnection.Send(new SelectTypeMessage()
                    {
                        selectedType = 2
                    });
                    break;
                case Keys.E:
                    this.QLabel.ForeColor = Color.Black;
                    this.WLabel.ForeColor = Color.Black;
                    this.ELabel.ForeColor = Color.IndianRed;
                    this.SelectedTroopLabel.Text = "Tank";
                    this.game.selectedType = Game.EntityTypes.Three;
                    this.openConnection.Send(new SelectTypeMessage()
                    {
                        selectedType = 3
                    });
                    break;
            }

            this.Refresh();
        }

        private void ApplicationWindow_Load(object sender, EventArgs e)
        {
            g = this.CreateGraphics();

            this.BringToFront();
            this.Focus();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(ApplicationWindow_KeyDown);
            this.QLabel.ForeColor = Color.IndianRed;
            this.WLabel.ForeColor = Color.Black;
            this.ELabel.ForeColor = Color.Black;
            this.SelectedTroopLabel.Text = "Normal";
            this.game.selectedType = Game.EntityTypes.One;
            this.game.otherSelectedType = Game.EntityTypes.One;
            this.game.energyLabel = this.energyLabel;
            this.game.readyButton = this.Ready;
            this.game.resetButton = this.ResetGame;
            this.game.disconnectButton = this.disconnectButton;
            this.game.pauseButton = this.PauseButton;
            this.ResetGame.Enabled = false;
            this.PauseButton.Enabled = false;
            this.Ready.BackColor = SystemColors.Control;
            this.disconnectButton.Enabled = false;
        }

        private void ResetGame_Click(object sender, EventArgs e)
        {
            this.game.Start(this.game.GetPlayerNumber(), this.openConnection);

            this.openConnection.Send(new ResetMessage()
            {
                reset = true
            });
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            this.game.Stop();

            if (this.game.isGameRunning())
            {
                StopServerThread();

                if (this.openConnection != null)
                {
                    this.openConnection.Dispose();
                }
            }

            this.disconnectButton.Enabled = false;
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if(this.game.isGameRunning())
            {
                this.openConnection.Send(new PauseMessage()
                {
                    pause = !this.game.gamePaused
            });

                this.game.gamePaused = !this.game.gamePaused;
            }
        }
    }
}   