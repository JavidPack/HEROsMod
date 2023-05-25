using HEROsMod.HEROsModNetwork;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace HEROsMod
{
	class HEROsModServerConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		/*
		[Label("Disable Achievements")]
		[Tooltip("While this is true, vanilla steam achievements will not be obtained.\nUse this if you don't wish to get achievements illegitimately.")]
		[DefaultValue(false)]
		public bool DisableAchievements { get; set; }
		*/

		[DefaultValue(false)]
		public bool FreezeNonLoggedIn { get; set; }

		[DefaultValue(true)]
		[ReloadRequired]
		public bool Telemetry { get; set; }

		public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
		{
			if (Network.Players[whoAmI].Group.IsAdmin)
				return true;

			message = this.GetLocalizedValue("AcceptClientChangesMessage");
			return false;
		}
	}
}
