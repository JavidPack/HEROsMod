using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace HEROsMod.HEROsModServices
{
	internal class PrefixEditor : HEROsModService
	{
		private PrefixWindow _prefixWindow;

		public PrefixEditor()
		{
			this._hotbarIcon = new UIImage(HEROsMod.instance.GetTexture("Images/reforge")/*Main.itemTexture[24]*/);
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = "Prefix Editor";

			_prefixWindow = new PrefixWindow();
			_prefixWindow.Y = 270;
			_prefixWindow.X = 130;
			this.AddUIView(_prefixWindow);
			_prefixWindow.Visible = false;
		}

		private void _hotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			_prefixWindow.Visible = !_prefixWindow.Visible;
			if (_prefixWindow.Visible)
			{
				Main.playerInventory = true;
			}
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("PrefixEditor");
			//base.MyGroupUpdated();
		}
	}

	internal class PrefixWindow : UIWindow
	{
		private Slot itemSlot;
		private UIScrollView prefixList;
		private int[] prefixes;
		private List<Particle> particles;
		private float _particleTimer;
		private float _newParticleTime = .1f;
		private List<Item> validPrefixes;

		private static Dictionary<int, Color> rarityColors = new Dictionary<int, Color>()
		{
			{-11, new Color(255, 175, 0) },
			{-1, new Color(130, 130, 130) },
			{1, new Color(150, 150, 255) },
			{2, new Color(150, 255, 150) },
			{3, new Color(255, 200, 150) },
			{4, new Color(255, 150, 150) },
			{5, new Color(255, 150, 255) },
			{6, new Color(210, 160, 255) },
			{7, new Color(150, 255, 10) },
			{8, new Color(255, 255, 10) },
			{9, new Color(5, 200, 255) },
		};

		public PrefixWindow()
		{
			this.CanMove = true;

			itemSlot = new Slot(0);
			itemSlot.functionalSlot = true;
			itemSlot.X = LargeSpacing;
			itemSlot.Y = LargeSpacing;
			itemSlot.ItemChanged += itemSlot_ItemChanged;

			prefixList = new UIScrollView();
			prefixList.X = itemSlot.X;
			prefixList.Y = itemSlot.Y + itemSlot.Height + Spacing;
			prefixList.Width = 250;
			prefixList.Height = 200;

			this.Width = prefixList.Width + LargeSpacing * 2;
			this.Height = prefixList.Y + prefixList.Height + LargeSpacing;

			UIImage bClsoe = new UIImage(closeTexture);
			bClsoe.X = Width - bClsoe.Width - LargeSpacing;
			bClsoe.Y = LargeSpacing;
			bClsoe.onLeftClick += bClsoe_onLeftClick;

			AddChild(itemSlot);
			AddChild(prefixList);
			AddChild(bClsoe);

			particles = new List<Particle>();
			_particleTimer = _newParticleTime;
		}

		public override void Update()
		{
			_particleTimer -= ModUtils.DeltaTime;
			if (_particleTimer <= 0)
			{
			}
			for (int i = 0; i < particles.Count; i++)
			{
				particles[i].Update();
				if (particles[i].Dead)
				{
					particles.RemoveAt(i);
					i--;
				}
			}
			base.Update();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			foreach (Particle particle in particles)
			{
				particle.Draw(spriteBatch, this.DrawPosition);
			}
		}

		private void bClsoe_onLeftClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}

		private void itemSlot_ItemChanged(object sender, EventArgs e)
		{
			prefixes = new int[83];
			for (int i = 0; i < 83; i++)
			{
				prefixes[i] = i + 1;
			}
			//prefixes = PrefixScraper.GetPrefixesForItem(itemSlot.item);
			GetValidPrefixesForItem();
			PopulatePrefixDropDown();
		}

		private void GetValidPrefixesForItem()
		{
			validPrefixes = new List<Item>();
			Item backUpItem = itemSlot.item.Clone();
			Item item = itemSlot.item;
			for (int i = 0; i < prefixes.Length; i++)
			{
				if (PrefixScraper.IsValidPrefix(prefixes[i], item))
				{
					item.SetDefaults(item.type);
					item.Prefix(prefixes[i]);
					if (item.prefix == prefixes[i])
					{
						validPrefixes.Add(item.Clone());
					}
				}
			}
			itemSlot.item = backUpItem.Clone();
			validPrefixes = validPrefixes.OrderBy(x => -x.rare).ToList();
		}

		private void PopulatePrefixDropDown()
		{
			prefixList.ClearContent();
			float yPos = Spacing;
			foreach (Item item in validPrefixes)
			{
				UILabel label = new UILabel(Lang.prefix[item.prefix].Value);
				label.Scale = .4f;
				label.X = Spacing;
				label.Y = yPos;
				label.Tag = item;
				label.onLeftClick += label_onLeftClick;
				label.onHover += label_onHover;
				yPos += label.Height;
				if (rarityColors.ContainsKey(item.rare))
				{
					label.ForegroundColor = rarityColors[item.rare];
				}
				prefixList.AddChild(label);
			}
			prefixList.ContentHeight = yPos + Spacing;
		}

		private void label_onHover(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			Item item = (Item)label.Tag;
			HoverText = item.Name;
			HoverItem = item.Clone();
		}

		private void label_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			Item item = (Item)label.Tag;
			itemSlot.item = item.Clone();
			//ModUtils.SetPrefix(itemSlot.item, prefixNum);
			//itemSlot.itemPrefix2(prefixNum);
			//Console.WriteLine(item.prefix);
			//itemSlot.item.Prefix(0);

			/*
            for (int i = 0; i < 20; i++)
            {
                float xVel = (float)(Main.rand.Next(400) - 200) / 100;
                float yVel = (float)(Main.rand.Next(100, 300) - 100) / 100;
                _particleTimer = _newParticleTime;
                Vector2 pos = itemSlot.Position + new Vector2(itemSlot.Width / 2, itemSlot.Height / 2);
                particles.Add(new Particle(Main.dustTexture, pos, new Vector2(xVel, yVel), 1f, new Rectangle(70, 50, 8, 8)));
            }
             */
		}

		private void bReforge_onLeftClick(object sender, EventArgs e)
		{
			itemSlot.item.Prefix(-2);
		}
	}

	internal class PrefixEntry
	{
		public string Name { get; set; }
		public int Type { get; set; }
		public Item Item { get; set; }

		public PrefixEntry()
		{
		}
	}

	internal class Particle
	{
		public Texture2D Texture { get; set; }
		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }
		public float Scale { get; set; }
		public float FallSpeed { get; set; }
		public bool Dead { get; set; }
		public float Rotation { get; set; }
		public Rectangle? Source { get; set; }
		public float LifeTimer { get; set; }

		public Particle(Texture2D texture, Vector2 position, Vector2 initialVelocity = new Vector2(), float scale = 1f, Rectangle? source = null, float lifeTimer = .2f)
		{
			this.Texture = texture;
			this.Position = position;
			this.Velocity = initialVelocity;
			this.Scale = scale;
			this.Source = source;
			this.LifeTimer = .35f;
			this.FallSpeed = .1f;
			Dead = false;
		}

		public void Update()
		{
			LifeTimer -= ModUtils.DeltaTime;
			if (LifeTimer <= 0)
			{
				this.Dead = true;
			}
			float xDecay = 1f;
			float xVel = Velocity.X;
			if (xVel > 0)
			{
				xVel -= xDecay * ModUtils.DeltaTime * 60;
				if (xVel < 0)
				{
					xVel = 0;
				}
			}
			else if (xVel < 0)
			{
				xVel += xDecay * ModUtils.DeltaTime * 60;
				if (xVel > 0)
				{
					xVel = 0;
				}
			}
			Velocity -= new Vector2(0, FallSpeed) * ModUtils.DeltaTime * 60;
			Position -= Velocity;
			Rotation += MathHelper.ToRadians(Velocity.X);
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
		{
			Vector2 origin = new Vector2((float)Texture.Width / 2, (float)Texture.Height / 2);
			if (Source != null)
			{
				origin = new Vector2((float)((Rectangle)Source).Width / 2, (float)((Rectangle)Source).Height / 2);
			}
			spriteBatch.Draw(Texture, Position + drawPosition, Source, Color.White, Rotation, origin, Scale, SpriteEffects.None, 0);
		}
	}
}