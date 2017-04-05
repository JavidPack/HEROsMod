//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    class CollectItemsTimer : EventTimer
//    {
//        public bool SubItem { get; set; }
//        public bool Collapsed { get; set; }
//        public List<CollectItemsTimer> SubTimers { get; set; }
//        public CollectItemsTimer(string name, int itemType, int steps, bool subItem = false) : base(name, itemType, steps)
//        {
//            this.SubTimers = new List<CollectItemsTimer>();
//            this.TargetItemNetID = itemType;
//            this._timerSaveID = 1;
//            this.SubItem = subItem;
//            Collapsed = false;
//        }
//        public override void Update()
//        {
//            if(!Completed)
//            {
//                ScanInventory();
//                foreach(CollectItemsTimer timer in SubTimers)
//                {
//                    timer.Update();
//                }
//            }
//            base.Update();
//        }
//        public void ScanInventory()
//        {
//            CurrentStep = 0;
//            Player player = Main.player[Main.myPlayer];
//            for(int i = 0; i < player.inventory.Length - 1; i++)
//            {
//                Item item = player.inventory[i];
//                if(item.netID == TargetItemNetID)
//                {
//                    CurrentStep += item.stack;
//                }
//            }
//        }

//        public override void Reset()
//        {
//            foreach(var subTimer in SubTimers)
//            {
//                subTimer.Reset();
//            }
//            base.Reset();
//        }

//        public void AddSubTimer(CollectItemsTimer timer)
//        {
//            timer.StepChanged += timer_StepChanged;
//            SubTimers.Add(timer);
//        }

//        void timer_StepChanged(EventTimer timer)
//        {
//            base.OnStepChanges(timer);
//        }

//        protected override float GetCompletionPercentage()
//        {
//            float result = 0;
//            int numberOfTimers = SubTimers.Count + 1;

//            result += base.GetCompletionPercentage() / numberOfTimers;
//            foreach(CollectItemsTimer timer in SubTimers)
//            {
//                result += timer.GetCompletionPercentage() / numberOfTimers;
//            }
//            return result;
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            writer.Write(SubItem);
//            writer.Write(SubTimers.Count);
//            for(int i = 0; i < SubTimers.Count; i++)
//            {
//                SubTimers[i].Save(ref writer);
//            }
//        }

//        public override void Load(ref System.IO.BinaryReader reader)
//        {
//            base.Load(ref reader);
//            this.SubItem = reader.ReadBoolean();
//            int numberOfSubTimers = reader.ReadInt32();
//            for(int i = 0;i < numberOfSubTimers; i++)
//            {
//                int timerType = reader.ReadInt32();
//                CollectItemsTimer timer = new CollectItemsTimer("", 0, 0, true);
//                timer.Load(ref reader);
//                AddSubTimer(timer);
//            }
//        }
//    }
//}
