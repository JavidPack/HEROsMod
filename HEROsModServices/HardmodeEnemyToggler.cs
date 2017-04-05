//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using HEROsModMod.UIKit;

//using Terraria;
//using HEROsModMod.UIKit.UIComponents;

//namespace HEROsModMod.HEROsModServices
//{
//	class HardmodeEnemyToggler : HEROsModService
//	{
//		public HardmodeEnemyToggler(UIHotbar hotbar)
//		{
//			IsInHotbar = true;
//			HotbarParent = hotbar;
//			this._name = "Hardmode Enemy Toggler";
//			this._hotbarIcon = new UIImage(Main.itemTexture[1991]);
//			this._hotbarIcon.Tooltip = "Toggle Hardmode Enemies";
//			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
//		}

//		public override void MyGroupUpdated()
//		{
//			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ToggleHardmodeEnemies");
//			base.MyGroupUpdated();
//		}

//		void _hotbarIcon_onLeftClick(object sender, EventArgs e)
//		{
//			if (ModUtils.NetworkMode == NetworkMode.None)
//			{
//				ToggleHardModeEnemies();
//			}
//			else
//			{
//				HEROsModNetwork.GeneralMessages.RequestToggleHardmodeEnemies();
//			}
//		}

//		public static void ToggleHardModeEnemies()
//		{
//			Main.hardMode = !Main.hardMode;
//			EnemyToggler.ClearNPCs();
//			if (ModUtils.NetworkMode == NetworkMode.Server)
//			{
//				NetMessage.SendData(7);
//			}
//		}
//	}
//}
