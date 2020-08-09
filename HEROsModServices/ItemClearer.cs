using HEROsMod.UIKit;
using System;
using Terraria;

namespace HEROsMod.HEROsModServices
{
	/// <summary>
	/// A Service that clears all items on the ground
	/// </summary>
	internal class ItemClearer : HEROsModService
	{
		public ItemClearer()
		{
			this._name = "Item Clearer";
			this._hotbarIcon = new UIImage(HEROsMod.instance.GetTexture("Images/canIcon").Value);
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("ClearItemsOnGround");
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			//ClearItems
			if (ModUtils.NetworkMode == NetworkMode.None)
			{
				for (int i = 0; i < Main.maxItems; i++)
				{
					Main.item[i].active = false;
				}
				Main.NewText(HEROsMod.HeroText("ItemsOnTheGroundWereCleared"));
			}
			else
			{
				HEROsModNetwork.GeneralMessages.RequestClearGroundItems();
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ClearItems");
			//base.MyGroupUpdated();
		}
	}
}