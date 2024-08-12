using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;

namespace HEROsMod.HEROsModNetwork
{
	internal class LoginService
	{
		private static BinaryWriter Writer
		{
			get { return HEROsModNetwork.Network.writer; }
		}

		public static Group MyGroup
		{
			get { return Network.Players[Main.myPlayer].Group; }
			set { Network.Players[Main.myPlayer].Group = value; }
		}

		public static event EventHandler GroupChanged;

		public static event EventHandler MyGroupChanged;

		public static void ProcessData(ref BinaryReader reader, int playerNumber)
		{
			MessageType msgType = (MessageType)reader.ReadByte();
			switch (msgType)
			{
				case MessageType.RequestLogin:
					ProcessLoginRequest(ref reader, playerNumber);
					break;

				case MessageType.LoginSucess:
					ProcessLoginSuccess(ref reader);
					break;

				case MessageType.RequestLogout:
					ProcessLogoutRequest(playerNumber);
					break;

				case MessageType.LogoutSucess:
					ProcessLogoutSuccess(ref reader);
					break;

				case MessageType.RequestRegistration:
					ReadRegistrationRequest(ref reader, playerNumber);
					break;

				case MessageType.RequestAddGroup:
					ProcessAddGroupReqest(ref reader, playerNumber);
					break;

				case MessageType.RequestDeleteGroup:
					ProcessDeleteGroupRequest(ref reader, playerNumber);
					break;

				case MessageType.RequestGroupList:
					SendGroupList(playerNumber);
					break;

				case MessageType.GroupList:
					ProcessGroupList(ref reader);
					break;

				case MessageType.SetPlayerGroup:
					ProcessGroupPermissions(ref reader);
					break;

				case MessageType.RequestSetGroupPermissions:
					ProcessSetGroupPermissionsRequest(ref reader, playerNumber);
					break;

				case MessageType.RequestPlayerInfo:
					ProcessPlayerInfoRequest(ref reader, playerNumber);
					break;

				case MessageType.PlayerInfo:
					ProcessPlayerInfo(ref reader);
					break;

				case MessageType.RequestSetPlayerGroup:
					ProcessSetPlayerGroupRequest(ref reader, playerNumber);
					break;

				case MessageType.RequestSetOfflinePlayerGroup:
					ProcessSetOfflinePlayerGroupRequest(ref reader, playerNumber);
					break;

				case MessageType.ServerToClientHandshake:
					Network.ServerUsingHEROsMod = true;
					HEROsMod.ServiceHotbar.Visible = true;
					GeneralMessages.TellSereverImUsingHEROsMod();
					break;
			}
		}

		private static void WriteHeader(MessageType msgType)
		{
			Network.ResetWriter();
			Writer.Write((byte)Network.MessageType.LoginMessage);
			Writer.Write((byte)msgType);
		}

		public static void RequestLogin(string username, string password)
		{
			WriteHeader(MessageType.RequestLogin);
			Writer.Write(username);
			Writer.Write(password);
			Network.SendDataToServer();
		}

		public static void ProcessLoginRequest(ref BinaryReader reader, int playerNumber)
		{
			string username = reader.ReadString();
			string password = reader.ReadString();
			ProcessLoginRequest(username, password, playerNumber);
		}

		public static void ProcessLoginRequest(string username, string password, int playerNumber)
		{
			int groupID = 0;
			int playerID = 0;
			for (int i = 0; i < Network.Players.Length; i++)
			{
				if (Network.Players[i].Username.ToLower() == username.ToLower())
				{
					Network.SendTextToPlayer(HEROsMod.HeroText("AccountAlreadyLoggedIn"), playerNumber);
					return;
				}
			}
			if (DatabaseController.Login(ref username, password, ref playerID, ref groupID))
			{
				Network.Players[playerNumber].Username = username;
				Network.Players[playerNumber].ID = playerID;
				if (groupID == 0)
				{
					groupID = Network.DefaultGroup.ID;
					DatabaseController.SetPlayerGroup(playerID, groupID);
				}
				Network.Players[playerNumber].Group = Network.GetGroupByID(groupID);
				if (Network.Players[playerNumber].UsingHEROsMod)
					LoginSuccess(playerNumber);
				Network.SendTextToPlayer(string.Format(HEROsMod.HeroText("LoggedInSuccessfully"),  Network.Players[playerNumber].Group.Name), playerNumber, Color.Green);
			}
			else
			{
				Network.SendTextToPlayer(HEROsMod.HeroText("InvalidUsernameOrPassword"), playerNumber, Color.Red);
			}
		}

