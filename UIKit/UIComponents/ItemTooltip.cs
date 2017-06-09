//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using HEROsModMod.HEROsModServices;
//using HEROsModMod.HEROsModVideo.Services.DropRateInfo;
////using HEROsModMod.HEROsModVideo.Services.ChestDropsInfo;

//using Terraria;

//namespace HEROsModMod.UIKit.UIComponents
//{
//    class ItemTooltip : UIWindow
//    {
//        //private int previousItemType = 0;
//        private Item previousItem;
//        public static Color statUpColor = new Color(79, 170, 72);
//        public static bool Debug = false;
//        static KeyBinding kDebugView;
//        static KeyBinding kAltView;
//        bool _previousAltKeyDown = false;

//        public ItemTooltip()
//        {
//            this.Width = 0;
//            this.Height = 0;
//            this.UpdateWhenOutOfBounds = true;
//            previousItem = new Item();
//            previousItem.SetDefaults(0);
//        }

//        public override void Update()
//        {
//            if(kDebugView.KeyPressed)
//            {
//                Debug = !Debug;
//            }
//            if(ModUtils.HoverItem != null && ModUtils.HoverItem.netID != 0)
//            {
//                this.Position = ModUtils.CursorPosition;

//                if(previousItem.IsNotTheSameAs(ModUtils.HoverItem))
//                {
//                    BuildStats(ModUtils.HoverItem);
//                    previousItem = ModUtils.HoverItem.Clone();
//                }

//                this.Y += 16f;
//                if (Debug)
//                    this.X -= Width - Spacing;
//                else
//                    this.X += 8f;
//                if (this.X + this.Width > Main.screenWidth)
//                {
//                    X = Main.screenWidth - this.Width;
//                }
//                if (this.Y + this.Height > Main.screenHeight)
//                {
//                    this.Y = Main.screenHeight - this.Height;
//                }
//                this.Visible = true;
//            }
//            else
//            {
//                this.Visible = false;
//                previousItem.SetDefaults(0);
//            }
//            if(this.Visible && kAltView.KeyDown != _previousAltKeyDown)
//            {
//                BuildStats(ModUtils.HoverItem);
//            }
//            _previousAltKeyDown = kAltView.KeyDown;
//            base.Update();
//        }

//        public static void SetKeyBindings()
//        {
//           // KeybindController.SetCatetory("Tooltip");
//            kDebugView = KeybindController.AddKeyBinding("Debug View", "F5");
//            kAltView = KeybindController.AddKeyBinding("Alt View", "LeftAlt");
//        }

//        static float yPos = 0;
//        static float maxWidth = 0;
//        static List<UIView> stats;

//        public void BuildStats(Item item)
//        {
//            yPos = Spacing;
//            maxWidth = 0f;
//            stats = new List<UIView>();
//            this.RemoveAllChildren();

//            Color itemColor = ModUtils.GetItemColor(item);
//            if(item.stack > 1)
//            {
//                AddStat(item.name, "x" + item.stack, itemColor, .5f);
//            }
//            else
//            {
//                AddLabel(item.name, itemColor, .5f);
//            }

//            if(item.prefix > 0)
//            {
//                yPos -= 5f;
//                AddLabel(Lang.prefix[item.prefix], itemColor, .35f);
//            }

//            if (item.netID != -1000)
//            {
//                if (kAltView.KeyDown)
//                {
//                    BuildItemDropStats(item);
//                }
//                else
//                {
//                    BuildItemStats(item);
//                }
//            }

//            this.Width = maxWidth + Spacing * 2;
//            this.Height = yPos;

//            for(int i = 0; i < stats.Count; i++)
//            {
//                stats[i].X = this.Width - stats[i].Width - Spacing;
//            }
//        }

//        private void BuildItemStats(Item item)
//        {
//            Item itemDefaults = new Item();
//            itemDefaults.netDefaults(item.netID);
//            //Melee Damage
//            if (item.melee)
//            {
//                AddCompareStat("Melee Damage", item.damage, itemDefaults.damage);
//            }

