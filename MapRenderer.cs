using HLMapFileLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HLMapFileLoader.Example
{
    public class Game1 : Game
    {
        private List<Mesh> meshes;
        private Camera camera;
        private BasicEffect effect;
        private SpriteFont arial12Font;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            meshes = new List<Mesh>();

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            effect = new BasicEffect(GraphicsDevice);

            camera = new Camera(this);
            Components.Add(camera);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            arial12Font = Content.Load<SpriteFont>("Arial12Font");

            List<Brush> brushes = Map.Load(@"Content\game.map", Content);
            foreach (Brush brush in brushes)
            {
                meshes.Add(new Mesh(GraphicsDevice, brush));
            }
        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (Mesh mesh in meshes)
            {
                mesh.Draw(effect, camera);
            }

            spriteBatch.Begin();

            spriteBatch.DrawString(arial12Font, "Move: W, A, S, D\nUpwards:SPACE\nDownwards:SHIFT\nRotate: UP, DOWN, LEFT, RIGHT", new Vector2(5, 5), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