		private static void LoginSuccess(int playerNumber)
		{
			if (Network.NetworkMode == NetworkMode.Server)
			{
				if (Network.Players[playerNumber].Group == null)
				{
					Network.Players[playerNumber].Group = Network.DefaultGroup;
				}
				WriteHeader(MessageType.LoginSucess);
				Writer.Write(Network.Players[playerNumber].Group.ID);
				Network.SendDataToPlayer(playerNumber);
				SendPlayerPermissions(playerNumber);
			}
		}

		private static void ProcessLoginSuccess(ref BinaryReader reader)
		{
			if (Network.NetworkMode != NetworkMode.Server)
			{
				int id = reader.ReadInt32();
				Network.Players[Main.myPlayer].Group = Network.GetGroupByID(id);
				HEROsModServices.Login.LoggedIn = true;
			}
		}

		public static void RequestLogout()
		{
			if (Network.NetworkMode != NetworkMode.Server)
			{
				WriteHeader(MessageType.RequestLogout);
				Network.SendDataToServer();
			}
		}

		public static void ProcessLogoutRequest(int playerNumber)
		{
			if (Network.NetworkMode == NetworkMode.Server)
			{
				HEROsModPlayer player = Network.Players[playerNumber];
				player.Group = Network.DefaultGroup;
				player.Username = String.Empty;
				if (player.UsingHEROsMod)
				{
					WriteHeader(MessageType.LogoutSucess);
					Writer.Write(player.Group.ID);
					Network.SendDataToPlayer(playerNumber);
					SendPlayerPermissions(playerNumber);
				}
				if (Network.WillFreezeNonLoggedIn)
				{
					// Network.SendPlayerToPosition(player, new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16));
				}
			}
		}

		private static void ProcessLogoutSuccess(ref BinaryReader reader)
		{
			if (Network.NetworkMode != NetworkMode.Server)
			{
				int id = reader.ReadInt32();
				Network.Players[Main.myPlayer].Group = Network.GetGroupByID(id);
				HEROsModServices.Login.LoggedIn = false;
			}
		}

		public static void RequestRegistration(string username, string password)
		{
			WriteHeader(MessageType.RequestRegistration);
			Writer.Write(username);
			Writer.Write(password);
			Network.SendDataToServer();
		}

		private static void ReadRegistrationRequest(ref BinaryReader reader, int playernNumber)
		{
			string username = reader.ReadString();
			string password = reader.ReadString();
			ProcessRegistrationRequest(username, password, playernNumber);
		}

		public static void ProcessRegistrationRequest(string username, string password, int playerNumber)
		{
			DatabaseController.RegistrationResult regResult = DatabaseController.Register(username, password);
			switch (regResult)
			{
				case DatabaseController.RegistrationResult.Sucess:
					Network.SendTextToPlayer(HEROsMod.HeroText("SuccessfullyRegistered"), playerNumber);
					for (int i = 0; i < Network.Players.Length; i++)
					{
						HEROsModPlayer player = Network.Players[i];
						if (player.ServerInstance.IsActive && player.Group.IsAdmin)
						{
							GeneralMessages.SendRegisteredUsersToPlayer(i);
						}
					}
					break;

				case DatabaseController.RegistrationResult.UsernameTaken:
					Network.SendTextToPlayer(HEROsMod.HeroText("UsernameAlreadyTaken"), playerNumber);
					break;

				case DatabaseController.RegistrationResult.Error:
					Network.SendTextToPlayer("An error occured when trying to register.", playerNumber);
					break;
			}
		}

		public static void RequestAddGroup(string groupName)
		{
			WriteHeader(MessageType.RequestAddGroup);
			Writer.Write(groupName);
			Network.SendDataToServer();
		}

		private static void ProcessAddGroupReqest(ref BinaryReader reader, int playerNumber)
		{
			if (Network.NetworkMode == NetworkMode.Server)
			{
				if (!Network.Players[playerNumber].Group.IsAdmin) return;
				string newGroupName = reader.ReadString();
				for (int i = 0; i < Network.Groups.Count; i++)
				{
					//Check to make sure that group does not already exist
					if (Network.Groups[i].Name.ToLower() == newGroupName.ToLower())
					{
						Network.SendTextToPlayer(HEROsMod.HeroText("GroupAlreadyExists"), playerNumber);
						return;
					}
				}
				Group newGroup = new Group(newGroupName);
				DatabaseController.AddGroup(ref newGroup);
				Network.Groups.Add(newGroup);
				if (GroupChanged != null)
					GroupChanged(null, EventArgs.Empty);
			}
		}

		public static void RequestDeleteGroup(int groupID)
		{
			WriteHeader(MessageType.RequestDeleteGroup);
			Writer.Write(groupID);
			Network.SendDataToServer();
		}

