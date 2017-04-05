//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.WorldObjects
//{
//    class ImageObject : WOImage
//    {
//        public Texture2D Image
//        {
//            get{ return base.Texture; }
//            set{ base.Texture= value; }
//        }

//        public ImageObject()
//        {
//            if(Editor.Images.Count > 0)
//                this.Image = Editor.Images[0];
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            Matrix Transform = Matrix.CreateScale(1f, 1f, 1f) * Matrix.CreateRotationZ(0f) * Matrix.CreateTranslation(new Vector3(0f, 0f, 0f));
//            spriteBatch.End();
//            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Transform);
//            base.Draw(spriteBatch);
//            spriteBatch.End();
//			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Transform);
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            int imageIndex = Editor.Images.IndexOf(this.Image);
//            string imageName = Editor.ImageNames[imageIndex];
//            writer.Write(imageName);
//        }
//        public override void Load(float saveVersion, ref System.IO.BinaryReader reader)
//        {
//            base.Load(saveVersion, ref reader);
//            if (saveVersion > 1)
//            {
//                string imgName = reader.ReadString();
//                for (int i = 0; i < Editor.ImageNames.Count; i++)
//                {
//                    if (imgName == Editor.ImageNames[i])
//                    {
//                        this.Image = Editor.Images[i];
//                    }
//                }
//            }
//        }
//    }
//}
