//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiNetwork.CTF
//{
//    public class CTFMessages
//    {
//        public static event EventHandler PlayerChangedTeam;
//        static BinaryWriter Writer
//        {
//            get { return GameikiNetwork.Network.writer; }
//        }

//        private static void WriteHeader(MessageType msgType)
//        {
//            Network.ResetWriter();
//            Writer.Write((byte)Network.MessageType.CTFMessage);
//            Writer.Write((byte)msgType);
//        }

//        public static void ProcessData(ref BinaryReader reader, int playerNumber)
//        {
//            MessageType msgType = (MessageType)reader.ReadByte();
//            switch (msgType)
//            {
//                case MessageType.TeamObjectPosition:
//                    ProcessTeamObjectPosition(ref reader);
//                    break;
//                case MessageType.RequestPlaceFlagPlatform:
//                    ProcessPlaceFlagPlatformRequest(ref reader, playerNumber);
//                    break;
//                case MessageType.RequestPlaceSpawnPlatform:
//                    ProcessPlaceSpawnPlatformRequest(ref reader, playerNumber);
//                    break;
//                case MessageType.SendFlagToBase:
//                    ProcessFlagSentToBase(ref reader);
//                    break;
//                case MessageType.RequestPickupFlag:
//                    ProcessPickupFlagRequest(playerNumber);
//                    break;
//                case MessageType.RequestThrowFlag:
//                    ProcessThrowFlagRequest(playerNumber);
//                    break;
//                case MessageType.CarrierChanged:
//                    ProcessCarrierChange(ref reader);
//                    break;
//                case MessageType.RequestStartPregameLobby:
//                    ProcessStartPregameLobbyRequest(ref reader, playerNumber);
//                    break;
//                case MessageType.RequestStartGame:
//                    ProcessGameStartRequest(playerNumber);
//                    break;
//                case MessageType.RequestEndGame:
//                    ProcessEndGameRequest(playerNumber);
//                    break;
//                case MessageType.LobbyStarted:
//                    ProcessLobbyStarted(ref reader);
//                    break;
//                case MessageType.GameStarted:
//                    ProcessGameStarted();
//                    break;
//                case MessageType.GameEnded:
//                    ProcessGameEnded();
//                    break;
//                case MessageType.SendPlayerToSpawnPlatform:
//                    ProcessSentToSpawnPlatform();
//                    break;
//                case MessageType.SendCTFSettings:
//                    ProcessCTFSettings(ref reader);
//                    break;
//                case MessageType.RequestSwitchTeam:
//                    ProcessSwitchTeamRequest(ref reader, playerNumber);
//                    break;
//                case MessageType.PlayerChangedTeam:
//                    ProcessPlayerTeamChange(ref reader);
//                    break;
//                case MessageType.TeamList:
//                    ProcessTeamList(ref reader);
//                    break;
//                case MessageType.TeamScored:
//                    ProcessTeamScored(ref reader);
//                    break;
//                case MessageType.MakePlayerGhost:
//                    ProcessMakePlayerGhost(ref reader);
//                    break;
                    
//            }
//        }

//        public static void SendTeamOjectPosition(WorldObject teamObject)
//        {
//            SendTeamObjectPositionToPlayer(teamObject, -2);
//        }

//        public static void SendTeamObjectPositionToPlayer(WorldObject teamObject, int playerNumber)
//        {
//            TeamObject? objectType = null;
//            if(teamObject is Flag)
//            {
//                objectType = TeamObject.Flag;
//            }
//            else if(teamObject is FlagPlatform)
//            {
//                objectType = TeamObject.FlagPlatform;
//            }
//            else if(teamObject is SpawnPlatform)
//            {
//                objectType = TeamObject.SpawnPlatform;
//            }
//            if(objectType == null) return;
//            WriteHeader(MessageType.TeamObjectPosition);
//            Writer.Write((byte)((TeamObject)objectType));
//            Writer.Write((byte)teamObject.TeamColor);
//            Writer.WriteVector2(teamObject.Position);
//            Writer.WriteVector2(teamObject.Velocity);
//            Writer.Write(teamObject.Placed);
//            Network.SendDataToPlayer(playerNumber);
//        }

