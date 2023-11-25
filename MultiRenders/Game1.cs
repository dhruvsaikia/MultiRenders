using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultiRenders
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Models _teapotModel; 
        private Effect _shader;
        private MouseState _previousMouseState;
        private SpriteFont _font;
        private bool _isFirstUpdate = true;

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
            _shader = Content.Load<Effect>("MyShader");
            _font = Content.Load<SpriteFont>("Arial12");

            _teapotModel = new Models(teapot, teapotTexture, Vector3.Zero, 1); 
            _teapotModel.SetShader(_shader);

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

                _teapotModel.SetPosition(_teapotModel.Translation.Translation + positionChange);
                _previousMouseState = currentMouseState;
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

            _teapotModel.Render(view, projection, cameraPosition);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, $"Teapot Position: {_teapotModel.Translation.Translation}", new Vector2(10, 10), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
