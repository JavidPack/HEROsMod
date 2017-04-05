//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using GameikiMod.GameikiServices;
//using Keys = Microsoft.Xna.Framework.Input.Keys;

//using Terraria;

//namespace GameikiMod.UIKit.UIComponents
//{
//    class KeybindWindow : UIWindow
//    {
//        UIScrollView scrollView;
//        static float spacing = 8f;

//        public KeybindWindow()
//        {
//            ModUtils.StopListeningForKeyEvents();
//            Main.blockInput = true;
//            this.Anchor = AnchorPosition.Center;
//            Width = 500;
//            Height = 600;

//            scrollView = new UIScrollView();
//            UIButton bOk = new UIButton("Ok");
//            UIButton bDefaults = new UIButton("Defaults");

//            bOk.Anchor = AnchorPosition.BottomRight;
//            bDefaults.Anchor = AnchorPosition.BottomRight;

//            bOk.Position = new Vector2(Width - spacing, Height - spacing);
//            bDefaults.Position = new Vector2(bOk.Position.X - bOk.Width - spacing, bOk.Position.Y);

//            scrollView.Position = new Vector2(spacing);
//            scrollView.Height = Height - bOk.Height - spacing * 3;
//            scrollView.Width = Width - spacing * 2;

//            bOk.onLeftClick += bOk_onLeftClick;
//            bDefaults.onLeftClick += bDefaults_onLeftClick;

//            AddControlsToScrollView();
//            AddChild(scrollView);
//            AddChild(bOk);
//            AddChild(bDefaults);

//        }
//        void Close()
//        {
//            ModUtils.StartListeningForKeyEvents();
//            Main.blockInput = false;
//            Parent.RemoveChild(this);
//        }
//        void bDefaults_onLeftClick(object sender, EventArgs e)
//        {
//            RestoreGameDefaults();
//            KeybindController.LoadDefaults();
//            AddControlsToScrollView();
//        }

//        void bOk_onLeftClick(object sender, EventArgs e)
//        {
//            KeybindController.SaveBindings();
//            Close();
//            if (Main.gameMenu)
//            {
//                Main.menuMode = 11;
//            }
//            else IngameOptions.Open();
//        }

//        static string[] GameControls = new string[]
//        {
//            "Up", "cUp",
//            "Down", "cDown",
//            "Left", "cLeft",
//            "Right", "cRight",
//            "Jump", "cJump",
//            "Throw", "cThrowItem",
//            "Inventory", "cInv",
//            "Quick Heal", "cHeal",
//            "Quick Buff", "cBuff",
//            "Grapple", "cHook",
//            "Auto Select", "cTorch",
//            "Toggle Map Style", "cMapStyle",
//            "Toggle Fullscreen", "cMapFull",
//            "Zoom In", "cMapZoomIn",
//            "Zoom Out", "cMapZoomOut",
//            "Decrease Transparency", "cMapAlphaUp",
//            "Increase Transparency", "cMapAlphaDown",

//        };

//        public static void SetGameControl(string varName, string value)
//        {
//            if (varName == GameControls[1]) Main.cUp = value;
//            if (varName == GameControls[3]) Main.cDown = value;
//            if (varName == GameControls[5]) Main.cLeft = value;
//            if (varName == GameControls[7]) Main.cRight = value;
//            if (varName == GameControls[9]) Main.cJump = value;
//            if (varName == GameControls[11]) Main.cThrowItem = value;
//            if (varName == GameControls[13]) Main.cInv = value;
//            if (varName == GameControls[15]) Main.cHeal = value;
//            if (varName == GameControls[17]) Main.cBuff = value;
//            if (varName == GameControls[19]) Main.cHook = value;
//            if (varName == GameControls[21]) Main.cTorch = value;

//            if (varName == GameControls[23]) Main.cMapStyle = value;
//            if (varName == GameControls[25]) Main.cMapFull = value;
//            if (varName == GameControls[27]) Main.cMapZoomIn = value;
//            if (varName == GameControls[29]) Main.cMapZoomOut = value;
//            if (varName == GameControls[31]) Main.cMapAlphaUp = value;
//            if (varName == GameControls[33]) Main.cMapAlphaDown = value;
//        }

