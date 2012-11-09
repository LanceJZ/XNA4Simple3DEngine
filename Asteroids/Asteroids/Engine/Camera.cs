#region Using
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Asteroids.Engine
{
    public class Camera : PositionedObject
    {
        #region Fields
        private Matrix cameraRotation;
        #endregion

        #region Properties
        public Matrix View
        {
            get;
            set;
        }

        public Matrix Projection
        {
            get;
            protected set;
        }

        public Vector3 Target
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public Camera(Game game, Vector3 position, Vector3 target, Vector3 rotation, bool Orthographic, float near, float far)
            : base(game)
        {
            Position = position;
            RotationInRadians = rotation;
            Target = target;

            if (Orthographic)
            {
                Projection = Matrix.CreateOrthographic(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height,
                    near, far);
            }
            else
            {
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                    (float)Game.Window.ClientBounds.Width / (float)Game.Window.ClientBounds.Height, near, far);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            cameraRotation = Matrix.Identity;
        }

        /// <summary>
        /// Allows the game component to update itself via the GameComponent.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // This rotates the free camera.
            cameraRotation = Matrix.CreateFromAxisAngle(cameraRotation.Forward, RotationInRadians.Z)
                * Matrix.CreateFromAxisAngle(cameraRotation.Right, RotationInRadians.X)
                * Matrix.CreateFromAxisAngle(cameraRotation.Up, RotationInRadians.Y);
            // Make sure the camera is always pointing forward.
            Target = Position + cameraRotation.Forward;
            View = Matrix.CreateLookAt(Position, Target, cameraRotation.Up);
        }

        public void Draw(BasicEffect effect)
        {
            effect.View = View;
            effect.Projection = Projection;
        }
        #endregion
    }
}
