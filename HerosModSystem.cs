using HEROsMod.HEROsModServices;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace HEROsMod
{
	public class HerosModSystem : ModSystem
	{
		public override void PostDrawFullscreenMap(ref string mouseText)
		{
			Teleporter.instance.PostDrawFullScreenMap(ref mouseText);
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

							HEROsMod.Draw(Main.spriteBatch);
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

			int rulerLayerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Ruler"));
			if (rulerLayerIndex != -1)
			{
				layers.Insert(rulerLayerIndex, new LegacyGameInterfaceLayer(
					"HerosMod: Game Scale UI",
					delegate {
						HEROsMod.DrawWorldUI(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.Game)
				);
			}
		}

		public override void PreSaveAndQuit()
		{
			HEROsMod.instance.prefixEditor.PreSaveAndQuit();
		}

		//public override void UpdateMusic(ref int music, ref AVF priority)
		//{
		//	HEROsMod.CheckIfGameEnteredOrLeft();
		//	//Console.WriteLine("?");
		//	//KeybindController.DoPreviousKeyState();
		//}

		public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
		{
			if (HEROsModNetwork.Network.CheckIncomingDataForHEROsModMessage(ref messageType, ref reader, playerNumber))
			{
				//ErrorLogger.Log("Hijacking: " + messageType);
				return true;
			}
			return false;
		}

		public override void PreUpdateEntities()
		{
			//Handle callbacks here as this is called on all sides
			//May need to filter allside vs clientside callbacks

			//Only godmode for now
			GodModeService.InvokeGodModeCallback();
		}		
	}
}
