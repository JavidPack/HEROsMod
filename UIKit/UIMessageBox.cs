using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HEROsMod.UIKit
{
	internal enum UIMessageBoxType
	{
		Ok,
		YesNo
	}

	internal class UIMessageBox : UIWindow
	{
		private UIMessageBoxType MessageType = UIMessageBoxType.Ok;
		private UIWrappingLabel label = new UIWrappingLabel();
		private UIButton okButton = null;
		private UIButton yesButton = null;
		private UIButton noButton = null;

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

		private void AddButtons()
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

		private void yesButton_onLeftClick(object sender, EventArgs e)
		{
			if (this.yesClicked != null)
				yesClicked(this, EventArgs.Empty);
			if (this.Parent != null)
			{
				if (UIView.exclusiveControl == this) UIView.exclusiveControl = null;
				this.Parent.RemoveChild(this);
			}
		}

		private void noButton_onLeftClick(object sender, EventArgs e)
		{
			if (this.noClicked != null)
				noClicked(this, EventArgs.Empty);
			if (this.Parent != null)
			{
				if (UIView.exclusiveControl == this) UIView.exclusiveControl = null;
				this.Parent.RemoveChild(this);
			}
		}

		private void okButton_onLeftClick(object sender, EventArgs e)
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

		private void PositionButtons()
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