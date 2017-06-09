using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HEROsMod.UIKit
{
	internal class UIListView : UIView
	{
		private List<UILabel> labels = new List<UILabel>();
		private List<string> items = new List<string>();
		public bool SelectableItems = true;
		private int selectedIndex = -1;
		public int SelectedIndex { get { return selectedIndex; } }

		public string[] Items
		{
			get { return items.ToArray(); }
		}

		private float width = 200;

		public UIListView()
		{
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
			float height = 0;
			if (labels.Count > 0)
			{
				height = labels[labels.Count - 1].Position.Y + labels[labels.Count - 1].Height;
			}
			return height;
		}

		public void AddItem(string text)
		{
			UILabel label = new UILabel(text);
			label.Tag = labels.Count;
			label.onLeftClick += label_onLeftClick;
			label.Scale = .5f;
			label.Position = new Vector2(0, Height);
			items.Add(text);
			labels.Add(label);
			this.AddChild(label);
		}

		public void ClearItems()
		{
			RemoveAllChildren();
			labels.Clear();
			items.Clear();
		}

		private void label_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			selectedIndex = (int)label.Tag;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (SelectableItems)
			{
				if (selectedIndex > -1)
				{
					UILabel label = labels[selectedIndex];
					Vector2 pos = label.DrawPosition;
					spriteBatch.Draw(ModUtils.DummyTexture, new Rectangle((int)pos.X, (int)pos.Y, (int)this.Width, (int)label.Height), Color.Pink);
				}
			}
			base.Draw(spriteBatch);
		}
	}
}