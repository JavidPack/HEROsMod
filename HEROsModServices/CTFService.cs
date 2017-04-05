//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using GameikiMod.UIKit.UIComponents;
//using System.IO;
//using GameikiMod.GameikiNetwork;
//using GameikiMod.GameikiNetwork.CTF;

//using Terraria;

//namespace GameikiMod.GameikiServices
//{
//    class CTFService : GameikiService
//    {
//        private SettingsWindow ctfSettings;
//        private TeamLists teamSelection;
//        private SpectatorView spectatorView;
//        public CTFService()
//        {
//			MultiplayerOnly = true;
//            this._name = "Capture the Flag";
//            this._hotbarIcon = new UIImage(UIView.GetEmbeddedTexture("Images/CTF/redFlag"));
//            this.HotbarIcon.Tooltip = "Capture the Flag Settings";
//            this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

//            spectatorView = new SpectatorView();
//            this.AddUIView(spectatorView);
//            spectatorView.Visible = CaptureTheFlag.GameInProgress;

//            ctfSettings = new SettingsWindow();
//            this.AddUIView(ctfSettings);
//            ctfSettings.Visible = false;

//            teamSelection = new TeamLists();
//            this.AddUIView(teamSelection);
//            teamSelection.Visible = false;

//            CaptureTheFlag.LobbyStarted += CaptureTheFlag_LobbyStarted;
//            CaptureTheFlag.GameStarted += CaptureTheFlag_GameStarted;
//            CaptureTheFlag.GameEnded += CaptureTheFlag_GameEnded;
//            CaptureTheFlag.PlayerChangedTeam += CaptureTheFlag_PlayerChangedTeam;
//            GameikiNetwork.GeneralMessages.PlayerJoined += GeneralMessages_PlayerJoined;
//            GameikiNetwork.GeneralMessages.PlayerLeft += GeneralMessages_PlayerLeft;
//        }

//        void GeneralMessages_PlayerLeft(GameikiPlayer player)
//        {
//            teamSelection.PopulateList();
//        }

//        void GeneralMessages_PlayerJoined(GameikiPlayer player)
//        {
//            teamSelection.PopulateList();
//        }

//        void CaptureTheFlag_GameEnded(object sender, EventArgs e)
//        {
//            teamSelection.Visible = false;
//            spectatorView.Visible = false;
//        }

//        void CaptureTheFlag_PlayerChangedTeam(object sender, EventArgs e)
//        {
//            teamSelection.PopulateList();
//            spectatorView.PopulatePlayers();
//        }

//        void CaptureTheFlag_GameStarted(object sender, EventArgs e)
//        {
//            teamSelection.Visible = false;
//            spectatorView.Visible = true;
//        }

//        void CaptureTheFlag_LobbyStarted(object sender, EventArgs e)
//        {
//            teamSelection.Visible = true;
//            ctfSettings.Visible = false;
//        }

//        void HotbarIcon_onLeftClick(object sender, EventArgs e)
//        {
//            if (CaptureTheFlag.InPregameLobby || CaptureTheFlag.GameInProgress)
//            {
//                teamSelection.Visible = !teamSelection.Visible;
//            }
//            else
//            {
//                ctfSettings.Visible = !ctfSettings.Visible;
//            }
//        }

//        public override void MyGroupUpdated()
//        {
//            this.HasPermissionToUse = GameikiNetwork.LoginService.MyGroup.HasPermission("StartCTF");
//            if (!HasPermissionToUse)
//            {
//                ctfSettings.Visible = false;
//            }
//            base.MyGroupUpdated();
//        }
//    }

//    class SettingsWindow : UIWindow
//    {

//        static Texture2D _blueButtonBG;
//        static Texture2D blueButtonBG
//        {
//            get
//            {
//                if (_blueButtonBG == null) _blueButtonBG = ModUtils.GetEmbeddedTexture("Images/CTF/blueButtonBG");
//                return _blueButtonBG;
//            }
//        }

//        static Texture2D _redButtonBG;
//        static Texture2D redButtonBG
//        {
//            get
//            {
//                if (_redButtonBG == null) _redButtonBG = ModUtils.GetEmbeddedTexture("Images/CTF/redButtonBG");
//                return _redButtonBG;
//            }
//        }

//        public bool closed = false;
//        static float spacing = 16f;
//        StartingInventory startingInv;
//        UICheckbox cbNeedsFlagToScore;
//        UICheckbox cbAllowBlockModification;
//        UICheckbox cbReplacePlayerInventory;
//        UIDropdown ddRounds;
//        UIDropdown ddFlagRespawn;
//        UIDropdown ddStartHealth;
//        UIDropdown ddStartMana;
//        UIDropdown ddCrossingTimer;
//        UICheckbox cbForceMediumcore;
//        UIButton bStart;
//        UIImage bClose;

//        static string SettingsSavePath = Main.SavePath + @"\CTFSettings";
//        public SettingsWindow()
//        {
//            this.Anchor = AnchorPosition.Left;
//            this.CanMove = true;
//            UILabel title = new UILabel("CTF Settings");
//            title.Scale = .6f;
//            bClose = new UIImage(closeTexture);
//            bClose.onLeftClick += bClose_onLeftClick;

//            title.X = spacing;
//            title.Y = spacing;
//            title.OverridesMouse = false;

//            UILabel roundsLabel = new UILabel("Points to win");
//            roundsLabel.Scale = .5f;
//            ddRounds = new UIDropdown();
//            ddRounds.Width = 75;
//            ddRounds.AddItem("1");
//            ddRounds.AddItem("3");
//            ddRounds.AddItem("5");
//            ddRounds.AddItem("7");
//            ddRounds.AddItem("9");
//            ddRounds.SelectedItem = 1;

//            UILabel flagRespawnLabel = new UILabel("Flag Respawn Time");
//            flagRespawnLabel.Scale = .5f;
//            ddFlagRespawn = new UIDropdown();
//            ddFlagRespawn.Width = 75;
//            ddFlagRespawn.AddItem("10");
//            ddFlagRespawn.AddItem("20");
//            ddFlagRespawn.AddItem("30");
//            ddFlagRespawn.AddItem("60");
//            ddFlagRespawn.AddItem("120");
//            ddFlagRespawn.SelectedItem = 1;

//            UILabel lStartHealth = new UILabel("Starting Health");
//            lStartHealth.Scale = .5f;
//            ddStartHealth = new UIDropdown();
//            ddStartHealth.Width = 200;
//            ddStartHealth.AddItem("Do Not Change");
//            ddStartHealth.AddItem("100");
//            ddStartHealth.AddItem("200");
//            ddStartHealth.AddItem("300");
//            ddStartHealth.AddItem("400");
//            ddStartHealth.AddItem("500");

//            UILabel lStartMana = new UILabel("Starting Mana");
//            lStartMana.Scale = .5f;
//            ddStartMana = new UIDropdown();
//            ddStartMana.Width = ddStartHealth.Width;
//            ddStartMana.AddItem("Do Not Change");
//            ddStartMana.AddItem("0");
//            ddStartMana.AddItem("20");
//            ddStartMana.AddItem("40");
//            ddStartMana.AddItem("60");
//            ddStartMana.AddItem("80");
//            ddStartMana.AddItem("100");
//            ddStartMana.AddItem("120");
//            ddStartMana.AddItem("140");
//            ddStartMana.AddItem("160");
//            ddStartMana.AddItem("200");

//            UILabel lCrossingTimer = new UILabel("Crossing Timer");
//            lCrossingTimer.Scale = .5f;
//            ddCrossingTimer = new UIDropdown();
//            ddCrossingTimer.Width = ddStartHealth.Width;
//            ddCrossingTimer.AddItem("0:00");
//            ddCrossingTimer.AddItem("0:30");
//            ddCrossingTimer.AddItem("1:00");
//            ddCrossingTimer.AddItem("2:00");
//            ddCrossingTimer.AddItem("3:00");
//            ddCrossingTimer.AddItem("5:00");

