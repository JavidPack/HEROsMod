using System.Collections.Generic;
using System.Linq;

using Terraria;
using Terraria.Utilities;

namespace HEROsMod.HEROsModServices
{
	internal class PrefixScraper
	{
		//  static List<PrefixGroup> prefixGroups;

		//  public static void Scrape()
		//  {
		//      #region prefixStr
		//      string prefixStr =
		//                  @"if (this.type == 1 || this.type == 4 || this.type == 6 || this.type == 7 || this.type == 10 || this.type == 24 || this.type == 45 || this.type == 46 || this.type == 65 || this.type == 103 || this.type == 104 || this.type == 121 || this.type == 122 || this.type == 155 || this.type == 190 || this.type == 196 || this.type == 198 || this.type == 199 || this.type == 200 || this.type == 201 || this.type == 202 || this.type == 203 || this.type == 204 || this.type == 213 || this.type == 217 || this.type == 273 || this.type == 367 || this.type == 368 || this.type == 426 || this.type == 482 || this.type == 483 || this.type == 484 || this.type == 653 || this.type == 654 || this.type == 656 || this.type == 657 || this.type == 659 || this.type == 660 || this.type == 671 || this.type == 672 || this.type == 674 || this.type == 675 || this.type == 676 || this.type == 723 || this.type == 724 || this.type == 757 || this.type == 776 || this.type == 777 || this.type == 778 || this.type == 787 || this.type == 795 || this.type == 797 || this.type == 798 || this.type == 799 || this.type == 881 || this.type == 882 || this.type == 921 || this.type == 922 || this.type == 989 || this.type == 990 || this.type == 991 || this.type == 992 || this.type == 993 || this.type == 1123 || this.type == 1166 || this.type == 1185 || this.type == 1188 || this.type == 1192 || this.type == 1195 || this.type == 1199 || this.type == 1202 || this.type == 1222 || this.type == 1223 || this.type == 1224 || this.type == 1226 || this.type == 1227 || this.type == 1230 || this.type == 1233 || this.type == 1234 || this.type == 1294 || this.type == 1304 || this.type == 1305 || this.type == 1306 || this.type == 1320 || this.type == 1327 || this.type == 1506 || this.type == 1507 || this.type == 1786 || this.type == 1826 || this.type == 1827 || this.type == 1909 || this.type == 1917 || this.type == 1928 || this.type == 2176 || this.type == 2273 || this.type == 2608 || this.type == 2341 || this.type == 2330 || this.type == 2320 || this.type == 2516 || this.type == 2517 || this.type == 2746 || this.type == 2745)
		//{
		//	int num9 = Main.rand.Next(40);
		//	if (num9 == 0)
		//	{
		//		num = 1;
		//	}
		//	if (num9 == 1)
		//	{
		//		num = 2;
		//	}
		//	if (num9 == 2)
		//	{
		//		num = 3;
		//	}
		//	if (num9 == 3)
		//	{
		//		num = 4;
		//	}
		//	if (num9 == 4)
		//	{
		//		num = 5;
		//	}
		//	if (num9 == 5)
		//	{
		//		num = 6;
		//	}
		//	if (num9 == 6)
		//	{
		//		num = 7;
		//	}
		//	if (num9 == 7)
		//	{
		//		num = 8;
		//	}
		//	if (num9 == 8)
		//	{
		//		num = 9;
		//	}
		//	if (num9 == 9)
		//	{
		//		num = 10;
		//	}
		//	if (num9 == 10)
		//	{
		//		num = 11;
		//	}
		//	if (num9 == 11)
		//	{
		//		num = 12;
		//	}
		//	if (num9 == 12)
		//	{
		//		num = 13;
		//	}
		//	if (num9 == 13)
		//	{
		//		num = 14;
		//	}
		//	if (num9 == 14)
		//	{
		//		num = 15;
		//	}
		//	if (num9 == 15)
		//	{
		//		num = 36;
		//	}
		//	if (num9 == 16)
		//	{
		//		num = 37;
		//	}
		//	if (num9 == 17)
		//	{
		//		num = 38;
		//	}
		//	if (num9 == 18)
		//	{
		//		num = 53;
		//	}
		//	if (num9 == 19)
		//	{
		//		num = 54;
		//	}
		//	if (num9 == 20)
		//	{
		//		num = 55;
		//	}
		//	if (num9 == 21)
		//	{
		//		num = 39;
		//	}
		//	if (num9 == 22)
		//	{
		//		num = 40;
		//	}
		//	if (num9 == 23)
		//	{
		//		num = 56;
		//	}
		//	if (num9 == 24)
		//	{
		//		num = 41;
		//	}
		//	if (num9 == 25)
		//	{
		//		num = 57;
		//	}
		//	if (num9 == 26)
		//	{
		//		num = 42;
		//	}
		//	if (num9 == 27)
		//	{
		//		num = 43;
		//	}
		//	if (num9 == 28)
		//	{
		//		num = 44;
		//	}
		//	if (num9 == 29)
		//	{
		//		num = 45;
		//	}
		//	if (num9 == 30)
		//	{
		//		num = 46;
		//	}
		//	if (num9 == 31)
		//	{
		//		num = 47;
		//	}
		//	if (num9 == 32)
		//	{
		//		num = 48;
		//	}
		//	if (num9 == 33)
		//	{
		//		num = 49;
		//	}
		//	if (num9 == 34)
		//	{
		//		num = 50;
		//	}
		//	if (num9 == 35)
		//	{
		//		num = 51;
		//	}
		//	if (num9 == 36)
		//	{
		//		num = 59;
		//	}
		//	if (num9 == 37)
		//	{
		//		num = 60;
		//	}
		//	if (num9 == 38)
		//	{
		//		num = 61;
		//	}
		//	if (num9 == 39)
		//	{
		//		num = 81;
		//	}
		//}
		//else if (this.type == 162 || this.type == 160 || this.type == 163 || this.type == 220 || this.type == 274 || this.type == 277 || this.type == 280 || this.type == 383 || this.type == 384 || this.type == 385 || this.type == 386 || this.type == 387 || this.type == 388 || this.type == 389 || this.type == 390 || this.type == 406 || this.type == 537 || this.type == 550 || this.type == 579 || this.type == 756 || this.type == 759 || this.type == 801 || this.type == 802 || this.type == 1186 || this.type == 1189 || this.type == 1190 || this.type == 1193 || this.type == 1196 || this.type == 1197 || this.type == 1200 || this.type == 1203 || this.type == 1204 || this.type == 1228 || this.type == 1231 || this.type == 1232 || this.type == 1259 || this.type == 1262 || this.type == 1297 || this.type == 1314 || this.type == 1325 || this.type == 1947 || this.type == 2332 || this.type == 2331 || this.type == 2342 || this.type == 2424 || this.type == 2611 || this.type == 2798)
		//{
		//	int num9 = Main.rand.Next(14);
		//	if (num9 == 0)
		//	{
		//		num = 36;
		//	}
		//	if (num9 == 1)
		//	{
		//		num = 37;
		//	}
		//	if (num9 == 2)
		//	{
		//		num = 38;
		//	}
		//	if (num9 == 3)
		//	{
		//		num = 53;
		//	}
		//	if (num9 == 4)
		//	{
		//		num = 54;
		//	}
		//	if (num9 == 5)
		//	{
		//		num = 55;
		//	}
		//	if (num9 == 6)
		//	{
		//		num = 39;
		//	}
		//	if (num9 == 7)
		//	{
		//		num = 40;
		//	}
		//	if (num9 == 8)
		//	{
		//		num = 56;
		//	}
		//	if (num9 == 9)
		//	{
		//		num = 41;
		//	}
		//	if (num9 == 10)
		//	{
		//		num = 57;
		//	}
		//	if (num9 == 11)
		//	{
		//		num = 59;
		//	}
		//	if (num9 == 12)
		//	{
		//		num = 60;
		//	}
		//	if (num9 == 13)
		//	{
		//		num = 61;
		//	}
		//}
		//else if (this.type == 39 || this.type == 44 || this.type == 95 || this.type == 96 || this.type == 98 || this.type == 99 || this.type == 120 || this.type == 164 || this.type == 197 || this.type == 219 || this.type == 266 || this.type == 281 || this.type == 434 || this.type == 435 || this.type == 436 || this.type == 481 || this.type == 506 || this.type == 533 || this.type == 534 || this.type == 578 || this.type == 655 || this.type == 658 || this.type == 661 || this.type == 679 || this.type == 682 || this.type == 725 || this.type == 758 || this.type == 759 || this.type == 760 || this.type == 796 || this.type == 800 || this.type == 905 || this.type == 923 || this.type == 964 || this.type == 986 || this.type == 1156 || this.type == 1187 || this.type == 1194 || this.type == 1201 || this.type == 1229 || this.type == 1254 || this.type == 1255 || this.type == 1258 || this.type == 1265 || this.type == 1319 || this.type == 1553 || this.type == 1782 || this.type == 1784 || this.type == 1835 || this.type == 1870 || this.type == 1910 || this.type == 1929 || this.type == 1946 || this.type == 2223 || this.type == 2269 || this.type == 2270 || this.type == 2624 || this.type == 2515 || this.type == 2747 || this.type == 2796 || this.type == 2797)
		//{
		//	int num9 = Main.rand.Next(36);
		//	if (num9 == 0)
		//	{
		//		num = 16;
		//	}
		//	if (num9 == 1)
		//	{
		//		num = 17;
		//	}
		//	if (num9 == 2)
		//	{
		//		num = 18;
		//	}
		//	if (num9 == 3)
		//	{
		//		num = 19;
		//	}
		//	if (num9 == 4)
		//	{
		//		num = 20;
		//	}
		//	if (num9 == 5)
		//	{
		//		num = 21;
		//	}
		//	if (num9 == 6)
		//	{
		//		num = 22;
		//	}
		//	if (num9 == 7)
		//	{
		//		num = 23;
		//	}
		//	if (num9 == 8)
		//	{
		//		num = 24;
		//	}
		//	if (num9 == 9)
		//	{
		//		num = 25;
		//	}
		//	if (num9 == 10)
		//	{
		//		num = 58;
		//	}
		//	if (num9 == 11)
		//	{
		//		num = 36;
		//	}
		//	if (num9 == 12)
		//	{
		//		num = 37;
		//	}
		//	if (num9 == 13)
		//	{
		//		num = 38;
		//	}
		//	if (num9 == 14)
		//	{
		//		num = 53;
		//	}
		//	if (num9 == 15)
		//	{
		//		num = 54;
		//	}
		//	if (num9 == 16)
		//	{
		//		num = 55;
		//	}
		//	if (num9 == 17)
		//	{
		//		num = 39;
		//	}
		//	if (num9 == 18)
		//	{
		//		num = 40;
		//	}
		//	if (num9 == 19)
		//	{
		//		num = 56;
		//	}
		//	if (num9 == 20)
		//	{
		//		num = 41;
		//	}
		//	if (num9 == 21)
		//	{
		//		num = 57;
		//	}
		//	if (num9 == 22)
		//	{
		//		num = 42;
		//	}
		//	if (num9 == 23)
		//	{
		//		num = 43;
		//	}
		//	if (num9 == 24)
		//	{
		//		num = 44;
		//	}
		//	if (num9 == 25)
		//	{
		//		num = 45;
		//	}
		//	if (num9 == 26)
		//	{
		//		num = 46;
		//	}
		//	if (num9 == 27)
		//	{
		//		num = 47;
		//	}
		//	if (num9 == 28)
		//	{
		//		num = 48;
		//	}
		//	if (num9 == 29)
		//	{
		//		num = 49;
		//	}
		//	if (num9 == 30)
		//	{
		//		num = 50;
		//	}
		//	if (num9 == 31)
		//	{
		//		num = 51;
		//	}
		//	if (num9 == 32)
		//	{
		//		num = 59;
		//	}
		//	if (num9 == 33)
		//	{
		//		num = 60;
		//	}
		//	if (num9 == 34)
		//	{
		//		num = 61;
		//	}
		//	if (num9 == 35)
		//	{
		//		num = 82;
		//	}
		//}
		//else if (this.type == 64 || this.type == 112 || this.type == 113 || this.type == 127 || this.type == 157 || this.type == 165 || this.type == 218 || this.type == 272 || this.type == 494 || this.type == 495 || this.type == 496 || this.type == 514 || this.type == 517 || this.type == 518 || this.type == 519 || this.type == 683 || this.type == 726 || this.type == 739 || this.type == 740 || this.type == 741 || this.type == 742 || this.type == 743 || this.type == 744 || this.type == 788 || this.type == 1121 || this.type == 1155 || this.type == 1157 || this.type == 1178 || this.type == 1244 || this.type == 1256 || this.type == 1260 || this.type == 1264 || this.type == 1266 || this.type == 1295 || this.type == 1296 || this.type == 1308 || this.type == 1309 || this.type == 1313 || this.type == 1336 || this.type == 1444 || this.type == 1445 || this.type == 1446 || this.type == 1572 || this.type == 1801 || this.type == 1802 || this.type == 1930 || this.type == 1931 || this.type == 2188 || this.type == 2622 || this.type == 2621 || this.type == 2584 || this.type == 2551 || this.type == 2366 || this.type == 2535 || this.type == 2365 || this.type == 2364 || this.type == 2623 || this.type == 2750 || this.type == 2795)
		//{
		//	int num9 = Main.rand.Next(36);
		//	if (num9 == 0)
		//	{
		//		num = 26;
		//	}
		//	if (num9 == 1)
		//	{
		//		num = 27;
		//	}
		//	if (num9 == 2)
		//	{
		//		num = 28;
		//	}
		//	if (num9 == 3)
		//	{
		//		num = 29;
		//	}
		//	if (num9 == 4)
		//	{
		//		num = 30;
		//	}
		//	if (num9 == 5)
		//	{
		//		num = 31;
		//	}
		//	if (num9 == 6)
		//	{
		//		num = 32;
		//	}
		//	if (num9 == 7)
		//	{
		//		num = 33;
		//	}
		//	if (num9 == 8)
		//	{
		//		num = 34;
		//	}
		//	if (num9 == 9)
		//	{
		//		num = 35;
		//	}
		//	if (num9 == 10)
		//	{
		//		num = 52;
		//	}
		//	if (num9 == 11)
		//	{
		//		num = 36;
		//	}
		//	if (num9 == 12)
		//	{
		//		num = 37;
		//	}
		//	if (num9 == 13)
		//	{
		//		num = 38;
		//	}
		//	if (num9 == 14)
		//	{
		//		num = 53;
		//	}
		//	if (num9 == 15)
		//	{
		//		num = 54;
		//	}
		//	if (num9 == 16)
		//	{
		//		num = 55;
		//	}
		//	if (num9 == 17)
		//	{
		//		num = 39;
		//	}
		//	if (num9 == 18)
		//	{
		//		num = 40;
		//	}
		//	if (num9 == 19)
		//	{
		//		num = 56;
		//	}
		//	if (num9 == 20)
		//	{
		//		num = 41;
		//	}
		//	if (num9 == 21)
		//	{
		//		num = 57;
		//	}
		//	if (num9 == 22)
		//	{
		//		num = 42;
		//	}
		//	if (num9 == 23)
		//	{
		//		num = 43;
		//	}
		//	if (num9 == 24)
		//	{
		//		num = 44;
		//	}
		//	if (num9 == 25)
		//	{
		//		num = 45;
		//	}
		//	if (num9 == 26)
		//	{
		//		num = 46;
		//	}
		//	if (num9 == 27)
		//	{
		//		num = 47;
		//	}
		//	if (num9 == 28)
		//	{
		//		num = 48;
		//	}
		//	if (num9 == 29)
		//	{
		//		num = 49;
		//	}
		//	if (num9 == 30)
		//	{
		//		num = 50;
		//	}
		//	if (num9 == 31)
		//	{
		//		num = 51;
		//	}
		//	if (num9 == 32)
		//	{
		//		num = 59;
		//	}
		//	if (num9 == 33)
		//	{
		//		num = 60;
		//	}
		//	if (num9 == 34)
		//	{
		//		num = 61;
		//	}
		//	if (num9 == 35)
		//	{
		//		num = 83;
		//	}
		//}
		//else if (this.type == 55 || this.type == 119 || this.type == 191 || this.type == 284 || this.type == 670 || this.type == 1122 || this.type == 1513 || this.type == 1569 || this.type == 1571 || this.type == 1825 || this.type == 1918)
		//{
		//	int num9 = Main.rand.Next(14);
		//	if (num9 == 0)
		//	{
		//		num = 36;
		//	}
		//	if (num9 == 1)
		//	{
		//		num = 37;
		//	}
		//	if (num9 == 2)
		//	{
		//		num = 38;
		//	}
		//	if (num9 == 3)
		//	{
		//		num = 53;
		//	}
		//	if (num9 == 4)
		//	{
		//		num = 54;
		//	}
		//	if (num9 == 5)
		//	{
		//		num = 55;
		//	}
		//	if (num9 == 6)
		//	{
		//		num = 39;
		//	}
		//	if (num9 == 7)
		//	{
		//		num = 40;
		//	}
		//	if (num9 == 8)
		//	{
		//		num = 56;
		//	}
		//	if (num9 == 9)
		//	{
		//		num = 41;
		//	}
		//	if (num9 == 10)
		//	{
		//		num = 57;
		//	}
		//	if (num9 == 11)
		//	{
		//		num = 59;
		//	}
		//	if (num9 == 12)
		//	{
		//		num = 60;
		//	}
		//	if (num9 == 13)
		//	{
		//		num = 61;
		//	}
		//}
		//else
		//{
		//	if (!this.accessory || this.type == 267 || this.type == 562 || this.type == 563 || this.type == 564 || this.type == 565 || this.type == 566 || this.type == 567 || this.type == 568 || this.type == 569 || this.type == 570 || this.type == 571 || this.type == 572 || this.type == 573 || this.type == 574 || this.type == 576 || this.type == 1307 || (this.type >= 1596 && this.type < 1610) || this.vanity)
		//	{
		//		result = false;
		//		return result;
		//	}
		//	num = Main.rand.Next(62, 81);
		//}";
		//      #endregion

