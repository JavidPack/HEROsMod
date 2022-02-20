using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class GroupInspector : HEROsModService
	{
		private GroupManagementWindow groupWindow;

		public GroupInspector(UIHotbar hotbar)
		{
			IsInHotbar = true;
			HotbarParent = hotbar;
			MultiplayerOnly = true;
			this._name = "Group Inspector";
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/manageGroups", AssetRequestMode.ImmediateLoad));
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("OpenGroupManagement");
			HEROsModNetwork.LoginService.GroupChanged += LoginService_GroupChanged;
		}

		private void LoginService_GroupChanged(object sender, EventArgs e)
		{
			if (groupWindow != null)
			{
				groupWindow.RefreshGroupList();
			}
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (groupWindow == null)
			{
				groupWindow = new GroupManagementWindow();
				groupWindow.Closed += groupWindow_Closed;
				this.AddUIView(groupWindow);
			}
			else
			{
				groupWindow.Close();
			}
		}

		private void groupWindow_Closed(object sender, EventArgs e)
		{
			groupWindow = null;
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.IsAdmin;
			if (!HasPermissionToUse)
			{
				if (groupWindow != null)
					groupWindow.Close();
			}
			//base.MyGroupUpdated();
		}

		public override void Destroy()
		{
			HEROsModNetwork.LoginService.GroupChanged -= LoginService_GroupChanged;
			base.Destroy();
		}
	}

	internal class GroupManagementWindow : UIWindow
	{
		private static float spacing = 16f;

		//Group group;
		private UIDropdown dropdown = new UIDropdown();
		private UIColorPicker colorPicker = new UIKit.UIColorPicker();

		private UIScrollView checkboxContainer = new UIScrollView();

		public event EventHandler Closed;

		public GroupManagementWindow()
		{
			UILabel title = new UILabel(HEROsMod.HeroText("GroupManagement"));
			UIButton bApply = new UIButton(HEROsMod.HeroText("Apply"));
			UIButton bDelete = new UIButton(HEROsMod.HeroText("DeleteGroup"));
			UIButton bNew = new UIButton(HEROsMod.HeroText("NewGroup"));
			UILabel label = new UILabel(HEROsMod.HeroText("Groups") + ":");
			UIImage bClose = new UIImage(closeTexture);
			dropdown.selectedChanged += dropdown_selectedChanged;
			this.Anchor = AnchorPosition.Center;
			this.CanMove = true;

			this.Width = 700;
			title.Scale = .6f;
			title.X = spacing;
			title.Y = spacing;
			title.OverridesMouse = false;
			bClose.X = Width - bClose.Width - spacing;
			bClose.Y = spacing;
			label.X = spacing;
			label.Y = title.Y + title.Height;
			label.Scale = .5f;
			dropdown.X = label.X + label.Width + 4;
			dropdown.Y = label.Y;
			dropdown.Width = 200;
			colorPicker.X = dropdown.X + dropdown.Width + spacing;
			colorPicker.Y = dropdown.Y;
			checkboxContainer.X = spacing;
			checkboxContainer.Y = colorPicker.Y + colorPicker.Height + spacing;
			checkboxContainer.Width = this.Width - spacing * 2;
			checkboxContainer.Height = 150;
			AddChild(checkboxContainer);
			AddChild(label);
			AddChild(title);

			RefreshGroupList();

			bApply.Anchor = AnchorPosition.TopRight;
			bApply.X = Width - spacing;
			bApply.Y = checkboxContainer.Y + checkboxContainer.Height + spacing;
			bNew.X = spacing;
			bNew.Y = bApply.Y;
			bNew.AutoSize = false;
			bNew.Width = 100;
			bDelete.X = bNew.X + bNew.Width + 8;
			bDelete.Y = bNew.Y;
			bDelete.AutoSize = false;
			bDelete.Width = 100;

			bApply.onLeftClick += bApply_onLeftClick;
			bClose.onLeftClick += bClose_onLeftClick;
			bNew.onLeftClick += bNew_onLeftClick;
			bDelete.onLeftClick += bDelete_onLeftClick;

			AddChild(bApply);
			AddChild(bClose);
			AddChild(bNew);
			AddChild(bDelete);
			AddChild(dropdown);
			AddChild(colorPicker);

			this.Height = bApply.Position.Y + bApply.Height + spacing;
			this.CenterToParent();
		}

		private void bDelete_onLeftClick(object sender, EventArgs e)
		{
			UIMessageBox mb = new UIMessageBox(string.Format(HEROsMod.HeroText("AreYouSureDeleteGroup"), dropdown.Text), UIMessageBoxType.YesNo, true);
			mb.yesClicked += mb_yesClicked;
			this.Parent.AddChild(mb);
		}

		private void mb_yesClicked(object sender, EventArgs e)
		{
			HEROsModNetwork.LoginService.RequestDeleteGroup(HEROsModNetwork.Network.Groups[dropdown.SelectedItem].ID);
		}

		private void bNew_onLeftClick(object sender, EventArgs e)
		{
			Parent.AddChild(new NewGroupWindow());
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			Close();
		}

		private void bApply_onLeftClick(object sender, EventArgs e)
		{
			HEROsModNetwork.Group group = new HEROsModNetwork.Group(dropdown.GetItem(dropdown.SelectedItem));
			group.ID = HEROsModNetwork.Network.Groups[dropdown.SelectedItem].ID;
			group.Color = colorPicker.Color;
			group.ImportPermissions(ExportPermissions());
			HEROsModNetwork.LoginService.RequestSetGroupPermissions(group);
		}

		public void RefreshGroupList()
		{
			int prevNumOfGroups = dropdown.ItemCount;
			int prevSelected = dropdown.SelectedItem;
			dropdown.ClearItems();
			for (int i = 0; i < HEROsModNetwork.Network.Groups.Count; i++)
			{
				dropdown.AddItem(HEROsModNetwork.Network.Groups[i].Name);
			}
			if (dropdown.ItemCount == prevNumOfGroups)
			{
				dropdown.SelectedItem = prevSelected;
			}
			RefreshGroupPermissions();
		}

		public void RefreshGroupPermissions()
		{
			checkboxContainer.ClearContent();
			for (int i = 0; i < HEROsModNetwork.Group.PermissionList.Count; i++)
			{
				UICheckbox cb = new UICheckbox(HEROsModNetwork.Group.PermissionList[i].Description);
				cb.Selected = HEROsModNetwork.Network.Groups[dropdown.SelectedItem].HasPermission(HEROsModNetwork.Group.PermissionList[i].Key);
				checkboxContainer.AddChild(cb);
				int index = i;
				cb.X = spacing + index % 2 * (Width / 2);
				cb.Y = index / 2 * (cb.Height) + spacing;
			}
			if (checkboxContainer.ChildCount > 0)
				checkboxContainer.ContentHeight = checkboxContainer.GetLastChild().Y + checkboxContainer.GetLastChild().Height + spacing;
			colorPicker.Color = HEROsModNetwork.Network.Groups[dropdown.SelectedItem].Color;
		}

		private byte[] ExportPermissions()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(memoryStream))
				{
					int numberOfPermissions = 0;
					List<string> permissions = new List<string>();
					for (int i = 1; i < checkboxContainer.ChildCount; i++)
					{
						UICheckbox cb = (UICheckbox)checkboxContainer.children[i];
						if (cb.Selected)
						{
							numberOfPermissions++;
							permissions.Add(HEROsModNetwork.Group.PermissionList[i - 1].Key);
						}
					}
					writer.Write(numberOfPermissions);
					foreach (var p in permissions)
					{
						writer.Write(p);
					}
					writer.Close();
					memoryStream.Close();
					return memoryStream.ToArray();
				}
			}
		}

		private void dropdown_selectedChanged(object sender, EventArgs e)
		{
			RefreshGroupPermissions();
		}

		public override void Update()
		{
			if (Main.gameMenu) Close();
			base.Update();
		}

		public void Close()
		{
			if (this.Parent != null)
				this.Parent.RemoveChild(this);
			if (Closed != null)
			{
				Closed(this, EventArgs.Empty);
			}
		}
	}

	internal class NewGroupWindow : UIWindow
	{
		private UILabel label = null;
		private UITextbox textbox = null;
		private static float spacing = 8f;

		public NewGroupWindow()
		{
			UIView.exclusiveControl = this;

			Width = 600;
			Height = 100;
			this.Anchor = AnchorPosition.Center;

			label = new UILabel(HEROsMod.HeroText("GroupName") + ":");
			textbox = new UITextbox();
			UIButton bSave = new UIButton(HEROsMod.HeroText("Create"));
			UIButton bCancel = new UIButton(HEROsMod.HeroText("Cancel"));

			label.Scale = .5f;

			label.Anchor = AnchorPosition.Left;
			textbox.Anchor = AnchorPosition.Left;
			bSave.Anchor = AnchorPosition.BottomRight;
			bCancel.Anchor = AnchorPosition.BottomRight;

			float tby = textbox.Height / 2 + spacing;
			label.Position = new Vector2(spacing, tby);
			textbox.Position = new Vector2(label.Position.X + label.Width + spacing, tby);
			bCancel.Position = new Vector2(this.Width - spacing, this.Height - spacing);
			bSave.Position = new Vector2(bCancel.Position.X - bCancel.Width - spacing, bCancel.Position.Y);

			bCancel.onLeftClick += bCancel_onLeftClick;
			bSave.onLeftClick += bSave_onLeftClick;
			textbox.OnEnterPress += bSave_onLeftClick;

			AddChild(label);
			AddChild(textbox);
			AddChild(bSave);
			AddChild(bCancel);

			textbox.Focus();
		}

		private void bSave_onLeftClick(object sender, EventArgs e)
		{
			if (textbox.Text.Length > 0)
			{
				textbox.Unfocus();
				HEROsModNetwork.LoginService.RequestAddGroup(textbox.Text);
				Close();
			}
		}

		private void bCancel_onLeftClick(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override float GetWidth()
		{
			return textbox.Width + label.Width + spacing * 4;
		}

		private void Close()
		{
			UIView.exclusiveControl = null;
			this.Parent.RemoveChild(this);
		}

		public override void Update()
		{
			if (Parent != null)
				this.Position = new Vector2(Parent.Width / 2, Parent.Height / 2);
			base.Update();
		}
	}
}