//            ddStartMana.UpdateWhenOutOfBounds = true;
//            cbNeedsFlagToScore = new UICheckbox("Need own flag at base to score");
//            cbAllowBlockModification = new UICheckbox("Allow Terrain Modification");
//            cbAllowBlockModification.Selected = true;
//            cbNeedsFlagToScore.Selected = true;
//            cbReplacePlayerInventory = new UICheckbox("Set Starting Items");
//            bStart = new UIButton("Start Game");
//            UIImage rBG = new UIImage(redButtonBG);
//            UIImage rBG2 = new UIImage(redButtonBG);
//            UIImage bBG = new UIImage(blueButtonBG);
//            UIImage bBG2 = new UIImage(blueButtonBG);

//            UILabel lPlaceRedSpawn = new UILabel("Place Red Spawn");
//            UILabel lPlaceBlueSpawn = new UILabel("Place Blue Spawn");
//            UILabel lPlaceRedPlatform = new UILabel("Place Red Flag");
//            UILabel lPlaceBluePlatform = new UILabel("Place Blue Flag");

//            lPlaceRedPlatform.Anchor = AnchorPosition.Center;
//            lPlaceBluePlatform.Anchor = AnchorPosition.Center;
//            lPlaceRedSpawn.Anchor = AnchorPosition.Center;
//            lPlaceBlueSpawn.Anchor = AnchorPosition.Center;

//            UIButton bSaveSettings = new UIButton("Save Settings");
//            UIButton bLoadSettings = new UIButton("Load Settings");

//            bSaveSettings.AutoSize = false;
//            bSaveSettings.Width = rBG.Width;
//            bLoadSettings.AutoSize = false;
//            bLoadSettings.Width = bSaveSettings.Width;

//            startingInv = new StartingInventory();
//            cbForceMediumcore = new UICheckbox("Force Mediumcore");



//            roundsLabel.X = spacing;
//            roundsLabel.Y = title.X + title.Height + spacing;
//            ddRounds.X = roundsLabel.X + roundsLabel.Width + 4;
//            ddRounds.Y = roundsLabel.Y;
//            flagRespawnLabel.X = roundsLabel.X;
//            flagRespawnLabel.Y = roundsLabel.Y + roundsLabel.Height + spacing;
//            ddFlagRespawn.X = flagRespawnLabel.X + flagRespawnLabel.Width + 4;
//            ddFlagRespawn.Y = flagRespawnLabel.Y;
//            lStartHealth.X = flagRespawnLabel.X;
//            lStartHealth.Y = flagRespawnLabel.Y + flagRespawnLabel.Height + spacing;
//            ddStartHealth.X = lStartHealth.X + lStartHealth.Width + 4;
//            ddStartHealth.Y = lStartHealth.Y;
//            lStartMana.X = lStartHealth.X;
//            lStartMana.Y = lStartHealth.Y + lStartHealth.Height + spacing;
//            ddStartMana.Y = lStartMana.Y;
//            ddStartMana.X = lStartMana.X + lStartMana.Width + 4;

//            lCrossingTimer.X = lStartMana.X;
//            lCrossingTimer.Y = lStartMana.Y + lStartMana.Height + spacing;

//            ddCrossingTimer.Y = lCrossingTimer.Y;
//            ddCrossingTimer.X = lCrossingTimer.X + lCrossingTimer.Width + 4;

//            cbNeedsFlagToScore.X = lCrossingTimer.X;
//            cbNeedsFlagToScore.Y = ddCrossingTimer.Y + ddCrossingTimer.Height + spacing;
//            cbAllowBlockModification.X = cbNeedsFlagToScore.X;
//            cbAllowBlockModification.Y = cbNeedsFlagToScore.Y + cbNeedsFlagToScore.Height;
//            cbReplacePlayerInventory.X = cbAllowBlockModification.X;
//            cbReplacePlayerInventory.Y = cbAllowBlockModification.Y + cbAllowBlockModification.Height;
//            ddFlagRespawn.X = ddStartHealth.X + ddStartHealth.Width - ddFlagRespawn.Width;
//            ddRounds.X = ddStartHealth.X + ddStartHealth.Width - ddRounds.Width;
//            ddStartMana.X = ddStartHealth.X + ddStartHealth.Width - ddStartMana.Width;

//            startingInv.X = ddStartHealth.X + ddStartHealth.Width + spacing;
//            startingInv.Y = spacing * 2 + bClose.Height;
//            cbForceMediumcore.X = startingInv.X + 8;
//            cbForceMediumcore.Y = startingInv.Y + startingInv.Height + spacing;

//            startingInv.Visible = false;
//            cbForceMediumcore.Visible = false;
//            Width = ddStartHealth.X + ddStartHealth.Width + spacing;

//            bStart.AutoSize = false;

//            bStart.Width = Width - spacing * 2;


//            rBG.X = cbReplacePlayerInventory.X;
//            rBG.Y = cbReplacePlayerInventory.Y + cbReplacePlayerInventory.Height + spacing;
//            rBG2.X = rBG.X;
//            rBG2.Y = rBG.Y + rBG.Height + spacing;
//            bBG.X = rBG.X + rBG.Width + spacing;
//            bBG.Y = rBG.Y;
//            bBG2.X = bBG.X;
//            bBG2.Y = bBG.Y + bBG.Height + spacing;

//            SetLabelScale(lPlaceRedSpawn, rBG.Width, rBG.Height);
//            SetLabelScale(lPlaceBlueSpawn, rBG.Width, rBG.Height);
//            SetLabelScale(lPlaceRedPlatform, rBG.Width, rBG.Height);
//            SetLabelScale(lPlaceBluePlatform, rBG.Width, rBG.Height);

//            float offset = 2f;
//            lPlaceRedSpawn.X = rBG.X + rBG.Width / 2;
//            lPlaceBlueSpawn.X = bBG.X + bBG.Width / 2;
//            lPlaceRedPlatform.X = rBG2.X + rBG2.Width / 2;
//            lPlaceBluePlatform.X = bBG2.X + bBG2.Width / 2;
//            lPlaceRedSpawn.Y = rBG.Y + rBG.Height / 2 + offset;
//            lPlaceBlueSpawn.Y = bBG.Y + bBG.Height / 2 + offset;
//            lPlaceRedPlatform.Y = rBG2.Y + rBG2.Height / 2 + offset;
//            lPlaceBluePlatform.Y = bBG2.Y + bBG2.Height / 2 + offset;

//            bSaveSettings.X = rBG2.X;
//            bSaveSettings.Y = rBG2.Y + rBG2.Height + spacing;
//            bLoadSettings.X = bBG2.X;
//            bLoadSettings.Y = bBG2.Y + bBG2.Height + spacing;

//            bStart.X = bSaveSettings.X;
//            bStart.Y = bSaveSettings.Y + bSaveSettings.Height + spacing;

//            Height = bStart.Y + bStart.Height + spacing;


//            bClose.X = Width - spacing * 2;
//            bClose.Y = spacing;

//            cbReplacePlayerInventory.SelectedChanged += cbReplacePlayerInventory_SelectedChanged;
//            rBG.onLeftClick += bPlaceRedSpawn_onLeftClick;
//            rBG2.onLeftClick += bPlaceRedPlatform_onLeftClick;
//            bBG.onLeftClick += bPlaceBlueSpawn_onLeftClick;
//            bBG2.onLeftClick += bPlaceBluePlatform_onLeftClick;
//            lPlaceRedSpawn.onLeftClick += bPlaceRedSpawn_onLeftClick;
//            lPlaceRedPlatform.onLeftClick += bPlaceRedPlatform_onLeftClick;
//            lPlaceBlueSpawn.onLeftClick += bPlaceBlueSpawn_onLeftClick;
//            lPlaceBluePlatform.onLeftClick += bPlaceBluePlatform_onLeftClick;
//            bSaveSettings.onLeftClick += bSaveSettings_onLeftClick;
//            bLoadSettings.onLeftClick += bLoadSettings_onLeftClick;
//            bStart.onLeftClick += bStart_onLeftClick;

