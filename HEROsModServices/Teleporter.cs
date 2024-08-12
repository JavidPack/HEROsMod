using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace HEROsMod.HEROsModServices
{
	internal class Teleporter : HEROsModService
	{
		// TODO, is this how I want to do this?
		public static Teleporter instance;
		private bool rightClickEnabled = true; // TODO: Remember this setting maybe?
		private bool rightMouseHadBeenReleased;

		public Teleporter()
		{
			instance = this;
		}

		public override void Update()
		{
			// This method coincidentally only called when fullscreen is closed.
			rightMouseHadBeenReleased = false;

			//if (Main.mapFullscreen)
			//{
			//	if (this.HasPermissionToUse)
			//	{
			//		if (Main.mouseRight && Main.keyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			//		{
			//			int mapWidth = Main.maxTilesX * 16;
			//			int mapHeight = Main.maxTilesY * 16;
			//			Vector2 cursorPosition = new Vector2(Main.mouseX, Main.mouseY);

			//			cursorPosition.X -= Main.screenWidth / 2;
			//			cursorPosition.Y -= Main.screenHeight / 2;

			//			Vector2 mapPosition = Main.mapFullscreenPos;
			//			Vector2 cursorWorldPosition = mapPosition;

			//			cursorPosition /= 16;
			//			cursorPosition *= 16 / Main.mapFullscreenScale;
			//			cursorWorldPosition += cursorPosition;
			//			cursorWorldPosition *= 16;

			//			Player player = Main.player[Main.myPlayer];
			//			cursorWorldPosition.Y -= player.height;
			//			if (cursorWorldPosition.X < 0) cursorWorldPosition.X = 0;
			//			else if (cursorWorldPosition.X + player.width > mapWidth) cursorWorldPosition.X = mapWidth - player.width;
			//			if (cursorWorldPosition.Y < 0) cursorWorldPosition.Y = 0;
			//			else if (cursorWorldPosition.Y + player.height > mapHeight) cursorWorldPosition.Y = mapHeight - player.height;
			//			player.position = cursorWorldPosition;
			//			player.velocity = Vector2.Zero;
			//			player.fallStart = (int)(player.position.Y / 16f);
			//		}
			//	}
			//}
			base.Update();
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("Teleport");
			//base.MyGroupUpdated();
		}

		public void PostDrawFullScreenMap(ref string mouseText)
		{
			// TODO: Detect MapLayer clicks?
			if (HasPermissionToUse)
			{
				// Toggle Button
				Texture2D inventoryTickTexture = TextureAssets.InventoryTickOn.Value;
				if (!rightClickEnabled)
					inventoryTickTexture = TextureAssets.InventoryTickOff.Value;
				Rectangle enableButtonRectangle = new Rectangle(17, Main.screenHeight - 72, inventoryTickTexture.Width, inventoryTickTexture.Height);

				if (enableButtonRectangle.Contains(Main.mouseX, Main.mouseY))
				{
					Main.LocalPlayer.mouseInterface = true;
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						rightClickEnabled = !rightClickEnabled;
						Main.mouseLeftRelease = false;
						Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
					}
					mouseText = Language.GetTextValue(rightClickEnabled ? "GameUI.Enabled" : "GameUI.Disabled");
				}
				var stringSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, HEROsMod.HeroText("RightClickToTeleport"), Vector2.One);

				Utils.DrawInvBG(Main.spriteBatch, new Rectangle(13, Main.screenHeight - 80, (int)stringSize.X + 30, (int)stringSize.Y)/*, new Color(63, 65, 151, 255)*/);
				Main.spriteBatch.Draw(inventoryTickTexture, enableButtonRectangle.TopLeft(), Color.White);

				Main.spriteBatch.DrawString(FontAssets.MouseText.Value, HEROsMod.HeroText("RightClickToTeleport"), new Vector2(36, Main.screenHeight - 77), Color.White);
				Terraria.GameInput.PlayerInput.SetZoom_Unscaled();

				rightMouseHadBeenReleased |= !Main.mouseRight;
				if (rightMouseHadBeenReleased && rightClickEnabled && Main.mouseRight && Main.keyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.LeftControl))
				{
					int mapWidth = Main.maxTilesX * 16;
					int mapHeight = Main.maxTilesY * 16;
					Vector2 cursorPosition = new Vector2(Main.mouseX, Main.mouseY);

					cursorPosition.X -= Main.screenWidth / 2f;
					cursorPosition.Y -= Main.screenHeight / 2f;

					Vector2 mapPosition = Main.mapFullscreenPos;
					Vector2 cursorWorldPosition = mapPosition;

					cursorPosition /= 16;
					cursorPosition *= 16 / Main.mapFullscreenScale;
					cursorWorldPosition += cursorPosition;
					cursorWorldPosition *= 16;

					Player player = Main.player[Main.myPlayer];
					cursorWorldPosition.Y -= player.height;
					if (cursorWorldPosition.X < 0) cursorWorldPosition.X = 0;
					else if (cursorWorldPosition.X + player.width > mapWidth) cursorWorldPosition.X = mapWidth - player.width;
					if (cursorWorldPosition.Y < 0) cursorWorldPosition.Y = 0;
					else if (cursorWorldPosition.Y + player.height > mapHeight) cursorWorldPosition.Y = mapHeight - player.height;

					if (Main.netMode == NetmodeID.SinglePlayer) // single
					{
						player.Teleport(cursorWorldPosition, 1, 0);
						player.position = cursorWorldPosition;
						player.velocity = Vector2.Zero;
						player.fallStart = (int)(player.position.Y / 16f);
					}
					else // 1, client
					{
						//ErrorLogger.Log("Teleport");
						HEROsModNetwork.GeneralMessages.RequestTeleport(cursorWorldPosition);
						//NetMessage.SendData(65, -1, -1, "", 0, player.whoAmI, cursorWorldPosition.X, cursorWorldPosition.Y, 1, 0, 0);
					}
					FlyCam.ForceReset = true;
				}
				Terraria.GameInput.PlayerInput.SetZoom_UI();
			}
		}
	}
}