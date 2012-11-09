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
    public abstract class WrapEntity : AnimatedModel
    {
        #region Fields
        protected int maxWidth;
        protected int maxHeight; 
        #endregion

        #region Constructor
        public WrapEntity(Game game)
            : base(game)
        {

        }
        #endregion

        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            maxHeight = Engine.Services.WindowHeight / 5;
            maxWidth = Engine.Services.WindowWidth / 5;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CheckBorders();
        }

        public virtual void CheckBorders()
        {
            if (Position.X > maxWidth)
                Position.X = -maxWidth;

            if (Position.X < -maxWidth)
                Position.X = maxWidth;

            if (Position.Y > maxHeight)
                Position.Y = -maxHeight;

            if (Position.Y < -maxHeight)
                Position.Y = maxHeight;
        }
        #endregion
    }
}
