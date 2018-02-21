using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;

namespace HEROsMod.UIKit
{
	internal class MasterView : UIView
	{
		private static MouseState mouseState = Mouse.GetState();
		private static MouseState previousMouseState = Mouse.GetState();
		private static GameScreen _gameScreen = null;

		public static GameScreen gameScreen
		{
			get
			{
				if (_gameScreen == null)
				{
					_gameScreen = new GameScreen();
					AddChildToMaster(_gameScreen);
				}
				return _gameScreen;
			}
		}

		private static MenuScreen _menuScreen = null;

		public static MenuScreen menuScreen
		{
			get
			{
				if (_menuScreen == null)
				{
					_menuScreen = new MenuScreen();
					AddChildToMaster(_menuScreen);
				}
				return _menuScreen;
			}
		}

		private static MapScreen _mapScreen = null;

		public static MapScreen mapScreen
		{
			get
			{
				if (_mapScreen == null)
				{
					_mapScreen = new MapScreen();
					AddChildToMaster(_mapScreen);
				}
				return _mapScreen;
			}
		}

		protected override float GetWidth()
		{
			return Main.screenWidth;
		}

		protected override float GetHeight()
		{
			return Main.screenHeight;
		}

		private static MasterView masterView = new MasterView();

		public static void ClearMasterView()
		{
			masterView.children.Clear();
			_mapScreen = null;
			_menuScreen = null;
			_gameScreen = null;
		}

		public static void UpdateMaster()
		{
			mouseState = Mouse.GetState();
			UIView.MouseLeftButton = mouseState.LeftButton == ButtonState.Pressed;
			UIView.MousePrevLeftButton = previousMouseState.LeftButton == ButtonState.Pressed;
			UIView.MouseRightButton = mouseState.RightButton == ButtonState.Pressed;
			UIView.MousePrevRightButton = previousMouseState.RightButton == ButtonState.Pressed;
			UIView.ScrollAmount = PlayerInput.ScrollWheelDeltaForUI;
			// UIView.ScrollAmount = (mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue) / 2;
			previousMouseState = mouseState;
			//HoverItem = EmptyItem;
			HoverText = "";
			GameMouseOverwritten = false;
			masterView.Update();
		}

		public static void DrawMaster(SpriteBatch spriteBatch)
		{
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, null, null, Main.UIScaleMatrix);
			masterView.Draw(spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, null, null, Main.UIScaleMatrix);
			//Console.WriteLine("update: " + UpdateCalls + "draws: " + DrawCalls);
		}

		public static void AddChildToMaster(UIView view)
		{
			masterView.AddChild(view);
		}

		public static void RemoveChildFromMaster(UIView view)
		{
			masterView.RemoveChild(view);
		}

		public class GameScreen : UIView
		{
			public GameScreen()
			{
				this.OverridesMouse = false;
			}

			public override void Update()
			{
				if (!Main.gameMenu && !Main.mapFullscreen)
					this.Visible = true;
				else this.Visible = false;
				base.Update();
			}

			protected override float GetWidth()
			{
				return this.Parent.Width;
			}

			protected override float GetHeight()
			{
				return this.Parent.Height;
			}
		}

		public class MenuScreen : UIView
		{
			public MenuScreen()
			{
				this.OverridesMouse = false;
			}

			public override void Update()
			{
				this.Visible = Main.gameMenu;
				base.Update();
			}

			protected override float GetWidth()
			{
				return this.Parent.Width;
			}

			protected override float GetHeight()
			{
				return this.Parent.Height;
			}
		}

		public class MapScreen : UIView
		{
			public MapScreen()
			{
				this.OverridesMouse = false;
			}

			public override void Update()
			{
				this.Visible = !Main.gameMenu && Main.mapFullscreen;
				base.Update();
			}

			protected override float GetWidth()
			{
				return this.Parent.Width;
			}

			protected override float GetHeight()
			{
				return this.Parent.Height;
			}
		}
	}
}