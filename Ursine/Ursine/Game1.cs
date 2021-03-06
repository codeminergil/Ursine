﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Ursine.Graphics;

namespace Ursine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Texture2D ballTexture;
        Texture2D cursorTexture;
        Vector2 mapPosition;
      //  Vector2 ballPosition;
        int scrollX;
        int scrollY;
        float ballSpeed;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GeometryFactory g = new GeometryFactory();
        MapGrid mapGrid;
        Vector2 targIso;
        Vector2 targCart;
        Vector2 Cords;
        string CordsStringClick;
        string CordsStringIso;
        string CordsStringCart;
        PlayerAgent player = new PlayerAgent();
        MapReader map;
        private SpriteFont font;

        List<Texture2D> TextureList;

        Vector2 mousePos;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            TextureLoader textureLoader = new TextureLoader();
            TextureList = textureLoader.InitialiseTextures(Content);//loads map textures
            map = new MapReader("Ursine.Resources.testMap2.txt");
            map.PlotMap(TextureList);
            scrollY = 0;
            scrollX = 0;
            mapPosition = new Vector2(0, 0);
            ballSpeed = 150f;

            player.goalCord = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            player.IsoCord = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            this.IsMouseVisible = true;

            mapGrid = new MapGrid(40, 40);
            mapGrid.ClearGrid(map.TerArray);
            mapGrid.PlotAStar(7, 0, 7, 0, map.TerArray);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball");
            font = Content.Load<SpriteFont>("Fonts/Cords");
            cursorTexture = Content.Load<Texture2D>("TileHighlight");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            //if (kstate.IsKeyDown(Keys.Up))
            //    //ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    player.IsoCord.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (kstate.IsKeyDown(Keys.Down))
            //    ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (kstate.IsKeyDown(Keys.Left))
            //    ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (kstate.IsKeyDown(Keys.Right))
            //    ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            var mousestate = Mouse.GetState();

            //Cart2Iso((map.TerArray[x, y].X * map.TerArray[x, y].Height) - (scrollX + scrollY),
            //    (map.TerArray[x, y].Y * map.TerArray[x, y].Height) - (scrollY - scrollX))

            Cords = new Vector2(mousestate.X + scrollX, mousestate.Y + scrollY);
            CordsStringIso = (Cords.X).ToString() + "," + (Cords.Y).ToString();

            CordsStringCart = System.Math.Round(g.Iso2Cart(Cords.X, Cords.Y).X).ToString()
                                + "," +
                                System.Math.Round(g.Iso2Cart(Cords.X, Cords.Y).Y).ToString();

            CordsStringClick = System.Math.Round(targCart.X).ToString()
                                + "," +
                                System.Math.Round(targCart.Y).ToString();

            if (kstate.IsKeyDown(Keys.W) || (mousestate.Y < 40 && mousestate.Y >= 0))
                scrollY -=5;

            if (kstate.IsKeyDown(Keys.S) || mousestate.Y > GraphicsDevice.DisplayMode.Height -40)
                scrollY+=5;

            if (kstate.IsKeyDown(Keys.A) || (mousestate.X < 40 && mousestate.X >= 0)) 
                scrollX-=5;

            if (kstate.IsKeyDown(Keys.D) || mousestate.X > GraphicsDevice.DisplayMode.Width -40)
                scrollX+=5;

            if (mousestate.LeftButton == ButtonState.Pressed)
            {
                mousePos = new Vector2(mousestate.X + scrollX, mousestate.Y + scrollY);
                targCart = g.Iso2Cart((float)System.Math.Round(mousePos.X) , (float)System.Math.Round(mousePos.Y) );
                targIso = mousePos;

                //prevent clicking into the void
                if (targCart.X < 0) { targCart.X = 0; targIso.X = g.Cart2Iso((float)System.Math.Round(targCart.X), (float)System.Math.Round(targCart.Y)).X; }
                if (targCart.Y < 0) { targCart.Y = 0; targIso.Y = g.Cart2Iso((float)System.Math.Round(targCart.X), (float)System.Math.Round(targCart.Y)).Y; }
                if (targCart.X > 39) { targCart.X = 39; targIso.X = g.Cart2Iso((float)System.Math.Round(targCart.X), (float)System.Math.Round(targCart.Y)).X; }
                if (targCart.Y > 19) { targCart.Y = 19; targIso.Y = g.Cart2Iso((float)System.Math.Round(targCart.X), (float)System.Math.Round(targCart.Y)).Y; }
                
                mapGrid.ClearGrid(map.TerArray);
                mapGrid.PlotAStar(7, 0, (int)targCart.X, (int)targCart.Y, map.TerArray);    //pass player later
            }

            if (player.IsoCord.X< targIso.X)
            {
                player.IsoCord = new Vector2(player.IsoCord.X + ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, player.IsoCord.Y);
            }
            if (player.IsoCord.X > targIso.X)
            {
                player.IsoCord = new Vector2(player.IsoCord.X - ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, player.IsoCord.Y);
            }
            if (player.IsoCord.Y < targIso.Y)
            {
                player.IsoCord = new Vector2(player.IsoCord.X, player.IsoCord.Y + ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (player.IsoCord.Y > targIso.Y)
            {
                player.IsoCord = new Vector2(player.IsoCord.X, player.IsoCord.Y - ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }


            //if(ballPosition.X < mousePos.X )
            //{
            //    ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            //if (ballPosition.X > mousePos.X)
            //{
            //    ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            //if (ballPosition.Y < mousePos.Y )
            //{
            //    ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}
            //if (ballPosition.Y > mousePos.Y )
            //{
            //    ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //}

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Magenta);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            map.PlotMap(TextureList);
            // foreach (var tile in map.MapTerrainList) 
            for (int x = 0; x < map.TerArray.GetLength(0); x++)
            {
                for (int y = 0; y < map.TerArray.GetLength(1); y++)
                {

                    spriteBatch.DrawString(
                                    font, 
                                    map.TerArray[x,y].X.ToString() + "," + map.TerArray[x, y].Y.ToString(),
                                    g.Cart2Iso((map.TerArray[x, y].X * map.TerArray[x, y].Height) - (scrollX + scrollY), (map.TerArray[x, y].Y * map.TerArray[x, y].Height) - (scrollY - scrollX)),
                                    Color.Black);
                    //MAp
                    spriteBatch.Draw(
                            map.TerArray[x, y].Texture,
                            g.Cart2Iso((map.TerArray[x, y].X * map.TerArray[x, y].Height) - (scrollX + scrollY), (map.TerArray[x, y].Y * map.TerArray[x, y].Height) - (scrollY - scrollX)),
                            Color.White
                            );
                }
            }

            Vector2 cursVector = g.Cart2Iso((float)System.Math.Round(targCart.X), (float)System.Math.Round(targCart.Y));

            spriteBatch.Draw(
               cursorTexture,
             new Vector2((cursVector.X*50-50) /*+scrollX*/, (cursVector.Y*50-25) /*+scrollY*/ ),
            Color.White
               );

            spriteBatch.Draw(
                    ballTexture,
                    new Vector2(player.IsoCord.X + scrollX, player.IsoCord.Y +scrollY),
                    null,
                    Color.White,
                    0f,
                    new Vector2(0, ballTexture.Height),
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                    );
            //debug BS below
            spriteBatch.DrawString(font, "Cords Iso:" + CordsStringIso, new Vector2(1000, 100), Color.Black);
            spriteBatch.DrawString(font, "Cords Click:" + CordsStringClick, new Vector2(1000, 150), Color.Black);
            spriteBatch.DrawString(font, "Cords Cart:" + CordsStringCart, new Vector2(1000, 200), Color.Black);
            spriteBatch.DrawString(font, "scrollx:" + scrollX, new Vector2(1000, 250), Color.Black);
            spriteBatch.DrawString(font, "scrolly:" + scrollY, new Vector2(1000, 300), Color.Black);

            spriteBatch.DrawString(font, "Array:" , new Vector2(1100, 10), Color.Black);
            string stringArray ="";
            for (int ay = 0; ay < mapGrid.PlayerAStarArray.GetLength(1); ay++)
            {
                for (int ax = 0; ax < mapGrid.PlayerAStarArray.GetLength(0); ax++)
                {
                    if (mapGrid.PlayerAStarArray[ax, ay].ToString().Length == 2)
                    { stringArray += "0" + mapGrid.PlayerAStarArray[ax, ay].ToString() + ","; }
                    else if (mapGrid.PlayerAStarArray[ax, ay].ToString().Length == 1)
                    { stringArray += "00" + mapGrid.PlayerAStarArray[ax, ay].ToString() + ","; }
                    else
                    { stringArray += mapGrid.PlayerAStarArray[ax, ay].ToString() + ","; }
                }
                stringArray += "\n";
            }

            spriteBatch.DrawString(font, stringArray , new Vector2(1100, 20), Color.Black);

            spriteBatch.DrawString(font, gameTime.TotalGameTime.ToString(), new Vector2(1000, 10), Color.Black);
            int v = 0;
            foreach(Terrain t in mapGrid.Path)
            {
                spriteBatch.DrawString(font, t.X+","+t.Y, new Vector2(900, 20+v), Color.Black);
                v+= 14;
            }
            v = 0;
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