//            AddChild(title);
//            AddChild(bClose);
//            AddChild(roundsLabel);
//            AddChild(flagRespawnLabel);
//            AddChild(lStartHealth);
//            AddChild(lStartMana);
//            AddChild(lCrossingTimer);
//            AddChild(cbNeedsFlagToScore);
//            AddChild(cbAllowBlockModification);
//            AddChild(cbReplacePlayerInventory);
//            AddChild(rBG);
//            AddChild(rBG2);
//            AddChild(bBG);
//            AddChild(bBG2);
//            AddChild(lPlaceRedSpawn);
//            AddChild(lPlaceBlueSpawn);
//            AddChild(lPlaceRedPlatform);
//            AddChild(lPlaceBluePlatform);
//            AddChild(bSaveSettings);
//            AddChild(bLoadSettings);
//            AddChild(bStart);
//            AddChild(startingInv);
//            AddChild(cbForceMediumcore);
//            AddChild(ddCrossingTimer);
//            AddChild(ddStartMana);
//            AddChild(ddStartHealth);
//            AddChild(ddFlagRespawn);
//            AddChild(ddRounds);


//            this.CenterToParent();
//            this.X = 100;
//        }

//        void bLoadSettings_onLeftClick(object sender, EventArgs e)
//        {
//            string[] files = GetSavedSettingsFiles();
//            if (files.Length == 0)
//            {
//                Main.NewText("No Settings Save files found.");
//                return;
//            }
//            LoadSettingsDialog lsd = new LoadSettingsDialog();
//            lsd.SettingsFileSelected += lsd_SettingsFileSelected;
//            lsd.FileSelectedForDeletion += lsd_FileSelectedForDeletion;
//            Parent.AddChild(lsd);
//        }

//        void lsd_FileSelectedForDeletion(string fileName)
//        {
//            if (File.Exists(SettingsSavePath + @"\" + fileName + ".ctfs"))
//            {
//                File.Delete(SettingsSavePath + @"\" + fileName + ".ctfs");
//            }
//            else Main.NewText("Could not delete the file");
//        }

//        void lsd_SettingsFileSelected(string fileName)
//        {
//            LoadSettings(fileName);
//        }

//        void bSaveSettings_onLeftClick(object sender, EventArgs e)
//        {
//            SaveSettingsDialog ssd = new SaveSettingsDialog();
//            ssd.NameSubmited += ssd_NameSubmited;
//            Parent.AddChild(ssd);

//        }

//        void ssd_NameSubmited(string name)
//        {
//            SaveSettings(name);
//        }

//        void SetLabelScale(UILabel label, float width, float height)
//        {
//            Vector2 size = label.font.MeasureString(label.Text);
//            if (size.X > width - 26)
//            {
//                label.Scale = (width - 26) / size.X;
//                if (size.Y * label.Scale > height) label.Scale = height / size.Y;
//            }
//            else label.Scale = height / size.Y;
//        }

//        void bPlaceRedSpawn_onLeftClick(object sender, EventArgs e)
//        {
//            CTFMessages.RequestPlaceSpawnPlatform(TeamColor.Red);
//        }

//        void bPlaceBlueSpawn_onLeftClick(object sender, EventArgs e)
//        {
//            CTFMessages.RequestPlaceSpawnPlatform(TeamColor.Blue);

//        }

//        void cbReplacePlayerInventory_SelectedChanged(object sender, EventArgs e)
//        {
//            UICheckbox cb = (UICheckbox)sender;
//            if (cb.Selected)
//            {
//                startingInv.Visible = true;
//                cbForceMediumcore.Visible = true;
//                Width = startingInv.X + startingInv.Width + spacing;

//            }
//            else
//            {
//                startingInv.Visible = false;
//                cbForceMediumcore.Visible = false;
//                Width = ddStartHealth.X + ddStartHealth.Width + spacing;
//            }
//            bClose.X = Width - spacing * 2;
//        }


//        void bStart_onLeftClick(object sender, EventArgs e)
//        {
//            //Set up lobby

//            using(MemoryStream memorySteam = new MemoryStream())
//            {
//                using (BinaryWriter writer = new BinaryWriter(memorySteam))
//                {
//                    writer.Write(cbNeedsFlagToScore.Selected);
//                    writer.Write(cbReplacePlayerInventory.Selected);
//                    writer.Write(cbAllowBlockModification.Selected);

//                    writer.Write(byte.Parse(ddRounds.GetItem(ddRounds.SelectedItem)));
//                    writer.Write(int.Parse(ddFlagRespawn.GetItem(ddFlagRespawn.SelectedItem)));
//                    int startingHealth = -1;
//                    if (ddStartHealth.SelectedItem > 0)
//                    {
//                        startingHealth = int.Parse(ddStartHealth.GetItem(ddStartHealth.SelectedItem));
//                    }
//                    writer.Write(startingHealth);
//                    int startingMana = -1;
//                    if (ddStartMana.SelectedItem > 0)
//                    {
//                        startingMana = int.Parse(ddStartMana.GetItem(ddStartMana.SelectedItem));
//                    }
//                    writer.Write(startingMana);
//                    writer.Write(GetCrossingTimerInSeconds());
//                    writer.Write(cbForceMediumcore.Selected);

//                    if (cbReplacePlayerInventory.Selected)
//                    {
//                        Item[] startingItems = startingInv.GetItems();
//                        for(int i = 0;i < startingItems.Length; i++)
//                        {
//                            Item item = startingItems[i];
//                            writer.Write(item.netID);
//                            writer.Write(item.stack);
//                            writer.Write(item.prefix);
//                        }

//                    }
//                    CTFMessages.RequestStartLobby(memorySteam.ToArray());
//                }
//            }
//        }

//        void bPlaceRedPlatform_onLeftClick(object sender, EventArgs e)
//        {
//            CTFMessages.RequestPlaceFlagPlatform(TeamColor.Red);
//        }

//        void bPlaceBluePlatform_onLeftClick(object sender, EventArgs e)
//        {
//            CTFMessages.RequestPlaceFlagPlatform(TeamColor.Blue);
//        }

//        public void Close()
//        {
//            this.Visible = false;
//        }
//        void bClose_onLeftClick(object sender, EventArgs e)
//        {
//            this.Close();
//        }

//        void SaveSettings(string name)
//        {
//            if (Directory.Exists(Main.SavePath))
//            {
//                string settingsDir = Main.SavePath + @"\CTFSettings";
//                if (!Directory.Exists(settingsDir))
//                    Directory.CreateDirectory(settingsDir);
//                if (Directory.Exists(settingsDir))
//                {
//                    using (FileStream fileStream = new FileStream(settingsDir + @"\" + name + ".ctfs", FileMode.Create))
//                    {
//                        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
//                        {
//                            ushort version = 0;
//                            binaryWriter.Write(version);

//                            binaryWriter.Write(ddFlagRespawn.SelectedItem);
//                            binaryWriter.Write(ddRounds.SelectedItem);
//                            binaryWriter.Write(ddStartHealth.SelectedItem);
//                            binaryWriter.Write(ddStartMana.SelectedItem);
//                            binaryWriter.Write(cbNeedsFlagToScore.Selected);
//                            binaryWriter.Write(cbAllowBlockModification.Selected);
//                            binaryWriter.Write(cbReplacePlayerInventory.Selected);
//                            binaryWriter.Write(cbForceMediumcore.Selected);