//        private static void ProcessTeamObjectPosition(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            TeamObject objectType = (TeamObject)reader.ReadByte();
//            TeamColor teamColor = (TeamColor)reader.ReadByte();
//            Team team = null;
//            switch(teamColor)
//            {
//                case TeamColor.Red:
//                    team = CaptureTheFlag.RedTeam;
//                    break;
//                case TeamColor.Blue:
//                    team = CaptureTheFlag.BlueTeam;
//                    break;
//            }
//            if(team == null)return;
//            WorldObject worldObject = null;
//            switch (objectType)
//            {
//                case TeamObject.Flag:
//                    worldObject = team.Flag;
//                    break;
//                case TeamObject.FlagPlatform:
//                    worldObject = team.FlagPlatform;
//                    break;
//                case TeamObject.SpawnPlatform:
//                    worldObject = team.SpawnPlatform;
//                    break;
//            }
//            if (worldObject == null) return;
//            worldObject.Position = reader.ReadVector2();
//            worldObject.Velocity = reader.ReadVector2();
//            worldObject.Placed = reader.ReadBoolean();
//            //Main.NewText("Got positionData", (byte)(Main.rand.Next() % 255), (byte)(Main.rand.Next() % 255), (byte)(Main.rand.Next() % 255));
//        }

//        public static void RequestPlaceFlagPlatform(TeamColor team, Vector2? position = null)
//        {
//            WriteHeader(MessageType.RequestPlaceFlagPlatform);
//            Writer.Write((byte)team);
//            Writer.Write(position != null);
//            if(position != null)
//            {
//                Writer.WriteVector2((Vector2)position);
//            }
//            Network.SendDataToServer();
//        }

//        private static void ProcessPlaceFlagPlatformRequest(ref BinaryReader reader, int playerNumber)
//        {
//            if(Network.Players[playerNumber].Group.HasPermission("StartCTF"))
//            {
//                TeamColor team = (TeamColor)reader.ReadByte();
//                Vector2 placePosition = Main.player[playerNumber].Center;
//                bool setPosition = reader.ReadBoolean();
//                if(setPosition)
//                {
//                    placePosition = reader.ReadVector2();
//                }
//                switch(team)
//                {
//                    case TeamColor.Red:
//                        CaptureTheFlag.RedTeam.PlaceFlagPlatform(placePosition);
//                        break;
//                    case TeamColor.Blue:
//                        CaptureTheFlag.BlueTeam.PlaceFlagPlatform(placePosition);
//                        break;    
//                }
//            }
//        }

//        public static void RequestPlaceSpawnPlatform(TeamColor team, Vector2? position = null)
//        {
//            WriteHeader(MessageType.RequestPlaceSpawnPlatform);
//            Writer.Write((byte)team);
//            Writer.Write(position != null);
//            if (position != null)
//            {
//                Writer.WriteVector2((Vector2)position);
//            }
//            Network.SendDataToServer();
//        }

//        private static void ProcessPlaceSpawnPlatformRequest(ref BinaryReader reader, int playerNumber)
//        {
//            if (Network.Players[playerNumber].Group.HasPermission("StartCTF"))
//            {
//                TeamColor team = (TeamColor)reader.ReadByte(); 
//                Vector2 placePosition = Main.player[playerNumber].Center;
//                bool setPosition = reader.ReadBoolean();
//                if (setPosition)
//                {
//                    placePosition = reader.ReadVector2();
//                }
//                switch (team)
//                {
//                    case TeamColor.Red:
//                        CaptureTheFlag.RedTeam.PlaceSpawnPlatform(placePosition);
//                        break;
//                    case TeamColor.Blue:
//                        CaptureTheFlag.BlueTeam.PlaceSpawnPlatform(placePosition);
//                        break;
//                }
//            }
//        }

