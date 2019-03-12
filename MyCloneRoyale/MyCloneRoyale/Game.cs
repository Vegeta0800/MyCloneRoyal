using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCloneRoyale.Messages;
using System.Windows.Forms;


namespace MyCloneRoyale
{
    public class Game
    {
        public enum EntityTypes
        {
            One = 1,
            Two = 2,
            Three = 3,
        }

        public List<GameObject> allGameObjects = new List<GameObject>();

        public OpenConnection connectionToOtherPlayer;
        private int myPlayerNumber;
        private int otherPlayerNumber;
        private int localRound;
        private int globalRound;
        private bool gameRunning;
        private bool timeout;
        private bool sendMessages = false;

        private bool ready;
        private bool receivedReady;

        public bool victory;
        public int health;
        public int otherHealth;
        private int energy;
        private int otherEnergy;
        private int interval;
        public bool gamePaused;
        private float width;
        public int clickCD;
        public EntityTypes selectedType;
        public EntityTypes otherSelectedType;
        public Label energyLabel;
        public Button readyButton;
        public Button resetButton;
        public Button pauseButton;
        public Button disconnectButton;

        public struct UserInput
        {
            public int UnitTypePlaced; // 0 for none
            public int x;
            public int y;
        }

        private Dictionary<int, UserInput> inputsReceived = new Dictionary<int, UserInput>();
        private Dictionary<int, UserInput> ownInputs = new Dictionary<int, UserInput>();

        public GameObject player1Base;
        public GameObject player2Base;

        private UserInput inputForCurrentRound = new UserInput();

        public void Ready()
        {
            if (!this.gameRunning)
            {
                this.ready = !this.ready;

                if (this.ready && this.receivedReady)
                {
                    this.gameRunning = true;
                }

                this.connectionToOtherPlayer.Send(new ReadyMessage()
                {
                    ready = this.ready,
                    gameRunning = this.gameRunning
                });
            }
        }

        public bool isReady()
        {
            return this.ready;
        }

        public bool isGameRunning()
        {
            return this.gameRunning;
        }

        public int GetPlayerNumber()
        {
            return this.myPlayerNumber;
        }

        public void Start(int myPlayerNumber, OpenConnection connectionToOtherPlayer)
        {
            // init here

            if(myPlayerNumber == 0)
            {
                this.ready = true;
                this.receivedReady = false;
                this.readyButton.BackColor = Color.IndianRed;
            }
            else
            {
                this.ready = false;
                this.receivedReady = true;
                this.readyButton.BackColor = SystemColors.Control;
            }

            this.resetButton.Enabled = false;
            this.readyButton.Enabled = true;


            this.connectionToOtherPlayer = connectionToOtherPlayer;
            this.myPlayerNumber = myPlayerNumber;
            this.allGameObjects.Clear();
            this.inputsReceived.Clear();
            this.localRound = 0;
            this.globalRound = 0;
            this.gamePaused = false;
            this.gameRunning = false;
            this.selectedType = EntityTypes.One;
            this.sendMessages = false;

            this.timeout = true;
            this.health = 100;
            this.otherHealth = 100;
            this.energy = 10;
            this.otherEnergy = 10;
            this.interval = 0;
            this.clickCD = 10;

            this.player1Base = new Base()
            {
                X = 0,
                health = 100,
                game = this,
                playerNumber = 0
            };

            this.player2Base = new Base()
            {
                X = width,
                health = 100,
                game = this,
                playerNumber = 1
            };

        }

        public void Stop()
        {
            if (this.gameRunning)
            {
                this.connectionToOtherPlayer.Send(new TimeoutMessage()
                {
                    gameRunning = false
                });

                this.gameRunning = false;
            }
           
        }

        void NextSimulationStep()
        {
            int length = allGameObjects.Count;
            for (int i = 0; i < length; i++)
            {
                length = allGameObjects.Count;
                allGameObjects[i].NextSimulationStep();
            }
        }

