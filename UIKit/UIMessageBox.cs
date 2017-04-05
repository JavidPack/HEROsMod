using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace HEROsMod.UIKit
{
    enum UIMessageBoxType
    {
        Ok,
        YesNo
    }
    class UIMessageBox : UIWindow
    {
        UIMessageBoxType MessageType = UIMessageBoxType.Ok;
        UIWrappingLabel label = new UIWrappingLabel();
        UIButton okButton = null;
        UIButton yesButton = null;
        UIButton noButton = null;

        public event EventHandler yesClicked;
        public event EventHandler noClicked;

        public string Text
        {
            get { return label.Text; }
            set 
            { 
                label.Text = value;
                this.Height = label.Height + 70;
                PositionButtons();
            }
        }

        public UIMessageBox()
        {
            AddButtons();
            this.Text = "";
        }

        public UIMessageBox(string text, bool exclusiveControl = false)
        {
            AddButtons();
            this.Text = text;
            if (exclusiveControl) UIView.exclusiveControl = this;
        }



        public UIMessageBox(string text, UIMessageBoxType messageBoxType, bool exclusiveControl = false)
        {
            this.MessageType = messageBoxType;
            AddButtons();
            this.Text = text;

            if (exclusiveControl) UIView.exclusiveControl = this;
        }

        void AddButtons()
        {
            this.Anchor = AnchorPosition.Center;
            label.Anchor = AnchorPosition.Top;
            label.Width = Width - 30;
            label.Position = new Vector2(Width / 2, 10);
            AddChild(label);
            if (MessageType == UIMessageBoxType.Ok)
            {
                okButton = new UIButton("Ok");
                okButton.Anchor = AnchorPosition.BottomRight;
                AddChild(okButton);
                okButton.onLeftClick += new EventHandler(okButton_onLeftClick);
            }
            else if (MessageType == UIMessageBoxType.YesNo)
            {
                noButton = new UIButton("No");
                yesButton = new UIButton("Yes");
                noButton.Anchor = AnchorPosition.BottomRight;
                yesButton.Anchor = AnchorPosition.BottomRight;
                AddChild(noButton);
                AddChild(yesButton);
                noButton.onLeftClick += noButton_onLeftClick;
                yesButton.onLeftClick += yesButton_onLeftClick;
            }
        }

        void yesButton_onLeftClick(object sender, EventArgs e)
        {
            if (this.yesClicked != null)
                yesClicked(this, EventArgs.Empty);
            if (this.Parent != null)
            {
                if (UIView.exclusiveControl == this) UIView.exclusiveControl = null;
                this.Parent.RemoveChild(this);
            }
        }

        void noButton_onLeftClick(object sender, EventArgs e)
        {
            if (this.noClicked != null)
                noClicked(this, EventArgs.Empty);
            if (this.Parent != null)
            {
                if (UIView.exclusiveControl == this) UIView.exclusiveControl = null;
                this.Parent.RemoveChild(this);
            }
        }

        void okButton_onLeftClick(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                if (UIView.exclusiveControl == this) UIView.exclusiveControl = null;
                this.Parent.RemoveChild(this);
            }
        }

        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Parent != null)
                CenterToParent();
            base.Draw(spriteBatch);
        }

        void PositionButtons()
        {
            if (MessageType == UIMessageBoxType.Ok)
            {
                okButton.Position = new Vector2(Width - 8, Height - 8);
            }
            else if (MessageType == UIMessageBoxType.YesNo)
            {
                noButton.Position = new Vector2(Width - 8, Height - 8);
                yesButton.Position = new Vector2(noButton.Position.X - noButton.Width - 8, noButton.Position.Y);
            }
        }

        
    }
}
