using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ModLoader;

namespace HEROsMod
{
	internal static class ModUtils
	{
		//private static MethodInfo _drawPlayerHeadMethod;
		private static MethodInfo _loadPlayersMethod;
		//private static MethodInfo _startRainMethod;
		//private static MethodInfo _stopRainMethod;
		private static MethodInfo _startSandstormMethod;
		private static MethodInfo _stopSandstormMethod;

		//private static MethodInfo _loadNPCMethod;
		//private static MethodInfo _loadProjectileMethod;
		//private static MethodInfo _loadTilesMethod;
		private static MethodInfo _mouseTextMethod;

		private static MethodInfo _invasionWarningMethod;
		private static MethodInfo _itemSortingSortMethod;
		private static FieldInfo _npcDefaultSpawnRate;
		private static FieldInfo _npcDefaultMaxSpawns;

		private static PropertyInfo _steamid;

		//   private static FieldInfo _mapIconTextures;
		private static FieldInfo _hueTexture;

		//   private static FieldInfo _hoverItem;

		private static Texture2D _dummyTexture;
		private static float _deltaTime;

		private static Texture2D _logoTexture;
		private static Texture2D _logoTexture2;
		private static Texture2D _testTubeTexture;

		internal static Item[] previousInventoryItems;

		public static event EventHandler InventoryChanged;

		public static bool InterfaceVisible { get; set; }
		//public static HEROsModMod.HEROsModVideo.TextureExtruder TextureExtruder { get; set; }

		/// <summary>
		/// A 1x1 pixel white texture.
		/// </summary>
		public static Texture2D DummyTexture
		{
			get
			{
				if (_dummyTexture == null)
				{
					_dummyTexture = new Texture2D(Main.instance.GraphicsDevice, 1, 1);
					_dummyTexture.SetData(new Color[] { Color.White });
				}
				return _dummyTexture;
			}
		}

		public static KeyboardState PreviousKeyboardState { get; set; }
		public static MouseState MouseState { get; set; }
		public static MouseState PreviousMouseState { get; set; }
		//public static UIKit.UIComponents.ItemTooltip ItemTooltip { get; set; }

		/// <summary>
		/// Time in seconds that has passed since the last update call.
		/// </summary>
		public static float DeltaTime
		{
			get { return _deltaTime; }
		}

		public static int NPCDefaultSpawnRate
		{
			get { return (int)_npcDefaultSpawnRate.GetValue(null); }
			set { _npcDefaultSpawnRate.SetValue(null, value); }
		}

		public static int NPCDefaultMaxSpawns
		{
			get { return (int)_npcDefaultMaxSpawns.GetValue(null); }
			set { _npcDefaultMaxSpawns.SetValue(null, value); }
		}

		public static string SteamID
		{
			get { return (string)_steamid.GetValue(null, null); }
		}

		//public static Texture2D RevealMapTexture
		//{
		//	get
		//	{
		//		return Main.mapIconTexture[7];
		//		//Texture2D[] textures = (Texture2D[])_mapIconTextures.GetValue(Main.instance);
		//		//return textures[7];
		//	}
		//}

		public static Texture2D HueTexture
		{
			get
			{
				return TextureAssets.Hue.Value;
				//return (Texture2D)_hueTexture.GetValue(Main.instance);
			}
		}

		//public static Item HoverItem
		//{
		//	get { return Main.HoverItem; }// (Item)_hoverItem.GetValue(null); }
		//	set { Main.HoverItem = value; }// _hoverItem.SetValue(null, value); }
		//}

		/// <summary>
		/// Gets or Sets if the game camera is free to move from the players position
		/// </summary>
		public static bool FreeCamera { get; set; }

		public static NetworkMode NetworkMode
		{
			get
			{
				return (NetworkMode)Main.netMode;
			}
		}

		/// <summary>
		/// Server Side Characters Enabled
		/// </summary>
		public static bool SSC
		{
			get
			{
				return Main.ServerSideCharacter;
			}
		}

