//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.RecipeDatabaseBuilder
//{
//    class Builder
//    {
//        static List<int> requiredTiles = new List<int>();
//        public static void AddRecipe(Recipe recipe)
//        {
//            for(int i = 0; i < recipe.requiredTile.Length; i++)
//            {
//                if(recipe.requiredTile[i] >= 0)
//                {
//                    if(!requiredTiles.Contains(recipe.requiredTile[i]))
//                    {
//                        requiredTiles.Add(recipe.requiredTile[i]);
//                    }
//                }
//            }
//        }

//        public static void WriteList()
//        {
//            requiredTiles = requiredTiles.OrderBy(x => x).ToList();
//            for (int i = 0; i < requiredTiles.Count; i++)
//            {
//                Console.WriteLine(requiredTiles[i]);
//            }
//        }

//        public static Item[] GetMRequiredMaterialsToCraftItem(Item item)
//        {
//            List<Item> materials = new List<Item>();
//            for (int i = 0; i < Main.recipe.Length; i++)
//            {
//                Recipe recipe = Main.recipe[i];
//                if(recipe.createItem.netID == item.netID)
//                {
//                    for(int j = 0; j < recipe.requiredItem.Length; j++)
//                    {
//                        Item requuiredItem = recipe.requiredItem[j];
//                        if(requuiredItem.netID != 0)
//                        {
//                            materials.Add(requuiredItem);
//                        }
//                    }
//                    break;
//                }
//            }
//            return materials.ToArray();
//        }

//        public static int CraftingTileToItemType(int tileNum)
//        {
//            switch(tileNum)
//            {
//                case 13: //Bottle
//                    return 31;
//                case 14: //Table
//                    return 32;
//                case 15: //Chair
//                    return 34;
//                case 16: //Anvil
//                    return 35;
//                case 17: //Furnace
//                    return 33;
//                case 18: //Work Bench
//                    return 36;
//                case 26: //Demon Alter
//                    return 0; //oops
//                case 77: //Hell Forge
//                    return 221;
//                case 86: //Loom
//                    return 332;
//                case 94: //Keg 
//                    return 352;
//                case 96: //Cooking Pot
//                    return 345;
//                case 101: //Bookcase
//                    return 354;
//                case 106: //Sawmill
//                    return 363;
//                case 114: //Tinkerer's Workshop
//                    return 398;
//                case 125: //Crystal Ball
//                    return 487;
//                case 133: //Adamantite Forge
//                    return 524;
//                case 134: //Mythril Anvil
//                    return 525;
//                case 217: //Blend-O-Matic
//                    return 995;
//                case 218: //Meat Grinder
//                    return 996;
//                case 220: //Extractinator
//                    return 997;
//                case 228: //Solidifier
//                    return 998;
//                case 243: //Imbuing Station
//                    return 1430;
//                case 247: //Autohammer
//                    return 1551;
//                case 283: //Heavy Work Bench
//                    return 2172;
//                case 300: //Bone Welder
//                    return 2192;
//                case 301: //Flesh Cloning Vat
//                    return 2193;
//                case 302: //Glass Kiln
//                    return 2194;
//                case 303: //Lihzahrd Furnace
//                    return 2195;
//                case 304: //Living Loom
//                    return 2196;
//                case 305: //Sky Mill
//                    return 2197;
//                case 306: //Ice Machine
//                    return 2198;
//                case 307: //Steampunk Boiler
//                    return 2203;
//                case 308: //Honey Dispenser
//                    return 2204;
//            }
//            return 0;
//        }
//    }

    
//}
