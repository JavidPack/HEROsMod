//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace GameikiMod.GameikiVideo.Editor.Actions
//{
//    class ToggleUI : StageAction
//    {
//        public bool UI { get; set; }
//        public ToggleUI()
//        {
//            UI = true;
//            this.Name = "Toggle UI";
//        }

//        public override void Execute()
//        {
//            ModUtils.InterfaceVisible = UI;
//            this.Completed = true;
//            base.Execute();
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            writer.Write(UI);
//        }

//        public override void Load(float saveVersion, ref System.IO.BinaryReader reader)
//        {
//            base.Load(saveVersion, ref reader);
//            this.UI = reader.ReadBoolean();
//        }
//    }
//}
