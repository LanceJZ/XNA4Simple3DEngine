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
    public class PlayerShot : Shot
    {
        #region Fields
        private UFOController UFOs;
        private Player player;
        #endregion
        #region Properties
        public UFOController UFOsReference
        {
            set
            {
                UFOs = value;
            }
        }

        public Player PlayerReference
        {
            set
            {
                player = value;
            }
        }
        #endregion
        #region Constructor
        public PlayerShot(Game game)
            : base(game)
        {

        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            emissiveColor = new Vector3(0.30f, 0, 0.98f);
            shotTimeLimit = 2;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DoesShotHitUFO();
        }
        #endregion
        #region Private Methods
        private void DoesShotHitUFO()
        {
            if (UFOs.LargeUFOEnabled)
            {
                if (ProximitySphere.Intersects(UFOs.LargeUFOCollision))
                {
                    player.GotPoints(200);
                    Deactivate();
                    UFOs.DestroyLargeUFO();
                    UFOs.Explosion();
                }
            }
            else if (UFOs.SmallUFOEnabled)
            {
                if (ProximitySphere.Intersects(UFOs.SmallUFOCollision))
                {
                    player.GotPoints(1000);
                    Deactivate();
                    UFOs.DestroySmallUFO();
                    UFOs.Explosion();
                }
            }
        }
        #endregion
    }
}
