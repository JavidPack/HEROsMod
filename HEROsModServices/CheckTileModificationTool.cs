using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class CheckTileModificationTool : HEROsModService
	{
		private static bool ListeningForInput = false;

		public CheckTileModificationTool()
		{
			MultiplayerOnly = true;
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/tileModification", AssetRequestMode.ImmediateLoad));
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("CheckTileForLastModification");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
			this.HasPermissionToUse = true;
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			ListeningForInput = !ListeningForInput;
		}

		public override void Update()
		{
			if (ListeningForInput && !Main.gameMenu)
			{
				if (ModUtils.MouseState.LeftButton == ButtonState.Pressed && ModUtils.PreviousMouseState.LeftButton == ButtonState.Released && !UIKit.UIView.GameMouseOverwritten)
				{
					HEROsModNetwork.GeneralMessages.RequestTileModificationCheck(ModUtils.CursorTileCoords);
				}
				if (ModUtils.MouseState.RightButton == ButtonState.Pressed && ModUtils.PreviousMouseState.RightButton == ButtonState.Released && !UIKit.UIView.GameMouseOverwritten)
				{
					ListeningForInput = false;
				}
				UIKit.UIView.OverWriteGameMouseInput();
			}
			base.Update();
		}

		public static void DrawBoxOnCursor(SpriteBatch spriteBatch)
		{
			if (ListeningForInput)
			{
				Vector2 pos = ModUtils.CursorWorldCoords;
				pos.X = (int)pos.X / 16 * 16;
				pos.Y = (int)pos.Y / 16 * 16;
				ModUtils.DrawBorderedRect(spriteBatch, Color.Blue, ModUtils.CursorTileCoords, new Vector2(1, 1), 2);
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("CheckTiles");
			//base.MyGroupUpdated();
		}

		public override void Destroy()
		{
			ListeningForInput = false;
			base.Destroy();
		}
	}
}