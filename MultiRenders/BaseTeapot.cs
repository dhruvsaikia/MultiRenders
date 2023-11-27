using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MultiRenders
{
    internal abstract class BaseTeapot
    {
        protected Model Mesh;
        protected Texture2D Texture;
        protected Effect Shader;
        public Matrix WorldMatrix;
        private Vector3 _cameraPosition;


        public BaseTeapot(Model mesh, Texture2D texture, Effect shader)
        {
            Mesh = mesh;
            Texture = texture;
            Shader = shader;
            WorldMatrix = Matrix.Identity;
        }
        public void SetCameraPosition(Vector3 cameraPosition)
        {
            _cameraPosition = cameraPosition;
        }


        protected abstract void SetShaderParameters(Matrix viewMatrix, Matrix projectionMatrix);

        public virtual void Update(GameTime gameTime, MouseState currentMouseState, MouseState previousMouseState)
        {
        }

        public virtual void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (Shader == null)
                throw new InvalidOperationException("Shader has not been set for the teapot.");

            if (Texture == null)
                throw new InvalidOperationException("Texture has not been set for the teapot.");

            SetShaderParameters(viewMatrix, projectionMatrix);  

            Matrix worldViewProjection = WorldMatrix * viewMatrix * projectionMatrix;

            foreach (var mesh in Mesh.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = Shader;
                    Shader.Parameters["WorldViewProjection"].SetValue(worldViewProjection);
                    Shader.Parameters["World"].SetValue(WorldMatrix);
                    Shader.Parameters["CameraPosition"].SetValue(new Vector3(0, 0, 3));
                }
                mesh.Draw();
            }
        }

    }
}
