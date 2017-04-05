//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Terraria;

//namespace GameikiMod.GameikiNetwork.CTF
//{
//    public class FlagPlatform : WorldObject
//    {
//        public FlagPlatform(TeamColor team)
//        {
//            TeamColor = team;
//            this._width = 38;
//            this._height = 30;

//            if (Network.NetworkMode != NetworkMode.Server)
//            {
//                switch (team)
//                {
//                    case TeamColor.Red:
//                        this._texture = ModUtils.GetEmbeddedTexture("Images/CTF/redPlatform");
//                        break;
//                    case CTF.TeamColor.Blue:
//                        this._texture = ModUtils.GetEmbeddedTexture("Images/CTF/bluePlatform");
//                        break;
//                }
//            }
//        }

//    }
//}
