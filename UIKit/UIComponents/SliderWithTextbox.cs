using System;

namespace HEROsMod.UIKit.UIComponents
{
	internal class SliderWithTextbox : UIView
	{
		public float Value
		{
			get { return slider.Value; }
			set
			{
				slider.Value = value;
			}
		}

		public float Min
		{
			get { return slider.MinValue; }
			set { slider.MinValue = value; }
		}

		public float Max
		{
			get { return slider.MaxValue; }
			set { slider.MaxValue = value; }
		}

		public event EventHandler ValueChanged;

		private UITextbox textbox;
		private UISlider slider;

		public SliderWithTextbox(float startValue, float minValue, float maxValue)
		{
			textbox = new UITextbox();
			textbox.Width = 125;
			textbox.KeyPressed += textbox_KeyPressed;
			textbox.OnLostFocus += textbox_OnLostFocus;
			textbox.Numeric = true;
			textbox.HasDecimal = true;
			slider = new UISlider();
			slider.valueChanged += slider_valueChanged;

			slider.X = textbox.X + textbox.Width + Spacing;
			AddChild(textbox);
			AddChild(slider);

			slider.MinValue = minValue;
			slider.MaxValue = maxValue;
			slider.Value = startValue;

			textbox.Text = slider.Value.ToString();

			this.Height = textbox.Height;
			this.Width = slider.X + slider.Width;
		}

		private void textbox_OnLostFocus(object sender, EventArgs e)
		{
			textbox.Text = slider.Value.ToString();
		}

		private void textbox_KeyPressed(object sender, char key)
		{
			if (textbox.Text.Length == 0 || textbox.Text == "-")
			{
				slider.Value = slider.MinValue;
				return;
			}
			slider.Value = float.Parse(textbox.Text);
			if (ValueChanged != null)
			{
				ValueChanged(this, EventArgs.Empty);
			}
		}

		private void slider_valueChanged(object sender, float value)
		{
			if (!textbox.HadFocus)
			{
				textbox.Text = slider.Value.ToString();
			}
			if (ValueChanged != null)
			{
				ValueChanged(this, EventArgs.Empty);
			}
		}
	}
}