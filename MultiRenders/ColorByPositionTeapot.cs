using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiRenders
{
    internal class ColorByPositionTeapot : BaseTeapot
    {
        public ColorByPositionTeapot(Model mesh, Texture2D texture, Effect shader)
            : base(mesh, texture, shader)
        {
            // Additional initialization specific to ColorByPositionTeapot
        }

        protected override void SetShaderParameters(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Shader.Parameters["WorldViewProjection"].SetValue(WorldMatrix * viewMatrix * projectionMatrix);
            Shader.Parameters["teapotTexture"].SetValue(Texture);

        }
        public override void Update(GameTime gameTime, MouseState currentMouseState, MouseState previousMouseState)
        {
            // Implement the specific update logic for Color By Position
            // For example, handling mouse input to change the position of the teapot

            // Example logic: Change teapot position based on mouse movement
            if (currentMouseState != previousMouseState)
            {
                Vector3 positionChange = new Vector3(
                    (currentMouseState.X - previousMouseState.X) * 0.01f,
                    -(currentMouseState.Y - previousMouseState.Y) * 0.01f,
                    (currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue) * 0.01f
                );
                WorldMatrix *= Matrix.CreateTranslation(positionChange);
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            // Set additional parameters for the shader if needed
            // For example, you can set the parameters related to "Color By Position" logic

            // Call the base Draw method to handle the rendering
            base.Draw(graphicsDevice, viewMatrix, projectionMatrix);
        }
    }
}