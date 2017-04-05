//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiServices
//{
//    class WikiService : GameikiService
//    {
//        static string saveFileName = "wiki.dat";
//        public static WikiSite currentSite = WikiSite.Gameiki;
//        static TileID[] tilesIDs;
//        public static bool pageVisited = false;
//        static TileExceptions[] tileExceptions;
//        public static bool wikiCursor = false;
//        static bool previousWikiCursor = false;
//        static Item tooltip;
//        static Player player
//        {
//            get { return Main.player[Main.myPlayer]; }
//        }

//        struct TileID
//        {
//            public int createTile;
//            public int createWall;
//            public int placeStyle;
//            public string name;
//            public TileID(int createTile, int createWall, int placeStyle, string name)
//            {
//                this.createTile = createTile;
//                this.createWall = createWall;
//                this.placeStyle = placeStyle;
//                this.name = name;
//            }

//            void placeStyleFromWidth(int width)
//            {

//            }
//        }

//        class TileExceptions
//        {
//            public int id;
//            public int width = -1;
//            public int height = -1;
//            public string autoName = "";
//            public int swapID = -1;
//            public TileExceptions(int id, int width)
//            {
//                this.id = id;
//                this.width = width;
//                AdjustWidthHeight();
//            }
//            public TileExceptions(int id, string autoName)
//            {
//                this.id = id;
//                this.autoName = autoName;
//            }
//            public TileExceptions(int id, int width, int height)
//            {
//                this.id = id;
//                this.width = width;
//                this.height = height;
//                AdjustWidthHeight();
//            }
//            public TileExceptions(int id, int width, int height, int swapID)
//            {
//                this.id = id;
//                this.width = width;
//                this.height = height;
//                this.swapID = swapID;
//                AdjustWidthHeight();
//            }

//            void AdjustWidthHeight()//STUPID CHAIRS!!!!
//            {
//                if (width > -1 && width < 10)
//                {
//                    width *= 18;
//                }
//                if (height > -1 && height < 10)
//                {
//                    height *= 18;
//                }
//            }
//        }


//        public WikiService()
//        {
//            Init();
//        }

//        public static void Init()
//        {
//            Item item = new Item();
//            tilesIDs = new TileID[Main.itemTexture.Length];
//            for (int i = 0; i < tilesIDs.Length; i++)
//            {
//                item.SetDefaults(i);
//                tilesIDs[i] = new TileID(item.createTile, item.createWall, item.placeStyle, item.name);
//            }
//            FillTileExceptions();
//        }

//        public static void GoToURL(string url)
//        {
//            pageVisited = true;
//            System.Diagnostics.Process.Start(url);
//        }

//        public static void GoToItem(string itemName)
//        {
//            string name = itemName.Replace(' ', '_');

//            //string site = "http://www.GameikiMod.com/wiki/";
//            string site = "http://GameikiMod.com/wiki/";
//            if (currentSite == WikiSite.Gamepedia) site = "http://terraria.gamepedia.com/";
//            if (currentSite == WikiSite.Wikia) site = "http://www.terraria.wikia.com/wiki/";

//            name = site + name;
//            pageVisited = true;
//            //Main.NewText(itemName, 175, 75, 255, false);
//            GoToURL(name);
//        }

