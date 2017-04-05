//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.WorldObjects
//{
//    class ItemViewer : WorldObject
//    {
//        private int _itemType;
//        private int _itemNumber;
//        public int ItemNumber
//        {
//            get { return _itemNumber; }
//            set
//            {
//                if(value >= Main.itemTexture.Length)
//                {
//                    value = Main.itemTexture.Length - 1;
//                }
//                _itemNumber = value;
//                Item item = new Item();
//                item.netDefaults(value);
//                this._itemType = item.type;
//            }
//        }
//        public ItemViewer()
//        {
//            ItemNumber = 1;
//            this.Anchor = UIKit.AnchorPosition.Center;
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            writer.Write(this.ItemNumber);
//        }

//        public override void Load(float saveVersion, ref System.IO.BinaryReader reader)
//        {
//            base.Load(saveVersion, ref reader);
//            this.ItemNumber = reader.ReadInt32();
//        }

//        public override float GetWidth()
//        {
//            return Main.itemTexture[_itemType].Width * Scale;
//        }
//        public override float GetHeight()
//        {
//            return Main.itemTexture[_itemType].Height * Scale;            
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            if (!this.Visible)
//                return;
//            Item item = new Item();
//            item.netDefaults(_itemNumber);
//            Texture2D itemTexture = Main.itemTexture[item.type];

//            Color color = Lighting.GetColor((int)((double)this.X + (double)item.width * 0.5) / 16, (int)((double)this.Y + (double)item.height * 0.5) / 16);
//            float num3 = 1f;
//            Color alpha = item.GetAlpha(color);
//            if (item.type == 662 || item.type == 663)
//            {
//                alpha.R = (byte)Main.DiscoR;
//                alpha.G = (byte)Main.DiscoG;
//                alpha.B = (byte)Main.DiscoB;
//                alpha.A = 255;
//            }
//            if (item.type == 520 || item.type == 521 || item.type == 547 || item.type == 548 || item.type == 549)
//            {
//                num3 = Main.essScale;
//                alpha.R = (byte)((float)alpha.R * num3);
//                alpha.G = (byte)((float)alpha.G * num3);
//                alpha.B = (byte)((float)alpha.B * num3);
//                alpha.A = (byte)((float)alpha.A * num3);
//            }
//            else if (item.type == 58 || item.type == 184)
//            {
//                num3 = Main.essScale * 0.25f + 0.75f;
//                alpha.R = (byte)((float)alpha.R * num3);
//                alpha.G = (byte)((float)alpha.G * num3);
//                alpha.B = (byte)((float)alpha.B * num3);
//                alpha.A = (byte)((float)alpha.A * num3);
//            }

//            Vector2 drawPosition = this.Position;
//            if (!this.ScreenRelative)
//            {
//                drawPosition -= Main.screenPosition;
//            }

//            if (item.type >= 1522 && item.type <= 1527)
//            {
//                Main.spriteBatch.Draw(itemTexture, drawPosition, null, new Color(250, 250, 250, (int)(Main.mouseTextColor / 2)), 0f, Origin / Scale, Scale, SpriteEffects.None, 0f);
//                return;
//            }
//            if (!EffectedByWorldLighting)
//            {
//                alpha = Color.White;
//                color = Color.White;
//            }
//            Main.spriteBatch.Draw(itemTexture, drawPosition, null, alpha, 0f, Origin / Scale, Scale, SpriteEffects.None, 0f);
//            if (item.color != default(Color))
//            {
//                Main.spriteBatch.Draw(itemTexture, drawPosition, null, item.GetColor(color), 0f, Origin / Scale, Scale, SpriteEffects.None, 0f);
//            }
//        }
//    }
//}
