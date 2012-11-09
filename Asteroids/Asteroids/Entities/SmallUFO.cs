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
    public class SmallUFO : UFO
    {
        #region Constructor
        public SmallUFO(Game game)
            : base(game)
        {
            
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            modelShipFilePath = "Models/SmallUFO";

            base.Initialize();

            ScalePercent = new Vector3(0.5f);
            InitializeProximitySphere(0.5f);
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

            soundEngine = game.Content.Load<SoundEffect>("Sounds/UFOSmall");
        }

        protected override void UFOHitPlayer()
        {
            base.UFOHitPlayer();

            player.GotPoints(1000);
        }
        #endregion
        #region Private Methods
        private void DoesUFOShot()
        {
            if (shotTimer < TotalSeconds)
            {
                if (Services.RandomNumber.NextDouble() > 0.25f)
                    FireShotRandom();
                else
                    AimAtShip();
            }
        }

        private void AimAtShip()
        {
            shotTimer = ResetTimer(shotTimeLimit);

            for (int shot = 0; shot < shots.Count; shot++)
            {
                if (!shots[shot].Visible)
                {
                    float angle = Services.AngleInRadians(Position, player.Position);
                    Vector3 velocity = Services.Vector3FromAngle(angle, 80);
                    shots[shot].Activate(Position, velocity);
                    UFOs.FireShot();
                    break;
                }
            }
        } 
        #endregion
    }
}
