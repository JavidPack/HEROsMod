//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.WorldObjects
//{
//    class Dummy : WorldObject
//    {
        
//        public override Vector2 Position
//        {
//            get
//            {
//                return npc.position;
//            }
//            set
//            {
//                npc.position = value;
//            }
//        }

//        int npcIndex = -1;
//        NPC npc
//        {
//            get
//            {
//                if(npcIndex >= 0)
//                {
//                    return Main.npc[npcIndex];
//                }
//                return null;
//            }
//        }

//        private Vector2 _previousPosition = Vector2.Zero;
//        private float _damageTimer = 0f;
//        private int _damageDealt = 0;
//        private List<HitText> _hitText;
//        public Dummy()
//        {
//            ModUtils.LoadTiles(128);
//            _hitText = new List<HitText>();
//            InitNPC();
//        }

//        private void InitNPC()
//        {
//            this.Anchor = UIKit.AnchorPosition.TopLeft;
//            npcIndex = NPC.NewNPC(100, 100, 3);
//            npc.aiStyle = 0;
//            npc.lifeMax = int.MaxValue;
//            npc.life = int.MaxValue;
//            npc.damage = 0;
//            npc.defense = 0;
//            npc.knockBackResist = 0f;
//            npc.color = Color.Transparent;
//            npc.alpha = 255;
//            npc.width = 32;
//            npc.height = 48;
//            npc.name = "Dummy";
//            npc.displayName = "Dummy";
//            npc.townNPC = true;
//        }

//        public override float GetWidth()
//        {
//            return npc.width;
//        }
//        public override float GetHeight()
//        {
//            return npc.height;
//        }

//        public override void Update()
//        {
//            if(npc.active)
//            {
//                _previousPosition = Position;
//            }
//            else
//            {
//                InitNPC();
//                Position = _previousPosition;
//            }
//            _damageTimer += ModUtils.DeltaTime;
//            if(_damageTimer >= 1f)
//            {
//                _damageTimer = 0;
//                if(_damageDealt > 0)
//                {
//                    HitText hitText = new HitText("DPS: " + _damageDealt, new Vector2(this.Position.X + this.Width / 2, this.Position.Y));
//                    this._hitText.Add(hitText);
//                }
//                _damageDealt = 0;
//            }
//            _damageDealt += npc.lifeMax - npc.life;
//            npc.life = int.MaxValue;
//            for (int i = 0; i < _hitText.Count;  i++)
//            {
//                _hitText[i].Update();
//                if(_hitText[i].Timer > 1f)
//                {
//                    _hitText.RemoveAt(i);
//                    i--;
//                }
//            }
//            base.Update();
//        }

//        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
//        {
//            Vector2 drawPosition = new Vector2(this.Left, this.Top);
//            if (!this.ScreenRelative)
//            {
//                drawPosition -= Main.screenPosition;
//            }
//            Color drawColor = Color.White;
//            if(EffectedByWorldLighting)
//            {
//                drawColor = Lighting.GetColor((int)this.X / 16, (int)this.Y / 16);
//            }
//            Texture2D mannequinTexture = Main.tileTexture[128];
//            for(int y = 0; y < 3; y++)
//            {
//                for(int x = 0; x < 2; x++)
//                {
//                    Vector2 pos = drawPosition;
//                    pos.X += 16 * x;
//                    pos.Y += 16 * y;
//                    spriteBatch.Draw(mannequinTexture, pos, new Rectangle(18 * x, 18 * y, 16, 16), drawColor);
//                }
//            }
//            foreach(HitText hitText in _hitText)
//            {
//                hitText.Draw(spriteBatch);
//            }
//        }
//    }

//    class HitText
//    {
//        public string Text { get; set; }
//        public Vector2 Position { get; set; }
//        public Color Color { get; set; }
//        public SpriteFont Font { get; set; }
//        public float Timer { get; set; }

//        public HitText(string text, Vector2 position)
//        {
//            this.Text = text;
//            this.Position = position;
//            this.Color = Color.Yellow;
//            this.Font = Main.fontCombatText[0];
//            this.Timer = 0f;
//        }

//        public void Update()
//        {
//            Position = new Vector2(Position.X, Position.Y - 1 * ModUtils.DeltaTime * 60);
//            Timer += ModUtils.DeltaTime;
//        }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            Vector2 origin = Font.MeasureString(Text) / 2;
//            spriteBatch.DrawString(Font, Text, Position - Main.screenPosition, Color, 0f, origin, 1f, SpriteEffects.None, 0f);
//        }
//    }
//}
