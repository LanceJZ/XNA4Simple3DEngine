#region Using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Asteroids.Engine
{
    public class AnimatedModel : PositionedObject
    {
        #region Fields
        private Matrix worldMatrix;
        private Matrix worldTranslate;
        private Matrix worldRotate;
        private Matrix worldScale;
        private BoundingSphere proximitySphere;
        #endregion

        #region Properties
        public Model ModelMesh
        {
            get;
            set;
        }

        public BoundingSphere ProximitySphere
        {
            get
            {
                return proximitySphere;
            }
        }
        #endregion

        #region Constructor
        public AnimatedModel(Game game)
            : base(game)
        {
            
        }
        #endregion

        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            ScalePercent = new Vector3(1);
            InitializeProximitySphere(1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateMatrixs();
            UpdateProximitySphere();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            for (int index = 0; index < ModelMesh.Meshes.Count; index++)
            {
                foreach (BasicEffect effect in ModelMesh.Meshes[index].Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = ModelMesh.Meshes[index].ParentBone.Transform * worldMatrix;

                    Services.Camera.Draw(effect);
                }

                ModelMesh.Meshes[index].Draw();
            }
        }
        #endregion

        #region Protected Methods
        protected void UpdateMatrixs()
        {
            worldTranslate = Matrix.CreateTranslation(Position);
            worldScale = Matrix.CreateScale(ScalePercent);
            worldRotate = Matrix.CreateRotationX(RotationInRadians.X);
            worldRotate *= Matrix.CreateRotationY(RotationInRadians.Y);
            worldRotate *= Matrix.CreateRotationZ(RotationInRadians.Z);
            worldMatrix = worldScale * worldRotate * worldTranslate;
        }

        protected void InitializeProximitySphere(float scale)
        {
            proximitySphere = new BoundingSphere();

            proximitySphere.Radius = 0;

            foreach (ModelMesh mesh in ModelMesh.Meshes)
            {
                proximitySphere.Radius += (mesh.BoundingSphere.Radius * scale);
            }
        }

        protected void UpdateProximitySphere()
        {
            proximitySphere.Center = Position;
        }
        #endregion
    }
}