//        static void FillTileExceptions()
//        {
//            tileExceptions = new TileExceptions[]{
//                new TileExceptions(4, -1, 1),        //torches
//                new TileExceptions(5, "Tree"),        //tree
//                new TileExceptions(10, -1, 3),        //doors
//                new TileExceptions(11, -1, 3, 10),    //open doors
//                new TileExceptions(14, 3),            //tables
//                new TileExceptions(12, "Life Crystal"),//Life crystals
//                new TileExceptions(15, -1, 40),       //chairs
//                new TileExceptions(16, 2),            //anvils
//                new TileExceptions(18, 2),            //workbenches
//                new TileExceptions(19, -1, 1),        //platforms
//                new TileExceptions(21, 2),            //chests
//                new TileExceptions(26, "Altar"),      //altars ADDITIONAL EXCEPTIONS NEEDED
//                new TileExceptions(27, "Sunflower"),  //sunflower
//                new TileExceptions(28, "Pot"),        //pot
//                new TileExceptions(42, -1, 2),        //haning laterns
//                new TileExceptions(63, "Sapphire"),   //gem ore
//                new TileExceptions(64, "Ruby"),   //gem ore
//                new TileExceptions(65, "Emerald"),   //gem ore
//                new TileExceptions(66, "Topaz"),   //gem ore
//                new TileExceptions(67, "Amethyst"),   //gem ore
//                new TileExceptions(68, "Diamond"),   //gem ore
//                new TileExceptions(79, -1, 2),        //beds
//                new TileExceptions(82, 1),            //sprout plant
//                new TileExceptions(83, 1, -1, 82),    //growing plant
//                new TileExceptions(84, 1, -1, 82),    //mature plant
//                new TileExceptions(87, 3),            //pianos
//                new TileExceptions(88, 3),            //dressers
//                new TileExceptions(91, 1),            //banners
//                new TileExceptions(99, 2, -1, 21),    //trash can
//                new TileExceptions(101, 3),           //book cases
//                new TileExceptions(105, 2),           //statue
//                new TileExceptions(133, 3),           //lateGame forges
//                new TileExceptions(134, 2),           //lateGame anvils
//                new TileExceptions(135, -1, 1),       //pressurePlates
//                new TileExceptions(137, -1, 1),       //dart traps
//                new TileExceptions(139, -1, 2),       //music boxes
//                new TileExceptions(144, 1),           //timers
//                new TileExceptions(149, 1),           //xmas lights
//                new TileExceptions(178, 1),            //gems
//                new TileExceptions(240, 3, 3)            //Trophies
//            };
//            //tilesIDs[i].createTile == 21 || tilesIDs[i].createTile == 105
//        }

//        public override void Update()
//        {
//            if (!Main.gameMenu && !Main.mapFullscreen)
//                //if (Main.playerInventory) button.Update(mouseNPC);
//                pageVisited = false;

//            if (Main.keyState.IsKeyDown(Keys.LeftAlt)) wikiCursor = true;
//            else if (Main.keyState.IsKeyUp(Keys.LeftAlt) && ModUtils.PreviousKeyboardState.IsKeyDown(Keys.LeftAlt)) wikiCursor = false;
//            if (wikiCursor && Main.mouseRight && Main.mouseRightRelease) wikiCursor = false;
//            if (wikiCursor && previousWikiCursor && Main.mouseLeft && Main.mouseLeftRelease)
//            {
//                CheckForWikiItem(tooltip);
//            }
//            previousWikiCursor = wikiCursor;
//            base.Update();
//        }

//        static bool UberTileException(Tile tile)
//        {
//            if (tile.type == 10)
//            {
//                int placeStyle = tile.frameY / (18 * 3);
//                if (placeStyle == 11)
//                {
//                    GoToItem("Lihzahrd Door");
//                    return true;
//                }
//            }
//            if (tile.type == 21)
//            {
//                int placeStyle = tile.frameX / (18 * 2);
//                if (placeStyle == 2)
//                {
//                    GoToItem("Gold Chest");
//                    return true;
//                }
//                else if (placeStyle == 4)
//                {
//                    GoToItem("Shadow Chest");
//                    return true;
//                }
//                else if (placeStyle == 4)
//                {
//                    GoToItem("Shadow Chest");
//                    return true;
//                }
//                else if (placeStyle == 23)
//                {
//                    GoToItem("Jungle Chest");
//                    return true;
//                }
//                else if (placeStyle == 24)
//                {
//                    GoToItem("Corruption Chest");
//                    return true;
//                }
//                else if (placeStyle == 25)
//                {
//                    GoToItem("Crimson Chest");
//                    return true;
//                }
//                else if (placeStyle == 26)
//                {
//                    GoToItem("Hallowed Chest");
//                    return true;
//                }
//                else if (placeStyle == 27)
//                {
//                    GoToItem("Frozen Chest");
//                    return true;
//                }
//            }
//            if (tile.type == 26)
//            {
//                int placeStyle = tile.frameX / (18 * 3);
//                if (placeStyle == 0) GoToItem("Demon Altar");
//                else GoToItem("Crimson Altar");
//                return true;
//            }
//            else if (tile.type == 31)
//            {
//                int placeStyle = tile.frameX / (18 * 2);
//                if (placeStyle == 0) GoToItem("Shadow Orb");
//                else GoToItem("Demon Heart");
//                return true;
//            }
//            return false;
//        }

//        static void CheckForWikiItem(Item tooltip)
//        {
//            if (Main.tile != null && Main.player[Main.myPlayer] != null)
//            {

