//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.SpeedRunService
//{
//    class TimerDisplay : UIView
//    {
//        List<UILabel> _timerLabels;
//        UIRect _timersView;
//        UIImage _fastClock;
//        float _itemBGScale = .85f;
//        UILabel _time;

//        private float _timerWidth = 0f;
//        List<EventTimer> eventTimers
//        {
//            get { return SpeedRunTimer.EventTimers; }
//        }

//        public TimerDisplay()
//        {

//            _time = new UILabel();
//            _time.Anchor = AnchorPosition.Left;
//            _time.Text = "00:00.00";
//            _time.Scale = .4f;
//            _timerWidth = _time.Width;

//            _timersView = new UIRect();
//            _fastClock = new UIImage(Main.itemTexture[889]);
//            _fastClock.Anchor = AnchorPosition.Top;
//            _fastClock.Y = 0;

//            _timersView.Y = _fastClock.Y + _fastClock.Height + Spacing;
//            _timersView.X = 0;
//            _timersView.Width = _time.Width + Spacing + Main.inventoryBack2Texture.Width * _itemBGScale;
//            _timersView.Height = 100;
//            _timersView.ForegroundColor = Color.Transparent;
//            _timersView.OverridesMouse = false;

//            _fastClock.X = _timersView.Width - Main.inventoryBack2Texture.Width * _itemBGScale / 2;
//            _time.X = _fastClock.X - _fastClock.Width / 2 - Spacing - _timerWidth;
//            _time.Y = _fastClock.Height / 2;

//            this.AddChild(_time);
//            this.AddChild(_fastClock);
//            this.AddChild(_timersView);
//            this.Width = _timersView.Width;
//            this.Height = _timersView.Y + _timersView.Height;
//            this.OverridesMouse = false;
//        }

//        public void PopulateTimerList()
//        {
//            _timerLabels = new List<UILabel>();
//            _timersView.RemoveAllChildren();
//            float yPos = 0f;
//            for (int i = 0; i < eventTimers.Count; i++)
//            {
//                CreateTimerUI(eventTimers[i], ref yPos);
//                if(eventTimers[i] is CollectItemsTimer)
//                {
//                    CollectItemsTimer itemTimer = (CollectItemsTimer)eventTimers[i];
//                    if (!itemTimer.Collapsed)
//                    {
//                        foreach (var subTimer in itemTimer.SubTimers)
//                        {
//                            CreateTimerUI(subTimer, ref yPos);
//                        }
//                    }
//                }

//            }
//            UILabel percentage = new UILabel(SpeedRunTimer.GetPercentage());
//            percentage.Scale = .5f;
//            percentage.Anchor = AnchorPosition.Top;
//            percentage.X = _fastClock.X;
//            percentage.Y = yPos;
//            _timersView.AddChild(percentage);
//            _timersView.Height = percentage.Y + percentage.Height;
//            this.Width = _timersView.Width;
//            this.Height = _timersView.Height;
//        }

//        private void CreateTimerUI(EventTimer timer, ref float yPos)
//        {
//            EventTimer currentTimer = timer;
//            UIImage itemBG = new UIImage(Main.inventoryBack13Texture);
//            itemBG.Scale = _itemBGScale;
//            if (currentTimer is CollectItemsTimer)
//            {
//                CollectItemsTimer collectTimer = (CollectItemsTimer)currentTimer;
//                if (collectTimer.SubItem)
//                {
//                    itemBG.Scale *= .75f;
//                }
//            }
//            itemBG.ForegroundColor = timer.Completed ? Color.Green : Color.Red;
//            itemBG.ForegroundColor *= .6f;
//            itemBG.X = _timersView.Width - itemBG.Width;
//            itemBG.Y = yPos;
//            itemBG.Tooltip = currentTimer.Name;
//            itemBG.Tag = currentTimer;
//            itemBG.onLeftClick += itemBG_onLeftClick;
//            itemBG.onRightClick += itemBG_onRightClick;

//            UIImage itemImage = new UIImage(timer.Texture);
//            itemImage.Anchor = AnchorPosition.Center;
//            float itemScale = 1f;
//            float pixelWidth = itemBG.Width * Scale * .6f;
//            if (itemImage.Width > pixelWidth || itemImage.Height > pixelWidth)
//            {
//                if (itemImage.Width > itemImage.Height)
//                {
//                    itemScale = pixelWidth / (float)itemImage.Width;
//                }
//                else
//                {
//                    itemScale = pixelWidth / (float)itemImage.Height;
//                }
//            }
//            itemImage.Scale = itemScale;
//            itemImage.Y = yPos + itemBG.Height / 2;
//            itemImage.X = itemBG.X + itemBG.Width / 2;
//            itemImage.OverridesMouse = false;

//            if (currentTimer.Completed)
//            {
//                UILabel time = new UILabel(SpeedRunTimer.FormatTime(timer.Timer));
//                time.OverridesMouse = false;
//                time.Anchor = AnchorPosition.Right;
//                time.Scale = .4f;
//                time.X = itemBG.X - Spacing;
//                time.Y = yPos + itemBG.Height / 2;
//                _timerLabels.Add(time);
//                _timersView.AddChild(time);
//            }

//            _timersView.AddChild(itemBG);
//            _timersView.AddChild(itemImage);
//            if (currentTimer.Steps > 1)
//            {
//                UILabel lSteps = new UILabel(currentTimer.CurrentStep + "/" + currentTimer.Steps);
//                lSteps.OverridesMouse = false;
//                lSteps.Scale = .35f;
//                lSteps.Scale *= itemBG.Scale / _itemBGScale;
//                lSteps.Anchor = AnchorPosition.Bottom;
//                lSteps.Y = yPos + itemBG.Height;
//                lSteps.X = itemBG.X + itemBG.Width / 2;
//                _timersView.AddChild(lSteps);
//            }
//            yPos += itemBG.Height + Spacing;

//        }

//        void itemBG_onRightClick(object sender, EventArgs e)
//        {
//            if (SpeedRunTimer.TrackingTime)
//            {
//                UIImage label = (UIImage)sender;
//                EventTimer timer = (EventTimer)label.Tag;
                
//                timer.RemoveStep();
//            }
//        }

//        void itemBG_onLeftClick(object sender, EventArgs e)
//        {
//            UIImage label = (UIImage)sender;
//            EventTimer timer = (EventTimer)label.Tag;
//            if (timer is CollectItemsTimer)
//            {
//                CollectItemsTimer itemTimer = (CollectItemsTimer)timer;
//                itemTimer.Collapsed = !itemTimer.Collapsed;
//                PopulateTimerList();
//            }
//            else
//            {
//                if (SpeedRunTimer.TrackingTime)
//                {
//                    timer.AddStep();
//                }
//            }
//        }

//        public void Tick(float deltaTime)
//        {
//            _time.Text = SpeedRunTimer.FormatTime(SpeedRunTimer.Timer);
//            for (int i = 0; i < eventTimers.Count; i++)
//            {
//                //_timerLabels[i].Text = SpeedRunTimer.FormatTime(eventTimers[i].Timer);
//            }
//        }

//        public override void Update()
//        {
//            this.Y = 100;
//            this.X = Main.screenWidth - this.Width - 16f;
//            base.Update();
//        }
//    }
//}
