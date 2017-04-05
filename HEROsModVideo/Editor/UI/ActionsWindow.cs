//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.GameikiVideo.Editor.Actions;
//using Action = GameikiMod.GameikiVideo.Editor.Actions.StageAction;
//using System.Reflection;


//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class ActionsWindow : UIWindow
//    {
//        UIScrollView scrollView;
//        public delegate void ActionEvent(Action action);
//        public event ActionEvent ActionSelected;

//        public ActionsWindow()
//        {
//            this.CanMove = true;
//            this.Width = 500;

//            scrollView = new UIScrollView();
//            scrollView.X = LargeSpacing;
//            scrollView.Y = LargeSpacing;
//            scrollView.Width = Width - LargeSpacing * 2;

//            string nameSpace = "GameikiMod.GameikiVideo.Editor.Actions";
//            var types = from type in Assembly.GetExecutingAssembly().GetTypes()
//                        where type.IsClass && type.Namespace == nameSpace && type.Name != "StageAction" && type.Name != "ActionEvent"
//                         select type;
//            types.ToList().ForEach(type => Console.WriteLine(type.Name));
//            var typeList = types.ToList();
//            //typeList[0].Name

//            float yPos = Spacing;
//            for (int i = 0; i < typeList.Count; i++)
//            {
//                UILabel label = new UILabel(typeList[i].Name);
//                label.Tag = typeList[i];
//                label.Scale = .4f;
//                label.X = Spacing;
//                label.Y = yPos;
//                label.onLeftClick += label_onLeftClick;
//                yPos += label.Height;
//                scrollView.AddChild(label);
//            }

//            AddChild(scrollView);
//        }

//        void label_onLeftClick(object sender, EventArgs e)
//        {
//            UILabel label = (UILabel)sender;
//            Type type = (Type)label.Tag;
//            StageAction action = (Action)Activator.CreateInstance(type);
            
//            if(ActionSelected != null)
//            {
//                ActionSelected(action);
//            }
//            //CreatingType = type;
//        }
//    }
//}
