//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace GameikiMod.GameikiVideo
//{
//    class TextureExtruder
//    {
//        enum Edge
//        {
//            Left,
//            Right,
//            Top,
//            Bottom
//        }

//        public Matrix WorldView { get; set; }
//        public Matrix ProjectionView { get; set; }
//        private List<VertexPositionColor> _edgeVertices;
//        private VertexPositionTexture[] _capVertices;
//        private Effect _effect;
//        GraphicsDevice _graphicsDevice;

//        public int VerticeCount { get { return _edgeVertices.Count; } }

//        public TextureExtruder(Matrix projection, Effect effect, GraphicsDevice graphicsDevice)
//        {
//            WorldView = Matrix.Identity;
//            ProjectionView = projection;
//            _edgeVertices = new List<VertexPositionColor>();
//            _capVertices = new VertexPositionTexture[6];
//            this._effect = effect;
//            _graphicsDevice = graphicsDevice;
//        }

//        public void DrawTexture(Texture2D texture, float depth, int pixelsPerVoxel)
//        {
//            DrawEdges(texture, depth, pixelsPerVoxel);
//            DrawCap(texture, depth);
//        }

//        public void DrawTexture(Texture2D texture, float depth, int pixelsPerVoxel, RenderTarget2D renderTarget, SpriteBatch spriteBatch)
//        {
//            DrawEdges(texture, depth, pixelsPerVoxel, renderTarget, spriteBatch);
//            DrawCap(texture, depth);
//        }

//        void DrawEdges(Texture2D texture, float depth, int pixelsPerVoxel)
//        {
//            _edgeVertices.Clear();

//            Color[] colorData = new Color[texture.Width * texture.Height];
//            texture.GetData(colorData);
//            for (int y = 0; y < texture.Height; y += pixelsPerVoxel)
//            {
//                for (int x = 0; x < texture.Width; x += pixelsPerVoxel)
//                {
//                    Color color = colorData[x + y * texture.Width];
//                    if (color.A > 0)
//                    {
//                        List<Edge> exposedEdges = GetExposedEdges(colorData, x, y, texture, pixelsPerVoxel);

//                        foreach (Edge edge in exposedEdges)
//                        {
//                            switch (edge)
//                            {
//                                case Edge.Left:
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, 0), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, depth), color));

//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, 0), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, 0), color));
//                                    break;
//                                case Edge.Right:
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, 0), color));

//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, 0), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, 0), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, depth), color));
//                                    break;
//                                case Edge.Top:
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, 0), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, 0), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, depth), color));

//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, 0), color));
//                                    break;

//                                case Edge.Bottom:
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, 0), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, 0), color));

//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, depth), color));
//                                    _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, 0), color));
//                                    break;
//                            }
//                        }
//                    }
//                }
//            }

//            _graphicsDevice.BlendState = BlendState.AlphaBlend;
//            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

//            RasterizerState rs = new RasterizerState();
//            _graphicsDevice.RasterizerState = rs;

//            _effect.CurrentTechnique = _effect.Techniques["ColoredNoShading"];
//            _effect.Parameters["xView"].SetValue(Matrix.Identity);
//            _effect.Parameters["xProjection"].SetValue(ProjectionView);
//            _effect.Parameters["xWorld"].SetValue(WorldView);

//            if (_edgeVertices.Count > 0)
//            {
//                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
//                {
//                    pass.Apply();
//                    _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _edgeVertices.ToArray(), 0, _edgeVertices.Count / 3, VertexPositionColor.VertexDeclaration);
//                }
//            }
//        }

//        void DrawEdges(Texture2D texture, float depth, int pixelsPerVoxel, RenderTarget2D renderTarget, SpriteBatch spriteBatch)
//        {

//            _graphicsDevice.SetRenderTarget(renderTarget);
//            _graphicsDevice.Clear(Color.Transparent);

//            spriteBatch.End();

//            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
//            _effect.CurrentTechnique = _effect.Techniques["Test"];
//            _effect.Parameters["TextureWidth"].SetValue(texture.Width);
//            _effect.Parameters["TextureHeight"].SetValue(texture.Height);
//            _effect.Parameters["PixelsPerVoxel"].SetValue(pixelsPerVoxel);
//            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
//            {
//                pass.Apply();
//                spriteBatch.Draw(texture, Vector2.Zero, Color.White);
//            }
//            spriteBatch.End();

//            spriteBatch.Begin();

//            _graphicsDevice.SetRenderTarget(null);
//            Color[] edgeData = new Color[renderTarget.Width * renderTarget.Height];
//            renderTarget.GetData(edgeData);

//            _edgeVertices.Clear();

