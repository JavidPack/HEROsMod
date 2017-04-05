//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    class TargetHealthTimer : EventTimer
//    {
//        private int _previousHealth = 0;
//        private int _heartsNeeded = 0;
//        private int _fruitNedded = 0;

//        public int TargetHealth { get; set; }
//        private int _startingHealth;
//        int MaxHealth
//        {
//            get
//            {
//                return Main.player[Main.myPlayer].statLifeMax;
//            }
//        }
//        public TargetHealthTimer(string name, int itemType, int targetHealth) : base(name, itemType, 1)
//        {
//            this._timerSaveID = 3;
//            this.TargetHealth = targetHealth;

//            this.Steps = GetNumberOfSteps(TargetHealth, MaxHealth);
//            _previousHealth = MaxHealth;
//        }

//        public int GetNumberOfSteps(int targetHealth, int maxHealth)
//        {
//            int heartSteps = 0;
//            int fruitSteps = 0;
//            int healthUntilTargetReached = targetHealth - maxHealth;

//            int heartTarget = targetHealth;
//            if(heartTarget > 400)
//            {
//                heartTarget = 400;
//                int fruitTarget = targetHealth;
//                if(maxHealth < fruitTarget)
//                {
//                    int fruitHealth = MaxHealth - 400;
//                    if (fruitHealth < 0)
//                        fruitHealth = 0;
//                    fruitSteps = ((fruitTarget - 400) - fruitHealth) / 5;
//                }
//            }
//            if(maxHealth < heartTarget)
//            {
//                heartSteps = (heartTarget - maxHealth) / 20;
//            }
//            _heartsNeeded = heartSteps;
//            _fruitNedded = fruitSteps;
//            return heartSteps + fruitSteps;
//        }

//        private int GetCurrentStep()
//        {
//            int result = 0;
//            int healthGained = MaxHealth - _startingHealth;

//            int currentHearts = MaxHealth;
//            if(currentHearts > 400)
//            {
//                currentHearts = 400;
//            }
//            currentHearts /= 20;
//            int startingHearts = _startingHealth;
//            if(startingHearts > 400)
//            {
//                startingHearts = 400;
//            }
//            startingHearts /= 20;
//            result += currentHearts - startingHearts;

//            int currentFruit = 0;
//            if (MaxHealth > 400)
//            {
//                currentFruit = MaxHealth;
//                currentFruit -= 400;
//                currentFruit /= 5;
//            }
//            int startingFruit = 0;
//            if(_startingHealth > 400)
//            {
//                startingFruit = _startingHealth;
//                startingFruit -= 400;
//                startingFruit /= 5;
//            }
//            result += currentFruit - startingFruit;

//            return result;
//        }

//        public override void Start()
//        {
//            this._startingHealth = MaxHealth;
//            Steps = GetNumberOfSteps(TargetHealth, MaxHealth);
//            base.Start();
//        }

//        public override void Update()
//        {
//            int maxLife = Main.player[Main.myPlayer].statLifeMax;
//            if(maxLife != _previousHealth)
//            {
//                CurrentStep = GetCurrentStep();
//            }
//            _previousHealth = Main.player[Main.myPlayer].statLifeMax;
//            base.Update();
//        }

//        public override void Save(ref System.IO.BinaryWriter writer)
//        {
//            base.Save(ref writer);
//            writer.Write(TargetHealth);
//        }

//        public override void Load(ref System.IO.BinaryReader reader)
//        {
//            base.Load(ref reader);
//            this.TargetHealth = reader.ReadInt32();
//        }
//    }
//}
