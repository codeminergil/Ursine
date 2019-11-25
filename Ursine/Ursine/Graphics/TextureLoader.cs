namespace Ursine.Graphics
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    public class TextureLoader
    {
        public List<Texture2D> InitialiseTextures(ContentManager Content)
        {
            List<Texture2D> TextureList = new List<Texture2D>();

            TextureList.Add( Content.Load<Texture2D>("tile"));
            TextureList.Add(Content.Load<Texture2D>("tile2"));
            TextureList.Add(Content.Load<Texture2D>("tileRIverBankLeft"));
            TextureList.Add(Content.Load<Texture2D>("tileRIverBankRight"));
            TextureList.Add(Content.Load<Texture2D>("grassTile"));

            return TextureList;

        }
    }
}
