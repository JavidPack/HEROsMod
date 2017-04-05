//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.GameikiVideo.Editor.Actions;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class StageView : UIScrollView
//    {

//        List<Stage> Stages;

//        public StageView(List<Stage> stages)
//        {
//            this.Stages = stages;
//        }

//        public void Refresh()
//        {
//            float prevScrollPosition = this.ScrollPosition;
//            this.ClearContent();
//            float yPos = Spacing;
//            for(int i = 0; i< Stages.Count; i++)
//            {
//                Stage stage = Stages[i];
//                UIStage stageUI = new UIStage(stage);
//                stageUI.X = Spacing;
//                stageUI.Y = yPos;
//                yPos += stageUI.Height;
//                stageUI.Width = this.Width - 20 - Spacing * 2;

//                stageUI.ActionAdded += stageUI_ActionAdded;
//                stageUI.ActionRemoved += stageUI_ActionRemoved;
//                stageUI.StageDeleted += stageUI_StageDeleted;
//                stageUI.StageMovedUp += stageUI_StageMovedUp;
//                stageUI.StageCollapsed += stageUI_StageCollapsed;
//                this.AddChild(stageUI);

//                if (!stageUI.Collapsed)
//                {
//                    for (int j = 0; j < stage.Actions.Count; j++)
//                    {
//                        StageAction action = stage.Actions[j];
//                        UIAction actionUI = new UIAction(action, stage);
//                        actionUI.X = stageUI.X + LargeSpacing;
//                        actionUI.Y = yPos;
//                        yPos += actionUI.Height;
//                        actionUI.Width = this.Width - actionUI.X - 20 - Spacing;
//                        actionUI.ActionRemoved += actionUI_ActionRemoved;
//                        actionUI.ActionMovedUp += actionUI_ActionMovedUp;
//                        actionUI.ActionMovedDown += actionUI_ActionMovedDown;
//                        actionUI.Tag = stage;
//                        this.AddChild(actionUI);
//                    }
//                }
//            }
//            this.ContentHeight = yPos;
//            this.ScrollPosition = prevScrollPosition;
//        }

//        void actionUI_ActionRemoved(StageAction action, Stage stage)
//        {
//            stage.RemoveAction(action);
//        }

//        void MoveAction(Stage stage, int oldIndex, int newIndex)
//        {
//            StageAction action = stage.Actions[oldIndex];
//            stage.Actions.RemoveAt(oldIndex);
//            stage.Actions.Insert(newIndex, action);
//            this.Refresh();
//        }

//        void actionUI_ActionMovedUp(StageAction action, Stage stage)
//        {
//            int actionIndex = stage.Actions.IndexOf(action);
//            if (actionIndex > 0)
//            {
//                MoveAction(stage, actionIndex, actionIndex - 1);
//            }
//        }

//        void actionUI_ActionMovedDown(StageAction action, Stage stage)
//        {
//            int actionIndex = stage.Actions.IndexOf(action);
//            if (actionIndex < stage.Actions.Count - 1)
//            {
//                MoveAction(stage, actionIndex, actionIndex + 1);
//            }
//        }

//        void stageUI_StageCollapsed(Stage stage)
//        {
//            Refresh();
//        }

//        void MoveStage(int oldIndex, int newIndex)
//        {
//            Stage stage = Stages[oldIndex];
//            Stages.RemoveAt(oldIndex);
//            Stages.Insert(newIndex, stage);
//            this.Refresh();
//        }

//        void stageUI_StageMovedDown(Stage stage)
//        {
//            int stageIndex = Stages.IndexOf(stage);
//            if (stageIndex < Stages.Count - 1)
//            {
//                MoveStage(stageIndex, stageIndex + 1);
//            }
//        }

//        void stageUI_StageMovedUp(Stage stage)
//        {
//            int stageIndex = Stages.IndexOf(stage);
//            if (stageIndex > 0)
//            {
//                MoveStage(stageIndex, stageIndex - 1);
//            }
//        }

//        void stageUI_StageDeleted(Stage stage)
//        {
//            Stages.Remove(stage);
//            Refresh();
//            if (Editor.PropertiesWindow.ObjectBeingInspected == stage)
//            {
//                Editor.PropertiesWindow.SetObject(null);
//            }
//        }

//        void stageUI_ActionRemoved(Stage sender, StageAction action)
//        {
//            Refresh();
//        }

//        void stageUI_ActionAdded(Stage sender, StageAction action)
//        {
//            Refresh();
//        }
//    }
//}
