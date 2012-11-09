#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Engine.Camera Camera;
        private Entities.Player Player;
        private Entities.Background Background;
        private Entities.RockController Rocks;
        private Entities.UFOController UFOs;
        private Entities.HUD screenHUD;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.Title = "Asteroids 3D in XNA 4 Chapter Five";
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";

            Camera = new Engine.Camera(this, new Vector3(0, 0, 275), Vector3.Forward, Vector3.Zero, false, 200, 325);
            Background = new Entities.Background(this);
            screenHUD = new Entities.HUD(this);
            Player = new Entities.Player(this);
            UFOs = new Entities.UFOController(this);
            Rocks = new Entities.RockController(this);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Engine.Services.Initialize(this, GraphicsDevice, Camera);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Background.spriteBatch = spriteBatch;
            screenHUD.SpriteBatchReferance = spriteBatch;
            Rocks.PlayerReference = Player;
            Rocks.UFOsReference = UFOs;
            Player.ScreenHUDReference = screenHUD;
            Player.UFOsReference = UFOs;
            UFOs.PlayerReference = Player;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !Player.Enabled)
            {
                Rocks.Initialize();
                UFOs.NewGame();
                Player.NewGame();
            }

            base.Update(gameTime);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