//                Main.player[Main.myPlayer].mouseInterface = true;
//                //FillTileExceptions(); //ONLY FOR TEST, REMOVE FOR RELEASE
//                if (ScanTooltip(tooltip) || ScanNPC() || ScanTileItem() || ScanWallItem())
//                {
//                    if (Main.keyState.IsKeyUp(Keys.LeftAlt)) wikiCursor = false;
//                }
//            }
//        }

//        public static bool LeftAltClick()
//        {
//            if (Main.keyState.IsKeyDown(Keys.LeftAlt) && Main.mouseLeft && Main.mouseLeftRelease)
//                return true;
//            return false;
//        }

//        static bool ScanTooltip(Item tooltip)
//        {
//            if (tooltip != null && tooltip.type > 0)
//            {
//                Main.mouseLeftRelease = false;
//                string[] names = tooltip.name.Split(',');
//                GoToItem(names[0]);
//                return true;
//            }
//            return false;
//        }

//        static bool ScanInventory()
//        {
//            if (Main.playerInventory)
//            {
//                float inventoryScale = .85f;
//                for (int x = 0; x < 10; x++)
//                {
//                    for (int y = 0; y < 5; y++)
//                    {
//                        int itemPosX = (int)(20f + (float)(x * 56) * inventoryScale);
//                        int itemPosY = (int)(20f + (float)(y * 56) * inventoryScale);
//                        int itemNum = x + y * 10;
//                        if (Main.player[Main.myPlayer].inventory[itemNum].type > 0)
//                        {
//                            if (Main.mouseX >= itemPosX && (float)Main.mouseX <= (float)itemPosX + (float)Main.inventoryBackTexture.Width * inventoryScale && Main.mouseY >= itemPosY && (float)Main.mouseY <= (float)itemPosY + (float)Main.inventoryBackTexture.Height * inventoryScale)
//                            {
//                                GoToItem(Main.player[Main.myPlayer].inventory[itemNum].name);
//                                //Main.mouseLeft = false;
//                                Main.mouseLeftRelease = false;
//                                return true;
//                            }
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        static bool ScanChest()
//        {
//            if (player.chest != -1)
//            {
//                float inventoryScale = 0.755f;
//                int invBottom = 258;

//                for (int x = 0; x < 10; x++)
//                {
//                    for (int y = 0; y < 4; y++)
//                    {
//                        int itemPosX = (int)(73f + (float)(x * 56) * inventoryScale);
//                        int itemPosY = (int)((float)invBottom + (float)(y * 56) * inventoryScale);
//                        int itemNum = x + y * 10;

//                        if (Main.mouseX >= itemPosX && (float)Main.mouseX <= (float)itemPosX + (float)Main.inventoryBackTexture.Width * inventoryScale && Main.mouseY >= itemPosY && (float)Main.mouseY <= (float)itemPosY + (float)Main.inventoryBackTexture.Height * inventoryScale)
//                        {

//                            Item chestItem = null;
//                            if (player.chest > -1) chestItem = Main.chest[player.chest].item[itemNum];
//                            else if (player.chest == -2) chestItem = player.bank.item[itemNum];
//                            else if (player.chest == -3) chestItem = player.bank2.item[itemNum];

//                            if (chestItem != null && chestItem.type > 0)
//                            {
//                                GoToItem(chestItem.name);
//                                Main.mouseLeftRelease = false;
//                                return true;
//                            }
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        static bool ScanNPC()
//        {
//            Rectangle mouseRect = new Rectangle((int)((float)Main.mouseX + Main.screenPosition.X), (int)((float)Main.mouseY + Main.screenPosition.Y), 1, 1);
//            for (int i = 0; i < 200; i++)
//            {
//                if (Main.npc[i] != null && Main.npc[i].active)
//                {
//                    Rectangle npcRect = new Rectangle((int)((double)Main.npc[i].position.X + (double)Main.npc[i].width * 0.5 - (double)Main.npcTexture[Main.npc[i].type].Width * 0.5), (int)(Main.npc[i].position.Y + (float)Main.npc[i].height - (float)(Main.npcTexture[Main.npc[i].type].Height / Main.npcFrameCount[Main.npc[i].type])), Main.npcTexture[Main.npc[i].type].Width, Main.npcTexture[Main.npc[i].type].Height / Main.npcFrameCount[Main.npc[i].type]);
//                    if (Main.npc[i].type >= 87 && Main.npc[i].type <= 92)
//                    {
//                        npcRect = new Rectangle((int)((double)Main.npc[i].position.X + (double)Main.npc[i].width * 0.5 - 32.0), (int)((double)Main.npc[i].position.Y + (double)Main.npc[i].height * 0.5 - 32.0), 64, 64);
//                    }
//                    if (mouseRect.Intersects(npcRect))
//                    {
//                        GoToItem(Main.npc[i].name);
//                        return true;
//                    }
//                }