//            //Ranged Damage
//            if (item.ranged)
//            {
//                AddCompareStat("Ranged Damage", item.damage, itemDefaults.damage);
//            }

//            //Magic Damaage
//            if (item.magic)
//            {
//                AddCompareStat("Magic Damage", item.damage, itemDefaults.damage);

//            }

//            if (item.damage > 0 && !item.melee && !item.ranged && !item.magic)
//            {
//                AddCompareStat("Damage", item.damage, itemDefaults.damage);
//            }

//            //Mana Use
//            if (item.mana > 0)
//            {
//                AddCompareStat("Mana", item.mana, itemDefaults.mana, "", false);
//            }

//            //Crit. Chance
//            if (item.crit > 0)
//            {
//                AddCompareStat("Crit. Chance", item.crit, itemDefaults.crit, "%");
//            }
//            //Speed
//            if (item.damage > 0)
//            {
//                AddCompareStat("Speed", item.useAnimation, itemDefaults.useAnimation, "", false, true);
//                //AddStat("Speed", GetSpeed(item.useAnimation) + "(" + item.useAnimation + ")");
//            }
//            //Knockback
//            if (item.knockBack > 0 || item.damage > 0)
//            {
//                AddCompareStat("Knockback", item.knockBack, itemDefaults.knockBack);
//                //AddStat("Knockback", GetKnockback(item.knockBack) + "(" + item.knockBack + ")");
//            }

//            //Defense
//            if (item.defense > 0)
//            {
//                AddCompareStat("Defense", item.defense, itemDefaults.defense);
//            }

//            if (item.shootSpeed > 0)
//            {
//                AddCompareStat("Velocity", item.shootSpeed, itemDefaults.shootSpeed);
//            }

//            //Pick
//            if (item.pick > 0)
//            {
//                AddStat("Pickaxe Power", "%" + item.pick);
//            }
//            //Axe
//            if (item.axe > 0)
//            {
//                AddStat("Axe Power", "%" + item.axe);
//            }
//            //Hammer
//            if (item.hammer > 0)
//            {
//                AddStat("Hammer Power", "%" + item.hammer);
//            }

//            //Bait Power
//            if (item.bait > 0)
//            {
//                AddStat("Bait Power", "%" + item.bait);
//            }

//            //fishing power
//            if (item.fishingPole > 0)
//            {
//                AddStat("Fishing Power", "%" + item.fishingPole);
//            }
//            if (item.tileBoost != 0)
//            {
//                if (item.tileBoost > 0)
//                {
//                    AddStat("Range", "+" + item.tileBoost);
//                }
//                else
//                {
//                    AddStat("Range", item.tileBoost.ToString());
//                }
//            }

//            //Equipable
//            if (item.legSlot >= 0 || item.headSlot >= 0 || item.bodySlot >= 0 || item.accessory)
//            {
//                AddLabel("Equipable");
//            }

//            //Quest Item
//            if (item.questItem)
//            {
//                AddLabel("Quest Item");
//            }
//            //Consumes Item
//            if (item.tileWand > 0)
//            {
//                AddLabel(Lang.tip[52] + Lang.itemName(item.tileWand, false));
//            }

//            //Vanity
//            {
//                if (item.vanity)
//                {
//                    AddLabel("Vanity Item");
//                }
//            }

//            //Ammo
//            if (item.ammo != 0)
//            {
//                AddLabel("Ammo");
//            }
//            //Restores life
//            if (item.healLife > 0)
//            {
//                AddStat("Heal", item.healLife.ToString());
//            }
//            //Restores Mana
//            if (item.healMana > 0)
//            {
//                AddStat("Mana", item.healMana.ToString());
//            }
//            //Consumable
//            if (item.consumable && item.ammo <= 0)
//            {
//                if (item.createTile > 0)
//                {
//                    AddLabel("Can be placed");
//                }
//                else
//                {
//                    AddLabel("Consumable");
//                }
//            }

//            //if Material
//            if (item.material)
//            {
//                AddLabel("Material");
//            }

