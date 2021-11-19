using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;

namespace HEROsMod.UIKit
{
	internal enum AnchorPosition
	{
		Left,
		Right,
		Top,
		Bottom,
		Center,
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	internal class UIView
	{
		internal static Asset<Texture2D> closeTexture;

		//statics
		public static UIView exclusiveControl = null;

		public static bool GameMouseOverwritten = false;
		protected static int MouseX { get { return Main.mouseX; } }
		protected static int MouseY { get { return Main.mouseY; } }
		protected static bool MouseLeftButton = false;
		protected static bool MousePrevLeftButton = false;
		protected static bool MouseRightButton = false;
		protected static bool MousePrevRightButton = false;
		public static int ScrollAmount = 0;

		public static string HoverText = "";
		//public static Item HoverItem = new Item();
		protected static readonly Item EmptyItem = new Item();
		public static bool HoverOverridden = false;

		public static float SmallSpacing = 4f;
		public static float Spacing = 8f;
		public static float LargeSpacing = 16f;

		/*
        protected static bool mouseLeftButton { get { return Main.mouseLeft; } }
        protected static bool mousePrevLeftButton { get { return !Main.mouseLeftRelease; } }
        protected static bool mouseRightButton { get { return Main.mouseRight; } }
        protected static bool mousePrevRightButton { get { return !Main.mouseRightRelease; } }
        */
		protected static Texture2D dummyTexture { get { return ModUtils.DummyTexture; } }
		protected static GraphicsDevice graphics { get { return Main.graphics.GraphicsDevice; } }

		//public bool centerOrigin = false;

		//Position and Size Vars
		private Vector2 _position = Vector2.Zero;

		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public float X
		{
			get { return _position.X; }
			set { _position.X = value; }
		}

		public float Y
		{
			get { return _position.Y; }
			set { _position.Y = value; }
		}

		public Vector2 DrawPosition
		{
			get
			{
				if (Parent != null)
				{
					return Parent.DrawPosition + Position + Offset - Parent.Origin;
				}
				else return Position + Offset;
			}
		}

		public float Width { get { return GetWidth(); } set { SetWidth(value); } }
		public float Height { get { return GetHeight(); } set { SetHeight(value); } }

		//C
		protected bool mouseForChildrenHandled = false;

		public List<UIView> children = new List<UIView>();
		private List<UIView> childrenToRemove = new List<UIView>();
		public UIView Parent { get; set; }

		//Mouse Events
		public delegate void ClickEventHandler(object sender, byte button);

		public event EventHandler onHover;

		public event EventHandler onLeftClick;

		public event EventHandler onRightClick;

		public event EventHandler onMouseEnter;

		public event EventHandler onMouseLeave;

		public event ClickEventHandler onMouseDown;

		public event ClickEventHandler onMouseUp;

		private static bool mouseUpHandled = false;
		private static bool mouseDownHandled = false;

		protected bool leftButtonDown = false;
		protected bool rightButtonDown = false;

		//Mouse Variables
		private bool mousePreviouslyIn = false;

		public bool MouseInside { get { return IsMouseInside(); } }

		public int ChildCount { get { return children.Count; } }

		public Color ForegroundColor { get; set; } = Color.White;

		public Color BackgroundColor { get; set; } = Color.White;

		public AnchorPosition Anchor { get; set; } = AnchorPosition.TopLeft;

		public Vector2 Origin => GetOrigin();

		public Vector2 Offset { get; set; } = Vector2.Zero;

		public float Scale { get; set; } = 1f;

		private float opacity = 1f;

		public float Opacity
		{
			get
			{
				if (Parent != null)
				{
					return opacity * Parent.Opacity;
				}
				return opacity;
			}
			set { opacity = value; }
		}

		private bool _visible = true;

		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
		}

		private bool _overridesMouse = true;

		public bool OverridesMouse
		{
			get { return _overridesMouse; }
			set { _overridesMouse = value; }
		}

		private string _tooltip = "";

