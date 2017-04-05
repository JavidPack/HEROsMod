//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.UIKit.UIComponents;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Services.Crafting
//{
//    class CraftingWindow : UIWindow
//    {
//        Recipe[] availableRecipes = new Recipe[0];
//        private UIScrollView _scrollView;
//        public CraftingWindow()
//        {
//            this.CanMove = true;
//            this.Width = 350;
//            this.Height = 250;

//            _scrollView = new UIScrollView();
//            _scrollView.Width = this.Width - LargeSpacing * 2;
//            _scrollView.Height = this.Height - LargeSpacing * 2;
//            _scrollView.X = LargeSpacing;
//            _scrollView.Y = LargeSpacing;

//            PopulateRecipes();
//            ModUtils.InventoryChanged += ModUtils_InventoryChanged;

//            this.AddChild(_scrollView);
//        }


//        void ModUtils_InventoryChanged(object sender, EventArgs e)
//        {
//            Player player = Main.player[Main.myPlayer];
//            availableRecipes = player.GetAvailableRecipes().OrderBy(x => !player.CanCraft(x)).ToArray();
//            PopulateRecipes();
//        }

        

//        public void PopulateRecipes()
//        {
//            float previousScrollPos = _scrollView.ScrollPosition;
//            float yPos = Spacing;
//            _scrollView.ClearContent();
//            for(int i =0; i < availableRecipes.Length; i++)
//            {
//                Recipe recipe = availableRecipes[i];

//                RecipeView itemView = new RecipeView(recipe);
//                itemView.X = 0;
//                itemView.Y = yPos;
//                itemView.CanCraftChanged += itemView_CanCraftChanged;
//                yPos += itemView.Height + Spacing;
//                _scrollView.AddChild(itemView);
//            }
//            _scrollView.ContentHeight = yPos;
//            _scrollView.ScrollPosition = previousScrollPos;
//        }

//        void itemView_CanCraftChanged(object sender, EventArgs e)
//        {
//            ReorderRecipes();
//        }

//        public void ReorderRecipes()
//        {
//            List<RecipeView> recipeViews = new List<RecipeView>();
//            foreach(UIView view in _scrollView.children)
//            {
//                if(view is RecipeView)
//                {
//                    recipeViews.Add((RecipeView)view);
//                }
//            }
//            recipeViews = recipeViews.OrderBy(x => !Main.player[Main.myPlayer].CanCraft(x.Recipe)).ToList();

//            float yPos = Spacing;
//            foreach(RecipeView view in recipeViews)
//            {
//                view.Y = yPos;
//                yPos += view.Height + Spacing;
//            }
//        }
//    }
//}
