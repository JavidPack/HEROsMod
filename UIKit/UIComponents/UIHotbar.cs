using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace HEROsMod.UIKit.UIComponents
{
	internal class UIHotbar : UIWindow
	{
		internal RasterizerState _rasterizerState = new RasterizerState
		{
			ScissorTestEnable = true
		};

		internal static float slideMoveSpeed = 8f;
		internal float lerpAmount;

		//internal UIHotbar parentHotbar;
		internal UIView HotBarParent;

		internal float shownPosition
		{
			get
			{
				return (float)Main.screenHeight - base.Height * 2 - 12f + 6;
			}
		}

		internal float hiddenPosition
		{
			get
			{
				if (HotBarParent != null)
				{
					return (float)Main.screenHeight - base.Height - 12f;
				}
				//else if (mod.hotbar != null && !mod.hotbar.hidden && hidden)
				//{
				//	return (float)Main.screenHeight - base.Height - 12f;
				//}
				else
				{
					return (float)Main.screenHeight;
				}
			}
		}

		public void Hide()
		{
			hidden = true;
			arrived = false;
		}

		public void Show()
		{
			arrived = false;
			hidden = false;
			Visible = true;
		}

		public virtual void RefreshHotbar()
		{
		}

		public override void Update()
		{
			DoSlideMovement();
			base.Update();
		}

		internal float spacing = 8f;

		public bool hidden;
		internal bool arrived;

		private bool _selected;

		internal bool selected
		{
			get { return _selected; }
			set
			{
				if (value == false)
				{
					hidden = true;
				}
				else
				{
					hidden = false;
					Visible = true;
					//HotBarParent
					if (HEROsMod.ServiceHotbar.HotBarChild != null && HEROsMod.ServiceHotbar.HotBarChild != this)
					{
						HEROsMod.ServiceHotbar.HotBarChild.selected = false;
					}
					this.CenterXAxisToParentCenter();
					HEROsMod.ServiceHotbar.HotBarChild = this;
				}
				arrived = false;
				_selected = value;
			}
		}

		public UIView buttonView;
		internal static Color buttonUnselectedColor = Color.LightSkyBlue;
		internal static Color buttonSelectedColor = Color.White;
		internal static Color buttonSelectedHiddenColor = Color.Blue;

		internal void DoSlideMovement()
		{
			if (!arrived)
			{
				//Main.NewText("Not Arrived");

				if (this.hidden)
				{
					this.lerpAmount -= .01f * slideMoveSpeed;
					if (this.lerpAmount < 0f)
					{
						this.lerpAmount = 0f;
						arrived = true;
						//	Main.NewText("Arrived, Not Visible");
						this.Visible = false;
					}
					float y = MathHelper.SmoothStep(this.hiddenPosition, this.shownPosition, this.lerpAmount);
					base.Position = new Vector2(base.Position.X, y);
				}
				else
				{
					this.lerpAmount += .01f * slideMoveSpeed;
					if (this.lerpAmount > 1f)
					{
						this.lerpAmount = 1f;
						arrived = true;
						//	Main.NewText("Arrived, Visible");
					}
					float y2 = MathHelper.SmoothStep(this.hiddenPosition, this.shownPosition, this.lerpAmount);
					base.Position = new Vector2(base.Position.X, y2);
				}
			}
			else if(!hidden)
			{
				Position = new Vector2(Position.X, shownPosition);
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{
				spriteBatch.End();
				//spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, this._rasterizerState);
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, this._rasterizerState, null, Main.UIScaleMatrix);
				//	Rectangle scissorRectangle = new Rectangle((int)base.X- (int)base.Width, (int)base.Y, (int)base.Width, (int)base.Height);
				//Parent.Position.Y
				//		Main.NewText((int)Parent.Position.Y + " " + (int)shownPosition);
				//	Rectangle scissorRectangle = new Rectangle((int)(base.X - base.Width / 2), (int)(shownPosition), (int)base.Width, (int)base.Height);
				Rectangle scissorRectangle = new Rectangle((int)(base.X - base.Width / 2), (int)(shownPosition), (int)base.Width, (int)(HotBarParent.Position.Y - shownPosition));
				/*if (scissorRectangle.X < 0)
				{
					scissorRectangle.Width += scissorRectangle.X;
					scissorRectangle.X = 0;
				}
				if (scissorRectangle.Y < 0)
				{
					scissorRectangle.Height += scissorRectangle.Y;
					scissorRectangle.Y = 0;
				}
				if ((float)scissorRectangle.X + base.Width > (float)Main.screenWidth)
				{
					scissorRectangle.Width = Main.screenWidth - scissorRectangle.X;
				}
				if ((float)scissorRectangle.Y + base.Height > (float)Main.screenHeight)
				{
					scissorRectangle.Height = Main.screenHeight - scissorRectangle.Y;
				}*/
				scissorRectangle = ModUtils.GetClippingRectangle(spriteBatch, scissorRectangle);
				Rectangle scissorRectangle2 = spriteBatch.GraphicsDevice.ScissorRectangle;
				spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;

				base.Draw(spriteBatch);

				spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle2;
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
			}
			//	base.Draw(spriteBatch);
		}
	}
}