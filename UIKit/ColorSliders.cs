using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent;

namespace HEROsMod.UIKit
{
	internal class HueSlider : UISlider
	{
		public HueSlider()
		{
			this.Width = ModUtils.HueTexture.Width;
		}

		public override void DrawBackground(SpriteBatch spriteBatch)
		{
			Vector2 pos = DrawPosition;
			pos.Y += (sliderTexture.Height - ModUtils.HueTexture.Height) / 2;
			Main.spriteBatch.Draw(ModUtils.HueTexture, pos, Color.White);
		}
	}

	internal class SaturationSlider : UISlider
	{
		public float Hue { get; set; }
		public float Luminosity { get; set; }

		public SaturationSlider()
		{
			this.Width = ModUtils.HueTexture.Width;
		}

		public override void DrawBackground(SpriteBatch spriteBatch)
		{
			Vector2 pos = DrawPosition;
			pos.Y += (sliderTexture.Height - ModUtils.HueTexture.Height) / 2;
			Main.spriteBatch.Draw(TextureAssets.ColorBar.Value, pos, Color.White);
			int fillWidth = 167;
			for (int k = 0; k <= fillWidth; k++)
			{
				float saturation = (float)k / (float)fillWidth;
				Color color4 = Main.hslToRgb(Hue, saturation, Luminosity);
				Main.spriteBatch.Draw(TextureAssets.ColorBlip.Value, new Vector2((float)(pos.X + k + 5), (float)(pos.Y + 4)), color4);
			}
		}
	}

	internal class LuminositySlider : UISlider
	{
		public float Hue { get; set; }
		public float Saturation { get; set; }

		public LuminositySlider()
		{
			this.Width = ModUtils.HueTexture.Width;
		}

		public override void DrawBackground(SpriteBatch spriteBatch)
		{
			Vector2 pos = DrawPosition;
			pos.Y += (sliderTexture.Height - ModUtils.HueTexture.Height) / 2;
			Main.spriteBatch.Draw(TextureAssets.ColorBar.Value, pos, Color.White);

			int fillWidth = 167;
			for (int l = 0; l <= fillWidth; l++)
			{
				float luminosity = (float)l / (float)fillWidth;
				Color color5 = Main.hslToRgb(Hue, Saturation, luminosity);
				Main.spriteBatch.Draw(TextureAssets.ColorBlip.Value, new Vector2((float)(pos.X + l + 5), (float)(pos.Y + 4)), color5);
			}
		}
	}
}