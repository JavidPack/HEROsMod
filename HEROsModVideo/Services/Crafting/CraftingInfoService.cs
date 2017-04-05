//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.GameikiServices;
//using GameikiMod.GameikiVideo.Services;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.Crafting
//{
//    class CraftingInfoService : GameikiService
//    {
//        CraftingWindow _craftingWindow;
//        public CraftingInfoService()
//        {
//            this._hotbarIcon = new UIKit.UIImage(Main.itemTexture[765]);
//            this.HotbarIcon.onLeftClick +=HotbarIcon_onLeftClick;
//            this.HotbarIcon.Tooltip = "Crafting Window";

//            this._craftingWindow = new CraftingWindow();
//            _craftingWindow.Visible = false;
//            _craftingWindow.CenterToParent();
//            _craftingWindow.Position -= new Vector2(_craftingWindow.Width / 2, _craftingWindow.Height / 2);
//            this.AddUIView(_craftingWindow);
//        }

//        void HotbarIcon_onLeftClick(object sender, EventArgs e)
//        {
//            _craftingWindow.Visible = !_craftingWindow.Visible;
//        }
//    }
//}
