/*
using HEROsMod.HEROsModNetwork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace HEROsMod
{
	class HEROsModServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Label("Disable Achievements")]
		[Tooltip("While this is true, vanilla steam achievements will not be obtained.\nUse this if you don't wish to get achievements illegitimately.")]
		[DefaultValue(false)]
		public bool DisableAchievements { get; set; }

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Network.Players[whoAmI].Group.IsAdmin)
				return true;

			message = $"You must be Admin in Heros Mod to change the server permissions.";
			return false;
		}
	}
}
*/