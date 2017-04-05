//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Terraria;

//namespace GameikiMod.GameikiNetwork.CTF
//{
//    public class SpawnPlatform : WorldObject
//    {
//        public SpawnPlatform(TeamColor team)
//        {
//            TeamColor = team;
//            this._width = 110;
//            this._height = 20;

//            if (Network.NetworkMode != NetworkMode.Server)
//            {
//                switch (team)
//                {
//                    case TeamColor.Red:
//                        this._texture = ModUtils.GetEmbeddedTexture("Images/CTF/redSpawn");
//                        break;
//                    case CTF.TeamColor.Blue:
//                        this._texture = ModUtils.GetEmbeddedTexture("Images/CTF/blueSpawn");
//                        break;
//                }
//            }
//        }
//    }
//}
