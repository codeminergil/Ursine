using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ursine
{
    public class Object
    {
        public int X { get; set;}
        public int Y { get; set; }
        public Vector2 vect { get; set; }
        public int Z { get; set; }
        public Texture2D Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Object(int x, int y, int z, Texture2D t, int width, int height)
        {
            X = x;
            Y = y;
            Z = z;
            Texture = t;
            Width = width;
            Height = height;
            vect = new Vector2(x, y);
        }

        public Object()
        { }

    }
}