//            Color[] colorData = new Color[texture.Width * texture.Height];
//            texture.GetData(colorData);


//            for (int y = 0; y < texture.Height; y += pixelsPerVoxel)
//            {
//                for (int x = 0; x < texture.Width; x += pixelsPerVoxel)
//                {
//                    Color color = colorData[x + y * texture.Width];
//                    if (color.A > 0)
//                    {
//                        Color edge = edgeData[x + y * texture.Width];
//                        if (edge.R > 0)
//                        {
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, 0), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, depth), color));

//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, 0), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, 0), color));
//                        }
//                        if (edge.G > 0)
//                        {
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, 0), color));

//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, 0), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, 0), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, depth), color));
//                        }
//                        if (edge.B > 0)
//                        {
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, 0), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, 0), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, depth), color));

//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y, 0), color));
//                        }
//                        if (edge.A > 0)
//                        {
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, 0), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, 0), color));

//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x + pixelsPerVoxel, y + pixelsPerVoxel, depth), color));
//                            _edgeVertices.Add(new VertexPositionColor(new Vector3(x, y + pixelsPerVoxel, 0), color));
//                        }
//                    }
//                }
//            }

//            _graphicsDevice.BlendState = BlendState.AlphaBlend;
//            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

//            _effect.CurrentTechnique = _effect.Techniques["ColoredNoShading"];
//            _effect.Parameters["xView"].SetValue(Matrix.Identity);
//            _effect.Parameters["xProjection"].SetValue(ProjectionView);
//            _effect.Parameters["xWorld"].SetValue(WorldView);

//            if (_edgeVertices.Count > 0)
//            {
//                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
//                {
//                    pass.Apply();
//                    _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _edgeVertices.ToArray(), 0, _edgeVertices.Count / 3, VertexPositionColor.VertexDeclaration);
//                }
//            }
//        }


//        List<Edge> GetExposedEdges(Color[] colorData, int x, int y, Texture2D texture, int pixelsPerVoxel)
//        {
//            List<Edge> ExposedEdges = new List<Edge>();
//            if (x == 0)
//            {
//                ExposedEdges.Add(Edge.Left);
//            }
//            else
//            {
//                if (colorData[x - pixelsPerVoxel + y * texture.Width].A == 0)
//                    ExposedEdges.Add(Edge.Left);
//            }

//            if (y == 0)
//            {
//                ExposedEdges.Add(Edge.Top);
//            }
//            else
//            {
//                if (colorData[x + (y - pixelsPerVoxel) * texture.Width].A == 0)
//                    ExposedEdges.Add(Edge.Top);
//            }
//            if (x + pixelsPerVoxel == texture.Width)
//            {
//                ExposedEdges.Add(Edge.Right);
//            }
//            else
//            {
//                if (colorData[x + pixelsPerVoxel + y * texture.Width].A == 0)
//                    ExposedEdges.Add(Edge.Right);

//            }
//            if (y + pixelsPerVoxel == texture.Height)
//            {
//                ExposedEdges.Add(Edge.Bottom);
//            }
//            else
//            {
//                if (colorData[x + (y + pixelsPerVoxel) * texture.Width].A == 0)
//                    ExposedEdges.Add(Edge.Bottom);
//            }

//            return ExposedEdges;
//        }


//        void DrawCap(Texture2D texture, float depth)
//        {
//            _capVertices[0] = new VertexPositionTexture(new Vector3(0, 0, depth), new Vector2(0, 0));
//            _capVertices[1] = new VertexPositionTexture(new Vector3(texture.Width, 0, depth), new Vector2(1, 0));
//            _capVertices[2] = new VertexPositionTexture(new Vector3(texture.Width, texture.Height, depth), new Vector2(1, 1));

//            _capVertices[3] = new VertexPositionTexture(new Vector3(0, 0, depth), new Vector2(0, 0));
//            _capVertices[4] = new VertexPositionTexture(new Vector3(texture.Width, texture.Height, depth), new Vector2(1, 1));
//            _capVertices[5] = new VertexPositionTexture(new Vector3(0, texture.Height, depth), new Vector2(0, 1));


//            _effect.CurrentTechnique = _effect.Techniques["TexturedNoShading"];
//            _effect.Parameters["xWorld"].SetValue(WorldView);
//            _effect.Parameters["xView"].SetValue(Matrix.Identity);
//            _effect.Parameters["xProjection"].SetValue(ProjectionView);
//            _effect.Parameters["xTexture"].SetValue(texture);

//            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
//            {
//                pass.Apply();
//                _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _capVertices, 0, _capVertices.Length / 3, VertexPositionTexture.VertexDeclaration);
//            }

//        }
//    }
//}
