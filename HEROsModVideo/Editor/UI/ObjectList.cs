//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.GameikiVideo.Editor.WorldObjects;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class ObjectList: UIWindow
//    {
//        UIScrollView scrollView;
//        public ObjectList()
//        {
//            this.Width = 350;
//            this.Height = 350;
//            this.CanMove = true;

//            UILabel lTitle = new UILabel("Object List");
//            lTitle.Scale = .4f;
//            lTitle.X = LargeSpacing;
//            lTitle.Y = LargeSpacing;

//            UIImage bClose = new UIImage(closeTexture);
//            bClose.X = this.Width - bClose.Width - LargeSpacing;
//            bClose.Y = LargeSpacing;
//            bClose.onLeftClick += bClose_onLeftClick;

//            scrollView = new UIScrollView();
//            scrollView.X = lTitle.X;
//            scrollView.Y = lTitle.Y + lTitle.Height + LargeSpacing;
//            scrollView.Width = this.Width - LargeSpacing * 2;
//            scrollView.Height = this.Height - scrollView.Y - LargeSpacing;

//            Refresh();

//            AddChild(lTitle);
//            AddChild(bClose);
//            AddChild(scrollView);
//        }

//        public void Refresh()
//        {
//            scrollView.ClearContent();
//            float yPos = Spacing;
//            foreach(WorldObject obj in Editor.WorldObjects)
//            {

//                UICheckbox cb = new UICheckbox(obj.GetType().Name);
//                //label.Scale = .4f;
//                cb.X = Spacing;
//                cb.Y = yPos;
//                cb.SelectedChanged += cb_SelectedChanged;
//                cb.Tag = obj;
//                scrollView.AddChild(cb);

//                UILabel bDelete = new UILabel("D");
//                bDelete.Scale = .4f;
//                bDelete.X = scrollView.Width - 20 - bDelete.Width - LargeSpacing;
//                bDelete.Y = cb.Y;
//                bDelete.Tag = obj;
//                bDelete.onLeftClick += bDelete_onLeftClick;
//                bDelete.Tooltip = "Delete";
//                scrollView.AddChild(bDelete);

//                UILabel bGoto = new UILabel("G");
//                bGoto.Scale = .4f;
//                cb.Selected = obj.Visible;
//                bGoto.X = bDelete.X - bGoto.Width - Spacing;
//                bGoto.Y = bDelete.Y;
//                bGoto.Tag = obj;
//                bGoto.onLeftClick += bGoto_onLeftClick;
//                bGoto.Tooltip = "Go To";
//                scrollView.AddChild(bGoto);

//                UILabel bProperties = new UILabel("P");
//                bProperties.Scale = .4f;
//                bProperties.X = bGoto.X - bProperties.Width - Spacing;
//                bProperties.Y = bDelete.Y;
//                bProperties.Tag = obj;
//                bProperties.onLeftClick += bProperties_onLeftClick;
//                bProperties.Tooltip = "Properties";
//                scrollView.AddChild(bProperties);

//                yPos += cb.Height;

//            }
//            scrollView.ContentHeight = yPos + Spacing;
//        }

//        void bProperties_onLeftClick(object sender, EventArgs e)
//        {
//            UIView view = (UIView)sender;
//            Editor.PropertiesWindow.SetObject(view.Tag);
//        }

//        void cb_SelectedChanged(object sender, EventArgs e)
//        {
//            UICheckbox cb = (UICheckbox)sender;
//            WorldObject wo = (WorldObject)cb.Tag;
//            wo.Visible = cb.Selected;
//        }

//        void bGoto_onLeftClick(object sender, EventArgs e)
//        {
//            WorldObject wo = (WorldObject)((UILabel)sender).Tag;
//            Editor.MoveCameraTo(wo.Position);
//        }

//        void bDelete_onLeftClick(object sender, EventArgs e)
//        {
//            WorldObject wo = (WorldObject)((UILabel)sender).Tag;
//            Editor.DeleteWorldObject(wo);
//        }

//        void bClose_onLeftClick(object sender, EventArgs e)
//        {
//            this.Visible = false;
//        }
//    }
//}
