using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using System.Collections.Generic;

namespace HEROsMod.UIKit
{
	internal class UIWrappingLabel : UIView
	{
		private DynamicSpriteFont font = UILabel.defaultFont;
		private float width = 200;
		private List<UILabel> labels = new List<UILabel>();
		private string text;

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				SetLabels();
			}
		}

		public UIWrappingLabel()
		{
			Scale = .5f;
		}

		public UIWrappingLabel(string text, float width)
		{
			this.Width = width;
			Scale = .5f;
			this.Text = text;
		}

		private void SetLabels()
		{
			for (int i = 0; i < labels.Count; i++)
			{
				RemoveChild(labels[i]);
			}
			labels.Clear();
			if (Text.Length > 0)
			{
				string[] words = Text.Split(' ');
				UILabel currentLabel = null;
				for (int i = 0; i < words.Length; i++)
				{
					Vector2 wordSize = font.MeasureString(words[i] + " ") * Scale;
					if (currentLabel == null || currentLabel.Width + wordSize.X > Width)
					{
						currentLabel = new UILabel();
						currentLabel.Scale = Scale;
						currentLabel.font = font;
						currentLabel.Position = new Vector2(0, labels.Count * currentLabel.Height);
						labels.Add(currentLabel);
						AddChild(currentLabel);
					}
					currentLabel.Text += words[i];
					if (i != words.Length - 1) currentLabel.Text += " ";
				}
			}
		}

		protected override float GetWidth()
		{
			return width;
		}

		protected override void SetWidth(float width)
		{
			this.width = width;
		}

		protected override float GetHeight()
		{
			float result = 0;
			for (int i = 0; i < labels.Count; i++)
			{
				result += labels[i].Height;
			}
			return result;
		}
	}
}