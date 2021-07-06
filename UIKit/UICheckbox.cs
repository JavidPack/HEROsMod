using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;

namespace HEROsMod.UIKit
{
	internal class UICheckbox : UIView
	{
		internal static Asset<Texture2D> checkboxTexture;
		internal static Asset<Texture2D> checkmarkTexture;

		private bool selected = false;

		public bool Selected
		{
			get { return selected; }
			set
			{
				if (value != selected)
				{
					selected = value;
					if (SelectedChanged != null)
						SelectedChanged(this, EventArgs.Empty);
				}
			}
		}

		public string Text
		{
			get { return label.Text; }
			set { label.Text = value; }
		}

		public event EventHandler SelectedChanged;

		private const int spacing = 4;

		private UILabel label;

		public UICheckbox(string text)
		{
			label = new UILabel(text);
			label.Scale = .5f;
			label.Position = new Vector2(checkboxTexture.Value.Width + spacing, 0);
			this.AddChild(label);
			this.onLeftClick += new EventHandler(UICheckbox_onLeftClick);
		}

		private void UICheckbox_onLeftClick(object sender, EventArgs e)
		{
			this.Selected = !Selected;
		}

		protected override float GetHeight()
		{
			return label.Height;
		}

		protected override float GetWidth()
		{
			return checkboxTexture.Value.Width + spacing + label.Width;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 pos = DrawPosition + new Vector2(0, (float)label.Height / 2 - (float)checkboxTexture.Value.Height / 1.2f);
			spriteBatch.Draw(checkboxTexture.Value, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
			if (Selected)
				spriteBatch.Draw(checkmarkTexture.Value, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);

			base.Draw(spriteBatch);
		}
	}
}