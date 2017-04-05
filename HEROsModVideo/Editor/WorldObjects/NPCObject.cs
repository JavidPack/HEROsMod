//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.WorldObjects
//{
//    class NPCObject : WorldObject
//    {
//        private int _npcType = 1;
//        private int _aiStyle = 0;
//        private bool _enableAI = false;
//        private WorldObject _target;
//        private WorldObject Target
//        {
//            get { return _target; }
//            set
//            {
//                if(value is Dummy)
//                {
//                    _target = value;
                    
//                }
//                else
//                {
//                    _target = null;
//                    Main.NewText("this is not a dummy");
//                }
//            }
//        }
//        public int NPCType 
//        {
//            get { return _npcType; }
//            set
//            {
//                _npcType = value;
//                InitNPC();
//            }
//        }


//        public bool EnableAI
//        {
//            get { return _enableAI; }
//            set
//            {
//                _enableAI = value;
//                if(value)
//                {
//                    npc.aiStyle = _aiStyle;
//                }
//                else
//                {
//                    npc.aiStyle = 0;
//                }
//            }
//        }

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
//                if (npcIndex >= 0)
//                {
//                    return Main.npc[npcIndex];
//                }
//                return null;
//            }
//        }
//        public NPCObject()
//        {
//            InitNPC();
//        }
//        public override float GetWidth()
//        {
//            return npc.width;
//        }
//        public override float GetHeight()
//        {
//            return npc.height;
//        }

//        private void InitNPC()
//        {
//            if(Lang.npcName(_npcType).Length > 0)
//            {
//                if(npc != null && npc.active)
//                {
//                    npc.SetDefaults(_npcType);
//                }
//                else
//                {
//                    Vector2 prevPosition = Vector2.Zero;
//                    if (npc != null)
//                        prevPosition = npc.position;
//                    this.Anchor = UIKit.AnchorPosition.TopLeft;
//                    npcIndex = NPC.NewNPC((int)prevPosition.X, (int)prevPosition.Y, _npcType);
//                }
//                _aiStyle = npc.aiStyle;
//                if(!EnableAI)
//                {
//                    npc.aiStyle = 0;
//                }
//            }
//            else
//            {
//                NPCType = 1;
//            }
//        }

//        public override void Update()
//        {
//            if(!npc.active)
//            {
//                InitNPC();
//            }
//            base.Update();
//        }
//    }
//}
