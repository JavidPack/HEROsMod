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
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace HEROsMod
{
	public class HEROsModModPlayer : ModPlayer
	{
		public override void SetControls()
		{
			if (FlyCam.Enabled)
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
		}
	}

}
