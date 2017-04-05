//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiServices
//{

//    /// <summary>
//    /// A service that changes the time of day
//    /// </summary>
//    class TimeChanger : GameikiService
//    {
//        public static bool TimePaused { get; set; }
//        private static double _pausedTime = 0;
//        public static double PausedTime
//        {
//            get { return _pausedTime; }
//            set { _pausedTime = value; }
//        }
//        private TimeControlWindow timeWindow;
//        public TimeChanger()
//        {
//            TimePaused = false;
//            this._name = "Time Changer";
//            this._hotbarIcon = new UIImage(UIView.GetEmbeddedTexture("Images/sunIcon"));
//            this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
//            this.HotbarIcon.Tooltip = "Change Time";

//            timeWindow = new TimeControlWindow();
//            Gameiki.ServiceHotbar.AddChild(timeWindow);

//            GameikiNetwork.GeneralMessages.TimePausedOrResumedByServer += GeneralMessages_TimePausedOrResumedByServer;
//        }

//        void GeneralMessages_TimePausedOrResumedByServer(bool timePaused)
//        {
//            TimePaused = timePaused;
//            timeWindow.TimePausedOfResumed();
//        }

//        void _hotbarIcon_onLeftClick(object sender, EventArgs e)
//        {
//            timeWindow.Visible = !timeWindow.Visible;
//            if (timeWindow.Visible)
//            {
//                timeWindow.X = this._hotbarIcon.X + this._hotbarIcon.Width / 2 - timeWindow.Width / 2;
//                timeWindow.Y = -timeWindow.Height;
//            }
//        }

//        public override void Update()
//        {
//            if (ModUtils.NetworkMode == NetworkMode.None)
//            {
//                if (TimePaused)
//                {
//                    Main.time = PausedTime;
//                }
//            }
//            base.Update();
//        }

//        public static void ToggleTimePause()
//        {
//            TimePaused = !TimePaused;
//            if (TimePaused)
//            {
//                PausedTime = Main.time;
//            }
//        }

//        public override void MyGroupUpdated()
//        {
//            this.HasPermissionToUse = GameikiNetwork.LoginService.MyGroup.HasPermission("ChangeTime");
//            base.MyGroupUpdated();
//        }

//        public override void Destroy()
//        {
//            GameikiNetwork.GeneralMessages.TimePausedOrResumedByServer -= GeneralMessages_TimePausedOrResumedByServer;
//            TimePaused = false;
//            Gameiki.ServiceHotbar.RemoveChild(timeWindow);
//            base.Destroy();
//        }
//    }

//    class TimeControlWindow : UIWindow
//    {
//        static float spacing = 8f;
//        public UIImage bPause;
//        static Texture2D _playTexture;
//        static Texture2D _pauseTexture;
//        public static Texture2D playTexture
//        {
//            get
//            {
//                if (_playTexture == null) _playTexture = GetEmbeddedTexture("Images/speed1");
//                return _playTexture;
//            }
//        }
//        public static Texture2D pauseTexture
//        {
//            get
//            {
//                if (_pauseTexture == null) _pauseTexture = GetEmbeddedTexture("Images/speed0");
//                return _pauseTexture;
//            }
//        }
//        public TimeControlWindow()
//        {
//            Height = 55;
//            UpdateWhenOutOfBounds = true;

//            /*
//            UIImage[] speedButtons = new UIImage[5];
//            string[] speedNames = new string[]
//            {
//                "Pause",
//                "Normal Speed",
//                "2x Speed",
//                "10x Speed",
//                "100x Speed"
//            };
//            for (int i = 0; i < speedButtons.Length; i++)
//            {
//                speedButtons[i] = new UIImage(GetEmbeddedTexture("Images/speed" + i + "");
//                speedButtons[i].Tooltip = speedNames[i];
//                speedButtons[i].onLeftClick +=TimeControlWindow_onLeftClick;
//                //speedButtons[i].onClick += new EventHandler(CollapseBarController_onClick);
//                AddChild(speedButtons[i]);
//            }
//            speedButtons[0].Tag = 0;
//            speedButtons[1].Tag = 1;
//            speedButtons[2].Tag = 2;
//            speedButtons[3].Tag = 10;
//            speedButtons[4].Tag = 100;

