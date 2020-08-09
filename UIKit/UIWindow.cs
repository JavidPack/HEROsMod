//using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace HEROsMod.UIKit
{
	internal class UIWindow : UIView
	{
		private bool clickAndDrag = true;
		public bool ClickAndDrag { get { return clickAndDrag; } set { clickAndDrag = value; } }
		private bool dragging = false;
		private Vector2 dragAnchor = Vector2.Zero;
		private float width = 500;
		private float height = 300;
		private bool _constrainInsideParent = true;
		public bool CanMove = false;

		public UIWindow()
		{
			BackgroundColor = new Color(53, 35, 111, 255) * 0.685f;
			this.onMouseDown += new ClickEventHandler(UIWindow_onMouseDown);
			this.onMouseUp += new ClickEventHandler(UIWindow_onMouseUp);
		}

		private void UIWindow_onMouseUp(object sender, byte button)
		{
			if (dragging) dragging = false;
		}

		private void UIWindow_onMouseDown(object sender, byte button)
		{
			this.MoveToFront();
			if (CanMove)
			{
				if (button == 0)
				{
					dragging = true;
					dragAnchor = new Vector2(MouseX, MouseY) - DrawPosition;
				}
			}
		}

		protected override void SetWidth(float width)
		{
			this.width = width;
		}

		protected override void SetHeight(float height)
		{
			this.height = height;
		}

		protected override float GetHeight()
		{
			return height;
		}

		protected override float GetWidth()
		{
			return width;
		}

		public override void Update()
		{
			base.Update();
			if (dragging)
			{
				Position = new Vector2(MouseX, MouseY) - dragAnchor;
				if (_constrainInsideParent)
				{
					if (Position.X - Origin.X < 0) X = Origin.X;
					else if (Position.X + Width - Origin.X > Parent.Width) X = Parent.Width - Width + Origin.X;
					if (Y - Origin.Y < 0) Y = Origin.Y;
					else if (Y + Height - Origin.Y > Parent.Height) Y = Parent.Height - Height + Origin.Y;
				}
			}

			if (Visible && (IsMouseInside()/* || button.MouseInside*/))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				Main.player[Main.myPlayer].cursorItemIconEnabled = false;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
				Utils.DrawInvBG(spriteBatch, DrawPosition.X - Origin.X, DrawPosition.Y - Origin.Y, Width, Height, BackgroundColor);
			//spriteBatch.Draw(dummyTexture, new Rectangle((int)DrawPosition.X, (int)DrawPosition.Y, (int)Width, (int)Height), Color.Blue);
			base.Draw(spriteBatch);
		}
	}
}