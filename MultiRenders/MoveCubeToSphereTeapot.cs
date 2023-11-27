using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MultiRenders
{
    internal class MoveCubeToSphereTeapot : BaseTeapot
    {
        private Model _cubeModel;
        private Model _sphereModel;
        private Texture2D _cubeTexture;
        private Texture2D _sphereTexture;
        private Effect _shader;
        private List<Vector3> _cubePositions;
        private Vector3 _cameraPosition;

        public MoveCubeToSphereTeapot(Model cubeModel, Model sphereModel, Texture2D cubeTexture, Texture2D sphereTexture, Effect shader)
         : base(null, cubeTexture, shader)
        {
            _cubeModel = cubeModel;
            _sphereModel = sphereModel;
            _cubeTexture = cubeTexture;
            _sphereTexture = sphereTexture;
            _shader = shader;
            _cubePositions = new List<Vector3>();
            _cameraPosition = new Vector3(0, 0, 1); 
        }

        public void SetCameraPosition(Vector3 cameraPosition)
        {
            _cameraPosition = cameraPosition;
        }
        protected override void SetShaderParameters(Matrix viewMatrix, Matrix projectionMatrix)
        {
            Matrix worldViewProjection = WorldMatrix * viewMatrix * projectionMatrix;
            _shader.Parameters["WorldViewProjection"].SetValue(worldViewProjection);
            _shader.Parameters["World"].SetValue(WorldMatrix);
            _shader.Parameters["LightPosition"].SetValue(new Vector4(0, 0, 1, 1)); 
            _shader.Parameters["CameraPosition"].SetValue(new Vector4(_cameraPosition, 1));
            _shader.Parameters["ObjTexture"].SetValue(_cubeTexture);
        }

        public override void Update(GameTime gameTime, MouseState currentMouseState, MouseState previousMouseState)
        {
            if (currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                _cubePositions.Add(new Vector3());
            }

            Vector3 sphereCenter = Vector3.Zero; 
            float moveSpeed = 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds; 

            for (int i = _cubePositions.Count - 1; i >= 0; i--)
            {
                Vector3 direction = sphereCenter - _cubePositions[i];
                if (direction.Length() < moveSpeed)
                {
                    _cubePositions.RemoveAt(i);
                }
                else
                {
                    direction.Normalize();
                    _cubePositions[i] += direction * moveSpeed;
                }
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            SetShaderParameters(viewMatrix, projectionMatrix);

            // Draw the sphere
            foreach (var mesh in _sphereModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Texture = _sphereTexture;
                    effect.TextureEnabled = true;
                    effect.EnableDefaultLighting(); 
                    effect.World = WorldMatrix;
                    effect.View = viewMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }

            foreach (var cubePosition in _cubePositions)
            {
                foreach (var mesh in _cubeModel.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.Texture = _cubeTexture;
                        effect.TextureEnabled = true;
                        effect.EnableDefaultLighting(); 
                        effect.World = Matrix.CreateTranslation(cubePosition);
                        effect.View = viewMatrix;
                        effect.Projection = projectionMatrix;
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
