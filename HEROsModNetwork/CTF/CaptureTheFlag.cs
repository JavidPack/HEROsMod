//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using GameikiMod.GameikiServices;
//using GameikiMod.UIKit;

//using Terraria;

//namespace GameikiMod.GameikiNetwork.CTF
//{
//    class CaptureTheFlag
//    {
//        public static Team RedTeam { get; set; }
//        public static Team BlueTeam { get; set; }

//        public static UILabel TipLabel;

//        public static KeyBinding PickUpFlagBinding;

//        private static Item[] _backedUpItems;
//        private static Vector2 _backedUpPosition;
//        private static int _backedUpHealth;
//        private static int _backedUpMana;

//        public static event EventHandler LobbyStarted;
//        public static event EventHandler GameStarted;
//        public static event EventHandler GameEnded;
//        public static event EventHandler PlayerChangedTeam;


//        public static bool GameInProgress { get; set; }
//        public static bool InPregameLobby { get; set; }
//        public static int LobbyStartedBy { get; set; }
//        public static bool RequiresOwnFlagToScore { get; set; }
//        public static int ScoreToWin { get; set; }
//        public static int FlagRespawnTime { get; set; }
//        public static bool ReplacePlayerInventory { get; set; }
//        public static bool ForceMediumCore { get; set; }
//        public static bool AllowTerrainModification { get; set;}
//        public static int? StartingHealth { get; set; }
//        public static int? StartingMana { get; set; }
//        public static Item[] StartingItems { get; set; }
//        public static bool AllowCrossingSides 
//        {
//            get
//            {
//                return CrossingTimer <= 0;
//            }
//        }
//        public static float DividerPosition { get; set; }
//        public static float CrossingTimer { get; set; }
//        public static Tile[,] ModifiedTiles { get; set; }
//        public static bool ListeningForTileChanges { get; set; }

//        public static bool CanStart
//        {
//            get
//            {
//                return (RedTeam.FlagPlatform.Placed && RedTeam.SpawnPlatform.Placed
//                    && BlueTeam.FlagPlatform.Placed && BlueTeam.SpawnPlatform.Placed);
//            }
//        }

//        public static void Init()
//        {

//            RedTeam = new Team(TeamColor.Red);
//            BlueTeam = new Team(TeamColor.Blue);

//            ResetSettings();

//            if (Network.NetworkMode != NetworkMode.Server)
//            {
//                TipLabel = new UILabel();
//                TipLabel.Anchor = AnchorPosition.Top;
//                TipLabel.Visible = false;
//                TipLabel.Scale = .5f;
//                UIKit.MasterView.gameScreen.AddChild(TipLabel);
//                //KeybindController.SetCatetory("Capture the Flag");
//                PickUpFlagBinding = KeybindController.AddKeyBinding("Pickup/Throw Flag", "F");
//                CTFMessages.PlayerChangedTeam += CTFMessages_PlayerChangedTeam;
//            }
//            else
//            {
//                RedTeam.Scored += Team_Scored;
//                BlueTeam.Scored += Team_Scored;
//            }
//        }

//        static void Team_Scored(object sender, EventArgs e)
//        {
//            if(sender == RedTeam)
//            {
//                Network.SendTextToAllPlayers("Red Team has scored!", Color.Red);
//                CTFMessages.TellClientsTeamScored(TeamColor.Red);
//                if(RedTeam.Score >= ScoreToWin)
//                {
//                    Network.SendTextToAllPlayers("Red Team Wins!", Color.Red);
//                    EndGame();
//                }
//            }
//            else if(sender == BlueTeam)
//            {
//                Network.SendTextToAllPlayers("Blue Team has scored!", Color.Blue);
//                CTFMessages.TellClientsTeamScored(TeamColor.Blue);
//                if (BlueTeam.Score >= ScoreToWin)
//                {
//                    Network.SendTextToAllPlayers("Blue Team Wins!", Color.Blue);
//                    EndGame();
//                }
//            }
//        }

