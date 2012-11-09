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
    public abstract class Rock : WrapEntity
    {
        #region Fields
        private string modelShipFilePath = "Models/Asteroid";
        protected Player player;
        protected RockController rocks;
        private UFOController UFOs;
        protected int maxVelocity;
        #endregion
        public UFOController UFOsReference
        {
            set
            {
                UFOs = value;
            }
        }
        #region Constructor
        public Rock(Game game, Player player, RockController rocks)
            : base(game)
        {
            this.player = player;
            this.rocks = rocks;
        } 
        #endregion
        #region Public and Protected Methods
        public override void Initialize()
        {
            base.Initialize();

            InitializeProximitySphere(1);
            maxVelocity = 20;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ModelMesh = Game.Content.Load<Model>(modelShipFilePath);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DoesPlayerShootRock();
            DoesLargeUFOShotRock();
            DoesSmallUFOShotRock();

            if (player.Enabled)
            {
                DoesRockHitPlayer();
            }

            if (UFOs.LargeUFOEnabled)
            {
                DoesRockHitLargeUFO();
            }
            else if (UFOs.SmallUFOEnabled)
            {
                DoesRockHitSmallUFO();
            }
        }

        public void Activate(Vector3 position)
        {
            Position = position;
            Enabled = true;
            Visible = true;
            RotationVelocity = new Vector3((float)(Engine.Services.RandomNumber.NextDouble()) / 4,
                (float)(Engine.Services.RandomNumber.NextDouble()) / 4,
                (float)(Engine.Services.RandomNumber.NextDouble()) / 4);

            Velocity = new Vector3(Engine.Services.RandomNumber.Next(-maxVelocity, maxVelocity),
                Engine.Services.RandomNumber.Next(-maxVelocity, maxVelocity), 0);

            UpdateMatrixs();
            UpdateProximitySphere();
        }

        public void RockDistroyed()
        {
            Deactivate();
            rocks.PlayExplosion();
        }

        public virtual void RockDistroyedByPlayer()
        {
            Deactivate();
            rocks.PlayExplosion();
        }

        public virtual void Deactivate()
        {
            Enabled = false;
            Visible = false;
        }
        #endregion
        #region Private Methods
        private void DoesPlayerShootRock()
        {
            for (int shotCount = 0; shotCount < player.Shots.Count; shotCount++)
            {
                if (player.Shots[shotCount].Visible)
                {
                    if (ProximitySphere.Intersects(player.Shots[shotCount].ProximitySphere))
                    {
                        player.Shots[shotCount].Deactivate();
                        RockDistroyedByPlayer();
                    }
                }
            }
        }

        private void DoesRockHitPlayer()
        {
            if (ProximitySphere.Intersects(player.ProximitySphere))
            {
                player.GotHit();
                RockDistroyedByPlayer();
            }
        }

        private void DoesLargeUFOShotRock()
        {
            for (int shot = 0; shot < UFOs.LargeUFOShots.Count; shot++)
            {
                if (UFOs.LargeUFOShots[shot].Visible)
                {
                    if (ProximitySphere.Intersects(UFOs.LargeUFOShots[shot].ProximitySphere))
                    {
                        UFOs.LargeUFOShots[shot].Deactivate();
                        RockDistroyed();
                    }
                }
            }
        }

        private void DoesSmallUFOShotRock()
        {
            for (int shot = 0; shot < UFOs.SmallUFOShots.Count; shot++)
            {
                if (UFOs.SmallUFOShots[shot].Visible)
                {
                    if (ProximitySphere.Intersects(UFOs.SmallUFOShots[shot].ProximitySphere))
                    {
                        UFOs.SmallUFOShots[shot].Deactivate();
                        RockDistroyed();
                    }
                }
            }
        }

        private void DoesRockHitLargeUFO()
        {
            if (ProximitySphere.Intersects(UFOs.LargeUFOCollision))
            {
                UFOs.DestroyLargeUFO();
                UFOs.Explosion();
                RockDistroyed();
            }
        }

        private void DoesRockHitSmallUFO()
        {
            if (ProximitySphere.Intersects(UFOs.SmallUFOCollision))
            {
                UFOs.DestroySmallUFO();
                UFOs.Explosion();
                RockDistroyed();
            }
        }
        #endregion
    }
}
