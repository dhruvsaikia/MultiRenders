using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiRenders.Models;

namespace MultiRenders
{
    internal class DynamicSpecularLightTeapot : BaseTeapot
    {
        private Vector3 lightPosition;
        private Vector3 lightColor;
        private Vector3 ambientColor;

        public void ResetLightPosition()
        {
            lightPosition = new Vector3(0, 0, 1);
        }

        public Vector3 GetLightPosition()
        {
            return lightPosition;
        }

        protected override void SetShaderParameters(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Shader.Parameters["WorldViewProjection"].SetValue(WorldMatrix * viewMatrix * projectionMatrix);
            Shader.Parameters["ShaderTexture"].SetValue(Texture);

        }
        public void UpdateLightPosition(MouseState currentMouseState, MouseState previousMouseState)
        {
            Vector2 mouseDelta = new Vector2(currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
            lightPosition.X += mouseDelta.X * 0.01f; 
            lightPosition.Y -= mouseDelta.Y * 0.01f; 
                                                     
        }

        public DynamicSpecularLightTeapot(Model mesh, Texture2D texture, Effect shader)
            : base(mesh, texture, shader)
        {

            lightPosition = new Vector3(0, 0, 10); 
            lightColor = new Vector3(1, 1, 1); 
            ambientColor = new Vector3(0.1f, 0.1f, 0.1f); 
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState, MouseState previousMouseState)
        {
            Vector2 mouseDelta = new Vector2(currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
            lightPosition.X += mouseDelta.X * 0.01f; 
            lightPosition.Y -= mouseDelta.Y * 0.01f;
        }

        public override void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {

            Matrix worldViewProjection = WorldMatrix * viewMatrix * projectionMatrix;


            Shader.Parameters["WorldViewProjection"].SetValue(worldViewProjection);
            Shader.Parameters["World"].SetValue(WorldMatrix);
            Shader.Parameters["View"].SetValue(viewMatrix);
            Shader.Parameters["Projection"].SetValue(projectionMatrix);
            Shader.Parameters["CameraPosition"].SetValue(new Vector3(0, 0, 3)); 
            Shader.Parameters["LightPosition"].SetValue(lightPosition);
            Shader.Parameters["LightColor"].SetValue(new Vector4(1, 1, 1, 1));
            Shader.Parameters["AmbientColor"].SetValue(new Vector4(ambientColor, 1.0f)); 
            Shader.Parameters["SpecularColor"].SetValue(new Vector4(0, 0, 1, 1)); 
            Shader.Parameters["SpecularPower"].SetValue(50f); 
            Shader.Parameters["ShaderTexture"].SetValue(Texture);

            foreach (var mesh in Mesh.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = Shader;
                }
                mesh.Draw();
            }

            base.Draw(graphicsDevice, viewMatrix, projectionMatrix);
        }
    }
}