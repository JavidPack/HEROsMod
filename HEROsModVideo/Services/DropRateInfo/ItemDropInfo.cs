//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml.Serialization;

//using Terraria;

//namespace HEROsMod.HEROsModVideo.Services.DropRateInfo
//{
//	[Serializable()]
//	public class ItemDropInfo
//	{
//		[XmlAttribute]
//		public int NPCType { get; set; }
//		[XmlAttribute]
//		public int Min { get; set; }
//		[XmlAttribute]
//		public int Max { get; set; }




//		[XmlAttribute]
//		public int SampleSize { get; set; }
//		[XmlAttribute]
//		public int TimesDropped { get; set; }

//		private int _currentSample;
//		private int _amountForCurrentSamle;

//		[XmlIgnore()]
//		public float Percent
//		{
//			get
//			{
//				return (float)TimesDropped / SampleSize;
//			}
//		}

//		public ItemDropInfo()
//		{
//			this.NPCType = 0;
//			this.SampleSize = 0;
//			this.Min = int.MaxValue;
//			this.Max = int.MinValue;
//			_currentSample = -1;
//			TimesDropped = 0;
//		}

//		public void AddData(int amount, int sampleNumber)
//		{
//			if (_currentSample != sampleNumber)
//			{
//				TimesDropped++;
//				_currentSample = sampleNumber;
//				_amountForCurrentSamle = 0;
//			}
//			_amountForCurrentSamle += amount;
//		}

//		public void FlushData(int currentSample)
//		{
//			if (_currentSample == currentSample)
//			{
//				if (_amountForCurrentSamle < Min)
//				{
//					Min = _amountForCurrentSamle;
//				}
//				if (_amountForCurrentSamle > Max)
//				{
//					Max = _amountForCurrentSamle;
//				}
//				_amountForCurrentSamle = 0;
//			}
//		}

//		public override string ToString()
//		{
//			return string.Format("%{0} {1}-{2} {3}", Percent * 100, Min, Max, Lang.itemName(NPCType, true));
//		}
//	}
//}
