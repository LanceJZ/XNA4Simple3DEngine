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
    public class LargeUFO : UFO
    {
        #region Constructor
        public LargeUFO(Game game)
            : base(game)
        {
            
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            modelShipFilePath = "Models/LargeUFO";

            base.Initialize();

            InitializeProximitySphere(0.75f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DoesUFOShot();
        }
        #endregion
        #region Protected Methods
        protected override void LoadContent()
        {
            base.LoadContent();

            soundEngine = game.Content.Load<SoundEffect>("Sounds/UFOLarge");
        }

        protected override void UFOHitPlayer()
        {
            base.UFOHitPlayer();

            player.GotPoints(200);
        }
        #endregion
        #region Private Methods
        private void DoesUFOShot()
        {
            if (shotTimer < TotalSeconds)
                FireShotRandom();
        }
        #endregion
    }
}
