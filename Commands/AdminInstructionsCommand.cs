using HEROsMod.HEROsModNetwork;
using System;
using Terraria.ModLoader;

namespace HEROsMod.Commands
{
	internal class AdminInstructionsCommand : ModCommand
	{
		public override CommandType Type => CommandType.Console;

		public override string Command => "HEROsAdmin";

		public override string Description => "Informs you how to become Admin in HERO's Mod";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Create an account, login, and type /auth " + Network.AuthCode + " to become Admin.");
			Console.ResetColor();
		}
	}
}