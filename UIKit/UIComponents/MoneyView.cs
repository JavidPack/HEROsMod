//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using HEROsMod.HEROsModVideo.Services.DropRateInfo;

//using Terraria;

//namespace HEROsMod.UIKit.UIComponents
//{
//    class MoneyView : UIView
//    {
//        private int _amount;
//        public int Amount
//        {
//            get
//            {
//                return _amount;
//            }
//            set
//            {
//                _amount = value;
//                Refresh();
//            }
//        }

//        public MoneyView(int amount)
//        {
//            this.Amount = amount;
//        }

//        private void Refresh()
//        {
//            this.RemoveAllChildren();
//            if(Amount > 0)
//            {
//                int amount = Amount;
//                int platinum = amount / DropCollection.PLATINUM_VALUE;
//                amount -= platinum * DropCollection.PLATINUM_VALUE;
//                int gold = amount / DropCollection.GOLD_VALUE;
//                amount -= gold * DropCollection.GOLD_VALUE;
//                int silver = amount / DropCollection.SILVER_VALUE;
//                amount -= silver * DropCollection.SILVER_VALUE;
//                int copper = amount;

//                float xPos = 0f;
//                if (platinum > 0)
//                {
//                    AddCoin(platinum, 74, ref xPos);
//                }
//                if (gold > 0)
//                {
//                    AddCoin(gold, 73, ref xPos);
//                }
//                if (silver > 0)
//                {
//                    AddCoin(silver, 72, ref xPos);
//                }
//                if (copper > 0)
//                {
//                    AddCoin(copper, 71, ref xPos);
//                }
//                this.Width = xPos;
//            }
//        }

//        private void AddCoin(int amount, int type, ref float xPos)
//        {
//            UILabel lAmount = new UILabel(amount.ToString());
//            lAmount.Scale = .4f;
//            lAmount.X = xPos + 2;
//            this.Height = lAmount.Height;
//            AddChild(lAmount);

//            UIImage coin = new UIImage(Main.itemTexture[type]);
//            coin.Anchor = AnchorPosition.Left;
//            coin.Y = this.Height / 2.5f;
//            coin.X = lAmount.X + lAmount.Width;
//            AddChild(coin);

//            xPos = coin.X + coin.Width;
//        }
//    }
//}