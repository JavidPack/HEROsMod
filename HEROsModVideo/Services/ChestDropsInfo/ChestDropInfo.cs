//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.GameikiVideo.Services.DropRateInfo;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.ChestDropsInfo
//{
//    class ChestDropInfo
//    {
//        public int Type { get; set; }
//        public List<ItemDropInfo> ItemDrops { get; set; }
//        private int _currentSample = 0;
//        private int _sampleSize = 0;

//        public ChestDropInfo()
//        {
//            ItemDrops = new List<ItemDropInfo>();
//        }

//        public void AddItems(Item[] items)
//        {
//            _sampleSize++;
//            for (int i = 0; i < items.Length; i++)
//            {
//                AddItem(items[i]);
//            }
//            _currentSample++;
//            foreach(var itemDrop in ItemDrops)
//            {
//                itemDrop.SampleSize = _sampleSize;
//            }
//        }


//        public void AddItem(Item item)
//        {
//            ItemDropInfo di = GetItemDrop(item);
//            di.AddData(item.stack, _currentSample);
//            di.FlushData(_currentSample);
//        }

//        public ItemDropInfo GetItemDrop(Item item)
//        {
//            for (int i = 0; i < ItemDrops.Count; i++)
//            {
//                if(ItemDrops[i].NPCType == item.netID)
//                {
//                    return ItemDrops[i];
//                }
//            }
//            ItemDropInfo dropInfo = new ItemDropInfo();
//            dropInfo.NPCType = item.netID;
//            ItemDrops.Add(dropInfo);
//            return dropInfo;
//        }

//        public override string ToString()
//        {
//            if(this.Type == -2)
//            {
//                return "Dungeon Chest";
//            }
//            if(this.Type == 327)
//            {
//                return "Gold Dungeon Chest";
//            }
//            return Lang.itemName(this.Type);
//        }
//    }
//}
