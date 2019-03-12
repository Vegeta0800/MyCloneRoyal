using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyCloneRoyale
{
    public partial class Canvas : UserControl
    {
        private Game game;

        public Canvas(Game game)
        {
            InitializeComponent();

            this.DoubleBuffered = true;

            this.game = game;
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            game.Render(e.Graphics, this.Width);
        }

        private void Canvas_MouseClick(object sender, MouseEventArgs e)
        {
                if (game.GetPlayerNumber() == 0)
                {
                    if (e.X < this.Width / 2)
                    {
                        game.OnClick(e.X, e.Y);
                    }
                }
                else
                {
                    if (e.X > this.Width / 2)
                    {
                        game.OnClick(e.X, e.Y);
                    }
                }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Canvas_Load(object sender, EventArgs e)
        {

        }
    }
}
