using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;

namespace HEROsMod.UIKit
{
	internal class UIButton : UIView
	{
		public static Asset<Texture2D> buttonBackground;
		private static Texture2D buttonFill;

		public static Texture2D ButtonFill
		{
			get
			{
				if (buttonFill == null)
				{
					Color[] edgeColors = new Color[buttonBackground.Value.Width * buttonBackground.Value.Height];
					buttonBackground.Value.GetData(edgeColors);
					Color[] fillColors = new Color[buttonBackground.Value.Height];
					for (int y = 0; y < fillColors.Length; y++)
					{
						fillColors[y] = edgeColors[buttonBackground.Value.Width - 1 + y * buttonBackground.Value.Width];
					}
					buttonFill = new Texture2D(UIView.graphics, 1, fillColors.Length);
					buttonFill.SetData(fillColors);
				}
				return buttonFill;
			}
		}

		private Color hoverColor = new Color(38, 42, 120);
		private Color drawColor;

		private UILabel label = new UILabel("");

		public string Text
		{
			get { return label.Text; }
			set
			{
				label.Text = value;
				label.Anchor = AnchorPosition.Center;
				ScaleText();
				label.CenterToParent();
				label.Position = new Vector2(label.Position.X, label.Position.Y + 2);
			}
		}

		public bool AutoSize { get; set; }

		public UIButton(string text)
		{
			AutoSize = true;
			this.AddChild(label);
			this.Text = text;
			this.BackgroundColor = new Color(28, 32, 119);
			drawColor = BackgroundColor;
			this.onMouseEnter += new EventHandler(UIButton_onMouseEnter);
			this.onMouseLeave += new EventHandler(UIButton_onMouseLeave);
		}

		public UIButton(string text, Color backgroundColor, Color hoverColor)
		{
			AutoSize = true;
			this.AddChild(label);
			this.Text = text;
			this.BackgroundColor = backgroundColor;
			drawColor = BackgroundColor;
			this.hoverColor = hoverColor;
			this.onMouseEnter += new EventHandler(UIButton_onMouseEnter);
			this.onMouseLeave += new EventHandler(UIButton_onMouseLeave);
		}
		public void SetBackgroundColor(Color color)
		{
			BackgroundColor = color;
			drawColor = BackgroundColor;
		}

		public void SetTextColor(Color color)
		{
			label.ForegroundColor = color;
		}

		private void UIButton_onMouseLeave(object sender, EventArgs e)
		{
			drawColor = BackgroundColor;
		}

		private void UIButton_onMouseEnter(object sender, EventArgs e)
		{
			drawColor = hoverColor;
		}

		private float width = 0;

		protected override float GetWidth()
		{
			if (AutoSize)
			{
				return label.Width + buttonBackground.Value.Width * 2 + 30;
			}
			else
			{
				return width;
			}
		}

		protected override void SetWidth(float width)
		{
			this.width = width;

			ScaleText();
			label.CenterToParent();
			label.Position = new Vector2(label.Position.X, label.Position.Y + 2);
		}

		protected override float GetHeight()
		{
			return buttonBackground.Value.Height;
		}

		private void ScaleText()
		{
			if (!AutoSize)
			{
				Vector2 size = label.font.MeasureString(label.Text);
				if (size.X > width - (buttonBackground.Value.Width * 2 + 10))
				{
					label.Scale = (width - (buttonBackground.Value.Width * 2 + 10)) / size.X;
					if (size.Y * label.Scale > this.Height) label.Scale = this.Height / size.Y;
				}
				else label.Scale = this.Height / size.Y;
			}
			else label.Scale = this.Height / label.font.MeasureString(label.Text).Y;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(buttonBackground.Value, DrawPosition, null, drawColor * Opacity, 0f, Origin, 1f, SpriteEffects.None, 0f);
			int fillWidth = (int)Width - 2 * buttonBackground.Value.Width;
			Vector2 pos = DrawPosition;
			pos.X += buttonBackground.Value.Width;
			spriteBatch.Draw(ButtonFill, pos - Origin, null, drawColor * Opacity, 0f, Vector2.Zero, new Vector2(fillWidth, 1f), SpriteEffects.None, 0f);
			pos.X += fillWidth;
			spriteBatch.Draw(buttonBackground.Value, pos, null, drawColor * Opacity, 0f, Origin, 1f, SpriteEffects.FlipHorizontally, 0f);
			base.Draw(spriteBatch);
		}
	}
}