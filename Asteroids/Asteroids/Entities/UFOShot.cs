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
    public class UFOShot : Shot
    {
        #region Fields
        private Player player;
        #endregion
        public Player PlayerReference
        {
            set
            {
                player = value;
            }
        }
        #region Constructor
        public UFOShot(Game game)
            : base(game)
        {

        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            emissiveColor = new Vector3(0.05f, 0, 0.80f);
            shotTimeLimit = 1.95f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (player.Enabled)
                DoesShotHitPlayer();
        }
        #endregion
        #region Private Methods
        private void DoesShotHitPlayer()
        {
            if (ProximitySphere.Intersects(player.ProximitySphere))
            {
                Deactivate();
                player.GotHit();
            }
        }
        #endregion
    }
}
