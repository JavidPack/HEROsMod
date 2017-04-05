//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.WorldObjects
//{
//    class WOImage : WorldObject
//    {
//        [HideInInspector]
//        public Texture2D Texture { get; set; }

//        public WOImage(Texture2D texture)
//        {
//            this.Texture = texture;
//            this.Anchor = UIKit.AnchorPosition.Center;
//        }

//        public WOImage()
//        {
//            this.Texture = null;
//            this.Anchor = UIKit.AnchorPosition.Center;
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            if (!this.Visible)
//                return;
//            if (this.Texture == null)
//                return;
//            Vector2 drawPosition = this.Position;
//            if(!this.ScreenRelative)
//            {
//                drawPosition -= Main.screenPosition;
//            }
//            Color drawColor = Color.White;
//            if(EffectedByWorldLighting)
//            {
//                drawColor = Lighting.GetColor((int)this.X / 16, (int)this.Y / 16);
//            }
//            spriteBatch.Draw(Texture, drawPosition, null, drawColor, 0f, Origin / Scale, this.Scale, SpriteEffects.None, 0);
//            base.Draw(spriteBatch);
//        }

//        public override float GetWidth()
//        {
//            //if (Texture == null)
//               // return 0;
//            return Texture.Width * Scale;
//        }

//        public override float GetHeight()
//        {
//            return Texture.Height * Scale;
//        }
//    }
//}
