using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace HEROsMod.HEROsModServices
{
	internal class TestHotbarService : HEROsModService
	{
		private TestHotbarWindow _testHotbarWindow;

		public TestHotbarService()
		{
			this._hotbarIcon = new UIImage(TextureAssets.Buff[3]);
			this.HotbarIcon.Tooltip = "Test Hotbar";
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

			_testHotbarWindow = new TestHotbarWindow();
			_testHotbarWindow.HotBarParent = HEROsMod.ServiceHotbar;
			_testHotbarWindow.Hide();
			this.AddUIView(_testHotbarWindow);
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			//Main.NewText("Toggle Hotbar");

			//_testHotbarWindow.Visible = !_testHotbarWindow.Visible;
			if (_testHotbarWindow.selected)
			{
				_testHotbarWindow.selected = false;
				_testHotbarWindow.Hide();
				//Main.NewText("Hide Hotbar");

				//uIImage.ForegroundColor = buttonUnselectedColor;
			}
			else
			{
				//DisableAllWindows();
				_testHotbarWindow.selected = true;
				_testHotbarWindow.Show();
				//Main.NewText("Show Hotbar");

				//uIImage.ForegroundColor = buttonSelectedColor;
			}
		}
	}

	internal class TestHotbarWindow : UIHotbar
	{
		public UIView buttonView;
		public UIImage bStampTiles;
		public UIImage bEyeDropper;
		public UIImage bFlipHorizontal;
		public UIImage bFlipVertical;
		public UIImage bToggleTransparentSelection;

		public TestHotbarWindow()
		{
			this.buttonView = new UIView();
			base.Visible = false;
			Main.instance.LoadItem(ItemID.Paintbrush);
			Main.instance.LoadItem(ItemID.EmptyDropper);
			Main.instance.LoadItem(ItemID.PadThai);
			Main.instance.LoadItem(ItemID.Safe);
			bStampTiles = new UIImage(TextureAssets.Item[ItemID.Paintbrush]);
			bEyeDropper = new UIImage(TextureAssets.Item[ItemID.EmptyDropper]);
			bFlipHorizontal = new UIImage(TextureAssets.Item[ItemID.PadThai]);
			bFlipVertical = new UIImage(TextureAssets.Item[ItemID.Safe]);
			bToggleTransparentSelection = new UIImage(TextureAssets.Buff[BuffID.Invisibility]);
			bStampTiles.Tooltip = "    Paint Tiles";
			bEyeDropper.Tooltip = "    Eye Dropper";
			bFlipHorizontal.Tooltip = "    Flip Horizontal";
			bFlipVertical.Tooltip = "    Flip Vertical";
			bToggleTransparentSelection.Tooltip = "    Toggle Transparent Selection: On";
			buttonView.AddChild(bStampTiles);
			buttonView.AddChild(bEyeDropper);
			buttonView.AddChild(bFlipHorizontal);
			buttonView.AddChild(bFlipVertical);
			buttonView.AddChild(bToggleTransparentSelection);
			base.Width = 200f;
			base.Height = 55f;
			this.buttonView.Height = base.Height;
			base.Anchor = AnchorPosition.Top;
			this.AddChild(this.buttonView);
			base.Position = new Vector2(Position.X, this.hiddenPosition);
			base.CenterXAxisToParentCenter();
			float num = this.spacing;
			for (int i = 0; i < this.buttonView.children.Count; i++)
			{
				this.buttonView.children[i].Anchor = AnchorPosition.Left;
				this.buttonView.children[i].Position = new Vector2(num, 0f);
				this.buttonView.children[i].CenterYAxisToParentCenter();
				this.buttonView.children[i].Visible = true;
				this.buttonView.children[i].ForegroundColor = buttonUnselectedColor;
				num += this.buttonView.children[i].Width + this.spacing;
			}
			this.Resize();
		}

		public void Resize()
		{
			float num = this.spacing;
			for (int i = 0; i < this.buttonView.children.Count; i++)
			{
				if (this.buttonView.children[i].Visible)
				{
					this.buttonView.children[i].X = num;
					num += this.buttonView.children[i].Width + this.spacing;
				}
			}
			base.Width = num;
			this.buttonView.Width = base.Width;
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
	}
}