//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.GameikiServices;
//using Microsoft.Xna.Framework.Input;
//using System.IO;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    public delegate void TimerEvent(EventTimer timer);
//    class SpeedRunTimer : GameikiService
//    {
//        public static float Timer { get; set; }
//        public static List<EventTimer> EventTimers { get; set; }
//        public static int CurrentEvent { get; set; }

//        public static bool TrackingTime { get; set; }

//        private SettingsWindow _settingsWindow;
//        private static TimerDisplay _timerDisplay;

//        private static GameikiMod.GameikiServices.KeyBinding kStartStop;
//        private static GameikiMod.GameikiServices.KeyBinding kReset;
//        private static GameikiMod.GameikiServices.KeyBinding kToggleDisplay;

//        public SpeedRunTimer()
//        {
//            EventTimers = new List<EventTimer>();
//            CurrentEvent = 0;
//            _settingsWindow = new SettingsWindow();
//            _settingsWindow.Visible = false;
//            _settingsWindow.TimerAdded += _settingsWindow_TimerAdded;
//            this.AddUIView(_settingsWindow);
//            //UIKit.MasterView.gameScreen.AddChild(_settingsWindow);

//            _timerDisplay = new TimerDisplay();
//            this.AddUIView(_timerDisplay);

//            this._hotbarIcon = new UIKit.UIImage(Main.itemTexture[889]);
//			this._hotbarIcon.Tooltip = "Open Speed Run Tool";
//			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;

//            this.HasPermissionToUse = true;

//            TrackingTime = false;
//        }

//        public static void SetKeyBindings()
//        {
//           // KeybindController.SetCatetory("Speed Run Timer");
//            kStartStop = KeybindController.AddKeyBinding("Start / Stop", "F1");
//            kReset = KeybindController.AddKeyBinding("Reset", "F2");
//            kToggleDisplay = KeybindController.AddKeyBinding("Toggle Timer Display", "F3");
//        }

//        void _settingsWindow_TimerAdded(EventTimer timer)
//        {
//            timer.StepChanged += timer_StepChanged;
//            EventTimers.Add(timer);
//            _timerDisplay.PopulateTimerList();
//        }

//        void timer_StepChanged(EventTimer timer)
//        {
//            _timerDisplay.PopulateTimerList();
//            CheckForCompletion();
//        }

//        void _hotbarIcon_onLeftClick(object sender, EventArgs e)
//        {
//            _settingsWindow.Visible = !_settingsWindow.Visible;
//        }

//        public static void Start()
//        {
//            if (EventTimers.Count > 0)
//            {
//                if(IsCompleted())
//                {
//                    Reset();
//                }
//                TrackingTime = true;
//                foreach(var timer in EventTimers)
//                {
//                    timer.Start();
//                }
//            }
//        }

//        public static void Stop()
//        {
//            TrackingTime = false;
//        }

//        public static void Reset()
//        {
//            Timer = 0f;
//            CurrentEvent = 0;
//            TrackingTime = false;
//            foreach(EventTimer timer in EventTimers)
//            {
//                timer.Reset();
//            }
//            _timerDisplay.PopulateTimerList();
//        }

//        public override void Update()
//        {
//            HandleInput();
//            if(TrackingTime)
//            {
//                foreach (EventTimer timer in EventTimers)
//                {
//                    timer.Update();
//                }
//                Timer += ModUtils.DeltaTime;
//                _timerDisplay.Tick(ModUtils.DeltaTime);
//            }
//        }

//        private void ProgressTimerStep()
//        {
//            if (TrackingTime)
//            {
//                EventTimer currentTimer = EventTimers[CurrentEvent];
//                currentTimer.CurrentStep++;
//                if (currentTimer.CurrentStep >= currentTimer.Steps)
//                {
//                    CurrentEvent++;
//                    if (CurrentEvent >= EventTimers.Count)
//                    {
//                        Stop();
//                    }
//                }
//                _timerDisplay.PopulateTimerList();
//            }
//        }



//        private void HandleInput()
//        {
//            if(kStartStop.KeyPressed)
//            {
//                if(TrackingTime)
//                {
//                    Stop();
//                }
//                else
//                {
//                    Start();
//                }
//            }
//            if(kReset.KeyPressed)
//            {
//                Reset();
//            }
//            if (kToggleDisplay.KeyPressed)
//            {
//                _timerDisplay.Visible = !_timerDisplay.Visible;
//            }
//        }

//        public static string FormatTime(float time)
//        {
//            int m = (int)time / 60;
//            int s = (int)time % 60;
//            string strS = s.ToString();
//            if (s < 10)
//            {
//                strS = "0" + strS;
//            }
//            int remainder = (int)((time - (int)time) * 100);

//            return m + ":" + strS + "." + remainder;
//        }

//        public static string oldGetPercentage()
//        {
//            if(CurrentEvent >= EventTimers.Count)
//            {
//                return "%100";
//            }
//            EventTimer currentTimer = EventTimers[CurrentEvent];
//            float perc = (float)CurrentEvent / EventTimers.Count;
//            perc += (float)currentTimer.CurrentStep / currentTimer.Steps * (1f / EventTimers.Count);

//            int percentage = (int)(perc * 100);
//            return "%" + percentage;
//        }

//        public static string GetPercentage()
//        {
//            float result = 0f;
//            for(int i = 0; i < EventTimers.Count; i++)
//            {
//                EventTimer currentTimer = EventTimers[i];
//                result += (float)currentTimer.CompletionPercentage * (1f / EventTimers.Count);
//            }
//            int percentage = (int)(result * 100);
//            return "%" + percentage;
//        }

//        public static void CheckForCompletion()
//        {
//            if(IsCompleted())
//            {
//                Stop();
//            }
//        }

//        public static bool IsCompleted()
//        {
//            for (int i = 0; i < EventTimers.Count; i++)
//            {
//                if (!EventTimers[i].Completed)
//                {
//                    return false;
//                }
//            }
//            return true;
//        }

//        public static void ClearTimers()
//        {
//            EventTimers.Clear();
//            _timerDisplay.PopulateTimerList();
//        }

//        public static string[] GetSavedSettingsFiles()
//        {
//            List<string> results = new List<string>();
//            string settingsDir = Main.SavePath + @"\SpeedRunSaves";
//            if (!Directory.Exists(settingsDir)) return results.ToArray();
//            string[] files = Directory.GetFiles(settingsDir);
//            for (int i = 0; i < files.Length; i++)
//            {
//                 results.Add(Path.GetFileNameWithoutExtension(files[i]));
//            }
//            return results.ToArray();
//        }

//        public static void NPCDied(int netID)
//        {
//            if (TrackingTime)
//            {
//                for (int i = 0; i < EventTimers.Count; i++)
//                {
//                    if (EventTimers[i] is KillBossTimer)
//                    {
//                        KillBossTimer timer = (KillBossTimer)EventTimers[i];
//                        if(timer.NPCNetID == netID)
//                        {
//                            timer.AddStep();
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