//        public static void SendFlagToBase(TeamColor team)
//        {
//            WriteHeader(MessageType.SendFlagToBase);
//            Writer.Write((byte)team);
//            Network.SendDataToAllGameikiUsers();
//        }

//        private static void ProcessFlagSentToBase(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            TeamColor team = (TeamColor)reader.ReadByte();
//            switch (team)
//            {
//                case TeamColor.Red:
//                    CaptureTheFlag.RedTeam.SendFlagToBase();
//                    break;
//                case TeamColor.Blue:
//                    CaptureTheFlag.BlueTeam.SendFlagToBase();
//                    break;
//            }
//        }

//        public static void RequestPickupFlag()
//        {
//            WriteHeader(MessageType.RequestPickupFlag);
//            Network.SendDataToServer();
//        }

//        private static void ProcessPickupFlagRequest(int playerNumber)
//        {
//            GameikiPlayer player = Network.Players[playerNumber];
//            Flag flag = null;
//            switch(player.CTFTeam)
//            {
//                case TeamColor.Red:
//                    flag = CaptureTheFlag.BlueTeam.Flag;
//                    break;
//                case TeamColor.Blue:
//                    flag = CaptureTheFlag.RedTeam.Flag;
//                    break;
//            }
//            if (flag == null || flag.Carrier != null || player.GameInstance.dead) return;
//            float distanceToFlag = Vector2.Distance(player.GameInstance.Center, flag.Center);
//            if(distanceToFlag < Flag.PickupDistance + 10)
//            {
//                flag.Pickup(player);
//            }
//        }

//        public static void RequestThrowFlag()
//        {
//            WriteHeader(MessageType.RequestThrowFlag);
//            Network.SendDataToServer();
//        }

//        private static void ProcessThrowFlagRequest(int playerNumber)
//        {
//            GameikiPlayer player = Network.Players[playerNumber];
//            Flag flag = null;
//            switch (player.CTFTeam)
//            {
//                case TeamColor.Red:
//                    flag = CaptureTheFlag.BlueTeam.Flag;
//                    break;
//                case TeamColor.Blue:
//                    flag = CaptureTheFlag.RedTeam.Flag;
//                    break;
//            }
//            if (flag == null || flag.Carrier != player) return;
//            flag.Throw();
//        }

//        public static void CarrierChanged(Flag flag)
//        {
//            WriteHeader(MessageType.CarrierChanged);
//            Writer.Write((byte)flag.TeamColor);
//            int carrierIndex = -1;
//            if(flag.Carrier != null)
//            {
//                carrierIndex = flag.Carrier.Index;
//            }
//            Writer.Write(carrierIndex);
//            Network.SendDataToAllGameikiUsers();
//            SendTeamObjectPositionToPlayer(flag, -2);
//        }

//        private static void ProcessCarrierChange(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            TeamColor teamColor = (TeamColor)reader.ReadByte();
//            Flag flag = null;
//            switch (teamColor)
//            {
//                case TeamColor.Red:
//                    flag = CaptureTheFlag.RedTeam.Flag;
//                    break;
//                case TeamColor.Blue:
//                    flag = CaptureTheFlag.BlueTeam.Flag;
//                    break;
//            }
//            if (flag == null) return;
//            int carrierIndex = reader.ReadInt32();
//            if(carrierIndex < 0)
//            {
//                flag.Carrier = null;
//            }
//            else
//            {
//                flag.Carrier = Network.Players[carrierIndex];
//            }
//        }

//        public static void RequestStartLobby(byte[] settings)
//        {
//            WriteHeader(MessageType.RequestStartPregameLobby);
//            Writer.Write(settings);
//            Network.SendDataToServer();
//        }

