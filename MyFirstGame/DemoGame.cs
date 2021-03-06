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
        private List<Vector2> havens = new List<Vector2>();
        private List<Shape2D> holesList =new List<Shape2D>();
        private List<Strite2D> jetsList = new List<Strite2D>();
        Random r = new Random();



        private Strite2D CreateJet()
        {
            bool check = false;
            Strite2D jet = null;
            while (!check)
            {
                int index = jetsList.Count;
                int rX = r.Next(-100, 100);
                int rY = r.Next(-100, 100);

                if (GetDisstanceBetween(new Vector2(rX, rY), player.Position) >= 50)
                {
                    if (jetsList.Count() > 0)
                    {
                        foreach (Strite2D jet2 in jetsList)
                        {

                            if (GetDisstanceBetween(new Vector2(jet2.Position.X,jet2.Position.Y), new Vector2(rX, rY)) > 10)
                            {
                                jet = new Strite2D(new Vector2(rX, rY), new Vector2(20, 10), "/Hookers/Jet", $"{index} jet");
                                check = true;
                            }
                        }
                        jetsList.Add(jet);
                    }
                    else
                    {
                        jet = new Strite2D(new Vector2(rX, rY), new Vector2(20, 10), "/Hookers/Jet", $"{index} jet");
                        jetsList.Add(jet);
                        check = true;
                    }

                }


            }
            return jet;
            
        }

        

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
            Logger.Warning("HOLES LOADING...");
            for (int i = 0; i < amount; i++)
            {
                CreateHole();
            }
            Logger.Info("HOLES LOADED!");
        }
        public void JetCreator(int amount)
        {
            Logger.Warning("JETS LOADING...");
            for (int i = 0; i < amount; i++)
            {
                CreateJet();
            }
            Logger.Info("JETS LOADED!");
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
            player = new Shape2D(new Vector2(0,0), new Vector2(20,40), "Player");
            myCoursor = new Shape2D(new Vector2(0, 0), new Vector2(5, 5), "myCursor");

           //
            HoleCreator(100);
            JetCreator(3);

            int hookCounterX = 0;
            int hookCounterY = 0;
            int hookPos = -10;

           
           if (player.Scale.X % 20 == 0 && player.Scale.Y % 20==0)
            {
                hookCounterX = (int) player.Scale.X / 20;
                hookCounterY = (int) player.Scale.Y / 20;
                
               for(int i=0; i<hookCounterX; i++)
                {
                    Vector2 hook = new Vector2(hookPos + 20, 0);
                    Vector2 hook2 = new Vector2(hookPos + 20, player.Scale.Y);
                    hookPos = hookPos + 20;
                    havens.Add(hook);
                    havens.Add(hook2);
                }
                hookPos = -10;
                for (int i = 0; i < hookCounterY; i++)
                {
                    Vector2 hook = new Vector2(0, hookPos + 20);
                    Vector2 hook2 = new Vector2(player.Scale.X,hookPos+20);
                    hookPos = hookPos + 20;
                    havens.Add(hook);
                    havens.Add(hook2);
                }
            }
           else
            {
              Logger.Error($"[DemoGame] - {player.Tag} size must be divisible by 20");
            }

            


            foreach (Vector2 hook in havens)
            {
                Logger.Info($"All hooks X: {hook.X}, Y:{hook.Y}");
            }
            


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
