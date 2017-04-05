using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;

namespace HEROsMod.UIKit
{
    class UIDropdown : UIView
    {
        static Texture2D capUp = GetEmbeddedTexture("Images/UIKit/dropdownCapUp");
        static Texture2D capDown = GetEmbeddedTexture("Images/UIKit/dropdownCapDown");
        bool itemsShown = false;
        UIWindow itemsWindow = new UIWindow();
        int selectedItem = 0;
        public int SelectedItem
        {
            get { return selectedItem; }
            set 
            { 
                selectedItem = value;
                selectedLabel.Text = items[value];

            }
        }
        List<string> items = new List<string>();
        UILabel selectedLabel = new UILabel("");
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

        void UIDropdown_selectedChanged(object sender, EventArgs e)
        {
            MouseLeftButton = false;
        }

        void UIDropdown_onLeftClick(object sender, EventArgs e)
        {
            this.MoveToFront();
            ToggleShowingItems();
        }

        void ToggleShowingItems()
        {
            if (itemsShown) HideItems();
            else ShowItems();
        }

        void ShowItems()
        {
            itemsWindow.Visible = true;
            itemsShown = true;
        }

        void HideItems()
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

        void bg_onMouseLeave(object sender, EventArgs e)
        {
            UIRect rect = (UIRect)sender;
            rect.ForegroundColor = Color.White * 0f;
        }

        void bg_onMouseEnter(object sender, EventArgs e)
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

        void label_onLeftClick(object sender, EventArgs e)
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
            else return UIButton.buttonBackground.Height;
        }

        public override void Update()
        {
            
            if(itemsShown &&! itemsWindow.MouseInside && MouseLeftButton)
            {
                HideItems();
            }
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            itemsWindow.Draw(spriteBatch);
            spriteBatch.Draw(UIButton.buttonBackground, DrawPosition, null, BackgroundColor, 0f, Origin, 1f, SpriteEffects.None, 0f);
            int fillWidth = (int)Width - 2 * UIButton.buttonBackground.Width;
            Vector2 pos = DrawPosition;
            pos.X += UIButton.buttonBackground.Width;
            spriteBatch.Draw(UIButton.ButtonFill, pos - Origin, null, BackgroundColor, 0f, Vector2.Zero, new Vector2(fillWidth, 1f), SpriteEffects.None, 0f);
            pos.X += fillWidth;
            spriteBatch.Draw(UIButton.buttonBackground, pos, null, BackgroundColor, 0f, Origin, 1f, SpriteEffects.FlipHorizontally, 0f);
            if (itemsWindow.Visible)
                spriteBatch.Draw(capUp, new Vector2(DrawPosition.X + Width - capUp.Width, DrawPosition.Y), Color.White);
            else spriteBatch.Draw(capDown, new Vector2(DrawPosition.X + Width - capUp.Width, DrawPosition.Y), Color.White);
            selectedLabel.Draw(spriteBatch);
        }
    }
}