//                            Item[] items = startingInv.GetItems();

//                            binaryWriter.Write(items.Length);
//                            for (int i = 0; i < items.Length; i++)
//                            {
//                                binaryWriter.Write(items[i].type);
//                                binaryWriter.Write(items[i].stack);
//                                binaryWriter.Write(items[i].prefix);
//                            }
//                            FlagPlatform redTeamFlagPlatform = CaptureTheFlag.RedTeam.FlagPlatform;
//                            FlagPlatform blueTeamFlagPlatform = CaptureTheFlag.BlueTeam.FlagPlatform;
//                            SpawnPlatform readTeamSpawnPlatform = CaptureTheFlag.RedTeam.SpawnPlatform;
//                            SpawnPlatform blueTeamSpawnPlatform = CaptureTheFlag.BlueTeam.SpawnPlatform;

//                            binaryWriter.Write(redTeamFlagPlatform.Placed);
//                            if (redTeamFlagPlatform.Placed)
//                            {
//                                binaryWriter.Write(redTeamFlagPlatform.Center.X);
//                                binaryWriter.Write(redTeamFlagPlatform.Center.Y);
//                            }
//                            binaryWriter.Write(blueTeamFlagPlatform.Placed);
//                            if (blueTeamFlagPlatform.Placed)
//                            {
//                                binaryWriter.Write(blueTeamFlagPlatform.Center.X);
//                                binaryWriter.Write(blueTeamFlagPlatform.Center.Y);
//                            }
//                            binaryWriter.Write(readTeamSpawnPlatform.Placed);
//                            if (readTeamSpawnPlatform.Placed)
//                            {
//                                binaryWriter.Write(readTeamSpawnPlatform.Center.X);
//                                binaryWriter.Write(readTeamSpawnPlatform.Center.Y);
//                            }
//                            binaryWriter.Write(blueTeamSpawnPlatform.Placed);
//                            if (blueTeamSpawnPlatform.Placed)
//                            {
//                                binaryWriter.Write(blueTeamSpawnPlatform.Center.X);
//                                binaryWriter.Write(blueTeamSpawnPlatform.Center.Y);
//                            }
//                            binaryWriter.Close();
//                        }
//                    }
//                }

//            }
//            else Main.NewText("Terraria save directory does not exist!");

//            Console.WriteLine(Main.SavePath + @"\CTFSettings");
//        }

//        void LoadSettings(string fileName)
//        {
//            string settingsPath = Main.SavePath + @"\CTFSettings\" + fileName + ".ctfs";
//            if (!File.Exists(settingsPath))
//            {
//                Main.NewText("File does not exist");
//                return;
//            }
//            using (FileStream fileStream = new FileStream(settingsPath, FileMode.Open))
//            {
//                using (BinaryReader binaryReader = new BinaryReader(fileStream))
//                {
//                    ushort version = binaryReader.ReadUInt16();
//                    ddFlagRespawn.SelectedItem = binaryReader.ReadInt32();
//                    ddRounds.SelectedItem = binaryReader.ReadInt32();
//                    ddStartHealth.SelectedItem = binaryReader.ReadInt32();
//                    ddStartMana.SelectedItem = binaryReader.ReadInt32();
//                    cbNeedsFlagToScore.Selected = binaryReader.ReadBoolean();
//                    cbAllowBlockModification.Selected = binaryReader.ReadBoolean();
//                    cbReplacePlayerInventory.Selected = binaryReader.ReadBoolean();
//                    cbForceMediumcore.Selected = binaryReader.ReadBoolean();

//                    List<Item> items = new List<Item>();
//                    int numOfItems = binaryReader.ReadInt32();
//                    for (int i = 0; i < numOfItems; i++)
//                    {
//                        Item item = new Item();
//                        item.SetDefaults(binaryReader.ReadInt32());
//                        item.stack = binaryReader.ReadInt32();
//                        item.prefix = binaryReader.ReadByte();
//                        items.Add(item);
//                    }
//                    startingInv.SetItems(items.ToArray());

//                    if (binaryReader.ReadBoolean())
//                    {
//                        Vector2 pos = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
//                        CTFMessages.RequestPlaceFlagPlatform(TeamColor.Red, pos);
//                    }
//                    if (binaryReader.ReadBoolean())
//                    {
//                        Vector2 pos = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
//                        CTFMessages.RequestPlaceFlagPlatform(TeamColor.Blue, pos);
//                    }
//                    if (binaryReader.ReadBoolean())
//                    {
//                        Vector2 pos = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
//                        CTFMessages.RequestPlaceSpawnPlatform(TeamColor.Red, pos);
//                    }
//                    if (binaryReader.ReadBoolean())
//                    {
//                        Vector2 pos = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
//                        CTFMessages.RequestPlaceSpawnPlatform(TeamColor.Blue, pos);
//                    }

//                    binaryReader.Close();
//                }
//            }
//        }

//        private int GetCrossingTimerInSeconds()
//        {
//            string ctStr = ddCrossingTimer.GetItem(ddCrossingTimer.SelectedItem);
//            int result = 0;
//            string[] parts = ctStr.Split(':');
//            int minutes = int.Parse(parts[0]);
//            int seconds = int.Parse(parts[1]);
//            result += minutes * 60;
//            result += seconds;
//            return result;
//        }

//        public static string[] GetSavedSettingsFiles()
//        {
//            List<string> results = new List<string>();
//            string settingsDir = Main.SavePath + @"\CTFSettings";
//            if (!Directory.Exists(settingsDir)) return results.ToArray();
//            string[] files = Directory.GetFiles(settingsDir);
//            for (int i = 0; i < files.Length; i++)
//            {
//                string extension = Path.GetExtension(files[i]);
//                if (extension.ToLower() == ".ctfs")
//                {
//                    results.Add(Path.GetFileNameWithoutExtension(files[i]));
//                }
//            }
//            return results.ToArray();
//        }

//        public override void Update()
//        {
//            //if (this.Parent != null && Main.gameMenu) this.Close();
//            //if (Mod.ctf.GameInProgress)
//            //{
//            //    bStart.Text = "End Game";
//            //}
//            //else bStart.Text = "Start Game";
//            if (cbReplacePlayerInventory.Selected && Visible)
//            {
//                Main.playerInventory = true;
//            }
//            base.Update();
//        }
//    }

//    class SaveSettingsDialog : UIWindow
//    {
//        public delegate void NameSubmitedEvent(string name);
//        public event NameSubmitedEvent NameSubmited;
//        UITextbox tbName;

//        static float spacing = 16f;
//        public SaveSettingsDialog()
//        {
//            UIView.exclusiveControl = this;
//            this.Anchor = AnchorPosition.Center;
//            UILabel lDescription = new UILabel("Save settings for later use.");
//            lDescription.Scale = .5f;
//            lDescription.X = spacing;
//            lDescription.Y = spacing;

//            UILabel lName = new UILabel("Name: ");
//            lName.Scale = .5f;
//            lName.X = lDescription.X;
//            lName.Y = lDescription.Y + lDescription.Height;

//            tbName = new UITextbox();
//            tbName.Width = lDescription.Width - lName.Width;
//            tbName.X = lName.X + lName.Width;
//            tbName.Y = lName.Y;
//            tbName.MaxCharacters = 24;


//            UIButton bSave = new UIButton("Save");
//            UIButton bCancel = new UIButton("Cancel");
//            bSave.Y = lName.Y + lName.Height + spacing;
//            bCancel.Y = bSave.Y;

//            this.Width = lDescription.Width + spacing * 2;
//            this.Height = bSave.Y + bSave.Height + spacing;

