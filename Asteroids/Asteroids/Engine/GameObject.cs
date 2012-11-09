#region Using
using System;
using Microsoft.Xna.Framework;
#endregion

namespace Asteroids.Engine
{
    public abstract class GameObject : DrawableGameComponent
    {
        private GameTime gameTime;

        #region Properties
        /// <summary>
        /// Returns total elapsed time game has been running in seconds, including hours, minutes, seconds, in milliseconds.
        /// </summary>
        /// <returns>float</returns>
        public float TotalSeconds //TODO: this was moved out of Services, so there are less out of class calls. Fix in tutor.
        {
            get
            {
                if (gameTime != null)
                {
                    return (float)((gameTime.TotalGameTime.Hours * 3600) + (gameTime.TotalGameTime.Minutes * 60)
                        + gameTime.TotalGameTime.Seconds + (gameTime.TotalGameTime.Milliseconds * .001));
                }
                else
                    return 0;
            }
        }
        #endregion
        public GameObject(Game game)
            : base(game)
        {
            game.Components.Add(this);
        }
        #region Public Methods
        /// <summary>
        /// Allows the game component to be updated.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.gameTime = gameTime;
        }
        #endregion
    }
}
