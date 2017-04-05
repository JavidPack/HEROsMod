//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.GameikiVideo.Editor.Actions;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    public delegate void ActionEvent(Stage sender, StageAction action);
//    public delegate void StageEvent(Stage stage);
//    class UIStage : UIWindow
//    {
//        public event ActionEvent ActionAdded;
//        public event ActionEvent ActionRemoved;

//        public event StageEvent StageDeleted;
//        public StageEvent StageMovedUp;
//        public StageEvent StageMovedDown;
//        public StageEvent StageCollapsed;

//        UILabel bAddAction;
//        UILabel bDeleteStage;
//        UILabel bMoveUp;
//        UILabel bMoveDown;
//        UILabel bTest;
//        UILabel bProperties;
//        UILabel bClone;
//        UILabel bCreateTemplate;
//        UILabel bCopyToStages;

//        public bool Collapsed
//        {
//            get { return Stage.Collapsed; }
//            set { Stage.Collapsed = value; }
//        }

//        public Stage Stage { get; set; }

//        UILabel lName;
//        UILabel lCollapse;
//        public UIStage(Stage stage)
//        {
//            this.Stage = stage;

//            lCollapse = new UILabel(Collapsed ? "[+]" : "[-]");
//            lCollapse.Scale = .4f;
//            lCollapse.Anchor = AnchorPosition.Left;
//            lCollapse.X = Spacing;
//            lCollapse.onLeftClick += collapse_onLeftClick;
//            lCollapse.Tooltip = "Collapse";

//            lName = new UILabel(stage.Name);
//            lName.X = lCollapse.X + lCollapse.Width + Spacing;
//            lName.Y = SmallSpacing;
//            lName.Scale = .5f;

//            stage.ActionRemoved += stage_ActionRemoved;

//            this.Height = lName.Y + lName.Height + SmallSpacing;
//            lCollapse.Y = this.Height / 2;

//            bAddAction = new UILabel("+");
//            bAddAction.Anchor = AnchorPosition.Right;
//            bAddAction.Scale = .4f;
//            bAddAction.X = Width - Spacing;
//            bAddAction.Y = Height / 2;
//            bAddAction.onLeftClick += bAddAction_onLeftClick;
//            bAddAction.Tooltip = "Add Action";

//            bDeleteStage = new UILabel("-");
//            bDeleteStage.Anchor = AnchorPosition.Right;
//            bDeleteStage.Scale = .4f;
//            bDeleteStage.X = bAddAction.X - bAddAction.Width - Spacing;
//            bDeleteStage.Y = bAddAction.Y;
//            bDeleteStage.onLeftClick += bDeleteStage_onLeftClick;
//            bDeleteStage.Tooltip = "Remove Stage";

//            bMoveDown = new UILabel("v");
//            bMoveDown.Anchor = AnchorPosition.Right;
//            bMoveDown.Scale = .4f;
//            bMoveDown.X = bDeleteStage.X - bDeleteStage.Width - Spacing;
//            bMoveDown.Y = bAddAction.Y;
//            bMoveDown.onLeftClick += bMoveDown_onLeftClick;
//            bMoveDown.Tooltip = "Move Down";

//            bMoveUp = new UILabel("^");
//            bMoveUp.Anchor = AnchorPosition.Center;
//            bMoveUp.Scale = .4f;
//            bMoveUp.X = bMoveDown.X - bMoveDown.Width - Spacing;
//            bMoveUp.Y = bAddAction.Y;
//            bMoveUp.onLeftClick += bMoveUp_onLeftClick;
//            bMoveUp.Tooltip = "Move Up";

//            bTest = new UILabel("T");
//            bTest.Anchor = AnchorPosition.Right;
//            bTest.Scale = .4f;
//            bTest.Y = bAddAction.Y;
//            bTest.X = bMoveUp.X - bMoveUp.Width - Spacing;
//            bTest.onLeftClick += bTest_onLeftClick;
//            bTest.Tooltip = "Test";

//            bProperties = new UILabel("P");
//            bProperties.Anchor = AnchorPosition.Right;
//            bProperties.Scale = .4f;
//            bProperties.Y = bAddAction.Y;
//            bProperties.X = bTest.X - bTest.Width - Spacing;
//            bProperties.onLeftClick += bProperties_onLeftClick;
//            bProperties.Tooltip = "Properties";

//            bClone = new UILabel("C");
//            bClone.Anchor = AnchorPosition.Right;
//            bClone.Scale = .4f;
//            bClone.Y = bAddAction.Y;
//            bClone.X = bProperties.X - bProperties.Width - Spacing;
//            bClone.onLeftClick += bClone_onLeftClick;
//            bClone.Tooltip = "Clone";

//            bCreateTemplate = new UILabel("CT");
//            bCreateTemplate.Anchor = AnchorPosition.Right;
//            bCreateTemplate.Scale = .4f;
//            bCreateTemplate.Y = bAddAction.Y;
//            bCreateTemplate.X = bClone.X - bClone.Width - Spacing;
//            bCreateTemplate.onLeftClick += bCreateTemplate_onLeftClick;
//            bCreateTemplate.Tooltip = "Create Template";

