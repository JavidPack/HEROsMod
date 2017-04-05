using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Terraria;
using Terraria.ModLoader;
using HEROsMod.HEROsModNetwork;
using Terraria.ID;

namespace HEROsMod.HEROsModServices
{
	class ToggleGravestones : HEROsModService
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
			this._hotbarIcon = new UIImage(UIView.GetEmbeddedTexture("Images/gravestone")/*Main.projectileTexture[43]*/);
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = "Disable Gravestones";
			this._hotbarIcon.Opacity = 1f;
			HEROsModNetwork.GeneralMessages.GravestonesToggleByServer += GeneralMessages_GravestonesToggleByServer;
		}

		void GeneralMessages_GravestonesToggleByServer(bool gravestonesCanSpawn)
		{
			if (gravestonesCanSpawn)
			{
				this._hotbarIcon.Opacity = 1f;
				this.HotbarIcon.Tooltip = "Disable Gravestones";
			}
			else
			{
				this._hotbarIcon.Opacity = .5f;
				this.HotbarIcon.Tooltip = "Enable Gravestones";
			}
			Network.GravestonesAllowed = gravestonesCanSpawn;
		}

		void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (ModUtils.NetworkMode != NetworkMode.None)
			{
				HEROsModNetwork.GeneralMessages.RequestToggleGravestones();
			}
			else
			{
				Main.NewText("You have " + (Network.GravestonesAllowed ? "disabled" : "enabled") + " gravestones");
				GeneralMessages_GravestonesToggleByServer(!Network.GravestonesAllowed);
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ToggleGravestones");
			//base.MyGroupUpdated();
		}
	}

	class GraveStoneGlobalProjectile : GlobalProjectile
	{
		public override bool Autoload(ref string name) => true;

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
