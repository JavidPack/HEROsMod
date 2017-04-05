//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using Microsoft.Xna.Framework;
//using System.IO;

//namespace GameikiMod.GameikiVideo.Editor.Actions
//{
//    public class StageAction
//    {
//        [HideInInspector]
//        public string Name { get; set; }

//        [HideInInspector]
//        public bool Completed { get; set; }
//        public bool HaultNextActionUntilCompletion { get; set; }

//        public StageAction()
//        {
//            Completed = false;
//        }

//        public virtual void Execute()
//        {

//        }

//        public virtual void PrepareExectution()
//        {

//        }

//        public StageAction Clone()
//        {
//            MemoryStream stream = new MemoryStream();
//            BinaryWriter writer = new BinaryWriter(stream);
//            this.Save(ref writer);
//            stream.Position = 0;
//            BinaryReader reader = new BinaryReader(stream);
//            Type type = Type.GetType(reader.ReadString());
//            StageAction action = (StageAction)Activator.CreateInstance(type);
//            action.Load(Editor.saveVersion, ref reader);
//            writer.Close();
//            reader.Close();
//            stream.Close();
//            writer.Dispose();
//            reader.Dispose();
//            stream.Dispose();
//            return action;
//        }

//        public virtual void Save(ref BinaryWriter writer)
//        {
//            //Save the object Type
//            Type type = this.GetType();
//            writer.Write(type.FullName);

//            writer.Write(this.Name);
//            writer.Write(this.HaultNextActionUntilCompletion);
//        }

//        public virtual void Load(float saveVersion, ref BinaryReader reader)
//        {
//            this.Name = reader.ReadString();
//            this.HaultNextActionUntilCompletion = reader.ReadBoolean();
//        }
//    }
//}
