//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using Microsoft.Xna.Framework;
//using GameikiMod.GameikiVideo.Editor;
//using GameikiMod.GameikiVideo.Editor.Actions;
//using System.IO;

//namespace GameikiMod.GameikiVideo.Editor
//{
//    public class Stage
//    {
//        public delegate void ActionEvent(Stage sender, StageAction action);
//        public event ActionEvent ActionRemoved;

//        public String Name { get; set; }
//        [HideInInspector]
//        public List<StageAction> Actions { get; set; }
//        [HideInInspector]
//        public bool Completed { get; set; }
//        [HideInInspector]
//        public bool Collapsed { get; set; }
//        [HideInInspector]
//        public bool IsTemplate { get; set; }

//        public Stage(string name)
//        {
//            this.IsTemplate = false;
//            this.Collapsed = false;
//            this.Name = name;
//            Actions = new List<StageAction>();

//            this.Completed = true;
//            UILabel label = new UILabel(name);

//        }

//        public void Execute()
//        {
//            this.Completed = false;
//            PrepareActionsForExecution();
//        }

//        public void UpdateExecution()
//        {
//            bool allActionsComplete = true;
//            for(int i = 0; i < Actions.Count; i++)
//            {
//                if(!Actions[i].Completed)
//                {
//                    Actions[i].Execute();
//                    allActionsComplete = false;
//                    if(Actions[i].HaultNextActionUntilCompletion)
//                        return;
//                }
//            }
//            if (!allActionsComplete)
//                return;
//            this.Completed = true;
//            SetAllActionsToIncomplete();
//        }

//        public void AddAction(StageAction action)
//        {
//            Actions.Add(action);
//            //action.ActionRemoved += action_ActionRemoved;
//        }


//        private void SetAllActionsToIncomplete()
//        {
//            foreach(StageAction action in Actions)
//            {
//                action.Completed = false;
//            }
//        }

//        public void PrepareActionsForExecution()
//        {
//            foreach(StageAction action in Actions)
//            {
//                action.PrepareExectution();
//            }
//        }

//        public void RemoveAction(StageAction action)
//        {
//            this.Actions.Remove(action);
//            if (Editor.PropertiesWindow.ObjectBeingInspected == action)
//            {
//                Editor.PropertiesWindow.SetObject(null);
//            }
//            if (ActionRemoved != null)
//            {
//                ActionRemoved(this, action);
//            }
//        }

//        public void Update()
//        {
//            if (!Completed)
//            {
//                this.UpdateExecution();
//            }
//        }

//        public Stage Clone()
//        {
//            MemoryStream stream = new MemoryStream();
//            BinaryWriter writer = new BinaryWriter(stream);
//            this.Save(ref writer);
//            stream.Position = 0;
//            BinaryReader reader = new BinaryReader(stream);
//            Stage result = new Stage("");
//            result.Load(Editor.saveVersion, ref reader);
//            writer.Close();
//            reader.Close();
//            stream.Close();
//            writer.Dispose();
//            reader.Dispose();
//            stream.Dispose();
//            return result;
//        }

//        public void Save(ref BinaryWriter writer)
//        {
//            writer.Write(this.Name);
//            writer.Write(this.Completed);
//            writer.Write(this.Actions.Count);
//            writer.Write(this.IsTemplate);
//            foreach(StageAction action in Actions)
//            {
//                action.Save(ref writer);
//            }
//        }

//        public void Load(int saveVersion, ref BinaryReader reader)
//        {
//            this.Name = reader.ReadString();
//            this.Collapsed = reader.ReadBoolean();
//            int numOfActions = reader.ReadInt32();
//            if(saveVersion > 0)
//            {
//                this.IsTemplate = reader.ReadBoolean();
//            }
//            for(int i = 0; i < numOfActions; i++)
//            {
//                Type type = Type.GetType(reader.ReadString());
//                StageAction action = (StageAction)Activator.CreateInstance(type);
//                action.Load(saveVersion, ref reader);
//                this.Actions.Add(action);
//            }
//        }
//    }
//}
