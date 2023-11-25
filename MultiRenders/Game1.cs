using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;

namespace MultiRenders
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Model _teapot;
        private Effect _shader;
        private Vector3 _teapotPosition;
        private Matrix _world;
        private Matrix _view;
        private Matrix _projection;
        private MouseState _previousMouseState;
        private SpriteFont _font;
        private Texture2D teapotTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            _teapotPosition = new Vector3(10, 0, 3); // Adjust the Z value as needed
            _world = Matrix.CreateTranslation(_teapotPosition);
            _view = Matrix.CreateLookAt(new Vector3(10, 0, 5), _teapotPosition, Vector3.Up);
            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), _graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            _previousMouseState = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _teapot = Content.Load<Model>("Teapot"); 
            _shader = Content.Load<Effect>("MyShader");
            _font = Content.Load<SpriteFont>("Arial12");
            teapotTexture = Content.Load<Texture2D>("Smiley2");
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState != _previousMouseState)
            {
                _teapotPosition.X += (currentMouseState.X - _previousMouseState.X) * 0.01f;
                _teapotPosition.Y -= (currentMouseState.Y - _previousMouseState.Y) * 0.01f;
                _teapotPosition.Z -= (currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue) * 0.01f;
                _previousMouseState = currentMouseState;
            }
            _world = Matrix.CreateTranslation(_teapotPosition);

            if (_graphics.PreferredBackBufferWidth != GraphicsDevice.Viewport.Width ||
                _graphics.PreferredBackBufferHeight != GraphicsDevice.Viewport.Height)
            {
                _graphics.PreferredBackBufferWidth = GraphicsDevice.Viewport.Width;
                _graphics.PreferredBackBufferHeight = GraphicsDevice.Viewport.Height;
                _projection = Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45),
                    _graphics.GraphicsDevice.Viewport.AspectRatio,
                    0.1f,
                    100f
                );
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix worldViewProjection = _world * _view * _projection;

            _shader.CurrentTechnique = _shader.Techniques["BasicColorDrawing"];
            _shader.Parameters["WorldViewProjection"].SetValue(worldViewProjection);
            _shader.Parameters["World"].SetValue(_world);
            _shader.Parameters["ViewProjection"].SetValue(_view * _projection);
            _shader.Parameters["teapotTexture"].SetValue(teapotTexture);

            // Apply the custom effect to each part of the teapot model
            foreach (var mesh in _teapot.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = _shader;
                }
            }

            // Draw the teapot model
            _teapot.Draw(_world, _view, _projection);

            // Draw the teapot position
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, $"Teapot Position: {_teapotPosition}", new Vector2(10, 10), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
