//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using GameikiMod.UIKit;
//using System.IO;
//using System.Reflection;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.WorldObjects
//{
//    class WorldObject //: UIKit.UIView
//    {
//        private float _width = 0;
//        private float _height = 0;

//        [HideInInspector]
//        public float Width
//        {
//            get { return GetWidth(); }
//            set { SetHeight(value); }
//        }
//        [HideInInspector]
//        public float Height
//        {
//            get { return GetHeight(); }
//            set { SetHeight(value); }
//        }
//        [HideInInspector]
//        public float Left
//        {
//            get { return this.Position.X - Origin.X; }
//            set { this.X = value + Origin.X; }
//        }
//        [HideInInspector]
//        public float Right
//        {
//            get { return this.Position.X + this.Width - Origin.X; }
//            set { this.X = value - this.Width + Origin.X; }
//        }
//        [HideInInspector]
//        public float Top
//        {
//            get { return this.Position.Y - Origin.Y; }
//            set { this.Y = value + Origin.Y; }
//        }
//        [HideInInspector]
//        public float Bottom
//        {
//            get { return this.Position.Y + this.Height - Origin.Y; }
//            set { this.Y = value - this.Height + Origin.Y; }
//        }

//        [HideInInspector]
//        public virtual Vector2 Position { get; set; }
//        public float X
//        {
//            get { return Position.X; }
//            set { this.Position = new Vector2(value, this.Position.Y); }
//        }
//        public float Y
//        {
//            get { return Position.Y; }
//            set { this.Position = new Vector2(this.Position.X, value); }
//        }
//        public DrawLayers DrawLayer { get; set; }
//        public AnchorPosition Anchor { get; set; }
//        private bool _screenRelative = false;
//        public bool ScreenRelative
//        {
//            get { return _screenRelative; }
//            set
//            {
//                if(value != _screenRelative)
//                {
//                    _screenRelative = value;
//                    if(value)
//                    {
//                        this.Position -= Main.screenPosition;
//                    }
//                    else
//                    {
//                        this.Position += Main.screenPosition;
//                    }
//                }
//            }
//        }
//        [HideInInspector]
//        public Vector2 Origin
//        {
//            get
//            {
//                return GetOrigin();
//            }
//        }
//        [Range(.1f, 5f)]
//        public float Scale { get; set; }
//        public bool EffectedByWorldLighting { get; set; }
//        [HideInInspector]
//        public Guid GUID { get; set; }
//        [HideInInspector]
//        public bool Visible { get; set; }

//        public bool Glow { get; set; }
//        public Color GlowColor { get; set; }
//        public float GlowSize { get; set; }

//        public WorldObject()
//        {
//            this.Glow = false;
//            this.GlowColor = Color.DarkOrange;
//            this.GlowSize = 1f;
//            this.Visible = true;
//            this.EffectedByWorldLighting = true;
//            this.GUID = Guid.NewGuid();
//            this.Scale = 1f;
//            this.ScreenRelative = false;
//            this.DrawLayer = DrawLayers.FrontOfBlocks;
//        }

//        public virtual void Update()
//        {

//        }

//        public virtual void Draw(SpriteBatch spriteBatch)
//        {
//            if (!this.Visible)
//            {
//                return;
//            }
//            if(Glow)
//            {
//                Vector2 pos = new Vector2(this.Left + this.Width / 2, this.Top + this.Height / 2);
//                Lighting.AddLight((int)pos.X / 16, (int)pos.Y / 16, (float)GlowColor.R / 255f, (float)GlowColor.G / 255f, (float)GlowColor.B / 255f);
//                //Lighting.AddLight
//            }
//        }

//        public virtual float GetWidth()
//        {
//            return _width * Scale;
//        }
//        public virtual float GetHeight()
//        {
//            return _height * Scale;
//        }

//        public virtual void SetWidth(float value)
//        {
//            _width = value;
//        }

//        public virtual void SetHeight(float value)
//        {
//            _height = value;
//        }

//        public bool Intersects(Vector2 p)
//        {
//            return p.X >= Left && p.X <= Right &&
//                    p.Y >= Top && p.Y <= Bottom;
//        }

//        private Vector2 GetOrigin()
//        {
//            float centerX = Width / 2;
//            float centerY = Height / 2;
//            if (Anchor == AnchorPosition.TopLeft)
//                return Vector2.Zero;
//            else if (Anchor == AnchorPosition.Left)
//                return new Vector2(0, centerY);
//            else if (Anchor == AnchorPosition.Right)
//                return new Vector2(Width, centerY);
//            else if (Anchor == AnchorPosition.Top)
//                return new Vector2(centerX, 0);
//            else if (Anchor == AnchorPosition.Bottom)
//                return new Vector2(centerX, Height);
//            else if (Anchor == AnchorPosition.Center)
//                return new Vector2(centerX, centerY);
//            else if (Anchor == AnchorPosition.TopRight)
//                return new Vector2(Width, 0);
//            else if (Anchor == AnchorPosition.BottomLeft)
//                return new Vector2(0, Height);
//            else if (Anchor == AnchorPosition.BottomRight)
//                return new Vector2(Width, Height);
//            return Vector2.Zero;
//        }

//        public virtual void Save(ref BinaryWriter writer)
//        {
//            //Save the object Type
//            Type type = this.GetType();
//            writer.Write(type.FullName);

//            writer.Write(this.X);
//            writer.Write(this.Y);
//            writer.Write(this._width);
//            writer.Write(this._height);
//            writer.Write(this.Scale);
//            writer.Write(this.ScreenRelative);
//            writer.Write((int)Anchor);
//            writer.Write((int)DrawLayer);
//            writer.Write(this.EffectedByWorldLighting);
//            writer.Write(this.GUID.ToString());
//            writer.Write(this.Visible);
//        }

//        public virtual void Load(float saveVersion, ref BinaryReader reader)
//        {
//            this.X = reader.ReadSingle();
//            this.Y = reader.ReadSingle();
//            this._width = reader.ReadSingle();
//            this._height = reader.ReadSingle();
//            this.Scale = reader.ReadSingle();
//            this._screenRelative = reader.ReadBoolean();
//            this.Anchor = (AnchorPosition)reader.ReadInt32();
//            this.DrawLayer = (DrawLayers)reader.ReadInt32();
//            this.EffectedByWorldLighting = reader.ReadBoolean();
//            this.GUID = Guid.Parse(reader.ReadString());
//            if(saveVersion > 0)
//            {
//                this.Visible = reader.ReadBoolean();
//            }
//        }

//        public WorldObject Clone()
//        {
//            MemoryStream stream = new MemoryStream();
//            BinaryWriter writer = new BinaryWriter(stream);
//            this.Save(ref writer);
//            stream.Position = 0;
//            BinaryReader reader = new BinaryReader(stream);
//            Type type = Type.GetType(reader.ReadString());
//            WorldObject wo = (WorldObject)Activator.CreateInstance(type);
//            wo.Load(Editor.saveVersion, ref reader);
//            writer.Close();
//            reader.Close();
//            stream.Close();
//            writer.Dispose();
//            reader.Dispose();
//            stream.Dispose();
//            wo.GUID = Guid.NewGuid();
//            return wo;
//        }
//    }

//    public enum DrawLayers
//    {
//        BehindWalls,
//        BehindBlocks,
//        FrontOfBlocks
//    }
//}