//            bCopyToStages = new UILabel("CS");
//            bCopyToStages.Anchor = AnchorPosition.Right;
//            bCopyToStages.Scale = .4f;
//            bCopyToStages.Y = bAddAction.Y;
//            bCopyToStages.X = bClone.X - bClone.Width - Spacing;
//            bCopyToStages.onLeftClick += bCopyToStages_onLeftClick;
//            bCopyToStages.Tooltip = "Copy To Stages";

//            if(!this.Stage.IsTemplate)
//            {
//                this.AddChild(bCreateTemplate);
//            }
//            else
//            {
//                this.AddChild(bCopyToStages);
//            }
//            this.AddChild(bClone);
//            this.AddChild(bProperties);
//            this.AddChild(bMoveDown);
//            this.AddChild(bMoveUp);
//            this.AddChild(bTest);
//            this.AddChild(bDeleteStage);
//            this.AddChild(bAddAction);
//            this.AddChild(lName);
//            this.AddChild(lCollapse);
//        }

//        void bCopyToStages_onLeftClick(object sender, EventArgs e)
//        {
//            Stage stage = this.Stage.Clone();
//            stage.IsTemplate = false;
//            Editor.AddStage(stage);
//        }

//        void bCreateTemplate_onLeftClick(object sender, EventArgs e)
//        {
//            Stage template = this.Stage.Clone();
//            template.IsTemplate = true;
//            Editor.AddStageTemplate(template);
//        }

//        void bClone_onLeftClick(object sender, EventArgs e)
//        {
//            Editor.AddStage(this.Stage.Clone());
//        }

//        void bProperties_onLeftClick(object sender, EventArgs e)
//        {
//            Editor.PropertiesWindow.SetObject(this.Stage);
//        }

//        void collapse_onLeftClick(object sender, EventArgs e)
//        {
//            this.Collapsed = !this.Collapsed;
//            this.lCollapse.Text = Collapsed ? "[+]" : "[-]";
//            if(StageCollapsed != null)
//            {
//                StageCollapsed(this.Stage);
//            }
//        }

//        void bMoveUp_onLeftClick(object sender, EventArgs e)
//        {
//            if(StageMovedUp != null)
//            {
//                StageMovedUp(this.Stage);
//            }
//        }

//        void bMoveDown_onLeftClick(object sender, EventArgs e)
//        {
//            if(StageMovedDown != null)
//            {
//                StageMovedDown(this.Stage);
//            }
//        }

//        void stage_ActionRemoved(Stage sender, StageAction action)
//        {
//            this.RemoveAction(action);
//        }

//        void bTest_onLeftClick(object sender, EventArgs e)
//        {
//            this.Stage.Execute();
//        }

//        void bDeleteStage_onLeftClick(object sender, EventArgs e)
//        {
//            if (StageDeleted != null)
//            {
//                StageDeleted(this.Stage);
//            }
//        }

//        void bAddAction_onLeftClick(object sender, EventArgs e)
//        {
//            Editor.ActionsWindow = new UI.ActionsWindow();
//            UIView.exclusiveControl = Editor.ActionsWindow;
//            Editor.ActionsWindow.ActionSelected += ActionsWindow_ActionSelected;
//            MasterView.gameScreen.AddChild(Editor.ActionsWindow);
//        }

//        void ActionsWindow_ActionSelected(StageAction action)
//        {
//            if (action != null)
//            {
//                MasterView.gameScreen.RemoveChild(Editor.ActionsWindow);
//                Editor.ActionsWindow.ActionSelected -= ActionsWindow_ActionSelected;
//                UIView.exclusiveControl = null;
//                AddAction(action);
//                this.Collapsed = false;
//                if (ActionAdded != null)
//                {
//                    ActionAdded(this.Stage, action);
//                }
//            }
//            else
//            {
//                Main.NewText("Action was null");
//            }
//        }

//        protected override void SetWidth(float width)
//        {
//            base.SetWidth(width);
//            bAddAction.X = Width - LargeSpacing;
//            bDeleteStage.X = bAddAction.X - bAddAction.Width - Spacing;
//            bMoveDown.X = bDeleteStage.X - bDeleteStage.Width - Spacing;
//            bMoveUp.X = bMoveDown.X - bMoveDown.Width - Spacing;
//            bTest.X = bMoveUp.X - bMoveUp.Width - Spacing;
//            bProperties.X = bTest.X - bTest.Width - Spacing;
//            bClone.X = bProperties.X - bProperties.Width - Spacing;
//            bCreateTemplate.X = bClone.X - bClone.Width - Spacing;
//            bCopyToStages.X = bClone.X - bClone.Width - Spacing;
//        }

//        public void AddAction(StageAction action)
//        {
//            this.Stage.Actions.Add(action);
//        }

//        public void RemoveAction(StageAction action)
//        {
//            if (ActionRemoved != null)
//            {
//                ActionRemoved(this.Stage, action);
//            }
//        }

//        public override void Update()
//        {
//            lName.Text = Stage.Name;
//            base.Update();
//        }
//    }
//}
