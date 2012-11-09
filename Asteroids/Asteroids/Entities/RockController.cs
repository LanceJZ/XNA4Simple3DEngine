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
    public class RockController : Controller
    {
        #region Fields
        private List<LargeRock> RocksLarge;
        private List<MediumRock> RocksMedium;
        private List<SmallRock> RocksSmall;
        private Player player;
        private UFOController UFOs;
        private Game game;
        private SoundEffect soundExplosion;
        private int numberOfRocks;
        #endregion
        public Player PlayerReference
        {
            set
            {
                player = value;
            }
        }

        public UFOController UFOsReference
        {
            set
            {
                UFOs = value;
            }
        }
        #region Constructor
        public RockController(Game game)
            : base(game)
        {
            RocksLarge = new List<LargeRock>();
            RocksMedium = new List<MediumRock>();
            RocksSmall = new List<SmallRock>();
            this.game = game;
        } 
        #endregion
        #region Public Methods
        protected override void LoadContent()
        {
            base.LoadContent();

            soundExplosion = game.Content.Load<SoundEffect>("Sounds/RockExplosion");
        }

        public override void Initialize()
        {
            base.Initialize();

            NewGame();
            numberOfRocks = 4;
            SpawnNewWave(numberOfRocks);
        }
        /// <summary>
        /// Spawns two medium sized rocks where the large one was.
        /// </summary>
        /// <param name="position"> Position of large rock.</param>
        public void SpawnMedRocks(Vector3 position)
        {
            for (int rockCount = 0; rockCount < 2; rockCount++)
            {
                bool spawnNewRock = true;

                for (int rockCheck = 0; rockCheck < RocksMedium.Count; rockCheck++)
                {
                    if (!RocksMedium[rockCheck].Enabled)
                    {
                        spawnNewRock = false;
                        RocksMedium[rockCheck].Activate(position);
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    RocksMedium.Add(new MediumRock(game, player, this));
                    RocksMedium[RocksMedium.Count - 1].Activate(position);
                    RocksMedium[RocksMedium.Count - 1].UFOsReference = UFOs;
                }
            }
        }
        /// <summary>
        /// Spawns two small sized rocks where the medium one was.
        /// </summary>
        /// <param name="position"> Position of the medium rock.</param>
        public void SpawnSmallRocks(Vector3 position)
        {
            for (int rockCount = 0; rockCount < 2; rockCount++)
            {
                bool spawnNewRock = true;

                for (int rockCheck = 0; rockCheck < RocksSmall.Count; rockCheck++)
                {
                    if (!RocksSmall[rockCheck].Enabled)
                    {
                        spawnNewRock = false;
                        RocksSmall[rockCheck].Activate(position);
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    RocksSmall.Add(new SmallRock(game, player, this));
                    RocksSmall[RocksSmall.Count - 1].Activate(position);
                    RocksSmall[RocksSmall.Count - 1].UFOsReference = UFOs;
                }
            }
        }

        public void SmallRockDistroyed()
        {
            if (CheckEndOfWave())
            {
                if (numberOfRocks < 12)
                    numberOfRocks++;

                SpawnNewWave(numberOfRocks);
            }
        }

        public void PlayExplosion()
        {
            soundExplosion.Play();
        }
        #endregion
        #region Private Methods
        private void NewGame()
        {
            foreach (Rock rock in RocksLarge)
            {
                rock.Enabled = false;
                rock.Visible = false;
            }

            foreach (Rock rock in RocksMedium)
            {
                rock.Enabled = false;
                rock.Visible = false;
            }

            foreach (Rock rock in RocksSmall)
            {
                rock.Enabled = false;
                rock.Visible = false;
            }
        }

        private void SpawnNewWave(int numberOfRocks)
        {
            for (int rock = 0; rock < numberOfRocks; rock++)
            {
                bool spawnNewRock = true;

                for (int rockCheck = 0; rockCheck < RocksLarge.Count; rockCheck++)
                {
                    if (!RocksLarge[rockCheck].Enabled)
                    {
                        RocksLarge[rockCheck].Activate(SpawnAtRandomLocation());
                        spawnNewRock = false;
                        break;
                    }
                }

                if (spawnNewRock)
                {
                    RocksLarge.Add(new LargeRock(game, player, this));
                    RocksLarge[RocksLarge.Count - 1].Activate(SpawnAtRandomLocation());
                    RocksLarge[RocksLarge.Count - 1].UFOsReference = UFOs;
                }
            }
        }

        private bool CheckEndOfWave()
        {
            bool endWave = true;

            foreach (Rock rock in RocksLarge)
            {
                if (rock.Enabled)
                    endWave = false;
            }

            foreach (Rock rock in RocksMedium)
            {
                if (rock.Enabled)
                    endWave = false;
            }

            foreach (Rock rock in RocksSmall)
            {
                if (rock.Enabled)
                    endWave = false;
            }

            return endWave;
        }
        #endregion
    }
}
