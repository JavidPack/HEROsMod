//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.GameikiVideo.Editor;
//using GameikiMod.GameikiVideo.Editor.Actions;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class StagesWindow : UIWindow
//    {
//        List<Stage> Stages
//        {
//            get { return Editor.Stages; }
//        }

//        UIButton bAddStage;
//        StageView scrollView;
//        public StagesWindow()
//        {
//            this.CanMove = true;
//            this.Width = 500;


//            bAddStage = new UIButton("+");
//            bAddStage.Tooltip = "Add Stage";
//            bAddStage.X = LargeSpacing;
//            bAddStage.Y = LargeSpacing;
//            bAddStage.onLeftClick += bAddStage_onLeftClick;


//            scrollView = new StageView(Stages);
//            scrollView.X = LargeSpacing;
//            scrollView.Y = bAddStage.Y + bAddStage.Height + Spacing;
//            scrollView.Width = Width - LargeSpacing * 2;

//            UIImage bClose = new UIImage(closeTexture);
//            bClose.X = this.Width - bClose.Width - LargeSpacing;
//            bClose.Y = LargeSpacing;
//            bClose.onLeftClick += bClose_onLeftClick;

//            this.Height = scrollView.Y + scrollView.Height + LargeSpacing;

//            AddChild(bAddStage);
//            AddChild(scrollView);
//            AddChild(bClose);

//            Refresh();
//        }

//        void bClose_onLeftClick(object sender, EventArgs e)
//        {
//            this.Visible = false;
//        }

//        void bAddStage_onLeftClick(object sender, EventArgs e)
//        {
//            Stage stage = new Stage("Stage " + (Stages.Count +  1));
//            Editor.AddStage(stage);
//        }

//        public void Refresh()
//        {
//            scrollView.Refresh();
//        }
//    }
//}
