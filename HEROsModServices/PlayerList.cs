using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using System;
using System.IO;
using System.Linq;
using Terraria;
using HEROsMod.HEROsModNetwork;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class PlayerList : HEROsModService
	{
		public static CurrentPlayersWindow playersWindow;

		public PlayerList()
		{
			MultiplayerOnly = true;
			this._name = "Player List";
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/connectedPlayers", AssetRequestMode.ImmediateLoad));
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("ViewConnectedPlayers");
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (playersWindow == null)
			{
				playersWindow = new CurrentPlayersWindow();
				playersWindow.Closed += PlayerWindowClosed;
				this.AddUIView(playersWindow);
			}
			else
			{
				playersWindow.Close();
			}
		}

		private void PlayerWindowClosed(object sender, EventArgs e)
		{
			playersWindow = null;
		}

		public override void MyGroupUpdated()
		{
			// TODO! This prevents snoop, since IsAdmin might not be true.
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.IsAdmin;
			if (!HasPermissionToUse)
			{
				if (playersWindow != null)
					playersWindow.Close();
			}
			//base.MyGroupUpdated();
		}

		public override void Destroy()
		{
			if (playersWindow != null)
			{
				playersWindow.Close();
			}
			base.Destroy();
		}
	}

	internal class CurrentPlayersWindow : UIWindow
	{
		private UIScrollView scrollView = new UIScrollView();
		public bool canOpenPlayerInfo = true;
		private static float spacing = 16f;
		private PlayerInfo playerInfo = null;

		public event EventHandler Closed;

		public CurrentPlayersWindow()
		{
			Width = 300;
			Height = 400;
			CanMove = true;
			Anchor = AnchorPosition.Center;
			CenterToParent();
			UILabel title = new UILabel(HEROsMod.HeroText("ConnectedPlayers"));
			UIImage bClose = new UIImage(closeTexture);

			bClose.Anchor = AnchorPosition.TopRight;
			bClose.X = Width - spacing;
			bClose.Y = spacing;
			title.Scale = .6f;
			title.X = spacing;
			title.Y = spacing;
			title.OverridesMouse = false;
			scrollView.Width = Width - spacing * 2;
			scrollView.Height = Height - title.Height - spacing * 2;
			scrollView.X = spacing;
			scrollView.Y = title.Y + title.Height;

			AddChild(scrollView);
			AddChild(title);
			AddChild(bClose);
			bClose.onLeftClick += bClose_onLeftClick;
			UpdateList();
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			Close();
		}

		public override void Update()
		{
			if (Main.gameMenu) this.Visible = false;
			base.Update();
		}

		public void UpdateList()
		{
			scrollView.ClearContent();
			float yPos = spacing;
			for (int i = 0; i < Main.player.Length; i++)
			{
				Player player = Main.player[i];
				if (player.active)
				{
					UIPlayerHead playerHead = new UIPlayerHead(player);
					playerHead.X = 8;
					playerHead.Y = yPos;
					yPos += playerHead.Height;
					UILabel label = new UILabel(player.name);
					label.Scale = .5f;
					label.Anchor = AnchorPosition.Left;
					label.X = playerHead.X + playerHead.Width + 8;
					label.Y = playerHead.Y + playerHead.Width / 2 + 8;
					label.onLeftClick += label_onLeftClick;
					label.Tag = i;

					scrollView.AddChild(playerHead);
					scrollView.AddChild(label);
				}
			}
			// TODO changes to offline users might not prop to all admin?
			if (HEROsModNetwork.LoginService.MyGroup.IsAdmin)
			{
				UILabel lOfflinePlayers = new UILabel(HEROsMod.HeroText("OfflineUsers"));
				lOfflinePlayers.Scale = .6f;
				lOfflinePlayers.X = Spacing;
				lOfflinePlayers.Y = yPos + Spacing;
				lOfflinePlayers.ForegroundColor = Microsoft.Xna.Framework.Color.Yellow;
				yPos = lOfflinePlayers.Y + lOfflinePlayers.Height;
				scrollView.AddChild(lOfflinePlayers);
				for (int i = 0; i < HEROsModNetwork.Network.RegisteredUsers.Count; i++)
				{
					HEROsModNetwork.UserWithID user = HEROsModNetwork.Network.RegisteredUsers[i];
					//ErrorLogger.Log("? " + user.ID);
					//foreach (var item in HEROsModNetwork.Network.Players)
					//{
					//	ErrorLogger.Log("U" + item.ID);
					//}
					if (HEROsModNetwork.Network.Players.Any(x => x.ID == user.ID))
					{
						//	ErrorLogger.Log("Continue on " + user.ID);
						continue;
					}
					UILabel lUser = new UILabel(user.Username);
					lUser.Scale = .5f;
					lUser.X = 40 + Spacing * 2;
					lUser.Y = yPos;
					lUser.ForegroundColor = new Microsoft.Xna.Framework.Color(200, 200, 200);
					yPos += lUser.Height;
					lUser.onLeftClick += lUser_onLeftClick;
					lUser.Tag = user.ID;
					scrollView.AddChild(lUser);
				}
			}

			scrollView.ContentHeight = yPos + spacing;
		}

		// is admin
		private void lUser_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			int userID = (int)label.Tag;

			OpenPlayerInfo(userID, true);

			//OpenPlayerInfo()
		}

		private void label_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			int playerIndex = (int)label.Tag;
			if (HEROsModNetwork.LoginService.MyGroup.IsAdmin)
			{
				HEROsModNetwork.LoginService.RequestPlayerInfo(playerIndex);
			}
			else
			{
				OpenPlayerInfo(playerIndex, false);
			}
		}

		public void OpenPlayerInfo(int indexOfRequestedPlayer, bool offlinePlayer)
		{
			ClosePlayerInfo();
			playerInfo = new PlayerInfo(indexOfRequestedPlayer, offlinePlayer);
			AddChild(playerInfo);
		}

		public void ClosePlayerInfo()
		{
			if (playerInfo != null)
			{
				playerInfo.Parent.RemoveChild(playerInfo);
				playerInfo = null;
			}
		}

		public void Close()
		{
			ClosePlayerInfo();
			if (this.Parent != null)
				this.Parent.RemoveChild(this);
			if (Closed != null)
			{
				Closed(this, EventArgs.Empty);
			}
		}
	}

	internal class PlayerInfo : UIWindow
	{
		private static float spacing = 8f;

		//UIButton bBan;
		//UIButton bKick;
		private UIDropdown dropdown;

		private HEROsModNetwork.HEROsModPlayer player;
		private int playerIndex;

		public PlayerInfo(int playerIndex, bool offlineUser)
		{
			if (!offlineUser)
				this.player = HEROsModNetwork.Network.Players[playerIndex];
			this.playerIndex = playerIndex;
			this.UpdateWhenOutOfBounds = true;
			Width = 350;
			UIImage bClose = new UIImage(closeTexture);
			UILabel lGroup = new UILabel("Group:");
			UIButton bBan = new UIButton("Ban");
			UIButton bKick = new UIButton("Kick");
			UILabel label = new UILabel();
			SnoopWindow snoopWindow = new SnoopWindow();
			snoopWindow.SetPlayer(Main.player[0]);
			dropdown = new UIDropdown();
			UIButton bTeleport = new UIButton("Teleport To");
			UIButton bRestore = new UIButton("Restore Changes Made by this Player");
			bTeleport.AutoSize = false;
			bTeleport.Width = 150;
			bRestore.AutoSize = false;

			// if logged in
			if (player != null && player.Username.Length > 0)
			{
				dropdown.AddItem(HEROsModNetwork.Network.AdminGroup.Name);
				for (int i = 0; i < HEROsModNetwork.Network.Groups.Count; i++)
				{
					dropdown.AddItem(HEROsModNetwork.Network.Groups[i].Name);
					if (player.Group.Name == HEROsModNetwork.Network.Groups[i].Name)
					{
						dropdown.SelectedItem = i + 1;
					}
				}
			}
			else if (player == null)
			{
				HEROsModNetwork.UserWithID user = HEROsModNetwork.Network.RegisteredUsers[playerIndex];
				dropdown.AddItem(HEROsModNetwork.Network.AdminGroup.Name);
				for (int i = 0; i < HEROsModNetwork.Network.Groups.Count; i++)
				{
					dropdown.AddItem(HEROsModNetwork.Network.Groups[i].Name);

					if (user.groupID == HEROsModNetwork.Network.Groups[i].ID)
					{
						dropdown.SelectedItem = i + 1;
					}
				}
			}
			dropdown.selectedChanged += dropdown_selectedChanged;

			bClose.Y = spacing;
			lGroup.Scale = .5f;
			lGroup.X = spacing;
			lGroup.Y = spacing;
			dropdown.X = lGroup.X + lGroup.Width + 4;
			dropdown.Y = lGroup.Y;
			dropdown.Width = 200;
			dropdown.UpdateWhenOutOfBounds = true;
			if (player != null && player.Username.Length > 0)
			{
				label.Text = "Logged in as " + player.Username;
			}
			else label.Text = "Not Logged In";
			label.X = spacing;
			label.Y = dropdown.Y + dropdown.Height + spacing;
			label.Scale = .35f;
			bBan.X = label.X;
			bBan.Y = label.Y + label.Height + spacing;
			bKick.X = bBan.X + bBan.Width + spacing;
			bKick.Y = bBan.Y;

			bTeleport.X = Width - bTeleport.Width - spacing;
			bTeleport.Y = bBan.Y;

			bRestore.X = Spacing;
			bRestore.Y = bTeleport.Y + bTeleport.Height + spacing;

			snoopWindow.X = bRestore.X;
			snoopWindow.Y = bRestore.Y + bRestore.Height + spacing;

			Width = bTeleport.X + bTeleport.Width + spacing;
			Height = bRestore.Y + bRestore.Height + spacing;

			bRestore.Width = Width - spacing * 2;

			AddChild(bClose);
			AddChild(label);
			HEROsModNetwork.Group myGroup = HEROsModNetwork.LoginService.MyGroup;
			if (!offlineUser)
			{
				if (myGroup.HasPermission("TeleportToPlayers")) AddChild(bTeleport);
				if (myGroup.HasPermission("Ban")) AddChild(bBan);
				if (myGroup.HasPermission("Kick")) AddChild(bKick);
				if (myGroup.HasPermission("Snoop"))
				{
					snoopWindow.SetPlayer(Main.player[playerIndex]);
					AddChild(snoopWindow);
					Width = snoopWindow.X + snoopWindow.Width + spacing * 2;
					Height = snoopWindow.Y + snoopWindow.Height + spacing * 2;
				}
			}
			if (myGroup.IsAdmin)
			{
				AddChild(lGroup);
				AddChild(dropdown);
				if (offlineUser || player != null && player.Username.Length > 0)
				{
					//AddChild(bRestore);
				}
			}
			bBan.Tag = Main.player[playerIndex].name;
			bKick.Tag = Main.player[playerIndex].name;

			bClose.X = Width - bClose.Width - spacing;
			bKick.onLeftClick += bKick_onLeftClick;
			bBan.onLeftClick += bBan_onLeftClick;
			bClose.onLeftClick += bClose_onLeftClick;
			bTeleport.onLeftClick += bTeleport_onLeftClick;
			bRestore.onLeftClick += bRestore_onLeftClick;
		}

		private void bRestore_onLeftClick(object sender, EventArgs e)
		{
			Main.NewText("Notice: this feature does not work with recent Tile changes");
			return;
			if (player != null)
			{
				HEROsModNetwork.GeneralMessages.RequestRestoreTiles(playerIndex, true);
			}
			else
			{
				HEROsModNetwork.GeneralMessages.RequestRestoreTiles(playerIndex, false);
			}
		}

		private void bBan_onLeftClick(object sender, EventArgs e)
		{
			//ServerTools.SendTextToServer(Messages.banPlayer + name);
			UIButton button = (UIButton)sender;
			string tag = (string)button.Tag;
			ModUtils.DebugText("Ban tag " + tag);

			HEROsModNetwork.GeneralMessages.RequestBanPlayer(tag);
		}

		private void bTeleport_onLeftClick(object sender, EventArgs e)
		{
			if (HEROsModNetwork.LoginService.MyGroup.HasPermission("TeleportToPlayers"))
			{
				Main.player[Main.myPlayer].Teleport(Main.player[playerIndex].position);
			}
		}

		private void bSnoop_onLeftClick(object sender, EventArgs e)
		{
			/*
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].name == name)
                {
                    SnoopWindow snoopWindow = new SnoopWindow();
                    this.Parent.Parent.AddChild(snoopWindow);
                    snoopWindow.SetPlayer(Main.player[i]);
                }
            }
             */
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			this.Parent.RemoveChild(this);
		}

		private void dropdown_selectedChanged(object sender, EventArgs e)
		{
			if (player == null)
			{
				HEROsModNetwork.Group playersNewGroup = HEROsModNetwork.Network.GetGroupByName(dropdown.GetItem(dropdown.SelectedItem));
				HEROsModNetwork.LoginService.RequestSetOfflinePlayerGroup(playerIndex, playersNewGroup);
			}
			else
			{
				//send new group to server
				HEROsModNetwork.Group playersNewGroup = HEROsModNetwork.Network.GetGroupByName(dropdown.GetItem(dropdown.SelectedItem));
				HEROsModNetwork.LoginService.RequestSetPlayerGroup(playerIndex, playersNewGroup);
			}
		}

		private void bKick_onLeftClick(object sender, EventArgs e)
		{
			UIButton button = (UIButton)sender;
			string tag = (string)button.Tag;
			ModUtils.DebugText("Kick tag " + tag);
			HEROsModNetwork.GeneralMessages.RequestKickPlayer(tag);
			//ServerTools.SendTextToServer(Messages.kickPlayer + tag);
			//TODO
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
		{
			this.X = Parent.Width;
			base.Draw(spriteBatch);
		}
	}

	internal class SnoopWindow : UIView
	{
		//static float spacing = 16f;
		private Player player;

		private UIWindow itemsView;

		public SnoopWindow()
		{
			itemsView = new UIWindow();
			//itemsView.X = spacing;
			//itemsView.Y = spacing;
			for (int i = 0; i < 50; i++)
			{
				Slot slot = new Slot(0, false);
				slot.functionalSlot = true;
				int index = i;
				slot.ItemChanged += (a, b) => ItemSlot_ItemChanged(slot, index);
				slot.X = 8 + i % 10 * slot.Width;
				slot.Y = 8 + i / 10 * slot.Height;

				itemsView.AddChild(slot);
			}
			Width = itemsView.GetLastChild().X + itemsView.GetLastChild().Width + 8;
			float yPos = itemsView.GetLastChild().Y + itemsView.GetLastChild().Height + 10;
			for (int i = 50; i < 58; i++)
			{
				int index = i - 50;
				Slot slot = new Slot(0, false);
				slot.functionalSlot = true;
				int idx = i;
				slot.ItemChanged += (a, b) => ItemSlot_ItemChanged(slot, idx);
				slot.Scale = .6f;
				slot.X = 8 + index % 2 * slot.Width;
				slot.Y = yPos + index / 2 * slot.Height;
				itemsView.AddChild(slot);
			}
			Slot mouseSlot = new Slot(0, false);
			mouseSlot.functionalSlot = true;
			mouseSlot.ItemChanged += (a, b) => ItemSlot_ItemChanged(mouseSlot, 58);
			mouseSlot.X = itemsView.GetLastChild().X + itemsView.GetLastChild().Width + 4;
			mouseSlot.Y = itemsView.GetLastChild().Y + itemsView.GetLastChild().Height - mouseSlot.Height;
			itemsView.AddChild(mouseSlot);
			float xPos = mouseSlot.X + mouseSlot.Width + 4;
			for (int i = 0; i < 16; i++)
			{
				Slot slot = new Slot(0, false);
				slot.functionalSlot = true;
				int index = i;
				slot.ItemChanged += (a, b) => ItemSlot_ItemChanged(slot, index, true);
				slot.Scale = .7f;
				slot.X = xPos + i % 8 * slot.Width;
				slot.Y = yPos + i / 8 * slot.Height;
				itemsView.AddChild(slot);
			}
			AddChild(itemsView);
			Height = mouseSlot.Y + mouseSlot.Height + 8;

			itemsView.Width = Width;
			itemsView.Height = Height;

			UILabel label = new UILabel("Mouse Item");
			label.Scale = .5f;
			label.Anchor = AnchorPosition.Left;
			label.Y = mouseSlot.Y + mouseSlot.Height / 2 + 4;
			label.X = mouseSlot.X + mouseSlot.Width + 4;
			itemsView.AddChild(label);
		}

		private void ItemSlot_ItemChanged(Slot slot, int index, bool armor = false)
		{
			Main.playerInventory = true;
			RequestSyncItemNonOwner(player, slot.item, index + (armor ? 59 : 0));
			//ErrorLogger.Log("Slot " + slot.item.type);
		}

		public override void Update()
		{
			if (player != null)
			{
				for (int i = 0; i < 59; i++)
				{
					Slot slot = (Slot)itemsView.children[i];
					slot.item = player.inventory[i].Clone();
				}
				for (int i = 0; i < 16; i++)
				{
					Slot slot = (Slot)itemsView.children[i + 59];
					slot.item = player.armor[i].Clone();
				}
			}
			base.Update();
		}

		public void SetPlayer(Player player)
		{
			this.player = player;
		}

		internal static void RequestSyncItemNonOwner(Player player, Item item, int index)
		{
			var packet = HEROsMod.instance.GetPacket();
			packet.Write((byte)Network.MessageType.GeneralMessage);
			packet.Write((byte)GeneralMessages.MessageType.SyncItemNonOwner);
			packet.Write((byte)player.whoAmI);
			packet.Write((byte)index);
			Terraria.ModLoader.IO.ItemIO.Send(item, packet, true);
			packet.Send();
		}

		internal static void ProcessSyncItemNonOwner(ref BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == Terraria.ID.NetmodeID.Server)
			{
				if (Network.Players[playerNumber].Group.HasPermission("Snoop"))
				{
					byte player = reader.ReadByte();
					int inventoryindex = reader.ReadByte();
					Item item = Terraria.ModLoader.IO.ItemIO.Receive(reader, true);

					var packet = HEROsMod.instance.GetPacket();
					packet.Write((byte)Network.MessageType.GeneralMessage);
					packet.Write((byte)GeneralMessages.MessageType.SyncItemNonOwner);
					packet.Write((byte)player);
					packet.Write((byte)inventoryindex);
					Terraria.ModLoader.IO.ItemIO.Send(item, packet, true);
					packet.Send(player);
				}
			}
			else
			{
				byte player = reader.ReadByte();
				int inventoryindex = reader.ReadByte();
				Item item = Terraria.ModLoader.IO.ItemIO.Receive(reader, true);

				if (player == Main.myPlayer)
				{
					if (inventoryindex < 59)
						Main.LocalPlayer.inventory[inventoryindex] = item;
					else
						Main.LocalPlayer.armor[inventoryindex - 59] = item;
					// send 5 or just let clientClone take care of it?
				}
				else
				{
					// Bug
				}
			}
		}
	}
}