//            }
//            return false;
//        }

//        static bool ScanTileItem()
//        {

//            int x = (int)((float)Main.mouseX + Main.screenPosition.X) / 16;
//            int y = (int)((float)Main.mouseY + Main.screenPosition.Y) / 16;
//            if (x < Main.tile.GetLength(0) && x >= 0 && y < Main.tile.GetLength(1) && y >= 0)
//            {
//                Tile tile = Main.tile[x, y];
//                TileExceptions exception = null;

//                if (tile != null && tile.active() && tile.type != -1 && !UberTileException(tile))
//                {
//                    for (int i = 0; i < tileExceptions.Length; i++) //look for exceptions
//                    {
//                        if (tile.type == tileExceptions[i].id)
//                        {
//                            exception = tileExceptions[i];
//                            break;
//                        }
//                    }

//                    if (exception != null && exception.autoName.Length > 0)//auto name exception
//                    {
//                        GoToItem(exception.autoName);
//                        return true;
//                    }
//                    else//no auto name exception
//                    {
//                        int tileType = tile.type;
//                        int placeStyle = 0;
//                        if (exception != null)//setup exception variables if exception exists
//                        {
//                            if (exception.swapID > -1) tileType = exception.swapID; //overide item id
//                            if (exception.width > -1 && exception.height > -1)
//                            {
//                                int itemsX = Main.tileTexture[tileType].Width / exception.width;
//                                placeStyle = tile.frameX / exception.width;
//                                placeStyle += tile.frameY / exception.height * itemsX;
//                            }
//                            else if (exception.width > -1) placeStyle = tile.frameX / exception.width; //get placestyle from width
//                            else if (exception.height > -1) placeStyle = tile.frameY / exception.height; //get placestye from height
//                        }
//                        for (int i = 1; i < tilesIDs.Length; i++)
//                        {
//                            if (tileType == tilesIDs[i].createTile)
//                            {
//                                if (exception != null)//if has exception
//                                {
//                                    if (tilesIDs[i].placeStyle == placeStyle)
//                                    {
//                                        GoToItem(tilesIDs[i].name);
//                                        return true;
//                                    }
//                                }
//                                else //no exception
//                                {
//                                    GoToItem(tilesIDs[i].name);
//                                    return true;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            return false;

//        }

//        static bool ScanWallItem()
//        {
//            int x = (int)((float)Main.mouseX + Main.screenPosition.X) / 16;
//            int y = (int)((float)Main.mouseY + Main.screenPosition.Y) / 16;
//            if (x < Main.tile.GetLength(0) && x >= 0 && y < Main.tile.GetLength(1) && y >= 0)
//            {
//                Tile tile = Main.tile[x, y];

//                if (tile != null && tile.wall != 0)
//                {

//                    Console.WriteLine(tile.wall);
//                    for (int i = 1; i < tilesIDs.Length; i++)
//                    {
//                        if (tile.wall == tilesIDs[i].createWall)
//                        {
//                            GoToItem(tilesIDs[i].name);
//                            return true;
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        public static void Save()
//        {
//            using (FileStream fileStream = new FileStream(Main.SavePath + Path.DirectorySeparatorChar + saveFileName, FileMode.Create))
//            {
//                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
//                {
//                    binaryWriter.Write((byte)currentSite);
//                }
//            }
//        }

//        public static void Load()
//        {
//            if (File.Exists(Main.SavePath + Path.DirectorySeparatorChar + saveFileName))
//            {
//                using (FileStream fileStream = new FileStream(Main.SavePath + Path.DirectorySeparatorChar + saveFileName, FileMode.Open))
//                {
//                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
//                    {
//                        try
//                        {
//                            currentSite = (WikiSite)binaryReader.ReadByte();
//                        }
//                        catch (Exception ex)
//                        {
//                            binaryReader.Close();

//                        }
//                    }
//                }
//            }
//        }

//        public enum WikiSite
//        {
//            Gameiki,
//            Gamepedia,
//            Wikia
//        }
//    }
//}