        public void Render(Graphics g, float width)
        {
            this.width = width;
            if (!timeout)
            {
                g.FillRectangle(Brushes.DarkRed, new Rectangle(0, 0, 700, 300));
            }
            else
            {
                g.FillRectangle(Brushes.Azure, new Rectangle(0, 0, 700, 300));
                g.FillRectangle(Brushes.LightGreen, new Rectangle(350, 0, 5, 300));

                if (this.gameRunning)
                {
                    if (!this.gamePaused)
                    {
                        this.readyButton.BackColor = SystemColors.Control;
                        this.readyButton.Enabled = false;
                        this.disconnectButton.Enabled = true;
                        this.pauseButton.Enabled = true;
                        this.clickCD = 10;

                        UpdateLockStepping();
                    }

                    if (!this.connectionToOtherPlayer.recievedHeartbeat)
                    {
                        Stop();
                        this.gameRunning = false;
                        this.timeout = true;
                    }
                    this.connectionToOtherPlayer.recievedHeartbeat = false;
                }


                if (this.myPlayerNumber == 0)
                {
                    g.FillRectangle(Brushes.Green, new Rectangle(12, 5, this.health, 15));
                    g.FillRectangle(Brushes.Green, new Rectangle(588, 5, this.otherHealth, 15));
                }
                else
                {
                    g.FillRectangle(Brushes.Green, new Rectangle(12, 5, this.otherHealth, 15));
                    g.FillRectangle(Brushes.Green, new Rectangle(588, 5, this.health, 15));
                }


                foreach (var o in allGameObjects)
                {
                    o.Render(g, width);
                }

                if (!sendMessages)
                {

                    if (this.health <= 0 || this.otherHealth <= 0)
                    {
                        if (this.health <= 0)
                        {
                            this.connectionToOtherPlayer.Send(new EndMessage()
                            {
                                victory = true
                            });
                            this.victory = false;
                        }
                        else if (this.otherHealth <= 0)
                        {
                            this.connectionToOtherPlayer.Send(new EndMessage()
                            {
                                victory = false
                            });
                            this.victory = true;
                        }
                        this.sendMessages = true;
                        this.resetButton.Enabled = true;
                    }

                }
                else
                {
                    if(this.victory)
                    {
                        this.gameRunning = false;
                        g.FillRectangle(Brushes.LightGreen, new Rectangle(0, 0, 700, 300));
                    }
                    else
                    {
                        this.gameRunning = false;
                        g.FillRectangle(Brushes.Red, new Rectangle(0, 0, 700, 300));
                    }

                }
            }
        }


        private void UpdateLockStepping()
        {
                // energy update
                if (this.interval < 10)
                {
                    this.interval++;
                }
                else
                {
                    this.interval = 0;
                    if(energy < 10)
                        this.energy++;
                }

            this.energyLabel.Text = this.energy.ToString();

            // store my input for current round
            this.ownInputs.Add(this.localRound, this.inputForCurrentRound);

            // send my input to other player
            this.connectionToOtherPlayer.Send(new GameInputMessage()
            {
                GameRound = this.localRound,
                UnitTypePlaced = this.inputForCurrentRound.UnitTypePlaced,
                X = this.inputForCurrentRound.x,
                Y = this.inputForCurrentRound.y,
                playerNumber = this.myPlayerNumber,
                entityType = 1,
                energy = this.energy,
                health = this.health,
                gameRunning = true
            });

            // reset input for next round
            this.inputForCurrentRound.UnitTypePlaced = 0;

            // start next local round
            this.localRound++;

            // check if we have all the inputs for the next global round
            if (this.ownInputs.ContainsKey(this.globalRound) && this.inputsReceived.ContainsKey(this.globalRound))
            {
                // yes! execute commands and simulate next round
                var myCommand = this.ownInputs[this.globalRound];
                var otherPlayersCommand = this.inputsReceived[this.globalRound];

                switch (this.otherSelectedType)
                {
                    #region EntityOne
                    case EntityTypes.One:
                        if (otherPlayersCommand.UnitTypePlaced != 0 && this.otherEnergy >= 3)
                        {
                            this.allGameObjects.Add(new EnemyOne()
                            {
                                X = otherPlayersCommand.x,
                                Y = otherPlayersCommand.y,
                                energyCost = 3,
                                health = 5,
                                speed = 2,
                                interval = 0,
                                viewRadius = 100,
                                shotRadius = 50,
                                shotInterval = 2,
                                shotDamage = 2,
                                playerNumber = this.otherPlayerNumber,
                                game = this,
                                bulletSpeed = 5,
                                activated = true
                            });

                            this.otherEnergy -= 3;
                        }
                        break;
                    #endregion
                    #region EntityTwo
                    case EntityTypes.Two:
                        if (otherPlayersCommand.UnitTypePlaced != 0 && this.otherEnergy >= 5)
                        {
                            this.allGameObjects.Add(new EnemyTwo()
                            {
                                X = otherPlayersCommand.x,
                                Y = otherPlayersCommand.y,
                                energyCost = 5,
                                health = 1,
                                speed = 5,
                                viewRadius = 200,
                                shotRadius = 150,
                                shotInterval = 3,
                                interval = 0,
                                shotDamage = 3,
                                playerNumber = this.otherPlayerNumber,
                                game = this,
                                bulletSpeed = 15,
                                activated = true
                            });

                            this.otherEnergy -= 5;
                        }
                        break;
                    #endregion
                    #region EntityThree
                    case EntityTypes.Three:
                        if (otherPlayersCommand.UnitTypePlaced != 0 && this.otherEnergy >= 6)
                        {
                            this.allGameObjects.Add(new EnemyThree()
                            {
                                X = otherPlayersCommand.x,
                                Y = otherPlayersCommand.y,
                                energyCost = 6,
                                health = 12,
                                interval = 0,
                                speed = 2,
                                viewRadius = 70,
                                shotRadius = 50,
                                shotInterval = 2,
                                shotDamage = 2,
                                playerNumber = this.otherPlayerNumber,
                                game = this,
                                bulletSpeed = 3,
                                activated = true
                            });

                            this.otherEnergy -= 6;
                        }
                        break;
                        #endregion
                }

                switch(this.selectedType)
                {
                    #region EntityOne
                    case EntityTypes.One:
                        if (myCommand.UnitTypePlaced != 0 && this.energy >= 3)
                        {
                            this.allGameObjects.Add(new EnemyOne()
                            {
                                X = myCommand.x,
                                Y = myCommand.y,
                                energyCost = 3,
                                health = 5,
                                speed = 2,
                                interval = 0,
                                viewRadius = 100,
                                shotRadius = 50,
                                shotInterval = 2,
                                shotDamage = 2,
                                playerNumber = this.myPlayerNumber,
                                game = this,
                                bulletSpeed = 5,
                                activated = true
                            });

                            this.energy -= 3;
                        }
                        break;
                    #endregion
                    #region EntityTwo
                    case EntityTypes.Two:
                        if (myCommand.UnitTypePlaced != 0 && this.energy >= 5)
                        {
                            this.allGameObjects.Add(new EnemyTwo()
                            {
                                X = myCommand.x,
                                Y = myCommand.y,
                                energyCost = 5,
                                health = 1,
                                speed = 5,
                                viewRadius = 200,
                                shotRadius = 150,
                                shotInterval = 3,
                                interval = 0,
                                shotDamage = 3,
                                playerNumber = this.myPlayerNumber,
                                game = this,
                                bulletSpeed = 15,
                                activated = true
                            });

                            this.energy -= 5;

                        }


                        break;
                    #endregion
                    #region EntityThree
                    case EntityTypes.Three:
                        if (myCommand.UnitTypePlaced != 0 && this.energy >= 6)
                        {
                            this.allGameObjects.Add(new EnemyThree()
                            {
                                X = myCommand.x,
                                Y = myCommand.y,
                                energyCost = 6,
                                health = 12,
                                interval = 0,
                                speed = 2,
                                viewRadius = 70,
                                shotRadius = 50,
                                shotInterval = 2,
                                shotDamage = 2,
                                playerNumber = this.myPlayerNumber,
                                game = this,
                                bulletSpeed = 3,
                                activated = true
                            });

                            this.energy -= 6;
                        }
                        break;
                        #endregion
                }


                // delete old inputs to conserve memory (but we could also keep them as a replay!)
                this.ownInputs.Remove(this.globalRound);
                this.inputsReceived.Remove(this.globalRound);


                // next round!
                this.globalRound++;
            }
                NextSimulationStep();

        }

