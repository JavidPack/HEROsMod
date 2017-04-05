//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.NPCSpawnData
//{
//    public class SpawnAreaInfo
//    {
//        public List<NPCInfo> NPCs;
//        public bool HardMode { get; set; }
//        public bool DayTime { get; set; }

//        public List<Biomes> Zones { get; set; }

//        public Layers Layer { get; set; }

//        public SpawnAreaInfo(Player player)
//        {
//            NPCs = new List<NPCInfo>();
//            DayTime = Main.dayTime;
//            HardMode = Main.hardMode;
//            Zones = new List<Biomes>();

//            if (player.position.Y > (float)((Main.maxTilesY - 200) * 16))
//            {
//                Layer = Layers.Hell;
//            }
//            else if ((double)player.position.Y > Main.rockLayer * 16.0 + (double)NPC.sHeight)
//            {
//                Layer = Layers.Caves;
//            }
//            else if ((double)player.position.Y > Main.worldSurface * 16.0 + (double)NPC.sHeight)
//            {
//                Layer = Layers.Underground;
//            }
//            else
//            {
//                Layer = Layers.Surface;
//            }

//            if (player.ZoneCrimson)
//                Zones.Add(Biomes.Blood);
//            if (player.ZoneWaterCandle)
//                Zones.Add(Biomes.Candle);
//            if (player.ZoneDungeon)
//                Zones.Add(Biomes.Dungeon);
//            if (player.ZoneCorrupt)
//                Zones.Add(Biomes.Evil);
//            if (player.ZoneHoly)
//                Zones.Add(Biomes.Holy);
//            if (player.ZoneJungle)
//                Zones.Add(Biomes.Jungle);
//            if (player.ZoneMeteor)
//                Zones.Add(Biomes.Meteor);
//            if (player.ZoneSnow)
//                Zones.Add(Biomes.Snow);

//            if(Zones.Count == 0)
//            {
//                Zones.Add(Biomes.None);
//            }
//        }

//        public void AddNPC(NPC npc)
//        {
//            NPCs.Add(new NPCInfo(npc.netID, npc.Center));
//        }

//        public enum Layers
//        {
//            Surface,
//            Underground,
//            Caves,
//            Hell
//        }

//        public enum Biomes
//        {
//            None,
//            Blood,
//            Candle,
//            Dungeon,
//            Evil,
//            Holy,
//            Jungle,
//            Meteor, 
//            Snow
//        }
//    }
//}