		public string Tooltip
		{
			get { return _tooltip; }
			set
			{
				if (value.Length > 0 && _tooltip.Length == 0)
				{
					//add event
					onHover += DisplayTooltip;
				}
				else if (value.Length == 0 && _tooltip.Length > 0)
				{
					//remove event
					onHover -= DisplayTooltip;
				}
				_tooltip = value;
			}
		}

		private bool _updateWhenOutOfBounds = false;

		public bool UpdateWhenOutOfBounds
		{
			get { return _updateWhenOutOfBounds; }
			set { _updateWhenOutOfBounds = value; }
		}

		public object Tag { get; set; }

		public virtual void Update()
		{
			if (Parent == null)
			{
				mouseDownHandled = false;
				mouseUpHandled = false;
				GameMouseOverwritten = false;
			}
			mouseForChildrenHandled = false;
			if (Visible)
			{
				for (int i = 0; i < children.Count; i++)
				{
					UIView child = children[children.Count - 1 - i];
					if (child.UpdateWhenOutOfBounds || child.InParent()) children[children.Count - 1 - i].Update();
				}
				while (childrenToRemove.Count > 0)
				{
					children.Remove(childrenToRemove[0]);
					childrenToRemove.RemoveAt(0);
				}

				if ((exclusiveControl == null && Parent == null) || this == exclusiveControl)
				{
					HandleMouseInput();
				}
			}
			/*
            if (Parent == null || Parent.IsMouseInside())
            {
                HandleMouseInput();
            }
             */
		}

		private void DisplayTooltip(object sender, EventArgs e)
		{
			HoverText = ((UIView)sender).Tooltip;
		}

		public static void OverWriteGameMouseInput()
		{
			GameMouseOverwritten = true;
			Main.mouseLeft = false;
			Main.mouseLeftRelease = false;
			Main.mouseRight = false;
			Main.mouseLeft = false;
			//Main.oldMouseState = Main.mouseState; // TODO?
			HoverOverridden = true;
		}

		private bool InParent()
		{
			float h = Parent.Height;

			return !((Position.Y + Offset.Y < 0 && Position.Y + Offset.Y + Height < 0) ||
				   (Position.Y + Offset.Y > h && Position.Y + Offset.Y + Height > h));
		}

		private void HandleMouseInput()
		{
			for (int i = 0; i < children.Count; i++)
			{
				UIView child = children[children.Count - 1 - i];
				if (child.Visible)
				{
					if (child.Parent == null || child.UpdateWhenOutOfBounds || child.mousePreviouslyIn || (child.InParent() && (child.Parent.MouseInside || child.Parent.UpdateWhenOutOfBounds) && !child.Parent.mouseForChildrenHandled))
					{
						child.HandleMouseInput();
					}
				}
			}

			if (this.OverridesMouse && MouseInside)
			{
				if (onMouseLeave != null)
				{
				}
				if (Parent != null)
				{
					Parent.mouseForChildrenHandled = true;
					if (this.OverridesMouse) OverWriteGameMouseInput();
				}
				onHover?.Invoke(this, new EventArgs());
				if (!mousePreviouslyIn)
				{
					onMouseEnter?.Invoke(this, new EventArgs());
				}
				if (!MousePrevLeftButton && MouseLeftButton)
				{
					leftButtonDown = true;
					if (onMouseDown != null && !mouseDownHandled)
					{
						onMouseDown(this, 0);
					}
				}
				if (MousePrevLeftButton && !MouseLeftButton)
				{
					if (onMouseUp != null && !mouseUpHandled)
					{
						onMouseUp(this, 0);
					}
					if (leftButtonDown && onLeftClick != null)
					{
						onLeftClick(this, EventArgs.Empty);
					}
				}
				if (!MousePrevRightButton && MouseRightButton)
				{
					rightButtonDown = true;
					onMouseDown?.Invoke(this, 1);
				}
				if (MousePrevRightButton && !MouseRightButton)
				{
					onMouseUp?.Invoke(this, 1);
					if (rightButtonDown && onRightClick != null)
					{
						onRightClick(this, EventArgs.Empty);
					}
				}
				mousePreviouslyIn = true;
			}
			else
			{
				if (onMouseLeave != null)
				{
				}
				if (mousePreviouslyIn)
				{
					onMouseLeave?.Invoke(this, new EventArgs());
				}
				mousePreviouslyIn = false;
			}

			if (!MouseLeftButton)
			{
				leftButtonDown = false;
			}
			if (!MouseRightButton)
				rightButtonDown = false;
		}