//        private static void ProcessStartPregameLobbyRequest(ref BinaryReader reader, int playerNumber)
//        {
//            if (CaptureTheFlag.GameInProgress || CaptureTheFlag.InPregameLobby) return;
//            if(Network.Players[playerNumber].Group.HasPermission("StartCTF"))
//            {
//                if(!CaptureTheFlag.CanStart)
//                {
//                    Network.SendTextToPlayer("You must place both flag platforms and spawn platforms to start the game.", playerNumber);
//                    return;
//                }
//                CaptureTheFlag.RequiresOwnFlagToScore = reader.ReadBoolean();
//                CaptureTheFlag.ReplacePlayerInventory = reader.ReadBoolean();
//                CaptureTheFlag.AllowTerrainModification = reader.ReadBoolean();

//                CaptureTheFlag.ScoreToWin = reader.ReadByte();
//                CaptureTheFlag.FlagRespawnTime = reader.ReadInt32();
//                int startingHealth = reader.ReadInt32();
//                CaptureTheFlag.StartingHealth = null;
//                if(startingHealth >= 0)
//                {
//                    CaptureTheFlag.StartingHealth = startingHealth;
//                }
//                int startingMana = reader.ReadInt32();
//                CaptureTheFlag.StartingMana = null;
//                if (startingMana >= 0)
//                {
//                    CaptureTheFlag.StartingMana = startingMana;
//                }
//                CaptureTheFlag.CrossingTimer = reader.ReadInt32();
//                CaptureTheFlag.ForceMediumCore = reader.ReadBoolean();

//                if(CaptureTheFlag.ReplacePlayerInventory)
//                {
//                    Item[] startingItems = new Item[50];
//                    for(int i = 0;i < startingItems.Length; i++)
//                    {
//                        startingItems[i] = new Item();
//                        startingItems[i].SetDefaults(reader.ReadInt32());
//                        startingItems[i].stack = reader.ReadInt32();
//                        startingItems[i].prefix = reader.ReadByte();
//                    }
//                    CaptureTheFlag.StartingItems = startingItems;
//                }
//                CaptureTheFlag.StartLobby(Network.Players[playerNumber]);
//            }
//        }

//        public static void TellClientLobbyStarted(int playerIndex)
//        {
//            WriteHeader(MessageType.LobbyStarted);
//            Writer.Write(CaptureTheFlag.LobbyStartedBy);
//            Network.SendDataToPlayer(playerIndex);
//        }

//        public static void TellAllClientsLobbyStarted()
//        {
//            TellClientLobbyStarted(-2);
//        }

//        private static void ProcessLobbyStarted(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            int startedByIndex = reader.ReadInt32();
//            CaptureTheFlag.StartLobby(Network.Players[startedByIndex]);
//        }

//        public static void RequestStartGame()
//        {
//            WriteHeader(MessageType.RequestStartGame);
//            Network.SendDataToServer();
//        }

//        private static void ProcessGameStartRequest(int playerNumber)
//        {
//            if(Network.Players[playerNumber].Group.HasPermission("StartCTF"))
//            {
//                CaptureTheFlag.StartGame();
//            }
//        }

//        public static void TellClientGameStarted(int playerIndex)
//        {

//            WriteHeader(MessageType.GameStarted);
//            Network.SendDataToPlayer(playerIndex);
//        }

//        public static void TellAllClientsGameStarted()
//        {
//            TellClientGameStarted(-2);
//        }

//        private static void ProcessGameStarted()
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            CaptureTheFlag.StartGame();
//        }

//        public static void RequestEndGame()
//        {
//            WriteHeader(MessageType.RequestEndGame);
//            Network.SendDataToServer();
//        }

//        private static void ProcessEndGameRequest(int playerNumber)
//        {
//            if (Network.Players[playerNumber].Group.HasPermission("StartCTF"))
//            {
//                CaptureTheFlag.EndGame();
//            }
//        }

//        public static void TellClientGameEnded(int playerNumber)
//        {

//            WriteHeader(MessageType.GameEnded);
//            Network.SendDataToPlayer(playerNumber);
//        }

