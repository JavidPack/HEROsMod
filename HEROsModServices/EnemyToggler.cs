using HEROsMod.UIKit;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace HEROsMod.HEROsModServices
{
	/// <summary>
	/// A Service that let's you toggle the enemies on the map
	/// </summary>
	internal class EnemyToggler : HEROsModService
	{
		public static bool EnemiesAllowed = true;

		public EnemyToggler()
		{
			this._name = "Enemy Toggler";
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/npcIcon", AssetRequestMode.ImmediateLoad));
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("DisableEnemySpawns");
			this._hotbarIcon.Opacity = 1f;
			HEROsModNetwork.GeneralMessages.EnemiesToggledByServer += GeneralMessages_EnemiesToggledByServer;
		}

		private void GeneralMessages_EnemiesToggledByServer(bool enemiesCanSpawn)
		{
			if (enemiesCanSpawn)
			{
				this._hotbarIcon.Opacity = 1f;
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("DisableEnemySpawns");
			}
			else
			{
				this._hotbarIcon.Opacity = .5f;
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("EnableEnemySpawns");
			}
		}

		public static void ToggleNPCs()
		{
			if (EnemiesAllowed)
			{
				ClearNPCs();
			}
			EnemiesAllowed = !EnemiesAllowed;
		}

		public static void ClearNPCs()
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i] != null && !Main.npc[i].townNPC)
				{
					Main.npc[i].life = 0;
					if (Main.netMode == NetmodeID.Server) NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, i, 0f, 0f, 0f, 0);
				}
			}
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (ModUtils.NetworkMode != NetworkMode.None)
			{
				HEROsModNetwork.GeneralMessages.RequestToggleEnemies();
			}
			else
			{
				ToggleNPCs();
				if (EnemiesAllowed)
				{
					this._hotbarIcon.Opacity = 1f;
					this.HotbarIcon.Tooltip = HEROsMod.HeroText("DisableEnemySpawns");
				}
				else
				{
					this._hotbarIcon.Opacity = .5f;
					this.HotbarIcon.Tooltip = HEROsMod.HeroText("EnableEnemySpawns");
				}
				if(EnemiesAllowed)
					Main.NewText(HEROsMod.HeroText("EnemySpawnsEnabled"));
				else
					Main.NewText(HEROsMod.HeroText("EnemySpawnsDisabled"));
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ToggleEnemies");
			//base.MyGroupUpdated();
		}

		public override void Destroy()
		{
			HEROsModNetwork.GeneralMessages.EnemiesToggledByServer -= GeneralMessages_EnemiesToggledByServer;
			EnemiesAllowed = true;
			base.Destroy();
		}
	}

	public class EnemyTogglerGlobalNPC : GlobalNPC
	{
	//	public override bool Autoload(ref string name) => true;

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			if (!EnemyToggler.EnemiesAllowed)
			{
				spawnRate = 0;
				maxSpawns = 0;
			}
		}
	}
}