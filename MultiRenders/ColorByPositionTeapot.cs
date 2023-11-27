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
        }

        protected override void SetShaderParameters(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Shader.Parameters["WorldViewProjection"].SetValue(WorldMatrix * viewMatrix * projectionMatrix);
            Shader.Parameters["teapotTexture"].SetValue(Texture);

        }
        public override void Update(GameTime gameTime, MouseState currentMouseState, MouseState previousMouseState)
        {

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
            base.Draw(graphicsDevice, viewMatrix, projectionMatrix);
        }
    }
}