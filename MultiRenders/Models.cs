using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MultiRenders
{
    internal class Models
    {
        // Properties for Part 1
        public Model Mesh { get; set; }
        public Matrix Translation { get; set; }
        public Matrix Rotation { get; set; }
        public Matrix Scale { get; set; }
        public Effect ShaderPart1 { get; set; } // Shader for Part 1
        public Effect ShaderPart2 { get; set; } // Shader for Part 2
        public Texture2D Texture { get; set; }
        public Vector3 DiffuseColor { get; set; }
        public float SpecularPower { get; set; }
        public Vector3 LightPosition { get; set; }
        public Vector3 LightDirection { get; set; }

        // Constructor

        public enum RenderMode
        {
            ColorByPosition,
            DynamicSpecularLighting,
        }
        public Models(Model _mesh, Texture2D _texture, Vector3 _position, float _scale)
        {
            Mesh = _mesh;
            Texture = _texture;
            Translation = Matrix.CreateTranslation(_position);
            Rotation = Matrix.Identity;
            Scale = Matrix.CreateScale(_scale);
            DiffuseColor = new Vector3(1, 1, 1);
            SpecularPower = 4.0f;
            LightPosition = new Vector3(0, 40, 50);
            LightDirection = new Vector3(0, 2, -2);
        }

        // Helper method to get the world transformation
        public Matrix GetTransform()
        {
            return Scale * Rotation * Translation;
        }

        // Method to set the shader for Part 1
        public void SetShaderPart1(Effect _effect)
        {
            ShaderPart1 = _effect;
        }

        // Method to set the shader for Part 2
        public void SetShaderPart2(Effect _effect)
        {
            ShaderPart2 = _effect;
        }

        // Method to move the light
        public void MoveLight(Vector3 move)
        {
            LightPosition = LightPosition + move;
        }

        // Method to set the position of the model
        public void SetPosition(Vector3 newPosition)
        {
            Translation = Matrix.CreateTranslation(newPosition);
        }

        // Method to set the light direction
        public void SetLightDirection(Vector3 newDirection)
        {
            LightDirection = newDirection;
        }

        // Render method with mode check
        public void Render(Matrix _view, Matrix _projection, Vector3 _cameraPosition, RenderMode currentMode)
        {
            Effect currentShader = currentMode == RenderMode.ColorByPosition ? ShaderPart1 : ShaderPart2;

            currentShader.Parameters["World"].SetValue(GetTransform());
            currentShader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
            currentShader.Parameters["CameraPosition"].SetValue(_cameraPosition);

            if (currentMode == RenderMode.ColorByPosition)
            {
                // Set the parameters for Part 1
                currentShader.Parameters["teapotTexture"].SetValue(Texture);
                currentShader.Parameters["DiffuseColor"].SetValue(DiffuseColor);
                currentShader.Parameters["LightDirection"].SetValue(LightDirection);
            }
            else if (currentMode == RenderMode.DynamicSpecularLighting)
            {
                // Set the parameters for Part 2
                currentShader.Parameters["LightPosition"].SetValue(LightPosition);
                currentShader.Parameters["LightColor"].SetValue(new Vector4(1, 1, 1, 1)); // Assuming white light for simplicity
                currentShader.Parameters["AmbientColor"].SetValue(new Vector4(0.1f, 0.1f, 0.1f, 1)); // Example ambient light
                currentShader.Parameters["SpecularColor"].SetValue(new Vector4(0, 0, 1, 1)); // Blue specular highlights
                currentShader.Parameters["SpecularPower"].SetValue(SpecularPower);
                currentShader.Parameters["ShaderTexture"].SetValue(Texture); // Use the appropriate texture for Part 2
            }

            // Apply the effect based on the current mode and draw the model
            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = currentShader;
                }
                mesh.Draw();
            }
        }
    }
}
