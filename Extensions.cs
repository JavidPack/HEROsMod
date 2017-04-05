//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Terraria;

//namespace GameikiMod
//{
//    static class Extensions
//    {
//        //public static Item[] GetRequiredTiles(this Recipe recipe)
//        //{
//        //    List<Item> result = new List<Item>();
//        //    for (int i = 0; i < recipe.requiredTile.Length; i++)
//        //    {
//        //        int requiredTile = recipe.requiredTile[i];
//        //        if(requiredTile > 0)
//        //        {
//        //            Item item = new Item();
//        //            item.netID = CraftingTileToItemType(requiredTile);
//        //            if (item.netID != -1000)
//        //            {
//        //                item.netDefaults(CraftingTileToItemType(requiredTile));
//        //            }
//        //            else
//        //            {
//        //                item.name = "Demon Altar";
//        //            }
//        //            result.Add(item);
//        //        }
//        //    }
//        //    return result.ToArray();
//        //}

//        //public static bool HasMaterialsForRecipe(this Player player, Recipe recipe)
//        //{
//        //    if (recipe.createItem.type > 0)
//        //    {
//        //        bool hasMats = true;
//        //        for (int i = 0; i < recipe.requiredItem.Length; i++)
//        //        {
//        //            Item requiredItem = recipe.requiredItem[i];
//        //            int amountRequiredItemPlayerHas = 0;
//        //            if (requiredItem.type > 0)
//        //            {
//        //                for (int j = 0; j < player.inventory.Length - 1; j++)
//        //                {
//        //                    Item invItem = player.inventory[j];
//        //                    if (recipe.IsValidMaterial(invItem, requiredItem))
//        //                    {
//        //                        amountRequiredItemPlayerHas += invItem.stack;
//        //                        if (amountRequiredItemPlayerHas >= requiredItem.stack)
//        //                        {
//        //                            break;
//        //                        }
//        //                    }
//        //                }
//        //                if (amountRequiredItemPlayerHas < requiredItem.stack)
//        //                {
//        //                    return false;
//        //                }
//        //            }
//        //        }
//        //        if (hasMats)
//        //        {
//        //            return true;
//        //        }
//        //    }
//        //    return false;
//        //}

//        //public static bool CanCraft(this Player player, Recipe recipe)
//        //{
//        //    if(!player.HasMaterialsForRecipe(recipe))
//        //        return false;
//        //    if (recipe.needWater && !player.adjWater)
//        //        return false;
//        //    if (recipe.needHoney && !player.adjHoney)
//        //        return false;
//        //    if (recipe.needLava && !player.adjLava)
//        //        return false;
//        //    if (!recipe.ByRequiredTiles(player))
//        //        return false;
//        //    return true;
//        //}

//        //public static bool ByRequiredTiles(this Recipe recipe, Player player)
//        //{
//        //    foreach(int tile in recipe.requiredTile)
//        //    {
//        //        if(tile == -1)
//        //        {
//        //            break;
//        //        }
//        //        if (!player.adjTile[tile])
//        //        {
//        //            return false;
//        //        }
//        //    }
//        //    return true;
//        //}

//        //public static void Craft(this Player player, Recipe recipe)
//        //{
//        //    if(!player.CanCraft(recipe))
//        //    {
//        //        return;
//        //    }
//        //    for (int i = 0; i < recipe.requiredItem.Length; i++)
//        //    {
//        //        Item requiredItem = recipe.requiredItem[i];
//        //        if (requiredItem.type > 0)
//        //        {
//        //            int amountToRemove = requiredItem.stack;
//        //            for (int j = 0; j < player.inventory.Length - 1; j++)
//        //            {
//        //                Item item = player.inventory[j];
//        //                if (recipe.IsValidMaterial(item, requiredItem))
//        //                {
//        //                    while(item.stack > 0 && amountToRemove > 0)
//        //                    {
//        //                        item.stack--;
//        //                        amountToRemove--;
//        //                        if(item.stack <= 0)
//        //                        {
//        //                            item.SetDefaults(0);
//        //                        }
//        //                    }
//        //                    if(amountToRemove == 0)
//        //                    {
//        //                        break;
//        //                    }
//        //                }
//        //            }

//        //            if (amountToRemove > 0)
//        //            {
//        //                Main.NewText("Craft Failed");
//        //            }
//        //        }
//        //    }
//        //    player.GiveItem(recipe.createItem.Clone());
//        //}

//        //public static bool IsValidMaterial(this Recipe recipe, Item item, Item requiredItem)
//        //{
//        //    return item.netID == requiredItem.netID || recipe.useWood(item.type, requiredItem.type) || recipe.useIronBar(item.type, requiredItem.type) || recipe.useSand(item.type, requiredItem.type) || recipe.usePressurePlate(item.type, requiredItem.type);
//        //}

//        //public static Recipe[] GetAvailableRecipes(this Player player)
//        //{
//        //    List<Recipe> result = new List<Recipe>();
//        //    for (int i = 0; i < Main.recipe.Length; i++)
//        //    {
//        //        Recipe recipe = Main.recipe[i];
//        //        if (recipe.createItem.type > 0)
//        //        {
//        //            if (player.HasMaterialsForRecipe(recipe))
//        //            {
//        //                result.Add(recipe);
//        //            }
//        //        }
//        //    }
//        //    return result.ToArray();
//        //}

