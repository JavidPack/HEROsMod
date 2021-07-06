using HEROsMod.UIKit;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class GodModeService : HEROsModService
	{
		private delegate void GodModeToggledEvent(bool enabled, bool prevEnabled);

		private static event GodModeToggledEvent GodModeToggled;

		private static bool _enabled = false;

		public static bool Enabled
		{
			get { return _enabled; }
			set
			{
				if (GodModeToggled != null)
				{
					GodModeToggled(value, _enabled);
				}
				_enabled = value;
			}
		}

		public GodModeService()
		{
			this._hotbarIcon = new UIImage(ModContent.Request<Texture2D>("HEROsMod/Images/godMode", AssetRequestMode.ImmediateLoad).Value/*Main.itemTexture[1990]*/);
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("ToggleGodMode");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
			GodModeToggled += GodModeService_GodModeToggled;
			Enabled = false;
		}

		private void GodModeService_GodModeToggled(bool enabled, bool prevEnabled)
		{
			if (enabled)
			{
				if (enabled != prevEnabled)
					Main.NewText(HEROsMod.HeroText("GodModeEnabled"));
				this.HotbarIcon.Opacity = 1f;
			}
			else
			{
				if (enabled != prevEnabled)
					Main.NewText(HEROsMod.HeroText("GodModeDisabled"));
				this.HotbarIcon.Opacity = .5f;
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("GodMode");
			if (!HasPermissionToUse)
			{
				Enabled = false;
			}
			//base.MyGroupUpdated();
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (ModUtils.NetworkMode == NetworkMode.None)
			{
				Enabled = !Enabled;
			}
			else
			{
				if (!Enabled)
				{
					HEROsModNetwork.GeneralMessages.RequestGodMode();
				}
				else
				{
					Enabled = false;
				}
			}
		}

		public override void Destroy()
		{
			Enabled = false;
			GodModeToggled -= GodModeService_GodModeToggled;
			base.Destroy();
		}
	}
}