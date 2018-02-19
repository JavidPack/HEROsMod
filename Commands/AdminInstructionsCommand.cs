using HEROsMod.HEROsModNetwork;
using System;
using Terraria.ModLoader;

namespace HEROsMod.Commands
{
	internal class AdminInstructionsCommand : ModCommand
	{
		public override CommandType Type => CommandType.Console;

		public override string Command => "HEROsAdmin";

		public override string Description => HEROsMod.HeroText("AdminInstructionsCommandDescription");

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(string.Format(HEROsMod.HeroText("DedicatedServerAutoMessage"), Network.AuthCode));
			Console.ResetColor();
		}
	}
}