//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.GameikiVideo.Editor.WorldObjects;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.Actions
//{
//    class MoveCameraToTarget : StageAction
//    {
//        public WorldObject Target { get; set; }
//        public bool Snap { get; set; }
//        [Range(1, 20)]
//        public float Duration { get; set; }
//        public bool SmoothStep { get; set; }
//        private float _lerpTimer = 0f;

//        private Vector2 cameraPosition
//        {
//            get
//            {
//                Vector2 result = Main.screenPosition;
//                result.X += Main.screenWidth / 2;
//                result.Y += Main.screenHeight / 2;
//                return result;
//            }
//            set
//            {
//                value.X -= Main.screenWidth / 2;
//                value.Y -= Main.screenHeight / 2;
//                Editor.PendingScreenPosition = value;
//            }
//        }

//        private Vector2 _startingPosition = Vector2.Zero;
//        public MoveCameraToTarget()
//        {
//            Name = "Move Camera";
//            this.Target = null;

//            this.SmoothStep = false;
//            this.Duration = 1f;
//            this.Snap = false;
//            this.HaultNextActionUntilCompletion = true;
//        }

//        public override void Execute()
//        {
//            if (Target != null)
//            {
//                if (Snap)
//                {
//                    Editor.MoveCameraTo(Target.Position);
//                    this.Completed = true;
//                }
//                else
//                {
//                    if(_lerpTimer == 0f)
//                    {
//                        this._startingPosition = cameraPosition;
//                    }
//                    _lerpTimer += ModUtils.DeltaTime / Duration;
//                    if(_lerpTimer >= 1f)
//                    {
//                        _lerpTimer = 1f;
//                        this.Completed = true;
//                    }
//                    if(SmoothStep)
//                    {
//                        cameraPosition = Vector2.SmoothStep(_startingPosition, Target.Position, _lerpTimer);
//                    }
//                    else
//                    {
//                        cameraPosition = Vector2.Lerp(_startingPosition, Target.Position, _lerpTimer);
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
//            this._startingPosition = cameraPosition;
//            ModUtils.FreeCamera = true;
//            this._lerpTimer = 0f;
//            base.PrepareExectution();
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            if (Target != null)
//                writer.Write(Target.GUID.ToString());
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
//            if(guidStr.Length > 0)
//            {
//                Guid guid = Guid.Parse(guidStr);
//                Target = Editor.GetWorldObjectByGuid(guid);
//            }
//            this.Snap = reader.ReadBoolean();
//            this.Duration = reader.ReadSingle();
//            this.SmoothStep = reader.ReadBoolean();
//        }
//    }
//}
