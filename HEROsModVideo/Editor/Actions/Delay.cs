//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace GameikiMod.GameikiVideo.Editor.Actions
//{
//    class Delay: StageAction
//    {
//        [HideInInspector]
//        public float Timer { get; set; }
//        public float Time { get; set; }
//        public Delay()
//        {
//            this.Time = 1f;
//            this.Timer = this.Time;
//            this.HaultNextActionUntilCompletion = true;
//            this.Name = "Delay";
//        }

//        public override void Execute()
//        {
//            this.Timer -= ModUtils.DeltaTime;
//            if(Timer <= 0)
//            {
//                this.Completed = true;
//            }
//            base.Execute();
//        }

//        public override void PrepareExectution()
//        {
//            this.Timer = Time;
//            base.PrepareExectution();
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            writer.Write(Time);
//        }

//        public override void Load(float saveVersion, ref System.IO.BinaryReader reader)
//        {
//            base.Load(saveVersion, ref reader);
//            this.Time = reader.ReadSingle();
//        }
//    }
//}
