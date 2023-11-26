using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static MultiRenders.Models;

namespace MultiRenders
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Models _teapotModel;
        private Models _teapotModelPart2; // Model for Part 2
        private Effect _shaderPart2; // Shader for Part 2
        private MouseState _previousMouseState;
        private SpriteFont _font;
        private bool _isFirstUpdate = true;
        private RenderMode _currentRenderMode = RenderMode.ColorByPosition; // Initial mode is Part 1

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 600;
        }

        protected override void Initialize()
        {
            ToolWindow toolWindow = new ToolWindow();
            toolWindow.Show();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Model teapot = Content.Load<Model>("Teapot");
            Texture2D teapotTexture = Content.Load<Texture2D>("Smiley2");
            Effect shaderPart1 = Content.Load<Effect>("MyShader"); // Shader for Part 1
            _shaderPart2 = Content.Load<Effect>("MyShader2"); // Shader for Part 2
            _font = Content.Load<SpriteFont>("Arial12");

            _teapotModel = new Models(teapot, teapotTexture, Vector3.Zero, 1);
            _teapotModel.SetShaderPart1(shaderPart1);

            // Create the Teapot model for Part 2
            _teapotModelPart2 = new Models(teapot, teapotTexture, new Vector3(2, 0, 0), 1); // Adjust the position as needed
            _teapotModelPart2.SetShaderPart2(_shaderPart2);

            _previousMouseState = Mouse.GetState();
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();

            if (_isFirstUpdate)
            {
                _previousMouseState = currentMouseState;
                _isFirstUpdate = false;
                return;
            }

            if (currentMouseState != _previousMouseState)
            {
                Vector3 positionChange = new Vector3(
                    (currentMouseState.X - _previousMouseState.X) * 0.01f,
                    -(currentMouseState.Y - _previousMouseState.Y) * 0.01f,
                    (currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue) * 0.01f
                );

                if (_currentRenderMode == RenderMode.ColorByPosition)
                {
                    _teapotModel.SetPosition(_teapotModel.Translation.Translation + positionChange);
                }
                else if (_currentRenderMode == RenderMode.DynamicSpecularLighting)
                {
                    _teapotModelPart2.SetPosition(_teapotModelPart2.Translation.Translation + positionChange);
                }

                _previousMouseState = currentMouseState;
            }

            // Check for mode switch (you can use any logic to switch modes)
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                _currentRenderMode = RenderMode.ColorByPosition;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                _currentRenderMode = RenderMode.DynamicSpecularLighting;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Vector3 cameraPosition = new Vector3(0, 0, 3);
            Vector3 cameraTarget = Vector3.Zero;
            Matrix view = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), _graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            if (_currentRenderMode == RenderMode.ColorByPosition)
            {
                _teapotModel.Render(view, projection, cameraPosition, _currentRenderMode);
            }
            else if (_currentRenderMode == RenderMode.DynamicSpecularLighting)
            {
                _teapotModelPart2.Render(view, projection, cameraPosition, _currentRenderMode);
            }

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, $"Teapot Position: {_currentRenderMode}", new Vector2(10, 10), Color.White); // Display the current mode
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
