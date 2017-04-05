using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace HEROsMod.UIKit
{
    class UIRect : UIView
    {
        public UIRect()
        {
            this.Width = 10;
            this.Height = 10;
        }

        public UIRect(Vector2 position, float width, float height)
        {
            this.Position = position;
            this.Width = width;
            this.Height = height;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = ModUtils.DummyTexture;
            spriteBatch.Draw(texture, new Rectangle((int)(DrawPosition.X - Origin.X), (int)(DrawPosition.Y - Origin.Y), (int)Width, (int)Height), ForegroundColor);
            base.Draw(spriteBatch);
        }
    }
}
