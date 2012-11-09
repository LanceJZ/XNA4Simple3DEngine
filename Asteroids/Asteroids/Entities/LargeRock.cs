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
    public class LargeRock : Rock
    {
        #region Constructor
        public LargeRock(Game game, Player player, RockController rocks)
            : base(game, player, rocks)
        {

        }
        #endregion
        #region Public Methods
        public override void RockDistroyedByPlayer()
        {
            base.RockDistroyedByPlayer();
            player.GotPoints(20);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            rocks.SpawnMedRocks(Position);
        }
        #endregion
    }
}
