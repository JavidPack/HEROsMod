//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.NPCSpawnData
//{
//    class NPCSpawnDataBuilder
//    {
//        public static List<SpawnAreaInfo> SpawnAreas { get; set; }
//        public static List<Vector2> VisibleSpawnPositions { get; set; }
//        const int npcSlotToCheck = 20;

//        public static List<SpawnConditions> spawnConditions { get; set; }
//        public static void Start()
//        {

//            SpawnAreas = new List<SpawnAreaInfo>();
//            spawnConditions = new List<SpawnConditions>();
//            GameikiServices.GodModeService.Enabled = true;

//            DoRoutine3(true, false);
//            DoRoutine3(false, false);
//            DoRoutine3(true, true);
//            DoRoutine3(false, true);
            
//            for (int i = 0; i < SpawnAreas.Count; i++)
//            {
//                for(int j = 0; j < SpawnAreas[i].NPCs.Count; j++)
//                {
//                    NPCInfo npcInfo = SpawnAreas[i].NPCs[j];
//                    SpawnConditions sc = GetSpawnConditionForNPC(npcInfo.NetID);
//                    sc.AddData(SpawnAreas[i], npcInfo);
//                }
//            }
//            spawnConditions = spawnConditions.OrderBy(x => Lang.npcName(x.NPCID)).ToList();
//            SetAllSpawnsVisible();
//            UIKit.MasterView.gameScreen.AddChild(new SelectedNPCDataWindow());
//        }

//        public static void DoRoutine(bool day, bool hc)
//        {
//            Main.dayTime = day;
//            Main.time = 27000.0;
//            Main.hardMode = hc;

//            int moveDistance = 1000;
//            int xMoves = Main.maxTilesX * 16 / moveDistance;
//            int yMoves = Main.maxTilesY * 16 / moveDistance;
//            int numberOfMoves = xMoves * yMoves;
//            for (int y = 0; y < yMoves; y++)
//            {
//                for (int x = 0; x < xMoves; x++)
//                {
//                    MovePlayer(x * moveDistance, y * moveDistance);
//                    SpawnAreaInfo spawnArea = new SpawnAreaInfo(Main.player[Main.myPlayer]);
//                    for (int i = 0; i < 50; i++)
//                    {
//                        ClearNCPs();
//                        SpawnNPC();
//                        CheckNPCs(spawnArea);
//                    }
//                    SpawnAreas.Add(spawnArea);
//                    Console.Write((x * y) + "/" + numberOfMoves);
//                }
//            }
//        }
//        public static void DoRoutine2(bool day, bool hc)
//        {
//            Main.dayTime = day;
//            Main.time = 27000.0;
//            Main.hardMode = hc;

//            int moveDistance = 100;
//            int xMoves = Main.maxTilesX * 16 / moveDistance;
//            int yMoves = Main.maxTilesY * 16 / moveDistance;
//            int numberOfMoves = xMoves * yMoves;
//            for (int y = 0; y < yMoves; y++)
//            {
//                MovePlayer(3 * moveDistance, y * moveDistance);
//                for (int i = 0; i < 9; i++)
//                {
//                    SetBiome(i, Main.player[Main.myPlayer]);
//                    SpawnAreaInfo spawnArea = new SpawnAreaInfo(Main.player[Main.myPlayer]);
//                    for (int j = 0; j < 50; j++)
//                    {
//                        ClearNCPs();
//                        SpawnNPC();
//                        CheckNPCs(spawnArea);
//                    }
//                    SpawnAreas.Add(spawnArea);
//                }
                    
//            }
//        }

//        static void DoRoutine3(bool day, bool hc)
//        {
//            Main.dayTime = day;
//            Main.time = 27000.0;
//            Main.hardMode = hc;

//            int moveDistance = 1000;
//            int xMoves = Main.maxTilesX * 16 / moveDistance;
//            int yMoves = Main.maxTilesY * 16 / moveDistance;
//            int numberOfMoves = xMoves * yMoves;
//            for (int y = 0; y < yMoves; y++)
//            {
//                for (int x = 0; x < xMoves; x++)
//                {
//                    MovePlayer(x * moveDistance, y * moveDistance);
//                    for (int i = 0; i < 9; i++)
//                    {
//                        SetBiome(i, Main.player[Main.myPlayer]);
//                        SpawnAreaInfo spawnArea = new SpawnAreaInfo(Main.player[Main.myPlayer]);
//                        for (int j = 0; j < 50; j++)
//                        {
//                            ClearNCPs();
//                            SpawnNPC();
//                            CheckNPCs(spawnArea);
//                        }
//                        SpawnAreas.Add(spawnArea);
//                    }
//                }
//            }
//        }

//		// TODO, update biomes? New Biomes?
//        static void SetBiome(int biomeNum, Player player)
//        {
//            ClearBiomes(player);

//            switch(biomeNum)
//            {
//                case 1:
//                    player.ZoneCrimson = true;
//                    break;
//                case 2:
//                    player.ZoneWaterCandle = true;
//                    break;
//                case 3:
//                    player.ZoneDungeon = true;
//                    break;
//                case 4:
//                    player.ZoneCorrupt = true;
//                    break;
//                case 5:
//                    player.ZoneHoly = true;
//                    break;
//                case 6:
//                    player.ZoneJungle = true;
//                    break;
//                case 7:
//                    player.ZoneMeteor = true;
//                    break;
//                case 8:
//                    player.ZoneSnow = true;
//                    break;

