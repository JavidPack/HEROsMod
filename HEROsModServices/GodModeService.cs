﻿using HEROsMod.UIKit;
using System;
using Terraria;

namespace HEROsMod.HEROsModServices
{
	internal class GodModeService : HEROsModService
	{
		private delegate void GodModeToggledEvent(bool enabled, bool prevEnabled);

		private static event GodModeToggledEvent GodModeToggled;

		internal static event Action<bool> GodModeCallback;

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

		internal static void ClearGodModeCallback()
		{
			GodModeCallback = null;
		}

		internal static void InvokeGodModeCallback()
		{
			GodModeCallback?.Invoke(Enabled);
		}

		public GodModeService()
		{
			this._hotbarIcon = new UIImage(HEROsMod.instance.GetTexture("Images/godMode")/*Main.itemTexture[1990]*/);
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