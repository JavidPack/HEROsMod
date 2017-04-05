//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{

//    class StageTemplateWindow : UIWindow
//    {
//        List<Stage> Stages
//        {
//            get { return Editor.StageTemplates; }
//        }

//        StageView scrollView;
//        public StageTemplateWindow()
//        {
//            this.CanMove = true;
//            this.Width = 500;

//            UILabel lTitle = new UILabel("Stage Templates");
//            lTitle.Scale = .5f;
//            lTitle.X = LargeSpacing;
//            lTitle.Y = LargeSpacing;
//            lTitle.OverridesMouse = false;

//            scrollView = new StageView(Stages);
//            scrollView.X = LargeSpacing;
//            scrollView.Y = lTitle.Y + lTitle.Height + Spacing;
//            scrollView.Width = Width - LargeSpacing * 2;

//            UIImage bClose = new UIImage(closeTexture);
//            bClose.X = this.Width - bClose.Width - LargeSpacing;
//            bClose.Y = LargeSpacing;
//            bClose.onLeftClick += bClose_onLeftClick;

//            this.Height = scrollView.Y + scrollView.Height + LargeSpacing;

//            AddChild(lTitle);
//            AddChild(scrollView);
//            AddChild(bClose);

//            Refresh();
//        }

//        void bClose_onLeftClick(object sender, EventArgs e)
//        {
//            this.Visible = false;
//        }

//        public void Refresh()
//        {
//            scrollView.Refresh();
//        }
//    }
//}
