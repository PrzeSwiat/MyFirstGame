using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstGame.Components
{
    public class Shape2D
    {
        public Vector2 Position = null;
        public Vector2 Scale = null;
        public string Tag = "";

        public Shape2D(Vector2 position, Vector2 scale, string tag)
        {
            this.Position = position;
            this.Scale = scale;
            this.Tag = tag;
            Logger.Info($"[Shape2D] {tag} registered successfully ");
            MainEngine.RegisterShape(this);
        }

        public void DestroyObj() 
        {
            Logger.Info($"[Shape2D] {Tag} has been destroyed ");
            MainEngine.UnregisterShape(this);

        }
    }
}
