using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SharpDX.MediaFoundation;

namespace MultiRenders
{
    internal class Models
    {
        public Model Mesh { get; set; }
        public Matrix Translation { get; set; }
        public Matrix Rotation { get; set; }
        public Matrix Scale { get; set; }
        public Effect Shader { get; set; }

        public Texture2D Texture { get; set; }

        public Vector3 DiffuseColor { get; set; }
        public float SpecularPower { get; set; }

        public Vector3 LightPosition { get; set; }
        public Vector3 LightDirection { get; set; }

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

        public Matrix GetTransform()
        {
            return Scale * Rotation * Translation;
        }

        public void SetShader(Effect _effect)
        {
            Shader = _effect;
            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = Shader;
                }
            }
        }
        public void MoveLight(Vector3 move)
        {
            LightPosition = new Vector3(
                LightPosition.X + move.X,
                LightPosition.Y + move.Y,
                LightPosition.Z + move.Z
                );
        }
        public void SetPosition(Vector3 newPosition)
        {
            
            Translation = Matrix.CreateTranslation(newPosition);
        }
        public void SetLightDirection(Vector3 newDirection)
        {
           
            LightDirection = newDirection;
        }

        public void Render(Matrix _view, Matrix _projection, Vector3 _cameraPosition)
        {
            Shader.Parameters["World"].SetValue(GetTransform());
            Shader.Parameters["WorldViewProjection"].SetValue(GetTransform() * _view * _projection);
            Shader.Parameters["teapotTexture"].SetValue(Texture);
            Shader.Parameters["CameraPosition"].SetValue(_cameraPosition);
            Shader.Parameters["DiffuseColor"].SetValue(DiffuseColor);
            Shader.Parameters["LightDirection"].SetValue(LightDirection);

            foreach (ModelMesh mesh in Mesh.Meshes)
            {
                mesh.Draw();
            }
        }
    }
}
