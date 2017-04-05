//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameikiMod.UIKit;
//using GameikiMod.UIKit.UIComponents;
//using GameikiMod.GameikiVideo.Editor.WorldObjects;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System.Reflection;

//using Terraria;

//namespace GameikiMod.GameikiVideo.Editor.UI
//{
//    class PropertiesWindow : UIWindow
//    {
//        public object ObjectBeingInspected { get; set; }
//        PropertyInfo selectedProperty;
//        List<UIView> controls;
//        public PropertiesWindow(object obj)
//        {
//            this.CanMove = true;
//            controls = new List<UIView>();
//            this.Width = 400;
//            SetObject(obj);

//        }


//        public void SetObject(object obj)
//        {
//            this.ObjectBeingInspected = obj;
//            this.RemoveAllChildren();
//            controls.Clear();
//            this.Visible = true;

//            UIImage bClose = new UIImage(closeTexture);
//            bClose.X = this.Width - bClose.Width - LargeSpacing;
//            bClose.Y = LargeSpacing;
//            bClose.onLeftClick += bClose_onLeftClick;
//            AddChild(bClose);

//            UILabel lName = new UILabel("Properties");
//            lName.Scale = .4f;
//            lName.X = Spacing;
//            lName.Y = Spacing;
//            AddChild(lName);

//            if(obj != null)
//            {
//                Type type = obj.GetType();
//                AddProperties(lName.Y + lName.Height, obj);

//                if(obj is WorldObject)
//                {
//                    UILabel bDelete = new UILabel("Delete");
//                    bDelete.Scale = .4f;
//                    bDelete.X = this.Width - bDelete.Width - Spacing;
//                    bDelete.Y = GetLastChild().Y + this.GetLastChild().Height + Spacing;
//                    bDelete.onLeftClick += bDelete_onLeftClick;
//                    AddChild(bDelete);

//                    UILabel bGoTo = new UILabel("GoTo");
//                    bGoTo.Scale = .4f;
//                    bGoTo.X = bDelete.X - bGoTo.Width - Spacing;
//                    bGoTo.Y = bDelete.Y;
//                    bGoTo.onLeftClick += bGoTo_onLeftClick;
//                    AddChild(bGoTo);

//                    UILabel bClone = new UILabel("Clone");
//                    bClone.Scale = .4f;
//                    bClone.X = bGoTo.X - bClone.Width - Spacing;
//                    bClone.Y = bDelete.Y;
//                    bClone.onLeftClick += bClone_onLeftClick;
//                    AddChild(bClone);

//                }
//            }
//            this.Height = this.GetLastChild().Y + this.GetLastChild().Height + Spacing;

//            if(obj == null)
//            {
//                this.Visible = false;
//            }
//        }

//        void bClone_onLeftClick(object sender, EventArgs e)
//        {
//            WorldObject wo = (WorldObject)ObjectBeingInspected;
//            Editor.AddWorldObject(wo.Clone());
//        }

//        void bGoTo_onLeftClick(object sender, EventArgs e)
//        {
//            WorldObject wo = (WorldObject)this.ObjectBeingInspected;
//            Editor.MoveCameraTo(wo.Position);
//        }

//        void bDelete_onLeftClick(object sender, EventArgs e)
//        {
//            WorldObject wo = (WorldObject)this.ObjectBeingInspected;
//            Editor.DeleteWorldObject(wo);
//            this.SetObject(null);
//        }

//        void bClose_onLeftClick(object sender, EventArgs e)
//        {
//            this.Visible = false;
//        }
//        private void AddProperties(float yPos, object obj)
//        {
//            Type type = obj.GetType();
//            foreach(var p in type.GetProperties())
//            {
//                bool hiddenInInspector = false;
//                var attributes = p.GetCustomAttributes(true);
//                foreach(var attr in attributes)
//                {
//                    if(attr is HideInInspector)
//                    {
//                        hiddenInInspector = true;
//                    }
//                }
//                if(!hiddenInInspector && p.CanRead && p.CanWrite)
//                {
//                    AddLabel(p.Name, ref yPos);
//                    if(p.PropertyType == typeof(WorldObject))
//                    {
//                        WorldObject worldObj = (WorldObject)p.GetValue(ObjectBeingInspected, null);
//                        if(worldObj == null)
//                        {
//                            UIButton bSelectTarget = new UIButton("SelectTarget");
//                            bSelectTarget.X = this.Width - bSelectTarget.Width - Spacing;
//                            bSelectTarget.Y = GetLastChild().Y;
//                            bSelectTarget.onLeftClick += bSelectTarget_onLeftClick;
//                            bSelectTarget.Tag = p;
//                            AddChild(bSelectTarget);
//                        }
//                        else
//                        {
//                            UIButton bGotoTarget = new UIButton("GoTo");
//                            bGotoTarget.X = this.Width - bGotoTarget.Width - Spacing;
//                            bGotoTarget.Y = GetLastChild().Y;
//                            bGotoTarget.onLeftClick += bGotoTarget_onLeftClick;
//                            bGotoTarget.Tag = worldObj;
//                            AddChild(bGotoTarget);

