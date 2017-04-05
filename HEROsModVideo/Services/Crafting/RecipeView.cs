//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.UIKit.UIComponents;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
////using GameikiMod.Interfaces;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.Crafting
//{
//    class RecipeView : UIWindow
//    {
//        public event EventHandler CanCraftChanged;
//        public Recipe Recipe { get; set; }
//        private ItemView _itemSlot;
//        UIButton bCraft;
//        public bool CanCraft { get; set; }
//        private bool _couldPreviouslyCraft;
//        public RecipeView(Recipe recipe)
//        {
//            this.Height = 32 + Spacing * 2;
//            this.Width = 300;

//            Player player = Main.player[Main.myPlayer];
//            CanCraft = player.CanCraft(recipe);
//            _couldPreviouslyCraft = CanCraft;

//            this.Recipe = recipe;
//            _itemSlot = new ItemView(recipe.createItem);
//            _itemSlot.item.stack = recipe.createItem.stack;
//            _itemSlot.X = Spacing;
//            _itemSlot.Y = Spacing;
//            _itemSlot.Width = 32;
//            _itemSlot.Height = 32;

//            UILabel lName = new UILabel(recipe.createItem.name);
//            lName.X = _itemSlot.X + _itemSlot.Width + Spacing;
//            lName.Y = SmallSpacing;
//            lName.Scale = .35f;
//            AddChild(lName);

//            bCraft = new UIButton("Craft");
//            bCraft.Anchor = AnchorPosition.Left;
//            bCraft.X = this.Width - bCraft.Width - Spacing;
//            bCraft.Y = this.Height / 2;
//            bCraft.onLeftClick += bCraft_onLeftClick;
//            AddChild(bCraft);

            
//            AddMaterials();
//            AddRequredTiles();
//            if (Recipe.needWater)
//                AddLiquid(Main.liquidTexture[0], 12, "Must be near water");
//            if(Recipe.needHoney)
//                AddLiquid(Main.liquidTexture[6], 12, "Must be near honey");
//            if (recipe.needLava)
//                AddLiquid(Main.liquidTexture[1], 0, "Must be near lava");

//            this.AddChild(_itemSlot);
//        }

//        void bCraft_onLeftClick(object sender, EventArgs e)
//        {
//            Player player = Main.player[Main.myPlayer];
//            player.Craft(Recipe);
//        }

//        void AddRequredTiles()
//        {
//            float xPos = bCraft.X - Spacing;
//            Item[] requiredTiles = Recipe.GetRequiredTiles();
//            foreach(Item requiredTile in requiredTiles)
//            {
//                ItemView itemView = new ItemView(requiredTile);
//                itemView.Width = 20;
//                itemView.Height = 20;
//                itemView.X = xPos - itemView.Width;
//                itemView.Y = this.Height - itemView.Height - SmallSpacing;
//                AddChild(itemView);
//                xPos -= itemView.Width - SmallSpacing;
//            }
//        }

//        void AddMaterials()
//        {
//            float xPos = _itemSlot.X + _itemSlot.Width + Spacing;
//            for(int i = 0; i < Recipe.requiredItem.Length; i++)
//            {
//                if(Recipe.requiredItem[i].type > 0)
//                {
//                    Item item = Recipe.requiredItem[i];
//                    ItemView itemView = new ItemView(item);
//                    itemView.item.stack = 1;
//                    itemView.Width = 20;
//                    itemView.Height = 20;
//                    itemView.X = xPos;
//                    itemView.Y = this.Height - itemView.Height - SmallSpacing;
//                    AddChild(itemView);
//                    if(item.stack > 1)
//                    {
//                        UILabel label = new UILabel("x" + item.stack);
//                        label.Anchor = AnchorPosition.Left;
//                        label.Scale = .3f;
//                        label.X = itemView.X + itemView.Width;
//                        label.Y = itemView.Y + itemView.Height / 2 + 2;
//                        AddChild(label);
//                        xPos = label.X + label.Width + SmallSpacing;
//                    }
//                    else
//                    {
//                        xPos += itemView.Width + SmallSpacing;
//                    }
//                }
//            }
//        }

//        void AddLiquid(Texture2D texture, int frame, string toolTip)
//        {
//            float xPos = bCraft.X - Spacing;
//            UIImage image = new UIImage(texture);
//            image.SourceRectangle = new Rectangle(18 * frame, 0, 16, 16);
//            image.Scale = 20 / 16;
//            image.X = xPos - image.Width;
//            image.Y = this.Height - image.Height - SmallSpacing;
//            image.Tooltip = toolTip;
//            AddChild(image);
//        }

//        public override void Update()
//        {
//            Player player = Main.player[Main.myPlayer];
//            CanCraft = player.CanCraft(Recipe);
//            if (CanCraft)
//            {
//                this.BackgroundColor = new Color(53, 35, 111, 255) * 0.685f;
//            }
//            else
//            {
//                this.BackgroundColor = Color.DarkOrange * .4f;
//            }
//            if (CanCraft != _couldPreviouslyCraft)
//            {
                
//                if(CanCraftChanged != null)
//                {
//                    CanCraftChanged(this, EventArgs.Empty);
//                }
//            }
//            _couldPreviouslyCraft = CanCraft;
//            base.Update();
//        }
//    }
//}
