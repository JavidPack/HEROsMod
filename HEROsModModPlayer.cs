using HEROsMod.HEROsModNetwork;
using HEROsMod.HEROsModServices;
using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace HEROsMod
{
	public class HEROsModModPlayer : ModPlayer
	{
	//	public override bool Autoload(ref string name) => true;

		private float FreezeNonLoggedInMessageTimer = 7f;

		public override void SetControls()
		{
			if (FlyCam.Enabled && !FlyCam.LockCamera)
			{
				player.controlDown = false;
				player.controlUp = false;
				player.controlLeft = false;
				player.controlRight = false;

				player.controlMount = false;
				player.controlHook = false;
				player.controlThrow = false;
				//	player.controlJump = false;
				player.controlSmart = false;
				player.controlTorch = false;
			}
			if(Main.netMode == NetmodeID.MultiplayerClient && !HEROsModServices.Login.LoggedIn && ModContent.GetInstance<HEROsModServerConfig>().FreezeNonLoggedIn)
			{
				player.frozen = true;
				FreezeNonLoggedInMessageTimer -= ModUtils.DeltaTime;
				if (FreezeNonLoggedInMessageTimer <= 0)
				{
					FreezeNonLoggedInMessageTimer = 7f;
					Main.NewText(HEROsMod.HeroText("LoginInstructions"), Color.Red);
				}
			}
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (GodModeService.Enabled)
			{
				return false;
			}
			return true;
		}

		public override void PreUpdate()
		{
			if (GodModeService.Enabled)
			{
				player.statLife = player.statLifeMax2;
				player.statMana = player.statManaMax2;
				player.wingTime = player.wingTimeMax;
			}
		}

		// TODO - make tmodloader hook, this only gets called while there are players in the world.
		private double time;

		public override void PostUpdate()
		{
			if (Main.dedServ)
			{
				if (time != Main.time)
				{
					time = Main.time;
					HEROsModNetwork.Network.Update();
				}
			}
			if (Main.myPlayer == player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient && !HEROsModServices.Login.LoggedIn && ModContent.GetInstance<HEROsModServerConfig>().FreezeNonLoggedIn)
			{
				// For visuals. Other players won't see this, but less error prone than Frozen debuff.
				player.frozen = true;
			}
		}
	}

}