//                            UIButton bRemoveTarget = new UIButton("Remove");
//                            bRemoveTarget.X = bGotoTarget.X - bRemoveTarget.Width;
//                            bRemoveTarget.Y = bGotoTarget.Y;
//                            bRemoveTarget.Tag = p;
//                            bRemoveTarget.onLeftClick += bRemoveTarget_onLeftClick;
//                            AddChild(bRemoveTarget);
//                        }

//                    }
//                    else if(p.PropertyType == typeof(string))
//                    {
//                        UITextbox strTextbox = new UITextbox();
//                        strTextbox.X = this.Width - strTextbox.Width - Spacing;
//                        strTextbox.Y = yPos;
//                        strTextbox.Tag = p;
//                        strTextbox.KeyPressed += strTextbox_KeyPressed;
//                        controls.Add(strTextbox);
//                        AddChild(strTextbox);
//                    }
//                    else if(p.PropertyType.IsEnum)
//                    {
//                        var members = p.PropertyType.GetMembers(BindingFlags.Public | BindingFlags.Static);
//                        UIDropdown dropdown = new UIDropdown();
//                        dropdown.Width = 200;
//                        dropdown.Y = yPos;
//                        dropdown.X = this.Width - dropdown.Width - Spacing;
                        
//                        foreach(var member in members)
//                        {
//                            dropdown.AddItem(member.Name);
//                        }
//                        dropdown.Tag = p;
//                        dropdown.selectedChanged += dropdown_selectedChanged;
//                        controls.Add(dropdown);
//                        AddChild(dropdown);
//                    }
//                    else if(p.PropertyType == typeof(Texture2D))
//                    {
//                        UIDropdown imageDropdown = new UIDropdown();
//                        imageDropdown.Width = 200;
//                        imageDropdown.Y = yPos;
//                        imageDropdown.X = this.Width - imageDropdown.Width - Spacing;

//                        foreach (string imageName in Editor.ImageNames)
//                        {
//                            imageDropdown.AddItem(imageName);
//                        }
//                        imageDropdown.Tag = p;
//                        imageDropdown.selectedChanged += imageDropdown_selectedChanged;
//                        controls.Add(imageDropdown);
//                        AddChild(imageDropdown);
//                    }
//                    else if(p.PropertyType == typeof(bool))
//                    {
//                        UICheckbox cb = new UICheckbox("Enabled");
//                        cb.Y = yPos;
//                        cb.X = this.Width - cb.Width - Spacing;
//                        cb.Tag = p;
//                        cb.SelectedChanged += cb_SelectedChanged;
//                        controls.Add(cb);
//                        AddChild(cb);
//                    }
//                    else if (p.PropertyType.IsNumeric())
//                    {
//                        Range range = null;
//                        foreach (var attr in attributes)
//                        {
//                            if (attr is Range)
//                            {
//                                range = (Range)attr;
//                            }
//                        }

//                        if (range == null)
//                        {
//                            UITextbox tb = new UITextbox();
//                            //tb.Text = p.GetValue(obj, null).ToString();
//                            tb.X = this.Width - tb.Width - Spacing;
//                            tb.Y = yPos;
//                            tb.Tag = p;
//                            tb.Numeric = true;
//                            tb.KeyPressed += tb_KeyPressed;
//                            controls.Add(tb);
//                            AddChild(tb);
//                        }
//                        else
//                        {
//                            SliderWithTextbox slider = new SliderWithTextbox(1f, range.Min, range.Max);
//                            slider.X = this.Width - slider.Width - Spacing;
//                            slider.Y = yPos;
//                            slider.Tag = p;
//                            slider.ValueChanged += slider_ValueChanged;
//                            controls.Add(slider);
//                            AddChild(slider);
//                        }
//                    }
//                    else if(p.PropertyType == typeof(Color))
//                    {
//                        UIColorPicker colorPicker = new UIColorPicker();
//                        colorPicker.X = this.Width - colorPicker.Width - Spacing;
//                        colorPicker.Y = yPos;
//                        colorPicker.Tag = p;
//                        colorPicker.ColorChanged += colorPicker_ColorChanged;
//                        controls.Add(colorPicker);
//                        AddChild(colorPicker);
//                    }
//                    yPos += GetLastChild().Height;
//                }
//            }
//        }

//        void colorPicker_ColorChanged(object sender, EventArgs e)
//        {
//            UIColorPicker colorPicker = (UIColorPicker)sender;
//            PropertyInfo p = (PropertyInfo)colorPicker.Tag;
//            p.SetValue(ObjectBeingInspected, colorPicker.Color, null);
//        }

//        void imageDropdown_selectedChanged(object sender, EventArgs e)
//        {
//            UIDropdown dropdown = (UIDropdown)sender;
//            PropertyInfo p = (PropertyInfo)dropdown.Tag;
//            p.SetValue(ObjectBeingInspected, Editor.Images[dropdown.SelectedItem], null);
//        }

