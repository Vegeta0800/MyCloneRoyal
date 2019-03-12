using System;
using System.Drawing;

namespace MyCloneRoyale
{
    public class EnemyTwo : GameObject
    {
        private GameObject focusedObject;

        // insert behavior / AI here:
        public override void NextSimulationStep()
        {
            if (this.activated)
            {
                if (this.health <= 0)
                    this.activated = false;

                focusedObject = null;
                this.shootOnBase = false;
                this.move = true;
                float distanceToEnemyBase = (float)(Math.Sqrt(((this.enemyBasePoint - this.X) * (this.enemyBasePoint - this.X)) + ((this.Y - this.Y) * (this.Y - this.Y))));

                if (distanceToEnemyBase <= this.shotRadius)
                {
                    this.move = false;
                    this.shootOnBase = true;
                }

                for (int i = 0; i < this.game.allGameObjects.Count; i++)
                {
                    if (this.game.allGameObjects[i].playerNumber != this.playerNumber)
                    {
                        if (!this.game.allGameObjects[i].isShot && this.game.allGameObjects[i].activated)
                        {

                            float distance = (float)(Math.Sqrt(((this.game.allGameObjects[i].X - this.X) * (this.game.allGameObjects[i].X - this.X)) + ((this.game.allGameObjects[i].Y - this.Y) * (this.game.allGameObjects[i].Y - this.Y))));

                            if (distance <= this.viewRadius)
                            {
                                if (!this.shootOnBase)
                                    focusedObject = this.game.allGameObjects[i];

                                if (distance <= this.shotRadius)
                                {
                                    this.move = false;
                                    this.shootOnBase = false;
                                }
                            }
                        }
                    }
                }


                if (this.move)
                {
                    if (focusedObject != null)
                    {
                        this.Xdir = focusedObject.X - this.X;
                        this.Ydir = focusedObject.Y - this.Y;

                        float length = (float)(Math.Sqrt((this.Xdir * this.Xdir) + (this.Ydir * this.Ydir)));

                        if (length > float.MinValue)
                        {
                            float invLength = 1.0f / length;

                            this.Xdir *= invLength;
                            this.Ydir *= invLength;
                        }
                    }
                    else
                    {
                        if (this.playerNumber == 0)
                        {
                            this.Xdir = 1;
                            this.Ydir = 0;
                        }
                        else
                        {
                            this.Xdir = -1;
                            this.Ydir = 0;
                        }
                    }

                    this.X += this.Xdir * (1 * this.speed);
                    this.Y += this.Ydir * (1 * this.speed);
                }
                else
                {
                    if (this.interval < (this.shotInterval * 10))
                        this.interval++;
                    else
                    {
                        this.interval = 0;

                        if (this.shootOnBase)
                        {
                            if (this.playerNumber == 0)
                                Shoot(this.game.player2Base);
                            else
                                Shoot(this.game.player1Base);
                        }
                        else
                            Shoot(this.focusedObject);
                    }
                }

            }
        }

        // render code here:
        public override void Render(Graphics g, float width)
        {
            if (this.activated)
            {
                const float radius = 12;
                Brush color;
                Pen pen;
                if (this.playerNumber == 0)
                {
                    this.enemyBasePoint = width;
                    color = Brushes.LightSkyBlue;
                    pen = Pens.Aqua;
                }
                else
                {
                    this.enemyBasePoint = 0;
                    color = Brushes.OrangeRed;
                    pen = Pens.PaleVioletRed;
                }

                g.FillRectangle(Brushes.Green, new Rectangle((int)(this.X - radius), (int)(this.Y - radius - 10), (int)(this.health * 5), 10));
                g.FillEllipse(color, this.X - radius, this.Y - radius, 2 * radius, 2 * radius);
                g.DrawEllipse(pen, this.X - viewRadius, this.Y - viewRadius, 2 * viewRadius, 2 * viewRadius);
                g.DrawEllipse(pen, this.X - this.shotRadius, this.Y - this.shotRadius, 2 * shotRadius, 2 * shotRadius);
            }
        }

        private void Shoot(GameObject target)
        {
            this.game.allGameObjects.Add(new Shot()
            {
                X = this.X,
                Y = this.Y,
                speed = this.bulletSpeed,
                target = target,
                shootOnBase = this.shootOnBase,
                playerNumber = this.playerNumber,
                shotDamage = this.shotDamage,
                activated = true,
                game = this.game,
                isShot = true
            });
        }
    }
}
