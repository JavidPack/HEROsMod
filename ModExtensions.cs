//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HEROsMod
//{
//	class ModExtensions
//	{
//		internal List<ModExtension> ModExtensionList = new List<ModExtension>();

//		public ModExtensions()
//		{
//			ModExtensionList = new List<ModExtension>();
//		}

//		internal void AddButton(Texture2D texture, Action buttonClickedAction, Action groupUpdated, Func<string> tooltip)
//		{
//			ModExtensionList.Add(new ModExtension(buttonClickedAction, groupUpdated, texture, tooltip));
//		}
//	}

//	class ModExtension
//	{
//		Action ButtonClicked;
//		Action GroupUpdated;
//		Texture2D ButtonTexture;
//		Func<string> ButtonTooltip;

//		public ModExtension(Action buttonClicked, Action groupUpdated, Texture2D buttonTexture, Func<string> buttonTooltip)
//		{
//			ButtonClicked = buttonClicked;
//			GroupUpdated = groupUpdated;
//			ButtonTexture = buttonTexture;
//			ButtonTooltip = buttonTooltip;
//		}
//	}
//}
///*

//	Client Only
//Buttons: Texture, Tooltip, Click Action

 

//*/