using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace HEROsMod.UIKit.UIComponents
{
	internal class ItemView : UIView
	{
		private Asset<Texture2D> _demonAltarTexture = null;

		private Asset<Texture2D> DemonAltarTexture
		{
			get
			{
				if (_demonAltarTexture == null)
				{
					_demonAltarTexture = HEROsMod.instance.Assets.Request<Texture2D>("Images/Demon_Altar", AssetRequestMode.ImmediateLoad);
				}
				return _demonAltarTexture;
			}
		}

		public Item item = new Item();
		public int index = -1;

		public ItemView(Item item)
		{
			this.Scale = .85f;
			this.item = item.Clone();
			this.onHover += Slot2_onHover;
		}

		private void Slot2_onHover(object sender, EventArgs e)
		{
			HoverText = item.Name;
			//HoverItem = item.Clone();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			//Demon altar exception
			Texture2D texture = null;
			if (item.netID == -1000)
			{
				texture = DemonAltarTexture.Value;
			}
			else
			{
				Main.instance.LoadItem(item.type);
				texture = TextureAssets.Item[item.type].Value;
			}

			float itemScale = 1f;
			float pixelWidth = Width * Scale;
			if (texture.Width > pixelWidth || texture.Height > pixelWidth)
			{
				if (texture.Width > texture.Height)
				{
					itemScale = pixelWidth / (float)texture.Width;
				}
				else
				{
					itemScale = pixelWidth / (float)texture.Height;
				}
			}
			//itemScale *= Scale;

			Vector2 pos = DrawPosition;

			//spriteBatch.Draw(ModUtils.DummyTexture, new Rectangle((int)pos.X, (int)pos.Y, (int)Width, (int)Height), Color.Red);

			Vector2 texturePos = new Vector2(this.Width / 2, this.Height / 2);
			Vector2 itemOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

			pos += texturePos;

			spriteBatch.Draw(texture, pos, null, item.GetAlpha(Color.White), 0f, itemOrigin, itemScale, SpriteEffects.None, 0f);
			if (item.color != default(Color))
			{
				spriteBatch.Draw(texture, pos, null, item.GetColor(Color.White), 0f, itemOrigin, itemScale, SpriteEffects.None, 0f);
			}
			if (item.stack > 1)
			{
				spriteBatch.DrawString(FontAssets.ItemStack.Value, item.stack.ToString(), new Vector2(DrawPosition.X + 10f * Scale, DrawPosition.Y + 26f * Scale), Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
			}
			base.Draw(spriteBatch);
		}
	}
}