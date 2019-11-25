using Microsoft.Xna.Framework.Graphics;

namespace Ursine
{
    public class Terrain : Object
    {
        public Terrain()
        {
        }

        public Terrain(int x, int y, int z, Texture2D t, int width, int height, bool passable, int PassCost) : base(x, y, z, t, width, height)
        {
        }

        public bool Passable { get; set; }
        public int PassCost { get; set; }

    }
}
