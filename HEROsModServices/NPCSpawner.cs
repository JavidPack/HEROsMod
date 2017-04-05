//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using HEROsModMod.UIKit;

//using Terraria;

//namespace HEROsModMod.HEROsModServices
//{
//    class NPCSpawner : HEROsModService
//    {
//        NPCSpawnerWindow npcSpawnWindow;
//        public NPCSpawner()
//        {
//            this._name = "NPC Spawner";
//            this._hotbarIcon = new UIImage(Main.npcHeadTexture[2]);
//            this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
//            this.HotbarIcon.Tooltip = "Spawn NPC";

//            npcSpawnWindow = new NPCSpawnerWindow();
//            HEROsMod.ServiceHotbar.AddChild(npcSpawnWindow);
//        }

//        void _hotbarIcon_onLeftClick(object sender, EventArgs e)
//        {
//            npcSpawnWindow.Visible = !npcSpawnWindow.Visible;
//            if (npcSpawnWindow.Visible)
//            {
//                npcSpawnWindow.X = HEROsMod.ServiceHotbar.Width / 2 - npcSpawnWindow.Width / 2;
//                npcSpawnWindow.Y = -npcSpawnWindow.Height;
//            }
//        }

//        public override void MyGroupUpdated()
//        {
//            this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("SpawnNPCs");
//            base.MyGroupUpdated();
//        }

//        public override void Destroy()
//        {
//            HEROsMod.ServiceHotbar.RemoveChild(npcSpawnWindow);
//            base.Destroy();
//        }
//    }

//    class NPCSpawnerWindow : UIWindow
//    {
//        static float spacing = 8f;
//        static int[] npcIds = new int[] { 22, 17, 18, 38, 20, 19, 54, 124, 107, 108, 142, 160, 178, 207, 208, 209, 227, 228, 229, 353, 368, 369 };

//        public NPCSpawnerWindow()
//        {
//            Height = 55;
//            UpdateWhenOutOfBounds = true;

//            for (int i = 1; i < Main.npcHeadTexture.Length; i++)
//            {
//                NPC npc = new NPC();
//                npc.SetDefaults(npcIds[i - 1]);
//                UIImage image = new UIImage(Main.npcHeadTexture[i]);
//                image.Tooltip = npc.name;
//                image.Tag = npcIds[i - 1];
//                image.onLeftClick += image_onLeftClick;
//                AddChild(image);
//            }

//            float xPos = spacing;
//            for (int i = 0; i < children.Count; i++)
//            {
//                if (children[i].Visible)
//                {
//                    children[i].X = xPos;
//                    xPos += children[i].Width + spacing;
//                    children[i].Y = Height / 2 - children[i].Height / 2;
//                }
//            }
//            Width = xPos;
//        }

//        void image_onLeftClick(object sender, EventArgs e)
//        {

//            UIImage image = (UIImage)sender;
//            int npcID = (int)image.Tag;
//            bool npcFound = false;
//            if (Main.netMode == 1)
//            {
//                HEROsModNetwork.GeneralMessages.RequestSpawnTownNPC(npcID);
//                return;
//            }
//            Player p = Main.player[Main.myPlayer];
//            for (int i = 0; i < Main.npc.Length; i++)
//            {
//                NPC n = Main.npc[i];
//                if (n.type == npcID)
//                {
//                    n.position = p.position;
//                    npcFound = true;
//                    break;
//                }
//            }
//            if (!npcFound) NPC.NewNPC((int)p.position.X, (int)p.position.Y, npcID);
//        }

//        public override void Update()
//        {
//            if (this.Visible)
//            {
//                if (!MouseInside)
//                {
//                    int mx = Main.mouseX;
//                    int my = Main.mouseY;
//                    float right = DrawPosition.X + Width;
//                    float left = DrawPosition.X;
//                    float top = DrawPosition.Y;
//                    float bottom = DrawPosition.Y + Height;
//                    float dist = 75f;
//                    bool outsideBounds = (mx > right && mx - right > dist) ||
//                                         (mx < left && left - mx > dist) ||
//                                         (my > bottom && my - bottom > dist) ||
//                                         (my < top && top - my > dist);
//                    if ((UIKit.UIView.MouseLeftButton && !MouseInside) || outsideBounds) this.Visible = false;
//                }
//            }
//            base.Update();
//        }

//    }
//}
