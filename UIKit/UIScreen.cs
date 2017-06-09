using Terraria;

namespace HEROsMod.UIKit
{
	internal class UIScreen : UIView
	{
		public UIScreen()
		{
			this.OverridesMouse = false;
		}

		protected override float GetWidth()
		{
			return Main.screenWidth;
		}

		protected override float GetHeight()
		{
			return Main.screenHeight;
		}
	}
}