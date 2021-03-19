using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;

namespace HEROsMod.HEROsModServices
{
	internal class CheckTileModificationTool : HEROsModService
	{
		internal static bool ListeningForInput = false;
		internal static Vector2 CheckCoordinates;

		public CheckTileModificationTool()
		{
			MultiplayerOnly = true;
			this._hotbarIcon = new UIImage(HEROsMod.instance.GetTexture("Images/tileModification")/*Main.itemTexture[1999]*/);
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("CheckTileForLastModification");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;
			this.HasPermissionToUse = true;
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			ListeningForInput = !ListeningForInput;
		}

		public override void UpdateGameScale()
		{
			if (ListeningForInput && !Main.gameMenu)
			{
				CheckCoordinates = ModUtils.CursorTileCoords;
				if (ModUtils.MouseState.LeftButton == ButtonState.Pressed && ModUtils.PreviousMouseState.LeftButton == ButtonState.Released && !UIKit.UIView.GameMouseOverwritten)
				{
					HEROsModNetwork.GeneralMessages.RequestTileModificationCheck(CheckCoordinates);
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
				ModUtils.DrawBorderedRect(spriteBatch, Color.Blue, CheckCoordinates, new Vector2(1, 1), 2);
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