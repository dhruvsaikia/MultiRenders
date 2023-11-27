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
            // Initialize lighting parameters
            lightPosition = new Vector3(0, 0, 10); // Example light position
            lightColor = new Vector3(1, 1, 1); // White light
            ambientColor = new Vector3(0.1f, 0.1f, 0.1f); // Low-intensity ambient light
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState, MouseState previousMouseState)
        {
            Vector2 mouseDelta = new Vector2(currentMouseState.X - previousMouseState.X, currentMouseState.Y - previousMouseState.Y);
            lightPosition.X += mouseDelta.X * 0.01f; 
            lightPosition.Y -= mouseDelta.Y * 0.01f;
        }

        public override void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (Shader == null)
            {
                throw new InvalidOperationException("Shader is not set on DynamicSpecularLightTeapot object.");
            }

            if (Texture == null)
            {
                throw new InvalidOperationException("Texture is not set on DynamicSpecularLightTeapot object.");
            }

            Matrix worldViewProjection = WorldMatrix * viewMatrix * projectionMatrix;

            // Set the parameters for the shader
            Shader.Parameters["WorldViewProjection"].SetValue(worldViewProjection);
            Shader.Parameters["World"].SetValue(WorldMatrix);
            Shader.Parameters["View"].SetValue(viewMatrix);
            Shader.Parameters["Projection"].SetValue(projectionMatrix);
            Shader.Parameters["CameraPosition"].SetValue(new Vector3(0, 0, 3)); // Update this as needed
            Shader.Parameters["LightPosition"].SetValue(lightPosition);
            Shader.Parameters["LightColor"].SetValue(new Vector4(1, 1, 1, 1)); // Assuming light color is a Vector3
            Shader.Parameters["AmbientColor"].SetValue(new Vector4(ambientColor, 1.0f)); // Assuming ambient color is a Vector3
            Shader.Parameters["SpecularColor"].SetValue(new Vector4(0, 0, 1, 1)); // Assuming specular color is white
            Shader.Parameters["SpecularPower"].SetValue(50f); // Example specular power
            Shader.Parameters["ShaderTexture"].SetValue(Texture);

            // Apply the shader to each part of the mesh and draw it
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