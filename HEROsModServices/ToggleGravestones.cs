using HEROsMod.HEROsModNetwork;
using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class ToggleGravestones : HEROsModService
	{
		public static int[] gravestoneProjectiles = new int[] {
		ProjectileID.Tombstone,
		ProjectileID.GraveMarker,
		ProjectileID.CrossGraveMarker,
		ProjectileID.Headstone,
		ProjectileID.Gravestone,
		ProjectileID.Obelisk,
		ProjectileID.RichGravestone1, ProjectileID.RichGravestone2,ProjectileID.RichGravestone3,
		ProjectileID.RichGravestone4, ProjectileID.RichGravestone5,
		};

		//  public ToggleGravestones()
		public ToggleGravestones(UIHotbar hotbar)
		{
			IsInHotbar = true;
			HotbarParent = hotbar;
			//MultiplayerOnly = true;
			this._name = "Gravestones Toggler";
			ModUtils.LoadProjectile(43);
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/gravestone", AssetRequestMode.ImmediateLoad)/*Main.projectileTexture[43]*/);
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("DisableGravestones");
			this._hotbarIcon.Opacity = 1f;
			HEROsModNetwork.GeneralMessages.GravestonesToggleByServer += GeneralMessages_GravestonesToggleByServer;
		}

		private void GeneralMessages_GravestonesToggleByServer(bool gravestonesCanSpawn)
		{
			if (gravestonesCanSpawn)
			{
				this._hotbarIcon.Opacity = 1f;
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("DisableGravestones");
			}
			else
			{
				this._hotbarIcon.Opacity = .5f;
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("EnableGravestones");
			}
			Network.GravestonesAllowed = gravestonesCanSpawn;
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (ModUtils.NetworkMode != NetworkMode.None)
			{
				HEROsModNetwork.GeneralMessages.RequestToggleGravestones();
			}
			else
			{
				if (Network.GravestonesAllowed)
					Main.NewText(HEROsMod.HeroText("YouHaveDisabledGravestones"));
				else
					Main.NewText(HEROsMod.HeroText("YouHaveEnabledGravestones"));
				GeneralMessages_GravestonesToggleByServer(!Network.GravestonesAllowed);
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ToggleGravestones");
			//base.MyGroupUpdated();
		}
	}

	internal class GraveStoneGlobalProjectile : GlobalProjectile
	{
	//	public override bool Autoload(ref string name) => true;

		public override bool PreAI(Projectile projectile)
		{
			if (!Network.GravestonesAllowed)
			{
				if (ToggleGravestones.gravestoneProjectiles.Contains(projectile.type))
				{
					//ErrorLogger.Log("Ded " + Main.dedServ + " projectile.type active false: " + projectile.type);
					projectile.active = false;
					return false;
				}
			}
			return base.PreAI(projectile);
		}
	}
}