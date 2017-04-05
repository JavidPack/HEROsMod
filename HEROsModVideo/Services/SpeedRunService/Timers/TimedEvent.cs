//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Graphics;
//using System.IO;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    public class EventTimer
//    {
//        public string Name { get; set; }

//        protected int _timerSaveID = 0;

//        public event TimerEvent StepChanged;
//        public int TargetItemNetID { get; set; }

//        public float Timer { get; set; }
//        public Texture2D Texture { get; set; }

//        public int Steps { get; set; }

//        private int _currentStep;
//        public int CurrentStep
//        {
//            get { return _currentStep; }
//            set
//            {
//                if(_currentStep != value)
//                {
//                    if(_currentStep < Steps && value >= Steps)
//                    {
//                        this.Timer = SpeedRunTimer.Timer;
//                    }
//                    _currentStep = value;
//                    if(_currentStep > Steps)
//                    {
//                        _currentStep = Steps;
//                    }
//                    if(_currentStep < 0)
//                    {
//                        _currentStep = 0;
//                    }
//                    if(StepChanged != null)
//                    {
//                        StepChanged(this);
//                    }
//                }
//            }
//        }

//        public float CompletionPercentage
//        {
//            get { return GetCompletionPercentage(); }
//        }

//        public bool Completed
//        {
//            get
//            {
//                return CurrentStep >= Steps;
//            }
//        }

//        public EventTimer(string name, int itemType, int steps)
//        {
//            this.Name = name;
//            this.Timer = 0f;
//            Item item = new Item();
//            item.netDefaults(itemType);
//            this.Texture = Main.itemTexture[item.type];
//            this.Steps = steps;
//            CurrentStep = 0;
//            this.TargetItemNetID = itemType;
//        }

//        public void AddStep()
//        {
//            this.CurrentStep++;
//        }

//        public void RemoveStep()
//        {
//            this.CurrentStep--;
//            if(CurrentStep < 0)
//            {
//                CurrentStep = 0;
//            }
//        }

//        public virtual void Reset()
//        {
//            Timer = 0f;
//            CurrentStep = 0;
//        }

//        public virtual void Update()
//        {

//        }

//        public virtual void Start()
//        {

//        }

//        public virtual void OnStepChanges(EventTimer timer)
//        {
//            if(StepChanged != null)
//            {
//                StepChanged(timer);
//            }
//        }

//        protected virtual float GetCompletionPercentage()
//        {
//            return (float)CurrentStep / Steps;
//        }

//        public virtual void Save(ref BinaryWriter writer)
//        {
//            writer.Write(_timerSaveID);
//            writer.Write(Name);
//            writer.Write(Steps);
//            writer.Write(TargetItemNetID);
//        }

//        public virtual void Load(ref BinaryReader reader)
//        {
//            this.Name = reader.ReadString();
//            this.Steps = reader.ReadInt32();
//            this.TargetItemNetID = reader.ReadInt32();

//            Item item = new Item();
//            item.netDefaults(TargetItemNetID);
//            this.Texture = Main.itemTexture[item.type];
//        }
        
//    }
//}
