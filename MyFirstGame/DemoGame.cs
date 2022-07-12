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
        private Shape2D player;
        private Shape2D myCoursor;
        private Vector2 acceleration = new Vector2(0,0);
        private bool left;
        private bool right;
        private bool up;
        private bool down;
        private float moveUnit = 2f;
        private float accelerationUnit = 0.2f;
        private Vector2 speed = new Vector2(0,0);
        private Vector2 centerPlayerPos = new Vector2 { X = 0, Y = 0 };
        private List<Shape2D> holesList =new List<Shape2D>();
        Random r = new Random();

        private Shape2D CreateHole()
        {
            bool check = false;
            Shape2D hole = null;
            while (!check)
            {
                int index = holesList.Count;
                int rX = r.Next(-5000, 5000);
                int rY = r.Next(-5000, 5000);
                
                if (GetDisstanceBetween(new Vector2(rX,rY),player.Position)>=250)
                {
                    if (holesList.Count() > 0)
                    {
                        foreach (Shape2D hole2 in holesList)
                        {
                            Vector2 holeCenter = new Vector2(hole2.Position.X + (hole2.Scale.X / 2), hole2.Position.Y + (hole2.Scale.Y / 2));

                            if (GetDisstanceBetween(holeCenter, new Vector2(rX + 100, rY + 100)) > 200)
                            {
                                hole = new Shape2D(new Vector2(rX, rY), new Vector2(100, 100), $"{index} hole");
                                check = true;
                            }
                        }
                        holesList.Add(hole);
                    }
                    else 
                    {
                        hole = new Shape2D(new Vector2(rX, rY), new Vector2(100, 100), $"{index} hole");
                        holesList.Add(hole);
                        check = true;
                    }
                
                }
                    

            }
            return hole;
        }


        public void HoleCreator(int amount)
        {
            Logger.Warning("GAME LOADING...");
            for (int i = 0; i < amount; i++)
            {
                CreateHole();
            }
            Logger.Info("GAME LOADED!");
        }

        public float GetDisstanceBetween(Vector2 vector1, Vector2 vector2)
        {

            return (float)Math.Sqrt(Math.Pow((vector1.X-vector2.X),2)+ Math.Pow((vector1.Y - vector2.Y), 2));
        }


        public bool HitHole()
        {
            foreach (Shape2D hole in holesList)
            {
                Vector2 holeCenter = new Vector2(hole.Position.X+(hole.Scale.X/2),hole.Position.Y+(hole.Scale.Y/2));
                if (GetDisstanceBetween(holeCenter, centerPlayerPos) <= (player.Scale.X+hole.Scale.X)/2)
                { 
                    return true;
                }
            }
            return false;
        }



        public override void OnDraw()
        {
          
        }

        public override void OnLoad()
        {
            //Creation of player
            player = new Shape2D(new Vector2(0,0), new Vector2(20,50), "Player");
            myCoursor = new Shape2D(new Vector2(0, 0), new Vector2(5, 5), "myCursor");
           //
            HoleCreator(100);
        }

        
        public override void OnUpdate()
        {
            if (player != null)
            {
                myCoursor.Position.X = (Cursor.Position.X+player.Position.X)-GetWindowX()/2;
                myCoursor.Position.Y = ((Cursor.Position.Y+player.Position.Y)-GetWindowY()/2)-30;


                //constans acceleration  "might not working if you cross the border!"
                player.Position.Y += speed.Y;
                player.Position.X += speed.X;
                centerPlayerPos.X = player.Position.X + (player.Scale.X / 2);
                centerPlayerPos.Y = player.Position.Y + (player.Scale.Y / 2);
                speed.X = moveUnit * acceleration.X;
                speed.Y = moveUnit * acceleration.Y;
                if (acceleration.X > 0)
                {
                    acceleration.X -= 0.1f;
                }
                if (acceleration.Y > 0)
                {
                    acceleration.Y -= 0.1f;
                }
                if (acceleration.X < 0)
                {
                    acceleration.X += 0.1f;
                }
                if (acceleration.Y < 0)
                {
                    acceleration.Y += 0.1f;
                }
                //

                //movement
                if (up)
                {
                    acceleration.Y -= accelerationUnit;
                    
                }
                if (down)
                {
                    acceleration.Y += accelerationUnit;
                }
                if (right)
                {
                    acceleration.X += accelerationUnit;
                }
                if (left)
                {
                    acceleration.X -= accelerationUnit;
                }
                //


                if (HitHole())
                { 
                    player.DestroyObj();
                    player = null;
                }

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