		public static void Init()
		{
			InitReflection();
			InterfaceVisible = true;

			if (NetworkMode != NetworkMode.Server)
			{
				FreeCamera = false;
				//keyBoardInput.ClearEventListener();
				//StartListeningForKeyEvents();
				//_logoTexture = HEROsMod.instance.GetTexture("Images/LogoNew0");
				//_logoTexture2 = HEROsMod.instance.GetTexture("Images/LogoNew1");
				//_testTubeTexture = HEROsMod.instance.GetTexture("Images/testTubeSpritesheet");
				//ItemTooltip = new UIKit.UIComponents.ItemTooltip();
				//UIKit.MasterView.gameScreen.AddChild(ItemTooltip);
				previousInventoryItems = new Item[Main.player[Main.myPlayer].inventory.Length];
				SetPreviousInventory();
				Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -10000, 10000);
				//Effect effect = HEROsMod.Content.Load<Effect>("effects");
				//TextureExtruder = new HEROsModVideo.TextureExtruder(projection, effect, HEROsMod.GraphicsDeviceReference);
			}
		}

		private static void InitReflection()
		{
			try
			{
				//	Main.DrawPlayerHead
				//_drawPlayerHeadMethod = Main.instance.GetType().GetMethod("DrawPlayerHead", BindingFlags.NonPublic | BindingFlags.Instance);
				_loadPlayersMethod = typeof(Main).GetMethod("LoadPlayers", BindingFlags.NonPublic | BindingFlags.Static);
				//_startRainMethod = typeof(Main).GetMethod("StartRain", BindingFlags.NonPublic | BindingFlags.Static);
				//_stopRainMethod = typeof(Main).GetMethod("StopRain", BindingFlags.NonPublic | BindingFlags.Static);
				_startSandstormMethod = typeof(Sandstorm).GetMethod("StartSandstorm", BindingFlags.NonPublic | BindingFlags.Static);
				_stopSandstormMethod = typeof(Sandstorm).GetMethod("StopSandstorm", BindingFlags.NonPublic | BindingFlags.Static);
				//   _loadNPCMethod = typeof(Main).GetMethod("LoadNPC", BindingFlags.NonPublic | BindingFlags.Instance);
				//  _loadProjectileMethod = typeof(Main).GetMethod("LoadProjectile", BindingFlags.NonPublic | BindingFlags.Instance);
				// _loadTilesMethod = typeof(Main).GetMethod("LoadTiles", BindingFlags.NonPublic | BindingFlags.Instance);
				_mouseTextMethod = typeof(Main).GetMethod("MouseText", BindingFlags.NonPublic | BindingFlags.Instance);
				_invasionWarningMethod = typeof(Main).GetMethod("InvasionWarning", BindingFlags.NonPublic | BindingFlags.Static);
				_npcDefaultSpawnRate = typeof(NPC).GetField("defaultSpawnRate", BindingFlags.NonPublic | BindingFlags.Static);
				_npcDefaultMaxSpawns = typeof(NPC).GetField("defaultMaxSpawns", BindingFlags.NonPublic | BindingFlags.Static);

				_steamid = typeof(ModLoader).GetProperty("SteamID64", BindingFlags.NonPublic | BindingFlags.Static);

				//	_mapIconTextures = Main.instance.GetType().GetField("mapIconTexture", BindingFlags.NonPublic | BindingFlags.Instance); // pub
				_hueTexture = Main.instance.GetType().GetField("hueTexture", BindingFlags.NonPublic | BindingFlags.Instance); // private
																															  //   _hoverItem = typeof(Main).GetField("toolTip", BindingFlags.NonPublic | BindingFlags.Static); //Main.toolTip
				Assembly terraria = Assembly.GetAssembly(typeof(Main));
				_itemSortingSortMethod = terraria.GetType("Terraria.UI.ItemSorting").GetMethod("Sort", BindingFlags.Public | BindingFlags.Static);
			}
			catch (Exception e)
			{
				ModUtils.DebugText(e.Message + " " + e.StackTrace);
			}
		}

		public static void Update()
		{
			if (!Main.gameMenu)
			{
				if (ItemChanged())
				{
					if (InventoryChanged != null)
					{
						InventoryChanged(null, EventArgs.Empty);
					}
					SetPreviousInventory();
				}
			}
		}

