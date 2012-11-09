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
    public class Shot : WrapEntity
    {
        #region Fields
        private string modelShipFilePath = "Models/Shot";
        protected Vector3 emissiveColor;
        private float shotTimer;
        protected float shotTimeLimit;
        #endregion

        #region Constructor
        public Shot(Game game)
            : base(game)
        {

        }
        #endregion

        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            Visible = false;
            emissiveColor = new Vector3(0);
            InitializeProximitySphere(1);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ModelMesh = Game.Content.Load<Model>(modelShipFilePath);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DoesShotExpire();
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (BasicEffect effect in ModelMesh.Meshes[0].Effects)
            {
                effect.EmissiveColor = emissiveColor;
            }

            base.Draw(gameTime);
        }

        public void Activate(Vector3 position, Vector3 velocity)
        {
            shotTimer = shotTimeLimit + TotalSeconds;
            Position = position;
            Velocity = velocity;
            Enabled = true;
            Visible = true;

            RotationVelocity = new Vector3((float)(Engine.Services.RandomNumber.NextDouble()),
                (float)(Engine.Services.RandomNumber.NextDouble()),
                (float)(Engine.Services.RandomNumber.NextDouble()));

            UpdateMatrixs();
            UpdateProximitySphere();
        }

        public void Deactivate()
        {
            Enabled = false;
            Visible = false;
        }
        #endregion
        #region Private Methods
        private void DoesShotExpire()
        {
            if (shotTimer < TotalSeconds)
            {
                Deactivate();
            }
        }
        #endregion
    }
}
