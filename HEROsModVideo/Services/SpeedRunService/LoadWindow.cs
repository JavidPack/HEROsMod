//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using Microsoft.Xna.Framework;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    class LoadWindow : UIWindow
//    {
//        public delegate void SettingsFileSelectedHandler(string fileName);
//        public event SettingsFileSelectedHandler SettingsFileSelected;
//        public event SettingsFileSelectedHandler FileSelectedForDeletion;
//        static float spacing = 16f;
//        UIScrollView scrollView;
//        public LoadWindow()
//        {

//            UIView.exclusiveControl = this;
//            this.Anchor = AnchorPosition.Center;

//            UILabel lSelectFile = new UILabel("Select File");
//            lSelectFile.X = spacing;
//            lSelectFile.Y = spacing;
//            lSelectFile.Scale = .5f;

//            scrollView = new UIScrollView();
//            scrollView.Width = 300;
//            scrollView.Height = 250;
//            scrollView.X = lSelectFile.X;
//            scrollView.Y = lSelectFile.Y + lSelectFile.Height;
//            scrollView.ContentHeight = scrollView.Height;

//            UpdateFilesList();

//            UIButton bCancel = new UIButton("Cancel");
//            bCancel.AutoSize = false;
//            bCancel.Width = scrollView.Width;
//            bCancel.X = scrollView.X;
//            bCancel.Y = scrollView.Y + scrollView.Height + 8;
//            bCancel.onLeftClick += bCancel_onLeftClick;

//            this.Width = scrollView.Width + spacing * 2;
//            this.Height = bCancel.Y + bCancel.Height + 8;
//            AddChild(lSelectFile);
//            AddChild(scrollView);
//            AddChild(bCancel);
//        }

//        void UpdateFilesList()
//        {
//            string[] files = SpeedRunTimer.GetSavedSettingsFiles();
//            scrollView.ClearContent();
//            float yPos = 0;
//            for (int i = 0; i < files.Length; i++)
//            {
//                UILabel label = new UILabel(files[i]);
//                label.X = spacing;
//                label.Y = yPos;
//                label.Scale = .5f;
//                label.onLeftClick += label_onLeftClick;
//                yPos += label.Height;
//                scrollView.AddChild(label);

//                UIImage image = new UIImage(closeTexture);
//                image.ForegroundColor = Color.Red;
//                image.Anchor = AnchorPosition.Right;
//                image.Position = new Vector2(scrollView.Width - 10 - spacing, label.Position.Y + label.Height / 2);
//                image.Tag = label.Text;
//                image.onLeftClick += image_onLeftClick;
//                scrollView.AddChild(image);
//            }
//            if (scrollView.ChildCount > 0)
//            {
//                UIView lastChild = scrollView.GetLastChild();
//                scrollView.ContentHeight = lastChild.Y + lastChild.Height;
//            }
//        }

//        void image_onLeftClick(object sender, EventArgs e)
//        {
//            UIImage image = (UIImage)sender;
//            string fileName = (string)image.Tag;
//            if (FileSelectedForDeletion != null)
//                FileSelectedForDeletion(fileName);
//            UpdateFilesList();
//        }

//        void label_onLeftClick(object sender, EventArgs e)
//        {
//            UILabel label = (UILabel)sender;
//            if (SettingsFileSelected != null)
//                SettingsFileSelected(label.Text);
//            Close();
//        }

//        void bCancel_onLeftClick(object sender, EventArgs e)
//        {
//            Close();
//        }

//        void Close()
//        {
//            if (UIView.exclusiveControl == this) UIView.exclusiveControl = null;
//            this.Parent.RemoveChild(this);
//        }

//        public override void Update()
//        {
//            CenterToParent();
//            base.Update();
//        }
//    }
//}
