using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace HEROsMod.UIKit
{
	internal class UIScrollBar : UIView
	{
		internal static Asset<Texture2D> ScrollbarTexture;
		private static Texture2D scrollbarFill;

		private static Texture2D ScrollbarFill
		{
			get
			{
				if (scrollbarFill == null)
				{
					Color[] edgeColors = new Color[ScrollbarTexture.Value.Width * ScrollbarTexture.Value.Height];
					ScrollbarTexture.Value.GetData(edgeColors);
					Color[] fillColors = new Color[ScrollbarTexture.Value.Width];
					for (int x = 0; x < fillColors.Length; x++)
					{
						fillColors[x] = edgeColors[x + (ScrollbarTexture.Value.Height - 1) * ScrollbarTexture.Value.Width];
					}
					scrollbarFill = new Texture2D(UIView.graphics, fillColors.Length, 1);
					scrollbarFill.SetData(fillColors);
				}
				return scrollbarFill;
			}
		}

		private float height = 100;

		protected override float GetHeight()
		{
			return height;
		}

		protected override void SetHeight(float height)
		{
			this.height = height;
		}

		protected override float GetWidth()
		{
			return ScrollbarTexture.Value.Width;
		}

		private void DrawScrollBar(SpriteBatch spriteBatch)
		{
			float fillHeight = Height - ScrollbarTexture.Value.Height * 2;
			Vector2 pos = DrawPosition;
			spriteBatch.Draw(ScrollbarTexture.Value, pos, null, Color.White * Opacity, 0f, Origin, 1f, SpriteEffects.None, 0f);
			pos.Y += ScrollbarTexture.Value.Height;
			spriteBatch.Draw(ScrollbarFill, pos - Origin, null, Color.White * Opacity, 0f, Vector2.Zero, new Vector2(1f, fillHeight), SpriteEffects.None, 0f);
			pos.Y += fillHeight;
			spriteBatch.Draw(ScrollbarTexture.Value, pos, null, Color.White * Opacity, 0f, Origin, 1f, SpriteEffects.FlipVertically, 0f);
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			DrawScrollBar(spriteBatch);
			base.Draw(spriteBatch);
		}
	}
}