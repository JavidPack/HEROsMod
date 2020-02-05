using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;

namespace HEROsMod.HEROsModVideo.Services.MobHUD
{
	internal class MobInfo
	{
		private static List<NPC> OnScreenNPCs = new List<NPC>();
		private static List<UIView> InfoViews = new List<UIView>();

		public static void Update()
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && NPCOnScreen(npc))
				{
					if (!OnScreenNPCs.Contains(npc))
					{
						//npc Moved on sceen
						OnScreenNPCs.Add(npc);
						MobInfoView infoView = new MobInfoView(npc);
						InfoViews.Add(infoView);
					}
				}
			}
			for (int i = 0; i < OnScreenNPCs.Count; i++)
			{
				if (!OnScreenNPCs[i].active || !NPCOnScreen(OnScreenNPCs[i]))
				{
					//npc left the screen
					OnScreenNPCs.RemoveAt(i);
					InfoViews.RemoveAt(i);
					i--;
				}
			}
			for (int i = 0; i < InfoViews.Count; i++)
			{
				InfoViews[i].Update();
			}
		}

		public static bool NPCOnScreen(NPC npc)
		{
			Rectangle screenRect = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
			Rectangle npcRect = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
			if (screenRect.Intersects(npcRect))
			{
				return true;
			}
			return false;
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < InfoViews.Count; i++)
			{
				InfoViews[i].Draw(spriteBatch);
			}
		}
	}
}