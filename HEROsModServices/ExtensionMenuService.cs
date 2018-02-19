using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace HEROsMod.HEROsModServices
{
	internal class ExtensionMenuService : HEROsModService
	{
		private ExtensionMenuWindow _extensionMenuHotbar;

		private List<GenericExtensionService> genericServices;

		public ExtensionMenuService()
		{
			genericServices = new List<GenericExtensionService>();

			IsHotbar = true;

			this._hotbarIcon = new UIImage(HEROsMod.instance.GetTexture("Images/extensions"));
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("ExtensionTools");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

			_extensionMenuHotbar = new ExtensionMenuWindow();
			_extensionMenuHotbar.HotBarParent = HEROsMod.ServiceHotbar;
			_extensionMenuHotbar.Hide();
			this.AddUIView(_extensionMenuHotbar);

			Hotbar = _extensionMenuHotbar;
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			bool childAvailable = false;
			foreach (var item in genericServices)
			{
				childAvailable |= item.HasPermissionToUse;
			}
			if (childAvailable)
			{
				if (_extensionMenuHotbar.selected)
				{
					_extensionMenuHotbar.selected = false;
					_extensionMenuHotbar.Hide();
				}
				else
				{
					_extensionMenuHotbar.selected = true;
					_extensionMenuHotbar.Show();
				}
			}
			else
			{
				Main.NewText(HEROsMod.HeroText("NoExtensionsLoadedNote"));
			}
		}

		public override void MyGroupUpdated()
		{
			bool childAvailable = false;
			foreach (var item in genericServices)
			{
				item.MyGroupUpdated();
				childAvailable |= item.HasPermissionToUse;
			}
			HasPermissionToUse = childAvailable;
			if (!HasPermissionToUse)
			{
				_extensionMenuHotbar.Hide();
			}
		}

		internal void AddGeneric(GenericExtensionService genericService)
		{
			genericServices.Add(genericService);
		}
	}

	internal class ExtensionMenuWindow : UIHotbar
	{
		public ExtensionMenuWindow()
		{
			this.buttonView = new UIView();
			base.Visible = false;

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
				num += this.buttonView.children[i].Width + this.spacing;
			}
			this.Resize();
		}

		public override void test()
		{
			base.CenterXAxisToParentCenter();
			float num = this.spacing;
			for (int i = 0; i < this.buttonView.children.Count; i++)
			{
				this.buttonView.children[i].Anchor = AnchorPosition.Left;
				this.buttonView.children[i].Position = new Vector2(num, 0f);
				this.buttonView.children[i].CenterYAxisToParentCenter();
				this.buttonView.children[i].Visible = true;
				num += this.buttonView.children[i].Width + this.spacing;
			}
			this.Resize();
		}

		public override void Update()
		{
			DoSlideMovement();
			base.Update();
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
	}
}