//            bCancel.X = this.Width - bCancel.Width - spacing;
//            bSave.X = bCancel.X - bSave.Width - spacing;

//            bSave.onLeftClick += bSave_onLeftClick;
//            bCancel.onLeftClick += bCancel_onLeftClick;
//            tbName.OnEnterPress += bSave_onLeftClick;

//            AddChild(lDescription);
//            AddChild(lName);
//            AddChild(tbName);
//            AddChild(bSave);
//            AddChild(bCancel);
//            tbName.Focus();
//        }

//        void bCancel_onLeftClick(object sender, EventArgs e)
//        {
//            Close();
//        }

//        void bSave_onLeftClick(object sender, EventArgs e)
//        {
//            if (tbName.Text.Length < 3)
//            {
//                Main.NewText("Settings name must be at least three characters long");
//                return;
//            }
//            if (NameSubmited != null)
//                NameSubmited(tbName.Text);
//            Close();
//        }

//        void Close()
//        {
//            tbName.Unfocus();
//            if (UIView.exclusiveControl == this) UIView.exclusiveControl = null;
//            this.Parent.RemoveChild(this);
//        }

//        public override void Update()
//        {
//            CenterToParent();
//            base.Update();
//        }
//    }

//    class LoadSettingsDialog : UIWindow
//    {
//        public delegate void SettingsFileSelectedHandler(string fileName);
//        public event SettingsFileSelectedHandler SettingsFileSelected;
//        public event SettingsFileSelectedHandler FileSelectedForDeletion;
//        static float spacing = 16f;
//        UIScrollView scrollView;
//        public LoadSettingsDialog()
//        {

//            UIView.exclusiveControl = this;
//            this.Anchor = AnchorPosition.Center;

//            UILabel lSelectFile = new UILabel("Select File");
//            lSelectFile.X = spacing;
//            lSelectFile.Y = spacing;
//            lSelectFile.Scale = .5f;

//            scrollView = new UIScrollView();
//            scrollView.Width = 300;
//            scrollView.Height = 250;
//            scrollView.X = lSelectFile.X;
//            scrollView.Y = lSelectFile.Y + lSelectFile.Height;
//            scrollView.ContentHeight = scrollView.Height;

//            UpdateFilesList();

//            UIButton bCancel = new UIButton("Cancel");
//            bCancel.AutoSize = false;
//            bCancel.Width = scrollView.Width;
//            bCancel.X = scrollView.X;
//            bCancel.Y = scrollView.Y + scrollView.Height + 8;
//            bCancel.onLeftClick += bCancel_onLeftClick;

//            this.Width = scrollView.Width + spacing * 2;
//            this.Height = bCancel.Y + bCancel.Height + 8;
//            AddChild(lSelectFile);
//            AddChild(scrollView);
//            AddChild(bCancel);
//        }

//        void UpdateFilesList()
//        {
//            string[] files = SettingsWindow.GetSavedSettingsFiles();
//            scrollView.ClearContent();
//            float yPos = 0;
//            for (int i = 0; i < files.Length; i++)
//            {
//                UILabel label = new UILabel(files[i]);
//                label.X = spacing;
//                label.Y = yPos;
//                label.Scale = .5f;
//                label.onLeftClick += label_onLeftClick;
//                yPos += label.Height;
//                scrollView.AddChild(label);

//                UIImage image = new UIImage(closeTexture);
//                image.ForegroundColor = Color.Red;
//                image.Anchor = AnchorPosition.Right;
//                image.Position = new Vector2(scrollView.Width - 10 - spacing, label.Position.Y + label.Height / 2);
//                image.Tag = label.Text;
//                image.onLeftClick += image_onLeftClick;
//                scrollView.AddChild(image);
//            }
//            if (scrollView.ChildCount > 0)
//            {
//                UIView lastChild = scrollView.GetLastChild();
//                scrollView.ContentHeight = lastChild.Y + lastChild.Height;
//            }
//        }

//        void image_onLeftClick(object sender, EventArgs e)
//        {
//            UIImage image = (UIImage)sender;
//            string fileName = (string)image.Tag;
//            if (FileSelectedForDeletion != null)
//                FileSelectedForDeletion(fileName);
//            UpdateFilesList();
//        }

//        void label_onLeftClick(object sender, EventArgs e)
//        {
//            UILabel label = (UILabel)sender;
//            if (SettingsFileSelected != null)
//                SettingsFileSelected(label.Text);
//            Close();
//        }

//        void bCancel_onLeftClick(object sender, EventArgs e)
//        {
//            Close();
//        }

//        void Close()
//        {
//            if (UIView.exclusiveControl == this) UIView.exclusiveControl = null;
//            this.Parent.RemoveChild(this);
//        }

//        public override void Update()
//        {
//            CenterToParent();
//            base.Update();
//        }
//    }

//    class StartingInventory : UIWindow
//    {
//        public StartingInventory()
//        {
//            for (int i = 0; i < 50; i++)
//            {
//                Slot slot = new Slot(0);
//                slot.functionalSlot = true;

//                slot.X = 8 + i % 10 * slot.Width;
//                slot.Y = 8 + i / 10 * slot.Height;

//                if (i == 0)
//                {
//                    slot.item.SetDefaults("Copper Shortsword");
//                    slot.item.Prefix(-1);
//                }
//                else if (i == 1)
//                {
//                    slot.item.SetDefaults("Copper Pickaxe");
//                    slot.item.Prefix(-1);
//                }
//                else if (i == 2)
//                {
//                    slot.item.SetDefaults("Copper Axe");
//                    slot.item.Prefix(-1);
//                }

//                AddChild(slot);
//            }

//            UIView lc = GetLastChild();

//            Width = lc.X + lc.Width + 8;
//            Height = lc.Y + lc.Height + 8;
//        }

//        public Item[] GetItems()
//        {
//            List<Item> result = new List<Item>();
//            for (int i = 0; i < 50; i++)
//            {
//                Slot slot = (Slot)GetChild(i);
//                result.Add(slot.item);
//            }
//            return result.ToArray();
//        }

//        public void SetItems(Item[] items)
//        {
//            for (int i = 0; i < 50; i++)
//            {
//                Slot slot = (Slot)GetChild(i);
//                slot.item = items[i];
//            }
//        }
//    }

//    class TeamLists : UIWindow
//    {
//        static float spacing = 16f;
//        static float lineWidth = 5f;
//        UIView rtContainer;
//        UIView btContainer;
//        UIButton bStartGame;
//        public float expirationTimer = 0f;
//        UILabel expirationTimeLabel;
//        UILabel lobbyExpiration;
//        UIButton bJoinRedTeam;
//        UIButton bJoinBlueTeam;
//        UIButton bSpectate;
//        public TeamLists()
//        {
//            UIImage bClose = new UIImage(closeTexture);
//            UILabel lRedTeam = new UILabel("Red Team");
//            UILabel lBlueTeam = new UILabel("Blue Team");
//            UIRect hLine = new UIRect();
//            UIRect vLine = new UIRect();
//            rtContainer = new UIView();
//            btContainer = new UIView();
//            UIRect rtRect = new UIRect();
//            UIRect btRect = new UIRect();
//            bJoinRedTeam = new UIButton("Join Red Team");
//            bJoinBlueTeam = new UIButton("Join Blue Team");
//            bSpectate = new UIButton("Spectate");
//            bStartGame = new UIButton("Start Game");
//            lobbyExpiration = new UILabel("Lobby will expire in:");
//            expirationTimeLabel = new UILabel();

//            this.Anchor = AnchorPosition.Center;
//            Width = 700;
//            Height = 516;
            
