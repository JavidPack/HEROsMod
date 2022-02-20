﻿using On.Terraria.GameContent.NetModules;
using HEROsMod.HEROsModNetwork;
using HEROsMod.HEROsModServices;
using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

// TODO, freeze is bypassable.
// TODO, regions prevent all the chest movement and right click.
// TODO -- Should I have all services use the same Global hooks?
namespace HEROsMod
{
	internal class HEROsMod : Mod
	{
		public static HEROsMod instance;
		internal static Dictionary<string, ModTranslation> translations; // reference to private field.
		internal List<UIKit.UIComponents.ModCategory> modCategories;
		internal Dictionary<string, Action<bool>> crossModGroupUpdated = new Dictionary<string, Action<bool>>();

		public override void Load()
		{
			// Since we are using hooks not in older versions, and since ItemID.Count changed, we need to do this.
			if (ModLoader.version < new Version(0, 10, 1, 5))
			{
				throw new Exception(HEROsMod.HeroText("UpdateTModLoaderToUse"));
			}

			try
			{
				instance = this;

				FieldInfo translationsField = typeof(Mod).GetField("translations", BindingFlags.Instance | BindingFlags.NonPublic);
				translations = (Dictionary<string, ModTranslation>)translationsField.GetValue(this);
				//LoadTranslations();

				modCategories = new List<UIKit.UIComponents.ModCategory>();

				//	AddGlobalItem("HEROsModGlobalItem", new HEROsModGlobalItem());
				// AddPlayer("HEROsModModPlayer", new HEROsModModPlayer());
				//if (ModUtils.NetworkMode != NetworkMode.Server)

				if (!Main.dedServ)
				{
					UIKit.UIButton.buttonBackground = HEROsMod.instance.GetTexture("Images/UIKit/buttonEdge");
					UIKit.UIView.closeTexture = HEROsMod.instance.GetTexture("Images/closeButton");
					UIKit.UITextbox.textboxBackground = HEROsMod.instance.GetTexture("Images/UIKit/textboxEdge");
					UIKit.UISlider.barTexture = HEROsMod.instance.GetTexture("Images/UIKit/barEdge");
					UIKit.UIScrollView.ScrollbgTexture = GetTexture("Images/UIKit/scrollbgEdge");
					UIKit.UIScrollBar.ScrollbarTexture = HEROsMod.instance.GetTexture("Images/UIKit/scrollbarEdge");
					UIKit.UIDropdown.capUp = HEROsMod.instance.GetTexture("Images/UIKit/dropdownCapUp");
					UIKit.UIDropdown.capDown = HEROsMod.instance.GetTexture("Images/UIKit/dropdownCapDown");
					UIKit.UICheckbox.checkboxTexture = HEROsMod.instance.GetTexture("Images/UIKit/checkBox");
					UIKit.UICheckbox.checkmarkTexture = HEROsMod.instance.GetTexture("Images/UIKit/checkMark");
				}

				Init();
			}
			catch (Exception e)
			{
				ModUtils.DebugText("Load:\n" + e.Message + "\n" + e.StackTrace + "\n");
			}
			// Intercept DeserializeAsServer method
			NetTextModule.DeserializeAsServer += NetTextModule_DeserializeAsServer;
		}

		internal static string HeroText(string key)
		{
			return translations[$"Mods.HEROsMod.{key}"].GetTranslation(Language.ActiveCulture);
			// This isn't good until after load....
			// return Language.GetTextValue($"Mods.HEROsMod.{category}.{key}");
		}

