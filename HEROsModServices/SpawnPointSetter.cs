using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HEROsMod.UIKit;

using Terraria;
using HEROsMod.UIKit.UIComponents;

namespace HEROsMod.HEROsModServices
{
    class SpawnPointSetter : HEROsModService
    {
        public SpawnPointSetter(UIHotbar hotbar)
		{
			IsInHotbar = true;
			HotbarParent = hotbar;
			this._name = "Spawn Point Setter";
            this._hotbarIcon = new UIImage(UIView.GetEmbeddedTexture("Images/spawn")/*Main.itemTexture[69]*/);
            this._hotbarIcon.Tooltip = "Set Spawn Point";
            this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
        }

        public override void MyGroupUpdated()
        {
            this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.IsAdmin;
            //base.MyGroupUpdated();
        }

        void HotbarIcon_onLeftClick(object sender, EventArgs e)
        {
            if(ModUtils.NetworkMode == NetworkMode.None)
            {
                //this.position.X = (float)(Main.spawnTileX * 16 + 8 - this.width / 2);
                //this.position.Y = (float)(Main.spawnTileY * 16 - this.height);

                Player player = Main.player[Main.myPlayer];

                Main.spawnTileX = (int)(player.position.X - 8 + player.width / 2) / 16;
                Main.spawnTileY= (int)(player.position.Y + player.height) / 16;

                Main.NewText(string.Format("Spawn Point set to X:{0} Y:{1}", Main.spawnTileX, Main.spawnTileY));
            }
            else
            {
                HEROsModNetwork.GeneralMessages.RequestSetSpawnPoint();
            }
        }
    }
}
