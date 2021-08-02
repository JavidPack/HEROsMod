using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class SpawnPointSetter : HEROsModService
	{
		public SpawnPointSetter(UIHotbar hotbar)
		{
			IsInHotbar = true;
			HotbarParent = hotbar;
			this._name = "Spawn Point Setter";
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/spawn", AssetRequestMode.ImmediateLoad)/*Main.itemTexture[69]*/);
			this._hotbarIcon.Tooltip = HEROsMod.HeroText("SetSpawnPoint");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.IsAdmin;
			//base.MyGroupUpdated();
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (ModUtils.NetworkMode == NetworkMode.None)
			{
				//this.position.X = (float)(Main.spawnTileX * 16 + 8 - this.width / 2);
				//this.position.Y = (float)(Main.spawnTileY * 16 - this.height);

				Player player = Main.player[Main.myPlayer];

				Main.spawnTileX = (int)(player.position.X - 8 + player.width / 2) / 16;
				Main.spawnTileY = (int)(player.position.Y + player.height) / 16;

				Main.NewText(string.Format(HEROsMod.HeroText("SpawnPointSetToXY"), Main.spawnTileX, Main.spawnTileY));
			}
			else
			{
				HEROsModNetwork.GeneralMessages.RequestSetSpawnPoint();
			}
		}
	}
}