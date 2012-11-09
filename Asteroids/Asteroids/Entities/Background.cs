#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Asteroids.Engine;
#endregion

namespace Asteroids.Entities
{
    public class Background : DrawableGameComponent
    {
        #region Fields
        private Texture2D SpriteTexture;
        private string spriteFileName = "Textures/starfield";
        #endregion
        #region Properties
        public SpriteBatch spriteBatch
        {
            set;
            get;
        }
        #endregion
        #region Constructor
        public Background(Game game)
            : base(game)
        {
            game.Components.Add(this);
        } 
        #endregion
        #region Public Methods
        protected override void LoadContent()
        {
            base.LoadContent();
            SpriteTexture = Game.Content.Load<Texture2D>(spriteFileName);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            spriteBatch.Draw(SpriteTexture, new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }
        #endregion
    }
}
