//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class SaveSceneDialog : UIWindow
//    {
//        public delegate void SaveEvent(string fileName);
//        public event SaveEvent FileSaved;

//        UITextbox textbox = new UITextbox();
//        public SaveSceneDialog()
//        {
//            this.Anchor = AnchorPosition.Center;
//            UILabel label = new UILabel("Enter Scene Name");
//            textbox = new UITextbox();
//            UIButton bSave = new UIButton("Save");
//            UIButton bCancel = new UIButton("Cancel");

//            label.Scale = .4f;
//            label.X = LargeSpacing;
//            label.Y = LargeSpacing;

//            textbox.X = label.X;
//            textbox.Y = label.Y + label.Height + LargeSpacing;
//            textbox.OnEnterPress += textbox_OnEnterPress;

//            this.Width = textbox.Width + LargeSpacing * 2;

//            bCancel.X = this.Width - bCancel.Width - LargeSpacing;
//            bCancel.Y = textbox.Y + textbox.Height + LargeSpacing;
//            bCancel.onLeftClick += bCancel_onLeftClick;

//            bSave.X = bCancel.X - bSave.Width - Spacing;
//            bSave.Y = bCancel.Y;
//            bSave.onLeftClick += bSave_onLeftClick;

//            this.Height = bSave.Y + bSave.Height + LargeSpacing;

//            AddChild(label);
//            AddChild(textbox);
//            AddChild(bSave);
//            AddChild(bCancel);
//        }

//        void textbox_OnEnterPress(object sender, EventArgs e)
//        {
//            Save();
//        }

//        private void Save()
//        {
//            if (textbox.Text.Length == 0)
//            {
//                return;
//            }
//            string[] fileNames = Editor.GetListOfSaves();
//            foreach (var fileName in fileNames)
//            {
//                if (textbox.Text + ".scene" == fileName)
//                {
//                    UIMessageBox mb = new UIMessageBox("This file already exists, would you like to overwrite?", UIMessageBoxType.YesNo, true);
//                    this.Parent.AddChild(mb);
//                    mb.yesClicked += mb_yesClicked;
//                    return;
//                }
//            }
//            Editor.Save(textbox.Text);
//            this.Visible = false;
//            this.textbox.Text = string.Empty;
//        }

//        void bSave_onLeftClick(object sender, EventArgs e)
//        {
//            Save();
//        }

//        void mb_yesClicked(object sender, EventArgs e)
//        {
//            textbox.Unfocus();
//            Editor.Save(textbox.Text);
//            this.Visible = false;
//            this.textbox.Text = string.Empty;
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
