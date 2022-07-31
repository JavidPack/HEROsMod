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
	[Label("$Mods.HEROsMod.Configuration.ModConfig")]
	class HEROsModServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		/*
		[Label("Disable Achievements")]
		[Tooltip("While this is true, vanilla steam achievements will not be obtained.\nUse this if you don't wish to get achievements illegitimately.")]
		[DefaultValue(false)]
		public bool DisableAchievements { get; set; }
		*/

		[Label("$Mods.HEROsMod.Configuration.Label.FreezeNonLoggedIn")]
		[Tooltip("$Mods.HEROsMod.Configuration.Tooltip.FreezeNonLoggedIn")]
		[DefaultValue(false)]
		public bool FreezeNonLoggedIn { get; set; }

		[Label("$Mods.HEROsMod.Configuration.Label.Telemetry")]
		[Tooltip("$Mods.HEROsMod.Configuration.Tooltip.Telemetry")]
		[DefaultValue(true)]
		[ReloadRequired]
		public bool Telemetry { get; set; }

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Network.Players[whoAmI].Group.IsAdmin)
				return true;

			message = HEROsMod.HeroText("Configuration.AcceptClientChangesMessage");
			return false;
		}
	}
}