//            lRedTeam.Scale = .65f;
//            lBlueTeam.Scale = .65f;
//            vLine.Anchor = AnchorPosition.Top;
//            rtRect.ForegroundColor = Color.Red * .2f;
//            btRect.ForegroundColor = Color.Blue * .2f;
//            bJoinBlueTeam.AutoSize = false;
//            bJoinRedTeam.AutoSize = false;
//            bJoinBlueTeam.Width = 150;
//            bJoinRedTeam.Width = 150;
//            bJoinBlueTeam.Anchor = AnchorPosition.Top;
//            bJoinRedTeam.Anchor = AnchorPosition.Top;
//            bSpectate.Anchor = AnchorPosition.Top;
//            lobbyExpiration.Scale = .35f;
//            expirationTimeLabel.Scale = lobbyExpiration.Scale;
//            expirationTimeLabel.Anchor = AnchorPosition.TopRight;

//            bClose.X = Width - bClose.Width - spacing;
//            bClose.Y = spacing;
//            lRedTeam.X = spacing;
//            lRedTeam.Y = bClose.Y + bClose.Height + spacing;
//            lBlueTeam.X = Width - lBlueTeam.Width - spacing;
//            lBlueTeam.Y = lRedTeam.Y;
//            hLine.X = spacing;
//            hLine.Y = lRedTeam.Y + lRedTeam.Height;
//            hLine.Width = Width - spacing * 2;
//            hLine.Height = lineWidth;
//            vLine.Y = hLine.Y;
//            vLine.X = Width / 2;
//            vLine.Height = Height - vLine.Y - 116;
//            vLine.Width = lineWidth;
//            rtContainer.X = spacing;
//            rtContainer.Y = hLine.Y + hLine.Height;
//            rtContainer.Width = (Width - spacing * 2) / 2;
//            rtContainer.Height = Height - rtContainer.Y - 116;
//            btContainer.X = Width / 2;
//            btContainer.Y = rtContainer.Y;
//            btContainer.Width = rtContainer.Width;
//            btContainer.Height = rtContainer.Height;
//            rtRect.Position = rtContainer.Position;
//            rtRect.Width = rtContainer.Width;
//            rtRect.Height = rtContainer.Height;
//            btRect.Position = btContainer.Position;
//            btRect.Width = btContainer.Width;
//            btRect.Height = btContainer.Height;
//            bJoinRedTeam.X = rtContainer.X + rtContainer.Width / 2;
//            bJoinRedTeam.Y = rtContainer.Y + rtContainer.Height + spacing;
//            bJoinBlueTeam.X = btContainer.X + btContainer.Width / 2;
//            bJoinBlueTeam.Y = bJoinRedTeam.Y;
//            bSpectate.X = Width / 2;
//            bSpectate.Y = Height - bSpectate.Height - spacing;
//            bStartGame.X = Width - bStartGame.Width - spacing;
//            bStartGame.Y = Height - bStartGame.Height - spacing;
//            lobbyExpiration.X = spacing;
//            lobbyExpiration.Y = bStartGame.Y + 12;
//            expirationTimeLabel.X = lobbyExpiration.X + lobbyExpiration.Width + 40;
//            expirationTimeLabel.Y = lobbyExpiration.Y;

//            bClose.onLeftClick += bClose_onLeftClick;
//            bJoinRedTeam.onLeftClick += bJoinRedTeam_onLeftClick;
//            bJoinBlueTeam.onLeftClick += bJoinBlueTeam_onLeftClick;
//            bSpectate.onLeftClick += bSpectate_onLeftClick;
//            bStartGame.onLeftClick += bStartGame_onLeftClick;

//            AddChild(bClose);
//            AddChild(lRedTeam);
//            AddChild(lBlueTeam);
//            AddChild(rtRect);
//            AddChild(btRect);
//            AddChild(rtContainer);
//            AddChild(btContainer);
//            AddChild(hLine);
//            AddChild(vLine);
//            AddChild(bJoinRedTeam);
//            AddChild(bJoinBlueTeam);
//            AddChild(bSpectate);
//            AddChild(bStartGame);
//            AddChild(lobbyExpiration);
//            AddChild(expirationTimeLabel);

//            expirationTimer = 7f;

//            PopulateList();
//        }

//        void bClose_onLeftClick(object sender, EventArgs e)
//        {
//            Hide();
//        }

//        void bStartGame_onLeftClick(object sender, EventArgs e)
//        {
//            if (CaptureTheFlag.InPregameLobby)CTFMessages.RequestStartGame();
//            else if (CaptureTheFlag.GameInProgress) CTFMessages.RequestEndGame();
//        }

//        void bJoinRedTeam_onLeftClick(object sender, EventArgs e)
//        {
//            CTFMessages.RequestSwitchTeam(TeamColor.Red);
//            if(CaptureTheFlag.GameInProgress)
//            {
//                Close();
//            }
//        }

//        void bJoinBlueTeam_onLeftClick(object sender, EventArgs e)
//        {
//            CTFMessages.RequestSwitchTeam(TeamColor.Blue);
//            if (CaptureTheFlag.GameInProgress)
//            {
//                Close();
//            }
//        }

//        void bSpectate_onLeftClick(object sender, EventArgs e)
//        {
//            CTFMessages.RequestSwitchTeam(TeamColor.None);
//            if (CaptureTheFlag.GameInProgress)
//            {
//                Close();
//            }
//        }

//        public void Hide()
//        {
//            this.Visible = false;
//        }

//        public void Show()
//        {
//            this.Visible = true;
//        }

//        public void Close()
//        {
//            Hide();
//        }

//        void AddToConainer(UIView container, string text)
//        {
//            UILabel label = new UILabel(text);
//            label.Scale = .35f;
//            if (container.ChildCount > 0)
//            {
//                UIView lc = container.GetLastChild();
//                label.Y = lc.Y + lc.Height;
//                label.X = lc.X;
//            }
//            else
//            {
//                label.X = 4;
//                label.Y = 4;
//            }
//            container.AddChild(label);
//        }

//        public void PopulateList()
//        {
//            btContainer.RemoveAllChildren();
//            rtContainer.RemoveAllChildren();
//            for(int i = 0; i < Network.Players.Length; i++)
//            {
//                GameikiPlayer player = Network.Players[i];
//                if(player.GameInstance.active)
//                {
//                    if (player.CTFTeam == TeamColor.Red)
//                    {
//                        AddToConainer(rtContainer, player.GameInstance.name);
//                    }
//                    else if(player.CTFTeam == TeamColor.Blue)
//                    {
//                        AddToConainer(btContainer, player.GameInstance.name);
//                    }
//                }
//            }
//        }

//        public override void Update()
//        {
//            if (this.Parent != null && Main.gameMenu) this.Close();
//            CenterToParent();
//            GameikiPlayer myPlayer = Network.Players[Main.myPlayer];
//            bJoinRedTeam.Visible = (CaptureTheFlag.InPregameLobby || CaptureTheFlag.GameInProgress) && myPlayer.CTFTeam != TeamColor.Red;
//            bJoinBlueTeam.Visible = (CaptureTheFlag.InPregameLobby || CaptureTheFlag.GameInProgress) && myPlayer.CTFTeam != TeamColor.Blue;
//            bSpectate.Visible = (CaptureTheFlag.InPregameLobby || CaptureTheFlag.GameInProgress) && myPlayer.CTFTeam != TeamColor.None;
//            if(expirationTimer > 0 && CaptureTheFlag.InPregameLobby)
//            {
//                expirationTimer -= ModUtils.DeltaTime;
//                lobbyExpiration.Visible = true;
//                expirationTimeLabel.Visible = true;

