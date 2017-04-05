//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.IO;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.ChestDropsInfo
//{
//    class ChestDropBuilder
//    {
//        static List<ChestInfo> chests = new List<ChestInfo>();
//        public static List<ChestDropInfo> chestDropsInfo = new List<ChestDropInfo>();

//        public static void Start()
//        {
//            Console.WriteLine("World Surface: " + Main.worldSurface);
//            Console.WriteLine("Rock Layer: " + Main.rockLayer);
//            chests = new List<ChestInfo>();
//            for (int i = 0; i < Main.chest.Length; i++)
//            {
//                Chest chest = Main.chest[i];
//                if (chest != null)
//                {
//                    //Chest.Unlock(chest.x, chest.y);
//                    int chestTileType = Main.tile[chest.x, chest.y].frameX / 36;
                    
//                    if(chestTileType != 2)
//                    {
//                        Chest.Unlock(chest.x, chest.y);
//                        chestTileType = Main.tile[chest.x, chest.y].frameX / 36;
//                    }
//                    //int chestType = Chest.typeToIcon[chestTileType];
//					int chestType = Chest.chestTypeToIcon[chestTileType];
//                    //if normal chest
//                    if (chestTileType == 0)
//                    {
//                        //chest is in dungeon
//                        if (chest.y > 1000)
//                        {
//                            chestType = -2;
//                        }
//                    }
//                    ChestInfo chestInfo = null;
//                    for (int j = 0; j < chest.item.Length; j++)
//                    {
//                        Item item = chest.item[j];
//                        if (item != null && item.type > 0)
//                        {
//                            if(chestInfo == null)
//                            {
//                                chestInfo = new ChestInfo();
//                                chestInfo.Type = chestType;
//                                chests.Add(chestInfo);
//                            }
//                            chestInfo.Items.Add(item.Clone());
//                        }
//                    }
//                }
//            }

//            for (int i = 0; i < chests.Count; i++)
//            {
//                ChestDropInfo cdi = GetChestDropInfoForChestType(chests[i].Type);
//                cdi.AddItems(chests[i].Items.ToArray());
//            }
//            chestDropsInfo = chestDropsInfo.OrderBy(x => x.Type).ToList();
//            SaveCompiledData();
//        }

//        //public static void GenorateWorld()
//        //{
//        //    Main.maxTilesX = 8400;
//        //    Main.maxTilesY = 2400;
//        //    Main.newWorldName = "ChestScanTest";
//        //    Main.worldName = Main.newWorldName;
//        //    Main.worldPathName = getWorldPathName(Main.worldName);

//        //    Main.menuMode = 10;
//        //    WorldGen.CreateNewWorld2();

//        //    //WorldGen.clearWorld();
//        //    //WorldGen.generateWorld(-1);
//        //    //WorldFile.saveWorld(true);
//        //    //Main.LoadWorlds();
//        //    //if (Main.menuMode == 10)
//        //    //{
//        //    //    Main.menuMode = 6;
//        //    //}
//        //}

//        static ChestDropInfo GetChestDropInfoForChestType(int chestType)
//        {
//            for(int i = 0; i < chestDropsInfo.Count; i++)
//            {
//                if(chestDropsInfo[i].Type == chestType)
//                {
//                    return chestDropsInfo[i];
//                }
//            }
//            ChestDropInfo cdi = new ChestDropInfo();
//            cdi.Type = chestType;
//            chestDropsInfo.Add(cdi);
//            return cdi;
//        }

//        public static void SaveCompiledData()
//        {
//            using (FileStream fileStream = new FileStream("compiledChestData.bin", FileMode.Create))
//            {
//                using (BinaryWriter writer = new BinaryWriter(fileStream))
//                {
//                    writer.Write(chestDropsInfo.Count);
//                    for (int i = 0; i < chestDropsInfo.Count; i++)
//                    {
//                        ChestDropInfo chest = chestDropsInfo[i];
//                        writer.Write(chest.Type);
//                        writer.Write(chest.ItemDrops.Count);
//                        for (int j = 0; j < chest.ItemDrops.Count; j++)
//                        {
//                            DropRateInfo.ItemDropInfo item = chest.ItemDrops[j];
//                            writer.Write(item.NPCType);
//                            writer.Write(item.Min);
//                            writer.Write(item.Max);
//                            writer.Write(item.SampleSize);
//                            writer.Write(item.TimesDropped);
//                        }
//                    }
//                    writer.Close();
//                    fileStream.Close();
//                }
//            }
//        }