		//      prefixGroups = new List<PrefixGroup>();

		//      string pattern = @"(type == \d+)|(num = \d+)";
		//      MatchCollection matches = Regex.Matches(prefixStr, pattern);

		//      bool lastMatchWasPrefix = true;
		//      PrefixGroup currentGroup = null;
		//      foreach (Match match in matches)
		//      {
		//          foreach (Capture capture in match.Captures)
		//          {
		//              //Console.WriteLine(capture.Value);
		//              pattern = @"type == \d+";
		//              Match m = Regex.Match(capture.Value, pattern);
		//              if (m.Success)
		//              {
		//                  if (lastMatchWasPrefix)
		//                  {
		//                      currentGroup = new PrefixGroup();
		//                      prefixGroups.Add(currentGroup);
		//                  }
		//                  lastMatchWasPrefix = false;
		//                  string[] parts = capture.Value.Split(' ');
		//                  int itemType = int.Parse(parts[2]);
		//                  currentGroup.Items.Add(itemType);
		//              }
		//              else
		//              {
		//                  lastMatchWasPrefix = true;

		//                  string[] parts = capture.Value.Split(' ');
		//                  int prefixType = int.Parse(parts[2]);
		//                  currentGroup.Prefixes.Add(prefixType);
		//              }
		//          }
		//      }

