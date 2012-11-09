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
    public class UFOController : Controller
    {
        #region Fields
        private LargeUFO largeUFOShip;
        private SmallUFO smallUFOShip;
        private Player player;
        private SoundEffect soundExplosion;
        private SoundEffect soundShot;
        private float spawnTimer;
        private float spawnTimeLimit;
        private int spawnCounter;
        private bool spawnedUFO;
        private Game game;
        #endregion
        #region Properties
        public BoundingSphere LargeUFOCollision
        {
            get
            {
                return largeUFOShip.ProximitySphere;
            }
        }

        public BoundingSphere SmallUFOCollision
        {
            get
            {
                return smallUFOShip.ProximitySphere;
            }
        }

        public bool LargeUFOEnabled
        {
            get
            {
                return largeUFOShip.Enabled;
            }
        }

        public bool SmallUFOEnabled
        {
            get
            {
                return smallUFOShip.Enabled;
            }
        }

        public List<UFOShot> LargeUFOShots
        {
            get
            {
                return largeUFOShip.Shots;
            }
        }

        public List<UFOShot> SmallUFOShots
        {
            get
            {
                return smallUFOShip.Shots;
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
        public UFOController(Game game)
            : base(game)
        {
            this.game = game;
            largeUFOShip = new LargeUFO(game);
            smallUFOShip = new SmallUFO(game);
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            spawnTimeLimit = 10.5f;
            spawnCounter = 0;
            largeUFOShip.PlayerReference = player;
            largeUFOShip.UFOsReference = this;
            smallUFOShip.PlayerReference = player;
            smallUFOShip.UFOsReference = this;
            ResetTimer();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!spawnedUFO)
                DoesUFOSpawn();
        }

        public void NewGame()
        {
            Initialize();
            largeUFOShip.NewGame();
            smallUFOShip.NewGame();
        }

        public void ResetTimer()
        {
            spawnTimer = spawnTimeLimit + TotalSeconds + Services.RandomNumber.Next((int)(spawnTimeLimit * 0.5f));
            spawnedUFO = false;
        }

        public void DestroyLargeUFO()
        {
            largeUFOShip.Deactivate();
        }

        public void DestroySmallUFO()
        {
            smallUFOShip.Deactivate();
        }

        public void Explosion()
        {
            soundExplosion.Play();
        }

        public void FireShot()
        {
            soundShot.Play();
        }
        #endregion
        #region Private and Protected Methods
        protected override void LoadContent()
        {
            base.LoadContent();

            soundExplosion = game.Content.Load<SoundEffect>("Sounds/UFOExplosion");
            soundShot = game.Content.Load<SoundEffect>("Sounds/UFOShot");
        }

        private void DoesUFOSpawn()
        {
            if (spawnTimer < TotalSeconds)
            {
                double spawnPercent = Math.Pow(0.915d, (double)spawnCounter);

                if (Services.RandomNumber.NextDouble() < spawnPercent)
                {
                    largeUFOShip.Activate(SpawnAtRandomLocation());
                }
                else
                {
                    smallUFOShip.Activate(SpawnAtRandomLocation());
                }

                spawnCounter++;
                spawnedUFO = true;
            }
        }
        #endregion
    }
}
