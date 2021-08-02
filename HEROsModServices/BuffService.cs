using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class BuffService : HEROsModService
	{
		private BuffWindow _buffWindow;

		//public static int[] Buffs = new int[] {
		//	112, 107, 1, 2, 3 , 4, 5, 6, 7, 8, 9, 10, 11, 12, 106, 13, 14,
		//	15, 16 ,17, 18, 19, 26, 27, 29, 93, 48, 63, 59, 58
		//};
		public static int[] SkipBuffs = new int[] {
			BuffID.Pygmies, BuffID.LeafCrystal, BuffID.IceBarrier, BuffID.BabySlime, BuffID.Ravens, BuffID.BeetleEndurance1, BuffID.BeetleEndurance2, BuffID.BeetleEndurance3,
			BuffID.BeetleMight1, BuffID.BeetleMight2, BuffID.BeetleMight3, BuffID.ImpMinion, BuffID.SpiderMinion, BuffID.TwinEyesMinion,
			BuffID.MinecartLeft, BuffID.MinecartLeftMech, BuffID.MinecartLeftWood, BuffID.MinecartRight, BuffID.MinecartRightMech, BuffID.MinecartRightWood,
			BuffID.SharknadoMinion, BuffID.UFOMinion, BuffID.DeadlySphere, BuffID.SolarShield1, BuffID.SolarShield2, BuffID.SolarShield3, BuffID.StardustDragonMinion,
			BuffID.StardustGuardianMinion, BuffID.HornetMinion, BuffID.PirateMinion, BuffID.StardustMinion };

		public BuffService()
		{
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/buffs", AssetRequestMode.ImmediateLoad));
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("OpenBuffWindow");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

			//_buffWindow = new BuffWindow();
			//this.AddUIView(_buffWindow);
			//_buffWindow.Visible = false;
		}

		//public override void PostSetupContent()
		//{
		//	if (!Main.dedServ)
		//	{
		//		_buffWindow = new BuffWindow();
		//		this.AddUIView(_buffWindow);
		//		_buffWindow.Visible = false;

		//		_buffWindow.Y = 270;
		//		_buffWindow.X = 130;
		//	}
		//}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("CanUseBuffs");
			if (!HasPermissionToUse && _buffWindow != null)
			{
				_buffWindow.Visible = false;
			}
			//base.MyGroupUpdated();
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (_buffWindow == null)
			{
				if (!Main.dedServ)
				{
					_buffWindow = new BuffWindow();
					this.AddUIView(_buffWindow);
					_buffWindow.Visible = false;

					_buffWindow.Y = 270;
					_buffWindow.X = 130;
				}
			}
			_buffWindow.Visible = !_buffWindow.Visible;
		}
	}

	internal class BuffWindow : UIWindow
	{
		private UITextbox tbSeconds;

		public BuffWindow()
		{
			this.CanMove = true;
			UILabel lTitle = new UILabel(HEROsMod.HeroText("Buffs"));
			UILabel lSeconds = new UILabel(HEROsMod.HeroText("Seconds"));
			tbSeconds = new UITextbox();
			UIScrollView scrollView = new UIScrollView();
			UIImage bClose = new UIImage(closeTexture);

			lTitle.Scale = .6f;
			lTitle.X = Spacing;
			lTitle.Y = Spacing;
			lTitle.OverridesMouse = false;

			bClose.Y = Spacing;
			bClose.onLeftClick += bClose_onLeftClick;

			tbSeconds.Text = "60";
			tbSeconds.Numeric = true;
			tbSeconds.MaxCharacters = 5;
			tbSeconds.Width = 75;
			tbSeconds.Y = lTitle.Y + lTitle.Height;

			scrollView.X = lTitle.X;
			scrollView.Y = tbSeconds.Y + tbSeconds.Height + Spacing;
			scrollView.Width = 300;
			scrollView.Height = 250;

			float yPos = Spacing;
			for (int i = 0; i < Main.debuff.Length/*BuffService.Buffs.Length*/; i++)
			{
				if ((Main.debuff[i] && i != BuffID.Wet) || Main.lightPet[i] || Main.vanityPet[i] || i == 0 || BuffService.SkipBuffs.Contains(i)) continue;
				//if (i >= BuffID.Count) ;
				int buffType = i;/*BuffService.Buffs[i];*/

				UIRect bg = new UIRect();
				bg.ForegroundColor = i % 2 == 0 ? Color.Transparent : Color.Blue * .1f;
				bg.X = Spacing;
				bg.Y = yPos;
				bg.Width = scrollView.Width - 20 - Spacing * 2;
				bg.Tag = buffType;
				string buffDescription = Lang.GetBuffDescription(buffType);
				bg.Tooltip = (buffDescription == null ? "" : buffDescription);
				bg.onLeftClick += bg_onLeftClick;

				UIImage buffImage = new UIImage(TextureAssets.Buff[buffType]);
				buffImage.X = Spacing;
				buffImage.Y = SmallSpacing / 2;
				buffImage.OverridesMouse = false;

				bg.Height = buffImage.Height + SmallSpacing;
				yPos += bg.Height;

				UILabel label = new UILabel(Lang.GetBuffName(buffType));
				label.Scale = .4f;
				label.Anchor = AnchorPosition.Left;
				label.X = buffImage.X + buffImage.Width + Spacing;
				label.Y = buffImage.Y + buffImage.Height / 2;
				label.OverridesMouse = false;

				bg.AddChild(buffImage);
				bg.AddChild(label);
				scrollView.AddChild(bg);
			}

			scrollView.ContentHeight = yPos;

			this.Width = scrollView.X + scrollView.Width + Spacing;
			this.Height = scrollView.Y + scrollView.Height + Spacing;

			tbSeconds.X = Width - tbSeconds.Width - Spacing;
			bClose.X = Width - bClose.Width - Spacing;

			lSeconds.Scale = .4f;
			lSeconds.Anchor = AnchorPosition.Right;
			lSeconds.X = tbSeconds.X - Spacing;
			lSeconds.Y = tbSeconds.Y + tbSeconds.Height / 2;

			AddChild(lTitle);
			AddChild(lSeconds);
			AddChild(tbSeconds);
			AddChild(scrollView);
			AddChild(bClose);
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}

		private void bg_onLeftClick(object sender, EventArgs e)
		{
			UIView view = (UIView)sender;
			int buffType = (int)view.Tag;

			if (tbSeconds.Text.Length == 0)
			{
				tbSeconds.Text = "60";
			}
			int seconds = int.Parse(tbSeconds.Text);

			Main.player[Main.myPlayer].AddBuff(buffType, seconds * 60);
		}
	}
}