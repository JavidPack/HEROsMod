//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiNetwork.CTF
//{
//    public class Flag : WorldObject
//    {
//        public static int PickupDistance = 50;
//        private static float ThrowDist = 10f;
//        private GameikiPlayer _carrier;
//        public float RespawnTimer { get; set; }
//        public GameikiPlayer Carrier 
//        { 
//            get
//            {
//                return _carrier;
//            }
//            set
//            {
//                _carrier = value;
//                if(_carrier != null)
//                {
//                    AtPlatform = false;
//                    RespawnTimer = CaptureTheFlag.FlagRespawnTime;
//                }
//                if(Network.NetworkMode == NetworkMode.Server)
//                {
//                    CTFMessages.CarrierChanged(this);
//                }
//            }
//        }

//        public bool AtPlatform { get; set; }
//        public Flag(TeamColor team)
//        {
//            RespawnTimer = 0f;
//            AtPlatform = true;
//            Carrier = null;
//            TeamColor = team;
//            this._width = 38;
//            this._height = 38;

//            if (Network.NetworkMode != NetworkMode.Server)
//            {
//                switch (team)
//                {
//                    case TeamColor.Red:
//                        this._texture = ModUtils.GetEmbeddedTexture("Images/CTF/redFlag");
//                        break;
//                    case CTF.TeamColor.Blue:
//                        this._texture = ModUtils.GetEmbeddedTexture("Images/CTF/blueFlag");
//                        break;
//                }
//            }
//        }

//        public override void Update()
//        {
//            if(Network.NetworkMode != NetworkMode.Server && CaptureTheFlag.GameInProgress)
//            {
//                float distanceToFlag = Vector2.Distance(Main.player[Main.myPlayer].Center, this.Center);
//                GameikiPlayer myPlayer = Network.Players[Main.myPlayer];
//                if(Carrier == null && !myPlayer.GameInstance.dead && distanceToFlag < PickupDistance && myPlayer.CTFTeam != TeamColor.None && myPlayer.CTFTeam != this.TeamColor)
//                {
//					//TODO, fix this
//					/*if (CaptureTheFlag.PickUpFlagBinding.key != Microsoft.Xna.Framework.Input.Keys.None)
//                        CaptureTheFlag.TipLabel.Text = "Press " + CaptureTheFlag.PickUpFlagBinding.key.ToString() + " to Pick Up Flag";
//                    else
//                        CaptureTheFlag.TipLabel.Text = "Pick Up Flag Keybinding Not Set!";*/
//                    CaptureTheFlag.TipLabel.Visible = true;

//                    if(CaptureTheFlag.PickUpFlagBinding.KeyPressed)
//                    {
//                        CTFMessages.RequestPickupFlag();
//                    }
//                }
//                if(Carrier == Network.Players[Main.myPlayer])
//                {
//                    if (CaptureTheFlag.PickUpFlagBinding.KeyPressed)
//                    {
//                        CTFMessages.RequestThrowFlag();
//                    }
//                }
                
//            }
//            if (Carrier == null && !AtPlatform)
//            {
//                RespawnTimer -= ModUtils.DeltaTime;
//            }

//            if (Network.NetworkMode == NetworkMode.Server && CaptureTheFlag.GameInProgress)
//            {
//                if(Carrier != null && Carrier.GameInstance.dead)
//                {
//                    Position = Carrier.GameInstance.position;
//                    Carrier = null;
//                }
//            }

//            //if there is no carrier and the flag is not at the base, do physics
//            if (Carrier == null && !AtPlatform)
//            {
//                base.Update();
//            }
//        }

//        public void Pickup(GameikiPlayer player)
//        {
//            Carrier = player;
//        }

//        public void Throw()
//        {
//            this.Position = Carrier.GameInstance.Center;
//            this.Position = new Vector2(Position.X, Position.Y - Height);
//            if(Carrier.GameInstance.direction > 0)
//            {
//                this.Position = new Vector2(Position.X - Width, Position.Y);
//            }
//            float xVel = Carrier.GameInstance.velocity.X;
//            xVel += ThrowDist * Carrier.GameInstance.direction;
//            float yVel = -5f;
//            this.Velocity = new Vector2(xVel, yVel);
//            Carrier = null;
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            if (Placed)
//            {
//                if (Carrier == null)
//                {
//                    spriteBatch.Draw(Texture, Position - Main.screenPosition, Color.White);

//                    //Draw timer
//                    if (CaptureTheFlag.GameInProgress && !AtPlatform && Carrier == null)
//                    {
//                        Vector2 pos = Position - Main.screenPosition;
//                        Color rectColor = Color.Red;
//                        if (TeamColor == CTF.TeamColor.Blue) rectColor = Color.Blue;
//                        Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y - 10, Texture.Width, 5);
//                        spriteBatch.Draw(ModUtils.DummyTexture, rect, Color.Black);
//                        rect.Width = (int)(Texture.Width * (RespawnTimer / CaptureTheFlag.FlagRespawnTime));
//                        spriteBatch.Draw(ModUtils.DummyTexture, rect, rectColor);
//                    }
//                }
//                else
//                {
//                    Vector2 anchor = new Vector2(15, -20);
//                    SpriteEffects spriteEffect = SpriteEffects.None;
//                    if (Carrier.GameInstance.direction > 0)
//                    {
//                        spriteEffect = SpriteEffects.FlipHorizontally;
//                        anchor.X = -anchor.X;
//                        anchor.X -= Carrier.GameInstance.width;
//                    }
//                    Vector2 pos = Carrier.GameInstance.position + anchor - Main.screenPosition;
//                    spriteBatch.Draw(Texture, pos, null, Color.White, 0f, Vector2.Zero, 1f, spriteEffect, 0);
//                }

//            }
//        }

//    }
//}
