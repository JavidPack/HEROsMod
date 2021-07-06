using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;

namespace HEROsMod.UIKit
{
	internal class UIDropdown : UIView
	{
		internal static Asset<Texture2D> capUp;
		internal static Asset<Texture2D> capDown;
		private bool itemsShown = false;
		private UIWindow itemsWindow = new UIWindow();
		private int selectedItem = 0;

		public int SelectedItem
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value;
				selectedLabel.Text = items[value];
			}
		}

		private List<string> items = new List<string>();
		private UILabel selectedLabel = new UILabel("");
		public int ItemCount { get { return items.Count; } }
		public string Text { get { return selectedLabel.Text; } }

		public event EventHandler selectedChanged;

		public bool ItemsVisible { get { return itemsWindow.Visible; } }

		public UIDropdown()
		{
			this.UpdateWhenOutOfBounds = true;
			itemsWindow.UpdateWhenOutOfBounds = true;
			selectedLabel.ForegroundColor = Color.Black;
			selectedLabel.Scale = .5f;
			selectedLabel.X = 6;
			selectedLabel.TextOutline = false;
			itemsWindow.UpdateWhenOutOfBounds = true;
			itemsWindow.BackgroundColor = new Color(81, 91, 184);
			this.onLeftClick += UIDropdown_onLeftClick;
			AddChild(selectedLabel);
			AddChild(itemsWindow);
			itemsWindow.Visible = false;
			selectedChanged += UIDropdown_selectedChanged;
		}

		private void UIDropdown_selectedChanged(object sender, EventArgs e)
		{
			MouseLeftButton = false;
		}

		private void UIDropdown_onLeftClick(object sender, EventArgs e)
		{
			this.MoveToFront();
			ToggleShowingItems();
		}

		private void ToggleShowingItems()
		{
			if (itemsShown) HideItems();
			else ShowItems();
		}

		private void ShowItems()
		{
			itemsWindow.Visible = true;
			itemsShown = true;
		}

		private void HideItems()
		{
			itemsWindow.Visible = false;
			itemsShown = false;
		}

		public void AddItem(string item)
		{
			items.Add(item);
			if (itemsWindow.ChildCount == 0) selectedLabel.Text = item;
			UILabel label = new UILabel(item);
			UIRect bg = new UIRect();
			label.Scale = .4f;
			label.X = 8;
			bg.X = 3;
			bg.Y = Height + ((label.Height) * itemsWindow.ChildCount);
			label.Tag = itemsWindow.ChildCount;
			bg.Tag = label.Tag;
			label.onLeftClick += label_onLeftClick;
			bg.onLeftClick += label_onLeftClick;
			bg.Width = itemsWindow.Width - 6;
			bg.Height = label.Height;
			bg.ForegroundColor = Color.White * 0f;
			bg.onMouseEnter += bg_onMouseEnter;
			bg.onMouseLeave += bg_onMouseLeave;

			itemsWindow.Height = bg.Y + bg.Height + 8;
			bg.AddChild(label);
			itemsWindow.AddChild(bg);
		}

		private void bg_onMouseLeave(object sender, EventArgs e)
		{
			UIRect rect = (UIRect)sender;
			rect.ForegroundColor = Color.White * 0f;
		}

		private void bg_onMouseEnter(object sender, EventArgs e)
		{
			UIRect rect = (UIRect)sender;
			rect.ForegroundColor = Color.Black * .1f;
		}

		public string GetItem(int index)
		{
			return items[index];
		}

		public void ClearItems()
		{
			itemsWindow.RemoveAllChildren();
			selectedItem = 0;
			selectedLabel.Text = "";
			items.Clear();
		}

		private void label_onLeftClick(object sender, EventArgs e)
		{
			this.MoveToFront();
			UIView label = (UIView)sender;
			int tag = (int)label.Tag;
			selectedLabel.Text = items[tag];
			if (tag != selectedItem)
			{
				selectedItem = tag;
				if (selectedChanged != null)
					selectedChanged(this, new EventArgs());
			}
			HideItems();
			UIView.MouseLeftButton = false;
		}

		private float width = 0;

		protected override float GetWidth()
		{
			return width;
		}

		protected override void SetWidth(float width)
		{
			this.width = width;
			itemsWindow.Width = width;
		}

		protected override float GetHeight()
		{
			if (itemsWindow.Visible)
				return itemsWindow.Height;
			else return UIButton.buttonBackground.Value.Height;
		}

		public override void Update()
		{
			if (itemsShown && !itemsWindow.MouseInside && MouseLeftButton)
			{
				HideItems();
			}
			base.Update();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			itemsWindow.Draw(spriteBatch);
			spriteBatch.Draw(UIButton.buttonBackground.Value, DrawPosition, null, BackgroundColor, 0f, Origin, 1f, SpriteEffects.None, 0f);
			int fillWidth = (int)Width - 2 * UIButton.buttonBackground.Value.Width;
			Vector2 pos = DrawPosition;
			pos.X += UIButton.buttonBackground.Value.Width;
			spriteBatch.Draw(UIButton.ButtonFill, pos - Origin, null, BackgroundColor, 0f, Vector2.Zero, new Vector2(fillWidth, 1f), SpriteEffects.None, 0f);
			pos.X += fillWidth;
			spriteBatch.Draw(UIButton.buttonBackground.Value, pos, null, BackgroundColor, 0f, Origin, 1f, SpriteEffects.FlipHorizontally, 0f);
			if (itemsWindow.Visible)
				spriteBatch.Draw(capUp.Value, new Vector2(DrawPosition.X + Width - capUp.Value.Width, DrawPosition.Y), Color.White);
			else spriteBatch.Draw(capDown.Value, new Vector2(DrawPosition.X + Width - capUp.Value.Width, DrawPosition.Y), Color.White);
			selectedLabel.Draw(spriteBatch);
		}
	}
}