using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class ItemBanner : HEROsModService
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
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/explosives", AssetRequestMode.ImmediateLoad));
			this._hotbarIcon.Tooltip = HEROsMod.HeroText("BanDestructiveExplosives");
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			HEROsModNetwork.GeneralMessages.ItemBannerToggleByServer += GeneralMessages_BannedItemsToggleByServer;
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
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
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("UnbanDestructiveExplosives");
			}
			else
			{
				this._hotbarIcon.Opacity = 1f;
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("BanDestructiveExplosives");
			}
			ItemsBanned = itemsbanned;
		}
	}

	internal class ItemBannerGlobalProjectile : GlobalProjectile
	{
	//	public override bool Autoload(ref string name) => true;

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
						Main.NewText(string.Format(HEROsMod.HeroText("ProjectileIsBannerdOnTheServer"), projectile.Name), Color.Red.R, Color.Red.G, Color.Red.B);
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