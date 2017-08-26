using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HEROsMod.UIKit
{
	internal class UIScrollBar : UIView
	{
		internal static Texture2D ScrollbarTexture;
		private static Texture2D scrollbarFill;

		private static Texture2D ScrollbarFill
		{
			get
			{
				if (scrollbarFill == null)
				{
					Color[] edgeColors = new Color[ScrollbarTexture.Width * ScrollbarTexture.Height];
					ScrollbarTexture.GetData(edgeColors);
					Color[] fillColors = new Color[ScrollbarTexture.Width];
					for (int x = 0; x < fillColors.Length; x++)
					{
						fillColors[x] = edgeColors[x + (ScrollbarTexture.Height - 1) * ScrollbarTexture.Width];
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
			return ScrollbarTexture.Width;
		}

		private void DrawScrollBar(SpriteBatch spriteBatch)
		{
			float fillHeight = Height - ScrollbarTexture.Height * 2;
			Vector2 pos = DrawPosition;
			spriteBatch.Draw(ScrollbarTexture, pos, null, Color.White * Opacity, 0f, Origin, 1f, SpriteEffects.None, 0f);
			pos.Y += ScrollbarTexture.Height;
			spriteBatch.Draw(ScrollbarFill, pos - Origin, null, Color.White * Opacity, 0f, Vector2.Zero, new Vector2(1f, fillHeight), SpriteEffects.None, 0f);
			pos.Y += fillHeight;
			spriteBatch.Draw(ScrollbarTexture, pos, null, Color.White * Opacity, 0f, Origin, 1f, SpriteEffects.FlipVertically, 0f);
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			DrawScrollBar(spriteBatch);
			base.Draw(spriteBatch);
		}
	}
}