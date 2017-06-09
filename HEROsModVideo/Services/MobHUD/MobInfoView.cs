using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace HEROsMod.HEROsModVideo.Services.MobHUD
{
	internal class MobInfoView : UIWindow
	{
		public NPC NPCTarget { get; set; }
		private Vector2 _targetPosition = Vector2.Zero;
		private float _radius = 10f;
		private float _time = 0f;
		private float _timeAdvanceSpeed = MathHelper.Pi / 20;

		private Vector2 _worldPos;
		private Vector2 _velocity;
		private float yPos = Spacing;
		private float maxWidth = 0f;
		private float elapsedTime = 0f;

		private List<UIView> stats;

		public MobInfoView(NPC npc)
		{
			stats = new List<UIView>();
			this.OverridesMouse = false;
			NPCTarget = npc;
			this.Anchor = AnchorPosition.Top;

			this._worldPos = npc.position;

			AddLabel(Lang.GetNPCNameValue(npc.netID));
			AddStat("ID", npc.netID.ToString());
			AddStat("Health", npc.lifeMax.ToString());
			AddStat("Damage", npc.damage.ToString());
			AddStat("Defense", npc.defense.ToString());
			AddStat("Knockback Resist", npc.knockBackResist.ToString());

			this.Width = maxWidth + Spacing * 2;
			this.Height = yPos;
			this._worldPos.Y -= this.Height;

			for (int i = 0; i < stats.Count; i++)
			{
				stats[i].X = this.Width - stats[i].Width - Spacing;
			}
		}

		public override void Update()
		{
			_timeAdvanceSpeed = MathHelper.Pi / 80;
			elapsedTime += ModUtils.DeltaTime;

			Vector2 targetPos = NPCTarget.position + _targetPosition;
			targetPos.X += NPCTarget.width / 2;
			targetPos.Y -= this.Height + 15 + _radius;
			_time += _timeAdvanceSpeed * ModUtils.DeltaTime * 60;
			if (_time > MathHelper.TwoPi)
			{
				_time -= MathHelper.TwoPi;
			}
			_targetPosition.X = (float)Math.Cos(_time) * _radius * .66f;
			_targetPosition.Y = (float)Math.Sin(_time) * _radius;

			SetPos(ModUtils.DeltaTime, targetPos);
			this.Position = _worldPos - Main.screenPosition;
			base.Update();
		}

		private void AddLabel(string text, Color? color = null, float scale = .4f)
		{
			UILabel label = new UILabel(text);
			label.Scale = scale;
			label.X = Spacing;
			label.Y = yPos;
			yPos += label.Height;
			if (label.Width > maxWidth)
			{
				maxWidth = label.Width;
			}
			label.ForegroundColor = Color.White;
			if (color != null)
			{
				Color c = (Color)color;
				label.ForegroundColor = c;
			}
			AddChild(label);
		}

		private float Mass = 10;
		private float SpringStiffness = 0f;
		private float Damping = 0f;

		public void SetPos(float elapsedSeconds, Vector2 desiredPosition)
		{
			Damping = 3.9f;
			SpringStiffness = 30;
			Mass = 0.5f;
			var delta = this._worldPos - desiredPosition;
			var force = -SpringStiffness * delta - Damping * _velocity;
			var acceleration = force / Mass;
			_velocity += acceleration * elapsedSeconds;
			this._worldPos += _velocity * elapsedSeconds;
		}

		private void AddStat(string text, string stat, Color? color = null, float scale = .4f)
		{
			UILabel label = new UILabel(text);
			label.Scale = scale;
			label.X = Spacing;
			label.Y = yPos;
			label.ForegroundColor = Color.White;
			AddChild(label);

			UILabel lStat = new UILabel(stat);
			lStat.Scale = scale;
			lStat.X = Width;
			lStat.Y = yPos;
			lStat.ForegroundColor = Color.White;
			if (color != null)
			{
				Color c = (Color)color;
				label.ForegroundColor = c;
				lStat.ForegroundColor = c;
			}
			AddChild(lStat);
			stats.Add(lStat);

			yPos += label.Height;

			if (label.Width + 20 + lStat.Width > maxWidth)
			{
				maxWidth = label.Width + 20 + lStat.Width;
			}
		}
	}
}