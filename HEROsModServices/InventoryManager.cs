using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.UI;

namespace HEROsMod.HEROsModServices
{
	internal class InventoryManager : HEROsModService
	{
		private static float scale = .5f;

		private static Player player
		{
			get { return Main.player[Main.myPlayer]; }
		}

		private static bool[] lockedSlots = new bool[40];

		public static string categoryName = "Inventory Manager";

		// public static KeyBinding kQuickMoveItem;
		private static KeyBinding kQuickStack;

		private static KeyBinding kSortInventory;
		private static KeyBinding kSwapHotbar;

		private static int[] _itemSortArray;
		private static bool Loaded;

		public InventoryManager()
		{
			this._name = "Inventory Manager";
			// _itemSortArray = UIKit.UIComponents.ItemBrowser.CategoriesToSortingArray();
			//ParseList();
		}

		public override void Unload()
		{
			Loaded = false;
		}

		public static void SetKeyBindings()
		{
			// KeybindController.SetCatetory("InventoryManager");

			//		kQuickMoveItem = KeybindController.AddKeyBinding("Quick Move Item", "LeftControl");
			kQuickStack = KeybindController.AddKeyBinding("Quick Stack", Keys.Q);
			kSortInventory = KeybindController.AddKeyBinding("Sort Inventory", Keys.C);
			kSwapHotbar = KeybindController.AddKeyBinding("Swap Hotbar", Keys.V);
		}

		public override void Update()
		{
			if (!Main.gameMenu && !Main.mapFullscreen)
			{
				//if (kQuickMoveItem.KeyDown && Main.playerInventory)
				//{
				//    CheckInventory();
				//    if (player.chest != -1) CheckChest();
				//}
				//if (kQuickMoveItem.KeyDown && Main.playerInventory && ((Main.mouseLeft && Main.mouseLeftRelease) || (Main.mouseRight && Main.mouseRightRelease)))
				//{
				//    CheckQuickCraft();
				//    CheckQuickBuy();
				//}

				if (Main.playerInventory)
				{
					if (kSortInventory.KeyPressed)
					{
						//Terraria.UI.ItemSorting.Sort();
						ModUtils.Sort();
						Recipe.FindRecipes();
						//Console.WriteLine("clean inv");
						//CleanInventory();
					}
				}

				//if (Main.playerInventory && player.chest != -1)
				{
					if (kQuickStack.KeyPressed)
					{
						Player player = Main.player[Main.myPlayer];
						if (player.chest != -1)
						{
							ChestUI.QuickStack();
						}
						else
						{
							player.QuickStackAllChests();
							Recipe.FindRecipes();
						}
					}
				}

				if (kSwapHotbar.KeyPressed)
				{
					SwapHotbar();
				}
			}
		}

		private static void SwapHotbar()
		{
			Item[] tempItems = new Item[10];
			for (int i = 0; i < 10; i++)
			{
				tempItems[i] = (Item)player.inventory[i].Clone();
				player.inventory[i] = (Item)player.inventory[40 + i].Clone();
			}

			for (int i = 0; i < 10; i++)
			{
				player.inventory[40 + i] = (Item)tempItems[i].Clone();
			}
			SoundEngine.PlaySound(SoundID.Grab);
		}

