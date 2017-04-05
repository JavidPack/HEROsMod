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
//    class WeatherChanger : GameikiService
//    {
//        private WeatherControlWindow weatherWindow;
//        public WeatherChanger()
//        {
//            this._name = "Weather Control";
//            this._hotbarIcon = new UIImage(WeatherControlWindow.rainTexture);
//            this.HotbarIcon.Tooltip = "Control Rain";
//            this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

//            weatherWindow = new WeatherControlWindow();
//            Gameiki.ServiceHotbar.AddChild(weatherWindow);
//        }

//        void HotbarIcon_onLeftClick(object sender, EventArgs e)
//        {
//            weatherWindow.Visible = !weatherWindow.Visible;
//            if (weatherWindow.Visible)
//            {
//                weatherWindow.X = this._hotbarIcon.X + this._hotbarIcon.Width / 2 - weatherWindow.Width / 2;
//                weatherWindow.Y = -weatherWindow.Height;
//            }
//        }

//        public override void MyGroupUpdated()
//        {
//            this.HasPermissionToUse = GameikiNetwork.LoginService.MyGroup.HasPermission("ControlWeather");
//            base.MyGroupUpdated();
//        }

//        public override void Destroy()
//        {
//            Gameiki.ServiceHotbar.RemoveChild(weatherWindow);
//            base.Destroy();
//        }
//    }

//    class WeatherControlWindow : UIWindow
//    {
//        static float spacing = 8f;

//        static Texture2D _rainTexture;
//        public static Texture2D rainTexture
//        {
//            get
//            {
//                if (_rainTexture == null) _rainTexture = GetEmbeddedTexture("Images/rainIcon");
//                return _rainTexture;
//            }
//        }

//        public WeatherControlWindow()
//        {
//            Height = 55;
//            UpdateWhenOutOfBounds = true;

//            UIImage bStopRain = new UIImage(GetEmbeddedTexture("Images/sunIcon"));
//            UIImage bStartRain = new UIImage(rainTexture);
//            bStartRain.Tooltip = "Start Rain";
//            bStopRain.Tooltip = "Stop Rain";
//            bStartRain.onLeftClick += bStartRain_onLeftClick;
//            bStopRain.onLeftClick += bStopRain_onLeftClick;
//            AddChild(bStopRain);
//            AddChild(bStartRain);

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

//        void bStopRain_onLeftClick(object sender, EventArgs e)
//        {
//            if (Main.netMode == 1)
//            {
//                GameikiNetwork.GeneralMessages.RequestStopRain();
//                return;
//            }
//            ModUtils.StopRain();
//        }

//        void bStartRain_onLeftClick(object sender, EventArgs e)
//        {
//            if (Main.netMode == 1)
//            {
//                GameikiNetwork.GeneralMessages.RequestStartRain();
//                return;
//            }
//            ModUtils.StartRain();
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

//    }
//}
