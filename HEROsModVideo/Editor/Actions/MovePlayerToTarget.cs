//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.Actions
//{
//    class MovePlayerToTarget : StageAction
//    {
//        public WorldObjects.WorldObject Target { get; set; }
//        public MovePlayerToTarget()
//        {
//            this.Name = "Move Player";
//            this.Target = null;
//        }

//        public override void Execute()
//        {
//            if (this.Target != null)
//            {
//                Player player = Main.player[Main.myPlayer];
//                player.position.X = Target.Left + Target.Width / 2 - player.width / 2;
//                player.position.Y = Target.Top + Target.Height / 2 - player.height / 2;

//                player.velocity = Vector2.Zero;
//                player.fallStart = (int)(player.position.Y / 16f);
//            }
//            else
//            {
//                Main.NewText("Move Player Target is null");
//            }
//            this.Completed = true;
//            base.Execute();
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            if (Target != null)
//                writer.Write(Target.GUID.ToString());
//            else
//                writer.Write("");
//        }

//        public override void Load(float saveVersion, ref System.IO.BinaryReader reader)
//        {
//            base.Load(saveVersion, ref reader);
//            string guidStr = reader.ReadString();
//            if (guidStr.Length > 0)
//            {
//                Guid guid = Guid.Parse(guidStr);
//                Target = Editor.GetWorldObjectByGuid(guid);
//            }
//        }
//    }
//}
