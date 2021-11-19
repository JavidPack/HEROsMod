using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	//class WaypointsModWorld : ModWorld
	//{
	//	public override bool Autoload(ref string name) => true;
	//	public override void Initialize()
	//	{
	//		points.Clear();
	//	}
	//	public static List<Waypoint> points = new List<Waypoint>();
	//	private const int saveVersion = 0;
	//	public override void SaveCustomData(BinaryWriter writer)
	//	{
	//		writer.Write(saveVersion);
	//		writer.Write(points.Count); //Number of waypoints
	//		for (int i = 0; i < points.Count; i++)
	//		{
	//			writer.Write(points[i].name);
	//			writer.WriteVector2(points[i].position);
	//			//binaryWriter.Write(points[i].position.Y);
	//		}
	//		//binaryWriter.Close();
	//	}

	//	public override void LoadCustomData(BinaryReader reader)
	//	{
	//		int loadVersion = reader.ReadInt32();
	//		if (loadVersion == 0)
	//		{
	//			int numOfWaypoints = reader.ReadInt32();
	//			for (int i = 0; i < numOfWaypoints; i++)
	//			{
	//				string name = reader.ReadString();
	//				Vector2 location = reader.ReadVector2();
	//				//float X = binaryReader.ReadSingle();
	//				//float Y = binaryReader.ReadSingle();
	//				//AddWaypoint(name, new Vector2(X, Y));
	//				points.Add(new Waypoint(name, location));
	//			}
	//		}
	//	}
	//}

	internal class Waypoints : HEROsModService
	{
		private static WaypointWindow waypointWindow;
		public static List<Waypoint> points = new List<Waypoint>();

		public Waypoints()
		{
			this._name = "Waypoints";
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/waypointIcon", AssetRequestMode.ImmediateLoad));
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("ViewWaypoints");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

			waypointWindow = new WaypointWindow();
			waypointWindow.Visible = false;
			this.AddUIView(waypointWindow);
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			waypointWindow.Visible = !waypointWindow.Visible;
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("AccessWaypoints");
			if (!HasPermissionToUse)
			{
				waypointWindow.Visible = false;
			}
			//base.MyGroupUpdated();
		}

		//public static void RequestAddWayPoint()
		//{
		//}

		public static void ClearPoints()
		{
			points.Clear();
			if (waypointWindow != null) waypointWindow.UpdateWaypointList();
		}

		public static bool AddWaypoint(string name, Vector2 position)
		{
			for (int i = 0; i < points.Count; i++)
			{
				if (points[i].name == name)
				{
					return false;
				}
			}
			Waypoint waypoint = new Waypoint(name, position);
			points.Add(waypoint);
			if (waypointWindow != null) waypointWindow.UpdateWaypointList();
			return true;
		}

		public static void RequestRemoveWaypoint()
		{
		}

		public static void RemoveWaypoint(int index)
		{
			points.RemoveAt(index);
			if (ModUtils.NetworkMode != NetworkMode.Server)
			{
				if (waypointWindow != null) waypointWindow.UpdateWaypointList();
			}
		}

		public override void Destroy()
		{
			points.Clear();
			base.Destroy();
		}

		//public static void Save()
		//{
		//	if (points.Count > 0)
		//	{
		//		string path = Main.worldPathName + ".waypoints";
		//		using (FileStream fileStream = new FileStream(path, FileMode.Create))
		//		{
		//			using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
		//			{
		//				binaryWriter.Write(points.Count); //Number of waypoints
		//				for (int i = 0; i < points.Count; i++)
		//				{
		//					binaryWriter.Write(points[i].name);
		//					binaryWriter.Write(points[i].position.X);
		//					binaryWriter.Write(points[i].position.Y);
		//				}
		//				binaryWriter.Close();
		//			}
		//		}
		//	}
		//}

		//public static void Load()
		//{
		//	if (points.Count > 0)
		//	{
		//		points.Clear();
		//		if (waypointWindow != null) waypointWindow.UpdateWaypointList();
		//	}
		//	string path = Main.worldPathName + ".waypoints";

		//	if (File.Exists(path))
		//	{
		//		using (FileStream fileStream = new FileStream(path, FileMode.Open))
		//		{
		//			using (BinaryReader binaryReader = new BinaryReader(fileStream))
		//			{
		//				int numOfWaypoints = binaryReader.ReadInt32();
		//				for (int i = 0; i < numOfWaypoints; i++)
		//				{
		//					string name = binaryReader.ReadString();
		//					float X = binaryReader.ReadSingle();
		//					float Y = binaryReader.ReadSingle();
		//					AddWaypoint(name, new Vector2(X, Y));
		//				}
		//				binaryReader.Close();
		//			}
		//		}
		//	}
		//}
	}

	internal class Waypoint
	{
		public string name;
		public Vector2 position;

		public Waypoint(string name, Vector2 position)
		{
			this.name = name;
			this.position = position;
		}
	}

	internal class WaypointWindow : UIWindow
	{
		private static float spacing = 8f;
		private static bool prevGameMenu = true;
		private UIScrollView scrollView;

		public WaypointWindow()
		{
			X = 50;
			Y = 100;
			this.CanMove = true;

			UILabel title = new UILabel(HEROsMod.HeroText("Waypoints"));
			title.Scale = .6f;
			title.X = spacing;
			title.Y = spacing;
			title.OverridesMouse = false;

			Height = 300 + title.Height + spacing * 2;

			UIButton bAddWaypoint = new UIButton(HEROsMod.HeroText("AddWaypoint"));
			UIButton bClose = new UIButton(Language.GetTextValue("LegacyInterface.71"));
			bAddWaypoint.Anchor = AnchorPosition.BottomRight;
			bClose.Anchor = AnchorPosition.BottomRight;
			bClose.onLeftClick += bClose_onLeftClick;
			bAddWaypoint.onLeftClick += bAddWaypoint_onLeftClick;
			Width = bClose.Width + bAddWaypoint.Width + spacing * 3;
			bClose.Position = new Vector2(Width - spacing, Height - spacing);
			bAddWaypoint.Position = new Vector2(bClose.Position.X - bClose.Width - spacing, bClose.Position.Y);

			scrollView = new UIScrollView();
			scrollView.Position = new Vector2(spacing, spacing);
			scrollView.Y = title.Y + title.Height + spacing;
			scrollView.X = title.X;
			scrollView.Width = Width - spacing * 2;
			scrollView.Height = Height - scrollView.Y - spacing * 2 - bClose.Height;

			AddChild(title);
			AddChild(bClose);
			AddChild(bAddWaypoint);
			AddChild(scrollView);
		}

		private void bAddWaypoint_onLeftClick(object sender, EventArgs e)
		{
			MasterView.gameScreen.AddChild(new NameWaypointWindow(Main.player[Main.myPlayer].position));
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}

		public override void Update()
		{
			if (Main.gameMenu && prevGameMenu)
			{
				Waypoints.points.Clear();
				UpdateWaypointList();
			}
			prevGameMenu = Main.gameMenu;
			base.Update();
		}

		public void UpdateWaypointList()
		{
			scrollView.ClearContent();
			float yPos = spacing;
			for (int i = 0; i < Waypoints.points.Count; i++)
			{
				UILabel label = new UILabel(Waypoints.points[i].name);
				label.Scale = .5f;
				label.X = spacing;
				label.Y = yPos;
				label.Tag = i;
				label.onLeftClick += label_onLeftClick;
				yPos += label.Height;
				scrollView.AddChild(label);

				UIImage image = new UIImage(closeTexture);
				image.ForegroundColor = Color.Red;
				image.Anchor = AnchorPosition.Right;
				image.Position = new Vector2(scrollView.Width - 10 - spacing, label.Position.Y + label.Height / 2);
				image.Tag = i;
				image.onLeftClick += image_onLeftClick;

				scrollView.AddChild(image);
			}
			scrollView.ContentHeight = scrollView.GetLastChild().Y + scrollView.GetLastChild().Height + spacing;
		}

		private void image_onLeftClick(object sender, EventArgs e)
		{
			UIImage image = (UIImage)sender;
			int waypointIndex = (int)image.Tag;
			if (ModUtils.NetworkMode == NetworkMode.None)
			{
				Waypoints.RemoveWaypoint(waypointIndex);
			}
			else
			{
				HEROsModNetwork.GeneralMessages.RequestRemoveWaypoint(waypointIndex);
			}
		}

		private void label_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			int waypointIndex = (int)label.Tag;
			Waypoint waypoint = Waypoints.points[waypointIndex];
			Main.player[Main.myPlayer].Teleport(waypoint.position);
		}
	}

	internal class NameWaypointWindow : UIWindow
	{
		private UILabel label = null;
		private UITextbox textbox = null;
		private static float spacing = 8f;
		private Vector2 waypointPos;

		public NameWaypointWindow(Vector2 waypointPos)
		{
			this.waypointPos = waypointPos;
			UIView.exclusiveControl = this;

			Width = 600;
			Height = 100;
			this.Anchor = AnchorPosition.Center;

			label = new UILabel(HEROsMod.HeroText("WaypointName"));
			textbox = new UITextbox();
			UIButton bSave = new UIButton(Language.GetTextValue("UI.Save"));
			UIButton bCancel = new UIButton(Language.GetTextValue("UI.Cancel"));

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

				if (ModUtils.NetworkMode == NetworkMode.None)
				{
					if (!Waypoints.AddWaypoint(textbox.Text, waypointPos))
					{
						UIMessageBox mb = new UIMessageBox(HEROsMod.HeroText("WaypointAlreadyExistsNote"), UIMessageBoxType.Ok, true);
						this.AddChild(mb);
					}
					else
					{
						Close();
					}
				}
				else
				{
					HEROsModNetwork.GeneralMessages.RequestAddWaypoint(textbox.Text, waypointPos);
					Close();
				}
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
			if (Main.gameMenu) Close();
			if (Parent != null)
				this.Position = new Vector2(Parent.Width / 2, Parent.Height / 2);
			base.Update();
		}
	}
}