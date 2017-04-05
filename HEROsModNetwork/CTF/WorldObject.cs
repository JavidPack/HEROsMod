//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiNetwork.CTF
//{
//    public class WorldObject
//    {
//        protected Texture2D _texture;
//        public delegate void WorldObjectMovedEvent(WorldObject worldObject);
//        public event WorldObjectMovedEvent Moved;
        
//        public Texture2D Texture
//        {
//            get{return _texture;}
//        }

//        public TeamColor TeamColor { get; set; }

//        private Vector2 previousPosition = Vector2.Zero;
//        private Vector2 position = Vector2.Zero;
//        private Vector2 velocity = Vector2.Zero;

//        protected int _width = 0;
//        protected int _height = 0;

//        public float Width
//        {
//            get 
//            {
//                return _width;
//            } 
//        }
//        public float Height
//        {
//            get
//            {
//                return _height;
//            }
//        }
        

//        public Vector2 Center { get { return new Vector2(position.X + Width / 2, position.Y + Height / 2); } }
//        public Vector2 Position { get { return position; } set { position = value; } }
//        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
//        public bool Placed { get; set; }

//        public WorldObject()
//        {
//            Placed = false;
//        }

//        public void SetLocation(Vector2 location)
//        {
//            Placed = true;

//            location.X = (int)location.X / 16 * 16;
//            location.Y = (int)location.Y / 16 * 16;
//            location.X -= Width / 2;
//            location.Y -= Height / 2;
//            this.position = location;
//        }

//        public virtual void Update()
//        {
//            if (Placed)
//            {

//                Vector2 prevVelocity = velocity;
//                float fallSpeed = .5f;
//                float maxFallSpeed = 10f;
//                this.velocity.Y = this.velocity.Y + fallSpeed;
//                if (this.velocity.Y > maxFallSpeed)
//                {
//                    this.velocity.Y = maxFallSpeed;
//                }
//                this.velocity.X = this.velocity.X * 0.95f;
//                if ((double)this.velocity.X < 0.1 && (double)this.velocity.X > -0.1)
//                {
//                    this.velocity.X = 0f;
//                }
//                this.velocity = Collision.TileCollision(this.position, this.velocity, (int)Width, (int)Height, false, false, 1);
//                Vector4 vector2 = Collision.SlopeCollision(this.position, this.velocity, (int)Width, (int)Height, fallSpeed, false);
//                this.position.X = vector2.X;
//                this.position.Y = vector2.Y;
//                this.velocity.X = vector2.Z;
//                this.velocity.Y = vector2.W;
//                this.position += this.velocity;
                
//                if(this.position != this.previousPosition)
//                {
//                    if(previousPosition.X - position.X >= 2f || previousPosition.X -  position.X <= -2 ||
//                        previousPosition.Y - position.Y >= 2f || previousPosition.Y - position.Y <= -2)
//                    {
//                        previousPosition = position;

//                        if(Moved != null)
//                        {
//                            Moved(this);
//                        }
//                    }
//                }
//            }
//        }

//        public virtual void Draw(SpriteBatch spriteBatch)
//        {
//            if (Main.netMode != 2)
//            {
//                if (Placed)
//                    spriteBatch.Draw(Texture, position - Main.screenPosition, Color.White);
//            }
//        }
//    }
//}
