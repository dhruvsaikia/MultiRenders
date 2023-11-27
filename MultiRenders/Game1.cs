﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static MultiRenders.Models;

namespace MultiRenders
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BaseTeapot _currentTeapot; // Base class for teapot
        private ColorByPositionTeapot _colorByPositionTeapot;
        private DynamicSpecularLightTeapot _dynamicSpecularLightTeapot;
        private MoveCubeToSphereTeapot _moveCubeToSphereTeapot;
        private SpriteFont _font;
        private MouseState _previousMouseState;
        private bool _isFirstUpdate = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 600;
        }
        private enum RenderMode
        {
            ColorByPosition,
            DynamicSpecularLighting,
            MoveCubeToSphere // Assume you have this mode as well
        }

        private RenderMode _currentRenderMode;

        protected override void Initialize()
        {
            ToolWindow toolWindow = new ToolWindow();
            toolWindow.Show();
            toolWindow.ModeChanged += ToolWindow_ModeChanged;

            base.Initialize();
        }

        private void ToolWindow_ModeChanged(object sender, EventArgs e)
        {
            ToolWindow toolWindow = sender as ToolWindow;

            if (toolWindow.RadioButtonColorByPositionChecked)
            {
                _currentRenderMode = RenderMode.ColorByPosition;
                _currentTeapot = _colorByPositionTeapot;
            }
            else if (toolWindow.RadioButtonDynamicLightSpecularChecked)
            {
                _currentRenderMode = RenderMode.DynamicSpecularLighting;
                _currentTeapot = _dynamicSpecularLightTeapot;
            }
            _currentTeapot.WorldMatrix = Matrix.Identity;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Arial12");

            Model teapotModel = Content.Load<Model>("Teapot");
            Texture2D teapotTexture = Content.Load<Texture2D>("Smiley2");
            Texture2D metalTexture = Content.Load<Texture2D>("Metal");
            Effect shaderPart1 = Content.Load<Effect>("MyShader"); 
            Effect shaderPart2 = Content.Load<Effect>("MyShader2");
            _colorByPositionTeapot = new ColorByPositionTeapot(teapotModel, teapotTexture, shaderPart1);
            _dynamicSpecularLightTeapot = new DynamicSpecularLightTeapot(teapotModel, metalTexture, shaderPart2);
            _currentTeapot = _colorByPositionTeapot;

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

            if (_currentRenderMode == RenderMode.DynamicSpecularLighting)
            {
                // Pass mouse state to update light position
                _dynamicSpecularLightTeapot.UpdateLightPosition(currentMouseState, _previousMouseState);
            }

            // Update the teapot normally, without light position logic
            _currentTeapot.Update(gameTime, currentMouseState, _previousMouseState);

            _previousMouseState = currentMouseState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Vector3 cameraPosition = new Vector3(0, 0, 1);
            Vector3 cameraTarget = Vector3.Zero;
            Matrix view = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), _graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 100f);

            _currentTeapot.Draw(GraphicsDevice, view, projection);

            Vector3 teapotPosition = _currentTeapot.WorldMatrix.Translation;
            string positionText = $"Teapot Position: X:{teapotPosition.X:F2}, Y:{teapotPosition.Y:F2}, Z:{teapotPosition.Z:F2}";

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, positionText, new Vector2(10, 10), Color.White);
            _spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;                 
            GraphicsDevice.BlendState = BlendState.Opaque;                 
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            base.Draw(gameTime);
        }
    }
}
