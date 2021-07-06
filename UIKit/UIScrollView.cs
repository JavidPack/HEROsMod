using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameInput;

namespace HEROsMod.UIKit
{
	internal class UIScrollView : UIView
	{
		private RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
		internal static Asset<Texture2D> ScrollbgTexture;
		private static Texture2D scrollbgFill;

		private static Texture2D ScrollbgFill
		{
			get
			{
				if (scrollbgFill == null)
				{
					Color[] edgeColors = new Color[ScrollbgTexture.Value.Width * ScrollbgTexture.Value.Height];
					ScrollbgTexture.Value.GetData(edgeColors);
					Color[] fillColors = new Color[ScrollbgTexture.Value.Width];
					for (int x = 0; x < fillColors.Length; x++)
					{
						fillColors[x] = edgeColors[x + (ScrollbgTexture.Value.Height - 1) * ScrollbgTexture.Value.Width];
					}
					scrollbgFill = new Texture2D(UIView.graphics, fillColors.Length, 1);
					scrollbgFill.SetData(fillColors);
				}
				return scrollbgFill;
			}
		}

		protected UIScrollBar scrollBar = new UIScrollBar();
		private float width = 150;
		private float height = 250;
		private float contentHeight = 0;
		private bool dragging = false;
		private Vector2 dragAnchor = Vector2.Zero;
		public bool OverrideDrawAndUpdate = false;

		public float ContentHeight
		{
			get
			{
				return contentHeight;
				/*
                float result = contentHeight - Height;
                if (result < Height) result = Height;
                return result;
                 */
			}
			set { contentHeight = value; }
		}

		private float scrollPosition = 0;

		public float ScrollPosition
		{
			get
			{
				float result = scrollPosition;
				if (scrollPosition < 0 || Height > ContentHeight)
					result = 0;
				if (scrollPosition > ContentHeight) result = ContentHeight;
				return result;
			}
			set
			{
				if (value < 0) value = 0;
				if (value > ContentHeight) value = ContentHeight;
				scrollPosition = value;
				UpdateChildOffset();
			}
		}

		public UIScrollView()
		{
			scrollBar.onMouseDown += new ClickEventHandler(scrollBar_onMouseDown);
			this.AddChild(scrollBar);
		}

		private void scrollBar_onMouseDown(object sender, byte button)
		{
			if (button == 0)
			{
				dragging = true;
				dragAnchor = new Vector2(MouseX, MouseY) - scrollBar.DrawPosition;
			}
		}

		protected override float GetHeight()
		{
			return height;
		}

		protected override float GetWidth()
		{
			return width;
		}

		protected override void SetWidth(float width)
		{
			this.width = width;
		}

		protected override void SetHeight(float height)
		{
			this.height = height;
		}

		public override void AddChild(UIView view)
		{
			//view.SetInScrollView(this);
			if (children.Count > 0 && contentHeight > 0)
			{
				float dest = (ScrollPosition / ContentHeight) * (ContentHeight - Height);
				view.Offset = new Vector2(view.Offset.X, -dest);
			}
			base.AddChild(view);
		}

		public void ClearContent()
		{
			ScrollPosition = 0;
			if (children.Count > 1)
			{
				for (int i = 1; i < children.Count; i++)
				{
					RemoveChild(GetChild(i));
				}
			}
		}

		public override void Update()
		{
			if (OverrideDrawAndUpdate)
			{
				scrollBar.Update();
			}
			else base.Update();
			if (!MouseLeftButton) dragging = false;

			float sbHeight = Height / ContentHeight * Height;
			if (sbHeight < 20) sbHeight = 20;
			if (sbHeight > Height) sbHeight = Height;
			float scrollSpace = Height - sbHeight;
			if (dragging)
			{
				float mouseOffset = (MouseY - DrawPosition.Y + Origin.Y) - dragAnchor.Y;
				float thing = mouseOffset / scrollSpace;
				ScrollPosition = ContentHeight * thing;
			}
			else if (ScrollAmount != 0 && IsMouseInside())
			{
				ScrollPosition -= ScrollAmount;
				ModUtils.DebugText("Start");
				ModUtils.DebugText("ScrollAmmount!" + ScrollAmount);
				ModUtils.DebugText("PlayerInput.ScrollWheelDeltaForUI!" + PlayerInput.ScrollWheelDeltaForUI);
				ModUtils.DebugText("PlayerInput.ScrollWheelDelta!" + PlayerInput.ScrollWheelDelta);
				PlayerInput.ScrollWheelDelta = 0;
				PlayerInput.ScrollWheelDeltaForUI *= -1;

				foreach (UIView child in children)
				{
					if (child.GetType() != typeof(UIScrollBar))
					{
						float dest = (ScrollPosition / ContentHeight) * (ContentHeight - Height);
						child.Offset = new Vector2(child.Offset.X, -dest);
					}
				}
			}
			float y = ScrollPosition / ContentHeight * scrollSpace;
			this.scrollBar.Height = sbHeight;
			this.scrollBar.Position = new Vector2(this.Width - scrollBar.Width, y);
		}