//        static void CTFMessages_PlayerChangedTeam(object sender, EventArgs e)
//        {
//            if(CaptureTheFlag.GameInProgress)
//            {
//                GameikiPlayer myPlayer = Network.Players[Main.myPlayer];
//                if(sender == myPlayer)
//                {
//                    if(myPlayer.CTFTeam != TeamColor.None)
//                    {
//                        string killMessage = string.Empty;
//                        if(myPlayer.CTFTeam == TeamColor.Red)
//                        {
//                            killMessage = " switched to the red team.";
//                        }
//                        else if(myPlayer.CTFTeam == TeamColor.Blue)
//                        {
//                            killMessage = " switched to the blue team.";
//                        }
//                        myPlayer.GameInstance.statLife = 0;
//                        myPlayer.GameInstance.KillMe(0, 1, false, killMessage);
//                    }
//                }

//            }
//            if(PlayerChangedTeam != null)
//            {
//                PlayerChangedTeam(sender, e);
//            }
//        }

//        public static void ResetSettings()
//        {
//            GameInProgress = false;
//            InPregameLobby = false;
//            LobbyStartedBy = -1;
//            RequiresOwnFlagToScore = true;
//            ScoreToWin = 3;
//            FlagRespawnTime = 10;
//            ReplacePlayerInventory = false;
//            ForceMediumCore = false;
//            AllowTerrainModification = true;
//            StartingHealth = null;
//            StartingMana = null;
//            StartingItems = null;
//            RedTeam.Score = 0;
//            BlueTeam.Score = 0;
//            CrossingTimer = 0f;
//            ListeningForTileChanges = false;
//        }

//        public static void UnplaceWorldObjects()
//        {
//            RedTeam.Flag.Placed = false;
//            BlueTeam.Flag.Placed = false;
//            RedTeam.FlagPlatform.Placed = false;
//            BlueTeam.FlagPlatform.Placed = false;
//            RedTeam.SpawnPlatform.Placed = false;
//            BlueTeam.SpawnPlatform.Placed = false;
//        }

//        public static void Update()
//        {
//            if (Network.NetworkMode != NetworkMode.Server)
//            {
//                TipLabel.Visible = false;
//                TipLabel.CenterXAxisToParentCenter();
//                TipLabel.Y = Main.screenHeight / 2 + 50;
//            }
//            else
//            {
//                if(!AllowCrossingSides)
//                {
//                    CheckPlayerCrossingSides();
//                }
//            }

//            /** Subtract from crossing Timer */
//            if (GameInProgress && CrossingTimer > 0)
//            {
//                CrossingTimer -= ModUtils.DeltaTime;
//                // If the crossing timer hits zero, AllowCrossing becomes true
//                // and players will be allowed to move into enemy territory
//            }
            
//            RedTeam.Update();
//            BlueTeam.Update();
//        }

//        public static void StartLobby(GameikiPlayer starter)
//        {
//            LobbyStartedBy = starter.Index;
//            InPregameLobby = true;
            
//            if(Network.NetworkMode == NetworkMode.Server)
//            {
//                CTFMessages.TellAllClientsLobbyStarted();
//                CTFMessages.SendCTFSettings(-2);
//            }

//            if(LobbyStarted != null)
//            {
//                LobbyStarted(null, EventArgs.Empty);
//            }
//        }

//        public static void StartGame()
//        {
//            InPregameLobby = false;
//            GameInProgress = true;
//            Main.ServerSideCharacter = true;
//            DividerPosition = RedTeam.FlagPlatform.Position.X + ((BlueTeam.FlagPlatform.Position.X - RedTeam.FlagPlatform.Position.X) / 2);

//            if(Network.NetworkMode == NetworkMode.Server)
//            {
//                BackupHostility();
//                SetHostilityForAllPlayers();
//                CTFMessages.TellAllClientsGameStarted();
//                SendPlayersToSpawnPlatforms();
//                LoginService.SendAllPlayersPermissions();
//                StartListeningForModifiedBlock();
//                Network.GravestonesAllowed = false;
                
