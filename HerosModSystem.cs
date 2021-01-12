using HEROsMod.HEROsModServices;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace HEROsMod
{
	public class HerosModSystem : ModSystem
	{
		public override void PostDrawFullscreenMap(ref string mouseText)
		{
			Teleporter.instance.PostDrawFullScreenMap();
			MapRevealer.instance.PostDrawFullScreenMap();
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int inventoryLayerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (inventoryLayerIndex != -1)
			{
				layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
					"HerosMod: UI",
					delegate {
						try
						{
							HEROsMod.Update();

							HEROsMod.ServiceHotbar.Update();

							HEROsMod.DrawBehindUI(Main.spriteBatch);

							HEROsMod.Draw(Main.spriteBatch);

							KeybindController.DoPreviousKeyState();
						}
						catch (Exception e)
						{
							ModUtils.DebugText("PostDrawInInventory Error: " + e.Message + e.StackTrace);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

		public override void PreSaveAndQuit()
		{
			HEROsMod.instance.prefixEditor.PreSaveAndQuit();
		}

		public override void UpdateMusic(ref int music, ref MusicPriority priority)
		{
			HEROsMod.CheckIfGameEnteredOrLeft();
			//Console.WriteLine("?");
			//KeybindController.DoPreviousKeyState();
		}
	}
}
