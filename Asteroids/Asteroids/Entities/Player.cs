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
    public class Player : WrapEntity
    {
        #region Fields
        private string modelShipFilePath = "Models/Player";
        private List<PlayerShot> shots;
        private HUD screenHUD;
        private UFOController UFOs;
        private SoundEffect soundShot;
        private SoundEffect soundHit;
        private SoundEffect soundThrust;
        private SoundEffect soundBonus;
        private float maxShipSpeed = 150;
        private float thrustSoundTime;
        private float thrustSoundTimeAmount;
        private int nextFreeHit;
        private bool hyper;
        private bool firebutton;
        private bool playerMusic;
        #endregion
        #region Properties
        public List<PlayerShot> Shots
        {
            get
            {
                return shots;
            }
        }

        public HUD ScreenHUDReference
        {
            set
            {
                screenHUD = value;
            }
        }

        public UFOController UFOsReference
        {
            set
            {
                UFOs = value;

                foreach (PlayerShot shot in shots)
                {
                    shot.PlayerReference = this;
                    shot.UFOsReference = UFOs;
                }
            }
        }
        #endregion
        #region Constructor
        public Player(Game game)
            : base(game)
        {
            shots = new List<PlayerShot>();

            for (int shotCount = 0; shotCount < 4; shotCount++)
            {
                shots.Add(new PlayerShot(game));
            }
        }
        #endregion
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            RotationInRadians.X = MathHelper.PiOver2;
            thrustSoundTimeAmount = (float)soundThrust.Duration.TotalSeconds;
            NewGame();
            GameOver();
            UpdateMatrixs();
            InitializeProximitySphere(0.33f);
            UpdateProximitySphere();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ModelMesh = Game.Content.Load<Model>(modelShipFilePath);
            soundShot = Game.Content.Load<SoundEffect>("Sounds/PlayerShot");
            soundHit = Game.Content.Load<SoundEffect>("Sounds/PlayerExplosion");
            soundThrust = Game.Content.Load<SoundEffect>("Sounds/PlayerThrust");
            soundBonus = Game.Content.Load<SoundEffect>("Sounds/WaveBonus");
            Services.Music = Game.Content.Load<Song>("Music/Background");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyInputControl();
        }

        public void NewGame()
        {
            Position = new Vector3(0);
            Velocity = new Vector3(0);
            Acceleration = new Vector3(0);
            RotationAcceleration = new Vector3(0);
            RotationVelocity = new Vector3(0);
            RotationInRadians.Z = MathHelper.Pi;
            screenHUD.PlayerHitsLeft = 3;
            screenHUD.PlayerScore = 0;
            Enabled = true;
            Visible = true;
            nextFreeHit = 10000;
            screenHUD.GameOver = false;
            SoundEffect.MasterVolume = 0.2f;

            if (playerMusic)
                MediaPlayer.Play(Services.Music);
        }

        public void GotPoints(int score)
        {
            screenHUD.PlayerScore += score;
            screenHUD.UpdateHighScore();

            if (screenHUD.PlayerScore > nextFreeHit)
            {
                soundBonus.Play();
                screenHUD.PlayerHitsLeft++;
                nextFreeHit += 10000;
            }
        }

        public void GotHit()
        {
            screenHUD.PlayerHitsLeft--;
            soundHit.Play();

            if (screenHUD.PlayerHitsLeft < 1)
            {
                GameOver();
            }
        }
        #endregion
        #region Private Methods
        private void GameOver()
        {
            Enabled = false;
            Visible = false;
            screenHUD.GameOver = true;
            SoundEffect.MasterVolume = 0.01f;

            if (playerMusic)
                MediaPlayer.Stop();
        }

        private void KeyInputControl()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (thrustSoundTime < TotalSeconds)
                {
                    thrustSoundTime = (float)(TotalSeconds + thrustSoundTimeAmount);
                    soundThrust.Play();
                }

                Acceleration = Engine.Services.Vector3FromAngle(RotationInRadians.Z - 
                    MathHelper.PiOver2, 50);

                if (Velocity.X > maxShipSpeed)
                {
                    Velocity.X = maxShipSpeed;
                    Acceleration.X = 0;
                }
                else if (Velocity.X < -maxShipSpeed)
                {
                    Velocity.X = -maxShipSpeed;
                    Acceleration.X = 0;
                }
                else if (Velocity.Y > maxShipSpeed)
                {
                    Velocity.Y = maxShipSpeed;
                    Acceleration.Y = 0;
                }
                else if (Velocity.Y < -maxShipSpeed)
                {
                    Velocity.Y = -maxShipSpeed;
                    Acceleration.Y = 0;
                }
            }
            else
                Acceleration = new Vector3(0);

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                RotationVelocity.Z = MathHelper.Pi;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                RotationVelocity.Z = -MathHelper.Pi;
            }
            else
                RotationVelocity.Z = 0;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                if (!firebutton)
                {
                    FireShot();
                    firebutton = true;
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Space) && Keyboard.GetState().IsKeyUp(Keys.LeftControl) && firebutton)
            {
                firebutton = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.RightShift) && !hyper)
            {
                Hyperspace();
                hyper = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.RightShift) && hyper)
                hyper = false;

            if (Keyboard.GetState().IsKeyDown(Keys.M))
            {
                MediaPlayer.Play(Services.Music);
                playerMusic = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M) && Keyboard.GetState().IsKeyDown(Keys.RightAlt))
            {
                MediaPlayer.Stop();
                playerMusic = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Add))
            {
                if (MediaPlayer.Volume < 1)
                    MediaPlayer.Volume += 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
            {
                if (MediaPlayer.Volume > 0)
                    MediaPlayer.Volume -= 0.01f;
            }
        }

        private void Hyperspace()
        {
            Position.X = Services.RandomNumber.Next(-maxWidth, maxWidth);
            Position.Y = Services.RandomNumber.Next(-maxHeight, maxHeight);
            Velocity = new Vector3(0);
            Acceleration = new Vector3(0);
        }

        private void FireShot()
        {
            for (int shotCount = 0; shotCount < shots.Count; shotCount++)
            {
                if (!shots[shotCount].Visible)
                {
                    soundShot.Play();

                    shots[shotCount].Activate(Position, Engine.Services.Vector3FromAngle(RotationInRadians.Z -
                        MathHelper.PiOver2, 110));

                    break;
                }
            }
        }
        #endregion
    }
}