//            }
//            else if(Network.NetworkMode == NetworkMode.Client)
//            {
//                BackupPlayerState();
//                ClearBuffs();
//                GameikiServices.GodModeService.Enabled = false;
//                if(ReplacePlayerInventory)
//                {
//                    ClearInventory();
//                    SetItems(StartingItems);
//                }
//                Player player = Main.player[Main.myPlayer];
//                if(StartingHealth != null)
//                {
//                    player.statLifeMax = (int)StartingHealth;
//                    player.statLife = player.statLifeMax;
//                }
//                if (StartingMana != null)
//                {
//                    player.statManaMax = (int)StartingMana;
//                    player.statMana = player.statManaMax;
//                }
//            }

//            if(GameStarted != null)
//            {
//                GameStarted(null, EventArgs.Empty);
//            }
//        }

//        public static void EndGame()
//        {
//            if (Network.NetworkMode == NetworkMode.Client)
//            {
//                if (GameInProgress)
//                {
//                    RestorePlayerState();
//                }
//                Main.player[Main.myPlayer].ghost = false;
//            }

//            if(Network.NetworkMode == NetworkMode.Server)
//            {
//                if(GameInProgress)
//                {
//                    RestoreHostility();
//                }
//                LobbyStartedBy = -1;
//                ResetSettings();
//                RedTeam.SendFlagToBase();
//                BlueTeam.SendFlagToBase();
//                CTFMessages.TellAllClientsGameEnded();
//                LoginService.SendAllPlayersPermissions();
//                SetAllPlayerTeamsToNone();
//                NetMessage.syncPlayers();
//                StopListeningForModifiedBlocks();
//                RestoreModifiedTiles();
//                Network.GravestonesAllowed = true;
//            }
//            GameInProgress = false;
//            Main.ServerSideCharacter = false;
            
            
//            if(GameEnded != null)
//            {
//                GameEnded(null, EventArgs.Empty);
//            }
//        }

//        public static void SendPlayersToSpawnPlatforms()
//        {
//            for(int i = 0;i < Network.Players.Length; i++)
//            {
//                GameikiPlayer player = Network.Players[i];
//                if(player.ServerInstance.IsActive)
//                {
//                    if(player.CTFTeam != TeamColor.None)
//                    {
//                        CTFMessages.SendPlayerToSpawnPlatform(player);
//                    }
//                }
//            }
//        }

//        public static void Draw(SpriteBatch spriteBatch)
//        {
//            RedTeam.Draw(spriteBatch);
//            BlueTeam.Draw(spriteBatch);
//            if(GameInProgress && !AllowCrossingSides)
//            {
//                DrawDivider(spriteBatch);
//            }
//        }

//        public static void SetAllPlayerTeamsToNone()
//        {
//            for (int i = 0; i < Network.Players.Length; i++)
//            {
//                if (Network.Players[i].ServerInstance.IsActive)
//                {
//                    CTFMessages.ChangePlayerTeam(Network.Players[i], TeamColor.None);
//                }
//            }
//        }

//        public static void BackupPlayerState()
//        {
//            Player player = Main.player[Main.myPlayer];
//            _backedUpPosition = player.position;
//            _backedUpHealth = player.statLifeMax;
//            _backedUpMana = player.statManaMax;
//            _backedUpItems = new Item[83];
//            for (int j = 0; j < _backedUpItems.Length; j++)
//            {
//                if (j < 59)
//                {
//                    _backedUpItems[j] = player.inventory[j].Clone();
//                }
//                else
//                {
//                    if(j > 82)
//                    {
//						_backedUpItems[j] = player.trashItem;//Main.trashItem;
//                    }
//                    if (j >= 75 && j <= 82)
//                    {
//                        int index = j - 58 - 17;
//                        _backedUpItems[j] = player.dye[index].Clone();
//                    }
//                    else
//                    {
//                        int index = j - 58 - 1;
//                        _backedUpItems[j] = player.armor[index].Clone();
//                    }
//                }
//            }
//        }