//            if (item.toolTip.Length > 0)
//            {
//                AddLabel(item.toolTip, new Color(207, 254, 255));
//            }
//            if (item.toolTip2.Length > 0)
//            {
//                AddLabel(item.toolTip2, new Color(207, 254, 255));
//            }

//            if (item.scale != itemDefaults.scale)
//            {
//                double num16 = (double)(item.scale - itemDefaults.scale);
//                num16 = num16 / (double)itemDefaults.scale * 100.0;
//                num16 = Math.Round(num16);
//                if (num16 > 0.0)
//                {
//                    AddLabel("+" + num16 + Lang.tip[43], statUpColor);
//                }
//                else
//                {
//                    AddLabel(num16 + Lang.tip[43], Color.Red);
//                }
//            }

//            if (item.buffTime > 0)
//            {
//                string duration = "";
//                if (item.buffTime / 60 >= 60)
//                {
//                    duration = Math.Round((double)(item.buffTime / 60) / 60.0) + " Min.";
//                }
//                else
//                {
//                    duration = Math.Round((double)item.buffTime / 60.0) + " Sec.";
//                }
//                AddStat("Duration", duration);
//            }

//            if (item.accessory && item.prefix > 0)
//            {
//                AddLabel(GetAccessoryStat(item), statUpColor);
//            }

//            if (item.value > 0)
//            {
//                AddMoneyStat(item.value / 5 * item.stack);
//            }
//        }

//        private void BuildItemDropStats(Item item)
//        {
//            List<string> name = new List<string>();
//            List<float> percent = new List<float>();
//            List<NPCDropTable> dropTables = DropTableBuilder.DropTable.NPCDropTables;
//            for(int i= 0; i < dropTables.Count; i++)
//            {
//                NPCDropTable dropTable = dropTables[i];
//                for(int j = 0; j < dropTable.Drops.Count; j++)
//                {
//                    ItemDropInfo itemDrop = dropTable.Drops[j];
//                    if(itemDrop.NPCType == item.netID)
//                    {
//                        name.Add(Lang.npcName(dropTable.NPCType));
//                        float percentage = (float)(Math.Round(itemDrop.Percent * 100, 2));
//                        percent.Add(percentage);
//                    }
//                }
//            }

//			// TODO Bring this feature back?
//            //List<ChestDropInfo> chests = ChestDropBuilder.chestDropsInfo;
//            //for (int i = 0; i < chests.Count; i++)
//            //{
//            //    ChestDropInfo chest = chests[i];
//            //    for(int j = 0; j < chest.ItemDrops.Count; j++)
//            //    {
//            //        ItemDropInfo itemDrop = chest.ItemDrops[j];
//            //        if(itemDrop.NPCType == item.netID)
//            //        {
//            //            name.Add(chest.ToString());
//            //            float percentage = (float)(Math.Round(itemDrop.Percent * 100, 2));
//            //            percent.Add(percentage);
//            //        }
//            //    }
//            //}

//            for (int i = 0; i < name.Count; i++)
//            {
//                AddStat(name[i], "%" + percent[i]);
//            }
//        }

//        private void AddLabel(string text, Color? color = null, float scale = .4f)
//        {
//            UILabel label = new UILabel(text);
//            label.Scale = scale;
//            label.X = Spacing;
//            label.Y = yPos;
//            yPos += label.Height;
//            if(label.Width > maxWidth)
//            {
//                maxWidth = label.Width;
//            }
//            label.ForegroundColor = Color.White;
//            if(color != null)
//            {
//                Color c = (Color) color;
//                label.ForegroundColor = c;
//            }
//            AddChild(label);
//        }

//        private void AddStat(string text, string stat, Color? color = null, float scale = .4f)
//        {
//            UILabel label = new UILabel(text);
//            label.Scale = scale;
//            label.X = Spacing;
//            label.Y = yPos;
//            label.ForegroundColor = Color.White;
//            AddChild(label);

