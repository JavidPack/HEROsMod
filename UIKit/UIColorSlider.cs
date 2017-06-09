using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HEROsMod.UIKit
{
	internal class UIColorSlider : UISlider
	{
		protected override float GetWidth()
		{
			return ModUtils.HueTexture.Width;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 pos = DrawPosition;
			pos.Y += (sliderTexture.Height - ModUtils.HueTexture.Height) / 2;
			spriteBatch.Draw(ModUtils.HueTexture, pos, null, BackgroundColor, 0f, Origin, 1f, SpriteEffects.None, 0f);
			Vector2 sliderPos = DrawPosition;
			sliderPos.X += padding - sliderTexture.Width / 2;
			sliderPos.X += (Width - padding * 2) * ((Value - MinValue) / (MaxValue - MinValue));
			spriteBatch.Draw(sliderTexture, sliderPos, null, BackgroundColor, 0f, Origin, 1f, SpriteEffects.None, 0f);
			//base.Draw(spriteBatch);
		}
	}
}