//        public static void RestorePlayerState()
//        {
//            Player player = Main.player[Main.myPlayer];
//            player.Teleport(_backedUpPosition);
//            SetItems(_backedUpItems);

//            player.statLifeMax = _backedUpHealth;
//            player.statManaMax = _backedUpMana;
//            NetMessage.SendData(16, -1, -1, "", Main.myPlayer, 0f, 0f, 0f, 0);
//        }

//        public static void ClearInventory()
//        {
//            Player player = Main.player[Main.myPlayer];
//            for (int j = 0; j < 84; j++)
//            {
//                if (j < 59)
//                {
//                    player.inventory[j].SetDefaults(0);
//                }
//                else
//                {
//                    if(j > 82)
//                    {
//                        player.trashItem.SetDefaults(0);
//                    }
//                    else if (j >= 75 && j <= 82)
//                    {
//                        int index = j - 58 - 17;
//                        player.dye[index].SetDefaults(0);
//                    }
//                    else
//                    {
//                        int index = j - 58 - 1;
//                        player.armor[index].SetDefaults(0);
//                    }
//                }
//            }
//        }

//        public static void SetItems(Item[] items)
//        {
//            Player player = Main.player[Main.myPlayer];
//            for (int i = 0; i < items.Length; i++)
//            {
//                if (i < 59)
//                {
//                    player.inventory[i].netDefaults(items[i].netID);
//                    player.inventory[i].prefix = items[i].prefix;
//                    player.inventory[i].stack = items[i].stack;
//                }
//                else
//                {
//                    if(i > 82)
//                    {
//                        player.trashItem.SetDefaults(items[i].netID);
//                        player.trashItem.prefix = items[i].prefix;
//                        player.trashItem.stack = items[i].stack;
//                    }
//                    if (i >= 75 && i <= 82)
//                    {
//                        int index = i - 58 - 17;
//                        player.dye[index].SetDefaults(items[i].netID);
//                        player.dye[index].prefix = items[i].prefix;
//                        player.dye[index].stack = items[i].stack;
//                    }
//                    else
//                    {
//                        int index = i - 58 - 1;
//                        player.armor[index].SetDefaults(items[i].netID);
//                        player.armor[index].prefix = items[i].prefix;
//                        player.armor[index].stack = items[i].stack;
//                    }
//                }
//                if(i < 83)
//                {
//                    NetMessage.SendData(5, -1, -1, items[i].name, Main.myPlayer, (float)i, items[i].prefix, 0f, 0);
//                }
//            }
//        }
//		// TODO- is 
//        public static void BackupHostility()
//        {
//            for (int i = 0; i < Network.Players.Length; i++)
//            {
//                GameikiPlayer player = Network.Players[i];
//                if (player.ServerInstance.IsActive)
//                {
//                    player.BackupHostility = player.GameInstance.hostile;
//                    player.BackupTeam = player.GameInstance.team;
//                }
//            }
            
//        }

//        public static void RestoreHostility()
//        {
//            for (int i = 0; i < Network.Players.Length; i++)
//            {
//                GameikiPlayer player = Network.Players[i];
//                if (player.ServerInstance.IsActive)
//                {
                    
//                    player.GameInstance.hostile = player.BackupHostility;
//                    player.GameInstance.team = player.BackupTeam;
//                    NetMessage.SendData(30, -1, -1, "", i, 0f, 0f, 0f, 0);
//                    NetMessage.SendData(45, -1, -1, "", i, 0f, 0f, 0f, 0);
//                }
//            }
//        }

//        public static void SetHostilityForAllPlayers()
//        {
//            for(int i = 0; i < Network.Players.Length; i++)
//            {
//                GameikiPlayer player = Network.Players[i];
//                if(player.ServerInstance.IsActive)
//                {
//                    SetPlayerHostility(player);
//                }
//            }
//        }

