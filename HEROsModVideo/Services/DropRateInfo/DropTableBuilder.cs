//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Diagnostics;
//using Microsoft.Xna.Framework;
//using System.Xml.Serialization;
//using System.Xml;
//using System.IO;

//using Terraria;

//namespace HEROsMod.HEROsModVideo.Services.DropRateInfo
//{
//	class DropTableBuilder
//	{
//		public static DropTable DropTable;
//		public static void Start()
//		{
//			//ExportDropTable();
//			//ImportDropTable();
//		}

//		public static void ExportDropTable()
//		{
//			List<DropCollection> dropCollections = new List<DropCollection>();
//			var watch = Stopwatch.StartNew();
//			for (int i = 0; i < Main.npcTexture.Length; i++)
//			{
//				//Console.WriteLine("NPC " + i);
//				DropCollection dc = new DropCollection(i, 1000000);
//				dropCollections.Add(dc);
//			}
//			watch.Stop();
//			//Console.WriteLine("DropTable Build Time: " + watch.Elapsed.TotalSeconds);

//			DropTable dt = new DropTable();
//			foreach (DropCollection collection in dropCollections)
//			{
//				NPCDropTable npcDropTable = new NPCDropTable();
//				npcDropTable.MinMoney = collection.MinMoney;
//				npcDropTable.MaxMoney = collection.MaxMoney;
//				npcDropTable.MoneyDrops = collection.MoneyDrops;
//				npcDropTable.SampleSize = collection.SampleSize;
//				npcDropTable.NPCType = collection.NPC.type;
//				foreach (ItemDropInfo dropData in collection.DropData)
//				{
//					npcDropTable.Drops.Add(dropData);
//				}
//				dt.NPCDropTables.Add(npcDropTable);
//			}

//			XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(dt.GetType());

//			XmlWriterSettings settings = new XmlWriterSettings();
//			settings.Encoding = Encoding.UTF8;
//			settings.Indent = true;
//			settings.OmitXmlDeclaration = false;

//			string xmlOut;
//			using (StringWriter textWriter = new StringWriter())
//			{
//				using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
//				{
//					serializer.Serialize(xmlWriter, dt);

//				}
//				xmlOut = textWriter.ToString();
//			}
//			File.WriteAllText("DropTable.xml", xmlOut);

//			//Console.WriteLine("finish");
//		}

//		public static void ImportDropTable()
//		{
//			XmlSerializer serializer = new XmlSerializer(typeof(DropTable));
//			using (StreamReader reader = new StreamReader("DropTable.xml", Encoding.UTF8))
//			{
//				DropTable = (DropTable)serializer.Deserialize(reader);
//			}
//		}
//	}

//	[Serializable()]
//	[XmlRoot("DropTable")]
//	public class DropTable
//	{
//		[XmlArray("NPCDropTables")]
//		[XmlArrayItem("NPCDropTable", typeof(NPCDropTable))]
//		public List<NPCDropTable> NPCDropTables { get; set; }

//		public DropTable()
//		{
//			NPCDropTables = new List<NPCDropTable>();
//		}

//	}

//	[Serializable()]
//	public class NPCDropTable
//	{
//		[XmlArray("Drops")]
//		[XmlArrayItem("Drop", typeof(ItemDropInfo))]
//		public List<ItemDropInfo> Drops { get; set; }

//		[XmlAttribute]
//		public int NPCType { get; set; }
//		[XmlAttribute]
//		public int MoneyDrops { get; set; }
//		[XmlAttribute]
//		public int MinMoney { get; set; }
//		[XmlAttribute]
//		public int MaxMoney { get; set; }
//		[XmlAttribute]
//		public int SampleSize { get; set; }

//		[XmlIgnore()]
//		public float MoneyDropPercentage
//		{
//			get
//			{
//				return (float)MoneyDrops / SampleSize * 100;
//			}
//		}

//		public NPCDropTable()
//		{
//			Drops = new List<ItemDropInfo>();
//		}

//		public static string FormatMoney(int amount)
//		{
//			if (amount == 0)
//			{
//				return "0c";
//			}
//			int platinum = amount / DropCollection.PLATINUM_VALUE;
//			amount -= platinum * DropCollection.PLATINUM_VALUE;
//			int gold = amount / DropCollection.GOLD_VALUE;
//			amount -= gold * DropCollection.GOLD_VALUE;
//			int silver = amount / DropCollection.SILVER_VALUE;
//			amount -= silver * DropCollection.SILVER_VALUE;
//			int copper = amount;

//			string result = "";
//			if (platinum > 0)
//			{
//				result += platinum + "p";
//			}
//			if (gold > 0)
//			{
//				result += gold + "g";
//			}
//			if (silver > 0)
//			{
//				result += silver + "s";
//			}
//			if (copper > 0)
//			{
//				result += copper + "c";
//			}
//			return result;

//		}

//		public override string ToString()
//		{
//			return string.Format("{0} Drops: {1} {2} - {3}", Lang.npcName(NPCType, true), Drops.Count, FormatMoney(MinMoney), FormatMoney(MaxMoney));
//			return base.ToString();
//		}
//	}
//}
