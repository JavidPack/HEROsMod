//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    class SaveWindow : UIWindow
//    {
//        public delegate void NameSubmitedEvent(string name);
//        public event NameSubmitedEvent NameSubmited;
//        UITextbox tbName;

//        static float spacing = 16f;
//        public SaveWindow()
//        {
//            UIView.exclusiveControl = this;
//            this.Anchor = AnchorPosition.Center;
//            UILabel lDescription = new UILabel("Save settings for later use.");
//            lDescription.Scale = .5f;
//            lDescription.X = spacing;
//            lDescription.Y = spacing;

//            UILabel lName = new UILabel("Name: ");
//            lName.Scale = .5f;
//            lName.X = lDescription.X;
//            lName.Y = lDescription.Y + lDescription.Height;

//            tbName = new UITextbox();
//            tbName.Width = lDescription.Width - lName.Width;
//            tbName.X = lName.X + lName.Width;
//            tbName.Y = lName.Y;
//            tbName.MaxCharacters = 24;


//            UIButton bSave = new UIButton("Save");
//            UIButton bCancel = new UIButton("Cancel");
//            bSave.Y = lName.Y + lName.Height + spacing;
//            bCancel.Y = bSave.Y;

//            this.Width = lDescription.Width + spacing * 2;
//            this.Height = bSave.Y + bSave.Height + spacing;

//            bCancel.X = this.Width - bCancel.Width - spacing;
//            bSave.X = bCancel.X - bSave.Width - spacing;

//            bSave.onLeftClick += bSave_onLeftClick;
//            bCancel.onLeftClick += bCancel_onLeftClick;
//            tbName.OnEnterPress += bSave_onLeftClick;

//            AddChild(lDescription);
//            AddChild(lName);
//            AddChild(tbName);
//            AddChild(bSave);
//            AddChild(bCancel);
//            tbName.Focus();
//        }

//        void bCancel_onLeftClick(object sender, EventArgs e)
//        {
//            Close();
//        }

//        void bSave_onLeftClick(object sender, EventArgs e)
//        {
//            if (tbName.Text.Length < 3)
//            {
//                Main.NewText("Settings name must be at least three characters long");
//                return;
//            }
//            if (NameSubmited != null)
//                NameSubmited(tbName.Text);
//            Close();
//        }

//        void Close()
//        {
//            tbName.Unfocus();
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
