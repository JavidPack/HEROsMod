using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Terraria;

namespace HEROsMod.HEROsModServices
{
	internal class SelectionTool
	{
		private static bool _dragging = false;
		private static Vector2 _anchor = Vector2.Zero;
		public static bool ListeningForInput { get; set; }
		public static bool Visible { get; set; }
		public static int X { get; set; }
		public static int Y { get; set; }

		public static Vector2 Position
		{
			get { return new Vector2(X, Y); }
			set
			{
				X = (int)value.X;
				Y = (int)value.Y;
			}
		}

		public static int Width { get; set; }
		public static int Height { get; set; }

		public static Vector2 Size
		{
			get { return new Vector2(Width, Height); }
			set
			{
				Width = (int)value.X;
				Height = (int)value.Y;
			}
		}

		public static void Init()
		{
			Visible = true;
			ListeningForInput = false;
		}

		public static void Reset()
		{
			X = 0;
			Y = 0;
			Width = 0;
			Height = 0;
			ListeningForInput = false;
			Visible = false;
		}

		public static void SetPositionWithCursorPosition()
		{
		}

		public static void Update()
		{
			if (ListeningForInput && !Main.gameMenu)
			{
				if (ModUtils.MouseState.LeftButton == ButtonState.Pressed && ModUtils.PreviousMouseState.LeftButton == ButtonState.Released && !UIKit.UIView.GameMouseOverwritten)
				{
					_dragging = true;
					Position = ModUtils.CursorTileCoords;
					_anchor = Position;
				}
				else if (_dragging && ModUtils.MouseState.LeftButton == ButtonState.Pressed)
				{
					Vector2 tileCoords = ModUtils.CursorTileCoords;
					Size = tileCoords - _anchor;
					if (Width < 0)
					{
						Width = -Width;
						X = (int)_anchor.X - Width;
					}
					if (Height < 0)
					{
						Height = -Height;
						Y = (int)_anchor.Y - Height;
					}
					Width++;
					Height++;
				}
				else if (ModUtils.MouseState.LeftButton == ButtonState.Released && ModUtils.PreviousMouseState.LeftButton == ButtonState.Pressed)
				{
					_dragging = false;
				}
				UIKit.UIView.OverWriteGameMouseInput();
			}
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			if (Visible)
			{
				ModUtils.DrawBorderedRect(spriteBatch, Color.Blue, Position, Size, 3, Width + "x" + Height);
				//Vector2 pos = ModUtils.GetWorldCoordsFromTileCoords(Position) - Main.screenPosition;
				//spriteBatch.Draw(ModUtils.DummyTexture, new Rectangle((int)pos.X, (int)pos.Y, Width * 16, Height * 16), Color.Blue * .5f);
			}
		}
	}
}