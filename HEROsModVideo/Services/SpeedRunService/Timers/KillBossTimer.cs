//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    class KillBossTimer  : EventTimer
//    {
//        public int NPCNetID = 0;

//        public KillBossTimer(string name, int itemType, int steps, int npcNetID) : base (name, itemType, steps)
//        {
//            this.NPCNetID = npcNetID;
//            this._timerSaveID = 2;
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            writer.Write(NPCNetID);
//        }

//        public override void Load(ref System.IO.BinaryReader reader)
//        {
//            base.Load(ref reader);
//            this.NPCNetID = reader.ReadInt32();
//        }

//    }
//}
