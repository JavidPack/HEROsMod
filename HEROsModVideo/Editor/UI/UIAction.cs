//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.GameikiVideo.Editor.Actions;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class UIAction : UIWindow
//    {
//        public delegate void ActionEvent(StageAction action, Stage stage);
//        public event ActionEvent ActionRemoved;
//        public event ActionEvent ActionMovedUp;
//        public event ActionEvent ActionMovedDown;
//        StageAction action;
//        Stage stage;

//        UILabel bRemoveAction;
//        UILabel bProperties;
//        UILabel bMoveUp;
//        UILabel bMoveDown;
//        UILabel bClone;
//        public UIAction(StageAction action, Stage stage)
//        {
//            this.stage = stage;
//            this.action = action;
//            UILabel label = new UILabel(action.Name);
//            label.X = Spacing;
//            label.Y = SmallSpacing;
//            label.Scale = .4f;
            

//            this.Height = label.Y + label.Height + SmallSpacing;


//            bRemoveAction = new UILabel("-");
//            bRemoveAction.Anchor = AnchorPosition.Right;
//            bRemoveAction.Scale = .4f;
//            bRemoveAction.X = Width - LargeSpacing;
//            bRemoveAction.Y = Height / 2;
//            bRemoveAction.Tooltip = "Remove Action";
//            bRemoveAction.onLeftClick += bRemoveAction_onLeftClick;

//            bMoveDown = new UILabel("v");
//            bMoveDown.Anchor = AnchorPosition.Right;
//            bMoveDown.Scale = .4f;
//            bMoveDown.X = bRemoveAction.X - bRemoveAction.Width - Spacing;
//            bMoveDown.Y = bRemoveAction.Y;
//            bMoveDown.Tooltip = "Move Down";
//            bMoveDown.onLeftClick += bMoveDown_onLeftClick;

//            bMoveUp = new UILabel("^");
//            bMoveUp.Anchor = AnchorPosition.Right;
//            bMoveUp.Scale = .4f;
//            bMoveUp.X = bMoveDown.X - bMoveDown.Width - Spacing;
//            bMoveUp.Y = bRemoveAction.Y;
//            bMoveUp.Tooltip = "Move Down";
//            bMoveUp.onLeftClick += bMoveUp_onLeftClick;

//            bProperties = new UILabel("P");
//            bProperties.Anchor = AnchorPosition.Right;
//            bProperties.Scale = .4f;
//            bProperties.X = bMoveUp.X - bMoveUp.Width - Spacing;
//            bProperties.Y = this.Height / 2;
//            bProperties.onLeftClick += bProperties_onLeftClick;
//            bProperties.Tooltip = "Properties";

//            bClone = new UILabel("C");
//            bClone.Anchor = AnchorPosition.Right;
//            bClone.Scale = .4f;
//            bClone.X = bProperties.X - bProperties.Width - Spacing;
//            bClone.Y = this.Height / 2;
//            bClone.onLeftClick += bClone_onLeftClick;
//            bClone.Tooltip = "Clone";


//            AddChild(bClone);
//            AddChild(bMoveDown);
//            AddChild(bMoveUp);
//            AddChild(bProperties);
//            AddChild(bRemoveAction);
//            this.AddChild(label);
//        }

//        void bClone_onLeftClick(object sender, EventArgs e)
//        {
//            int indexOfAction = stage.Actions.IndexOf(this.action);
//            stage.Actions.Insert(indexOfAction + 1, action.Clone());
//            Editor.stageWindow.Refresh();
//            Editor.stageTemplateWindow.Refresh();
//        }

//        void bMoveUp_onLeftClick(object sender, EventArgs e)
//        {
//            if(ActionMovedUp != null)
//            {
//                ActionMovedUp(this.action, this.stage);
//            }
//        }

//        void bMoveDown_onLeftClick(object sender, EventArgs e)
//        {
//            if (ActionMovedDown != null)
//            {
//                ActionMovedDown(this.action, this.stage);
//            }
//        }

//        void bProperties_onLeftClick(object sender, EventArgs e)
//        {
//            Editor.PropertiesWindow.SetObject(this.action);
//        }

//        void bRemoveAction_onLeftClick(object sender, EventArgs e)
//        {
//            if(ActionRemoved != null)
//            {
//                ActionRemoved(this.action, this.stage);
//            }
//        }

//        protected override void SetWidth(float width)
//        {
//            base.SetWidth(width);
//            bRemoveAction.X = Width - LargeSpacing;
//            bMoveDown.X = bRemoveAction.X - bRemoveAction.Width - Spacing;
//            bMoveUp.X = bMoveDown.X - bMoveDown.Width - Spacing;
//            bProperties.X = bMoveUp.X - bMoveUp.Width - Spacing;
//            bClone.X = bProperties.X - bProperties.Width - Spacing;
//        }
//    }
//}
