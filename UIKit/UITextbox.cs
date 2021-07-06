using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;

namespace HEROsMod.UIKit
{
	internal class UITextbox : UIView
	{
		private RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
		internal static Asset<Texture2D> textboxBackground;
		internal static Texture2D textboxFill;

		private static Texture2D TextboxFill
		{
			get
			{
				if (textboxFill == null)
				{
					Color[] edgeColors = new Color[textboxBackground.Value.Width * textboxBackground.Value.Height];
					textboxBackground.Value.GetData(edgeColors);
					Color[] fillColors = new Color[textboxBackground.Value.Height];
					for (int y = 0; y < fillColors.Length; y++)
					{
						fillColors[y] = edgeColors[textboxBackground.Value.Width - 1 + y * textboxBackground.Value.Width];
					}
					textboxFill = new Texture2D(UIView.graphics, 1, fillColors.Length);
					textboxFill.SetData(fillColors);
				}
				return textboxFill;
			}
		}

		private bool focused = false;
		public bool HadFocus { get { return focused; } }
		public bool Numeric { get; set; }
		public bool HasDecimal { get; set; }
		private static float blinkTime = 1f;
		private static float timer = 0f;

		//bool eventSet = false;
		private float width = 200;

		public delegate void KeyPressedHandler(object sender, char key);

		public event EventHandler OnTabPress;

		public event EventHandler OnEnterPress;

		public event EventHandler OnLostFocus;

		public event KeyPressedHandler KeyPressed;

		private bool drawCarrot = false;
		private UILabel label = new UILabel();
		private static int padding = 4;
		private string text = "";

		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		private int maxCharacters = 20;

		public int MaxCharacters
		{
			get { return maxCharacters; }
			set { maxCharacters = value; }
		}

		private bool passwordBox = false;

		public bool PasswordBox
		{
			get { return passwordBox; }
			set { passwordBox = value; }
		}

		private string passwordString
		{
			get
			{
				string result = "";
				for (int i = 0; i < Text.Length; i++) result += "*";
				return result;
			}
		}

		public UITextbox()
		{
			this.onLeftClick += new EventHandler(UITextbox_onLeftClick);
			this.onRightClick += (a, b) => {
				Text = "";
				KeyPressed?.Invoke(this, ' ');
			};
			label.ForegroundColor = Color.Black;
			label.Scale = Height / label.Height;
			label.TextOutline = false;
			Numeric = false;
			HasDecimal = false;
			label.Position = new Vector2(4, 4);
			this.AddChild(label);
		}

		private void UITextbox_onLeftClick(object sender, EventArgs e)
		{
			Focus();
		}

		public void Focus()
		{
			if (!focused)
			{
				focused = true;
				//		Main.blockInput = true;
				//		Main.clrInput();
				timer = 0f;
				//eventSet = true;
			}
			//ModUtils.StopListeningForKeyEvents();
			//Main.RemoveKeyEvent();
			//keyBoardInput.newKeyEvent += new Action<char>(KeyboardInput_newKeyEvent);
		}

		public void Unfocus()
		{
			if (focused)
			{
				focused = false;
				//		Main.blockInput = false;

				OnLostFocus?.Invoke(this, EventArgs.Empty);
			}
			//if (!eventSet) return;
			//eventSet = false;
			//keyBoardInput.newKeyEvent -= new Action<char>(KeyboardInput_newKeyEvent);
			//ModUtils.StartListeningForKeyEvents();
			//Main.AddKeyEvent();
		}

		/*void KeyboardInput_newKeyEvent(char obj)
        {
            if (obj.Equals('\b'))
            {
                if (Text.Length > 0)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                    SetLabelPosition();
					KeyPressed?.Invoke(this, obj);
				}
            }
            else if(obj.Equals(''))
            {
                this.Unfocus();
            }
            else if (obj.Equals('\t'))
            {
				OnTabPress?.Invoke(this, new EventArgs());
			}
            else if (obj.Equals('\r'))
            {
                Main.chatRelease = false;
				OnEnterPress?.Invoke(this, new EventArgs());
			}*/
		/*else
		{
			for (int i = 0; i < label.font.Characters.Count; i++)
			{
				if (Text.Length < MaxCharacters && obj == label.font.Characters[i])
				{
					if(Numeric && obj.Equals('.'))
					{
						bool containsDecimal = false;
						for(int j = 0; j < text.Length; j++)
						{
							if(text[j] == obj)
							{
								containsDecimal = true;
								break;
							}
						}
						if(!containsDecimal)
						{
							Text += obj;
							SetLabelPosition();
							if (KeyPressed != null)
								KeyPressed(this, obj);
						}
					}
					if (!Numeric || char.IsNumber(obj) || (text.Length == 0 && obj.Equals('-')))
					{
						Text += obj;
						SetLabelPosition();
						if (KeyPressed != null)
							KeyPressed(this, obj);
					}
					break;
				}
			}
		}*/
		//}

