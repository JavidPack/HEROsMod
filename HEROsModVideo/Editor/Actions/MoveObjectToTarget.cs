//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.GameikiVideo.Editor.WorldObjects;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.Actions
//{
//    class MoveObjectToTarget : StageAction
//    {
//        public WorldObject Move { get; set; }
//        public WorldObject To { get; set; }
//        public bool Snap { get; set; }
//        public float Duration { get; set; }
//        public bool SmoothStep { get; set; }
//        private float _lerpTimer = 0f;
//        private Vector2 _startingPosition = Vector2.Zero;

//        public MoveObjectToTarget()
//        {
//            Name = "Move Object";
//            this.Move = null;
//            this.To = null;

//            this.SmoothStep = false;
//            this.Duration = 1f;
//            this.Snap = false;
//            this.HaultNextActionUntilCompletion = true;
//        }

//        public override void Execute()
//        {
//            if (Move != null && To != null)
//            {
//                if (Snap)
//                {
//                    Move.Position = To.Position;
//                    this.Completed = true;
//                }
//                else
//                {
//                    if (_lerpTimer == 0f)
//                    {
//                        this._startingPosition = Move.Position;
//                    }
//                    _lerpTimer += ModUtils.DeltaTime / Duration;
//                    if (_lerpTimer >= 1f)
//                    {
//                        _lerpTimer = 1f;
//                        this.Completed = true;
//                    }
//                    if (SmoothStep)
//                    {
//                        Move.Position = Vector2.SmoothStep(_startingPosition, To.Position, _lerpTimer);
//                    }
//                    else
//                    {
//                        Move.Position = Vector2.Lerp(_startingPosition, To.Position, _lerpTimer);
//                    }
//                }
//            }
//            else
//            {
//                Main.NewText("Move Camera Target is null");
//                this.Completed = true;
//            }
//            base.Execute();
//        }

//        public override void PrepareExectution()
//        {
//            this._startingPosition = Move.Position;
//            ModUtils.FreeCamera = true;
//            this._lerpTimer = 0f;
//            base.PrepareExectution();
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            if (Move != null)
//                writer.Write(Move.GUID.ToString());
//            else
//                writer.Write("");

//            if (To != null)
//                writer.Write(To.GUID.ToString());
//            else
//                writer.Write("");
//            writer.Write(Snap);
//            writer.Write(Duration);
//            writer.Write(SmoothStep);
//        }

//        public override void Load(float saveVersion, ref System.IO.BinaryReader reader)
//        {
//            base.Load(saveVersion, ref reader);
//            string guidStr = reader.ReadString();
//            if (guidStr.Length > 0)
//            {
//                Guid guid = Guid.Parse(guidStr);
//                Move = Editor.GetWorldObjectByGuid(guid);
//            }
//            guidStr = reader.ReadString();
//            if (guidStr.Length > 0)
//            {
//                Guid guid = Guid.Parse(guidStr);
//                To = Editor.GetWorldObjectByGuid(guid);
//            }
//            this.Snap = reader.ReadBoolean();
//            this.Duration = reader.ReadSingle();
//            this.SmoothStep = reader.ReadBoolean();
//        }
//    }
//}
