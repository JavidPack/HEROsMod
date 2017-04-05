using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HEROsMod.UIKit;

using Terraria;

namespace HEROsMod.HEROsModServices
{
    class GodModeService : HEROsModService
    {
        delegate void GodModeToggledEvent(bool enabled, bool prevEnabled);
        static event GodModeToggledEvent GodModeToggled;

        static bool _enabled = false;
        public static bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (GodModeToggled != null)
                {
                    GodModeToggled(value, _enabled);
                }
                _enabled = value;
                
            }
        }
        public GodModeService()
        {
            this._hotbarIcon = new UIImage(UIView.GetEmbeddedTexture("Images/godMode")/*Main.itemTexture[1990]*/);
            this.HotbarIcon.Tooltip = "Toggle God Mode";
            this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
            GodModeToggled += GodModeService_GodModeToggled;
            Enabled = false;
        }

        void GodModeService_GodModeToggled(bool enabled, bool prevEnabled)
        {
            if(enabled)
            {
                if (enabled != prevEnabled)
                    Main.NewText("God Mode Enabled");
                this.HotbarIcon.Opacity = 1f;
            }
            else
            {
                if (enabled != prevEnabled)
                    Main.NewText("God Mode Disabled");
                this.HotbarIcon.Opacity = .5f;
            }
        }

        public override void MyGroupUpdated()
        {
            this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("GodMode");
            if(!HasPermissionToUse)
            {
                Enabled = false;
            }
            //base.MyGroupUpdated();
        }

        void HotbarIcon_onLeftClick(object sender, EventArgs e)
        {
            if(ModUtils.NetworkMode == NetworkMode.None)
            {
                Enabled = !Enabled;
            }
            else
            {
                if(!Enabled)
                {
                    HEROsModNetwork.GeneralMessages.RequestGodMode();
                }
                else
                {
                    Enabled = false;
                }
            }
        }

        public override void Destroy()
        {
            Enabled = false;
            GodModeToggled -= GodModeService_GodModeToggled;
            base.Destroy();
        }
    }
}