//        public static void SetPlayerHostility(GameikiPlayer player)
//        {
//            if (player.ServerInstance.IsActive)
//            {
//                if(player.CTFTeam == TeamColor.None)
//                {
//                    player.GameInstance.hostile = false;
//                    player.GameInstance.team = 0;
//                    CTFMessages.MakePlayerGhost(player);
//                }
//                else
//                {
//                    player.GameInstance.hostile = true;
//                    if(player.CTFTeam == TeamColor.Red)
//                    {
//                        player.GameInstance.team = 1;
//                    }
//                    else if(player.CTFTeam == TeamColor.Blue)
//                    {
//                        player.GameInstance.team = 3;
//                    }
//                }
//                NetMessage.SendData(30, -1, -1, "", player.Index, 0f, 0f, 0f, 0);
//                NetMessage.SendData(45, -1, -1, "", player.Index, 0f, 0f, 0f, 0);
                
//            }
//        }

//        public static void ClearBuffs()
//        {
//            Player player = Main.player[Main.myPlayer];
//            for(int i= 0;i < player.buffType.Length; i++)
//            {
//                player.DelBuff(0);
//            }
//        }

//        private static void CheckPlayerCrossingSides()
//        {
//            for(int i = 0;i < Network.Players.Length; i++)
//            {
//                GameikiPlayer player = Network.Players[i];
//                if(player.ServerInstance.IsActive/*.active */&& player.CTFTeam != TeamColor.None)
//                {
//                    FlagPlatform platform = RedTeam.FlagPlatform;
//                    if (player.CTFTeam == TeamColor.Blue) platform = BlueTeam.FlagPlatform;

//                    if (platform.Position.X < DividerPosition)
//                    {
//                        if (player.GameInstance.position.X + player.GameInstance.width > DividerPosition)
//                        {
//                            player.GameInstance.position.X = DividerPosition - player.GameInstance.width;
//                            NetMessage.SendData(13, -1, -1, "", player.Index);
//                        }
//                    }
//                    else
//                    {
//                        if (player.GameInstance.position.X < DividerPosition)
//                        {
//                            player.GameInstance.position.X = DividerPosition;
//                            NetMessage.SendData(13, -1, -1, "", player.Index);
//                        }
//                    }
//                }
//            }
//        }

//        private static void DrawDivider(SpriteBatch spriteBatch)
//        {
//            if(Main.screenPosition.X <= DividerPosition && Main.screenPosition.X + Main.screenWidth >= DividerPosition)
//            {
//                int drawWidth = 4;
//                Vector2 pos = new Vector2(DividerPosition - Main.screenPosition.X - drawWidth / 2, 0);
//                Rectangle source = new Rectangle(0, 0, drawWidth, Main.screenHeight);

//                spriteBatch.Draw(ModUtils.DummyTexture, pos, source, Color.Red);
//            }
//        }

//        public static void StartListeningForModifiedBlock()
//        {
//            ResetModifiedTiles();
//            ListeningForTileChanges = true;
//        }

//        public static void StopListeningForModifiedBlocks()
//        {
//            ListeningForTileChanges = false;
//        }
//        public static void ResetModifiedTiles()
//        {
//            ModifiedTiles = new Tile[Main.tile.GetLength(0), Main.tile.GetLength(1)];
//        }

//        public static void RestoreModifiedTiles()
//        {
//            if (ModifiedTiles == null) return;
//            for (int y = 0; y < ModifiedTiles.GetLength(1); y++)
//            {
//                for (int x = 0; x < ModifiedTiles.GetLength(0); x++)
//                {
//                    Tile tile = Main.tile[x, y];
//                    Tile backupTile = ModifiedTiles[x, y];

//                    if (backupTile != null && tile != backupTile)
//                    {
//                        tile.CopyFrom(backupTile);
//                        NetMessage.SendData(20, -1, -1, "", 1, x, y, 0f, 0);
//                    }
//                }
//            }
//        }
//    }
//}
