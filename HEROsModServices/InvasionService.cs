using HEROsMod.UIKit;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class InvasionService : HEROsModService
	{
		//invasion types
		// 0 - None
		// 1 - Goblin Army
		// 2 - Frost Legion
		// 3 - Pirates

		private EventWindow _eventWindow;

		public InvasionService()
		{
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/event", AssetRequestMode.ImmediateLoad)/*Main.itemTexture[14]*/);
			this._hotbarIcon.Tooltip = HEROsMod.HeroText("OpenEventStarter");
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;

			_eventWindow = new EventWindow();
			_eventWindow.Visible = false;
			this.AddUIView(_eventWindow);
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			_eventWindow.Visible = !_eventWindow.Visible;
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("StartEvents");
			//base.MyGroupUpdated();
		}

		public static void StopAllEvents()
		{
			if (ModUtils.NetworkMode != NetworkMode.Client)
			{
				Main.bloodMoon = false;
				Main.invasionType = 0;
				Main.eclipse = false;
				Main.stopMoonEvent();
				EnemyToggler.ClearNPCs();
				Main.NewText(HEROsMod.HeroText("AllEventsHaveBeenStopped"));
			}
			else
			{
				HEROsModNetwork.GeneralMessages.RequestStopEvents();
			}
		}

		private static void StartBloodMoon()
		{
			if (Main.dayTime)
			{
				Main.dayTime = false;
				Main.time = 0.0;
			}
			Main.bloodMoon = true;
		}

		private static void StartSolarEclipse()
		{
			if (!Main.dayTime)
			{
				Main.dayTime = true;
				Main.time = 0.0;
			}
			Main.eclipse = true;
		}

		private static void StartInvasion(int type)
		{
			int numberOfPlayers = 0;
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					numberOfPlayers++;
				}
			}
			Main.invasionType = type;
			Main.invasionSize = 80 + 40 * numberOfPlayers;
			if (type == 3)
			{
				Main.invasionSize += 40 + 20 * numberOfPlayers;
			}
			Main.invasionWarn = 0;
			if (Main.rand.Next(2) == 0)
			{
				Main.invasionX = 0.0;
				return;
			}
			Main.invasionX = (double)Main.maxTilesX;
			ModUtils.InvasionWarning();
		}

		private static void StartGoblinArmyInvasion()
		{
			Main.invasionDelay = 0;
			StartInvasion(1);
		}

		private static void StartFrostLegionInvasion()
		{
			Main.invasionDelay = 0;
			StartInvasion(2);
		}

		private static void StartPirateInvasion()
		{
			Main.invasionDelay = 0;
			StartInvasion(3);
		}

		private static void StartPumpkinMoon()
		{
			if (Main.dayTime)
			{
				Main.dayTime = false;
				Main.time = 0.0;
			}
			Main.startPumpkinMoon();
		}

		private static void StartFrostMoon()
		{
			if (Main.dayTime)
			{
				Main.dayTime = false;
				Main.time = 0.0;
			}
			Main.startSnowMoon();
		}

		public static void StartEvent(Events e)
		{
			if (ModUtils.NetworkMode != NetworkMode.Client)
			{
				StopAllEvents();
				switch (e)
				{
					case Events.GoblinArmy:
						StartGoblinArmyInvasion();
						break;

					case Events.FrostLegion:
						StartFrostLegionInvasion();
						break;

					case Events.Pirates:
						StartPirateInvasion();
						break;

					case Events.SolarEclipse:
						StartSolarEclipse();
						break;

					case Events.BloodMoon:
						StartBloodMoon();
						break;

					case Events.PumpkinMoon:
						StartPumpkinMoon();
						break;

					case Events.FrostMoon:
						StartFrostMoon();
						break;
				}
			}
			else
			{
				HEROsModNetwork.GeneralMessages.RequestStartEvent(e);
			}
		}
	}

	internal class EventWindow : UIWindow
	{
		public EventWindow()
		{
			string[] buttonText = new string[]
			{
				"Goblin Invasion",
				"Frost Legion Invasion",
				"Pirate Invasion",
				"Solar Eclipse",
				"Blood Moon",
				"Pumpkin Moon",
				"Frost Moon"
			};

			this.CanMove = true;
			int buttonWidth = 175;

			UILabel lTitle = new UILabel(HEROsMod.HeroText("Events"));
			lTitle.Scale = .6f;
			lTitle.X = LargeSpacing;
			lTitle.Y = LargeSpacing;
			lTitle.OverridesMouse = false;
			AddChild(lTitle);

			UIImage bClose = new UIImage(closeTexture);
			bClose.X = buttonWidth + LargeSpacing - bClose.Width;
			bClose.Y = LargeSpacing;
			bClose.onLeftClick += bClose_onLeftClick;
			AddChild(bClose);

			UIButton[] buttons = new UIButton[buttonText.Length];
			float yPos = lTitle.Y + lTitle.Height + SmallSpacing;
			for (int i = 0; i < buttonText.Length; i++)
			{
				buttons[i] = new UIButton(buttonText[i]);
				buttons[i].AutoSize = false;
				buttons[i].Width = buttonWidth;
				buttons[i].X = LargeSpacing;
				buttons[i].Y = yPos;
				buttons[i].Tag = i;
				buttons[i].onLeftClick += EventWindow_onLeftClick;
				yPos += buttons[i].Height + Spacing;
				AddChild(buttons[i]);
			}

			UIButton bStopEvents = new UIButton(HEROsMod.HeroText("Stop Events"));
			bStopEvents.AutoSize = false;
			bStopEvents.Width = buttonWidth;
			bStopEvents.X = LargeSpacing;
			bStopEvents.Y = yPos + Spacing;
			bStopEvents.onLeftClick += bStopEvents_onLeftClick;
			AddChild(bStopEvents);

			this.Height = bStopEvents.Y + bStopEvents.Height + LargeSpacing;
			this.Width = buttonWidth + LargeSpacing * 2;
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}

		private void bStopEvents_onLeftClick(object sender, EventArgs e)
		{
			InvasionService.StopAllEvents();
		}

		private void EventWindow_onLeftClick(object sender, EventArgs e)
		{
			UIButton button = (UIButton)sender;
			Events ev = (Events)((int)button.Tag);
			InvasionService.StartEvent(ev);
		}
	}

	internal enum Events
	{
		GoblinArmy,
		FrostLegion,
		Pirates,
		SolarEclipse,
		BloodMoon,
		PumpkinMoon,
		FrostMoon
	}
}