using System;
using System.Drawing;
using MyCloneRoyale.Messages;

namespace MyCloneRoyale
{
    public class Shot : GameObject
    {
        public GameObject target;

        // insert behavior / AI here:
        public override void NextSimulationStep()
        {
            if (this.activated && target != null)
            {
                #region Collision

                if (this.shootOnBase)
                {
                    float distanceToBase = (float)(Math.Sqrt(((this.target.X - this.X) * (this.target.X - this.X))));
                    if (distanceToBase <= 4)
                    {
                        target.health -= this.shotDamage;
                        this.game.connectionToOtherPlayer.Send(new DamageMessage()
                        {
                            health = target.health,
                            targetPlayer = target.playerNumber
                        });
                        this.activated = false;
                    }
                }
                else
                {
                    float distanceToTarget = (float)(Math.Sqrt(((this.target.X - this.X) * (this.target.X - this.X)) + ((this.target.Y - this.Y) * (this.target.Y - this.Y))));

                    if (distanceToTarget <= 15)
                    {
                        target.health -= this.shotDamage;
                        this.activated = false;
                    }
                }


                #endregion


                #region Movement
                if (this.shootOnBase)
                {
                    if (this.playerNumber == 0)
                        this.Xdir = 1;
                    else
                        this.Xdir = -1;
                    this.X += this.Xdir * this.speed;
                }
                else
                {
                    this.Xdir = target.X - this.X;
                    this.Ydir = target.Y - this.Y;

                    float length = (float)(Math.Sqrt((this.Xdir * this.Xdir) + (this.Ydir * this.Ydir)));

                    if (length > float.MinValue)
                    {
                        float invLength = 1.0f / length;

                        this.Xdir *= invLength;
                        this.Ydir *= invLength;
                    }

                    this.X += this.Xdir * this.speed;
                    this.Y += this.Ydir * this.speed;
                }
                #endregion
            }
        }

        // render code here:
        public override void Render(Graphics g, float width)
        {
            if (this.activated)
            {
                const float radius = 5;

                Brush color;
                if (this.playerNumber == 0)
                {
                    color = Brushes.DarkBlue;
                }
                else
                {
                    color = Brushes.DarkRed;
                }

                g.FillEllipse(color, this.X - radius, this.Y - radius, 2 * radius, 2 * radius);
            }
        }

    }
}
