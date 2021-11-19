using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class TimeWeatherChanger : HEROsModService
	{
		public static bool TimePaused { get; set; }

		public static double PausedTime { get; set; } = 0;

		//	public static bool PausedTimeDayTime = false;
		private TimeWeatherControlHotbar timeWeatherHotbar;

		public TimeWeatherChanger()
		{
			IsHotbar = true;

			TimePaused = false;
			this._name = "Time Weather Control";
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/timeRain", AssetRequestMode.ImmediateLoad));
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("ChangeTimeRain");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

			//timeWeatherHotbar = new TimeWeatherControlHotbar();
			//HEROsMod.ServiceHotbar.AddChild(timeWeatherHotbar);

			timeWeatherHotbar = new TimeWeatherControlHotbar();
			timeWeatherHotbar.HotBarParent = HEROsMod.ServiceHotbar;
			timeWeatherHotbar.Hide();
			this.AddUIView(timeWeatherHotbar);

			Hotbar = timeWeatherHotbar;

			HEROsModNetwork.GeneralMessages.TimePausedOrResumedByServer += GeneralMessages_TimePausedOrResumedByServer;
		}

		private void GeneralMessages_TimePausedOrResumedByServer(bool timePaused)
		{
			TimePaused = timePaused;
			timeWeatherHotbar.TimePausedOfResumed();
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (timeWeatherHotbar.selected)
			{
				timeWeatherHotbar.selected = false;
				timeWeatherHotbar.Hide();
			}
			else
			{
				timeWeatherHotbar.selected = true;
				timeWeatherHotbar.Show();
			}

			//timeWeatherHotbar.Visible = !timeWeatherHotbar.Visible;
			//if (timeWeatherHotbar.Visible)
			//{
			//	timeWeatherHotbar.X = this._hotbarIcon.X + this._hotbarIcon.Width / 2 - timeWeatherHotbar.Width / 2;
			//	timeWeatherHotbar.Y = -timeWeatherHotbar.Height;
			//}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("ChangeTimeWeather");
			if (!HasPermissionToUse)
			{
				timeWeatherHotbar.Hide();
			}
			//base.MyGroupUpdated();
		}

		public override void Destroy()
		{
			HEROsModNetwork.GeneralMessages.TimePausedOrResumedByServer -= GeneralMessages_TimePausedOrResumedByServer;
			TimePaused = false;
			HEROsMod.ServiceHotbar.RemoveChild(timeWeatherHotbar);
			base.Destroy();
		}

		public static void ToggleTimePause()
		{
			TimePaused = !TimePaused;
			if (TimePaused)
			{
				PausedTime = Main.time;
			}
		}

		public override void Update()
		{
			if (ModUtils.NetworkMode == NetworkMode.None)
			{
				if (TimePaused)
				{
					Main.time = PausedTime;
				}
			}
			base.Update();
		}
	}

	internal class TimeWeatherControlHotbar : UIHotbar
	{
		//	static float spacing = 8f;

		public UIImage bPause;
		private static Asset<Texture2D> _playTexture;
		private static Asset<Texture2D> _pauseTexture;

		//static Texture2D _rainTexture;
		//public static Texture2D rainTexture
		//{
		//	get
		//	{
		//		if (_rainTexture == null) _rainTexture = HEROsMod.instance.GetTexture("Images/rainIcon");
		//		return _rainTexture;
		//	}
		//}
		public static Asset<Texture2D> playTexture
		{
			get
			{
				if (_playTexture == null) _playTexture = HEROsMod.instance.Assets.Request<Texture2D>("Images/speed1", AssetRequestMode.ImmediateLoad);
				return _playTexture;
			}
		}

		public static Asset<Texture2D> pauseTexture
		{
			get
			{
				if (_pauseTexture == null) _pauseTexture = HEROsMod.instance.Assets.Request<Texture2D>("Images/speed0", AssetRequestMode.ImmediateLoad);
				return _pauseTexture;
			}
		}

		public TimeWeatherControlHotbar()
		{
			buttonView = new UIView();
			Height = 54;
			UpdateWhenOutOfBounds = true;

			buttonView.Height = Height;
			Anchor = AnchorPosition.Top;
			AddChild(buttonView);
		}

		public override void RefreshHotbar()
		{
			Height = 54;
			UpdateWhenOutOfBounds = true;

			UIImage bStopRain = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/rainStop", AssetRequestMode.ImmediateLoad));
			UIImage bStartRain = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/rainIcon", AssetRequestMode.ImmediateLoad));
			bStartRain.Tooltip = HEROsMod.HeroText("StartRain");
			bStopRain.Tooltip = HEROsMod.HeroText("StopRain");
			bStartRain.onLeftClick += bStartRain_onLeftClick;
			bStopRain.onLeftClick += bStopRain_onLeftClick;
			buttonView.AddChild(bStopRain);
			buttonView.AddChild(bStartRain);

			UIImage bStopSandstorm = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/rainStop", AssetRequestMode.ImmediateLoad));
			UIImage bStartSandstorm = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/rainIcon", AssetRequestMode.ImmediateLoad));
			bStartSandstorm.Tooltip = HEROsMod.HeroText("StartSandstorm");
			bStopSandstorm.Tooltip = HEROsMod.HeroText("StopSandstorm");
			bStartSandstorm.onLeftClick += bStartSandstorm_onLeftClick;
			bStopSandstorm.onLeftClick += bStopSandstorm_onLeftClick;
			buttonView.AddChild(bStopSandstorm);
			buttonView.AddChild(bStartSandstorm);

			UIImage nightButton = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/moonIcon", AssetRequestMode.ImmediateLoad));
			nightButton.Tooltip = HEROsMod.HeroText("Night");
			nightButton.onLeftClick += nightButton_onLeftClick;
			UIImage noonButton = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sunIcon", AssetRequestMode.ImmediateLoad));
			noonButton.Tooltip = HEROsMod.HeroText("Noon");
			noonButton.onLeftClick += noonButton_onLeftClick;
			bPause = new UIImage(TimeWeatherChanger.TimePaused ? playTexture : pauseTexture);
			bPause.onLeftClick += bPause_onLeftClick;
			bPause.Tooltip = TimeWeatherChanger.TimePaused ? HEROsMod.HeroText("ResumeTime") : HEROsMod.HeroText("PauseTime");// "Toggle Freeze Time";

			buttonView.AddChild(nightButton);
			buttonView.AddChild(noonButton);
			buttonView.AddChild(bPause);

			UIImage sundialButton = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/timeRain", AssetRequestMode.ImmediateLoad));
			sundialButton.Tooltip = HEROsMod.HeroText("ForceEnchantedSundial");
			sundialButton.onLeftClick += sundialButton_onLeftClick;
			buttonView.AddChild(sundialButton);

			//float xPos = spacing;
			//for (int i = 0; i < children.Count; i++)
			//{
			//	if (children[i].Visible)
			//	{
			//		children[i].X = xPos;
			//		xPos += children[i].Width + spacing;
			//		children[i].Y = Height / 2 - children[i].Height / 2;
			//	}
			//}
			//Width = xPos;

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
			//this.Resize();
			base.Width = num;
			this.buttonView.Width = base.Width;
		}

		//public void Resize()
		//{
		//	float num = this.spacing;
		//	for (int i = 0; i < this.buttonView.children.Count; i++)
		//	{
		//		if (this.buttonView.children[i].Visible)
		//		{
		//			this.buttonView.children[i].X = num;
		//			num += this.buttonView.children[i].Width + this.spacing;
		//		}
		//	}
		//	base.Width = num;
		//	this.buttonView.Width = base.Width;
		//}

		private void sundialButton_onLeftClick(object sender, EventArgs e)
		{
			if (Main.netMode == 1) // Client
			{
				HEROsModNetwork.GeneralMessages.RequestForcedSundial();
			}
			else // Single
			{
				Main.fastForwardTime = true;
				Main.sundialCooldown = 0;
				//NetMessage.SendData(7, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
			}

			//if (/*!Main.fastForwardTime &&*/ (Main.netMode == 1 || Main.sundialCooldown == 0))
			//{
			//	if (Main.sundialCooldown == 0)
			//	{
			//		if (Main.netMode == 1)
			//		{
			//			NetMessage.SendData(51, -1, -1, "", Main.myPlayer, 3f, 0f, 0f, 0, 0, 0);
			//			return;
			//		}
			//		Main.fastForwardTime = true;
			//		Main.sundialCooldown = 8;
			//		NetMessage.SendData(7, -1, -1, "", 0, 0f, 0f, 0f, 0, 0, 0);
			//	}
			//}
		}

		private void bPause_onLeftClick(object sender, EventArgs e)
		{
			if (Main.netMode != 1)
			{
				TimeWeatherChanger.ToggleTimePause();
				UIImage b = (UIImage)sender;
				TimePausedOfResumed();
				if(TimeWeatherChanger.TimePaused)
					Main.NewText(HEROsMod.HeroText("TimeHasBeenPaused"));
				else
					Main.NewText(HEROsMod.HeroText("TimeHasResumed"));
			}
			else
			{
				HEROsModNetwork.GeneralMessages.ReqestTimeChange(HEROsModNetwork.GeneralMessages.TimeChangeType.Pause);
			}
		}

		private void bStopRain_onLeftClick(object sender, EventArgs e)
		{
			if (Main.netMode == 1)
			{
				HEROsModNetwork.GeneralMessages.RequestStopRain();
				return;
			}
			Main.NewText(HEROsMod.HeroText("RainHasBeenTurnedOff"));

			ModUtils.StopRain();
		}

		private void bStartRain_onLeftClick(object sender, EventArgs e)
		{
			if (Main.netMode == 1)
			{
				HEROsModNetwork.GeneralMessages.RequestStartRain();
				return;
			}
			Main.NewText(HEROsMod.HeroText("RainHasBeenTurnedOn"));
			ModUtils.StartRain();
		}

		private void bStopSandstorm_onLeftClick(object sender, EventArgs e)
		{
			if (Main.netMode == 1)
			{
				HEROsModNetwork.GeneralMessages.RequestStopSandstorm();
				return;
			}
			Main.NewText(HEROsMod.HeroText("SandstormHasBeenTurnedOff"));

			ModUtils.StopSandstorm();
		}

		private void bStartSandstorm_onLeftClick(object sender, EventArgs e)
		{
			if (Main.netMode == 1)
			{
				HEROsModNetwork.GeneralMessages.RequestStartSandstorm();
				return;
			}
			Main.NewText(HEROsMod.HeroText("SandstormHasBeenTurnedOn"));
			ModUtils.StartSandstorm();
		}

		public void TimePausedOfResumed()
		{
			if (TimeWeatherChanger.TimePaused)
			{
				bPause.Texture = playTexture;
			}
			else
			{
				bPause.Texture = pauseTexture;
			}
			bPause.Tooltip = TimeWeatherChanger.TimePaused ? HEROsMod.HeroText("ResumeTime") : HEROsMod.HeroText("PauseTime");
		}

		private void nightButton_onLeftClick(object sender, EventArgs e)
		{
			if (Main.netMode != 1)
			{
				Main.dayTime = false;
				Main.time = 0;// 27000.0;
			}
			else
			{
				HEROsModNetwork.GeneralMessages.ReqestTimeChange(HEROsModNetwork.GeneralMessages.TimeChangeType.SetToNight);
			}
		}

		private void noonButton_onLeftClick(object sender, EventArgs e)
		{
			if (Main.netMode != 1)
			{
				Main.dayTime = true;
				Main.time = 27000.0;
			}
			else
			{
				HEROsModNetwork.GeneralMessages.ReqestTimeChange(HEROsModNetwork.GeneralMessages.TimeChangeType.SetToNoon);
			}
		}

		private void TimeControlWindow_onLeftClick(object sender, EventArgs e)
		{
			UIImage b = (UIImage)sender;
			int rate = (int)b.Tag;
			if (rate > 0)
			{
				//pauseTime = false;
				Main.dayRate = (int)b.Tag;
			}
			else
			{
				//pauseTime = true;
				//previousTime = Main.time;
			}
		}

		internal static void Unload()
		{
			_playTexture = null;
			_pauseTexture = null;
		}

		//public override void Update()
		//{
		//	if (this.Visible)
		//	{
		//		if (!MouseInside)
		//		{
		//			int mx = Main.mouseX;
		//			int my = Main.mouseY;
		//			float right = DrawPosition.X + Width;
		//			float left = DrawPosition.X;
		//			float top = DrawPosition.Y;
		//			float bottom = DrawPosition.Y + Height;
		//			float dist = 75f;
		//			bool outsideBounds = (mx > right && mx - right > dist) ||
		//								 (mx < left && left - mx > dist) ||
		//								 (my > bottom && my - bottom > dist) ||
		//								 (my < top && top - my > dist);
		//			if ((UIKit.UIView.MouseLeftButton && !MouseInside) || outsideBounds) this.Visible = false;
		//		}
		//	}
		//	base.Update();
		//}
	}
}