//                int m = (int)expirationTimer / 60;
//                int s = (int)expirationTimer % 60;
//                string strS = s.ToString();
//                if(s < 10)
//                {
//                    strS = "0" + strS;
//                }
//                expirationTimeLabel.Text = m + ":" + strS;
//            }
//            else
//            {
//                lobbyExpiration.Visible = false;
//                expirationTimeLabel.Visible = false;
//            }
//            if (CaptureTheFlag.InPregameLobby) bStartGame.Text = "Start Game";
//            else if (CaptureTheFlag.GameInProgress) bStartGame.Text = "End Game";
//            bStartGame.Visible = CaptureTheFlag.LobbyStartedBy == Main.myPlayer && (CaptureTheFlag.InPregameLobby || CaptureTheFlag.GameInProgress);
//            base.Update();
//        }
//    }

//    class SpectatorView : UIView
//    {

//        static Texture2D _blueScore;
//        static Texture2D blueScoreTexture
//        {
//            get
//            {
//                if (_blueScore == null) _blueScore = ModUtils.GetEmbeddedTexture("Images/CTF/blueScore");
//                return _blueScore;
//            }
//        }

//        static Texture2D _redScore;
//        static Texture2D redScoreTexture
//        {
//            get
//            {
//                if (_redScore == null) _redScore = ModUtils.GetEmbeddedTexture("Images/CTF/redScore");
//                return _redScore;
//            }
//        }

//        static Texture2D _blueIcon;
//        static Texture2D blueIconTexture
//        {
//            get
//            {
//                if (_blueIcon == null) _blueIcon = ModUtils.GetEmbeddedTexture("Images/CTF/blueTeamIcon");
//                return _blueIcon;
//            }
//        }

//        static Texture2D _redIcon;
//        static Texture2D redIconTexture
//        {
//            get
//            {
//                if (_redIcon == null) _redIcon = ModUtils.GetEmbeddedTexture("Images/CTF/redTeamIcon");
//                return _redIcon;
//            }
//        }

//        static Texture2D _blueBanner;
//        static Texture2D blueBannerTexture
//        {
//            get
//            {
//                if (_blueBanner == null) _blueBanner = ModUtils.GetEmbeddedTexture("Images/CTF/blueBanner");
//                return _blueBanner;
//            }
//        }

//        static Texture2D _redBanner;
//        static Texture2D redBannerTexture
//        {
//            get
//            {
//                if (_redBanner == null) _redBanner = ModUtils.GetEmbeddedTexture("Images/CTF/redBanner");
//                return _redBanner;
//            }
//        }

//        static float spacing = 16f;
//        SnoopWindow snoopWindow;
//        UIView redTeam;
//        UIView blueTeam;
//        UILabel lName;
//        int playerToFollow = -1;
//        Vector2 positionBeforeFollowing;
//        UIButton bStopFollowing;
//        UIImage redScore;
//        UIImage blueScore;
//        UILabel lRedScore;
//        UILabel lBlueScore;
//        UILabel lScoreToWin;
//        UILabel lTimeUntilCrossingAllowed;
//        //AutoCam autoCam;
//        //InspectorWindow inspector;
//        //PointsDebug pointsDebug;
//        //UIButton bToggleInspector;
//        public SpectatorView()
//        {
//            Width = 0;
//            Height = 0;
//            OverridesMouse = false;
//            snoopWindow = new SnoopWindow();
//            snoopWindow.UpdateWhenOutOfBounds = true;
//            snoopWindow.Visible = false;
//            redTeam = new UIView();
//            blueTeam = new UIView();
//            lName = new UILabel();
//            bStopFollowing = new UIButton("Stop Following");
//            redScore = new UIImage(redScoreTexture);
//            blueScore = new UIImage(blueScoreTexture);
//            lRedScore = new UILabel("0");
//            lBlueScore = new UILabel("0");
//            lScoreToWin = new UILabel();
//            lScoreToWin.Text = "First to " + CaptureTheFlag.ScoreToWin + " Wins";
//            lScoreToWin.Scale = .5f;
//            lTimeUntilCrossingAllowed = new UILabel("");
//            lTimeUntilCrossingAllowed.Visible = false;

//            redTeam.UpdateWhenOutOfBounds = true;
//            blueTeam.UpdateWhenOutOfBounds = true;
//            lName.UpdateWhenOutOfBounds = true;
//            bStopFollowing.UpdateWhenOutOfBounds = true;
//            redScore.UpdateWhenOutOfBounds = true;
//            blueScore.UpdateWhenOutOfBounds = true;
//            lRedScore.UpdateWhenOutOfBounds = true;
//            lBlueScore.UpdateWhenOutOfBounds = true;
//            lScoreToWin.UpdateWhenOutOfBounds = true;
//            lTimeUntilCrossingAllowed.UpdateWhenOutOfBounds = true;

//            redTeam.Width = 40;
//            blueTeam.Width = redTeam.Width;
//            redTeam.X = spacing;
//            redTeam.Y = 350;
//            blueTeam.Y = redTeam.Y;
//            lName.Anchor = AnchorPosition.Top;
//            bStopFollowing.Anchor = AnchorPosition.Top;
//            bStopFollowing.onLeftClick += bStopFollowing_onLeftClick;
//            lRedScore.Anchor = AnchorPosition.Center;
//            lBlueScore.Anchor = AnchorPosition.Center;
//            lScoreToWin.Anchor = AnchorPosition.Top;
//            lTimeUntilCrossingAllowed.Anchor = AnchorPosition.Top;
//            lRedScore.Scale = .8f;
//            lBlueScore.Scale = lRedScore.Scale;

//            redScore.Y = spacing;
//            blueScore.Y = redScore.Y;
//            lRedScore.Y = redScore.Y + redScore.Height / 2 + 4;
//            lBlueScore.Y = blueScore.Y + blueScore.Height / 2 + 4;
//            lScoreToWin.Y = redScore.Y + redScore.Height + 8;
//            lTimeUntilCrossingAllowed.Y = lScoreToWin.Y + lScoreToWin.Height;
//            lTimeUntilCrossingAllowed.Scale = lScoreToWin.Scale;

//            AddChild(redTeam);
//            AddChild(blueTeam);
//            AddChild(snoopWindow);
//            AddChild(lName);
//            AddChild(bStopFollowing);
//            AddChild(redScore);
//            AddChild(blueScore);
//            AddChild(lRedScore);
//            AddChild(lBlueScore);
//            AddChild(lScoreToWin);
//            AddChild(lTimeUntilCrossingAllowed);

//            /*
//            autoCam = new AutoCam();
//            inspector = new InspectorWindow();
//            inspector.order = false;
//            inspector.SetIncpectorObject(new AutoCamVars());
//            inspector.Y = 300;
//            inspector.X = 100;
//            inspector.Visible = false;
//            UIKit.MasterView.gameScreen.AddChild(inspector);
//            pointsDebug = new PointsDebug();
//            pointsDebug.Y = 300;
//            pointsDebug.Visible = false;
//            UIKit.MasterView.gameScreen.AddChild(pointsDebug);
//            pointsDebug.X = Main.screenWidth - 300;
//            bToggleInspector = new UIButton("Toggle AutoCam Inspector");
//            bToggleInspector.Y = bStopFollowing.Y + bStopFollowing.Height + spacing;
//            bToggleInspector.UpdateWhenOutOfBounds = true;
//            bToggleInspector.Anchor = AnchorPosition.Top;
//            bToggleInspector.onLeftClick += bToggleInspector_onLeftClick;
//            AddChild(bToggleInspector);
//             */
//        }

//        void bToggleInspector_onLeftClick(object sender, EventArgs e)
//        {
//            //inspector.Visible = !inspector.Visible;
//            //pointsDebug.Visible = inspector.Visible;
//        }

//        void bStopFollowing_onLeftClick(object sender, EventArgs e)
//        {
//            playerToFollow = -1;
//            Main.player[Main.myPlayer].position = positionBeforeFollowing;
//        }