//        //public static void GiveItem(this Player player, Item item)
//        //{
//        //    int amountToGive = item.stack;
//        //    //iterate through existing Items
//        //    for(int i = 0; i < player.inventory.Length; i++)
//        //    {
//        //        Item invItem = player.inventory[i];
//        //        if(invItem.netID == item.netID)
//        //        {
//        //            while(invItem.stack < invItem.maxStack && amountToGive > 0)
//        //            {
//        //                invItem.stack++;
//        //                amountToGive--;
//        //            }
//        //            if (amountToGive == 0)
//        //            {
//        //                return;
//        //            }
//        //        }
//        //    }
//        //    if(Main.mouseItem.netID == item.netID)
//        //    {
//        //        while (Main.mouseItem.stack < Main.mouseItem.maxStack && amountToGive > 0)
//        //        {
//        //            Main.mouseItem.stack++;
//        //            amountToGive--;
//        //        }
//        //        if (amountToGive == 0)
//        //        {
//        //            return;
//        //        }
//        //    }

//        //    //if we can't stack, look for empty slots
//        //    for(int i = 10; i < 40; i++)
//        //    {
//        //        Item invItem = player.inventory[i];
//        //        if(invItem.type == 0)
//        //        {
//        //            invItem.netDefaults(item.netID);
//        //            invItem.stack = amountToGive;
//        //            invItem.Prefix(item.prefix);
//        //            return;
//        //        }
//        //    }
//        //    for (int i = 0; i < 10; i++)
//        //    {
//        //        Item invItem = player.inventory[i];
//        //        if (invItem.type == 0)
//        //        {
//        //            invItem.netDefaults(item.netID);
//        //            invItem.stack = amountToGive;
//        //            invItem.Prefix(item.prefix);
//        //            return;
//        //        }
//        //    }
//        //    if(Main.mouseItem.type == 0)
//        //    {
//        //        Main.mouseItem.netDefaults(item.netID);
//        //        Main.mouseItem.stack = amountToGive;
//        //        Main.mouseItem.Prefix(item.prefix);
//        //        return;
//        //    }
//        //}

//        //static int CraftingTileToItemType(int tileNum)
//        //{
//        //    switch (tileNum)
//        //    {
//        //        case 13: //Bottle
//        //            return 31;
//        //        case 14: //Table
//        //            return 32;
//        //        case 15: //Chair
//        //            return 34;
//        //        case 16: //Anvil
//        //            return 35;
//        //        case 17: //Furnace
//        //            return 33;
//        //        case 18: //Work Bench
//        //            return 36;
//        //        case 26: //Demon Alter
//        //            return -1000; //oops
//        //        case 77: //Hell Forge
//        //            return 221;
//        //        case 86: //Loom
//        //            return 332;
//        //        case 94: //Keg 
//        //            return 352;
//        //        case 96: //Cooking Pot
//        //            return 345;
//        //        case 101: //Bookcase
//        //            return 354;
//        //        case 106: //Sawmill
//        //            return 363;
//        //        case 114: //Tinkerer's Workshop
//        //            return 398;
//        //        case 125: //Crystal Ball
//        //            return 487;
//        //        case 133: //Adamantite Forge
//        //            return 524;
//        //        case 134: //Mythril Anvil
//        //            return 525;
//        //        case 217: //Blend-O-Matic
//        //            return 995;
//        //        case 218: //Meat Grinder
//        //            return 996;
//        //        case 219: //Extractinator
//        //            return 997;
//        //        case 220: //Solidifier
//        //            return 998;
//        //        case 243: //Imbuing Station
//        //            return 1430;
//        //        case 247: //Autohammer
//        //            return 1551;
//        //        case 283: //Heavy Work Bench
//        //            return 2172;
//        //        case 300: //Bone Welder
//        //            return 2192;
//        //        case 301: //Flesh Cloning Vat
//        //            return 2193;
//        //        case 302: //Glass Kiln
//        //            return 2194;
//        //        case 303: //Lihzahrd Furnace
//        //            return 2195;
//        //        case 304: //Living Loom
//        //            return 2196;
//        //        case 305: //Sky Mill
//        //            return 2197;
//        //        case 306: //Ice Machine
//        //            return 2198;
//        //        case 307: //Steampunk Boiler
//        //            return 2203;
//        //        case 308: //Honey Dispenser
//        //            return 2204;
//        //    }
//        //    return 0;
//        //}

//        //public static bool IsNumeric(this Type type)
//        //{
//        //    switch (Type.GetTypeCode(type))
//        //    {
//        //        case TypeCode.Byte:
//        //        case TypeCode.SByte:
//        //        case TypeCode.UInt16:
//        //        case TypeCode.UInt32:
//        //        case TypeCode.UInt64:
//        //        case TypeCode.Int16:
//        //        case TypeCode.Int32:
//        //        case TypeCode.Int64:
//        //        case TypeCode.Decimal:
//        //        case TypeCode.Double:
//        //        case TypeCode.Single:
//        //            return true;
//        //        default:
//        //            return false;
//        //    }
//        //}
//	}
//}
