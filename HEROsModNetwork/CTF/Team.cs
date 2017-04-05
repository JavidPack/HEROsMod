//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiNetwork.CTF
//{
//    class Team
//    {
//        TeamColor TeamColor { get; set; }
//        public FlagPlatform FlagPlatform { get; set; }
//        public Flag Flag { get; set; }
//        public SpawnPlatform SpawnPlatform { get; set; }
//        public int Score { get; set; }

//        public event EventHandler Scored;

//        public Team(TeamColor teamColor)
//        {
//            Score = 0;
//            TeamColor = teamColor;
//            FlagPlatform = new FlagPlatform(TeamColor);
//            Flag = new Flag(TeamColor);
//            SpawnPlatform = new SpawnPlatform(TeamColor);

//            FlagPlatform.Moved += TeamObject_Moved;
//            Flag.Moved += TeamObject_Moved;
//            SpawnPlatform.Moved += TeamObject_Moved;
//        }

//        void TeamObject_Moved(WorldObject worldObject)
//        {
//            CTFMessages.SendTeamOjectPosition(worldObject);
//            if (worldObject is FlagPlatform)
//            {
//                if (Flag.AtPlatform)
//                {
//                    Vector2 pos = FlagPlatform.Position;
//                    pos.Y -= Flag.Height;
//                    pos.X += FlagPlatform.Width / 2;
//                    Flag.Position = pos;
//                    Flag.Velocity = Vector2.Zero;
//                    CTFMessages.SendTeamOjectPosition(Flag);
//                }
//            }
//        }

//        public void Update()
//        {
//            FlagPlatform.Update();
//            Flag.Update();
//            SpawnPlatform.Update();

//            if(Network.NetworkMode == NetworkMode.Server)
//            {
//                if(Flag.RespawnTimer <= 0)
//                {
//                    SendFlagToBase();
//                }
//                if(Flag.Carrier != null)
//                {
//                    Team opposingTeam = null;
//                    if(TeamColor == TeamColor.Red)
//                    {
//                        opposingTeam = CaptureTheFlag.BlueTeam;
//                    }
//                    else if(TeamColor == TeamColor.Blue)
//                    {
//                        opposingTeam = CaptureTheFlag.RedTeam;
//                    }
//                    float distaceToPlatform = Vector2.Distance(Flag.Carrier.GameInstance.Center, opposingTeam.FlagPlatform.Center);
//                    if(distaceToPlatform < Flag.PickupDistance)
//                    {
//                        SendFlagToBase();
//                        opposingTeam.Score++;
//                        if(Scored != null)
//                        {
//                            Scored(opposingTeam, EventArgs.Empty);
//                        }
//                    }
//                }
//            }
//        }

//        public void PlaceFlagPlatform(Vector2 position)
//        {
//            FlagPlatform.SetLocation(position);
//            SendFlagToBase();
//            CTFMessages.SendTeamOjectPosition(FlagPlatform);
//        }

//        public void PlaceSpawnPlatform(Vector2 position)
//        {
//            SpawnPlatform.SetLocation(position);
//            CTFMessages.SendTeamOjectPosition(SpawnPlatform);
//        }

//        public void SendFlagToBase()
//        {
//            Flag.Placed = true;
//            Flag.AtPlatform = true;
//            Flag.Carrier = null;

//            Flag.RespawnTimer = CaptureTheFlag.FlagRespawnTime;
//            Vector2 pos = FlagPlatform.Position;
//            pos.Y -= Flag.Height;
//            pos.X += FlagPlatform.Width / 2;
//            Flag.Position = pos;
//            Flag.Velocity = Vector2.Zero;

//            if(Network.NetworkMode == NetworkMode.Server)
//            {
//                CTFMessages.SendFlagToBase(TeamColor);
//            }
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            SpawnPlatform.Draw(spriteBatch);
//            FlagPlatform.Draw(spriteBatch);
//            Flag.Draw(spriteBatch);
//        }
//    }
//}
