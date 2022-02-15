using HEROsMod.UIKit;
using System;
using Terraria;

namespace HEROsMod.HEROsModServices
{
	internal class BuddhaModeService : HEROsModService
	{
		private delegate void BuddhaModeToggledEvent(bool enabled, bool prevEnabled);

		private static event BuddhaModeToggledEvent BuddhaModeToggled;

		private static bool _enabled = false;

		public static bool Enabled
		{
			get { return _enabled; }
			set
			{
				if (BuddhaModeToggled != null)
				{
					BuddhaModeToggled(value, _enabled);
				}
				_enabled = value;
			}
		}

		public BuddhaModeService()
		{
			this._hotbarIcon = new UIImage(HEROsMod.instance.GetTexture("Images/buddhaMode")/*Main.itemTexture[1990]*/);
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("ToggleBuddhaMode");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
			BuddhaModeToggled += BuddhaModeService_BuddhaModeToggled;
			Enabled = false;
		}

		private void BuddhaModeService_BuddhaModeToggled(bool enabled, bool prevEnabled)
		{
			if (enabled)
			{
				if (enabled != prevEnabled)
					Main.NewText(HEROsMod.HeroText("BuddhaModeEnabled"));
				this.HotbarIcon.Opacity = 1f;
			}
			else
			{
				if (enabled != prevEnabled)
					Main.NewText(HEROsMod.HeroText("BuddhaModeDisabled"));
				this.HotbarIcon.Opacity = .5f;
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("BuddhaMode");
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
					HEROsModNetwork.GeneralMessages.RequestBuddhaMode();
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
			BuddhaModeToggled -= BuddhaModeService_BuddhaModeToggled;
			base.Destroy();
		}
	}
}