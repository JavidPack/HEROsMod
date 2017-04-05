using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terraria;

namespace HEROsMod.UIKit
{
    class UIScreen : UIView
    {
        public UIScreen()
        {
            this.OverridesMouse = false;
        }

        protected override float GetWidth()
        {
            return Main.screenWidth;
        }
        protected override float GetHeight()
        {
            return Main.screenHeight;
        }
    }
}