		// Clear EVERYthing, mod is unloaded.
		public override void Unload()
		{
			UIKit.UIComponents.ItemBrowser.Filters = null;
			UIKit.UIComponents.ItemBrowser.DefaultSorts = null;
			UIKit.UIComponents.ItemBrowser.Categories = null;
			UIKit.UIComponents.ItemBrowser.CategoriesLoaded = false;
			UIKit.UIButton.buttonBackground = null;
			UIKit.UIView.closeTexture = null;
			UIKit.UITextbox.textboxBackground = null;
			UIKit.UISlider.barTexture = null;
			UIKit.UIScrollView.ScrollbgTexture = null;
			UIKit.UIScrollBar.ScrollbarTexture = null;
			UIKit.UIDropdown.capUp = null;
			UIKit.UIDropdown.capDown = null;
			UIKit.UICheckbox.checkboxTexture = null;
			UIKit.UICheckbox.checkmarkTexture = null;
			HEROsModServices.Login._loginTexture = null;
			HEROsModServices.Login._logoutTexture = null;
			try
			{
				KeybindController.bindings.Clear();
				if (ServiceController != null)
				{
					if (ServiceController.Services != null)
					{
						foreach (var service in ServiceController.Services)
						{
							service.Unload();
						}
					}
					ServiceController.RemoveAllServices();
				}
				HEROsModNetwork.Network.ResetAllPlayers();
				HEROsModNetwork.Network.ServerUsingHEROsMod = false;
				HEROsModNetwork.Network.Regions.Clear();
				MasterView.ClearMasterView();
			}
			catch (Exception e)
			{
				ModUtils.DebugText("Unload:\n" + e.Message + "\n" + e.StackTrace + "\n");
			}
			extensionMenuService = null;
			miscOptions = null;
			prefixEditor = null;
			_hotbar = null;
			ServiceController = null;
			TimeWeatherControlHotbar.Unload();
			ModUtils.previousInventoryItems = null;
			modCategories = null;
			translations = null;
			instance = null;
			NetTextModule.DeserializeAsServer -= NetTextModule_DeserializeAsServer;
		}
		
		private bool NetTextModule_DeserializeAsServer(NetTextModule.orig_DeserializeAsServer orig, Terraria.GameContent.NetModules.NetTextModule self, BinaryReader reader, int senderPlayerId)
		{
			long savedPosition = reader.BaseStream.Position;
			ChatMessage message = ChatMessage.Deserialize(reader);
			reader.BaseStream.Position = savedPosition;

			Color chatColor = Network.Players[senderPlayerId].Group?.Color ?? new Color(255, 255, 255);
			Terraria.Net.NetPacket packet = Terraria.GameContent.NetModules.NetTextModule.SerializeServerMessage(NetworkText.FromLiteral(message.Text), chatColor, (byte)senderPlayerId);
			Terraria.Net.NetManager.Instance.Broadcast(packet);

			return true;
		}

		public override void PostSetupContent()
		{
			if (!Main.dedServ)
			{
				foreach (var service in ServiceController.Services)
				{
					service.PostSetupContent();
				}
			}
		}

		public override void PostDrawFullscreenMap(ref string mouseText)
		{
			Teleporter.instance.PostDrawFullScreenMap();
			MapRevealer.instance.PostDrawFullScreenMap();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int inventoryLayerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (inventoryLayerIndex != -1)
			{
				layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
					"HerosMod: UI",
					delegate
					{
						try
						{
							HEROsMod.Update();

							HEROsMod.ServiceHotbar.Update();

							HEROsMod.DrawBehindUI(Main.spriteBatch);

							HEROsMod.Draw(Main.spriteBatch);

							KeybindController.DoPreviousKeyState();
						}
						catch (Exception e)
						{
							ModUtils.DebugText("HerosMod: UI Error: " + e.Message + e.StackTrace);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);

				// Technically before the above GameInterfaceLayer 
				layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
					"HerosMod: UI (Game Scale)",
					delegate {
						try
						{
							if (ModUtils.NetworkMode != NetworkMode.Server)
							{
								foreach (var service in ServiceController.Services)
								{
									service.UpdateGameScale();
								}
							}
							SelectionTool.Update();
						}
						catch (Exception e)
						{
							ModUtils.DebugText("HerosMod: UI (Game Scale) Error: " + e.Message + e.StackTrace);
						}
						return true;
					},
					InterfaceScaleType.Game)
				);
			}
		}

		public override void HotKeyPressed(string name)
		{
			//	ErrorLogger.Log("HKP " + name);
			KeybindController.HotKeyPressed(name);
		}

