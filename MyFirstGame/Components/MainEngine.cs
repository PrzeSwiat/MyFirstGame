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


        private static List<Shape2D> shape2Ds = new List<Shape2D>();


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
            g.FillRectangle(new SolidBrush(Color.Red), shape2Ds[0].Position.X, shape2Ds[0].Position.Y, shape2Ds[0].Scale.X, shape2Ds[0].Scale.Y);

            foreach (Shape2D shape in shape2Ds)
            {
                if (shape != shape2Ds[0])
                g.FillEllipse(new SolidBrush(Color.Blue), shape.Position.X, shape.Position.Y, shape.Scale.X, shape.Scale.Y);
            }



        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
        public abstract void GetKeyDown(KeyEventArgs e);
        public abstract void GetKeyUp(KeyEventArgs e);
    }
}