//             */
//            UIImage nightButton = new UIImage(GetEmbeddedTexture("Images/moonIcon"));
//            nightButton.Tooltip = "Night";
//            nightButton.onLeftClick += nightButton_onLeftClick;
//            UIImage noonButton = new UIImage(GetEmbeddedTexture("Images/sunIcon"));
//            noonButton.Tooltip = "Noon";
//            noonButton.onLeftClick += noonButton_onLeftClick;
//            bPause = new UIImage(pauseTexture);
//            bPause.onLeftClick += bPause_onLeftClick;
//            AddChild(nightButton);
//            AddChild(noonButton);
//            AddChild(bPause);

//            float xPos = spacing;
//            for (int i = 0; i < children.Count; i++)
//            {
//                if (children[i].Visible)
//                {
//                    children[i].X = xPos;
//                    xPos += children[i].Width + spacing;
//                    children[i].Y = Height / 2 - children[i].Height / 2;
//                }
//            }
//            Width = xPos;
//        }

//        void bPause_onLeftClick(object sender, EventArgs e)
//        {
//            if (Main.netMode != 1)
//            {
//                TimeChanger.ToggleTimePause();
//                UIImage b = (UIImage)sender;
//                TimePausedOfResumed();
//            }
//            else
//            {
//                GameikiNetwork.GeneralMessages.ReqestTimeChange(GameikiNetwork.GeneralMessages.TimeChangeType.Pause);
//            }
//        }

//        public override void Update()
//        {
//            if (this.Visible)
//            {
//                if (!MouseInside)
//                {
//                    int mx = Main.mouseX;
//                    int my = Main.mouseY;
//                    float right = DrawPosition.X + Width;
//                    float left = DrawPosition.X;
//                    float top = DrawPosition.Y;
//                    float bottom = DrawPosition.Y + Height;
//                    float dist = 75f;
//                    bool outsideBounds = (mx > right && mx - right > dist) ||
//                                         (mx < left && left - mx > dist) ||
//                                         (my > bottom && my - bottom > dist) ||
//                                         (my < top && top - my > dist);
//                    if ((UIKit.UIView.MouseLeftButton && !MouseInside) || outsideBounds) this.Visible = false;
//                }
//            }
//            base.Update();
//        }

//        public void TimePausedOfResumed()
//        {
//            if (TimeChanger.TimePaused)
//            {
//                bPause.Texture = playTexture;
//            }
//            else bPause.Texture = pauseTexture;
//        }

//        void nightButton_onLeftClick(object sender, EventArgs e)
//        {
//            if (Main.netMode != 1)
//            {
//                Main.dayTime = false;
//				Main.time = 0;// 27000.0;
//            }
//            else
//            {
//                GameikiNetwork.GeneralMessages.ReqestTimeChange(GameikiNetwork.GeneralMessages.TimeChangeType.SetToNight);   
//            }
//        }

//        void noonButton_onLeftClick(object sender, EventArgs e)
//        {
//            if (Main.netMode != 1)
//            {
//                Main.dayTime = true;
//                Main.time = 27000.0;
//            }
//            else
//            {
//                GameikiNetwork.GeneralMessages.ReqestTimeChange(GameikiNetwork.GeneralMessages.TimeChangeType.SetToNoon);
//            }
//        }

//        void TimeControlWindow_onLeftClick(object sender, EventArgs e)
//        {
//            UIImage b = (UIImage)sender;
//            int rate = (int)b.Tag;
//            if (rate > 0)
//            {
//                //pauseTime = false;
//                Main.dayRate = (int)b.Tag;
//            }
//            else
//            {
//                //pauseTime = true;
//                //previousTime = Main.time;
//            }
//        }
//    }
//}