		public void UpdateChildOffset()
		{
			foreach (UIView child in children)
			{
				if (child.GetType() != typeof(UIScrollBar))
				{
					float dest = (ScrollPosition / ContentHeight) * (ContentHeight - Height);
					child.Offset = new Vector2(child.Offset.X, -dest);
				}
			}
		}

		private void DrawScrollbg(SpriteBatch spriteBatch)
		{
			Vector2 pos = DrawPosition;
			float fillHeight = Height - ScrollbgTexture.Value.Height * 2;
			pos.X += Width - ScrollbgTexture.Value.Width;
			spriteBatch.Draw(ScrollbgTexture.Value, pos, null, Color.White * Opacity, 0f, Origin, 1f, SpriteEffects.None, 0f);
			pos.Y += ScrollbgTexture.Value.Height;
			spriteBatch.Draw(ScrollbgFill, pos - Origin, null, Color.White * Opacity, 0f, Vector2.Zero, new Vector2(1f, fillHeight), SpriteEffects.None, 0f);
			pos.Y += fillHeight;
			spriteBatch.Draw(ScrollbgTexture.Value, pos, null, Color.White * Opacity, 0f, Origin, 1f, SpriteEffects.FlipVertically, 0f);
		}

		/*
        void DrawScrollBar(SpriteBatch spriteBatch)
        {
            float sbHeight = Height / ContentHeight * Height;
            Vector2 pos = DrawPosition;
            float scrollSpace = Height - sbHeight;
            float y = scrollPosition / contentHeight * scrollSpace;
            pos.Y += y;
            float fillHeight = sbHeight - ScrollbarTexture.Height * 2;
            pos.X += Width - ScrollbarTexture.Width;
            spriteBatch.Draw(ScrollbarTexture, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
            pos.Y += ScrollbarTexture.Height;
            spriteBatch.Draw(ScrollbarFill, pos - Origin, null, Color.White, 0f, Vector2.Zero, new Vector2(1f, fillHeight), SpriteEffects.None, 0f);
            pos.Y += fillHeight;
            spriteBatch.Draw(ScrollbarTexture, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.FlipVertically, 0f);
        }
         */

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 pos = DrawPosition - Origin;
			Utils.DrawInvBG(spriteBatch, pos.X, pos.Y, Width, Height, new Color(33, 15, 91, 255) * (0.685f * Opacity));

			DrawScrollbg(spriteBatch);
			//DrawScrollBar(spriteBatch);
			if (pos.X <= Main.screenWidth && pos.Y <= Main.screenHeight && pos.X + Width >= 0 && pos.Y + Height >= 0)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, _rasterizerState, null, Main.UIScaleMatrix);

				Rectangle cutRect = new Rectangle((int)pos.X, (int)pos.Y, (int)Width, (int)Height);
				/*if (cutRect.X < 0)
				{
					cutRect.Width += cutRect.X;
					cutRect.X = 0;
				}
				if (cutRect.Y < 0)
				{
					cutRect.Height += cutRect.Y;
					cutRect.Y = 0;
				}
				if (cutRect.X + Width > Main.screenWidth) cutRect.Width = Main.screenWidth - cutRect.X;
				if (cutRect.Y + Height > Main.screenHeight) cutRect.Height = Main.screenHeight - cutRect.Y;*/
				cutRect = ModUtils.GetClippingRectangle(spriteBatch, cutRect);

				Rectangle currentRect = spriteBatch.GraphicsDevice.ScissorRectangle;
				spriteBatch.GraphicsDevice.ScissorRectangle = cutRect;

				if (OverrideDrawAndUpdate)
				{
					scrollBar.Draw(spriteBatch);
				}
				else base.Draw(spriteBatch);

				spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Main.UIScaleMatrix);
				scrollBar.Draw(spriteBatch);
			}
		}
	}
}