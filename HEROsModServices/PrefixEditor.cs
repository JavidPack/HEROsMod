using HEROsMod.UIKit;
using HEROsMod.UIKit.UIComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class PrefixEditor : HEROsModService
	{
		private PrefixWindow _prefixWindow;

		public PrefixEditor()
		{
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/reforge")/*Main.itemTexture[24]*/);
			this._hotbarIcon.onLeftClick += _hotbarIcon_onLeftClick;
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("PrefixEditor");

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
			if (!HasPermissionToUse)
			{
				_prefixWindow.bClose_onLeftClick(null, null);
			}
		}

		internal void PreSaveAndQuit()
		{
			_prefixWindow.bClose_onLeftClick(null, null);
		}
	}

	internal class PrefixWindow : UIWindow
	{
		internal Slot itemSlot;
		private UIScrollView prefixList;
		private List<Particle> particles;
		private float _particleTimer;
		private float _newParticleTime = .1f;
		private List<Item> validPrefixes;

		private static Dictionary<int, Color> rarityColors = new Dictionary<int, Color>()
		{
			{-11, Terraria.ID.Colors.RarityAmber },
			{-1, Terraria.ID.Colors.RarityTrash },
			{0, Color.White },
			{1, Terraria.ID.Colors.RarityBlue },
			{2, Terraria.ID.Colors.RarityGreen },
			{3, Terraria.ID.Colors.RarityOrange },
			{4, Terraria.ID.Colors.RarityRed },
			{5, Terraria.ID.Colors.RarityPink },
			{6, Terraria.ID.Colors.RarityPurple },
			{7, Terraria.ID.Colors.RarityLime },
			{8, Terraria.ID.Colors.RarityYellow },
			{9, Terraria.ID.Colors.RarityCyan },
			{10, new Color(255, 40, 100) },
			{11, new Color(180, 40, 255) },
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
			bClsoe.onLeftClick += bClose_onLeftClick;

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

		internal void bClose_onLeftClick(object sender, EventArgs e)
		{
			this.Visible = false;
			ClearOutItemSlots();
		}

		internal void ClearOutItemSlots()
		{
			if (!itemSlot.item.IsAir)
			{
				Item item = itemSlot.item.Clone();

				Player player = Main.LocalPlayer;
				item.position = player.Center;
				Item item2 = player.GetItem(player.whoAmI, item, GetItemSettings.GetItemInDropItemCheck);
				if (item2.stack > 0)
				{
					int num = Item.NewItem(player.GetItemSource_Misc(ItemSourceID.PlayerDropItemCheck), (int)player.position.X, (int)player.position.Y, player.width, player.height, item2.type, item2.stack, false, (int)item.prefix, true, false);
					Main.item[num].newAndShiny = false;
					if (Main.netMode == 1)
					{
						NetMessage.SendData(21, -1, -1, null, num, 1f, 0f, 0f, 0, 0, 0);
					}
					else
					{
						HEROsMod.instance.Logger.Warn("HerosMod: You left an item in the prefix editor with a full inventory and have lost the item: " + item2.Name);
					}

				}
				itemSlot.item.TurnToAir();
				itemSlot_ItemChanged(null, null);
			}
		}

		internal void itemSlot_ItemChanged(object sender, EventArgs e)
		{
			GetValidPrefixesForItem();
			PopulatePrefixDropDown();
		}

		private void GetValidPrefixesForItem()
		{
			validPrefixes = new List<Item>();
			if (itemSlot.item.IsAir)
				return;
			Item item = itemSlot.item.Clone();

			var validPrefixValues = new HashSet<int>();
			int remainingAttempts = 100;
			while (remainingAttempts > 0)
			{
				item.SetDefaults(item.type);
				item.Prefix(-2);
				remainingAttempts--;
				if (item.prefix != 0 && validPrefixValues.Add(item.prefix))
				{
					remainingAttempts = 100;
					validPrefixes.Add(item.Clone());
				}
			}

			validPrefixes = validPrefixes.OrderBy(x => -x.value-x.rare).ToList();
		}

		private void PopulatePrefixDropDown()
		{
			prefixList.ClearContent();
			float yPos = Spacing;
			foreach (Item item in validPrefixes)
			{
				UILabel label = new UILabel(Lang.prefix[item.prefix].Value);
				if (item.prefix == 0)
					label.Text = "No Prefix";
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
				if(item.rare > 11)
					label.ForegroundColor = rarityColors[11];
				prefixList.AddChild(label);
			}
			prefixList.ContentHeight = yPos + Spacing;
		}

		private void label_onHover(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			Item item = (Item)label.Tag;
			HoverText = item.Name;
			//HoverItem = item.Clone();
		}

		private void label_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			Item item = (Item)label.Tag;
			Item reforgeItem = new Item();
			reforgeItem.netDefaults(itemSlot.item.netID);
			reforgeItem = reforgeItem.CloneWithModdedDataFrom(itemSlot.item);
			reforgeItem.Prefix(item.prefix);
			itemSlot.item = reforgeItem.Clone();
			SoundEngine.PlaySound(SoundID.Item37);
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