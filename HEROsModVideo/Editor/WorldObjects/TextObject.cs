//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.WorldObjects
//{
//    class TextObject : WorldObject
//    {
//        public string Text { get; set; }
//        [HideInInspector]
//        public SpriteFont Font { get; set; }
//        public Color TextColor { get; set; }
//        [Range(0,20)]
//        public float BorderSize { get; set; }
//        public Color BorderColor { get; set; }
//        public TextObject()
//        {
//            this.Font = Main.fontDeathText;
//            this.TextColor = Color.White;
//            this.BorderSize = 1f;
//            this.BorderColor = Color.Black;
//            this.Text = "Text";
//            this.EffectedByWorldLighting = false;
//            this.Anchor = UIKit.AnchorPosition.Center;
//        }

//        public override float GetWidth()
//        {
//            return this.Font.MeasureString(this.Text).X * Scale;
//        }
//        public override float GetHeight()
//        {
//            return this.Font.MeasureString(this.Text).Y * Scale;
//        }

//        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
//        {
//            if (!this.Visible)
//                return;
//            Vector2 drawPosition = this.Position;
//            if(!this.ScreenRelative)
//            {
//                drawPosition -= Main.screenPosition;
//            }
//            if(BorderSize > 0)
//            {
//                ModUtils.DrawStringBorder(spriteBatch, this.Font, drawPosition, Text, this.BorderColor, this.BorderSize, this.Origin / Scale, Scale);
//            }
//            spriteBatch.DrawString(Main.fontDeathText, this.Text, drawPosition, this.TextColor, 0f, this.Origin/ Scale, this.Scale, SpriteEffects.None, 0f);
//            base.Draw(spriteBatch);
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            writer.Write(this.Text);
//            writer.WriteRGB(this.TextColor);
//            writer.Write(this.BorderSize);
//            writer.WriteRGB(this.BorderColor);
//        }

//        public override void Load(float saveVersion, ref System.IO.BinaryReader reader)
//        {
//            base.Load(saveVersion, ref reader);
//            if(saveVersion > 1)
//            {
//                this.Text = reader.ReadString();
//            }
//            if(saveVersion > 2)
//            {
//                this.TextColor = reader.ReadRGB();
//            }
//            if(saveVersion > 3)
//            {
//                this.BorderSize = reader.ReadSingle();
//                this.BorderColor = reader.ReadRGB();
//            }
//        }
//    }
//}