//        public static void TellAllClientsGameEnded()
//        {
//            TellClientGameEnded(-2);
//        }

//        private static void ProcessGameEnded()
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            CaptureTheFlag.EndGame();
//        }

//        public static void SendPlayerToSpawnPlatform(GameikiPlayer player)
//        {
//            if(player.CTFTeam == TeamColor.None) return;
//            WriteHeader(MessageType.SendPlayerToSpawnPlatform);
//            Network.SendDataToPlayer(player.Index);
            
//        }

//        private static void ProcessSentToSpawnPlatform()
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            GameikiPlayer myPlayer = Network.Players[Main.myPlayer];

//            SpawnPlatform platform = null;
//            switch (myPlayer.CTFTeam)
//            {
//                case TeamColor.Red:
//                    platform = CaptureTheFlag.RedTeam.SpawnPlatform;
//                    break;
//                case TeamColor.Blue:
//                    platform = CaptureTheFlag.BlueTeam.SpawnPlatform;
//                    break;
//            }
//            if (platform == null) return;
//            float Y = platform.Position.Y - myPlayer.GameInstance.height + platform.Height;
//            float X = platform.Position.X + platform.Width / 2 - myPlayer.GameInstance.width / 2;
//            myPlayer.GameInstance.Teleport(new Vector2(X, Y));
//            NetMessage.SendData(13, -1, -1, "", Main.myPlayer, 0f, 0f, 0f, 0);
//            myPlayer.GameInstance.statLife = myPlayer.GameInstance.statLifeMax;
//            myPlayer.GameInstance.statMana = myPlayer.GameInstance.statManaMax;
//            NetMessage.SendData(16, -1, -1, "", Main.myPlayer, 0f, 0f, 0f, 0);
//        }

//        public static void SendCTFSettings(int playerNumber)
//        {
//            WriteHeader(MessageType.SendCTFSettings);
//            Writer.Write(CaptureTheFlag.LobbyStartedBy);
//            Writer.Write(CaptureTheFlag.GameInProgress);
//            Writer.Write(CaptureTheFlag.InPregameLobby);
//            Writer.Write(CaptureTheFlag.RequiresOwnFlagToScore);
//            Writer.Write(CaptureTheFlag.ReplacePlayerInventory);
//            Writer.Write(CaptureTheFlag.AllowTerrainModification);
//            Writer.Write(CaptureTheFlag.ScoreToWin);
//            Writer.Write(CaptureTheFlag.FlagRespawnTime);
//            Writer.Write(CaptureTheFlag.CrossingTimer);

//            int startingHealth = -1;
//            if (CaptureTheFlag.StartingHealth != null)
//            {
//                startingHealth = (int)CaptureTheFlag.StartingHealth;
//            }
//            Writer.Write(startingHealth);
//            int startingMana = -1;
//            if (CaptureTheFlag.StartingMana != null)
//            {
//                startingMana = (int)CaptureTheFlag.StartingMana;
//            }
//            Writer.Write(startingMana);
//            Writer.Write(CaptureTheFlag.ForceMediumCore);
//            if (CaptureTheFlag.ReplacePlayerInventory)
//            {
//                Item[] startingItems = new Item[50];
//                for (int i = 0; i < CaptureTheFlag.StartingItems.Length; i++)
//                {
//                    Writer.Write(CaptureTheFlag.StartingItems[i].netID);
//                    Writer.Write(CaptureTheFlag.StartingItems[i].stack);
//                    Writer.Write(CaptureTheFlag.StartingItems[i].prefix);
//                }
//            }
//            Writer.Write(CaptureTheFlag.RedTeam.Flag.RespawnTimer);
//            Writer.Write(CaptureTheFlag.BlueTeam.Flag.RespawnTimer);
//            Writer.Write(CaptureTheFlag.RedTeam.Score);
//            Writer.Write(CaptureTheFlag.BlueTeam.Score);
//            Network.SendDataToPlayer(playerNumber);
//        }

