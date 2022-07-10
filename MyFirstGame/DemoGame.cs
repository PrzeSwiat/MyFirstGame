using MyFirstGame.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFirstGame
{
    class DemoGame: Components.MainEngine
    {
        public DemoGame() : base(new Components.Vector2(512, 512), "Novum") { }
        Shape2D player;
        private float accelerationX = 0;
        private float accelerationY = 0;
        private bool left;
        private bool right;
        private bool up;
        private bool down;
        private float moveUnit = 2f;
        private float accelerationUnit = 0.2f;
        private float speedX = 0;
        private float speedY = 0;

        public override void OnDraw()
        {
          
        }

        public override void OnLoad()
        {
            player = new Shape2D(new Vector2(10,10), new Vector2(10,10), "test");

        }

        
        public override void OnUpdate()
        {
            if (player != null)
            {
                //constans acceleration  "might not working if you cross the border!"
                player.Position.Y += speedY;
                player.Position.X += speedX;
                speedX = moveUnit * accelerationX;
                speedY = moveUnit * accelerationY;
                if (accelerationX > 0)
                {
                    accelerationX -= 0.1f;
                }
                if (accelerationY > 0)
                {
                    accelerationY -= 0.1f;
                }
                if (accelerationX < 0)
                {
                    accelerationX += 0.1f;
                }
                if (accelerationY < 0)
                {
                    accelerationY += 0.1f;
                }
                //

                //movement
                if (up)
                {
                    accelerationY -= accelerationUnit;
                    
                }
                if (down)
                {
                    accelerationY += accelerationUnit;
                }
                if (right)
                {
                    accelerationX += accelerationUnit;
                }
                if (left)
                {
                    accelerationX -= accelerationUnit;
                }
                //



            }
           


        }

        public override void GetKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) 
            {
                up = true;
            }
            if (e.KeyCode == Keys.S)
            {
                down = true;
            }
            if (e.KeyCode == Keys.D)
            {
                right = true;            
            }
            if (e.KeyCode == Keys.A)
            {
                left = true;
            }
        }

        public override void GetKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                up = false;
            }
            if (e.KeyCode == Keys.S)
            {
                down = false;
            }
            if (e.KeyCode == Keys.D)
            {
                right = false;
            }
            if (e.KeyCode == Keys.A)
            {
                left = false;
            }
        }
    }
}