//        public static string GetGameControl(string varName)
//        {
//            if (varName == GameControls[1]) return Main.cUp;
//            if (varName == GameControls[3]) return Main.cDown;
//            if (varName == GameControls[5]) return Main.cLeft;
//            if (varName == GameControls[7]) return Main.cRight;
//            if (varName == GameControls[9]) return Main.cJump;
//            if (varName == GameControls[11]) return Main.cThrowItem;
//            if (varName == GameControls[13]) return Main.cInv;
//            if (varName == GameControls[15]) return Main.cHeal;
//            if (varName == GameControls[17]) return Main.cBuff;
//            if (varName == GameControls[19]) return Main.cHook;
//            if (varName == GameControls[21]) return Main.cTorch;

//            if (varName == GameControls[23]) return Main.cMapStyle;
//            if (varName == GameControls[25]) return Main.cMapFull;
//            if (varName == GameControls[27]) return Main.cMapZoomIn;
//            if (varName == GameControls[29]) return Main.cMapZoomOut;
//            if (varName == GameControls[31]) return Main.cMapAlphaUp;
//            if (varName == GameControls[33]) return Main.cMapAlphaDown;
//            return "";
//        }

//        static void RestoreGameDefaults()
//        {
//            Main.cUp = "W";
//            Main.cDown = "S";
//            Main.cLeft = "A";
//            Main.cRight = "D";
//            Main.cJump = "Space";
//            Main.cThrowItem = "T";
//            Main.cInv = "Escape";
//            Main.cHeal = "H";
//            Main.cMana = "M";
//            Main.cBuff = "B";
//            Main.cHook = "E";
//            Main.cTorch = "LeftShift";
//            Main.cMapStyle = "Tab";
//            Main.cMapFull = "M";
//            Main.cMapZoomIn = "Add";
//            Main.cMapZoomOut = "Subtract";
//            Main.cMapAlphaUp = "PageUp";
//            Main.cMapAlphaDown = "PageDown";
//        }

//        void AddControlsToScrollView()
//        {
//            scrollView.ClearContent();
//            UIWindow gameControlsWindow = GetGameControlsWindow("Game Controls", 0, 22);
//            scrollView.AddChild(gameControlsWindow);
//            UIWindow mapControlsWindow = GetGameControlsWindow("Map Controls", 22, GameControls.Length);
//            mapControlsWindow.Position = new Vector2(gameControlsWindow.Position.X, gameControlsWindow.Position.Y + gameControlsWindow.Height + spacing);
//            scrollView.AddChild(mapControlsWindow);

//            UIView controls = GetKeyBindControls();
//            controls.Position = new Vector2(controls.Position.X, mapControlsWindow.Position.Y + mapControlsWindow.Height);
//            scrollView.AddChild(controls);
//            scrollView.ContentHeight = gameControlsWindow.Height + mapControlsWindow.Height + controls.Height + spacing * 3;
//        }
//        UIWindow GetGameControlsWindow(string title, int start, int end)
//        {
//            UIWindow window = new UIWindow();
//            window.Width = scrollView.Width - 20 - spacing * 2;
//            window.Position = new Vector2(spacing);
//            UILabel categoryTitle = new UILabel(title);
//            categoryTitle.Scale = .6f;
//            categoryTitle.Position = new Vector2(spacing);
//            window.AddChild(categoryTitle);
//            for (int i = start; i < end; i += 2)
//            {
//                UILabel bindTitle = new UILabel(GameControls[i]);
//                bindTitle.Scale = .4f;
//                UIButton bindButton = new UIButton(GetGameControl(GameControls[i + 1]));
//                bindButton.AutoSize = false;
//                bindButton.Width = 100;
//                bindTitle.Position = new Vector2(spacing, window.GetLastChild().Position.Y + window.GetLastChild().Height + spacing * 2);
//                bindButton.Anchor = AnchorPosition.TopRight;
//                bindButton.Position = new Vector2(window.Width - spacing, bindTitle.Position.Y - 8);
//                bindButton.Tag = GameControls[i + 1];

//                bindButton.onLeftClick += bindButton_onLeftClick2;

//                window.AddChild(bindButton);
//                window.AddChild(bindTitle);
//            }
//            window.Height = window.GetLastChild().Position.Y + window.GetLastChild().Height + spacing;
//            return window;
//        }
//        UIView GetKeyBindControls()
//        {
//            UIView controls = new UIView();
//            controls.Width = scrollView.Width - 20 - spacing * 2;

