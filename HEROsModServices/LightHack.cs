using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	/// <summary>
	/// A service that hacks the lighting values for the player
	/// </summary>
	class LightHack : HEROsModService
	{
		internal static int LightStrength;
		internal static float[] LightStrengthValues = new float[] { 0, .25f, .5f, 1f };
		private static string[] LightStrengthStrings = new string[] { HEROsMod.HeroText("LightHackDisabled"), HEROsMod.HeroText("LightHack25%"), HEROsMod.HeroText("LightHack50%"), HEROsMod.HeroText("LightHack100%") };

		public LightHack(UIHotbar hotbar)
		{
			IsInHotbar = true;
			HotbarParent = hotbar;
			this._name = "Light Hack";
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/lighthack", AssetRequestMode.ImmediateLoad));
			this._hotbarIcon.onLeftClick += (s, e) =>
			{
				buttonLogic(true);
			};
			this._hotbarIcon.onRightClick += (s, e) =>
			{
				buttonLogic(false);
			};
			this.HotbarIcon.Tooltip = LightStrengthStrings[LightStrength];
			_hotbarIcon.Opacity = 0.5f;
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("LightHack");
		}

		public void buttonLogic(bool leftMouse)
		{
			LightStrength = leftMouse ? (LightStrength + 1) % LightStrengthStrings.Length : (LightStrength + LightStrengthStrings.Length - 1) % LightStrengthStrings.Length;
			HotbarIcon.Tooltip = LightStrengthStrings[LightStrength];
			_hotbarIcon.Opacity = (LightStrengthValues[LightStrength] + 1f) / 2;
		}
	}

	public class LightHackGlobalWall : GlobalWall
	{
		public override void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
		{
			if (LightHack.LightStrength > 0)
			{
				r = MathHelper.Clamp(r + LightHack.LightStrengthValues[LightHack.LightStrength], 0, 1);
				g = MathHelper.Clamp(g + LightHack.LightStrengthValues[LightHack.LightStrength], 0, 1);
				b = MathHelper.Clamp(b + LightHack.LightStrengthValues[LightHack.LightStrength], 0, 1);
			}
		}
	}
}
