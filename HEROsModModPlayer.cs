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
using Terraria.GameInput;
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
				Player.controlDown = false;
				Player.controlUp = false;
				Player.controlLeft = false;
				Player.controlRight = false;

				Player.controlMount = false;
				Player.controlHook = false;
				Player.controlThrow = false;
				//	player.controlJump = false;
				Player.controlSmart = false;
				Player.controlTorch = false;
			}
			if(Main.netMode == NetmodeID.MultiplayerClient && !HEROsModServices.Login.LoggedIn && ModContent.GetInstance<HEROsModServerConfig>().FreezeNonLoggedIn)
			{
				Player.frozen = true;
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
			if (GodModeService.Enabled && !GodModeService.BuddhaMode)
			{
				return false;
			}
			return true;
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (GodModeService.Enabled && GodModeService.BuddhaMode)
			{
				Player.statLife = Player.statLifeMax2;
				Player.lifeRegen = 999;
				return false;
			}
			return true;
		}

		public override void PreUpdate()
		{
			if (GodModeService.Enabled)
			{
				if (!GodModeService.BuddhaMode)
					Player.statLife = Player.statLifeMax2;
				Player.statMana = Player.statManaMax2;
				Player.wingTime = Player.wingTimeMax;
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
			if (Main.myPlayer == Player.whoAmI && Main.netMode == NetmodeID.MultiplayerClient && !HEROsModServices.Login.LoggedIn && ModContent.GetInstance<HEROsModServerConfig>().FreezeNonLoggedIn)
			{
				// For visuals. Other players won't see this, but less error prone than Frozen debuff.
				Player.frozen = true;
			}
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			KeybindController.HotKeyPressed(triggersSet.KeyStatus);
		}
	}

}