        public void OnClick(int x, int y)
        {
            if (this.gameRunning)
            {
                this.inputForCurrentRound.x = x;
                this.inputForCurrentRound.y = y;
                this.inputForCurrentRound.UnitTypePlaced = 1;
            }
        }

        public void OnNetworkInputMessage(GameInputMessage m)
        {
            if (this.gameRunning)
            {
                // received input from other player, store it
                this.inputsReceived.Add(m.GameRound, new UserInput()
                {
                    UnitTypePlaced = m.UnitTypePlaced,
                    x = m.X,
                    y = m.Y,
                });

                this.otherPlayerNumber = m.playerNumber;
                this.otherEnergy = m.energy;
                this.otherHealth = m.health;
            }
        }

        public void OnNetworkReadyMessage(ReadyMessage m)
        {
            this.receivedReady = m.ready;
            this.gameRunning = m.gameRunning;
        }

        public void OnNetworkDamageMessage(DamageMessage m)
        {
            if(this.myPlayerNumber == m.targetPlayer)
                this.health = m.health;
        }

        public void OnNetworkTimeoutMessage(TimeoutMessage m)
        {
            this.timeout = m.gameRunning;
        }

        public void OnNetworkEndMessage(EndMessage m)
        {
            if (this.gameRunning)
            {
                this.victory = m.victory;
            }
        }

        public void OnNetworkResetMessage(ResetMessage m)
        {
            if (!this.gameRunning)
            {
                Start(this.myPlayerNumber, this.connectionToOtherPlayer);
            }
        }

        public void OnNetworkPausedMessage(PauseMessage m)
        {
            if (this.gameRunning)
            {
                this.gamePaused = m.pause;
            }
        }

        public void OnNetworkTypeMessage(SelectTypeMessage m)
        {
                switch (m.selectedType)
                {
                    case 1:
                        this.otherSelectedType = EntityTypes.One;
                        break;
                    case 2:
                        this.otherSelectedType = EntityTypes.Two;
                        break;
                    case 3:
                        this.otherSelectedType = EntityTypes.Three;
                        break;
                    default:
                        this.otherSelectedType = EntityTypes.One;
                        break;
                }
        }
    }
}