		private static bool ItemChanged()
		{
			Player player = Main.player[Main.myPlayer];
			for (int i = 0; i < player.inventory.Length - 1; i++)
			{
				if (player.inventory[i].IsNotSameTypePrefixAndStack(previousInventoryItems[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static void SetPreviousInventory()
		{
			Player player = Main.player[Main.myPlayer];
			for (int i = 0; i < player.inventory.Length; i++)
			{
				previousInventoryItems[i] = player.inventory[i].Clone();
			}
		}

		/// <summary>
		/// Draw the head of a player on screen
		/// </summary>
		/// <param name="player">Player who's head is to be drawn</param>
		/// <param name="x">X Draw Pos</param>
		/// <param name="y">Y Draw Pos</param>
		/// <param name="alpha">Draw Alpha</param>
		/// <param name="scale">Draw Scale</param>
		//public static void DrawPlayerHead(Player player, float x, float y, float alpha = 1f, float scale = 1f)
		//{
		//	_drawPlayerHeadMethod.Invoke(Main.instance, new object[] { player, x, y, alpha, scale });
		//}

		public static void LoadPlayers()
		{
			_loadPlayersMethod.Invoke(null, null);
		}

		public static void StartRain()
		{
			//_startRainMethod.Invoke(null, null);
			Main.StartRain();
		}

		public static void StopRain()
		{
			//_stopRainMethod.Invoke(null, null);
			Main.StopRain();
		}

		public static void StartSandstorm()
		{
			_startSandstormMethod.Invoke(null, null);
		}

		public static void StopSandstorm()
		{
			_stopSandstormMethod.Invoke(null, null);
		}

		public static void LoadNPC(int i)
		{
			Main.instance.LoadNPC(i);
		}

		public static void LoadProjectile(int i)
		{
			Main.instance.LoadProjectile(i);
		}

		public static void LoadTiles(int i)
		{
			Main.instance.LoadTiles(i);
		}

		public static void MouseText(string cursorText, int rare = 0, byte diff = 0)
		{
			_mouseTextMethod.Invoke(Main.instance, new object[] { cursorText, rare, diff });
		}

		public static void InvasionWarning()
		{
			_invasionWarningMethod.Invoke(null, null);
		}

		public static void Sort()
		{
			_itemSortingSortMethod.Invoke(null, null);
		}

		public static void MoveToPosition(Vector2 newPos)
		{
			Player player = Main.player[Main.myPlayer];
			player.position = newPos;
			player.velocity = Vector2.Zero;
			player.fallStart = (int)(player.position.Y / 16f);
		}

		//private static void KeyEvent(char obj)
		//{
		//    if (Main.keyCount < 10)
		//    {
		//        Main.keyInt[Main.keyCount] = (int)obj;
		//        Main.keyString[Main.keyCount] = string.Concat(obj);
		//        Main.keyCount++;
		//    }
		//}

		//public static void StartListeningForKeyEvents()
		//{
		//    keyBoardInput.newKeyEvent += KeyEvent;
		//}

		//public static void StopListeningForKeyEvents()
		//{
		//    keyBoardInput.newKeyEvent -= KeyEvent;
		//}

		/// <summary>
		/// Set the Delta Time
		/// </summary>
		/// <param name="gameTime">Games current Game Time</param>
		public static void SetDeltaTime(/*GameTime gameTime*/)
		{
			_deltaTime = 1f / 60f;// (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		public static void SetDeltaTime(float deltaTime)
		{
			_deltaTime = deltaTime;
		}

		public static bool StringStartsWith(string str, string startStr)
		{
			if (str.Length >= startStr.Length)
			{
				if (str.Substring(0, startStr.Length) == startStr) return true;
			}
			return false;
		}

		public static string GetEndOfString(string str, string startStr)
		{
			return str.Substring(startStr.Length, str.Length - startStr.Length);
		}

		//public static void SelectedMenuHook(ref int selectedMenu)
		//{
		//    switch (Main.menuMode)
		//    {
		//        case 0: //if main menu
		//            switch (selectedMenu) //Main menu
		//            {
		//                case 0: // Single Player
		//                    HEROsMod.CreateiveDisabled = true;
		//                    Main.PlaySound(10, -1, -1, 1);
		//                    Main.menuMode = 1;
		//                    LoadPlayers();
		//                    break;
		//                case 1: // Creative Mode
		//                    Main.PlaySound(10, -1, -1, 1);
		//                    Main.menuMode = 1;
		//                    LoadPlayers();
		//                    break;
		//                case 2: // Mulitplayer
		//                    Main.PlaySound(10, -1, -1, 1);
		//                    Main.menuMode = 12;
		//                    break;
		//                case 3: // Settings
		//                    Main.PlaySound(10, -1, -1, 1);
		//                    Main.menuMode = 11;
		//                    break;
		//                case 4: // Exit
		//                   //Steam.Kill();
		//                    Main.instance.Exit();
		//                    break;
		//            }
		//            selectedMenu = -1;
		//            break;
		//        case 12:
		//            switch (selectedMenu)
		//            {
		//                case 0:
		//                    LoadPlayers();
		//  Main.menuMultiplayer = true;
		//  Main.PlaySound(10, -1, -1, 1);
		//  Main.menuMode = 1;
		//                    break;
		//                case 1:
		//                    Main.menuMode = -2;
		//                    UIKit.MasterView.menuScreen.AddChild(new UIKit.UIComponents.HostPlayWindow());
		//                    break;
		//                case 2:
		//                    Main.PlaySound(11, -1, -1, 1);
		//  Main.menuMode = 0;
		//                    break;
		//            }
		//            selectedMenu = -1;
		//            break;
		//    }
		//}

		//public static void MainMenuHook(ref string[] menuItems, ref int selectedMenu, ref int numberOfMenuItems, ref int spacing)
		//{
		//    switch (Main.menuMode)
		//    {
		//        case 0:
		//            Main.eclipse = false;
		//            Main.pumpkinMoon = false;
		//            Main.snowMoon = false;
		//            Main.ServerSideCharacter = false;
		//            Main.menuMultiplayer = false;
		//            Main.menuServer = false;
		//            Main.netMode = 0;
		//            menuItems[0] = Lang.menu[12];
		//            menuItems[1] = "Creative Mode";
		//            menuItems[2] = Lang.menu[13];
		//            menuItems[3] = Lang.menu[14];
		//            menuItems[4] = Lang.menu[15];
		//            numberOfMenuItems = 5;
		//            spacing = 73;
		//            break;
		//        case 27: //if controls window
		//            Main.menuMode = -2;
		//            UIKit.MasterView.menuScreen.AddChild(new HEROsModMod.UIKit.UIComponents.KeybindWindow());
		//            break;
		//    }
		//}

		//public static void DrawPlayerChat(ref int textBlinkerCount, ref int textBlinkerState)
		//{
		//    Vector2 offset = HEROsMod.ServiceHotbar.ChatOffsetPosition;
		//    if (Main.chatMode)
		//    {
		//        textBlinkerCount++;
		//        if (textBlinkerCount >= 20)
		//        {
		//            if (textBlinkerState == 0)
		//            {
		//                textBlinkerState = 1;
		//            }
		//            else
		//            {
		//                textBlinkerState = 0;
		//            }
		//            textBlinkerCount = 0;
		//        }
		//        string text = Main.chatText;
		//        if (textBlinkerState == 1)
		//        {
		//            text += "|";
		//        }
		//        if (Main.screenWidth > 800)
		//        {
		//            int i = Main.screenWidth - 300;
		//            int num = 78;
		//            Main.spriteBatch.Draw(Main.textBackTexture, new Vector2((float)num, (float)(Main.screenHeight - 36)) + offset, new Rectangle?(new Rectangle(0, 0, Main.textBackTexture.Width - 100, Main.textBackTexture.Height)), new Color(100, 100, 100, 100), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		//            i -= 400;
		//            num += 400;
		//            while (i > 0)
		//            {
		//                if (i > 300)
		//                {
		//                    Main.spriteBatch.Draw(Main.textBackTexture, new Vector2((float)num, (float)(Main.screenHeight - 36)) + offset, new Rectangle?(new Rectangle(100, 0, Main.textBackTexture.Width - 200, Main.textBackTexture.Height)), new Color(100, 100, 100, 100), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		//                    i -= 300;
		//                    num += 300;
		//                }
		//                else
		//                {
		//                    Main.spriteBatch.Draw(Main.textBackTexture, new Vector2((float)num, (float)(Main.screenHeight - 36)) + offset, new Rectangle?(new Rectangle(Main.textBackTexture.Width - i, 0, Main.textBackTexture.Width - (Main.textBackTexture.Width - i), Main.textBackTexture.Height)), new Color(100, 100, 100, 100), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		//                    i = 0;
		//                }
		//            }
		//        }
		//        else
		//        {
		//            Main.spriteBatch.Draw(Main.textBackTexture, new Vector2(78f, (float)(Main.screenHeight - 36)) + offset, new Rectangle?(new Rectangle(0, 0, Main.textBackTexture.Width, Main.textBackTexture.Height)), new Color(100, 100, 100, 100), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		//        }
		//        for (int j = 0; j < 5; j++)
		//        {
		//            int num2 = 0;
		//            int num3 = 0;
		//            Color black = Color.Black;
		//            if (j == 0)
		//            {
		//                num2 = -2;
		//            }
		//            if (j == 1)
		//            {
		//                num2 = 2;
		//            }
		//            if (j == 2)
		//            {
		//                num3 = -2;
		//            }
		//            if (j == 3)
		//            {
		//                num3 = 2;
		//            }
		//            if (j == 4)
		//            {
		//                black = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		//            }
		//            Main.spriteBatch.DrawString(Main.fontMouseText, text, new Vector2((float)(88 + num2), (float)(Main.screenHeight - 30 + num3)) + offset, black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		//        }
		//    }
		//    int num4 = Main.startChatLine;
		//    int num5 = Main.startChatLine + Main.showCount;
		//    if (num5 >= Main.numChatLines)
		//    {
		//        num5 = --Main.numChatLines;
		//        num4 = num5 - Main.showCount;
		//    }
		//    int num6 = 0;
		//    for (int k = num4; k < num5; k++)
		//    {
		//        if (Main.chatMode || Main.chatLine[k].showTime > 0)
		//        {
		//            float num7 = (float)Main.mouseTextColor / 255f;
		//            for (int j = 0; j < 5; j++)
		//            {
		//                int num2 = 0;
		//                int num3 = 0;
		//                Color black = Color.Black;
		//                if (j == 0)
		//                {
		//                    num2 = -2;
		//                }
		//                if (j == 1)
		//                {
		//                    num2 = 2;
		//                }
		//                if (j == 2)
		//                {
		//                    num3 = -2;
		//                }
		//                if (j == 3)
		//                {
		//                    num3 = 2;
		//                }
		//                if (j == 4)
		//                {
		//                    black = new Color((int)((byte)((float)Main.chatLine[k].color.R * num7)), (int)((byte)((float)Main.chatLine[k].color.G * num7)), (int)((byte)((float)Main.chatLine[k].color.B * num7)), (int)Main.mouseTextColor);
		//                }
		//                Main.spriteBatch.DrawString(Main.fontMouseText, Main.chatLine[k].text, new Vector2((float)(88 + num2), (float)(Main.screenHeight - 30 + num3 - 28 - num6 * 21)) + offset, black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		//            }
		//        }
		//        num6++;
		//    }
		//}

		public static Vector2 CursorPosition
		{
			get
			{
				return new Vector2(Main.mouseX, Main.mouseY);
			}
		}

		public static Vector2 CursorWorldCoords
		{
			get
			{
				return CursorPosition + Main.screenPosition;
			}
		}

		public static Vector2 CursorTileCoords
		{
			get
			{
				return GetTileCoordsFromWorldCoords(GetCursorWorldCoords());
			}
		}

		public static Vector2 GetCursorWorldCoords()
		{
			return new Vector2((int)Main.screenPosition.X + Main.mouseX, (int)Main.screenPosition.Y + Main.mouseY);
		}

		public static Vector2 GetTileCoordsFromWorldCoords(Vector2 worldCoords)
		{
			return new Vector2((int)worldCoords.X / 16, (int)worldCoords.Y / 16);
		}

		public static Vector2 GetWorldCoordsFromTileCoords(Vector2 tileCoords)
		{
			return new Vector2((int)tileCoords.X * 16, (int)tileCoords.Y * 16);
		}

		public static void DrawBorderedRect(SpriteBatch spriteBatch, Color color, Color borderColor, Vector2 position, Vector2 size, int borderWidth)
		{
			size *= 16;
			Vector2 pos = ModUtils.GetWorldCoordsFromTileCoords(position) - Main.screenPosition;
			spriteBatch.Draw(ModUtils.DummyTexture, new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y), color);

			spriteBatch.Draw(ModUtils.DummyTexture, new Rectangle((int)pos.X - borderWidth, (int)pos.Y - borderWidth, (int)size.X + borderWidth * 2, borderWidth), borderColor);
			spriteBatch.Draw(ModUtils.DummyTexture, new Rectangle((int)pos.X - borderWidth, (int)pos.Y + (int)size.Y, (int)size.X + borderWidth * 2, borderWidth), borderColor);
			spriteBatch.Draw(ModUtils.DummyTexture, new Rectangle((int)pos.X - borderWidth, (int)pos.Y, (int)borderWidth, (int)size.Y), borderColor);
			spriteBatch.Draw(ModUtils.DummyTexture, new Rectangle((int)pos.X + (int)size.X, (int)pos.Y, (int)borderWidth, (int)size.Y), borderColor);
		}

		public static void DrawBorderedRect(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size, int borderWidth)
		{
			Color fillColor = color * .3f;
			ModUtils.DrawBorderedRect(spriteBatch, fillColor, color, position, size, borderWidth);
		}

		public static void DrawBorderedRect(SpriteBatch spriteBatch, Color color, Vector2 position, Vector2 size, int borderWidth, string text)
		{
			DrawBorderedRect(spriteBatch, color, position, size, borderWidth);
			Vector2 pos = ModUtils.GetWorldCoordsFromTileCoords(position) - Main.screenPosition;
			pos.X += 2;
			pos.Y += 2;
			spriteBatch.DrawString(FontAssets.MouseText.Value, text, pos, Color.White, 0f, Vector2.Zero, .7f, SpriteEffects.None, 0);
		}

		//public static void DrawModVersion(SpriteBatch spriteBatch)
		//{
		//    Vector2 versionSize = Main.fontMouseText.MeasureString(Main.versionNumber);
		//    Vector2 pos = new Vector2(8 + versionSize.X, Main.screenHeight - versionSize.Y - 2);
		//    Terraria.Utils.DrawBorderStringFourWay(spriteBatch, Main.fontMouseText, "  - HEROsMod Mod v" + HEROsMod.Version, pos.X, pos.Y, Color.White, Color.Black, Vector2.Zero);
		//    //spriteBatch.DrawString(Main.fontMouseText, "  HEROsMod Mod - v" + Mod.Version, pos, Color.White);
		//}

		public static void DrawStringBorder(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, string text, Color borderColor, float boarderSize, Vector2 origin, float scale)
		{
			Vector2 pos = Vector2.Zero;
			int i = 0;
			while (i < 4)
			{
				switch (i)
				{
					case 0:
						pos.X = position.X - boarderSize;
						pos.Y = position.Y;
						break;

					case 1:
						pos.X = position.X + boarderSize;
						pos.Y = position.Y;
						break;

					case 2:
						pos.X = position.X;
						pos.Y = position.Y - boarderSize;
						break;

					case 3:
						pos.X = position.X;
						pos.Y = position.Y + boarderSize;
						break;
				}
				spriteBatch.DrawString(font, text, pos, borderColor, 0f, origin, scale, SpriteEffects.None, 0f);
				i++;
			}
		}

		//public static void GhostHook(Player player)
		//{
		//    player.immune = false;
		//    player.immuneAlpha = 0;
		//    player.controlUp = false;
		//    player.controlLeft = false;
		//    player.controlDown = false;
		//    player.controlRight = false;
		//    player.controlJump = false;
		//    if (Main.hasFocus && !Main.drawingPlayerChat && !Main.editSign && !Main.editChest && !Main.blockInput)
		//    {
		//        Keys[] pressedKeys = Main.keyState.GetPressedKeys();
		//        if (Main.blockKey != Keys.None)
		//        {
		//            bool flag = false;
		//            for (int i = 0; i < pressedKeys.Length; i++)
		//            {
		//                if (pressedKeys[i] == Main.blockKey)
		//                {
		//                    pressedKeys[i] = Keys.None;
		//                    flag = true;
		//                }
		//            }
		//            if (!flag)
		//            {
		//                Main.blockKey = Keys.None;
		//            }
		//        }
		//        for (int j = 0; j < pressedKeys.Length; j++)
		//        {
		//            string a = string.Concat(pressedKeys[j]);
		//            if (a == Main.cUp)
		//            {
		//                player.controlUp = true;
		//            }
		//            if (a == Main.cLeft)
		//            {
		//                player.controlLeft = true;
		//            }
		//            if (a == Main.cDown)
		//            {
		//                player.controlDown = true;
		//            }
		//            if (a == Main.cRight)
		//            {
		//                player.controlRight = true;
		//            }
		//            if (a == Main.cJump)
		//            {
		//                player.controlJump = true;
		//            }
		//        }
		//    }
		//    player.velocity = Vector2.Zero;
		//    float moveSpeed = 5f;
		//    if(Main.keyState.IsKeyDown(Keys.LeftShift))
		//    {
		//        moveSpeed += 5f;
		//    }
		//    if (player.controlUp || player.controlJump)
		//    {
		//        player.velocity.Y -= moveSpeed;
		//        if (player.velocity.Y < -moveSpeed)
		//        {
		//            player.velocity.Y = -moveSpeed;
		//        }
		//    }
		//    else if (player.controlDown)
		//    {
		//        player.velocity.Y += moveSpeed;
		//        if (player.velocity.Y > moveSpeed)
		//        {
		//            player.velocity.Y = moveSpeed;
		//        }
		//    }
		//    if (player.controlLeft && !player.controlRight)
		//    {
		//        player.velocity.X -= moveSpeed;
		//        if (player.velocity.X < -moveSpeed)
		//        {
		//            player.velocity.X = -moveSpeed;
		//        }
		//    }
		//    else if (player.controlRight && !player.controlLeft)
		//    {
		//        player.velocity.X += moveSpeed;
		//        if (player.velocity.X > moveSpeed)
		//        {
		//            player.velocity.X = moveSpeed;
		//        }
		//    }
		//    player.position += player.velocity;
		//    player.ghostFrameCounter++;
		//    if (player.velocity.X < 0f)
		//    {
		//        player.direction = -1;
		//    }
		//    else if (player.velocity.X > 0f)
		//    {
		//        player.direction = 1;
		//    }
		//    if (player.ghostFrameCounter >= 8)
		//    {
		//        player.ghostFrameCounter = 0;
		//        player.ghostFrame++;
		//        if (player.ghostFrame >= 4)
		//        {
		//            player.ghostFrame = 0;
		//        }
		//    }
		//    if (player.position.X < Main.leftWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
		//    {
		//        player.position.X = Main.leftWorld + (float)(Lighting.offScreenTiles * 16) + 16f;
		//        player.velocity.X = 0f;
		//    }
		//    if (player.position.X + (float)player.width > Main.rightWorld - (float)(Lighting.offScreenTiles * 16) - 32f)
		//    {
		//        player.position.X = Main.rightWorld - (float)(Lighting.offScreenTiles * 16) - 32f - (float)player.width;
		//        player.velocity.X = 0f;
		//    }
		//    if (player.position.Y < Main.topWorld + (float)(Lighting.offScreenTiles * 16) + 16f)
		//    {
		//        player.position.Y = Main.topWorld + (float)(Lighting.offScreenTiles * 16) + 16f;
		//        if ((double)player.velocity.Y < -0.1)
		//        {
		//            player.velocity.Y = -0.1f;
		//        }
		//    }
		//    if (player.position.Y > Main.bottomWorld - (float)(Lighting.offScreenTiles * 16) - 32f - (float)player.height)
		//    {
		//        player.position.Y = Main.bottomWorld - (float)(Lighting.offScreenTiles * 16) - 32f - (float)player.height;
		//        player.velocity.Y = 0f;
		//    }
		//}

		//public static void DrawLogo(SpriteBatch spriteBatch, Color color, Color color2, float rotation, float scale)
		//{
		//    spriteBatch.End();
		//    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
		//    Main.spriteBatch.Draw(_logoTexture, new Vector2((float)(Main.screenWidth / 2), 100f), new Rectangle?(new Rectangle(0, 0, Main.logoTexture.Width, Main.logoTexture.Height)), color, rotation, new Vector2((float)(Main.logoTexture.Width / 2), (float)(Main.logoTexture.Height / 2)), scale, SpriteEffects.None, 0f);
		//    Main.spriteBatch.Draw(_logoTexture2, new Vector2((float)(Main.screenWidth / 2), 100f), new Rectangle?(new Rectangle(0, 0, Main.logoTexture.Width, Main.logoTexture.Height)), color2, rotation, new Vector2((float)(Main.logoTexture.Width / 2), (float)(Main.logoTexture.Height / 2)), scale, SpriteEffects.None, 0f);

		//    //DrawTestTube(spriteBatch, new Vector2(50,50), rotation, scale);
		//    spriteBatch.End();
		//    spriteBatch.Begin();
		//}

		//public static void DrawTestTube(SpriteBatch spriteBatch, Vector2 pos, float rotation, float scale)
		//{
		//    Rectangle tubeBGSource = new Rectangle(0, 0, 32, 183);
		//    Rectangle tubeLiquidSource = new Rectangle(38, 24, 24, 157);
		//    Rectangle liquidCapSource = new Rectangle(0, 185, 24, 12);
		//    Vector2 liquidOffset = new Vector2(4, 25);

		//    Matrix matrix =
		//        //Matrix.CreateTranslation(liquidOffset.X, liquidOffset.Y, 0)
		//        Matrix.CreateRotationZ(rotation)
		//        * Matrix.CreateScale(new Vector3(scale, scale, 1))
		//        ;

		//    //Vector2 pos = Vector2.Transform(Vector2.Zero, matrix);
		//    Vector2 transsformedPos = Vector2.Transform(liquidOffset, matrix);

		//    spriteBatch.Draw(_testTubeTexture, pos, tubeBGSource, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
		//    spriteBatch.Draw(_testTubeTexture, transsformedPos + pos, tubeLiquidSource, Color.Red, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
		//    spriteBatch.Draw(_testTubeTexture, transsformedPos + pos, liquidCapSource, Color.Red, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
		//}

		private static Dictionary<int, Color> rarityColors = new Dictionary<int, Color>()
		{
			{-11, new Color(255, 175, 0) },
			{-1, new Color(130, 130, 130) },
			{1, new Color(150, 150, 255) },
			{2, new Color(150, 255, 150) },
			{3, new Color(255, 200, 150) },
			{4, new Color(255, 150, 150) },
			{5, new Color(255, 150, 255) },
			{6, new Color(210, 160, 255) },
			{7, new Color(150, 255, 10) },
			{8, new Color(255, 255, 10) },
			{9, new Color(5, 200, 255) },
		};

		public static Color GetItemColor(Item item)
		{
			if (rarityColors.ContainsKey(item.rare))
			{
				return rarityColors[item.rare];
			}
			return Color.White;
		}

		private static bool debug = false;

		public static void DebugText(string message)
		{
			if (debug)
			{
				string header = "HERO's Mod: ";
				if (Main.dedServ)
				{
					Console.WriteLine(header + message);
				}
				else
				{
					if (Main.gameMenu)
					{
						HEROsMod.instance.Logger.Debug(header + Main.myPlayer + ": " + message);
					}
					else
					{
						Main.NewText(header + message);
					}
				}
			}
		}

		public static Rectangle GetClippingRectangle(SpriteBatch spriteBatch, Rectangle r)
		{
			//Vector2 vector = new Vector2(this._innerDimensions.X, this._innerDimensions.Y);
			//Vector2 position = new Vector2(this._innerDimensions.Width, this._innerDimensions.Height) + vector;
			Vector2 vector = new Vector2(r.X, r.Y);
			Vector2 position = new Vector2(r.Width, r.Height) + vector;
			vector = Vector2.Transform(vector, Main.UIScaleMatrix);
			position = Vector2.Transform(position, Main.UIScaleMatrix);
			Rectangle result = new Rectangle((int)vector.X, (int)vector.Y, (int)(position.X - vector.X), (int)(position.Y - vector.Y));
			int width = spriteBatch.GraphicsDevice.Viewport.Width;
			int height = spriteBatch.GraphicsDevice.Viewport.Height;
			result.X = Utils.Clamp<int>(result.X, 0, width);
			result.Y = Utils.Clamp<int>(result.Y, 0, height);
			result.Width = Utils.Clamp<int>(result.Width, 0, width - result.X);
			result.Height = Utils.Clamp<int>(result.Height, 0, height - result.Y);
			return result;
		}
	}

	public enum NetworkMode : byte
	{
		None,
		Client,
		Server
	}
}