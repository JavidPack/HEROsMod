//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//using Terraria;

//namespace GameikiMod.UIKit
//{
//    class UITestController
//    {
//        static UIWindow window;
//        static UIImage image;
//        public static void Init()
//        {
//            window = new UIWindow();
//            window.CanMove = true;
//            window.Position = new Vector2(100, 100);
//            window.Height = 500;
            
//            UILabel label = new UILabel("Test Label");
//            label.Position = new Vector2(100, 50);
//            label.Anchor = AnchorPosition.Center;
//            label.Scale = .5f;

//            image = new UIImage(UIView.GetEmbeddedTexture("Images/sunIcon"));
//            image.Anchor = AnchorPosition.Center;
//            image.Position = new Vector2(100, 150);
//            image.Scale = 2f;

//            UIButton bCancel = new UIButton("Cancel");
//            UIButton bOk = new UIButton("Ok");
//            bCancel.Anchor = AnchorPosition.BottomRight;
//            bCancel.Position = new Vector2(window.Width - 8, window.Height - 8);
//            bOk.Anchor = bCancel.Anchor;
//            bOk.Position = new Vector2(bCancel.Position.X - bCancel.Width - 8, bCancel.Position.Y);
            

//            UISlider slider = new UISlider();
//            slider.Position = new Vector2(100, 100);
//            slider.Anchor = AnchorPosition.Center;
//            slider.Value = 1f;
//            slider.valueChanged += new UISlider.SliderEventHandler(slider_valueChanged);

//            UICheckbox checkbox = new UICheckbox("Check Me");
//            checkbox.Position = new Vector2(100, 200);
//            checkbox.Anchor = AnchorPosition.Center;

           
//            UIListView listView = new UIListView();
//            listView.Position = new Vector2(8, 8);
//            for (int i = 0; i < 20; i++)
//            {
//                listView.AddItem("Item " + i);
//            }
             

//            UIScrollView scrollView = new UIScrollView();
//            scrollView.Position = new Vector2(8, 8);
//            scrollView.AddChild(listView);
//            scrollView.ContentHeight = listView.Height;
//            scrollView.ScrollPosition = scrollView.ContentHeight / 2;

//            UITextbox textbox = new UITextbox();
//            textbox.Position = new Vector2(180, 250);

//            UIWrappingLabel wrappingText = new UIWrappingLabel();
//            wrappingText.Text = "This is a test for wrapping text.  I sure hope that is does not break... That would really really suck.";
//            wrappingText.Position = new Vector2(100, 350);

//            //UIMessageBox messageBox = new UIMessageBox("This is a test Message Box with some test text in it for testing the test Message Box that is only for testing purposes");


//            window.AddChild(label);
//            window.AddChild(image);
//            window.AddChild(bCancel);
//            window.AddChild(bOk);
//            window.AddChild(slider);
//            window.AddChild(checkbox);
//            window.AddChild(scrollView);
//            window.AddChild(textbox);
//            window.AddChild(wrappingText);
//            //window.AddChild(messageBox);
//            //window.AddChild(listView);
//            slider.CenterXAxisToParentCenter();
//            image.CenterXAxisToParentCenter();
//            label.CenterXAxisToParentCenter();
//            checkbox.CenterXAxisToParentCenter();

//        }

//        static void slider_valueChanged(object sender, float value)
//        {
//            image.Opacity = value;
//        }


//        public static void Update()
//        {
//            window.Update();
//        }

//        public static void Draw(SpriteBatch spriteBatch)
//        {
//            window.Draw(spriteBatch);
//        }
//    }
//}
