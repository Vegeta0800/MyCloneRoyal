using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCloneRoyale
{
    public abstract class GameObject
    {
        public int energyCost;
        public int health;
        public int speed;
        public int viewRadius;
        public int shotRadius;
        public int shotInterval;
        public int shotDamage;
        public int bulletSpeed;

        public float X;
        public float Y;
        public float Xdir;
        public float Ydir;
        public float enemyBasePoint;

        public int playerNumber;
        public Game game;

        public bool move;
        public bool shootOnBase;
        public bool activated;

        public bool isShot;

        public int interval;

        // insert behavior / AI here:
        public abstract void NextSimulationStep();

        // render code here:
        public abstract void Render(Graphics g, float width);

    }
}
