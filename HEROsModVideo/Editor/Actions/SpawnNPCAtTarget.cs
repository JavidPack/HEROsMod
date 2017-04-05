//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.GameikiVideo.Editor.WorldObjects;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.Actions
//{
//    class SpawnNPCAtTarget : StageAction
//    {
//        public WorldObject Target { get; set; }
//        public int NPCType { get; set; }

//        public SpawnNPCAtTarget()
//        {

//        }

//        public override void Execute()
//        {
//            if(Target != null)
//            {
//                if(Lang.npcName(NPCType).Length > 0)
//                {
//                    NPC.NewNPC((int)Target.X, (int)Target.Y, NPCType);
//                }
//            }
//            this.Completed = true;
//            base.Execute();
//        }
//    }
//}
