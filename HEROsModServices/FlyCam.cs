using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using System;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class FlyCam : HEROsModService
	{
		/* ====Hooks=====
         * FlyCam requires a that you wrap the code that locks the camera
         * to the players position in an if statement.  This code is in
         * the Draw method of the main class.  Refer to previous implementations
         * look for 'if(!HEROsMod.ModUtils.FreeCamera)'
         */

		internal static Vector2 FlyCamPosition = Vector2.Zero;

		internal static bool Enabled { get; set; }

		/// <summary>
		/// A service that allows the player to take controll of the camera
		/// </summary>
		public FlyCam()
		{
			Enabled = false;
			this._name = "Fly Camera";
			this._hotbarIcon = new UIKit.UIImage(Main.itemTexture[493]);
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = "Enable Fly Camera";
			//Make sure FreeCamera is off by default
			Disable();
		}

		private void Enable()
		{
			this._hotbarIcon.Opacity = 1f;
			Enabled = true;
			this.HotbarIcon.Tooltip = "Disable Fly Camera";
		}

		private void Disable()
		{
			this._hotbarIcon.Opacity = .5f;
			Enabled = false;
			this.HotbarIcon.Tooltip = "Enable Fly Camera";
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			Enabled = !Enabled;
			if (Enabled)
			{
				Enable();
			}
			else
			{
				Disable();
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("Flycam");
			if (!HasPermissionToUse)
			{
				Enabled = false;
			}
			base.MyGroupUpdated();
		}

		public override void Update()
		{
			if (Enabled && !Main.blockInput)
			{
				//move camera with arrow keys
				float speed = 30f;

				if (Main.keyState.IsKeyDown(Keys.LeftAlt)) speed *= .3f;
				if (Main.keyState.IsKeyDown(Keys.LeftShift)) speed *= 1.5f;

				if (PlayerInput.Triggers.Current.KeyStatus["Left"]) FlyCamPosition.X -= speed;
				if (PlayerInput.Triggers.Current.KeyStatus["Right"]) FlyCamPosition.X += speed;
				if (PlayerInput.Triggers.Current.KeyStatus["Up"]) FlyCamPosition.Y -= speed;
				if (PlayerInput.Triggers.Current.KeyStatus["Down"]) FlyCamPosition.Y += speed;

				//Vector2 size = new Vector2(Main.screenWidth, Main.screenHeight);
				//Main.screenPosition = FlyCamPosition - size / 2;

				//float x = (Main.mouseX + Main.screenPosition.X) / 16f;
				//float y = (Main.mouseY + Main.screenPosition.Y) / 16f;
				Player player = Main.player[Main.myPlayer];

				//if player right clicks, move their character to that position.
				if (!Main.mapFullscreen && !player.mouseInterface && Main.mouseRight)
				{
					Vector2 cursorPosition = new Vector2(Main.mouseX - player.width / 2, Main.mouseY - player.height);
					Vector2 cursorWorldPosition = Main.screenPosition + cursorPosition;

					int mapWidth = Main.maxTilesX * 16;
					int mapHeight = Main.maxTilesY * 16;
					if (cursorWorldPosition.X < 0) cursorWorldPosition.X = 0;
					else if (cursorWorldPosition.X + player.width > mapWidth) cursorWorldPosition.X = mapWidth - player.width;
					if (cursorWorldPosition.Y < 0) cursorWorldPosition.Y = 0;
					else if (cursorWorldPosition.Y + player.height > mapHeight) cursorWorldPosition.Y = mapHeight - player.height;

					player.position = cursorWorldPosition;
					player.velocity = Vector2.Zero;
					player.fallStart = (int)(player.position.Y / 16f);
				}
			}
			else
			{
				//Vector2 size = new Vector2(Main.screenWidth, Main.screenHeight);
				FlyCamPosition = Main.screenPosition;// + size / 2;
			}
			base.Update();
		}

		public override void Destroy()
		{
			//Make sure we turn free camera off when the service is removed
			Disable();
			base.Destroy();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (Enabled)
			{
				Main.spriteBatch.DrawString(Main.fontMouseText, "Right Click to teleport", new Vector2(15, Main.screenHeight - 120), Color.White);
				Main.spriteBatch.DrawString(Main.fontMouseText, "Arrow Keys to pan, Shift for faster, Alt for slower", new Vector2(15, Main.screenHeight - 90), Color.White);
			}
		}
	}

	public class FlyCamModPlayer : ModPlayer
	{
		public override bool Autoload(ref string name) => true;

		public override void ModifyScreenPosition()
		{
			if (FlyCam.Enabled)
			{
				Main.screenPosition = FlyCam.FlyCamPosition;
			}
		}
	}
}