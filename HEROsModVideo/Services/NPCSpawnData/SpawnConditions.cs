//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace GameikiMod.GameikiVideo.Services.NPCSpawnData
//{
//    class SpawnConditions
//    {
//        public int NPCID { get; set; }
//        public bool Night { get; set; }
//        public bool Day { get; set; }
//        public bool HM { get; set; }
//        public bool NoHM { get; set; }
//        public List<SpawnAreaInfo.Layers> Layers { get; set; }
//        public List<ZoneCount> Zones { get; set; }

//        public SpawnConditions(int npcID)
//        {
//            this.NPCID = npcID;
//            Layers = new List<SpawnAreaInfo.Layers>();
//            Zones = new List<ZoneCount>();
//        }

//        public void AddData(SpawnAreaInfo spawnArea, NPCInfo npcInfo)
//        {
//            if(spawnArea.DayTime)
//            {
//                Day = true;
//            }
//            else
//            {
//                Night = true;
//            }
//            if(spawnArea.HardMode)
//            {
//                HM = true;
//            }
//            else
//            {
//                NoHM = true;
//            }
//            if(!Layers.Contains(spawnArea.Layer))
//            {
//                Layers.Add(spawnArea.Layer);
//            }

//            for(int i = 0; i < spawnArea.Zones.Count; i++)
//            {
//                ZoneCount zc = GetZone(spawnArea.Zones[i]);
//                zc.Count++;
//            }
//        }

//        private ZoneCount GetZone(SpawnAreaInfo.Biomes zone)
//        {
//            for(int i = 0; i< Zones.Count; i++)
//            {
//                if(Zones[i].Zone == zone)
//                {
//                    return Zones[i];
//                }
//            }
//            ZoneCount z = new ZoneCount(zone);
//            Zones.Add(z);
//            return z;
//        }

//        public override string ToString()
//        {
//            string result = string.Empty;
//            if (Day) result += "Day ";
//            if (Night) result += "Night ";
//            if (HM) result += "HM ";
//            if (NoHM) result += "NoHM ";

//            for(int i = 0; i < Layers.Count; i++)
//            {
//                result += Layers[i].ToString() + " ";
//            }
//            for (int i = 0; i < Zones.Count; i++)
//            {
//                result += Zones[i].ToString() + " ";
//            }
//            return result;
//        }
//    }

//    public class ZoneCount
//    {
//        public SpawnAreaInfo.Biomes Zone { get; set; }
//        public int Count { get; set; }

//        public ZoneCount(SpawnAreaInfo.Biomes zone)
//        {
//            this.Zone = zone;
//            this.Count = 0;
//        }
//        public override string ToString()
//        {
//            return this.Zone.ToString() + "(" + Count + ")";
//        }
//    }

//}