//        public override void Update()
//        {
//            if (Main.gameMenu)
//                Close();
//            //autoCam.Update();
//            if (Main.playerInventory) this.Hide();
//            else this.Show();
//            if (CaptureTheFlag.GameInProgress)
//            {

//                if (Network.Players[Main.myPlayer].CTFTeam == TeamColor.None)
//                {
//                    //playerToFollow = AutoCam.followPlayer;
//                    if (playerToFollow >= 0)
//                    {
//                        bStopFollowing.Visible = true;
//                        bStopFollowing.X = Parent.Width / 2;
//                        bStopFollowing.Y = lName.Y + lName.Height + spacing;
//                        //bToggleInspector.X = bStopFollowing.X;
//                        FlyCam.Enabled = true;
//                        Player myPlayer = Main.player[Main.myPlayer];
//                        myPlayer.position = Main.player[playerToFollow].position;
//                        myPlayer.position.X -= Main.screenWidth;
//                        Main.screenPosition = Main.player[playerToFollow].Center;
//                        Main.screenPosition.X -= Main.screenWidth / 2;
//                        Main.screenPosition.Y -= Main.screenHeight / 2;
//                    }
//                    else
//                    {
//                        FlyCam.Enabled = false;
//                        bStopFollowing.Visible = false;
//                    }
//                }
//                else
//                {
//                    FlyCam.Enabled = false;
//                    bStopFollowing.Visible = false;
//                }
//                lRedScore.Text = CaptureTheFlag.RedTeam.Score.ToString();
//                lBlueScore.Text = CaptureTheFlag.BlueTeam.Score.ToString();
//                redScore.X = Parent.Width / 2 - redScore.Width - spacing;
//                lRedScore.X = redScore.X + redScore.Width / 2;
//                blueScore.X = Parent.Width / 2 + spacing;
//                lBlueScore.X = blueScore.X + blueScore.Width / 2;
//                lScoreToWin.X = Parent.Width / 2;
//                lTimeUntilCrossingAllowed.X = Parent.Width / 2;
//                lScoreToWin.Text = "First to " + CaptureTheFlag.ScoreToWin + " Wins";

//                lTimeUntilCrossingAllowed.Visible = !CaptureTheFlag.AllowCrossingSides;
//                if(!CaptureTheFlag.AllowCrossingSides)
//                {
//                    int minutes = (int)CaptureTheFlag.CrossingTimer / 60;
//                    int seconds = (int)CaptureTheFlag.CrossingTimer % 60;
//                    string secondsStr = seconds.ToString();
//                    if(seconds < 10)
//                    {
//                        secondsStr = "0" + seconds;
//                    }
//                    lTimeUntilCrossingAllowed.Text = "Side Protection Time " + minutes + ":" + secondsStr;
//                }
//            }
//            blueTeam.X = Parent.Width - blueTeam.Width - spacing;
//            base.Update();
//        }

//        public void Close()
//        {
//            this.Parent.RemoveChild(this);
//            //inspector.Parent.RemoveChild(inspector);
//            //inspector = null;
//            //pointsDebug.Parent.RemoveChild(pointsDebug);
//            //pointsDebug = null;
//        }

//        public void Hide()
//        {
//            redTeam.Visible = false;
//            blueTeam.Visible = false;
//            snoopWindow.Visible = false;
//            lName.Visible = false;
//        }

//        public void Show()
//        {
//            redTeam.Visible = true;
//            blueTeam.Visible = true;
//        }

//        public void PopulatePlayers()
//        {
//            redTeam.RemoveAllChildren();
//            blueTeam.RemoveAllChildren();
//            for (int i = 0; i < Network.Players.Length; i++)
//            {
//                GameikiPlayer player = Network.Players[i];
//                if (player.GameInstance.active)
//                {
//                    if (player.CTFTeam != TeamColor.None)
//                    {
//                        UIView view = redTeam;

//                        UIPlayerHead playerHead = new UIPlayerHead(player.GameInstance);
//                        UIImage bg = new UIImage(redBannerTexture);
//                        if (player.CTFTeam == TeamColor.Blue)
//                        {
//                            view = blueTeam;
//                            playerHead.lookRight = false;
//                            bg.Texture = blueBannerTexture;
//                        }
//                        bg.Anchor = AnchorPosition.Top;
//                        //bg.ForegroundColor = Color.Blue;
//                        if (view.ChildCount > 0)
//                        {
//                            playerHead.Y = view.GetLastChild().Y + playerHead.Height + spacing;
//                        }
//                        //bg.Scale = 1.3f;
//                        bg.X = playerHead.Width / 2 + 4;
//                        bg.Y = playerHead.Y - 2;
//                        playerHead.Tag = i;
//                        playerHead.onLeftClick += playerHead_onLeftClick;
//                        playerHead.onMouseEnter += playerHead_onMouseEnter;
//                        playerHead.onMouseLeave += playerHead_onMouseLeave;
//                        view.AddChild(bg);
//                        view.AddChild(playerHead);
//                        view.Height = playerHead.Y + playerHead.Height;
//                    }
//                }
//            }
//        }

//        void playerHead_onLeftClick(object sender, EventArgs e)
//        {
//            if (Network.Players[Main.myPlayer].CTFTeam != TeamColor.None)
//            {
//                if (playerToFollow < 0) positionBeforeFollowing = Main.player[Main.myPlayer].position;
//                UIPlayerHead playerHead = (UIPlayerHead)sender;
//                playerToFollow = (int)playerHead.Tag;
//            }
//        }


//        void playerHead_onMouseLeave(object sender, EventArgs e)
//        {
//            snoopWindow.Visible = false;
//            lName.Visible = false;
//        }

//        void playerHead_onMouseEnter(object sender, EventArgs e)
//        {
//            UIPlayerHead playerHead = (UIPlayerHead)sender;
//            GameikiPlayer hoverPlayer = Network.Players[(int)playerHead.Tag];
//            if (Network.Players[Main.myPlayer].CTFTeam == TeamColor.None)
//            {
//                snoopWindow.SetPlayer(playerHead.DrawPlayer);
//                snoopWindow.Visible = true;
//                snoopWindow.X = playerHead.DrawPosition.X + playerHead.Width + spacing;
//                if (hoverPlayer.CTFTeam == TeamColor.Blue) snoopWindow.X = playerHead.DrawPosition.X - snoopWindow.Width - spacing;
//                snoopWindow.Y = playerHead.DrawPosition.Y + playerHead.Height / 2 - snoopWindow.Height / 2;
//                if (snoopWindow.Y + snoopWindow.Height > Parent.Height) snoopWindow.Y = Parent.Height - snoopWindow.Height;
//            }
//            lName.Visible = true;
//            lName.Text = playerHead.DrawPlayer.name;
//            lName.X = Parent.Width / 2;
//            lName.Y = Parent.Height / 2 + 200;
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            //if (Netplay.serverSock[Main.myPlayer].ctfPlayer.team == null)
//            if (CaptureTheFlag.GameInProgress)
//            {
//                Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
//                for (int i = 0; i < Main.player.Length; i++)
//                {
//                    Player player = Main.player[i];
//                    if (player.active && Network.Players[i].CTFTeam != TeamColor.None)
//                    {
//                        Vector2 pos = player.position - Main.screenPosition;
//                        Rectangle playerRect = player.getRect();
//                        if (playerRect.Intersects(screenRect))
//                        {
//                            Texture2D texture = redIconTexture;
//                            if (Network.Players[i].CTFTeam == TeamColor.Blue) texture = blueIconTexture;
//                            pos.X += player.width / 2 - texture.Width / 2;
//                            pos.Y -= texture.Height + 30;
//                            spriteBatch.Draw(texture, pos, Color.White);
//                        }
//                    }
//                }
//            }
//            base.Draw(spriteBatch);
//        }

//    }
//}
