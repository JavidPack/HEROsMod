//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using System.Xml.Serialization;

//using Terraria;

//namespace HEROsMod.HEROsModVideo.Services.DropRateInfo
//{
//	[Serializable()]
//	public class DropCollection
//	{
//		public const int SILVER_VALUE = 100;
//		public const int GOLD_VALUE = 10000;
//		public const int PLATINUM_VALUE = 1000000;

//		public List<ItemDropInfo> DropData { get; set; }
//		public NPC NPC { get; set; }
//		public int SampleSize { get; set; }


//		public int MinMoney { get; set; }
//		public int MaxMoney { get; set; }

//		public int MoneyDrops { get; set; }
//		private int _currentMoney = 0;

//		public DropCollection(int npcNum, int sampleSize)
//		{
//			DropData = new List<ItemDropInfo>();

//			this.NPC = new NPC();
//			this.NPC.SetDefaults(npcNum);
//			this.NPC.position = new Vector2(1000, 1000);
//			this.SampleSize = sampleSize;
//			this.MinMoney = int.MaxValue;
//			this.MaxMoney = 0;
//			this.MoneyDrops = 0;
//			CollectData();
//			DropData = DropData.OrderBy(x => -x.Percent).ToList();
//		}

//		public void CollectData()
//		{
//			for (int i = 0; i < SampleSize; i++)
//			{
//				ClearItems();
//				NPC.NPCLoot();
//				CheckItems(i);
//				FlushDropData(i);
//			}
//		}

//		static void ClearItems()
//		{
//			for (int i = 0; i < 20; i++)
//			{
//				Main.item[i].active = false;
//			}

//		}

//		void CheckItems(int currentSample)
//		{
//			_currentMoney = 0;
//			for (int i = 0; i < 20; i++)
//			{
//				Item item = Main.item[i];
//				if (item.type != 0 && item.active && item.stack > 0)
//				{
//					//if item is coin
//					if (item.type >= 71 && item.type <= 74)
//					{
//						if (_currentMoney == 0)
//						{
//							MoneyDrops++;
//						}
//						switch (item.type)
//						{
//							case 71: //copper
//								_currentMoney += item.stack;
//								break;
//							case 72: //silver
//								_currentMoney += item.stack * SILVER_VALUE;
//								break;
//							case 73: //gold
//								_currentMoney += item.stack * GOLD_VALUE;
//								break;
//							case 74: //platinum
//								_currentMoney += item.stack * PLATINUM_VALUE;
//								break;
//						}
//					}
//					else
//					{
//						ItemDropInfo dropInfo = null;
//						foreach (var itemData in DropData)
//						{
//							if (item.netID == itemData.NPCType)
//							{
//								dropInfo = itemData;
//								break;
//							}
//						}
//						if (dropInfo == null)
//						{
//							dropInfo = new ItemDropInfo();
//							dropInfo.NPCType = item.type;
//							dropInfo.SampleSize = SampleSize;
//							DropData.Add(dropInfo);
//						}
//						dropInfo.AddData(item.stack, currentSample);
//					}
//				}
//			}
//		}
//		void FlushDropData(int currentSample)
//		{
//			if (_currentMoney > 0)
//			{
//				if (_currentMoney < MinMoney)
//				{
//					MinMoney = _currentMoney;
//				}
//				if (_currentMoney > MaxMoney)
//				{
//					MaxMoney = _currentMoney;
//				}
//			}
//			foreach (var itemDropInfo in DropData)
//			{
//				itemDropInfo.FlushData(currentSample);
//			}
//		}
//	}
//}
