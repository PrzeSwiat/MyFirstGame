using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstGame.Components
{
    public class Strite2D
    {
        public Vector2 Position = null;
        public Vector2 Scale = null;
        public string Directory = "";
        public string Tag = "";
        public Image Strite = null;
        public Vector2 hook;

        public Strite2D(Vector2 position, Vector2 scale, string directory, string tag)
        {
            this.Position = position;
            this.Scale = scale;
            this.Directory = directory;
            this.Tag = tag;
            
            Image tmp = Image.FromFile($"Assets/Sprites/{directory}.png");
            Bitmap bmp = new Bitmap(tmp, (int)this.Scale.X, (int)this.Scale.Y);
            Strite = bmp;

            //Logger.Info($"[Strite2D] {tag} registered successfully ");
            MainEngine.RegisterStrite(this);
            hook = new Vector2((scale.X / 2), 0);

        }



        public void DestroyObj()
        {
            Logger.Info($"[Strite2D] {Tag} has been destroyed ");
            MainEngine.UnregisterStrite(this);

        }
    }
}
