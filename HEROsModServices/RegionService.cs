using HEROsMod.HEROsModNetwork;
using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class RegionService : HEROsModService
	{
		private static bool _regionsVisible = false;

		public static bool RegionsVisible
		{
			get
			{
				return _regionsVisible;
			}
			set
			{
				if (value != _regionsVisible)
				{
					if (value)
					{
						Main.NewText(HEROsMod.HeroText("RegionsVisible"));
					}
					else
					{
						Main.NewText(HEROsMod.HeroText("RegionsInvisible"));
					}
					_regionsVisible = value;
				}
			}
		}

		private SelectionConformationWindow _confirmationWindow;
		private RegionWindow _regionWindow;
		private bool canEdit;
		private bool canView;

		public RegionService()
		{
			MultiplayerOnly = true;
			RegionsVisible = false;
			canEdit = false;
			canView = false;
			this._name = "Region Service";
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/region", AssetRequestMode.ImmediateLoad)/*Main.itemTexture[1337]*/);
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("OpenRegionsWindow");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
			this.HasPermissionToUse = true;

			_regionWindow = new RegionWindow();
			this.AddUIView(_regionWindow);
			_regionWindow.Visible = false;
			_confirmationWindow = new SelectionConformationWindow();
			this.AddUIView(_confirmationWindow);
			_confirmationWindow.Visible = false;

			_regionWindow.bCreateRegion.onLeftClick += bCreateRegion_onLeftClick;
			_regionWindow.bAddPlayer.onLeftClick += bAddPlayer_onLeftClick;
			_regionWindow.bAddGroup.onLeftClick += bAddGroup_onLeftClick;
			_confirmationWindow.bConfirm.onLeftClick += bConfirm_onLeftClick;
			_confirmationWindow.bCancel.onLeftClick += bCancel_onLeftClick;

			HEROsModNetwork.GeneralMessages.RegionsUpdated += GeneralMessages_RegionsUpdated;
		}

		private void bAddGroup_onLeftClick(object sender, EventArgs e)
		{
			this.AddUIView(new PlayerGroupSelectionWindow(_regionWindow.SelectedRegion, HEROsMod.HeroText("SelectGroup"), false));
		}

		private void bAddPlayer_onLeftClick(object sender, EventArgs e)
		{
			this.AddUIView(new PlayerGroupSelectionWindow(_regionWindow.SelectedRegion, HEROsMod.HeroText("SelectPlayer"), true));
		}

		private void GeneralMessages_RegionsUpdated(object sender, EventArgs e)
		{
			if (_regionWindow != null)
			{
				if (_regionWindow.SelectedRegion != null)
				{
					int prevSelectedRegionID = _regionWindow.SelectedRegion.ID;
					_regionWindow.PopulateRegionsList();
					Region prevSelectedRegion = Network.GetRegionByID(prevSelectedRegionID);
					if (prevSelectedRegion != null)
					{
						_regionWindow.SelectRegion(prevSelectedRegion);
					}
				}
				else
				{
					_regionWindow.PopulateRegionsList();
				}
			}
		}

		public override void MyGroupUpdated()
		{
			canView = HEROsModNetwork.LoginService.MyGroup.HasPermission("ViewRegions");
			canEdit = HEROsModNetwork.LoginService.MyGroup.HasPermission("EditRegions");
			this.HasPermissionToUse = canView || canEdit;
			// this._canAccessSettings = HEROsModNetwork.LoginService.MyGroup.IsAdmin;

			if (canEdit)
			{
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("OpenRegionsWindow");
			}
			else if (canView)
			{
				_confirmationWindow.Close();
				_regionWindow.Close();
				SelectionTool.Reset();
				HotbarIcon.Tooltip = HEROsMod.HeroText("ToggleRegionsVisible");
			}

			//base.MyGroupUpdated();
		}

		public void ToggleRegionsVisible()
		{
		}

		private void bCancel_onLeftClick(object sender, EventArgs e)
		{
			SelectionTool.Reset();
			_confirmationWindow.Close();
			_regionWindow.Visible = true;
		}

		private void bConfirm_onLeftClick(object sender, EventArgs e)
		{
			if (SelectionTool.Width != 0 && SelectionTool.Height != 0)
			{
				SelectionTool.ListeningForInput = false;
				_confirmationWindow.Close();
				NameRegionWindow nameWindow = new NameRegionWindow();
				this.AddUIView(nameWindow);
			}
			else
			{
				Main.NewText(HEROsMod.HeroText("PleaseMakeASelection"));
			}
		}

		private void bCreateRegion_onLeftClick(object sender, EventArgs e)
		{
			SelectionTool.ListeningForInput = true;
			SelectionTool.Visible = true;
			_regionWindow.Close();
			_confirmationWindow.Visible = true;
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (canEdit)
			{
				if (_confirmationWindow.Visible) return;
				_regionWindow.Visible = !_regionWindow.Visible;
			}
			else
			{
				RegionsVisible = !RegionsVisible;
			}
		}

		public override void Update()
		{
			Vector2 mouseTile = ModUtils.CursorTileCoords;
			int id = 0;
			string str = string.Format(HEROsMod.HeroText("LastChangedBy"), id);
			UIView.HoverText = str;
			//ModUtils.MouseText(str);
			base.Update();
		}

		public static void DrawRegions(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < HEROsModNetwork.Network.Regions.Count; i++)
			{
				HEROsModNetwork.Region region = HEROsModNetwork.Network.Regions[i];
				ModUtils.DrawBorderedRect(spriteBatch, region.Color, region.Position, region.Size, 3, region.Name);
			}
		}
	}

	internal class RegionWindow : UIWindow
	{
		//private Color _prevRegionColor;

		public UIButton bCreateRegion;
		private UILabel lTitle;
		private UIScrollView scrollView;
		private UIButton bDeleteRegion;
		public Region SelectedRegion { get; set; }

		public UIButton bAddPlayer;
		public UIButton bAddGroup;
		private UIButton bBack;
		private UIColorSlider colorSlider;
		private UIButton bSaveColor;
		private UICheckbox bProtectChests;

		public RegionWindow()
		{
			SelectedRegion = null;
			this.CanMove = true;
			this.Width = 600;
			this.Anchor = AnchorPosition.Center;
			bCreateRegion = new UIButton(HEROsMod.HeroText("CreateRegion"));
			lTitle = new UILabel(HEROsMod.HeroText("Regions"));
			scrollView = new UIScrollView();
			bDeleteRegion = new UIButton(HEROsMod.HeroText("DeleteRegion"));
			bAddGroup = new UIButton(HEROsMod.HeroText("AddGroup"));
			bAddPlayer = new UIButton(HEROsMod.HeroText("AddPlayer"));
			bBack = new UIButton(Language.GetTextValue("UI.Back"));
			UIButton bToggleRegions = new UIButton(HEROsMod.HeroText("ToggleRegionsVisible"));
			bProtectChests = new UICheckbox(HEROsMod.HeroText("ProtectChests"));
			colorSlider = new UIColorSlider();
			bSaveColor = new UIButton(HEROsMod.HeroText("SaveColor"));
			UIImage bClose = new UIImage(closeTexture);

			lTitle.X = Spacing;
			lTitle.Y = Spacing;
			lTitle.Scale = .6f;
			lTitle.OverridesMouse = false;

			bClose.X = Width - bClose.Width - Spacing;
			bClose.Y = Spacing;
			bClose.onLeftClick += bClose_onLeftClick;

			bCreateRegion.AutoSize = false;
			bDeleteRegion.AutoSize = false;
			bAddGroup.AutoSize = false;
			bAddPlayer.AutoSize = false;
			bSaveColor.AutoSize = false;
			bBack.AutoSize = false;
			bCreateRegion.Width = colorSlider.Width;
			bAddGroup.Width = 100;
			bAddPlayer.Width = bAddGroup.Width;
			bDeleteRegion.Width = colorSlider.Width;
			bSaveColor.Width = 100;
			bBack.Width = bSaveColor.Width;

			bCreateRegion.X = lTitle.X;
			bCreateRegion.Y = lTitle.Y + lTitle.Height;
			bDeleteRegion.X = bCreateRegion.X;
			bDeleteRegion.Y = bCreateRegion.Y;
			bBack.X = bDeleteRegion.X + bDeleteRegion.Width + Spacing;
			bBack.Y = bDeleteRegion.Y;

			colorSlider.Anchor = AnchorPosition.Left;
			colorSlider.X = Spacing;
			bSaveColor.Y = bBack.Y + bBack.Height + Spacing;
			bSaveColor.X = colorSlider.X + colorSlider.Width + Spacing;
			colorSlider.Y = bSaveColor.Y + bSaveColor.Height / 2;

			bProtectChests.X = colorSlider.X + colorSlider.Width + bSaveColor.Width + Spacing * 2;
			bProtectChests.Y = bBack.Y + bBack.Height + Spacing;

			bAddGroup.X = Width - bAddGroup.Width - Spacing;
			bAddGroup.Y = bDeleteRegion.Y;
			bAddPlayer.X = bAddGroup.X;
			bAddPlayer.Y = bAddGroup.Y + bAddPlayer.Height + Spacing;

			scrollView.X = Spacing;
			scrollView.Y = bSaveColor.Y + bSaveColor.Height + Spacing;
			scrollView.Width = Width - Spacing * 2;
			scrollView.Height = 150;

			bToggleRegions.X = scrollView.X;
			bToggleRegions.Y = scrollView.Y + scrollView.Height + Spacing;

			bBack.onLeftClick += bBack_onLeftClick;
			bDeleteRegion.onLeftClick += bDeleteRegion_onLeftClick;
			bToggleRegions.onLeftClick += bToggleRegions_onLeftClick;
			bProtectChests.onLeftClick += bProtectChests_onLeftClick;
			colorSlider.valueChanged += colorSlider_valueChanged;
			bSaveColor.onLeftClick += bSaveColor_onLeftClick;

			bDeleteRegion.Visible = false;
			bAddPlayer.Visible = false;
			bAddGroup.Visible = false;
			bBack.Visible = false;
			colorSlider.Visible = false;
			bSaveColor.Visible = false;
			bProtectChests.Visible = false;

			this.Height = bToggleRegions.Y + bToggleRegions.Height + Spacing;

			AddChild(lTitle);
			AddChild(bClose);
			AddChild(scrollView);
			AddChild(bCreateRegion);
			AddChild(bDeleteRegion);
			AddChild(bAddPlayer);
			AddChild(bAddGroup);
			AddChild(bBack);
			AddChild(bToggleRegions);
			AddChild(bProtectChests);
			AddChild(colorSlider);
			AddChild(bSaveColor);

			this.CenterToParent();

			PopulateRegionsList();
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}

		private void bSaveColor_onLeftClick(object sender, EventArgs e)
		{
			GeneralMessages.RequestToChangeColorOfRegion(SelectedRegion, SelectedRegion.Color);
			//_prevRegionColor = new Color(SelectedRegion.Color.ToVector3());
		}

		private void colorSlider_valueChanged(object sender, float value)
		{
			if (SelectedRegion != null)
			{
				SelectedRegion.Color = Main.hslToRgb(colorSlider.Value, 1f, .5f);
			}
		}

		private void bToggleRegions_onLeftClick(object sender, EventArgs e)
		{
			RegionService.RegionsVisible = !RegionService.RegionsVisible;
		}

		private void bProtectChests_onLeftClick(object sender, EventArgs e)
		{
			GeneralMessages.RequestToChangeChestProtectionOfRegion(SelectedRegion, !SelectedRegion.ChestsProtected);
		}

		private void bDeleteRegion_onLeftClick(object sender, EventArgs e)
		{
			if (SelectedRegion == null)
			{
				PopulateRegionsList();
				return;
			}
			GeneralMessages.RequestRemoveRegion(SelectedRegion);
		}

		private void bBack_onLeftClick(object sender, EventArgs e)
		{
			//SelectedRegion.Color = new Color(_prevRegionColor.ToVector3());
			PopulateRegionsList();
		}

		public void PopulateRegionsList()
		{
			scrollView.ClearContent();

			bCreateRegion.Visible = true;
			bDeleteRegion.Visible = false;
			bAddGroup.Visible = false;
			bAddPlayer.Visible = false;
			bBack.Visible = false;
			colorSlider.Visible = false;
			bSaveColor.Visible = false;
			bProtectChests.Visible = false;

			SelectedRegion = null;

			float yPos = Spacing;
			for (int i = 0; i < HEROsModNetwork.Network.Regions.Count; i++)
			{
				HEROsModNetwork.Region region = HEROsModNetwork.Network.Regions[i];
				UIButton bEdit = new UIButton(Language.GetTextValue("LegacyInterface.48")); // "Edit"
				UIRect bg = new UIRect();
				bg.X = LargeSpacing;
				bg.Y = yPos;
				bg.Height = bEdit.Height + SmallSpacing * 2;
				bg.Width = scrollView.Width - 20 - LargeSpacing * 2;

				yPos += bg.Height;

				bg.ForegroundColor = i % 2 == 0 ? Color.Transparent : Color.Blue * .1f;

				UILabel label = new UILabel(region.Name);
				label.X = Spacing;
				label.Y = Spacing;
				label.Scale = .5f;

				bEdit.X = bg.Width - bEdit.Width - Spacing;
				bEdit.Y = SmallSpacing;
				bEdit.Tag = i;
				bEdit.onLeftClick += bEdit_onLeftClick;

				UIRect regionColor = new UIRect();
				regionColor.ForegroundColor = region.Color;
				regionColor.Height = bg.Height - Spacing * 2;
				regionColor.Width = regionColor.Height;
				regionColor.Y = Spacing;

				UILabel lX = new UILabel("X: " + region.X);
				UILabel lY = new UILabel("Y: " + region.Y);
				UILabel lWidth = new UILabel("Width: " + region.Width);
				UILabel lHeight = new UILabel("Height: " + region.Height);

				lX.Scale = .35f;
				lY.Scale = lX.Scale;
				lWidth.Scale = lX.Scale;
				lHeight.Scale = lX.Scale;

				lWidth.X = bEdit.X - 100;
				lHeight.X = lWidth.X;
				lHeight.Y = bg.Height - lHeight.Height;

				lX.X = lWidth.X - 75;
				lY.X = lX.X;
				lX.Y = lWidth.Y;
				lY.Y = lHeight.Y;

				regionColor.X = lX.X - regionColor.Width - Spacing;

				bg.AddChild(label);
				bg.AddChild(bEdit);
				bg.AddChild(lX);
				bg.AddChild(lY);
				bg.AddChild(lWidth);
				bg.AddChild(lHeight);
				bg.AddChild(regionColor);

				scrollView.AddChild(bg);
			}
			scrollView.ContentHeight = yPos + Spacing;
		}

		private void bEdit_onLeftClick(object sender, EventArgs e)
		{
			UIButton buttion = (UIButton)sender;
			int regionIndex = (int)buttion.Tag;

			Region region = Network.Regions[regionIndex];
			SelectRegion(region);
		}

		public void SelectRegion(Region region)
		{
			scrollView.ClearContent();

			SelectedRegion = region;
			ModUtils.DebugText(string.Format(HEROsMod.HeroText("SelectedRegion"), region.Name, region.ID));
			bCreateRegion.Visible = false;
			bDeleteRegion.Visible = true;
			bAddGroup.Visible = true;
			bAddPlayer.Visible = true;
			bBack.Visible = true;
			colorSlider.Visible = true;
			bSaveColor.Visible = true;
			bProtectChests.Visible = true;
			bProtectChests.Selected = region.ChestsProtected;

			colorSlider.Value = Main.rgbToHsl(region.Color).X;
			//_prevRegionColor = new Color(region.Color.ToVector3());

			float yPos = Spacing;
			int itemCount = 0;
			for (int i = 0; i < region.AllowedGroupsIDs.Count; i++)
			{
				Group group = Network.GetGroupByID(region.AllowedGroupsIDs[i]);
				if (group == null) continue;

				UIButton bRemove = new UIButton(HEROsMod.HeroText("Remove"));
				UIRect bg = new UIRect();
				bg.X = LargeSpacing;
				bg.Y = yPos;
				bg.Height = bRemove.Height + SmallSpacing * 2;
				bg.Width = scrollView.Width - 20 - LargeSpacing * 2;

				yPos += bg.Height;

				bg.ForegroundColor = itemCount % 2 == 0 ? Color.Transparent : Color.Blue * .1f;

				UILabel label = new UILabel(HEROsMod.HeroText("Group"));
				label.X = Spacing;
				label.Y = Spacing;
				label.Scale = .5f;

				UILabel lName = new UILabel(group.Name);
				lName.X = label.X + 100;
				lName.Y = label.Y;
				lName.Scale = label.Scale;

				bRemove.X = bg.Width - bRemove.Width - Spacing;
				bRemove.Y = SmallSpacing;
				bRemove.Tag = group.ID;
				bRemove.onLeftClick += RemoveGroup;

				bg.AddChild(label);
				bg.AddChild(bRemove);
				bg.AddChild(lName);

				scrollView.AddChild(bg);
				itemCount++;
			}

			for (int i = 0; i < region.AllowedPlayersIDs.Count; i++)
			{
				UserWithID user = null;
				for (int j = 0; j < Network.RegisteredUsers.Count; j++)
				{
					if (Network.RegisteredUsers[j].ID == region.AllowedPlayersIDs[i])
					{
						user = Network.RegisteredUsers[j];
						break;
					}
				}
				if (user == null) continue;

				UIButton bRemove = new UIButton(HEROsMod.HeroText("Remove"));
				UIRect bg = new UIRect();
				bg.X = LargeSpacing;
				bg.Y = yPos;
				bg.Height = bRemove.Height + SmallSpacing * 2;
				bg.Width = scrollView.Width - 20 - LargeSpacing * 2;

				yPos += bg.Height;

				bg.ForegroundColor = itemCount % 2 == 0 ? Color.Transparent : Color.Blue * .1f;

				UILabel label = new UILabel(HEROsMod.HeroText("User"));
				label.X = Spacing;
				label.Y = Spacing;
				label.Scale = .5f;

				UILabel lName = new UILabel(user.Username);
				lName.X = label.X + 100;
				lName.Y = label.Y;
				lName.Scale = label.Scale;

				bRemove.X = bg.Width - bRemove.Width - Spacing;
				bRemove.Y = SmallSpacing;
				bRemove.Tag = user.ID;
				bRemove.onLeftClick += RemovePlayer;

				bg.AddChild(label);
				bg.AddChild(bRemove);
				bg.AddChild(lName);

				scrollView.AddChild(bg);
				itemCount++;
			}
			scrollView.ContentHeight = yPos + Spacing;
		}

		private void RemovePlayer(object sender, EventArgs e)
		{
			UIButton button = (UIButton)sender;
			int playerID = (int)button.Tag;
			GeneralMessages.RequestRemovePlayerFromRegion(SelectedRegion, playerID);
		}

		private void RemoveGroup(object sender, EventArgs e)
		{
			UIButton button = (UIButton)sender;
			int groupID = (int)button.Tag;
			GeneralMessages.RequestRemoveGroupFromRegion(SelectedRegion, groupID);
		}

		private void bRemoveAllRegions_onLeftClick(object sender, EventArgs e)
		{
			for (int i = 0; i < HEROsModNetwork.Network.Regions.Count; i++)
			{
				HEROsModNetwork.GeneralMessages.RequestRemoveRegion(HEROsModNetwork.Network.Regions[i]);
			}
		}

		public override void Update()
		{
			if (SelectedRegion != null)
			{
				//UIRect rect = (UIRect)colorSlider.Tag;
				//rect.ForegroundColor = Main.hslToRgb(colorSlider.Value, 1f, .5f);
			}
			base.Update();
		}

		public void Close()
		{
			this.Visible = false;
			colorSlider.Visible = false;
		}
	}

	internal class SelectionConformationWindow : UIWindow
	{
		public UIButton bConfirm;
		public UIButton bCancel;

		public SelectionConformationWindow()
		{
			this.Anchor = AnchorPosition.BottomRight;
			bConfirm = new UIButton(HEROsMod.HeroText("Confirm"));
			bCancel = new UIButton(HEROsMod.HeroText("Cancel"));
			bConfirm.X = Spacing;
			bConfirm.Y = Spacing;
			bCancel.X = bConfirm.X + bConfirm.Width + Spacing;
			bCancel.Y = bConfirm.Y;
			this.Width = bCancel.X + bCancel.Width + Spacing;
			this.Height = bConfirm.Y + bConfirm.Height + Spacing;
			AddChild(bConfirm);
			AddChild(bCancel);
		}

		public override void Update()
		{
			this.X = Main.screenWidth - 100;
			this.Y = Main.screenHeight - 100;
			base.Update();
		}

		public void Close()
		{
			this.Visible = false;
		}
	}

	internal class NameRegionWindow : UIWindow
	{
		private UILabel label = null;
		private UITextbox textbox = null;
		public UIButton bSave;
		private static float spacing = 8f;

		public NameRegionWindow()
		{
			UIView.exclusiveControl = this;

			Width = 600;
			Height = 100;
			this.Anchor = AnchorPosition.Center;

			label = new UILabel(HEROsMod.HeroText("RegionName"));
			textbox = new UITextbox();
			bSave = new UIButton(HEROsMod.HeroText("Ok"));
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
			if (textbox.Text.Length >= 3)
			{
				textbox.Unfocus();
				HEROsModNetwork.Region region = new HEROsModNetwork.Region(textbox.Text, SelectionTool.Position, SelectionTool.Size);
				HEROsModNetwork.GeneralMessages.RequestCreateRegion(region);
				SelectionTool.Reset();
			}
			else
			{
				SelectionTool.Reset();
				Main.NewText(HEROsMod.HeroText("RegionNameTooShort"));
			}
			this.Close();
		}

		private void bCancel_onLeftClick(object sender, EventArgs e)
		{
			SelectionTool.Reset();
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
			if (Main.gameMenu) Close();
			if (Parent != null)
				this.Position = new Vector2(Parent.Width / 2, Parent.Height / 2);
			base.Update();
		}
	}

	internal class PlayerGroupSelectionWindow : UIWindow
	{
		private Region _region;

		public PlayerGroupSelectionWindow(Region region, string title, bool addingPlayer)
		{
			CenterToParent();
			this.CanMove = true;
			this._region = region;
			UIView.exclusiveControl = this;
			UILabel lTitle = new UILabel(title);
			UIScrollView scrollView = new UIScrollView();
			UIButton bCancel = new UIButton(HEROsMod.HeroText("Cancel"));

			lTitle.X = Spacing;
			lTitle.Y = Spacing;
			lTitle.Scale = .6f;

			scrollView.X = lTitle.X;
			scrollView.Y = lTitle.Y + lTitle.Height + SmallSpacing;

			scrollView.Width = 300;
			scrollView.Height = 350;

			bCancel.X = scrollView.X + scrollView.Width - bCancel.Width;
			bCancel.Y = scrollView.Y + scrollView.Height + Spacing;
			bCancel.onLeftClick += bCancel_onLeftClick;

			this.Anchor = AnchorPosition.Center;
			this.Width = scrollView.Width + Spacing * 2;
			this.Height = bCancel.Y + bCancel.Height + Spacing;
			this.X = Main.screenWidth / 2;
			this.Y = Main.screenHeight / 2;

			AddChild(lTitle);
			AddChild(scrollView);
			AddChild(bCancel);

			float yPos = Spacing;
			if (addingPlayer)
			{
				for (int i = 0; i < Network.RegisteredUsers.Count; i++)
				{
					UserWithID user = Network.RegisteredUsers[i];
					UILabel userLabel = new UILabel(user.Username);
					userLabel.Tag = user.ID;
					userLabel.X = Spacing;
					userLabel.Y = yPos;
					userLabel.Scale = .35f;
					yPos += userLabel.Height;
					userLabel.onLeftClick += userLabel_onLeftClick;
					scrollView.AddChild(userLabel);
				}
			}
			else
			{
				for (int i = 0; i < Network.Groups.Count; i++)
				{
					Group group = Network.Groups[i];
					UILabel groupLabel = new UILabel(group.Name);
					groupLabel.Tag = group.ID;
					groupLabel.X = Spacing;
					groupLabel.Y = yPos;
					groupLabel.Scale = .35f;
					yPos += groupLabel.Height;
					groupLabel.onLeftClick += groupLabel_onLeftClick;
					scrollView.AddChild(groupLabel);
				}
			}
			scrollView.ContentHeight = yPos + Spacing;
		}

		private void bCancel_onLeftClick(object sender, EventArgs e)
		{
			Close();
		}

		public void Close()
		{
			UIView.exclusiveControl = null;
			Parent.RemoveChild(this);
		}

		private void userLabel_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			int playerID = (int)label.Tag;
			GeneralMessages.RequestAddPlayerToRegion(_region, playerID);
			Close();
		}

		private void groupLabel_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			int groupID = (int)label.Tag;
			GeneralMessages.RequestAddGroupToRegion(_region, groupID);
			Close();
		}

		public override void Update()
		{
			//CenterToParent();
			base.Update();
		}
	}

	internal class ColorPicker : UIWindow
	{
		private UIRect colorRect;
		private UIColorSlider hue;
		private UISlider saturation;
		private UISlider luminosity;

		public ColorPicker()
		{
			colorRect = new UIRect();
			hue = new UIColorSlider();
			saturation = new UISlider();
			luminosity = new UISlider();

			colorRect.Width = hue.Width;
			colorRect.Height = 30;
			colorRect.X = Spacing;
			colorRect.Y = Spacing;

			hue.X = Spacing;
			hue.Y = colorRect.Y + colorRect.Height + Spacing;
			saturation.X = hue.X;
			saturation.Y = hue.Y + hue.Height + Spacing;
			luminosity.X = hue.X;
			luminosity.Y = saturation.Y + saturation.Height + Spacing;

			this.Width = hue.Width + Spacing * 2;
			this.Height = luminosity.Y + luminosity.Height + Spacing;

			AddChild(colorRect);
			AddChild(hue);
			AddChild(saturation);
			AddChild(luminosity);
		}

		public override void Update()
		{
			colorRect.ForegroundColor = Main.hslToRgb(hue.Value, 1f, .5f);
			base.Update();
		}
	}
}