//        private static void ProcessCTFSettings(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            CaptureTheFlag.LobbyStartedBy = reader.ReadInt32();
//            CaptureTheFlag.GameInProgress = reader.ReadBoolean();
//            CaptureTheFlag.InPregameLobby = reader.ReadBoolean();
//            CaptureTheFlag.RequiresOwnFlagToScore = reader.ReadBoolean();
//            CaptureTheFlag.ReplacePlayerInventory = reader.ReadBoolean();
//            CaptureTheFlag.AllowTerrainModification = reader.ReadBoolean();

//            CaptureTheFlag.ScoreToWin = reader.ReadInt32();
//            CaptureTheFlag.FlagRespawnTime = reader.ReadInt32();
//            CaptureTheFlag.CrossingTimer = reader.ReadSingle();

//            int startingHealth = reader.ReadInt32();
//            CaptureTheFlag.StartingHealth = null;
//            if (startingHealth >= 0)
//            {
//                CaptureTheFlag.StartingHealth = startingHealth;
//            }
//            int startingMana = reader.ReadInt32();
//            CaptureTheFlag.StartingMana = null;
//            if (startingMana >= 0)
//            {
//                CaptureTheFlag.StartingMana = startingMana;
//            }
//            CaptureTheFlag.ForceMediumCore = reader.ReadBoolean();

//            if (CaptureTheFlag.ReplacePlayerInventory)
//            {
//                Item[] startingItems = new Item[50];
//                for (int i = 0; i < startingItems.Length; i++)
//                {
//                    startingItems[i] = new Item();
//                    startingItems[i].SetDefaults(reader.ReadInt32());
//                    startingItems[i].stack = reader.ReadInt32();
//                    startingItems[i].prefix = reader.ReadByte();
//                }
//                CaptureTheFlag.StartingItems = startingItems;
//            }
//            CaptureTheFlag.RedTeam.Flag.RespawnTimer = reader.ReadSingle();
//            CaptureTheFlag.BlueTeam.Flag.RespawnTimer = reader.ReadSingle();
//            CaptureTheFlag.RedTeam.Score = reader.ReadInt32();
//            CaptureTheFlag.BlueTeam.Score = reader.ReadInt32();
//        }

//        public static void RequestSwitchTeam(TeamColor teamColor)
//        {
//            WriteHeader(MessageType.RequestSwitchTeam);
//            Writer.Write((byte)teamColor);
//            Network.SendDataToServer();
//        }

//        public static void ProcessSwitchTeamRequest(ref BinaryReader reader, int playerNumber)
//        {
//            if (!CaptureTheFlag.InPregameLobby && !CaptureTheFlag.GameInProgress) return;
//            TeamColor teamColor = (TeamColor)reader.ReadByte();
//            ChangePlayerTeam(Network.Players[playerNumber], teamColor);
//        }

//        public static void ChangePlayerTeam(GameikiPlayer player, TeamColor teamColor)
//        {
//            WriteHeader(MessageType.PlayerChangedTeam);
//            player.CTFTeam = teamColor;
//            Writer.Write(player.Index);
//            Writer.Write((byte)player.CTFTeam);
//            if(CaptureTheFlag.GameInProgress)
//            {
//                CTF.CaptureTheFlag.SetPlayerHostility(player);
//            }
//            Network.SendDataToAllGameikiUsers();
//        }

//        private static void ProcessPlayerTeamChange(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            GameikiPlayer player = Network.Players[reader.ReadInt32()];
//            player.CTFTeam = (TeamColor)reader.ReadByte();
            
