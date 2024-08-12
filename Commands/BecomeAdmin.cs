using HEROsMod.HEROsModNetwork;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace HEROsMod.Commands
{
	internal class BecomeAdmin : ModCommand
	{
		public override CommandType Type => CommandType.World;

		public override string Command => "auth";

		public override string Description => HEROsMod.HeroText("MakesYouAdminInHEROsMod");

		public override string Usage => "/auth SecretCode";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if(Main.netMode == NetmodeID.SinglePlayer)
			{
				throw new UsageException("Only use this command while on server.");
			}
			if (args.Length != 1 || args[0].Length != 6)
			{
				throw new UsageException();
			}
			if (args[0] == Network.AuthCode.ToString())
			{
				
				if (Network.Players[caller.Player.whoAmI].Username.Length > 0)
				{
					Network.Players[caller.Player.whoAmI].Group = Network.AdminGroup;
					DatabaseController.SetPlayerGroup(Network.Players[caller.Player.whoAmI].ID, Network.Players[caller.Player.whoAmI].Group.ID);
					LoginService.SendPlayerPermissions(caller.Player.whoAmI);
					Network.SendTextToPlayer(HEROsMod.HeroText("YouAreNowAdmin"), caller.Player.whoAmI);
					return;
				}
				else
				{
					Network.SendTextToPlayer(HEROsMod.HeroText("PleaseLoginFirst"), caller.Player.whoAmI);
					return;
				}
			}
			throw new UsageException(HEROsMod.HeroText("AuthCodeIsIncorrect"));
		}
	}
}