		private void SetLabelPosition()
		{
			label.Position = new Vector2(padding, 0);

			Vector2 size = label.font.MeasureString(Text + "|") * label.Scale;
			if (passwordBox)
			{
				size = label.font.MeasureString(passwordString + "|") * label.Scale;
			}
			if (size.X > Width - padding * 2)
			{
				label.Position = new Vector2(padding - (size.X - (Width - padding * 2)), 0);
			}
		}

		protected override float GetWidth()
		{
			return width;
		}

		protected override void SetWidth(float width)
		{
			this.width = width;
		}

		protected override float GetHeight()
		{
			return textboxBackground.Value.Height;
		}

		public override void Update()
		{
			base.Update();
			if (!IsMouseInside() && MouseLeftButton)
			{
				Unfocus();
			}
			if (focused)
			{
				timer += ModUtils.DeltaTime;
				if (timer < blinkTime / 2) drawCarrot = true;
				else drawCarrot = false;
				if (timer >= blinkTime) timer = 0;
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (focused)
			{
				Terraria.GameInput.PlayerInput.WritingText = true;
				Main.instance.HandleIME();
				string oldText = Text;
				Text = Main.GetInputText(Text);
				if (oldText != Text)
				{
					KeyPressed?.Invoke(this, ' ');
				}
				if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab) && !Main.oldInputText.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Tab))
				{
					OnTabPress?.Invoke(this, new EventArgs());
				}
				if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && !Main.oldInputText.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
				{
					OnEnterPress?.Invoke(this, new EventArgs());
				}
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);
				Main.instance.DrawWindowsIMEPanel(new Vector2(98f, (float)(Main.screenHeight - 36)), 0f);
			}

			spriteBatch.Draw(textboxBackground.Value, DrawPosition, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0f);
			int fillWidth = (int)Width - 2 * textboxBackground.Value.Width;
			Vector2 pos = DrawPosition;
			pos.X += textboxBackground.Value.Width;
			spriteBatch.Draw(TextboxFill, pos - Origin, null, Color.White, 0f, Vector2.Zero, new Vector2(fillWidth, 1f), SpriteEffects.None, 0f);
			pos.X += fillWidth;
			spriteBatch.Draw(textboxBackground.Value, pos, null, Color.White, 0f, Origin, 1f, SpriteEffects.FlipHorizontally, 0f);
			string drawString = Text;
			if (PasswordBox) drawString = passwordString;
			if (drawCarrot && focused) drawString += "|";
			label.Text = drawString;

			pos = DrawPosition - Origin;

			if (pos.X <= Main.screenWidth && pos.Y <= Main.screenHeight && pos.X + Width >= 0 && pos.Y + Height >= 0)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, _rasterizerState, null, Main.UIScaleMatrix);

				Rectangle cutRect = new Rectangle((int)pos.X, (int)pos.Y, (int)Width, (int)Height);
				/*if (cutRect.X < 0)
				{
					cutRect.Width += cutRect.X;
					cutRect.X = 0;
				}
				if (cutRect.Y < 0)
				{
					cutRect.Height += cutRect.Y;
					cutRect.Y = 0;
				}
				if (cutRect.X + Width > Main.screenWidth) cutRect.Width = Main.screenWidth - cutRect.X;
				if (cutRect.Y + Height > Main.screenHeight) cutRect.Height = Main.screenHeight - cutRect.Y;*/
				cutRect = ModUtils.GetClippingRectangle(spriteBatch, cutRect);

				Rectangle currentRect = spriteBatch.GraphicsDevice.ScissorRectangle;
				spriteBatch.GraphicsDevice.ScissorRectangle = cutRect;

				base.Draw(spriteBatch);

				spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, Main.UIScaleMatrix);
			}
		}
	}
}