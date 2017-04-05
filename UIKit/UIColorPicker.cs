using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using Terraria;

namespace HEROsMod.UIKit
{
    class UIColorPicker: UIView
    {
        public event EventHandler ColorChanged;
        public Color Color
        {
            get { return Main.hslToRgb(Hue, Saturation, Luminosity); }
            set 
            {
                Vector3 hsl = Main.rgbToHsl(value);
                Hue = hsl.X;
                Saturation = hsl.Y;
                Luminosity = hsl.Z;
            }
        }
        public float Hue
        {
            get { return hueSlider.Value; }
            set 
            {
                hueSlider.Value = value;
                saturationSlider.Hue = value;
                luminositySlider.Hue = value;
            }
        }

        public float Saturation
        {
            get { return saturationSlider.Value; }
            set 
            { 
                saturationSlider.Value = value;
                luminositySlider.Saturation = value;
            }
        }

        public float Luminosity
        {
            get { return luminositySlider.Value; }
            set 
            {
                luminositySlider.Value = value;
                saturationSlider.Luminosity = value;
            }
        }

        HueSlider hueSlider;
        SaturationSlider saturationSlider;
        LuminositySlider luminositySlider;

        public UIColorPicker()
        {
            hueSlider = new HueSlider();
            saturationSlider = new SaturationSlider();
            luminositySlider = new LuminositySlider();

            saturationSlider.Y = hueSlider.Height;
            luminositySlider.Y = saturationSlider.Y + saturationSlider.Height;
            this.Width = hueSlider.Width;
            this.Height = luminositySlider.Y + luminositySlider.Height;

            hueSlider.valueChanged += hueSlider_valueChanged;
            saturationSlider.valueChanged += saturationSlider_valueChanged;
            luminositySlider.valueChanged += luminositySlider_valueChanged;

            this.Color = Color.White;

            AddChild(hueSlider);
            AddChild(saturationSlider);
            AddChild(luminositySlider);
        }

        void TriggerColorChangedEvent()
        {
            if(ColorChanged != null)
            {
                ColorChanged(this, EventArgs.Empty);
            }
        }

        void luminositySlider_valueChanged(object sender, float value)
        {
            Luminosity = luminositySlider.Value;
            TriggerColorChangedEvent();
        }

        void saturationSlider_valueChanged(object sender, float value)
        {
            Saturation = saturationSlider.Value;
            TriggerColorChangedEvent();
        }

        void hueSlider_valueChanged(object sender, float value)
        {
            Hue = hueSlider.Value;
            TriggerColorChangedEvent();
        }
    }
}
