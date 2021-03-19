using HEROsMod.HEROsModNetwork;
using HEROsMod.HEROsModServices;
using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace HEROsMod
{
	internal class HEROsModModWorld : ModWorld
	{
		public override bool Autoload(ref string name) => true;

		//private const int saveVersion = 0;

		// When a world is loaded on Server or client, we need to load settings.
		public override void Initialize()
		{
			HEROsModNetwork.DatabaseController.InitializeWorld();
			HEROsModNetwork.Network.InitializeWorld();
		}

		public override TagCompound Save()
		{
			//if (Main.dedServ) // What about clients? do they save?
			{
				HEROsModNetwork.DatabaseController.SaveSetting();
			}
			return null;
		}

		public override void PostDrawTiles()
		{
			if (RegionService.RegionsVisible || SelectionTool.Visible || CheckTileModificationTool.ListeningForInput)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, 
DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
				if (SelectionTool.Visible)
					SelectionTool.Draw(Main.spriteBatch);
				if (RegionService.RegionsVisible)
					RegionService.DrawRegions(Main.spriteBatch);
				if(CheckTileModificationTool.ListeningForInput)
					CheckTileModificationTool.DrawBoxOnCursor(Main.spriteBatch);
				Main.spriteBatch.End();
			}
		}
	}
}
