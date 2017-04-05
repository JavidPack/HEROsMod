using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HEROsMod.UIKit;

using Terraria;

namespace HEROsMod.HEROsModServices
{
	/// <summary>
	/// A Service that clears all items on the ground
	/// </summary>
	class ItemClearer : HEROsModService
	{
		public ItemClearer()
		{
			this._name = "Item Clearer";
			this._hotbarIcon = new UIImage(UIView.GetEmbeddedTexture("Images/canIcon"));
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = "Clear Items on Ground";
		}

		void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			//ClearItems
			if (ModUtils.NetworkMode == NetworkMode.None)
			{
				for (int i = 0; i < Main.item.Length; i++)
				{
					Main.item[i].active = false;
				}
				Main.NewText("Items on the ground were cleared");
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