//        void strTextbox_KeyPressed(object sender, char key)
//        {
//            UITextbox textbox = (UITextbox)sender;
//            PropertyInfo p = (PropertyInfo)textbox.Tag;
//            p.SetValue(ObjectBeingInspected, textbox.Text, null);
//        }


//        void slider_ValueChanged(object sender, EventArgs e)
//        {
//            SliderWithTextbox slider = (SliderWithTextbox)sender;
//            PropertyInfo p = (PropertyInfo)slider.Tag;
//            p.SetValue(ObjectBeingInspected, slider.Value, null);
//        }

//        void tb_KeyPressed(object sender, char key)
//        {
//            UITextbox textbox = (UITextbox)sender;
//            PropertyInfo p = (PropertyInfo)textbox.Tag;

//            // workaround for odd bug
//            if (!controls.Contains(textbox))
//                return;
            
//            if(textbox.Text.Length == 0 || textbox.Text == "-")
//            {
//                p.SetValue(ObjectBeingInspected, 0, null);
//                return;
//            }

//            var converter = System.ComponentModel.TypeDescriptor.GetConverter(p.PropertyType);
//            var value = converter.ConvertFrom(textbox.Text);
//            p.SetValue(ObjectBeingInspected, value, null);
//        }

//        void bRemoveTarget_onLeftClick(object sender, EventArgs e)
//        {
//            PropertyInfo p = (PropertyInfo)((UIButton)sender).Tag;
//            p.SetValue(ObjectBeingInspected, null, null);
//            SetObject(ObjectBeingInspected);
//        }

//        void bGotoTarget_onLeftClick(object sender, EventArgs e)
//        {
//            WorldObject worldObj = (WorldObject)((UIView)sender).Tag;
//            Main.screenPosition = worldObj.Position;
//            Main.screenPosition.X -= Main.screenWidth / 2;
//            Main.screenPosition.Y -= Main.screenHeight / 2;
//        }

//        void bSelectTarget_onLeftClick(object sender, EventArgs e)
//        {
//            selectedProperty = (PropertyInfo)((UIButton)sender).Tag;
//            Editor.WorldObjectSelected += Editor_WorldObjectSelected;
//            Main.NewText("Select world object");
//        }

//        void Editor_WorldObjectSelected(WorldObject obj)
//        {
//            if(selectedProperty != null && obj != null)
//            {
//                selectedProperty.SetValue(ObjectBeingInspected, obj, null);
//                Main.NewText("Success");
//                SetObject(ObjectBeingInspected);
//            }
//            Editor.WorldObjectSelected -= Editor_WorldObjectSelected;
//        }

//        void dropdown_selectedChanged(object sender, EventArgs e)
//        {
//            UIDropdown dropdown = (UIDropdown)sender;
//            PropertyInfo p = (PropertyInfo)dropdown.Tag;
//            p.SetValue(ObjectBeingInspected, Enum.ToObject(p.PropertyType, dropdown.SelectedItem), null);
//        }

//        void cb_SelectedChanged(object sender, EventArgs e)
//        {
//            UICheckbox cb = (UICheckbox)sender;
//            ((PropertyInfo)cb.Tag).SetValue(ObjectBeingInspected, cb.Selected, null);
//        }

//        private void AddLabel(string text, ref float yPos)
//        {
//            UILabel label = new UILabel(text);
//            label.Scale = .4f;
//            label.X = Spacing;
//            label.Y = yPos;
//            this.AddChild(label);
//        }

//        public override void Update()
//        {
//            foreach(UIView control in controls)
//            {
//                PropertyInfo p = (PropertyInfo)control.Tag;
//                if(control is UITextbox)
//                {
//                    UITextbox textbox = (UITextbox)control;
//                    if(!textbox.HadFocus)
//                    {
//                        ((UITextbox)control).Text = p.GetValue(this.ObjectBeingInspected, null).ToString();
//                    }
//                }
//                else if(control is UIDropdown)
//                {
//                    if(p.PropertyType.IsEnum)
//                    {
//                        var value = p.GetValue(ObjectBeingInspected, null);
//                        int underlyingValue = (int)Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));
//                        ((UIDropdown)control).SelectedItem = underlyingValue;
//                    }
//                }
//                else if (control is UICheckbox)
//                {
//                    ((UICheckbox)control).Selected = (bool)p.GetValue(ObjectBeingInspected, null);
//                }
//                else if (control is SliderWithTextbox)
//                {
//                    ((SliderWithTextbox)control).Value = (float)p.GetValue(ObjectBeingInspected, null);
//                }
//                else if (control is UIColorPicker)
//                {
//                    ((UIColorPicker)control).Color = (Color)p.GetValue(ObjectBeingInspected, null);
//                }
//            }
//            base.Update();
//        }
//    }
//}