		private static void ProcessDeleteGroupRequest(ref BinaryReader reader, int playerNumber)
		{
			if (Network.NetworkMode == NetworkMode.Server)
			{
				if (!Network.Players[playerNumber].Group.IsAdmin) return;
				Group groupToDelete = Network.GetGroupByID(reader.ReadInt32());
				if (groupToDelete != null)
				{
					if (groupToDelete.Name.ToLower() != "default")
					{
						for (int i = 0; i < Network.Players.Length; i++)
						{
							if (Network.Players[i].Group == groupToDelete)
							{
								Network.Players[i].Group = Network.DefaultGroup;
								LoginService.SendPlayerPermissions(i);
							}
						}
						DatabaseController.DeleteGroup(groupToDelete);
						Network.Groups.Remove(groupToDelete);
						if (GroupChanged != null)
							GroupChanged(null, EventArgs.Empty);
					}
					else
					{
						Network.SendTextToPlayer(HEROsMod.HeroText("CantDeleteDefaultGroup"), playerNumber);
					}
				}
				else
				{
					Network.SendTextToPlayer("Group could not be found", playerNumber);
				}
			}
		}

		public static void RequestGroupList()
		{
			WriteHeader(MessageType.RequestGroupList);
			Network.SendDataToServer();
		}

		public static void SendGroupList(int playerNumber)
		{
			if (Network.NetworkMode == NetworkMode.Server)
			{
				WriteHeader(MessageType.GroupList);
				int numOfGroups = Network.Groups.Count;
				Writer.Write(numOfGroups);
				for (int i = 0; i < Network.Groups.Count; i++)
				{
					Writer.Write(Network.Groups[i].Name);
					Writer.Write(Network.Groups[i].ID);
					Writer.WriteRGB(Network.Groups[i].Color);
					byte[] permissions = Network.Groups[i].ExportPermissions();
					Writer.Write(permissions.Length);
					Writer.Write(permissions);
				}
				Network.SendDataToPlayer(playerNumber);
			}
		}

		private static void ProcessGroupList(ref BinaryReader reader)
		{
			if (Network.NetworkMode != NetworkMode.Server)
			{
				Network.Groups.Clear();
				int numOfGroups = reader.ReadInt32();
				for (int i = 0; i < numOfGroups; i++)
				{
					string groupName = reader.ReadString();
					Group group = new Group(groupName);
					group.ID = reader.ReadInt32();
					group.Color = reader.ReadRGB();
					int permissionsLength = reader.ReadInt32();
					group.ImportPermissions(reader.ReadBytes(permissionsLength));
					Network.Groups.Add(group);
				}
				if (GroupChanged != null)
					GroupChanged(null, EventArgs.Empty);
			}
		}

		public static void SendAllPlayersPermissions()
		{
			for (int i = 0; i < Network.Players.Length; i++)
			{
				HEROsModPlayer player = Network.Players[i];
				if (player.ServerInstance.IsActive)
				{
					SendPlayerPermissions(i);
				}
			}
		}

		public static void SendPlayerPermissions(int playerNumber)
		{
			if (Network.NetworkMode == NetworkMode.Server)
			{
				WriteHeader(MessageType.SetPlayerGroup);
				HEROsModPlayer player = Network.Players[playerNumber];
				Group group = player.Group;
				Writer.Write(group.Name);
				Writer.Write(group.ID);
				Writer.Write(group.IsAdmin);
				Writer.WriteRGB(group.Color);
				byte[] permissions = group.ExportPermissions();
				Writer.Write(permissions.Length);
				Writer.Write(permissions);
				Network.SendDataToPlayer(playerNumber);

				if (group.IsAdmin) GeneralMessages.SendRegisteredUsersToPlayer(playerNumber);
			}
		}

		private static void ProcessGroupPermissions(ref BinaryReader reader)
		{
			string groupName = reader.ReadString();
			Group group = new Group(groupName);
			group.ID = reader.ReadInt32();
			bool isAdmin = reader.ReadBoolean();
			if (isAdmin)
			{
				group.ID = -1;
				group.IsAdmin = true;
				//group.MakeAdmin();
			}
			group.Color = reader.ReadRGB();
			int permissionsLength = reader.ReadInt32();
			group.ImportPermissions(reader.ReadBytes(permissionsLength));

			MyGroup = group;
			if (MyGroupChanged != null)
				MyGroupChanged(null, EventArgs.Empty);
		}

		public static void RequestSetGroupPermissions(Group group)
		{
			WriteHeader(MessageType.RequestSetGroupPermissions);
			Writer.Write(group.ID);
			byte[] permissions = group.ExportPermissions();
			Writer.Write(permissions.Length);
			Writer.Write(permissions);
			Writer.WriteRGB(group.Color);
			Network.SendDataToServer();
		}

