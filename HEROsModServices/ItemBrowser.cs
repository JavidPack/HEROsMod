using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace HEROsMod.HEROsModServices
{
	internal class ItemBrowser : HEROsModService
	{
		private UIKit.UIComponents.ItemBrowser _itemBrowserWindow;

		public ItemBrowser()
		{
			this._name = "Item Browser";
			this._hotbarIcon = new UIImage(UIView.GetEmbeddedTexture("Images/items")/*Main.itemTexture[10]*/);
			this.HotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = "Item Browser";

			_itemBrowserWindow = new UIKit.UIComponents.ItemBrowser();
			_itemBrowserWindow.CenterToParent();
			_itemBrowserWindow.Position -= new Vector2(_itemBrowserWindow.Width / 2, (_itemBrowserWindow.Height / 2) + 30);
			this.AddUIView(_itemBrowserWindow);
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			// Toggle item browser window
			_itemBrowserWindow.Visible = !_itemBrowserWindow.Visible;
			// If browser window is visible, open the player inventory
			// Mouse items do no work unless the player inventory is open
			if (_itemBrowserWindow.Visible)
			{
				if (!UIKit.UIComponents.ItemBrowser.CategoriesLoaded)
				{
					ModUtils.DebugText("_hotbarIcon_onLeftClick calling ParseList2");

					UIKit.UIComponents.ItemBrowser.ParseList2();
					_itemBrowserWindow.SelectedCategory = null;
				}
				Main.playerInventory = true;
				_itemBrowserWindow.SearchBox.Focus();
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ItemBrowser");
			if (!HasPermissionToUse)
			{
				_itemBrowserWindow.Visible = false;
			}
			//base.MyGroupUpdated();
		}

		public override void Unload()
		{
			global::HEROsMod.UIKit.UIComponents.ItemBrowser.Unload();
		}
	}
}