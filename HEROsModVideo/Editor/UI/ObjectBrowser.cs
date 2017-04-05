//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.GameikiVideo.Editor.WorldObjects;
//using System.Reflection;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class ObjectBrowser : UIWindow
//    {
//        public Type CreatingType { get; set; }
//        UIScrollView scrollView = new UIScrollView();
//        public ObjectBrowser()
//        {
//            this.CanMove = true;
//            this.Width = 350;
//            this.Height = 350;
//            CreatingType = null;

//            UILabel lTitle = new UILabel("Object Browser");
//            lTitle.Scale = .4f;
//            lTitle.X = LargeSpacing;
//            lTitle.Y = LargeSpacing;

//            UIImage bClose = new UIImage(closeTexture);
//            bClose.X = this.Width - bClose.Width - LargeSpacing;
//            bClose.Y = LargeSpacing;
//            bClose.onLeftClick += bClose_onLeftClick;

//            scrollView.X = LargeSpacing;
//            scrollView.Y = lTitle.Y + lTitle.Height + Spacing;
//            scrollView.Width = Width - LargeSpacing * 2;
//            scrollView.Height = this.Height - scrollView.Y - LargeSpacing;

//            string nameSpace = "GameikiMod.GameikiVideo.Editor.WorldObjects";
//            var types = from type in Assembly.GetExecutingAssembly().GetTypes()
//                        where type.IsClass && type.Namespace == nameSpace && type.Name != "WorldObject" && type.Name != "WOImage"
//                        select type;
//            var typeList = types.ToList();

//            float yPos = Spacing;
//            for(int i = 0; i < typeList.Count; i++)
//            {
//                if (!typeList[i].IsSubclassOf(typeof(WorldObject)))
//                    continue;
//                UILabel label = new UILabel(typeList[i].Name);
//                label.Tag = typeList[i];
//                label.Scale = .4f;
//                label.X = Spacing;
//                label.Y = yPos;
//                label.onLeftClick += label_onLeftClick;
//                yPos += label.Height;
//                scrollView.AddChild(label);
//            }
//            scrollView.ContentHeight = yPos;

//            AddChild(scrollView);
//            AddChild(bClose);
//            AddChild(lTitle);
//        }

//        void bClose_onLeftClick(object sender, EventArgs e)
//        {
//            this.Visible = false;
//        }

//        void label_onLeftClick(object sender, EventArgs e)
//        {
//            UILabel label = (UILabel)sender;
//            Type type = (Type)label.Tag;

//            CreatingType = type;
//        }
//    }
//}
