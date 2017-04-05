//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Graphics;
//using System.IO;
//using System.Text.RegularExpressions;

//using Terraria;
//using Terraria.ModLoader;

//namespace HEROsModMod.HEROsModServices
//{
//	class ZoomToolsService : HEROsModService
//	{
//		public class ZoomToolsServiceModPlayer : ModPlayer
//		{
//			public override bool Autoload(ref string name) => true;

//			int oldX;
//			int oldY;
//			public override void PreUpdate()
//			{
//				oldX = Main.mouseX;
//				oldY = Main.mouseY;
//				if (player.whoAmI == Main.myPlayer)
//				{
//					AdjustMousePosition();
//				}
//			}
//			public override void PostUpdate()
//			{
//				if (player.whoAmI == Main.myPlayer)
//				{
//					Main.mouseX = oldX;
//					Main.mouseY = oldY;
//				}
//			}
//		}

//		public static Matrix OffsetMatrix
//		{
//			get
//			{
//				float transX = Main.screenWidth / 2;
//				float transY = Main.screenHeight / 2;

//				return Matrix.Identity
//					* Matrix.CreateTranslation(-Main.screenWidth / 2, -Main.screenHeight / 2, 0f)
//					* Matrix.CreateScale(Scale, Scale, 1f)
//					* Matrix.CreateTranslation(transX, transY, 0f);
//			}
//		}

//		private static int _mouseXBackup;
//		private static int _mouseYBackup;

//		static KeyBinding _zoomInKeyBinding;
//		static KeyBinding _zoomOutKeyBinding;

//		static float Scale = 1.5f;

//		public ZoomToolsService()
//		{
//			//SetKeyBindings();
//			Scale = 1f;
//		}

//		public static void SetKeyBindings()
//		{
//			_zoomInKeyBinding = KeybindController.AddKeyBinding("Zoom Camera In", "OemPlus");
//			_zoomOutKeyBinding = KeybindController.AddKeyBinding("Zoom Camera Out", "OemMinus");
//		}

//		public static Vector2 AdjustMousePosition()
//		{
//			_mouseXBackup = Main.mouseX;
//			_mouseYBackup = Main.mouseY;

//			Vector2 mousePos = new Vector2(ModUtils.MouseState.X, ModUtils.MouseState.Y);
//			mousePos = Vector2.Transform(mousePos, Matrix.Invert(OffsetMatrix));


//			Main.mouseX = (int)mousePos.X;
//			Main.mouseY = (int)mousePos.Y;

//			return mousePos;
//		}

//		public static void RestoreMousePosition()
//		{
//			Main.mouseX = _mouseXBackup;
//			Main.mouseY = _mouseYBackup;
//		}

//		static float[] scales = new float[] { .25f, .5f, .75f, 1f, 1.25f, 1.5f, 2f, 3f, 5f };
//		public override void Update()
//		{
//			if (_zoomInKeyBinding.KeyPressed)
//			{
//			//	ErrorLogger.Log("Zoom in prssed");
//				for (int i = 0; i < scales.Length; i++)
//				{
//					if (Scale < scales[i])
//					{
//						Scale = scales[i];
//						Main.NewText("Zoom Level: " + Scale);
//						break;
//					}
//				}
//			}
//			if (_zoomOutKeyBinding.KeyPressed)
//			{
//			//	ErrorLogger.Log("Zoom out down");
//				for (int i = scales.Length - 1; i >= 0; i--)
//				{
//					if (Scale > scales[i])
//					{
//						Scale = scales[i];
//						Main.NewText("Zoom Level: " + Scale);
//						break;
//					}
//				}
//			}
//			base.Update();
//		}


//	}
//}
