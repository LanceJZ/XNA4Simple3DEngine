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
    public class UFO : WrapEntity
    {
        #region Fields
        protected List<UFOShot> shots;
        protected SoundEffect soundEngine;
        protected Player player;
        protected UFOController UFOs;
        protected Game game;
        protected string modelShipFilePath;
        private float vectorTimer;
        protected float vectorTimeLimit;
        protected float shotTimeLimit;
        protected float shotTimer;
        protected float soundPlayTimeAmount;
        private float soundPlayTime;
        #endregion
        #region Properties
        public List<UFOShot> Shots
        {
            get
            {
                return shots;
            }
        }

        public Player PlayerReference
        {
            set
            {
                player = value;

                foreach (UFOShot shot in shots)
                {
                    shot.PlayerReference = player;
                }
            }
        }

        public UFOController UFOsReference
        {
            set
            {
                UFOs = value;
            }
        }
        #endregion
        #region Constructor
        public UFO(Game game)
            : base(game)
        {
            this.game = game;
            shots = new List<UFOShot>();

            for (int shot = 0; shot < 2; shot++)
                shots.Add(new UFOShot(game));
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            Deactivate();
            vectorTimeLimit = 6;
            shotTimeLimit = 1;
            maxWidth += (int)ProximitySphere.Radius * 3;
            maxHeight += (int)(ProximitySphere.Radius * 1.5f);
            soundPlayTimeAmount = (float)soundEngine.Duration.TotalSeconds;
        }

        public void Activate(Vector3 position)
        {
            Position = position;
            Enabled = true;
            Visible = true;
            shotTimer = ResetTimer(shotTimeLimit);
            vectorTimer = ResetTimer(vectorTimeLimit);

            if (Position.X > 0)
            {
                Velocity.X = Services.RandomNumber.Next(-40, -20);
            }
            else
            {
                Velocity.X = Services.RandomNumber.Next(20, 40);
            }

            if (Position.Y > 0)
            {
                Velocity.Y = Services.RandomNumber.Next(-20, -10);
            }
            else
            {
                Velocity.Y = Services.RandomNumber.Next(10, 20);
            }

            RotationVelocity = new Vector3(0, (float)(Engine.Services.RandomNumber.NextDouble()) * 8, 0);
        }

        public void Deactivate()
        {
            Enabled = false;
            Visible = false;
            UFOs.ResetTimer();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            DoesUFOChangeVector();
            UFOEngineSound();

            if (player.Enabled)
                DoesUFOHitPlayer();
        }

        public override void CheckBorders()
        {
            if (Position.X > maxWidth || Position.X < -maxWidth)
                Deactivate();

            base.CheckBorders();
        }

        public void NewGame()
        {
            Deactivate();

            foreach (UFOShot shot in shots)
                shot.Deactivate();                
        }
        #endregion
        #region Private Methods
        private void DoesUFOChangeVector()
        {
            if (vectorTimer < TotalSeconds)
            {
                Velocity.Y = ChangeVector();
                vectorTimer = ResetTimer(vectorTimeLimit);
            }
        }

        private int ChangeVector()
        {
            int Y;

            if (Services.RandomNumber.NextDouble() > 0.5f)
                Y = Services.RandomNumber.Next(10, 20);
            else
                Y = Services.RandomNumber.Next(-20, -10);

            return Y;
        }

        private void UFOEngineSound()
        {
            if (soundPlayTime < TotalSeconds)
            {
                soundPlayTime = TotalSeconds + soundPlayTimeAmount;
                soundEngine.Play(0.7f, 0, 0);
            }
        }

        private void DoesUFOHitPlayer()
        {
            if (ProximitySphere.Intersects(player.ProximitySphere))
            {
                Deactivate();
                UFOHitPlayer();
                player.GotHit();
            }
        }
        #endregion
        #region Protected methods
        protected override void LoadContent()
        {
            base.LoadContent();

            ModelMesh = Game.Content.Load<Model>(modelShipFilePath);
        }

        protected void FireShotRandom()
        {
            shotTimer = ResetTimer(shotTimeLimit);

            for (int shot = 0; shot < shots.Count; shot++)
            {
                if (!shots[shot].Visible)
                {
                    float angle = (float)Services.RandomNumber.NextDouble() * (MathHelper.Pi * 2);
                    Vector3 velocity = Services.Vector3FromAngle(angle, 80);
                    shots[shot].Activate(Position, velocity);
                    UFOs.FireShot();
                    break;
                }
            }
        }

        protected float ResetTimer(float timerAmount)
        {
            float timer;

            timer = (float)(Services.RandomNumber.NextDouble() * timerAmount + (timerAmount * 0.5f))
                + TotalSeconds;

            return timer;
        }

        protected virtual void UFOHitPlayer()
        {
            UFOs.Explosion();
        }
        #endregion
    }
}