//            UILabel lStat = new UILabel(stat);
//            lStat.Scale = scale;
//            lStat.X = Width;
//            lStat.Y = yPos;
//            lStat.ForegroundColor = Color.White;
//            if (color != null)
//            {
//                Color c = (Color)color;
//                label.ForegroundColor = c;
//                lStat.ForegroundColor = c;
//            }
//            AddChild(lStat);
//            stats.Add(lStat);

//            yPos += label.Height;

//            if (label.Width + 20 + lStat.Width > maxWidth)
//            {
//                maxWidth = label.Width + 20 + lStat.Width;
//            }
//        }

//        private void AddCompareStat(string text, float stat, float baseStat, string prefix = "", bool highIsGood = true, bool percentDif = false, float scale = .4f)
//        {
//            if(stat == baseStat)
//            {
//                AddStat(text, prefix + stat);
//                return;
//            }
//            UILabel label = new UILabel(text);
//            label.Scale = scale;
//            label.X = Spacing;
//            label.Y = yPos;
//            label.ForegroundColor = Color.White;
//            AddChild(label);

//            CompareStat cmpStat = new CompareStat(stat, baseStat, prefix, highIsGood, percentDif, scale);
//            cmpStat.Y = yPos;
//            cmpStat.X = Width;
//            AddChild(cmpStat);
//            stats.Add(cmpStat);

//            yPos += label.Height;

//            if (label.Width + 20 + cmpStat.Width > maxWidth)
//            {
//                maxWidth = label.Width + 20 + cmpStat.Width;
//            }

//        }

//        private void AddMoneyStat(int value, float scale = .4f)
//        {
//            UILabel label = new UILabel("Sell Value");
//            label.Scale = scale;
//            label.X = Spacing;
//            label.Y = yPos;
//            AddChild(label);

//            MoneyView moneyView = new MoneyView(value);
//            moneyView.Y = yPos;
//            moneyView.X = Width;
//            AddChild(moneyView);
//            stats.Add(moneyView);

//            yPos += label.Height;

//            if (label.Width + 20 + moneyView.Width > maxWidth)
//            {
//                maxWidth = label.Width + 20 + moneyView.Width;
//            }

//        }

//        static string[] useSpeeds = new string[]
//        {
//			"Insanely Fast",
//			"Very Fast",
//			"Fast",
//			"Average",
//			"Slow",
//			"Very Slow",
//			"Extremely Slow",
//			"Snail"
//        };
//        public static string GetSpeed(int useSpeed)
//        {
//            if (useSpeed <= 8)
//            {
//                return useSpeeds[0];
//            }
//            else if (useSpeed <= 20)
//            {
//                return useSpeeds[1];
//            }
//            else if (useSpeed <= 25)
//            {
//                return useSpeeds[2];
//            }
//            else if (useSpeed <= 30)
//            {
//                return useSpeeds[3];
//            }
//            else if (useSpeed <= 35)
//            {
//                return useSpeeds[4];
//            }
//            else if (useSpeed <= 45)
//            {
//                return useSpeeds[5];
//            }
//            else if (useSpeed <= 55)
//            {
//                return useSpeeds[6];
//            }
//            else
//            {
//                return useSpeeds[7];
//            }
//        }

//        static string[] knockbackPowers = new string[]
//        {
//			"None",
//			"Extremely Weak",
//			"Very Weak",
//			"Weak",
//			"Average",
//			"Strong",
//			"Very Strong",
//			"Extremely Strong",
//			"Insane"
//        };

//        public static string GetKnockback(float knockback)
//        {
//            if (knockback == 0f)
//            {
//                return knockbackPowers[0];
//            }
//            else if ((double)knockback <= 1.5)
//            {
//                return knockbackPowers[1];
//            }
//            else if (knockback <= 3f)
//            {
//                return knockbackPowers[2];
//            }
//            else if (knockback <= 4f)
//            {
//                return knockbackPowers[3];
//            }
//            else if (knockback <= 6f)
//            {
//                return knockbackPowers[4];
//            }
//            else if (knockback <= 7f)
//            {
//                return knockbackPowers[5];
//            }
//            else if (knockback <= 9f)
//            {
//                return knockbackPowers[6];
//            }
//            else if (knockback <= 11f)
//            {
//                return knockbackPowers[7];
//            }
//            else
//            {
//                return knockbackPowers[8];
//            }
//        }