//            }
//        }
//        static void ClearBiomes(Player player)
//        {
//            //player.ZoneBlood = false;
//            player.ZoneCrimson = false;
//			//player.ZoneCandle = false;
//            player.ZoneWaterCandle = false;
//			player.ZoneDungeon = false;
//            //player.ZoneEvil = false;
//            player.ZoneCorrupt = false;
//			player.ZoneHoly = false;
//            player.ZoneJungle = false;
//            player.ZoneMeteor = false;
//            player.ZoneSnow = false;
//        }

//        public static SpawnConditions GetSpawnConditionForNPC(int npcID)
//        {
//            for(int i = 0; i < spawnConditions.Count; i++)
//            {
//                if(spawnConditions[i].NPCID == npcID)
//                {
//                    return spawnConditions[i];
//                }
//            }
//            SpawnConditions sc = new SpawnConditions(npcID);
//            spawnConditions.Add(sc);
//            return sc;
//        }

//        static Vector2 GetDrawPosition(Vector2 pos)
//        {
//            Vector2 ogPos = pos;
//            pos /= 16f;
//            pos -= Main.mapFullscreenPos;
//            pos *= 16;
//            pos *= Main.mapFullscreenScale / 16;

//            pos.X += Main.screenWidth / 2;
//            pos.Y += Main.screenHeight / 2;
//            return pos;
//        }

//        public static void DrawOnMap(SpriteBatch spriteBatch)
//        {
//            if(Main.mapFullscreen)
//            {
//                for (int i = 0; i < VisibleSpawnPositions.Count; i++)
//                {
//                    Vector2 pos = GetDrawPosition(VisibleSpawnPositions[i]);
//                    spriteBatch.Draw(ModUtils.DummyTexture, pos, null, Color.Blue, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);

//                }
//            }
//        }

//        static void SetAllSpawnsVisible()
//        {
//            VisibleSpawnPositions = new List<Vector2>();
//            for (int i = 0; i < SpawnAreas.Count; i++)
//            {
//                SpawnAreaInfo spawnArea = SpawnAreas[i];
//                for (int j = 0; j < spawnArea.NPCs.Count; j++)
//                {
//                    VisibleSpawnPositions.Add(spawnArea.NPCs[j].Position);
//                }
//            }
//        }

//        static void MovePlayer(float x, float y)
//        {
//            Player player = Main.player[Main.myPlayer];
//            player.position = new Vector2(x, y);
//            //MoveCamera();
//            //Test();
//            //player.UpdateBiomes();
//        }

//        static void Test()
//        {
//            int firstTileX = (int)(Main.screenPosition.X / 16f - 1f);
//            int lastTileX = (int)((Main.screenPosition.X + (float)Main.screenWidth) / 16f) + 2;
//            int firstTileY = (int)(Main.screenPosition.Y / 16f - 1f);
//            int lastTileY = (int)((Main.screenPosition.Y + (float)Main.screenHeight) / 16f) + 2;
//            if (firstTileX < 0)
//            {
//                firstTileX = 0;
//            }
//            if (lastTileX > Main.maxTilesX)
//            {
//                lastTileX = Main.maxTilesX;
//            }
//            if (firstTileY < 0)
//            {
//                firstTileY = 0;
//            }
//            if (lastTileY > Main.maxTilesY)
//            {
//                lastTileY = Main.maxTilesY;
//            }
//            Lighting.LightTiles(firstTileX, lastTileX, firstTileY, lastTileY);
            
//        }

//        static void SpawnNPC()
//        {
//            NPC.SpawnNPC();
//        }
//        static void ClearNCPs()
//        {
//            for(int i = 0; i < npcSlotToCheck; i++)
//            {
//                Main.npc[i].active = false;
//            }
//        }


//        static void CheckNPCs(SpawnAreaInfo spawnArea)
//        {
//            for(int i = 0; i< npcSlotToCheck; i++)
//            {
//                if(Main.npc[i].active)
//                {
//                    spawnArea.AddNPC(Main.npc[i]);
//                }
//            }
//        }


//        static void MoveCamera()
//        {
//            int num2 = 21;
//            if (Main.cameraX != 0f && !Main.player[Main.myPlayer].pulley)
//            {
//                Main.cameraX = 0f;
//            }
//            if (Main.cameraX > 0f)
//            {
//                Main.cameraX -= 1f;
//                if (Main.cameraX < 0f)
//                {
//                    Main.cameraX = 0f;
//                }
//            }
//            if (Main.cameraX < 0f)
//            {
//                Main.cameraX += 1f;
//                if (Main.cameraX > 0f)
//                {
//                    Main.cameraX = 0f;
//                }
//            }
//            Main.screenPosition.X = Main.player[Main.myPlayer].position.X + (float)Main.player[Main.myPlayer].width * 0.5f - (float)Main.screenWidth * 0.5f + Main.cameraX;
//            Main.screenPosition.Y = Main.player[Main.myPlayer].position.Y + (float)Main.player[Main.myPlayer].height - (float)num2 - (float)Main.screenHeight * 0.5f + Main.player[Main.myPlayer].gfxOffY;
//        }
//    }
//}
