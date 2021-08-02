using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class MiscOptions : HEROsModService
	{
		private MiscOptionsWindow _miscOptionsHotbar;

		public MiscOptions()
		{
			IsHotbar = true;

			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/settings", AssetRequestMode.ImmediateLoad)/*Main.buffTexture[BuffID.Confused]*/);
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("MiscOptions");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

			_miscOptionsHotbar = new MiscOptionsWindow();
			_miscOptionsHotbar.HotBarParent = HEROsMod.ServiceHotbar;
			_miscOptionsHotbar.Hide();
			this.AddUIView(_miscOptionsHotbar);

			Hotbar = _miscOptionsHotbar;
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			//Main.NewText("Toggle Hotbar");

			//_testHotbarWindow.Visible = !_testHotbarWindow.Visible;
			if (_miscOptionsHotbar.selected)
			{
				_miscOptionsHotbar.selected = false;
				_miscOptionsHotbar.Hide();
				//	Main.NewText("Hide Hotbar");

				//uIImage.ForegroundColor = buttonUnselectedColor;
			}
			else
			{
				//DisableAllWindows();
				_miscOptionsHotbar.selected = true;
				_miscOptionsHotbar.Show();
				//Main.NewText("Show Hotbar");

				//uIImage.ForegroundColor = buttonSelectedColor;
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ToggleBannedItems") ||
			HEROsModNetwork.LoginService.MyGroup.HasPermission("RevealMap") ||
			HEROsModNetwork.LoginService.MyGroup.HasPermission("LightHack") ||
			HEROsModNetwork.LoginService.MyGroup.IsAdmin ||
			HEROsModNetwork.LoginService.MyGroup.HasPermission("ToggleHardmodeEnemies") ||
			HEROsModNetwork.LoginService.MyGroup.HasPermission("ToggleGravestones");
			if (!HasPermissionToUse)
			{
				_miscOptionsHotbar.Hide();
			}

			//base.MyGroupUpdated();
		}
	}

	internal class MiscOptionsWindow : UIHotbar
	{
		//public UIImage bStampTiles;
		//public UIImage bEyeDropper;
		//public UIImage bFlipHorizontal;
		//public UIImage bFlipVertical;
		//public UIImage bToggleTransparentSelection;

		public MiscOptionsWindow()
		{
			this.buttonView = new UIView();
			//base.Visible = false;
			//bStampTiles = new UIImage(Main.itemTexture[ItemID.Paintbrush]);
			//bEyeDropper = new UIImage(Main.itemTexture[ItemID.EmptyDropper]);
			//bFlipHorizontal = new UIImage(Main.itemTexture[ItemID.PadThai]);
			//bFlipVertical = new UIImage(Main.itemTexture[ItemID.Safe]);
			//bToggleTransparentSelection = new UIImage(Main.buffTexture[BuffID.Invisibility]);
			//bStampTiles.Tooltip = "    Paint Tiles";
			//bEyeDropper.Tooltip = "    Eye Dropper";
			//bFlipHorizontal.Tooltip = "    Flip Horizontal";
			//bFlipVertical.Tooltip = "    Flip Vertical";
			//bToggleTransparentSelection.Tooltip = "    Toggle Transparent Selection: On";
			//buttonView.AddChild(bStampTiles);
			//buttonView.AddChild(bEyeDropper);
			//buttonView.AddChild(bFlipHorizontal);
			//buttonView.AddChild(bFlipVertical);
			//buttonView.AddChild(bToggleTransparentSelection);

			//base.Width = 200f;
			Height = 55f;
			UpdateWhenOutOfBounds = true;
			this.buttonView.Height = base.Height;
			base.Anchor = AnchorPosition.Top;
			this.AddChild(this.buttonView);
			//base.Position = new Vector2(Position.X, this.hiddenPosition);
			//base.CenterXAxisToParentCenter();
			float num = this.spacing;
			for (int i = 0; i < this.buttonView.children.Count; i++)
			{
				this.buttonView.children[i].Anchor = AnchorPosition.Left;
				this.buttonView.children[i].Position = new Vector2(num, 0f);
				this.buttonView.children[i].CenterYAxisToParentCenter();
				this.buttonView.children[i].Visible = true;
				//this.buttonView.children[i].ForegroundColor = buttonUnselectedColor;
				num += this.buttonView.children[i].Width + this.spacing;
			}
			this.Resize();
		}

		public override void RefreshHotbar()
		{
			//ModUtils.DebugText("TEST " + buttonView.ChildCount);
			//base.Width = 200f;
			//base.Height = 55f;
			//this.buttonView.Height = base.Height;
			//base.Anchor = AnchorPosition.Top;
			//this.AddChild(this.buttonView);
			//base.Position = new Vector2(Position.X, this.hiddenPosition);
			base.CenterXAxisToParentCenter();
			float num = this.spacing;
			for (int i = 0; i < this.buttonView.children.Count; i++)
			{
				this.buttonView.children[i].Anchor = AnchorPosition.Left;
				this.buttonView.children[i].Position = new Vector2(num, 0f);
				this.buttonView.children[i].CenterYAxisToParentCenter();
				this.buttonView.children[i].Visible = true;
				//this.buttonView.children[i].ForegroundColor = buttonUnselectedColor;
				num += this.buttonView.children[i].Width + this.spacing;
			}
			this.Resize();
			//buttonView.BackgroundColor = Color.Pink;
			//buttonView.ForegroundColor = Color.Red;
			//base.Visible = false;
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

		//public void Hide()
		//{
		//	hidden = true;
		//	arrived = false;
		//}

		//public void Show()
		//{
		//	arrived = false;
		//	hidden = false;
		//	Visible = true;
		//}
	}
}