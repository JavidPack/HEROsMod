using HEROsMod.HEROsModServices;
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
