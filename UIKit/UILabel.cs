using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace HEROsMod.UIKit
{
    class UILabel : UIView
    {
        public static SpriteFont defaultFont { get { return Main.fontDeathText; } }
        public SpriteFont font;
        private string text = "";
        public string Text
        {
            get { return text;}
            set
            {
                text = value;
                SetWidthHeight();
            }
        }
        bool textOutline = true;
        public bool TextOutline
        {
            get { return textOutline; }
            set { textOutline = value; }
        }
       
        float width = 0;
        float height = 0;

        public UILabel(string text)
        {
            font = defaultFont;
            this.Text = text;
        }

        public UILabel()
        {
            font = defaultFont;
            this.Text = "";
        }

        protected override Vector2 GetOrigin()
        {
            return base.GetOrigin();
        }

        void SetWidthHeight()
        {
            if (Text != null)
            {
                Vector2 size = font.MeasureString(Text);
                width = size.X;
                height = size.Y;
            }
            else
            {
                width = 0;
                height = 0;
            }
        }

        protected override float GetWidth()
        {
            return width * Scale;
        }

        protected override float GetHeight()
        {
            if (height == 0)
            {
                return font.MeasureString("H").Y * Scale;
            }
            else return height * Scale;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Text != null)
            {
                if(TextOutline)
                    Utils.DrawBorderStringFourWay(spriteBatch, font, Text, DrawPosition.X, DrawPosition.Y, ForegroundColor, Color.Black * Opacity, Origin / Scale, Scale);
                else
                    spriteBatch.DrawString(font, Text, DrawPosition, ForegroundColor * Opacity, 0f, Origin / Scale, Scale, SpriteEffects.None, 0f);
            }
            base.Draw(spriteBatch);
        }
    }
}