//        public static string GetAccessoryStat (Item item)
//        {
//            if (!item.accessory || item.prefix <= 0)
//                return "";

//            if (item.prefix == 62)
//            {
//                return "+1" + Lang.tip[25];
//            }
//            if (item.prefix == 63)
//            {
//                return "+2" + Lang.tip[25];
//            }
//            if (item.prefix == 64)
//            {
//                return "+3" + Lang.tip[25];
//            }
//            if (item.prefix == 65)
//            {
//                return "+4" + Lang.tip[25];
//            }
//            if (item.prefix == 66)
//            {
//                return "+20 " + Lang.tip[31];
//            }
//            if (item.prefix == 67)
//            {
//                return "+2" + Lang.tip[5];
//            }
//            if (item.prefix == 68)
//            {
//                return "+4" + Lang.tip[5];
//            }
//            if (item.prefix == 69)
//            {
//                return "+1" + Lang.tip[39];
//            }
//            if (item.prefix == 70)
//            {
//                return "+2" + Lang.tip[39];
//            }
//            if (item.prefix == 71)
//            {
//                return "+3" + Lang.tip[39];
//            }
//            if (item.prefix == 72)
//            {
//                return "+4" + Lang.tip[39];
//            }
//            if (item.prefix == 73)
//            {
//                return "+1" + Lang.tip[46];
//            }
//            if (item.prefix == 74)
//            {
//                return "+2" + Lang.tip[46];
//            }
//            if (item.prefix == 75)
//            {
//                return "+3" + Lang.tip[46];
//            }
//            if (item.prefix == 76)
//            {
//                return "+4" + Lang.tip[46];
//            }
//            if (item.prefix == 77)
//            {
//                return "+1" + Lang.tip[47];
//            }
//            if (item.prefix == 78)
//            {
//                return "+2" + Lang.tip[47];
//            }
//            if (item.prefix == 79)
//            {
//                return "+3" + Lang.tip[47];
//            }
//            if (item.prefix == 80)
//            {
//                return "+4" + Lang.tip[47];
//            }

//            return "";
//        }
//    }

//    class CompareStat : UIView
//    {
//        UILabel lBaseStat;
//        UILabel lDifferenceStat;
//        UILabel cap;

//        public CompareStat(float stat, float baseStat, string prefix = "", bool highIsGood = true, bool percentDif = false, float scale = .4f)
//        {
//            float difference = stat - baseStat;
//            difference = (float)Math.Round(difference, 3);
//            if(percentDif)
//            {
//                difference = ((stat / baseStat) - 1) * 100;
//            }

//            lBaseStat = new UILabel(")" + prefix + baseStat);
//            string percentPrefix = string.Empty;
//            if (percentDif) percentPrefix = "%";
//            lDifferenceStat = new UILabel(prefix + percentPrefix + difference);
//            cap = new UILabel("(");

//            lBaseStat.Scale = scale;
//            lDifferenceStat.Scale = scale;
//            cap.Scale = scale;

//            if(difference > 0)
//            {
//                lDifferenceStat.Text = "+" + prefix + difference;
//            }

//            lDifferenceStat.X = cap.X + cap.Width;
//            lBaseStat.X = lDifferenceStat.X + lDifferenceStat.Width;
//            //lDifferenceStat.X = lBaseStat.X + lBaseStat.Width;
//            //cap.X = lDifferenceStat.X + lDifferenceStat.Width;

//            this.Width = lBaseStat.X + lBaseStat.Width;
//            this.Height = lBaseStat.Height;

//            if((difference > 0 && highIsGood) || (difference < 0 && !highIsGood))
//            {
//                lDifferenceStat.ForegroundColor = ItemTooltip.statUpColor;
//            }
//            else
//            {
//                lDifferenceStat.ForegroundColor = Color.Red;
//            }

//            AddChild(lBaseStat);
//            AddChild(lDifferenceStat);
//            AddChild(cap);

//        }

//    }
//}