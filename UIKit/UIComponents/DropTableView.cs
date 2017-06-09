//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using HEROsMod.UIKit;
//using HEROsMod.HEROsModVideo.Services.DropRateInfo;

//using Terraria;

//namespace HEROsMod.UIKit.UIComponents
//{
//    class DropTableView : UIView
//    {
//        NPCDropTable dropTable;
//        int iconSize = 32;
//        public DropTableView(NPCDropTable dropTable, float width)
//        {
//            this.Width = width;
//            this.Height = 0;
//            this.dropTable = dropTable;
//            PopulateList();
//        }

//        public void PopulateList()
//        {
//            this.RemoveAllChildren();
//            float yPos = 0f;

//            if (dropTable.MoneyDrops > 0)
//            {
//                MoneyView minMoney = new MoneyView(dropTable.MinMoney);
//                //minMoney.X = Spacing * 2 + 40;
//                AddChild(minMoney);

//                UILabel seperator = new UILabel(" - ");
//                seperator.Scale = .4f;
//                seperator.X = minMoney.X + minMoney.Width;
//                AddChild(seperator);

//                MoneyView maxMoney = new MoneyView(dropTable.MaxMoney);
//                maxMoney.X = seperator.X + seperator.Width;
//                AddChild(maxMoney);

//                yPos = minMoney.Y + minMoney.Height + 2;
//            }

//            float amountWidth = 35f;
//            foreach(var item in dropTable.Drops)
//            {
//                string dropAmount = GetDropAmount(item);

//                UIImage itemImage = new UIImage(Main.itemTexture[item.NPCType]);
//                itemImage.Anchor = AnchorPosition.Center;
//                itemImage.X = amountWidth + iconSize / 2;
//                itemImage.Y = yPos + iconSize / 2;
//                float itemScale = 1f;
//                float pixelWidth = iconSize;
//                if (itemImage.Width > pixelWidth || itemImage.Height > pixelWidth)
//                {
//                    if (itemImage.Width > itemImage.Height)
//                    {
//                        itemScale = pixelWidth / (float)itemImage.Width;
//                    }
//                    else
//                    {
//                        itemScale = pixelWidth / (float)itemImage.Height;
//                    }
//                }
//                itemImage.Scale = itemScale;
//                AddChild(itemImage);

//                if (dropAmount.Length > 0)
//                {
//                    UILabel lDropAmount = new UILabel(dropAmount);
//                    lDropAmount.Scale = .3f;
//                    lDropAmount.Anchor = AnchorPosition.Center;
//                    lDropAmount.X = amountWidth / 2;
//                    lDropAmount.Y = itemImage.Y;
//                    AddChild(lDropAmount);
//                }

//                UILabel lName = new UILabel(Lang.itemName(item.NPCType, true));
//                lName.Anchor = AnchorPosition.Left;
//                lName.Scale = .35f;
//                lName.X = itemImage.X + iconSize / 2 + Spacing;
//                lName.Y = itemImage.Y;
//                AddChild(lName);

//                float percent = item.Percent * 100;
//                percent = (float)Math.Round(percent, 2);
//                UILabel lPercentage = new UILabel("%" + percent);
//                lPercentage.Anchor = AnchorPosition.Left;
//                lPercentage.Scale = .35f;
//                lPercentage.X = this.Width - lPercentage.Width - Spacing;
//                lPercentage.Y = itemImage.Y;
//                AddChild(lPercentage);

//                yPos += iconSize + 2;
//            }
//            this.Height = yPos;
//        }

//        private string GetDropAmount(ItemDropInfo item)
//        {
//            if(item.Min == item.Max)
//            {
//                if(item.Min == 1)
//                {
//                    return "";
//                }
//                else
//                {
//                    return item.Min.ToString();
//                }
//            }
//            else
//            {
//                return item.Min + "-" + item.Max;
//            }
//        }

//    }
//}