		public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			CheckIfGameEnteredOrLeft();
			//Console.WriteLine("?");
			//KeybindController.DoPreviousKeyState();
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			//ErrorLogger.Log("HandlePacket");
			HEROsModNetwork.Network.HEROsModMessaged(reader, whoAmI);
		}

		public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
		{
			if (HEROsModNetwork.Network.CheckIncomingDataForHEROsModMessage(ref messageType, ref reader, playerNumber))
			{
				//ErrorLogger.Log("Hijacking: " + messageType);
				return true;
			}
			return false;
		}

		/*
		private void LoadTranslations()
		{
			// 0.10.1.2 already does this
			if (ModLoader.version >= new Version(0, 10, 1, 2))
				return;

			var modTranslationDictionary = new Dictionary<string, ModTranslation>();
			var translationFiles = File.Where(x => Path.GetExtension(x.Key) == ".lang");
			foreach (var translationFile in translationFiles)
			{
				string translationFileContents = System.Text.Encoding.UTF8.GetString(translationFile.Value);
				GameCulture culture = GameCulture.FromName(Path.GetFileNameWithoutExtension(translationFile.Key));

				using (StringReader reader = new StringReader(translationFileContents))
				{
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						int split = line.IndexOf('=');
						if (split < 0)
							continue; // lines witout a = are ignored
						string key = line.Substring(0, split).Trim().Replace(" ", "_");
						string value = line.Substring(split + 1).Trim();
						if (value.Length == 0)
						{
							continue;
						}
						value = value.Replace("\\n", "\n");
						// TODO: Maybe prepend key with filename: en.US.ItemName.lang would automatically assume "ItemName." for all entries.
						//string key = key;
						ModTranslation mt;
						if (!modTranslationDictionary.TryGetValue(key, out mt))
							modTranslationDictionary[key] = mt = CreateTranslation(key);
						mt.AddTranslation(culture, value);
					}
				}
			}

			foreach (var value in modTranslationDictionary.Values)
			{
				AddTranslation(value);
			}
		}
		*/

		//public override Matrix ModifyTransformMatrix(Matrix Transform)
		//{
		//	if (!Main.gameMenu)
		//	{
		//		return Transform *= HEROsModMod.HEROsModServices.ZoomToolsService.OffsetMatrix;
		//	}
		//	return Transform;
		//}

		//public static bool CreateiveDisabled = false;

		private static bool _prevGameMenu = true;
		//internal ModExtensions modExtensions;

		// Holds all the loaded services.
		public static ServiceController ServiceController;

		public static RenderTarget2D RenderTarget { get; set; }

		private static ServiceHotbar _hotbar;
		public static ServiceHotbar ServiceHotbar
		{
			get { return _hotbar; }
		}

		public static void Init()
		{
			ModUtils.Init();
			//	IncreaseNetworkMessageSize();
			HEROsModNetwork.Network.Init();
			//	HEROsModNetwork.CTF.CaptureTheFlag.Init();

			//if (ModUtils.NetworkMode != NetworkMode.Server)
			if (!Main.dedServ)
			{
				UIView.exclusiveControl = null;
				//HEROsModVideo.Services.DropRateInfo.DropTableBuilder.ImportDropTable();
				InventoryManager.SetKeyBindings();
				//AchievementManger.Load();
				//ZoomToolsService.SetKeyBindings();
				//KeybindController.LoadBindings();
				//UIKit.UIComponents.ItemTooltip.SetKeyBindings();
				//HEROsModVideo.Services.SpeedRunService.SpeedRunTimer.SetKeyBindings();
				//HEROsModVideo.Editor.Editor.SetKeyBindings();
				ServiceController = new ServiceController();
				_hotbar = new ServiceHotbar();
				SelectionTool.Init();

				//PrefixScraper.Scrape();
				//UIKit.MasterView.menuScreen.AddChild(new UIKit.UIComponents.DropTableView(HEROsModVideo.Services.DropRateInfo.DropTableBuilder.DropTable.NPCDropTables[113], 350));
				//UIKit.MasterView.gameScreen.AddChild(new HEROsModVideo.Services.Crafting.CraftingWindow());
				UIKit.UIColorPicker colorPicker = new UIKit.UIColorPicker();
				colorPicker.X = 200;
				UIKit.MasterView.menuScreen.AddChild(colorPicker);

				//InventoryManager.Load();
				//HEROsModVideo.Services.ChestDropsInfo.ChestDropBuilder.LoadCompiledData();
				//HEROsModVideo.Services.ChestDropsInfo.ChestDropBuilder.GenorateWorld();
				LoadAddServices();
			}
			//instance.modExtensions = new ModExtensions();
		}

		public override object Call(params object[] args)
		{
			int argsLength = args.Length;
			Array.Resize(ref args, 6);

			try
			{
				string message = args[0] as string;
				if (message == "AddSimpleButton")
				{
					ModUtils.DebugText("Button Adding...");
					RegisterButton(
						args[1] as string,
						args[2] as Texture2D,
						args[3] as Action,
						args[4] as Action<bool>,
						args[5] as Func<string>
					);
					ModUtils.DebugText("...Button Added");
				}
				else if (message == "AddPermission")
				{
					ModUtils.DebugText("Permission Adding...");
					// Internal,
					RegisterPermission(
						args[1] as string,
						args[2] as string,
						args[3] as Action<bool>
					);
					ModUtils.DebugText("...Permission Added");
				}
				else if (message == "AddItemCategory")
				{
					ModUtils.DebugText("Item Category Adding...");
					string sortName = args[1] as string;
					string parentName = args[2] as string;
					Predicate<Item> belongs = args[3] as Predicate<Item>;
					if (!Main.dedServ)
						modCategories.Add(new UIKit.UIComponents.ModCategory(sortName, parentName, belongs));
					ModUtils.DebugText("...Item Category Added");
				}
				else if (message == "HasPermission")
				{
					if (/*Main.netMode != Terraria.ID.NetmodeID.Server ||*/ argsLength != 3) // for now, only allow this call on Server (2) --> why??
						return false;
					//int player = Convert.ToInt32(args[1]); // Convert.ToInt32 doesn't throw exception, casting does. Exception is better in this case.
					//string permission = args[2] as string;
					return Network.Players[(int)args[1]].Group?.HasPermission(args[2] as string) ?? false; // Group might be null, so checking permissions on entering world won't work reliably.
				}
				else if (message == "HideHotbar")
				{
					if (!ServiceHotbar.Collapsed)
					{
						ServiceHotbar.collapseArrow_onLeftClick(null, null);
						if(!ServiceHotbar.Collapsed) // sub hotbars
							ServiceHotbar.collapseArrow_onLeftClick(null, null);
					}
				}
				else
				{
					Logger.Error("Call Error: Unknown Message: " + message);
				}
			}
			catch (Exception e)
			{
				Logger.Error("Call Error: " + e.StackTrace + e.Message);
			}
			return null;
		}

		public void RegisterButton(string permissionName, Texture2D texture, Action buttonClickedAction, Action<bool> groupUpdated, Func<string> tooltip)
		{
			if (!Main.dedServ)
			{
				GenericExtensionService genericService = new GenericExtensionService(instance.extensionMenuService, texture, permissionName, buttonClickedAction, groupUpdated, tooltip);
				ServiceController.AddService(genericService);
				instance.extensionMenuService.AddGeneric(genericService);
				//modExtensions.AddButton(texture, buttonClickedAction, groupUpdated, tooltip);
			}
		}

		public void RegisterPermission(string permissionName, string permissionDisplayName, Action<bool> groupUpdated)
		{
			ModUtils.DebugText($"RegisterPermission: {permissionName} - {permissionDisplayName}");
			Group.PermissionList.Add(new PermissionInfo(permissionName, permissionDisplayName));
			//foreach (var item in Network.Groups)
			//{
			//}
			//Network.DefaultGroup.Permissions.Add(permissionName, false);
			Network.AdminGroup.Permissions.Add(permissionName, true);

			if (groupUpdated != null)
			{
				crossModGroupUpdated[permissionName] = groupUpdated;
			}

			//modExtensions.AddButton(texture, buttonClickedAction, tooltip);
		}

		private MiscOptions miscOptions;
		internal PrefixEditor prefixEditor;
		private ExtensionMenuService extensionMenuService;

		// TODO, is this ok to do on load rather than on enter?
		public static void LoadAddServices()
		{
			//Console.WriteLine("Game entered");
			//ErrorLogger.Log("Game Entered");

			//if (CreateiveDisabled)
			//{
			//	ServiceHotbar.Visible = false;
			//	ServiceController.AddService(new PSAService());
			//	ServiceController.AddService(new WikiService());
			//	ServiceController.AddService(new InventoryManager());
			//	return;
			//}

			//ServiceController.AddService(new WeatherChanger());
			//ServiceController.AddService(new TimeChanger());
			//	ServiceController.AddService(new WikiService());
			//ServiceController.AddService(new TestHotbarService());

			ServiceController.AddService(new ItemBrowser());
			ServiceController.AddService(new InfiniteReach());
			ServiceController.AddService(new FlyCam());
			ServiceController.AddService(new EnemyToggler());
			ServiceController.AddService(new ItemClearer());
			ServiceController.AddService(new TimeWeatherChanger());
			ServiceController.AddService(new Waypoints());
			ServiceController.AddService(new InventoryManager());
			ServiceController.AddService(new MobSpawner());
			ServiceController.AddService(new BuffService());
			ServiceController.AddService(new GodModeService());
			instance.prefixEditor = new PrefixEditor();
			ServiceController.AddService(instance.prefixEditor);
			//		ServiceController.AddService(new InvasionService());
			ServiceController.AddService(new Teleporter());
			ServiceController.AddService(new RegionService());
			ServiceController.AddService(new CheckTileModificationTool());
			ServiceController.AddService(new PlayerList());

			instance.miscOptions = new MiscOptions();
			ServiceController.AddService(instance.miscOptions);
			ServiceController.AddService(new SpawnPointSetter(instance.miscOptions.Hotbar));
			ServiceController.AddService(new MapRevealer(instance.miscOptions.Hotbar));
			ServiceController.AddService(new LightHack(instance.miscOptions.Hotbar));
			ServiceController.AddService(new ItemBanner(instance.miscOptions.Hotbar));
			ServiceController.AddService(new ToggleGravestones(instance.miscOptions.Hotbar));
			ServiceController.AddService(new GroupInspector(instance.miscOptions.Hotbar));

			instance.extensionMenuService = new ExtensionMenuService();
			ServiceController.AddService(instance.extensionMenuService);

			ServiceController.AddService(new Login());

			if (ModContent.GetInstance<HEROsModServerConfig>().Telemetry)
			{
				ServiceController.AddService(new PSAService());
				ServiceController.AddService(new StatsService());
			}

			//ServiceController.AddService(new HardmodeEnemyToggler(multiplayerOption.Hotbar));
			//ServiceController.AddService(new ZoomToolsService());
			//	ServiceController.AddService(new HEROsModVideo.Services.SpeedRunService.SpeedRunTimer());
			//	ServiceController.AddService(new HEROsModVideo.Services.Crafting.CraftingInfoService());
			//ServiceController.AddService(new HEROsModVideo.Editor.Editor());

			ServiceHotbar.Visible = true;

			//if (ModUtils.NetworkMode == NetworkMode.Client)
			{
				//	ServiceHotbar.Visible = HEROsModNetwork.Network.ServerUsingHEROsMod;
				//	ServiceController.AddService(new CTFService());
				//ServiceController.MyGroupChanged();
			}
			//HEROsModVideo.Services.NPCSpawnData.NPCSpawnDataBuilder.Start();
			//HEROsModVideo.Services.ChestDropsInfo.ChestDropBuilder.Start();
		}

		public static void Update(/*GameTime gameTime*/)
		{
			if (ModUtils.NetworkMode != NetworkMode.Server)
			{
				ModUtils.PreviousKeyboardState = Main.keyState;
				ModUtils.PreviousMouseState = ModUtils.MouseState;
				ModUtils.MouseState = Mouse.GetState();

				ModUtils.SetDeltaTime(/*gameTime*/);
				ModUtils.Update();
				//HEROsModVideo.Services.MobHUD.MobInfo.Update();
				//CheckIfGameEnteredOrLeft();
				//Update all services in the ServiceController
				foreach (var service in ServiceController.Services)
				{
					service.Update();
				}
				MasterView.UpdateMaster();
				//if (Main.ingameOptionsWindow && (IngameOptions.category == 2 || IngameOptions.category == 3))
				//{
				//	HEROsModMod.UIKit.MasterView.gameScreen.AddChild(new HEROsModMod.UIKit.UIComponents.KeybindWindow());
				//	IngameOptions.Close();
				//}

				// This is the alternate tooltip code.
				//if (!Main.gameMenu)
				//{
				//	ModUtils.ItemTooltip.Update();
				//}

				// Unused 3D code

				//float speed = .03f;
				//if (Main.keyState.IsKeyDown(Keys.Left))
				//{
				//	angle -= speed;
				//}
				//if (Main.keyState.IsKeyDown(Keys.Right))
				//{
				//	angle += speed;
				//}
				//if (Main.keyState.IsKeyDown(Keys.Up))
				//{
				//	angle2 -= speed;
				//}
				//if (Main.keyState.IsKeyDown(Keys.Down))
				//{
				//	angle2 += speed;
				//}
				//if (Main.keyState.IsKeyDown(Keys.X))
				//{
				//	zoom += speed;
				//}
				//if (Main.keyState.IsKeyDown(Keys.Z))
				//{
				//	zoom -= speed;

				//}

				//Matrix worldMatrix = Matrix.Identity
				//	* Matrix.CreateTranslation(new Vector3(-Main.screenWidth / 2, -Main.screenHeight / 2, 0f))
				//	* Matrix.CreateRotationX(angle2)
				//	* Matrix.CreateRotationY(angle)
				//	* Matrix.CreateTranslation(new Vector3(Main.screenWidth / 2 / zoom, Main.screenHeight / 2 / zoom, 0f))
				//	* Matrix.CreateScale(zoom);

				//   ModUtils.TextureExtruder.WorldView = worldMatrix;
			}
			HEROsModNetwork.Network.Update();
			//	HEROsModNetwork.CTF.CaptureTheFlag.Update();
		}

		//Not working since update not called in title screen.
		private static void CheckIfGameEnteredOrLeft()
		{
			if (Main.gameMenu && !_prevGameMenu)
			{
				GameLeft();
			}
			else if (!Main.gameMenu && _prevGameMenu)
			{
				GameEntered();
			}
			_prevGameMenu = Main.gameMenu;
		}

		public static void GameEntered()
		{
			ModUtils.DebugText("Game Entered");

			if (ModUtils.NetworkMode == NetworkMode.None)
			{
				foreach (HEROsModService service in ServiceController.Services)
				{
					service.HasPermissionToUse = !service.MultiplayerOnly;
				}
				ServiceController.ServiceRemovedCall();
			}
			else
			{
				foreach (HEROsModService service in ServiceController.Services)
				{
					service.HasPermissionToUse = true;
				}
				ServiceController.MyGroupChanged();
			}
			//ServiceController.MyGroupChanged();
		}

		public static void GameLeft()
		{
			ModUtils.DebugText("Game left");
			Login.LoggedIn = false;
			ServiceController.MyGroupChanged();
		}

		public override void PreSaveAndQuit()
		{
			instance.prefixEditor.PreSaveAndQuit();
		}

		//public static void SaveSettings()
		//{
		//	InventoryManager.Save();
		//}

		public static void Draw(SpriteBatch spriteBatch)
		{
			UIKit.MasterView.DrawMaster(spriteBatch);
			//if (Main.gameMenu)
			//{
			//	ModUtils.DrawModVersion(spriteBatch);
			//}
			//else
			{
				foreach (var service in ServiceController.Services)
				{
					service.Draw(spriteBatch);
				}
				//ModUtils.ItemTooltip.Draw(spriteBatch);
				if (Main.mapFullscreen)
				{
					//HEROsModVideo.Services.NPCSpawnData.NPCSpawnDataBuilder.DrawOnMap(spriteBatch);
				}
			}

			float x = Main.fontMouseText.MeasureString(UIView.HoverText).X;
			Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
			if (vector.Y > (float)(Main.screenHeight - 30))
			{
				vector.Y = (float)(Main.screenHeight - 30);
			}
			if (vector.X > (float)Main.screenWidth - x)
			{
				vector.X = (float)(Main.screenWidth - x - 30);
			}
			Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, UIView.HoverText, vector.X, vector.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero, 1f);
		}

		public static void DrawBehindUI(SpriteBatch spriteBatch)
		{
			if (!Main.gameMenu)
			{
				HEROsModVideo.Services.MobHUD.MobInfo.Draw(spriteBatch);
				//HEROsModNetwork.CTF.CaptureTheFlag.Draw(spriteBatch);
			}
		}
	}
}