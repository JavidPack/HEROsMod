using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HEROsMod.UIKit;

using Terraria;
using HEROsMod.UIKit.UIComponents;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace HEROsMod.HEROsModServices
{
	class ItemBanner : HEROsModService
	{
		public static bool ItemsBanned { get; set; }
		public static int[] bannedProjectiles = new int[]
		{
			ProjectileID.RocketII,
			ProjectileID.RocketIV,
			ProjectileID.Bomb,
			ProjectileID.StickyBomb,
			ProjectileID.BombFish,
			ProjectileID.Dynamite,
			ProjectileID.StickyDynamite,
			ProjectileID.BouncyDynamite,
		};

		public ItemBanner(UIHotbar hotbar)
		{
			IsInHotbar = true;
			HotbarParent = hotbar;
			MultiplayerOnly = true;
			this._name = "Item Banner";
			this._hotbarIcon = new UIImage(UIView.GetEmbeddedTexture("Images/explosives")/*Main.itemTexture[888]*/);
			this._hotbarIcon.Tooltip = "Ban Destructive Explosives";
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			HEROsModNetwork.GeneralMessages.ItemBannerToggleByServer += GeneralMessages_BannedItemsToggleByServer;
		}

		void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (ModUtils.NetworkMode == NetworkMode.Client)
			{
				HEROsModNetwork.GeneralMessages.RequestToggleBannedItems();
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ToggleBannedItems");
			//base.MyGroupUpdated();
		}

		public void GeneralMessages_BannedItemsToggleByServer(bool itemsbanned)
		{
			if (itemsbanned)
			{
				this._hotbarIcon.Opacity = .5f;
				this.HotbarIcon.Tooltip = "Unban Destructive Explosives";
			}
			else
			{
				this._hotbarIcon.Opacity = 1f;
				this.HotbarIcon.Tooltip = "Ban Destructive Explosives";
			}
			ItemsBanned = itemsbanned;
		}
	}

	class ItemBannerGlobalProjectile : GlobalProjectile
	{
		public override bool Autoload(ref string name) => true;

		// Is only called on server??
		public override bool PreAI(Projectile projectile)
		{
			if (ItemBanner.ItemsBanned)
			{
				if (ItemBanner.bannedProjectiles.Contains(projectile.type))
				{
					if (!Main.dedServ)
					{
						//Projectile newProj = new Projectile();
						//newProj.SetDefaults(type);
						Main.NewText(projectile.Name + " is banned on the server", Color.Red.R, Color.Red.G, Color.Red.B );
					}
					//ErrorLogger.Log(Main.dedServ + " Item Banned");
					projectile.active = false;
					return false;
				}
			}
			return base.PreAI(projectile);
		}
	}
}
