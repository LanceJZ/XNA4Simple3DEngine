#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Asteroids.Engine;
#endregion

namespace Asteroids.Entities
{
    public class SmallRock : Rock
    {
        #region Constructor
        public SmallRock(Game game, Player player, RockController rocks)
            : base(game, player, rocks)
        {

        } 
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();
            ScalePercent = new Vector3(0.25f);
            InitializeProximitySphere(0.25f);
            maxVelocity = 35;
        }

        public override void RockDistroyedByPlayer()
        {
            base.RockDistroyedByPlayer();
            player.GotPoints(100);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            rocks.SmallRockDistroyed();
        }
        #endregion
    }
}