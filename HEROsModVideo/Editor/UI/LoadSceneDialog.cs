//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class LoadSceneDialog : UIWindow
//    {
//        UIScrollView scrollView;
//        public LoadSceneDialog()
//        {
//            this.Anchor = AnchorPosition.Center;

//            this.Width = 350;
//            this.Height = 250;

//            UILabel label = new UILabel("Select Scene");
//            label.Scale = .4f;
//            label.X = LargeSpacing;
//            label.Y = LargeSpacing;

//            UIButton bCancel = new UIButton("Cancel");
//            bCancel.X = this.Width - bCancel.Width - LargeSpacing;
//            bCancel.Y = this.Height - bCancel.Height - LargeSpacing;
//            bCancel.onLeftClick += bCancel_onLeftClick;

//            scrollView = new UIScrollView();
//            scrollView.Height = this.Height - label.Height - bCancel.Height - LargeSpacing * 4;
//            scrollView.Width = this.Width - LargeSpacing * 2;
//            scrollView.X = LargeSpacing;
//            scrollView.Y = label.Y + label.Height + LargeSpacing;

//            AddChild(label);
//            AddChild(scrollView);
//            AddChild(bCancel);
//        }

//        public void PopulateScenes()
//        {
//            scrollView.ClearContent();
//            float yPos = Spacing;
//            string[] fileNames = Editor.GetListOfSaves();
//            foreach(string fileName in fileNames)
//            {
//                UILabel label = new UILabel(fileName);
//                label.Scale = .4f;
//                label.X = Spacing;
//                label.Y = yPos;
//                yPos += label.Height;
//                label.onLeftClick += label_onLeftClick;
//                scrollView.AddChild(label);
//            }
//            scrollView.ContentHeight = yPos + Spacing;
//        }

//        void label_onLeftClick(object sender, EventArgs e)
//        {
//            UILabel label = (UILabel)sender;
//            Editor.Load(label.Text);
//            this.Visible = false;
//        }

//        void bCancel_onLeftClick(object sender, EventArgs e)
//        {
//            this.Visible = false;
//        }

//        public override void Update()
//        {
//            this.CenterToParent();
//            base.Update();
//        }
//    }
//}