		//      PrefixGroup lastGroup = prefixGroups[prefixGroups.Count - 1];
		//      lastGroup.Accesories = true;

		//      for(int i = 62; i <= 81; i++)
		//      {
		//          lastGroup.Prefixes.Add(i);
		//      }
		//  }

		//public static int[] GetPrefixesForItem(Item item)
		//{
		//    for(int i = 0;i < prefixGroups.Count; i++)
		//    {
		//        if(prefixGroups[i].ItemInGroup(item))
		//        {
		//            return prefixGroups[i].Prefixes.ToArray();
		//        }
		//    }
		//    return new int[0];
		//}

		// TODO!! Fix prefixes for new items.
		public static bool IsValidPrefix(int pre, Item item)
		{
			if (pre == 0 || item.type == 0)
			{
				return false;
			}
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom();
			}
			if (item.type == 1 || item.type == 4 || item.type == 6 || item.type == 7 || item.type == 10 || item.type == 24 || item.type == 45 || item.type == 46 || item.type == 65 || item.type == 103 || item.type == 104 || item.type == 121 || item.type == 122 || item.type == 155 || item.type == 190 || item.type == 196 || item.type == 198 || item.type == 199 || item.type == 200 || item.type == 201 || item.type == 202 || item.type == 203 || item.type == 204 || item.type == 213 || item.type == 217 || item.type == 273 || item.type == 367 || item.type == 368 || item.type == 426 || item.type == 482 || item.type == 483 || item.type == 484 || item.type == 653 || item.type == 654 || item.type == 656 || item.type == 657 || item.type == 659 || item.type == 660 || item.type == 671 || item.type == 672 || item.type == 674 || item.type == 675 || item.type == 676 || item.type == 723 || item.type == 724 || item.type == 757 || item.type == 776 || item.type == 777 || item.type == 778 || item.type == 787 || item.type == 795 || item.type == 797 || item.type == 798 || item.type == 799 || item.type == 881 || item.type == 882 || item.type == 921 || item.type == 922 || item.type == 989 || item.type == 990 || item.type == 991 || item.type == 992 || item.type == 993 || item.type == 1123 || item.type == 1166 || item.type == 1185 || item.type == 1188 || item.type == 1192 || item.type == 1195 || item.type == 1199 || item.type == 1202 || item.type == 1222 || item.type == 1223 || item.type == 1224 || item.type == 1226 || item.type == 1227 || item.type == 1230 || item.type == 1233 || item.type == 1234 || item.type == 1294 || item.type == 1304 || item.type == 1305 || item.type == 1306 || item.type == 1320 || item.type == 1327 || item.type == 1506 || item.type == 1507 || item.type == 1786 || item.type == 1826 || item.type == 1827 || item.type == 1909 || item.type == 1917 || item.type == 1928 || item.type == 2176 || item.type == 2273 || item.type == 2608 || item.type == 2341 || item.type == 2330 || item.type == 2320 || item.type == 2516 || item.type == 2517 || item.type == 2746 || item.type == 2745 || item.type == 3063 || item.type == 3018 || item.type == 3211 || item.type == 3013 || item.type == 3258 || item.type == 3106 || item.type == 3065 || item.type == 2880 || item.type == 3481 || item.type == 3482 || item.type == 3483 || item.type == 3484 || item.type == 3485 || item.type == 3487 || item.type == 3488 || item.type == 3489 || item.type == 3490 || item.type == 3491 || item.type == 3493 || item.type == 3494 || item.type == 3495 || item.type == 3496 || item.type == 3497 || item.type == 3498 || item.type == 3500 || item.type == 3501 || item.type == 3502 || item.type == 3503 || item.type == 3504 || item.type == 3505 || item.type == 3506 || item.type == 3507 || item.type == 3508 || item.type == 3509 || item.type == 3511 || item.type == 3512 || item.type == 3513 || item.type == 3514 || item.type == 3515 || item.type == 3517 || item.type == 3518 || item.type == 3519 || item.type == 3520 || item.type == 3521 || item.type == 3522 || item.type == 3523 || item.type == 3524 || item.type == 3525 || (item.type >= 3462 && item.type <= 3466) || (item.type >= 2772 && item.type <= 2786) || item.type == 3349 || item.type == 3352 || item.type == 3351 || MeleePrefix(item))
			{
				int[] valid = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 59, 60, 61, 81 };
				return valid.Contains(pre);
			}
			else if (item.type == 162 || item.type == 160 || item.type == 163 || item.type == 220 || item.type == 274 || item.type == 277 || item.type == 280 || item.type == 383 || item.type == 384 || item.type == 385 || item.type == 386 || item.type == 387 || item.type == 388 || item.type == 389 || item.type == 390 || item.type == 406 || item.type == 537 || item.type == 550 || item.type == 579 || item.type == 756 || item.type == 759 || item.type == 801 || item.type == 802 || item.type == 1186 || item.type == 1189 || item.type == 1190 || item.type == 1193 || item.type == 1196 || item.type == 1197 || item.type == 1200 || item.type == 1203 || item.type == 1204 || item.type == 1228 || item.type == 1231 || item.type == 1232 || item.type == 1259 || item.type == 1262 || item.type == 1297 || item.type == 1314 || item.type == 1325 || item.type == 1947 || item.type == 2332 || item.type == 2331 || item.type == 2342 || item.type == 2424 || item.type == 2611 || item.type == 2798 || item.type == 3012 || item.type == 3473 || item.type == 3098 || item.type == 3368 || WeaponPrefix(item))
			{
				int[] valid = new int[] { 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 59, 60, 61 };
				return valid.Contains(pre);
			}
			else if (item.type == 39 || item.type == 44 || item.type == 95 || item.type == 96 || item.type == 98 || item.type == 99 || item.type == 120 || item.type == 164 || item.type == 197 || item.type == 219 || item.type == 266 || item.type == 281 || item.type == 434 || item.type == 435 || item.type == 436 || item.type == 481 || item.type == 506 || item.type == 533 || item.type == 534 || item.type == 578 || item.type == 655 || item.type == 658 || item.type == 661 || item.type == 679 || item.type == 682 || item.type == 725 || item.type == 758 || item.type == 759 || item.type == 760 || item.type == 796 || item.type == 800 || item.type == 905 || item.type == 923 || item.type == 964 || item.type == 986 || item.type == 1156 || item.type == 1187 || item.type == 1194 || item.type == 1201 || item.type == 1229 || item.type == 1254 || item.type == 1255 || item.type == 1258 || item.type == 1265 || item.type == 1319 || item.type == 1553 || item.type == 1782 || item.type == 1784 || item.type == 1835 || item.type == 1870 || item.type == 1910 || item.type == 1929 || item.type == 1946 || item.type == 2223 || item.type == 2269 || item.type == 2270 || item.type == 2624 || item.type == 2515 || item.type == 2747 || item.type == 2796 || item.type == 2797 || item.type == 3052 || item.type == 2888 || item.type == 3019 || item.type == 3029 || item.type == 3007 || item.type == 3008 || item.type == 3210 || item.type == 3107 || item.type == 3245 || item.type == 3475 || item.type == 3540 || item.type == 3480 || item.type == 3486 || item.type == 3492 || item.type == 3498 || item.type == 3504 || item.type == 3510 || item.type == 3516 || item.type == 3350 || item.type == 3546 || RangedPrefix(item))
			{
				int[] valid = new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 58, 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 59, 60, 61, 82 };
				return valid.Contains(pre);
			}
			else if (item.type == 64 || item.type == 112 || item.type == 113 || item.type == 127 || item.type == 157 || item.type == 165 || item.type == 218 || item.type == 272 || item.type == 494 || item.type == 495 || item.type == 496 || item.type == 514 || item.type == 517 || item.type == 518 || item.type == 519 || item.type == 683 || item.type == 726 || item.type == 739 || item.type == 740 || item.type == 741 || item.type == 742 || item.type == 743 || item.type == 744 || item.type == 788 || item.type == 1121 || item.type == 1155 || item.type == 1157 || item.type == 1178 || item.type == 1244 || item.type == 1256 || item.type == 1260 || item.type == 1264 || item.type == 1266 || item.type == 1295 || item.type == 1296 || item.type == 1308 || item.type == 1309 || item.type == 1313 || item.type == 1336 || item.type == 1444 || item.type == 1445 || item.type == 1446 || item.type == 1572 || item.type == 1801 || item.type == 1802 || item.type == 1930 || item.type == 1931 || item.type == 2188 || item.type == 2622 || item.type == 2621 || item.type == 2584 || item.type == 2551 || item.type == 2366 || item.type == 2535 || item.type == 2365 || item.type == 2364 || item.type == 2623 || item.type == 2750 || item.type == 2795 || item.type == 3053 || item.type == 3051 || item.type == 3209 || item.type == 3014 || item.type == 3105 || item.type == 2882 || item.type == 3269 || item.type == 3006 || item.type == 3377 || item.type == 3069 || item.type == 2749 || item.type == 3249 || item.type == 3476 || item.type == 3474 || item.type == 3531 || item.type == 3541 || item.type == 3542 || item.type == 3569 || item.type == 3570 || item.type == 3571 || item.type == 3531 || MagicPrefix(item))
			{
				int[] valid = new int[] { 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 52, 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 59, 60, 61, 83 };
				return valid.Contains(pre);
			}
			else if (item.type == 55 || item.type == 119 || item.type == 191 || item.type == 284 || item.type == 670 || item.type == 1122 || item.type == 1513 || item.type == 1569 || item.type == 1571 || item.type == 1825 || item.type == 1918 || item.type == 3054 || item.type == 3262 || (item.type >= 3278 && item.type <= 3292) || (item.type >= 3315 && item.type <= 3317) || item.type == 3389 || item.type == 3030 || item.type == 3543 || WeaponPrefix(item))
			{
				int[] valid = new int[] { 36, 37, 38, 53, 54, 55, 39, 40, 56, 41, 57, 59, 60, 61 };
				return valid.Contains(pre);
			}
			else
			{
				if (!item.accessory || item.type == 267 || item.type == 562 || item.type == 563 || item.type == 564 || item.type == 565 || item.type == 566 || item.type == 567 || item.type == 568 || item.type == 569 || item.type == 570 || item.type == 571 || item.type == 572 || item.type == 573 || item.type == 574 || item.type == 576 || item.type == 1307 || (item.type >= 1596 && item.type < 1610) || item.vanity)
				{
					return false;
				}
				int[] valid = new int[] { 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80 };
				return valid.Contains(pre);
			}
		}

		private static bool GeneralPrefix(Item item)
		{
			return item.maxStack == 1 && item.damage > 0 && item.ammo == 0 && !item.accessory;
		}

		//add to Terraria.Item.Prefix
		internal static bool MeleePrefix(Item item)
		{
			return item.modItem != null && GeneralPrefix(item) && item.melee && !item.noUseGraphic;
		}

		//add to Terraria.Item.Prefix
		internal static bool WeaponPrefix(Item item)
		{
			return item.modItem != null && GeneralPrefix(item) && item.melee && item.noUseGraphic;
		}

		//add to Terraria.Item.Prefix
		internal static bool RangedPrefix(Item item)
		{
			return item.modItem != null && GeneralPrefix(item) && (item.ranged || item.thrown);
		}

		//add to Terraria.Item.Prefix
		internal static bool MagicPrefix(Item item)
		{
			return item.modItem != null && GeneralPrefix(item) && (item.magic || item.summon);
		}
	}

	internal class PrefixGroup
	{
		public List<int> Items { get; set; }
		public List<int> Prefixes { get; set; }
		public bool Accesories { get; set; }

		public PrefixGroup()
		{
			Items = new List<int>();
			Prefixes = new List<int>();
			Accesories = false;
		}

		public bool ItemInGroup(Item item)
		{
			if (this.Accesories && item.accessory)
			{
				return true;
			}
			if (Items.Contains(item.type))
			{
				return true;
			}
			return false;
		}
	}
}