		private static void ProcessSetGroupPermissionsRequest(ref BinaryReader reader, int playerNumber)
		{
			if (Network.NetworkMode == NetworkMode.Server)
			{
				if (!Network.Players[playerNumber].Group.IsAdmin) return;
				int id = reader.ReadInt32();
				Group group = Network.GetGroupByID(id);
				int permissionsLength = reader.ReadInt32();
				group.ImportPermissions(reader.ReadBytes(permissionsLength));
				group.Color = reader.ReadRGB();
				DatabaseController.SetGroupPermissions(group);

				for (int i = 0; i < Network.Players.Length; i++)
				{
					//if (Network.Players[i].Group?.ID == group.ID)
					if (Network.Players[i].Group == group)
						{
						SendPlayerPermissions(i);
					}
				}
				GroupChanged?.Invoke(null, EventArgs.Empty);
			}
		}

		public static void RequestPlayerInfo(int indexOfRequestedPlayer)
		{
			WriteHeader(MessageType.RequestPlayerInfo);
			Writer.Write(indexOfRequestedPlayer);
			Network.SendDataToServer();
		}

		private static void ProcessPlayerInfoRequest(ref BinaryReader reader, int playerNumber)
		{
			Group playerGroup = Network.Players[playerNumber].Group;
			if (playerGroup.IsAdmin)
			{
				int indexOfRequestedPlayer = reader.ReadInt32();
				SendPlayerInfo(indexOfRequestedPlayer, playerNumber);
			}
		}

		private static void SendPlayerInfo(int indexOfRequestedPlayer, int playerNumber)
		{
			WriteHeader(MessageType.PlayerInfo);
			HEROsModPlayer player = Network.Players[indexOfRequestedPlayer];
			Writer.Write(player.Username);
			Writer.Write(player.Group.ID);
			Writer.Write(indexOfRequestedPlayer);
			Network.SendDataToPlayer(playerNumber);
		}

		private static void ProcessPlayerInfo(ref BinaryReader reader)
		{
			string username = reader.ReadString();
			int groupID = reader.ReadInt32();
			int indexOfRequestedPlayer = reader.ReadInt32();
			Network.Players[indexOfRequestedPlayer].Username = username;
			Network.Players[indexOfRequestedPlayer].Group = Network.GetGroupByID(groupID);
			if (HEROsModServices.PlayerList.playersWindow != null)
			{
				HEROsModServices.PlayerList.playersWindow.OpenPlayerInfo(indexOfRequestedPlayer, false);
			}
		}

		public static void RequestSetPlayerGroup(int playerIndex, Group group)
		{
			WriteHeader(MessageType.RequestSetPlayerGroup);
			Writer.Write(playerIndex);
			Writer.Write(group.ID);
			Network.SendDataToServer();
		}

		private static void ProcessSetPlayerGroupRequest(ref BinaryReader reader, int playerNumber)
		{
			if (Network.Players[playerNumber].Group.IsAdmin)
			{
				int playerIndex = reader.ReadInt32();
				int groupID = reader.ReadInt32();

				Network.Players[playerIndex].Group = Network.GetGroupByID(groupID);
				SendPlayerPermissions(playerIndex);
				DatabaseController.SetPlayerGroup(Network.Players[playerIndex].ID, groupID);
			}
		}

		public static void RequestSetOfflinePlayerGroup(int playerIndex, Group group)
		{
			WriteHeader(MessageType.RequestSetOfflinePlayerGroup);
			Writer.Write(playerIndex);
			Writer.Write(group.ID);
			Network.SendDataToServer();
		}

		private static void ProcessSetOfflinePlayerGroupRequest(ref BinaryReader reader, int playerNumber)
		{
			if (Network.Players[playerNumber].Group.IsAdmin)
			{
				int id = reader.ReadInt32();
				int groupID = reader.ReadInt32();

				//Network.Players[id].Group = Network.GetGroupByID(groupID);
				//SendPlayerPermissions(id);
				DatabaseController.SetPlayerGroup(id, groupID);
				for (int i = 0; i < Network.Players.Length; i++)
				{
					HEROsModPlayer player = Network.Players[i];
					if (player.ServerInstance.IsActive && player.Group.IsAdmin)
					{
						GeneralMessages.SendRegisteredUsersToPlayer(i);
					}
				}
				//GeneralMessages.SendRegisteredUsersToPlayer(playerNumber);
			}
		}

		public enum MessageType
		{
			RequestLogin,
			LoginSucess,
			RequestLogout,
			LogoutSucess,
			RequestRegistration,
			RequestAddGroup,
			RequestDeleteGroup,
			RequestGroupList,
			GroupList,
			SetPlayerGroup,
			RequestSetGroupPermissions,
			RequestPlayerInfo,
			PlayerInfo,
			RequestSetPlayerGroup,
			RequestSetOfflinePlayerGroup,
			ServerToClientHandshake
		}
	}
}