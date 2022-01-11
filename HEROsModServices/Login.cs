using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class Login : HEROsModService
	{
		private static bool _loggedIn = false;

		public static bool LoggedIn
		{
			get { return _loggedIn; }
			set
			{
				_loggedIn = value;
				LoginStatusChanged?.Invoke(null, EventArgs.Empty);
			}
		}

		internal static Asset<Texture2D> _loginTexture;
		internal static Asset<Texture2D> _logoutTexture;

		private static event EventHandler LoginStatusChanged;

		public Login()
		{
			MultiplayerOnly = true;
			if (_loginTexture == null)
			{
				_loginTexture = HEROsMod.instance.Assets.Request<Texture2D>("Images/login", AssetRequestMode.ImmediateLoad);
			}
			if (_logoutTexture == null)
			{
				_logoutTexture = HEROsMod.instance.Assets.Request<Texture2D>("Images/logout", AssetRequestMode.ImmediateLoad);
			}
			this._name = "Login";
			this._hotbarIcon = new UIImage(_loginTexture);
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			LoginStatusChanged += Login_LoginStatusChanged;
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("Login");
			this.HasPermissionToUse = true;
		}

		private void Login_LoginStatusChanged(object sender, EventArgs e)
		{
			//ErrorLogger.Log("Login_LoginStatusChanged to "+ LoggedIn);
			if (LoggedIn)
			{
				this._hotbarIcon.Texture = _logoutTexture;
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("Logout");
			}
			else
			{
				this._hotbarIcon.Texture = _loginTexture;
				this.HotbarIcon.Tooltip = HEROsMod.HeroText("Login");
			}
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (LoggedIn)
			{
				HEROsModNetwork.LoginService.RequestLogout();
			}
			else
			{
				MasterView.gameScreen.AddChild(new LoginWindow());
			}
		}

		public override void Destroy()
		{
			//ErrorLogger.Log("Destroy");
			LoginStatusChanged -= Login_LoginStatusChanged;
			LoggedIn = false;
			base.Destroy();
		}
	}

	internal class LoginWindow : UIWindow
	{
		private UILabel lPassword = null;
		private UITextbox tbPassword = null;
		private UITextbox tbUsername = null;
		private UILabel lUsername = null;
		private UILabel lSaveLogin = null;
		private UIButton bSaveNone = null;
		private UIButton bSaveDefault = null;
		private UIButton bSavePlayer = null;
		private UICheckbox cbRememberPassword = null;
		private Color originalBGColor;
		private Color selectedBGColor;
		private LoginStorage loginStorage;
		private LoginSaveType saveType = LoginSaveType.None;
		private static float spacing = 16f;

		public LoginWindow()
		{
			UIView.exclusiveControl = this;

			Width = 600;
			this.Anchor = AnchorPosition.Center;

			lUsername = new UILabel(HEROsMod.HeroText("Username"));
			tbUsername = new UITextbox();
			lPassword = new UILabel(HEROsMod.HeroText("Password"));
			tbPassword = new UITextbox();
			tbPassword.PasswordBox = true;
			lSaveLogin = new UILabel(HEROsMod.HeroText("SaveLogin"));
			bSaveNone = new UIButton(HEROsMod.HeroText("SaveLoginNone"));
			bSaveDefault = new UIButton(HEROsMod.HeroText("SaveLoginDefault"));
			bSavePlayer = new UIButton(HEROsMod.HeroText("SaveLoginPlayer"));
			cbRememberPassword = new UICheckbox(HEROsMod.HeroText("RememberPassword"));
			UIButton bLogin = new UIButton(HEROsMod.HeroText("Login"));
			UIButton bCancel = new UIButton(HEROsMod.HeroText("Cancel"));
			UIButton bRegister = new UIButton(HEROsMod.HeroText("Register"));
			bRegister.AutoSize = false;
			bRegister.Width = 100;

			originalBGColor = bSaveNone.BackgroundColor;
			selectedBGColor = new Color(68, 72, 179);

			// Begin loading "Remember Me" data
			loginStorage = new LoginStorage();
			if (loginStorage.LoadJSON())
			{
				LoginInfo userInfo;
				string destinationServer = RetrieveDestinationServer();

				userInfo = loginStorage.GetLogin(destinationServer, Main.player[Main.myPlayer].name);

				SetToggle(userInfo.GetSaveType());

				if (userInfo.Username != "")
				{
					tbUsername.Text = userInfo.Username;
				}

				if (userInfo.Password != "")
				{
					tbPassword.Text = userInfo.Password;
					cbRememberPassword.Selected = true;
				}
			}
			else
			{
				SetToggle(LoginSaveType.None);
			}

			lUsername.Scale = .5f;
			lPassword.Scale = .5f;
			lSaveLogin.Scale = .5f;

			bLogin.Anchor = AnchorPosition.TopRight;
			bCancel.Anchor = AnchorPosition.TopRight;

			tbUsername.Width = 300;
			tbPassword.Width = tbUsername.Width;
			lUsername.X = spacing;
			lUsername.Y = spacing;
			tbUsername.X = lUsername.X + lSaveLogin.Width + spacing;
			tbUsername.Y = lUsername.Y;
			lPassword.X = lUsername.X;
			lPassword.Y = lUsername.Y + lUsername.Height + spacing;
			tbPassword.X = tbUsername.X;
			tbPassword.Y = lPassword.Y;
			lSaveLogin.X = lUsername.X;
			lSaveLogin.Y = lPassword.Y + lPassword.Height + spacing;
			bSaveNone.X = lSaveLogin.X + lSaveLogin.Width + spacing;
			bSaveNone.Y = lSaveLogin.Y;
			bSaveDefault.X = bSaveNone.X + bSaveNone.Width;
			bSaveDefault.Y = bSaveNone.Y;
			bSavePlayer.X = bSaveDefault.X + bSaveDefault.Width;
			bSavePlayer.Y = bSaveDefault.Y;
			cbRememberPassword.X = bSaveNone.X;
			cbRememberPassword.Y = lSaveLogin.Y + lSaveLogin.Height + spacing;

			bCancel.Position = new Vector2(this.Width - spacing, cbRememberPassword.Y + cbRememberPassword.Height + spacing);
			bLogin.Position = new Vector2(bCancel.Position.X - bCancel.Width - spacing - lSaveLogin.Width / 2, bCancel.Position.Y);
			bRegister.X = spacing;
			bRegister.Y = bCancel.Y;
			this.Height = bCancel.Y + bCancel.Height + spacing;
			
			bSaveNone.Tooltip = HEROsMod.HeroText("SaveLoginNoneTooltip");
			bSaveDefault.Tooltip = HEROsMod.HeroText("SaveLoginDefaultTooltip");
			bSavePlayer.Tooltip = HEROsMod.HeroText("SaveLoginPlayerTooltip");
			bRegister.Tooltip = HEROsMod.HeroText("RegisterTooltip");

			bCancel.onLeftClick += bCancel_onLeftClick;
			bLogin.onLeftClick += bLogin_onLeftClick;
			bRegister.onLeftClick += bRegister_onLeftClick;
			tbUsername.OnEnterPress += bLogin_onLeftClick;
			tbPassword.OnEnterPress += bLogin_onLeftClick;
			tbUsername.OnTabPress += tbUsername_OnTabPress;
			tbPassword.OnTabPress += tbPassword_OnTabPress;
			bSaveNone.onLeftClick += BSaveNone_onLeftClick;
			bSaveDefault.onLeftClick += BSaveDefault_onLeftClick;
			bSavePlayer.onLeftClick += BSavePlayer_onLeftClick;
			cbRememberPassword.onLeftClick += BRememberPassword_onLeftClick;

			AddChild(lUsername);
			AddChild(tbUsername);
			AddChild(lPassword);
			AddChild(tbPassword);
			AddChild(lSaveLogin);
			AddChild(bSaveNone);
			AddChild(bSaveDefault);
			AddChild(bSavePlayer);
			AddChild(cbRememberPassword);
			AddChild(bLogin);
			AddChild(bCancel);
			AddChild(bRegister);

			if (tbUsername.Text != "")
			{
				tbPassword.Focus();
			}
			else
			{
				tbUsername.Focus();
			}
		}

		private void SetToggle(LoginSaveType _saveType)
		{
			saveType = _saveType;

			UIButton[] buttons = { bSaveNone, bSaveDefault, bSavePlayer };

			foreach (var button in buttons)
			{
				button.SetBackgroundColor(originalBGColor);
			}

			if (_saveType == LoginSaveType.None)
			{
				bSaveNone.SetBackgroundColor(selectedBGColor);
				cbRememberPassword.Selected = false;
			}
			else if (_saveType == LoginSaveType.Default)
			{
				bSaveDefault.SetBackgroundColor(selectedBGColor);
			}
			else if (_saveType == LoginSaveType.Player)
			{
				bSavePlayer.SetBackgroundColor(selectedBGColor);
			}
		}

		private void BSaveNone_onLeftClick(object sender, EventArgs e)
			=> SaveTypeToggle_onLeftClick(sender, e, LoginSaveType.None);

		private void BSaveDefault_onLeftClick(object sender, EventArgs e)
			=> SaveTypeToggle_onLeftClick(sender, e, LoginSaveType.Default);

		private void BSavePlayer_onLeftClick(object sender, EventArgs e)
			=> SaveTypeToggle_onLeftClick(sender, e, LoginSaveType.Player);

		private void SaveTypeToggle_onLeftClick(object sender, EventArgs e, LoginSaveType saveType)
			=> SetToggle(saveType);

		private void BRememberPassword_onLeftClick(object sender, EventArgs e)
		{
			if (saveType == LoginSaveType.None)
				SetToggle(LoginSaveType.Default);
		}


		private void bRegister_onLeftClick(object sender, EventArgs e)
		{
			if (tbUsername.Text.Length > 0 && tbPassword.Text.Length > 0)
			{
				tbUsername.Unfocus();
				tbPassword.Unfocus();
				SaveLogin();
				HEROsModNetwork.LoginService.RequestRegistration(tbUsername.Text, tbPassword.Text);
				Close();
			}
			else
			{
				Main.NewText(HEROsMod.HeroText("PleaseFillInUsernamePassword"));
			}
		}

		private void tbPassword_OnTabPress(object sender, EventArgs e)
		{
			tbPassword.Unfocus();
			tbUsername.Focus();
		}

		private void tbUsername_OnTabPress(object sender, EventArgs e)
		{
			tbUsername.Unfocus();
			tbPassword.Focus();
		}

		private void bLogin_onLeftClick(object sender, EventArgs e)
		{
			if (tbUsername.Text.Length > 0 && tbPassword.Text.Length > 0)
			{
				tbUsername.Unfocus();
				tbPassword.Unfocus();
				SaveLogin();
				HEROsModNetwork.LoginService.RequestLogin(tbUsername.Text, tbPassword.Text);
				Close();
			}
			else
			{
				Main.NewText(HEROsMod.HeroText("PleaseFillInUsernamePassword"));
			}
		}

		private string RetrieveDestinationServer()
		{
			if (Netplay.ServerIP != null && Netplay.ListenPort != 0)
			{
				return Netplay.ServerIP.ToString() + ":" + Netplay.ListenPort.ToString();
			}
			else if (Main.LobbyId > 0)
			{
				return Main.LobbyId.ToString();
			}
			else if (Main.worldName != null && Main.worldName != "")
			{
				return Main.worldName;
			}
			else
			{
				return "";
			}
		}

		private void SaveLogin()
		{
			var username = "";
			var password = "";

			username = tbUsername.Text;

			if (cbRememberPassword.Selected)
				password = tbPassword.Text;

			string destinationServer = RetrieveDestinationServer();

			if (saveType == LoginSaveType.Default)
			{
				loginStorage.AddLogin(destinationServer, username, password);
			}
			else if (saveType == LoginSaveType.Player)
			{
				loginStorage.AddLogin(destinationServer, Main.player[Main.myPlayer].name, username, password);
			}
			else if (saveType == LoginSaveType.None)
			{
				loginStorage.RemoveLogin(destinationServer, Main.player[Main.myPlayer].name);
			}

			loginStorage.SaveJSON();
		}

		private void bCancel_onLeftClick(object sender, EventArgs e)
		{
			this.Close();
		}

		protected override float GetWidth()
		{
			return tbPassword.Width + lPassword.Width + spacing * 4;
		}

		private void Close()
		{
			UIView.exclusiveControl = null;
			this.Parent.RemoveChild(this);
		}

		public override void Update()
		{
			if (Main.gameMenu) this.Close();
			if (Parent != null)
				this.Position = new Vector2(Parent.Width / 2, Parent.Height / 2);
			base.Update();
		}
	}
}