//            if(CaptureTheFlag.GameInProgress)
//            {
//                if (player.CTFTeam == TeamColor.None)
//                {
//                    if (player.Index == Main.myPlayer)
//                    {
//                        player.GameInstance.ghost = true;
//                    }
//                    else
//                    {
//                        player.GameInstance.ghost = true;
//                        player.GameInstance.active = false;
//                    }
//                }
//                else
//                {
//                    if (player.Index == Main.myPlayer)
//                    {
//                        player.GameInstance.ghost = false;
//                    }
//                    else
//                    {
//                        player.GameInstance.ghost = false;
//                        player.GameInstance.active = true;
//                    }
//                }
//            }

//            if (PlayerChangedTeam != null)
//            {
//                PlayerChangedTeam(player, EventArgs.Empty);
//            }
//        }

//        public static void SendTeamListToPlayer(int playerIndex)
//        {
//            WriteHeader(MessageType.TeamList);
//            List<GameikiPlayer> activePlayers = new List<GameikiPlayer>();
//            for(int i = 0; i < Network.Players.Length; i++)
//            {
//                GameikiPlayer player = Network.Players[i];
//                if(player.ServerInstance.IsActive)
//                {
//                    activePlayers.Add(player);
//                }
//            }
//            Writer.Write(activePlayers.Count);
//            for(int i = 0;i < activePlayers.Count; i++)
//            {
//                GameikiPlayer player = activePlayers[i];
//                Writer.Write(player.Index);
//                Writer.Write((byte)player.CTFTeam);
//            }
//            Network.SendDataToPlayer(playerIndex);
//        }

//        private static void ProcessTeamList(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            int numberOfActivePlayer = reader.ReadInt32();
//            for(int i = 0;i < numberOfActivePlayer; i++)
//            {
//                int playerIndex = reader.ReadInt32();
//                Network.Players[playerIndex].CTFTeam = (TeamColor)reader.ReadByte();
//            }
//            if (PlayerChangedTeam != null)
//            {
//                PlayerChangedTeam(null, EventArgs.Empty);
//            }
//        }

//        public static void TellClientsTeamScored(TeamColor color)
//        {
//            WriteHeader(MessageType.TeamScored);
//            Writer.Write((byte)color);
//            Network.SendDataToAllGameikiUsers();
//        }

//        private static void ProcessTeamScored(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            TeamColor teamColor = (TeamColor)reader.ReadByte();
//            switch(teamColor)
//            {
//                case TeamColor.Red:
//                    CaptureTheFlag.RedTeam.Score++;
//                    break;
//                case TeamColor.Blue:
//                    CaptureTheFlag.BlueTeam.Score++;
//                    break;
//            }
//        }

//        public static void MakePlayerGhost(GameikiPlayer player)
//        {
//            WriteHeader(MessageType.MakePlayerGhost);
//            Writer.Write(player.Index);
//            Network.SendDataToAllGameikiUsers();
//        }

//        private static void ProcessMakePlayerGhost(ref BinaryReader reader)
//        {
//            if (Network.NetworkMode == NetworkMode.Server) return;
//            int playerIndex = reader.ReadInt32();
//            if(playerIndex == Main.myPlayer)
//            {
//                Main.player[playerIndex].ghost = true;
//            }
//            else
//            {
//                Main.player[playerIndex].active = false;
//            }
//        }

//        public enum MessageType
//        {
//            TeamObjectPosition,
//            RequestPlaceFlagPlatform,
//            RequestPlaceSpawnPlatform,
//            SendFlagToBase,
//            RequestPickupFlag,
//            RequestThrowFlag,
//            CarrierChanged,
//            RequestStartPregameLobby,
//            RequestStartGame,
//            RequestEndGame,
//            LobbyStarted,
//            GameStarted,
//            GameEnded,
//            SendPlayerToSpawnPlatform,
//            SendCTFSettings,
//            RequestSwitchTeam,
//            PlayerChangedTeam,
//            TeamList,
//            TeamScored,
//            MakePlayerGhost
//        }
//    }

//    public enum TeamObject
//    {
//        Flag,
//        FlagPlatform,
//        SpawnPlatform
//    }

//    public enum TeamColor
//    {
//        None,
//        Red,
//        Blue
//    }
//}