//        public static void LoadCompiledData()
//        {
//            chestDropsInfo = new List<ChestDropInfo>();
//            using (FileStream fileStream = new FileStream("compiledChestData.bin", FileMode.Open))
//            {
//                using (BinaryReader reader = new BinaryReader(fileStream))
//                {
//                    int numOfChests = reader.ReadInt32();
//                    for (int i = 0; i < numOfChests; i++)
//                    {
//                        ChestDropInfo cdi = new ChestDropInfo();
//                        cdi.Type = reader.ReadInt32();
//                        chestDropsInfo.Add(cdi);
//                        int numOfItems = reader.ReadInt32();
//                        for (int j = 0; j < numOfItems; j++)
//                        {
//                            DropRateInfo.ItemDropInfo itemDrop = new DropRateInfo.ItemDropInfo();
//                            itemDrop.NPCType = reader.ReadInt32();
//                            itemDrop.Min = reader.ReadInt32();
//                            itemDrop.Max = reader.ReadInt32();
//                            itemDrop.SampleSize = reader.ReadInt32();
//                            itemDrop.TimesDropped = reader.ReadInt32();
//                            cdi.ItemDrops.Add(itemDrop);
//                        }
//                    }
//                }
//            }
//        }

//        public static void SaveData()
//        {
//            using(FileStream fileStream = new FileStream("chestData.bin", FileMode.Create))
//            {
//                using(BinaryWriter writer = new BinaryWriter(fileStream))
//                {
//                    writer.Write(chests.Count);
//                    for(int i = 0; i < chests.Count; i++)
//                    {
//                        ChestInfo chest = chests[i];
//                        writer.Write(chest.Type);
//                        writer.Write(chest.Items.Count);
//                        for(int j = 0; j < chest.Items.Count; j++)
//                        {
//                            Item item = chest.Items[j];
//                            writer.Write(item.netID);
//                            writer.Write(item.prefix);
//                            writer.Write(item.stack);
//                        }
//                    }
//                    writer.Close();
//                    fileStream.Close();
//                }
//            }
//        }

//        public static void LoadData()
//        {
//            chests = new List<ChestInfo>();
//            using(FileStream fileStream = new FileStream("chestData.bin", FileMode.Open))
//            {
//                using(BinaryReader reader = new BinaryReader(fileStream))
//                {
//                    int numOfChests = reader.ReadInt32();
//                    for(int i = 0; i < numOfChests; i++)
//                    {
//                        ChestInfo ci = new ChestInfo();
//                        ci.Type = reader.ReadInt32();
//                        chests.Add(ci);
//                        int numOfItems = reader.ReadInt32();
//                        for(int j = 0; j < numOfItems; j++)
//                        {
//                            Item item = new Item();
//                            item.netDefaults(reader.ReadInt32());
//                            item.Prefix(reader.ReadByte());
//                            item.stack = reader.ReadInt32();
//                            ci.Items.Add(item);
//                        }
//                    }
//                }
//            }
//            for (int i = 0; i < chests.Count; i++)
//            {
//                ChestDropInfo cdi = GetChestDropInfoForChestType(chests[i].Type);
//                cdi.AddItems(chests[i].Items.ToArray());
//            }
//            chestDropsInfo = chestDropsInfo.OrderBy(x => x.Type).ToList();
//        }

//        private static string getWorldPathName(string worldName)
//        {
//            string text = "";
//            for (int i = 0; i < worldName.Length; i++)
//            {
//                string text2 = worldName.Substring(i, 1);
//                string str;
//                if (text2 == "a" || text2 == "b" || text2 == "c" || text2 == "d" || text2 == "e" || text2 == "f" || text2 == "g" || text2 == "h" || text2 == "i" || text2 == "j" || text2 == "k" || text2 == "l" || text2 == "m" || text2 == "n" || text2 == "o" || text2 == "p" || text2 == "q" || text2 == "r" || text2 == "s" || text2 == "t" || text2 == "u" || text2 == "v" || text2 == "w" || text2 == "x" || text2 == "y" || text2 == "z" || text2 == "A" || text2 == "B" || text2 == "C" || text2 == "D" || text2 == "E" || text2 == "F" || text2 == "G" || text2 == "H" || text2 == "I" || text2 == "J" || text2 == "K" || text2 == "L" || text2 == "M" || text2 == "N" || text2 == "O" || text2 == "P" || text2 == "Q" || text2 == "R" || text2 == "S" || text2 == "T" || text2 == "U" || text2 == "V" || text2 == "W" || text2 == "X" || text2 == "Y" || text2 == "Z" || text2 == "1" || text2 == "2" || text2 == "3" || text2 == "4" || text2 == "5" || text2 == "6" || text2 == "7" || text2 == "8" || text2 == "9" || text2 == "0")
//                {
//                    str = text2;
//                }
//                else if (text2 == " ")
//                {
//                    str = "_";
//                }
//                else
//                {
//                    str = "-";
//                }
//                text += str;
//            }
//            string path = string.Concat(new object[]
//			{
//				Main.WorldPath,
//				Path.DirectorySeparatorChar,
//				text,
//				".wld"
//			});
//            string fullPath = Path.GetFullPath(path);
//            if (fullPath.StartsWith("\\\\.\\", StringComparison.Ordinal))
//            {
//                text += "_";
//            }
//            return string.Concat(new object[]
//			{
//				Main.WorldPath,
//				Path.DirectorySeparatorChar,
//				text,
//				".wld"
//			});
//        }

//    }
//}
