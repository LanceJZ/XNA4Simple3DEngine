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
    public abstract class Controller : GameObject
    {
        #region Fields
        private int maxHeight;
        private int maxWidth; 
        #endregion
        #region Constructor
        public Controller(Game game)
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
        #endregion
        #region Protected Methods
        protected Vector3 SpawnAtRandomLocation()
        {
            int x;
            int y;

            if (Engine.Services.RandomNumber.Next(2) == 1)
                x = -maxWidth + Engine.Services.RandomNumber.Next(maxWidth / 10);
            else
                x = maxWidth - Engine.Services.RandomNumber.Next(maxWidth / 10);

            y = Engine.Services.RandomNumber.Next(maxHeight * 2) - maxHeight;

            return new Vector3(x, y, 0);
        }
        #endregion
    }
}
