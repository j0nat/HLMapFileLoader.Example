using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HLMapFileLoader.Example
{
    class Camera : GameComponent
    {
        private Vector3 cameraLookAt;
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Matrix Projection { get; protected set; }
        public Matrix View { get { return Matrix.CreateLookAt(Position, cameraLookAt, Vector3.Up); } }

        public Camera(Game game) : base(game)
        {
            this.cameraLookAt = Vector3.Zero;
            this.Position = Vector3.Zero;
            this.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, Game.GraphicsDevice.Viewport.AspectRatio,
                0.05f, 10000.0f);

            SetPosition(new Vector3(-331.68f, 103.5f, 214.47f));
            SetRotation(new Vector3(0.39f, 2.1f, 0));
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            Vector3 moveVector = Vector3.Zero;

            if (ks.IsKeyDown(Keys.W))
                moveVector.Z = 1;

            if (ks.IsKeyDown(Keys.S))
                moveVector.Z = -1;

            if (ks.IsKeyDown(Keys.A))
                moveVector.X = 1;

            if (ks.IsKeyDown(Keys.D))
                moveVector.X = -1;

            if (ks.IsKeyDown(Keys.Space))
                moveVector.Y = 1;

            if (ks.IsKeyDown(Keys.LeftShift))
                moveVector.Y = -1;

            if (ks.IsKeyDown(Keys.Up))
                SetRotation(new Vector3(Rotation.X - 0.05f, Rotation.Y, Rotation.Z));

            if (ks.IsKeyDown(Keys.Down))
                SetRotation(new Vector3(Rotation.X + 0.05f, Rotation.Y, Rotation.Z));

            if (ks.IsKeyDown(Keys.Right))
                SetRotation(new Vector3(Rotation.X, Rotation.Y - 0.05f, Rotation.Z));

            if (ks.IsKeyDown(Keys.Left))
                SetRotation(new Vector3(Rotation.X, Rotation.Y + 0.05f, Rotation.Z));

            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();

                moveVector *= (float)gameTime.ElapsedGameTime.TotalSeconds * 225f;

                Vector3 location = PreviewMove(moveVector);

                Move(location);
            }

            base.Update(gameTime);
        }

        private void UpdateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y);
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);

            cameraLookAt = Position + lookAtOffset;
        }

        public Vector3 PreviewMove(Vector3 movement)
        {
            Matrix rotate = Matrix.CreateRotationY(Rotation.Y);

            movement = Vector3.Transform(movement, rotate);

            return Position + movement;
        }

        public void Move(Vector3 position)
        {
            Position = position;

            UpdateLookAt();
        }

        public void SetPosition(Vector3 position)
        {
            Vector3 cameraView = PreviewMove(position);

            Move(cameraView);
        }

        public void SetRotation(Vector3 rotation)
        {
            this.Rotation = rotation;

            UpdateLookAt();
        }
    }
}