		private float width = 0;
		private float height = 0;

		protected virtual void SetWidth(float width)
		{
			this.width = width;
		}

		protected virtual void SetHeight(float height)
		{
			this.height = height;
		}

		protected virtual float GetWidth()
		{
			return width;
		}

		protected virtual float GetHeight()
		{
			return height;
		}

		protected virtual Vector2 GetOrigin()
		{
			float centerX = Width / 2;
			float centerY = Height / 2;
			if (Anchor == AnchorPosition.TopLeft)
				return Vector2.Zero;
			else if (Anchor == AnchorPosition.Left)
				return new Vector2(0, centerY);
			else if (Anchor == AnchorPosition.Right)
				return new Vector2(Width, centerY);
			else if (Anchor == AnchorPosition.Top)
				return new Vector2(centerX, 0);
			else if (Anchor == AnchorPosition.Bottom)
				return new Vector2(centerX, Height);
			else if (Anchor == AnchorPosition.Center)
				return new Vector2(centerX, centerY);
			else if (Anchor == AnchorPosition.TopRight)
				return new Vector2(Width, 0);
			else if (Anchor == AnchorPosition.BottomLeft)
				return new Vector2(0, Height);
			else if (Anchor == AnchorPosition.BottomRight)
				return new Vector2(Width, Height);
			return Vector2.Zero;
		}

		protected virtual bool IsMouseInside()
		{
			/*
            if (inScrollView != null)
            {
                if (!inScrollView.MouseInside)
                    return false;
            }
             */
			Vector2 pos = DrawPosition - Origin;
			if (MouseX >= pos.X && MouseX <= pos.X + Width &&
				MouseY >= pos.Y && MouseY <= pos.Y + Height)
			{
				return true;
			}
			return false;
		}

		protected virtual Vector2 GetParentCenter()
		{
			float w = Main.screenWidth;
			float h = Main.screenHeight;
			if (Parent != null)
			{
				w = Parent.Width;
				h = Parent.Height;
			}
			return new Vector2(w / 2, h / 2);
		}

		public void CenterToParent()
		{
			Position = GetParentCenter();
		}

		public void CenterXAxisToParentCenter()
		{
			Position = new Vector2(GetParentCenter().X, Position.Y);
		}

		public void CenterYAxisToParentCenter()
		{
			Position = new Vector2(Position.X, GetParentCenter().Y);
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{
				int prevChildCount = ChildCount;
				for (int i = 0; i < ChildCount; i++)
				{
					if (ChildCount != prevChildCount)
					{
						//Main.NewText("We broke");
						break;
					}
					UIView child = children[i];
					if (child.UpdateWhenOutOfBounds || child.InParent())
					{
						if (child.Visible) child.Draw(spriteBatch);
					}
				}
			}
		}

		public UIView GetChild(int index)
		{
			return children[index];
		}

		public UIView GetLastChild()
		{
			return children[children.Count - 1];
		}

		public virtual void AddChild(UIView view)
		{
			view.Parent = this;
			view.onMouseDown += new ClickEventHandler(view_onMouseDown);
			view.onMouseUp += new ClickEventHandler(view_onMouseUp);
			children.Add(view);
		}

		public void RemoveAllChildren()
		{
			children.Clear();
		}

		private void view_onMouseUp(object sender, byte button)
		{
			mouseUpHandled = true;
		}

		private void view_onMouseDown(object sender, byte button)
		{
			mouseDownHandled = true;
		}

		public void RemoveChild(UIView view)
		{
			childrenToRemove.Add(view);
			//children.Remove(view);
		}

		public void MoveToFront()
		{
			this.Parent.children.Remove(this);
			this.Parent.children.Add(this);
		}
	}
}