//            List<UIWindow> windows = new List<UIWindow>();
//            for (int i = 0; i < KeybindController.bindCategories.Count; i++)
//            {
//                UIWindow window = new UIWindow();
//                window.Position = new Vector2(spacing);
//                window.Width = controls.Width;
//                BindCategory category = KeybindController.bindCategories[i];
//                if (category.bindings.Count > 0)
//                {
//                    UILabel categoryTitle = new UILabel(category.name);
//                    categoryTitle.Scale = .6f;
//                    categoryTitle.Position = new Vector2(spacing);
//                    window.AddChild(categoryTitle);
//                    for (int j = 0; j < category.bindings.Count; j++)
//                    {
//                        KeyBinding binding = category.bindings[j];

//                        UILabel bindTitle = new UILabel(binding.name);
//                        bindTitle.Scale = .4f;
//                        UIButton bindButton = new UIButton(binding.key.ToString());
//                        bindButton.AutoSize = false;
//                        bindButton.Width = 100;
//                        bindTitle.Position = new Vector2(spacing, window.GetLastChild().Position.Y + window.GetLastChild().Height + spacing * 2);
//                        bindButton.Anchor = AnchorPosition.TopRight;
//                        bindButton.Position = new Vector2(window.Width - spacing, bindTitle.Position.Y - 8);
//                        bindButton.Tag = binding;
//                        bindButton.onLeftClick += bindButton_onLeftClick;
//                        bindButton.onRightClick += bindButton_onRightClick;

//                        window.AddChild(bindButton);
//                        window.AddChild(bindTitle);
//                    }
//                }
//                if (windows.Count > 0) window.Position = new Vector2(spacing, windows[windows.Count - 1].Position.Y + windows[windows.Count - 1].Height + spacing);
//                window.Height = window.GetLastChild().Position.Y + window.GetLastChild().Height + spacing;
//                windows.Add(window);
//                controls.AddChild(window);
//            }
//            if (windows.Count > 0)
//            {
//                controls.Height = windows[windows.Count - 1].Position.Y + windows[windows.Count - 1].Height + spacing;
//            }
//            return controls;
//        }

//        void bindButton_onRightClick(object sender, EventArgs e)
//        {
//            UIButton button = (UIButton)sender;
//            KeyBinding keyBinding = (KeyBinding)button.Tag;
//            keyBinding.key = Keys.None;
//            button.Text = keyBinding.key.ToString();
//        }

//        void bindButton_onLeftClick(object sender, EventArgs e)
//        {
//            UIButton button = (UIButton)sender;
//            Parent.AddChild(new BindDialog(button));
//        }


//        void bindButton_onLeftClick2(object sender, EventArgs e)
//        {
//            UIButton button = (UIButton)sender;
//            Parent.AddChild(new BindDialog(button, true));
//        }

//        public override void Update()
//        {
//            this.CenterToParent();
//            base.Update();
//        }
//    }
//    class BindDialog : UIWindow
//    {
//        UIButton button;
//        bool gameControl;
//        public BindDialog(UIButton button, bool gameControl = false)
//        {
//            this.gameControl = gameControl;
//            this.button = button;
//            UIView.exclusiveControl = this;

//            this.Anchor = AnchorPosition.Center;
//            Width = 250;
//            UIWrappingLabel label = new UIWrappingLabel("Press any key to bind.  Click anywhere to cancel.", 220);
//            label.Anchor = AnchorPosition.Center;
//            Height = label.Height + 20;
//            AddChild(label);
//            label.CenterToParent();
//        }

//        public override void Update()
//        {
//            Keys[] keysPressed = Main.keyState.GetPressedKeys();
//            if (MouseLeftButton) this.Close();
//            if (keysPressed.Length > 0)
//            {
//                Keys pressedKey = keysPressed[0];
//                if (pressedKey != Keys.None)
//                {
//                    if (gameControl)
//                    {
//                        string varName = (string)button.Tag;
//                        KeybindWindow.SetGameControl(varName, pressedKey.ToString());
//                        button.Text = KeybindWindow.GetGameControl(varName);
//                    }
//                    else
//                    {
//                        KeyBinding binding = (KeyBinding)button.Tag;
//                        binding.key = pressedKey;
//                        button.Text = pressedKey.ToString();
//                    }
//                    this.Close();
//                }
//            }
//            this.CenterToParent();
//            base.Update();
//        }

//        void Close()
//        {
//            UIView.exclusiveControl = null;
//            Parent.RemoveChild(this);
//        }
//    }
//}
