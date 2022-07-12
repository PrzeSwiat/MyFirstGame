using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFirstGame.Components
{
    class Canvas : Form
    { 
        public Canvas()
        {
            this.DoubleBuffered = true;
            
        }
    
    }
    public abstract class MainEngine
    {
        private Vector2 ScreenSize = new Vector2(512, 512);
        public string title = "Novum";
        private Canvas Window = null;
        private Thread MainThread = null;
        private Vector2 CameraPos = Vector2.Zero();
        private static List<Shape2D> shape2Ds = new List<Shape2D>();
        private static List<Strite2D> strite2Ds = new List<Strite2D>();
        private Vector2 MousePos = Vector2.Zero();


        public float GetWindowX()
        {
            return Window.Width;
        }
        public float GetWindowY()
        {
            return Window.Height;
        }

        protected MainEngine(Vector2 screenSize, string title)
        {
            Logger.Info("Game is starting...");
            this.ScreenSize = screenSize;
            this.title = title;

            Window = new Canvas();
            Window.Size = new Size((int)ScreenSize.X, (int)ScreenSize.Y);
            Window.Text = title;
            Window.Paint += Window_Paint;
            Window.KeyDown += Window_KeyDown;
            Window.KeyUp += Window_KeyUp;

            MainThread = new Thread(GameLoop);
            MainThread.Start(); 

            Application.Run(Window);
        }


        public Vector2 GetMousePos()
        {
            return MousePos;
        }


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            GetKeyUp(e);

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            GetKeyDown(e);
        }

        public static void RegisterShape(Shape2D shape) 
        {
            shape2Ds.Add(shape);
        }

        public static void UnregisterShape(Shape2D shape)
        {
            shape2Ds.Remove(shape);
            
        }
        public static void RegisterStrite(Strite2D shape)
        {
            strite2Ds.Add(shape);
        }

        public static void UnregisterStrite(Strite2D shape)
        {
            strite2Ds.Remove(shape);

        }

        void GameLoop() 
        {
            OnLoad();
            while (MainThread.IsAlive)
            {
                try
                { 
                
                    OnDraw();

                    Window.BeginInvoke((MethodInvoker)delegate { Window.Refresh(); });
                    OnUpdate();
                    MousePos.X = Cursor.Position.X;
                    MousePos.Y = Cursor.Position.Y;
                    Thread.Sleep(5);
                }
                catch 
                {
                    Logger.Error("[MainEngine] - Game is not found...");
                    MainThread.Abort();
                }


            }

        }

        private void Window_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear (Color.Gray);
            g.TranslateTransform(-CameraPos.X+(Window.Width/2), -CameraPos.Y+(Window.Height/2));

           try {
                foreach (Strite2D strite in strite2Ds)
                {
                    string name = strite.Tag;
                    int found = name.IndexOf(" ");
                    if (name.Substring(found + 1) == "jet")
                    {
                        g.DrawImage(strite.Strite, strite.Position.X, strite.Position.Y, strite.Scale.X, strite.Scale.Y);
                    }
                }
                foreach (Shape2D shape in shape2Ds)
                {
                    
                    if (shape.Tag == "Player")
                    {
                        CameraPos.X = shape.Position.X;
                        CameraPos.Y = shape.Position.Y;
                        g.FillRectangle(new SolidBrush(Color.Red), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);

                    }
                    
                    else
                    {
                        g.FillEllipse(new SolidBrush(Color.Black), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);

                    }


                }
                
            } catch
            {
                Logger.Warning("[MainEngine] - Player removed from board");
            }
            





        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
        public abstract void GetKeyDown(KeyEventArgs e);
        public abstract void GetKeyUp(KeyEventArgs e);

    }
}