		private static Item MoveItemToContainer(Item _item, byte destination, int invNum)
		{
			Item item = _item;
			int chestNum = player.chest;

			if (item.type > 0)
			{
				Item[] container = new Item[0];
				if (destination == 0) container = player.inventory;
				else if (destination == 1) container = Main.chest[player.chest].item;
				else if (destination == 2) container = player.bank.item;
				else if (destination == 3) container = player.bank2.item;

				for (int i = 0; i < container.Length; i++)  //interate slots in chest
				{
					Item containerItem = container[i];
					if (containerItem.type == item.type)  //if chest item is the same type as clicked inventory item
					{
						int remainingChestItemStackSpace = containerItem.maxStack - containerItem.stack;

						containerItem.stack += item.stack;

						if (containerItem.stack > containerItem.maxStack)
							containerItem.stack = containerItem.maxStack;

						item.stack -= remainingChestItemStackSpace;
						if (player.chest > -1)
						{
							if (destination == 1)
								NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)i, 0f, 0f, 0);
							else if (destination == 0) NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)invNum, 0f, 0f, 0);
						}
						if (item.stack <= 0) //if the stack is empty
						{
							item.SetDefaults(0, false);
							SoundEngine.PlaySound(SoundID.Grab);
							break;
						}
					}
				}

				if (destination == 0 && item.type > 0 && item.ammo > 0)
				{
					for (int i = 54; i < 58; i++)
					{
						Item ammoItem = container[i];
						if (ammoItem.type == 0)
						{
							container[i] = (Item)item.Clone();
							item.SetDefaults(0, false);
							if (Main.netMode == 1)
							{
								if (player.chest > -1)
								{
									if (destination == 1)
										NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)i, 0f, 0f, 0);
									else if (destination == 0) NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)invNum, 0f, 0f, 0);
								}
							}
							SoundEngine.PlaySound(SoundID.Grab);
							break;
						}
					}
				}

				if (item.type > 0) //if the item did not get turned to consumed by the previous step, we look for an empty slot in the chest
				{
					int numOfSlots = 50;
					if (destination > 0) numOfSlots = 40;
					for (int i = 0; i < numOfSlots; i++)
					{
						Item containerItem = container[i];

						if (containerItem.type == 0)
						{
							container[i] = (Item)item.Clone();
							item.SetDefaults(0, false);
							if (Main.netMode == 1)
							{
								if (player.chest > -1)
								{
									if (destination == 1)
										NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)i, 0f, 0f, 0);
									else if (destination == 0) NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, (float)invNum, 0f, 0f, 0);
								}
							}
							SoundEngine.PlaySound(SoundID.Grab);
							break;
						}
					}
				}
				if (destination == 0)
					player.inventory = container;
				else if (destination == 1) Main.chest[player.chest].item = container;
				else if (destination == 2) player.bank.item = container;
				else if (destination == 3) player.bank2.item = container;
			}
			Recipe.FindRecipes();
			return item;
		}

		//static bool CheckChest()
		//{
		//	float inventoryScale = 0.755f;
		//	int invBottom = 258;

		//	for (int x = 0; x < 10; x++)
		//	{
		//		for (int y = 0; y < 4; y++)
		//		{
		//			int itemPosX = (int)(73f + (float)(x * 56) * inventoryScale);
		//			int itemPosY = (int)((float)invBottom + (float)(y * 56) * inventoryScale);
		//			int itemNum = x + y * 10;
		//			if (Main.mouseX >= itemPosX && (float)Main.mouseX <= (float)itemPosX + (float)Main.inventoryBackTexture.Width * inventoryScale && Main.mouseY >= itemPosY && (float)Main.mouseY <= (float)itemPosY + (float)Main.inventoryBackTexture.Height * inventoryScale)
		//			{
		//				Main.player[Main.myPlayer].mouseInterface = true;
		//				Player p = player;

		//				for (int i = 0; i < 10; i++)
		//				{
		//					int slotNum = i - 1;
		//					if (i == 0) slotNum = 9;
		//					Keys keyNum = (Keys)Enum.Parse(typeof(Keys), "D" + i.ToString());
		//					if (Main.keyState.IsKeyDown(keyNum) && ModUtils.PreviousKeyboardState.IsKeyUp(keyNum))
		//					{
		//						Item tempItem = new Item();
		//						tempItem.SetDefaults(0, false);
		//						if (player.chest > -1)
		//						{
		//							tempItem = (Item)Main.chest[player.chest].item[itemNum].Clone();
		//							Main.chest[player.chest].item[itemNum] = (Item)p.inventory[slotNum].Clone();
		//						}
		//						else if (player.chest == -2)
		//						{
		//							tempItem = (Item)p.bank.item[itemNum].Clone();
		//							p.bank.item[itemNum] = (Item)p.inventory[slotNum].Clone();
		//						}
		//						else if (player.chest == -3)
		//						{
		//							tempItem = (Item)p.bank2.item[itemNum].Clone();
		//							p.bank2.item[itemNum] = (Item)p.inventory[slotNum].Clone();
		//						}
		//						p.inventory[slotNum] = (Item)tempItem.Clone();
		//					}
		//				}

		//				if (player.chest != -1 && Main.mouseLeft && Main.mouseLeftRelease)
		//				{
		//					if (p.chest > -1) Main.chest[p.chest].item[itemNum] = MoveItemToContainer(Main.chest[p.chest].item[itemNum], 0, itemNum);
		//					else if (p.chest == -2) p.bank.item[itemNum] = MoveItemToContainer(p.bank.item[itemNum], 0, itemNum);
		//					else if (p.chest == -3) p.bank2.item[itemNum] = MoveItemToContainer(p.bank2.item[itemNum], 0, itemNum);
		//					Main.mouseLeftRelease = false;
		//				}
		//				return true;
		//			}
		//		}

		//	}
		//	return false;
		//}

		//static bool CheckInventory()
		//{
		//	if (Main.playerInventory)
		//	{
		//		float inventoryScale = .85f;
		//		for (int x = 0; x < 10; x++)
		//		{
		//			for (int y = 0; y < 5; y++)
		//			{
		//				int itemPosX = (int)(20f + (float)(x * 56) * inventoryScale);
		//				int itemPosY = (int)(20f + (float)(y * 56) * inventoryScale);
		//				int itemNum = x + y * 10;
		//				if (Main.mouseX >= itemPosX && (float)Main.mouseX <= (float)itemPosX + (float)Main.inventoryBackTexture.Width * inventoryScale && Main.mouseY >= itemPosY && (float)Main.mouseY <= (float)itemPosY + (float)Main.inventoryBackTexture.Height * inventoryScale)
		//				{
		//					if (Main.player[Main.myPlayer].inventory[itemNum].type > 0)
		//					{
		//						Main.player[Main.myPlayer].mouseInterface = true;
		//						Player p = player;
		//						Item inventoryItem = p.inventory[itemNum]; //clicked inventory item
		//						Item[] currentOpenChestItems = new Item[0];

		//						for (int i = 0; i < 10; i++)
		//						{
		//							int slotNum = i - 1;
		//							if (i == 0) slotNum = 9;
		//							Keys keyNum = (Keys)Enum.Parse(typeof(Keys), "D" + i.ToString());
		//							if (slotNum != itemNum && Main.keyState.IsKeyDown(keyNum) && ModUtils.PreviousKeyboardState.IsKeyUp(keyNum))
		//							{
		//								Item tempItem = (Item)inventoryItem.Clone();
		//								p.inventory[itemNum] = (Item)p.inventory[slotNum].Clone();
		//								p.inventory[slotNum] = (Item)tempItem.Clone();
		//							}
		//						}

		//						if (player.chest != -1 && Main.mouseLeft && Main.mouseLeftRelease)
		//						{
		//							if (p.chest > -1) p.inventory[itemNum] = MoveItemToContainer(inventoryItem, 1, -1);
		//							else if (p.chest == -2) p.inventory[itemNum] = MoveItemToContainer(inventoryItem, 2, -1);
		//							else if (p.chest == -3) p.inventory[itemNum] = MoveItemToContainer(inventoryItem, 3, -1);
		//							Main.mouseLeftRelease = false;

		//						}
		//					}
		//					if (Main.mouseRight && Main.mouseRightRelease)
		//					{
		//						if (itemNum >= 10 && itemNum < 50)
		//						{
		//							lockedSlots[itemNum - 10] = !lockedSlots[itemNum - 10];
		//							Main.mouseRightRelease = false;
		//						}
		//					}

		//					return true;

		//				}
		//			}
		//		}
		//	}
		//	return false;
		//}

		private static void DrawLocks(SpriteBatch spriteBatch)
		{
			if (Main.playerInventory)
			{
				float inventoryScale = .85f;
				for (int x = 0; x < 10; x++)
				{
					for (int y = 1; y < 5; y++)
					{
						int itemPosX = (int)(20f + (float)(x * 56) * inventoryScale);
						int itemPosY = (int)(20f + (float)(y * 56) * inventoryScale);
						int itemNum = x + y * 10;

						bool locked = lockedSlots[itemNum - 10];
						if (locked)
						{
							float scale = .75f;
							Vector2 pos = new Vector2(itemPosX + 25, itemPosY + 2);
							spriteBatch.Draw(TextureAssets.HbLock[0].Value, pos, null, Color.DarkGray * .8f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}

		private static bool CheckQuickCraft()
		{
			for (int num75 = 0; num75 < Recipe.maxRecipes; num75++)
			{
				int num50 = (Main.screenHeight - 600) / 2;
				int num51 = (int)((float)Main.screenHeight / 600f * 250f);
				if (Main.screenHeight < 700)
				{
					num50 = (Main.screenHeight - 508) / 2;
					num51 = (int)((float)Main.screenHeight / 600f * 200f);
				}
				float inventoryScale = 100f / (Math.Abs(Main.availableRecipeY[num75]) + 100f);
				if ((double)inventoryScale < 0.75)
				{
					inventoryScale = 0.75f;
				}
				if (Main.recFastScroll)
				{
					inventoryScale = 0.75f;
				}

				if (num75 < Main.numAvailableRecipes && Math.Abs(Main.availableRecipeY[num75]) <= (float)num51)
				{
					int num76 = (int)(46f - 26f * inventoryScale);
					int num77 = (int)(410f + Main.availableRecipeY[num75] * inventoryScale - 30f * inventoryScale + (float)num50);

					if (Main.mouseX >= num76 && (float)Main.mouseX <= (float)num76 + (float)TextureAssets.InventoryBack.Value.Width * inventoryScale && Main.mouseY >= num77 && (float)Main.mouseY <= (float)num77 + (float)TextureAssets.InventoryBack.Value.Height * inventoryScale)
					{
						Main.player[Main.myPlayer].mouseInterface = true;
						if (Main.focusRecipe == num75 && Main.guideItem.type == 0)
						{
							if (Main.mouseItem.type == 0 || (Main.mouseItem.type == Main.recipe[Main.availableRecipe[num75]].createItem.type && Main.mouseItem.stack + Main.recipe[Main.availableRecipe[num75]].createItem.stack <= Main.mouseItem.maxStack))
							{
								if (Main.mouseLeft && Main.mouseLeftRelease)
								{
									Recipe r = Main.recipe[Main.availableRecipe[num75]];
									int prevNumAvailableRecipes = Main.numAvailableRecipes;
									Item tempItem = (Item)r.createItem.Clone();
									tempItem.stack = 0;

									bool playsound = false;
									bool matsLeft = true;
									while (matsLeft && tempItem.stack + r.createItem.stack <= r.createItem.maxStack)
									{
										playsound = true;
										tempItem.stack += r.createItem.stack;
										r.Create();
										if (Main.numAvailableRecipes <= 0 || r.createItem.type != Main.recipe[Main.availableRecipe[Main.focusRecipe]].createItem.type || r.createItem.stack != Main.recipe[Main.availableRecipe[Main.focusRecipe]].createItem.stack)
										{
											matsLeft = false;
										}
									}
									if (playsound)
									{
										Main.mouseLeftRelease = false;
										SoundEngine.PlaySound(SoundID.Grab);
									}
									Main.mouseItem = (Item)tempItem.Clone();
									Recipe.FindRecipes();
								}
								else if (Main.mouseRight && Main.mouseRightRelease)
								{
									Player p = player;
									Recipe r = Main.recipe[Main.availableRecipe[num75]];
									//int prevNumAvailableRecipes = Main.numAvailableRecipes;
									int prevRecipeNum = Main.availableRecipe[num75];
									Item tempItem = (Item)r.createItem.Clone();
									tempItem.stack = 0;

									bool hasEmptySlot = false;
									for (int i = 0; i < p.inventory.Length - 9; i++)
									{
										if (p.inventory[i].type == 0)
										{
											hasEmptySlot = true;
											break;
										}
									}

									if (hasEmptySlot)
									{
										bool playsound = false;

										bool matsLeft = true;
										while (matsLeft && tempItem.stack + r.createItem.stack <= r.createItem.maxStack)
										{
											playsound = true;
											tempItem.stack += r.createItem.stack;
											r.Create();
											if (Main.numAvailableRecipes <= 0 || r.createItem.type != Main.recipe[Main.availableRecipe[Main.focusRecipe]].createItem.type)
											{
												matsLeft = false;
											}
										}
										//Console.WriteLine("poo");
										if (playsound)
										{
											Main.mouseRightRelease = false;
											SoundEngine.PlaySound(SoundID.Grab);
										}
										if (tempItem.stack > 0)
										{
											for (int i = 0; i < p.inventory.Length - 9; i++)
											{
												//subtract 9 from the inventory length so we don't overflow into ammo and currency slots
												//not sure what the 9th is though, mouse slot? anyway it's 9
												int lastInventorySlot = p.inventory.Length - 9 - 1;
												int index = lastInventorySlot - i;
												if (p.inventory[index].type == 0)
												{
													p.inventory[index] = (Item)tempItem.Clone();
													break;
												}
											}
										}
									}
									Recipe.FindRecipes();
								}
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private static void CheckQuickBuy()
		{
			if (Main.npcShop > 0)
			{
				float inventoryScale = 0.755f;
				int invBottom = 258;
				if (Main.mouseX > 73 && Main.mouseX < (int)(73f + 560f * inventoryScale) && Main.mouseY > invBottom && Main.mouseY < (int)((float)invBottom + 224f * inventoryScale))
				{
					Main.player[Main.myPlayer].mouseInterface = true;
				}
				for (int num120 = 0; num120 < 10; num120++)
				{
					for (int num121 = 0; num121 < 4; num121++)
					{
						int num122 = (int)(73f + (float)(num120 * 56) * inventoryScale);
						int num123 = (int)((float)invBottom + (float)(num121 * 56) * inventoryScale);
						int num124 = num120 + num121 * 10;
						Color white13 = new Color(100, 100, 100, 100);
						if (Main.mouseX >= num122 && (float)Main.mouseX <= (float)num122 + (float)TextureAssets.InventoryBack.Value.Width * inventoryScale && Main.mouseY >= num123 && (float)Main.mouseY <= (float)num123 + (float)TextureAssets.InventoryBack.Value.Height * inventoryScale)
						{
							Main.player[Main.myPlayer].mouseInterface = true;

							if (Main.mouseItem.type == 0 && Main.instance.shop[Main.npcShop].item[num124].type > 0)
							{
								if ((Main.player[Main.myPlayer].selectedItem != num124 || Main.player[Main.myPlayer].itemAnimation <= 0) && Main.player[Main.myPlayer].itemTime == 0)
								{
									if (!Main.instance.shop[Main.npcShop].item[num124].buyOnce)
									{
										Player p = player;
										Item shopItem = Main.instance.shop[Main.npcShop].item[num124];
										if (Main.mouseLeft && Main.mouseLeftRelease)
										{
											Item tempItem = (Item)shopItem.Clone();
											tempItem.stack = 0;

											bool playsound = false;
											while (p.BuyItem(Main.instance.shop[Main.npcShop].item[num124].value) && tempItem.stack + 1 <= tempItem.maxStack)
											{
												playsound = true;
												tempItem.stack++;
											}
											if (playsound) SoundEngine.PlaySound(SoundID.Coins);
											if (tempItem.stack > 0)
												Main.mouseItem = (Item)tempItem.Clone();
											Recipe.FindRecipes();
										}
										else if (Main.mouseRight && Main.mouseRightRelease)
										{
											Item tempItem = (Item)shopItem.Clone();
											tempItem.stack = 0;

											bool hasEmptySlot = false;
											for (int i = 0; i < p.inventory.Length - 9; i++)
											{
												if (p.inventory[i].type == 0)
												{
													hasEmptySlot = true;
													break;
												}
											}
											if (hasEmptySlot)
											{
												bool playsound = false;
												while (p.BuyItem(Main.instance.shop[Main.npcShop].item[num124].value) && tempItem.stack + 1 <= tempItem.maxStack)
												{
													tempItem.stack++;
													playsound = true;
												}

												if (playsound) SoundEngine.PlaySound(SoundID.Coins);
												if (tempItem.stack > 0)
												{
													for (int i = 0; i < p.inventory.Length - 9; i++)
													{
														//subtract 9 from the inventory length so we don't overflow into ammo and currency slots
														//not sure what the 9th is though, mouse slot? anyway it's 9
														int lastInventorySlot = p.inventory.Length - 9 - 1;
														int index = lastInventorySlot - i;
														if (p.inventory[index].type == 0)
														{
															p.inventory[index] = (Item)tempItem.Clone();
															break;
														}
													}
												}
											}
											Recipe.FindRecipes();
										}
									}
								}
							}
						}
					}
				}
			}
		}

		//public static void CleanInventory()
		//{
		//	if (!Loaded)
		//	{
		//		_itemSortArray = UIKit.UIComponents.ItemBrowser.CategoriesToSortingArray();
		//		Loaded = true;
		//	}

		//	bool playSound = false;
		//	Player p = player;
		//	if (player.chest == -1)
		//	{
		//		for (int i = p.inventory.Length - 9; i < p.inventory.Length - 1; i++)
		//		{
		//			Item item1 = p.inventory[i];
		//			if (item1.type != 0 && item1.stack < item1.maxStack)
		//			{
		//				for (int j = 0; j < p.inventory.Length - 1; j++)
		//				{
		//					Item item2 = p.inventory[j];
		//					if (i != j && item2.stack < item2.maxStack)
		//					{
		//						if (item1.type == item2.type)
		//						{
		//							playSound = true;
		//							int currentItem1StackSpaceRemaining = item1.maxStack - item1.stack;

		//							item1.stack += item2.stack;
		//							item2.stack -= currentItem1StackSpaceRemaining;
		//							if (item2.stack <= 0)
		//							{
		//								p.inventory[j].SetDefaults(0, false);
		//							}
		//							if (item1.stack >= item1.maxStack)
		//							{
		//								p.inventory[i].stack = item1.maxStack;
		//								break;
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}

		//		for (int i = 0; i < p.inventory.Length - 9; i++)
		//		{
		//			Item item1 = p.inventory[i];
		//			if (item1.type != 0 && item1.stack < item1.maxStack)
		//			{
		//				for (int j = i; j < p.inventory.Length - 9; j++)
		//				{
		//					Item item2 = p.inventory[j];
		//					if (i != j) //not same slot
		//					{
		//						if (item1.type == item2.type)
		//						{
		//							playSound = true;
		//							int currentItem1StackSpaceRemaining = item1.maxStack - item1.stack;

		//							item1.stack += item2.stack;
		//							item2.stack -= currentItem1StackSpaceRemaining;
		//							if (item2.stack <= 0)
		//							{
		//								p.inventory[j].SetDefaults(0, false);
		//							}
		//							if (item1.stack >= item1.maxStack)
		//							{
		//								p.inventory[i].stack = item1.maxStack;
		//								break;
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}
		//		int numOfUnlockedSlots = 0;
		//		foreach (bool slot in lockedSlots)
		//		{
		//			if (!slot) numOfUnlockedSlots++;
		//		}
		//		Item[] items = new Item[numOfUnlockedSlots];
		//		int count = 0;
		//		for (int i = 0; i < 40; i++)
		//		{
		//			if (!lockedSlots[i])
		//			{
		//				items[count] = (Item)player.inventory[i + 10].Clone();
		//				count++;
		//			}
		//		}
		//		items = items.OrderBy(s => _itemSortArray[s.type]).ToArray();
		//		count = 0;
		//		for (int i = 0; i < 40; i++)
		//		{
		//			if (!lockedSlots[i])
		//			{
		//				if (player.inventory[i + 10].type != items[count].type) playSound = true;
		//				player.inventory[i + 10] = (Item)items[count].Clone();
		//				count++;
		//			}
		//		}
		//		if (playSound)
		//		{
		//			Main.PlaySound(7, -1, -1, 1);
		//		}
		//	}
		//	else
		//	{
		//		Item[] chestContainer = new Item[0];
		//		if (player.chest > -1) chestContainer = Main.chest[player.chest].item;
		//		else if (player.chest == -2) chestContainer = p.bank.item;
		//		else if (player.chest == -3) chestContainer = p.bank2.item;

		//		for (int i = 0; i < chestContainer.Length; i++)
		//		{
		//			Item item1 = chestContainer[i];
		//			if (item1.type != 0 && item1.stack < item1.maxStack)
		//			{
		//				for (int j = i; j < chestContainer.Length; j++)
		//				{
		//					Item item2 = chestContainer[j];
		//					if (i != j) //not same slot
		//					{
		//						if (item1.type == item2.type)
		//						{
		//							playSound = true;
		//							int currentItem1StackSpaceRemaining = item1.maxStack - item1.stack;

		//							item1.stack += item2.stack;
		//							item2.stack -= currentItem1StackSpaceRemaining;
		//							if (item2.stack <= 0)
		//							{
		//								chestContainer[j].SetDefaults(0, false);
		//							}
		//							if (item1.stack >= item1.maxStack)
		//							{
		//								chestContainer[i].stack = item1.maxStack;
		//								break;
		//							}
		//							if (Main.netMode == 1 && player.chest > -1)
		//							{
		//								NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)i, 0f, 0f, 0);
		//								NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)j, 0f, 0f, 0);
		//							}
		//						}
		//					}
		//				}
		//			}
		//		}

		//		Item[] items = new Item[40];
		//		for (int i = 0; i < items.Length; i++)
		//		{
		//			items[i] = (Item)chestContainer[i].Clone();
		//		}
		//		items = items.OrderBy(s => _itemSortArray[s.type]).ToArray();
		//		for (int i = 0; i < items.Length; i++)
		//		{
		//			if (chestContainer[i].type != items[i].type) playSound = true;
		//			chestContainer[i] = (Item)items[i].Clone();
		//			if (Main.netMode == 1 && player.chest > -1)
		//			{
		//				NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)i, 0f, 0f, 0);
		//			}
		//		}

		//		if (playSound)
		//		{
		//			Main.PlaySound(7, -1, -1, 1);
		//		}
		//	}
		//}

		public override void Draw(SpriteBatch spriteBatch)
		{
			DrawLocks(spriteBatch);
		}

		//private static void QuickStack()
		//{
		//	if (Main.player[Main.myPlayer].chest > -1)
		//	{
		//		Main.MoveCoins(Main.player[Main.myPlayer].inventory, Main.chest[Main.player[Main.myPlayer].chest].item);
		//	}
		//	else if (Main.player[Main.myPlayer].chest == -3)
		//	{
		//		Main.MoveCoins(Main.player[Main.myPlayer].inventory, Main.player[Main.myPlayer].bank2.item);
		//	}
		//	else
		//	{
		//		Main.MoveCoins(Main.player[Main.myPlayer].inventory, Main.player[Main.myPlayer].bank.item);
		//	}
		//	if (Main.player[Main.myPlayer].chest > -1)
		//	{
		//		for (int num35 = 0; num35 < Chest.maxItems; num35++)
		//		{
		//			if (Main.chest[Main.player[Main.myPlayer].chest].item[num35].type > 0 && Main.chest[Main.player[Main.myPlayer].chest].item[num35].stack < Main.chest[Main.player[Main.myPlayer].chest].item[num35].maxStack)
		//			{
		//				for (int n = 0; n < 58; n++)
		//				{
		//					if (Main.chest[Main.player[Main.myPlayer].chest].item[num35].IsTheSameAs(Main.player[Main.myPlayer].inventory[n]))
		//					{
		//						int num53 = Main.player[Main.myPlayer].inventory[n].stack;
		//						if (Main.chest[Main.player[Main.myPlayer].chest].item[num35].stack + num53 > Main.chest[Main.player[Main.myPlayer].chest].item[num35].maxStack)
		//						{
		//							num53 = Main.chest[Main.player[Main.myPlayer].chest].item[num35].maxStack - Main.chest[Main.player[Main.myPlayer].chest].item[num35].stack;
		//						}
		//						Main.PlaySound(7, -1, -1, 1);
		//						Main.chest[Main.player[Main.myPlayer].chest].item[num35].stack += num53;
		//						Main.player[Main.myPlayer].inventory[n].stack -= num53;
		//						if (Main.player[Main.myPlayer].inventory[n].stack == 0)
		//						{
		//							Main.player[Main.myPlayer].inventory[n].SetDefaults(0, false);
		//						}
		//						else if (Main.chest[Main.player[Main.myPlayer].chest].item[num35].type == 0)
		//						{
		//							Main.chest[Main.player[Main.myPlayer].chest].item[num35] = Main.player[Main.myPlayer].inventory[n].Clone();
		//							Main.player[Main.myPlayer].inventory[n].SetDefaults(0, false);
		//						}
		//						if (Main.netMode == 1)
		//						{
		//							NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)num35, 0f, 0f, 0);
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}
		//	else if (Main.player[Main.myPlayer].chest == -3)
		//	{
		//		for (int num35 = 0; num35 < Chest.maxItems; num35++)
		//		{
		//			if (Main.player[Main.myPlayer].bank2.item[num35].type > 0 && Main.player[Main.myPlayer].bank2.item[num35].stack < Main.player[Main.myPlayer].bank2.item[num35].maxStack)
		//			{
		//				for (int n = 0; n < 58; n++)
		//				{
		//					if (Main.player[Main.myPlayer].bank2.item[num35].IsTheSameAs(Main.player[Main.myPlayer].inventory[n]))
		//					{
		//						int num53 = Main.player[Main.myPlayer].inventory[n].stack;
		//						if (Main.player[Main.myPlayer].bank2.item[num35].stack + num53 > Main.player[Main.myPlayer].bank2.item[num35].maxStack)
		//						{
		//							num53 = Main.player[Main.myPlayer].bank2.item[num35].maxStack - Main.player[Main.myPlayer].bank2.item[num35].stack;
		//						}
		//						Main.PlaySound(7, -1, -1, 1);
		//						Main.player[Main.myPlayer].bank2.item[num35].stack += num53;
		//						Main.player[Main.myPlayer].inventory[n].stack -= num53;
		//						if (Main.player[Main.myPlayer].inventory[n].stack == 0)
		//						{
		//							Main.player[Main.myPlayer].inventory[n].SetDefaults(0, false);
		//						}
		//						else if (Main.player[Main.myPlayer].bank2.item[num35].type == 0)
		//						{
		//							Main.player[Main.myPlayer].bank2.item[num35] = Main.player[Main.myPlayer].inventory[n].Clone();
		//							Main.player[Main.myPlayer].inventory[n].SetDefaults(0, false);
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}
		//	else
		//	{
		//		for (int num35 = 0; num35 < Chest.maxItems; num35++)
		//		{
		//			if (Main.player[Main.myPlayer].bank.item[num35].type > 0 && Main.player[Main.myPlayer].bank.item[num35].stack < Main.player[Main.myPlayer].bank.item[num35].maxStack)
		//			{
		//				for (int n = 0; n < 58; n++)
		//				{
		//					if (Main.player[Main.myPlayer].bank.item[num35].IsTheSameAs(Main.player[Main.myPlayer].inventory[n]))
		//					{
		//						int num53 = Main.player[Main.myPlayer].inventory[n].stack;
		//						if (Main.player[Main.myPlayer].bank.item[num35].stack + num53 > Main.player[Main.myPlayer].bank.item[num35].maxStack)
		//						{
		//							num53 = Main.player[Main.myPlayer].bank.item[num35].maxStack - Main.player[Main.myPlayer].bank.item[num35].stack;
		//						}
		//						Main.PlaySound(7, -1, -1, 1);
		//						Main.player[Main.myPlayer].bank.item[num35].stack += num53;
		//						Main.player[Main.myPlayer].inventory[n].stack -= num53;
		//						if (Main.player[Main.myPlayer].inventory[n].stack == 0)
		//						{
		//							Main.player[Main.myPlayer].inventory[n].SetDefaults(0, false);
		//						}
		//						else if (Main.player[Main.myPlayer].bank.item[num35].type == 0)
		//						{
		//							Main.player[Main.myPlayer].bank.item[num35] = Main.player[Main.myPlayer].inventory[n].Clone();
		//							Main.player[Main.myPlayer].inventory[n].SetDefaults(0, false);
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}
		//}

		//static int currentSaveVersion = 2;
		//public static void Save()
		//{
		//	using (FileStream fileStream = new FileStream(Main.SavePath + Path.DirectorySeparatorChar + "inventoryManager.dat", FileMode.Create))
		//	{
		//		using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
		//		{
		//			int saveVersion = currentSaveVersion;
		//			binaryWriter.Write(saveVersion);
		//			for (int i = 0; i < lockedSlots.Length; i++)
		//			{
		//				binaryWriter.Write(lockedSlots[i]);
		//			}
		//			binaryWriter.Close();
		//		}
		//	}
		//}

		//public static void Load()
		//{
		//	for (int i = 0; i < lockedSlots.Length; i++)
		//		lockedSlots[i] = false;
		//	if (File.Exists(Main.SavePath + Path.DirectorySeparatorChar + "inventoryManager.dat"))
		//	{
		//		using (FileStream fileStream = new FileStream(Main.SavePath + Path.DirectorySeparatorChar + "inventoryManager.dat", FileMode.Open))
		//		{
		//			using (BinaryReader binaryReader = new BinaryReader(fileStream))
		//			{
		//				try
		//				{
		//					int saveVersion = binaryReader.Read();
		//					if (saveVersion != currentSaveVersion)
		//					{
		//						for (int i = 0; i < 40; i++)
		//						{
		//							lockedSlots[i] = false;
		//						}
		//					}
		//					else
		//					{
		//						for (int i = 0; i < 40; i++)
		//						{
		//							lockedSlots[i] = binaryReader.ReadBoolean();
		//						}
		//					}
		//					binaryReader.Close();
		//				}
		//				catch
		//				{
		//					for (int i = 0; i < 40; i++)
		//					{
		//						lockedSlots[i] = false;
		//					}
		//					binaryReader.Close();
		//				}
		//			}
		//		}
		//	}
		//}

		//private static void ExportItems()
		//{
		//	Item[] items = new Item[Main.itemTexture.Length];
		//	string outString = "";
		//	for (int i = 0; i < items.Length; i++)
		//	{
		//		items[i] = new Item();
		//		items[i].SetDefaults(i, false);
		//		outString += i + " " + items[i].name + ",\r\n";
		//	}
		//	File.WriteAllText("items.txt", outString);
		//}

		//static void ParseList()
		//{
		//	string list = File.ReadAllText("list.txt");
		//	string pattern = @"\n\d+\s[\(\)\.\w\d\s'_-]+,";
		//	MatchCollection matches = Regex.Matches(list, pattern);

		//	int[] nums = new int[Main.itemTexture.Length];
		//	int count = 0;
		//	foreach (Match match in matches)
		//	{
		//		foreach (Capture capture in match.Captures)
		//		{
		//			string pattern2 = @"\n\d+\s";
		//			Match m = Regex.Match(capture.Value, pattern2);
		//			if (m.Success)
		//			{
		//				nums[count] = int.Parse(m.Groups[0].ToString());
		//				//Console.WriteLine(nums[count]);
		//				count++;
		//			}
		//		}
		//	}
		//	Console.WriteLine(count);

		//	int[] nums2 = new int[nums.Length];
		//	nums2[0] = 5000;
		//	bool[] missing = new bool[Main.itemTexture.Length];
		//	for (int i = 0; i < missing.Length; i++) missing[i] = true;
		//	for (int i = 0; i < nums.Length; i++)
		//	{
		//		if (nums[i] > 0) missing[nums[i]] = false;
		//		nums2[nums[i]] = i;
		//	}

		//	string output = "#region sorting\r\npublic static int[] itemSortNums = new int[]\r\n{\r\n";
		//	for (int i = 0; i < nums.Length; i++)
		//	{
		//		output += nums2[i] + ",";
		//		if (i % 50 == 0) output += "\r\n";
		//	}
		//	for (int i = 0; i < missing.Length; i++)
		//	{
		//		if (missing[i]) Console.WriteLine("missing" + i);
		//	}
		//	File.WriteAllText("output.txt", output);
		//}
	}
}