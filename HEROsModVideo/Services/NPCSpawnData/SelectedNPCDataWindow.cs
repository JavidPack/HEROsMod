//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.NPCSpawnData
//{
//    class SelectedNPCDataWindow : UIWindow
//    {
//        List<int> spawnedNPCTypes;
//        public SelectedNPCDataWindow()
//        {
//            this.Width = 250;
//            this.Height = 300;
//            this.CanMove = true;
//            spawnedNPCTypes = new List<int>();

//            for(int i = 0; i < NPCSpawnDataBuilder.spawnConditions.Count; i++)
//            {
//                spawnedNPCTypes.Add(NPCSpawnDataBuilder.spawnConditions[i].NPCID);
//            }

//            UIScrollView scrollView = new UIScrollView();
//            float yPos = Spacing;
//            for(int i = 0; i < spawnedNPCTypes.Count; i++)
//            {
//                UILabel label = new UILabel(Lang.npcName(spawnedNPCTypes[i]));
//                label.Scale = .4f;
//                label.Tag = spawnedNPCTypes[i];
//                label.X = Spacing;
//                label.Y = yPos;
//                label.Tooltip = NPCSpawnDataBuilder.spawnConditions[i].ToString();
//                yPos += label.Height;
//                scrollView.AddChild(label);
//                label.onLeftClick += label_onLeftClick;
//            }
//            scrollView.ContentHeight = yPos;
//            scrollView.X = Spacing;
//            scrollView.Y = Spacing;
//            scrollView.Width = this.Width - Spacing * 2;
//            scrollView.Height = this.Height - Spacing * 2;

//            AddChild(scrollView);

//        }

//        void label_onLeftClick(object sender, EventArgs e)
//        {
//            UILabel label = (UILabel)sender;
//            int npcID = (int)label.Tag;

//            NPCSpawnDataBuilder.VisibleSpawnPositions = new List<Vector2>();
//            for (int i = 0; i < NPCSpawnDataBuilder.SpawnAreas.Count; i++)
//            {
//                SpawnAreaInfo spawnArea = NPCSpawnDataBuilder.SpawnAreas[i];
//                for (int j = 0; j < spawnArea.NPCs.Count; j++)
//                {
//                    if(spawnArea.NPCs[j].NetID == npcID)
//                    {
//                        NPCSpawnDataBuilder.VisibleSpawnPositions.Add(spawnArea.NPCs[j].Position);
//                    }
//                }
//            }
//        }
//    }
//}
