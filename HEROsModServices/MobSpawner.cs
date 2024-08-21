using HEROsMod.UIKit;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;

//using HEROsMod.HEROsModVideo.Services.DropRateInfo;

using Terraria;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class MobSpawner : HEROsModService
	{
		public const int NumberOfNegativeNPCs = 65;
		private MobSpawnWindow _spawnWindow;

		public MobSpawner()
		{
			this._hotbarIcon = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/npcs", AssetRequestMode.ImmediateLoad)/*Main.itemTexture[666]*/);
			this.HotbarIcon.Tooltip = HEROsMod.HeroText("OpenMobSpawnWindow");
			this.HotbarIcon.onLeftClick += HotbarIcon_onLeftClick;

			_spawnWindow = new MobSpawnWindow();
			_spawnWindow.CenterToParent();
			_spawnWindow.Position -= new Vector2(_spawnWindow.Width / 2, _spawnWindow.Height / 2);
			_spawnWindow.Visible = false;
			this.AddUIView(_spawnWindow);
		}

		private void HotbarIcon_onLeftClick(object sender, EventArgs e)
		{
			if (!_spawnWindow.Loaded)
			{
				ModUtils.DebugText("HotbarIcon_onLeftClick calling GetNPCList and BuildList");
				_spawnWindow.GetNPCList();
				_spawnWindow.BuildList();
			}
			_spawnWindow.Visible = !_spawnWindow.Visible;
		}

		public override void MyGroupUpdated()
		{
			this.HasPermissionToUse = HEROsModNetwork.LoginService.MyGroup.HasPermission("SpawnNPCs");
			if (!HasPermissionToUse)
			{
				_spawnWindow.Visible = false;
			}

			//base.MyGroupUpdated();
		}

		public override void Destroy()
		{
			_spawnWindow.Close();
			base.Destroy();
		}

		public override void Unload()
		{
			ModUtils.DebugText("MobSpawner Unload False");

			_spawnWindow.Loaded = false;
		}
	}

	internal class MobSpawnWindow : UIWindow
	{
		private List<NPCStats> npcList;
		private List<NPCStats> category;
		private List<NPCStats> searchResults;
		private MobInfoPanel mobInfo;
		private UITextbox searchBox;
		private UIScrollView scrollView;
		private UIImage bClose;
		private UIButton bAllNPCs;
		private UIButton bTownNPCs;
		private UIButton bCritters;
		private UIButton bFriendly;
		private UIButton bBoss;
		private UIButton bMod;
		
		//Slot itemSlot;

		public bool Loaded = false;
		//private bool _mobInfoAddToParent = false;

		public MobSpawnWindow()
		{
			//GetNPCList();

			this.CanMove = true;
			UILabel lTiltle = new UILabel(HEROsMod.HeroText("NPCSpawner"));
			lTiltle.OverridesMouse = false;
			scrollView = new UIScrollView();
			searchBox = new UITextbox();
			searchBox.KeyPressed += searchBox_KeyPressed;
			mobInfo = new MobInfoPanel();
			mobInfo.Visible = false;

			AddChild(mobInfo);

			lTiltle.X = Spacing;
			lTiltle.Y = Spacing;
			lTiltle.Scale = .6f;
			lTiltle.OverridesMouse = false;

			searchBox.Y = lTiltle.Y + lTiltle.Height;

			scrollView.X = lTiltle.X;
			scrollView.Y = searchBox.Y + searchBox.Height + Spacing;
			scrollView.Width = 250;
			scrollView.Height = 300;

			//category = npcList;
			//searchResults = category;
			// BuildList();

			bAllNPCs = new UIButton(HEROsMod.HeroText("All"));
			bAllNPCs.X = scrollView.X + scrollView.Width + Spacing;
			bAllNPCs.Y = scrollView.Y;
			bAllNPCs.onLeftClick += bAllNPCs_onLeftClick;
			bAllNPCs.SetTextColor(Color.Yellow);

			bTownNPCs = new UIButton(HEROsMod.HeroText("TownNPCs"));
			bTownNPCs.X = bAllNPCs.X;
			bTownNPCs.Y = bAllNPCs.Y + bAllNPCs.Height + Spacing;
			bTownNPCs.onLeftClick += bTownNPCs_onLeftClick;

			bCritters = new UIButton(HEROsMod.HeroText("Critters"));
			bCritters.X = bTownNPCs.X;
			bCritters.Y = bTownNPCs.Y + bTownNPCs.Height + Spacing;
			bCritters.onLeftClick += bCritters_onLeftClick;

			bFriendly = new UIButton(HEROsMod.HeroText("Friendly"));
			bFriendly.X = bCritters.X;
			bFriendly.Y = bCritters.Y + bCritters.Height + Spacing;
			bFriendly.onLeftClick += bFriendly_onLeftClick;

			bBoss = new UIButton(HEROsMod.HeroText("Boss"));
			bBoss.X = bFriendly.X;
			bBoss.Y = bFriendly.Y + bFriendly.Height + Spacing;
			bBoss.onLeftClick += bBoss_onLeftClick;

			bMod = new UIButton(HEROsMod.HeroText("Mod"));
			bMod.X = bBoss.X;
			bMod.Y = bBoss.Y + bBoss.Height + Spacing;
			bMod.onLeftClick += (a, b) => bMod_onLeftClick(a, b, true);
			bMod.onRightClick += (a, b) => bMod_onLeftClick(a, b, false);
			bMod.Tooltip = "";

			bAllNPCs.AutoSize = false;
			bTownNPCs.AutoSize = false;
			bCritters.AutoSize = false;
			bFriendly.AutoSize = false;
			bBoss.AutoSize = false;
			bMod.AutoSize = false;
			bAllNPCs.Width = 100;
			bTownNPCs.Width = bAllNPCs.Width;
			bCritters.Width = bTownNPCs.Width;
			bFriendly.Width = bAllNPCs.Width;
			bBoss.Width = bAllNPCs.Width;
			bMod.Width = bAllNPCs.Width;

			//itemSlot = new Slot(0);
			//itemSlot.functionalSlot = true;
			//itemSlot.Y = bBoss.Y + bBoss.Height + Spacing;
			//itemSlot.X = bBoss.X + bBoss.Width / 2 - itemSlot.Width / 2;
			//itemSlot.ItemChanged += itemSlot_ItemChanged;

			AddChild(lTiltle);
			AddChild(scrollView);
			AddChild(searchBox);
			AddChild(bAllNPCs);
			AddChild(bTownNPCs);
			AddChild(bCritters);
			AddChild(bFriendly);
			AddChild(bBoss);
			AddChild(bMod);
			//AddChild(itemSlot);

			this.Width = bTownNPCs.X + bTownNPCs.Width + Spacing;
			this.Height = scrollView.Y + scrollView.Height + Spacing;
			searchBox.Width = Width - Spacing * 2;
			searchBox.X = Spacing;

			bClose = new UIImage(closeTexture);
			bClose.X = Width - bClose.Width - Spacing;
			bClose.Y = Spacing;
			AddChild(bClose);
			bClose.onLeftClick += bClose_onLeftClick;
		}

		//void itemSlot_ItemChanged(object sender, EventArgs e)
		//{
		//	if (itemSlot.item.type == 0)
		//	{
		//		category = npcList;
		//	}
		//	else
		//	{
		//		List<int> npcIDs = new List<int>();
		//		List<NPCDropTable> dropTables = DropTableBuilder.DropTable.NPCDropTables;
		//		for (int i = 0; i < dropTables.Count; i++)
		//		{
		//			NPCDropTable dropTable = dropTables[i];
		//			for (int j = 0; j < dropTable.Drops.Count; j++)
		//			{
		//				ItemDropInfo itemDrop = dropTable.Drops[j];
		//				if (itemDrop.NPCType == itemSlot.item.netID)
		//				{
		//					npcIDs.Add(dropTable.NPCType);
		//				}
		//			}
		//		}

		//		category = new List<NPCStats>();
		//		for (int i = 0; i < npcIDs.Count; i++)
		//		{
		//			for (int j = 0; j < npcList.Count; j++)
		//			{
		//				NPCStats npcStat = npcList[j];
		//				if (npcStat.NetID == npcIDs[i])
		//				{
		//					category.Add(npcStat);
		//					break;
		//				}
		//			}
		//		}

		//	}
		//	searchResults = category;
		//	BuildList();
		//	searchBox.Text = string.Empty;
		//	WhiteAllButtons();
		//}

		// increment
		public int lastModNameNumber = 0;

		private void bMod_onLeftClick(object sender, EventArgs e, bool left)
		{
			var mods = ModLoader.Mods.Select(x => x.Name).ToList();
			mods = mods.Intersect(npcList.Select(npc => npc.Mod?.Name)).ToList();
			mods.Sort();
			if (mods.Count == 0)
			{
				Main.NewText("No NPC have been added by mods.");
				return;
			}
			if (bMod.Tooltip == "")
			{
			}
			else
			{
				lastModNameNumber = left ? (lastModNameNumber + 1) % mods.Count : (mods.Count + lastModNameNumber - 1) % mods.Count;
			}
			string currentMod = mods[lastModNameNumber];

			category = new List<NPCStats>();
			foreach (var npc in npcList)
			{
				if (/*npc.Boss && */npc.Mod != null && npc.Mod.Name == currentMod)
				{
					category.Add(npc);
				}
			}
			searchResults = category;
			BuildList();
			searchBox.Text = string.Empty;

			UIButton button = (UIButton)sender;
			WhiteAllButtons();
			bMod.Tooltip = currentMod;
			button.SetTextColor(Color.Yellow);
		}

		private void bBoss_onLeftClick(object sender, EventArgs e)
		{
			category = new List<NPCStats>();
			foreach (var npc in npcList)
			{
				if (npc.Boss)
				{
					category.Add(npc);
				}
			}
			// TODO: sort by BossChecklist order?
			searchResults = category;
			BuildList();
			searchBox.Text = string.Empty;

			UIButton button = (UIButton)sender;
			WhiteAllButtons();
			button.SetTextColor(Color.Yellow);
		}

		private void bFriendly_onLeftClick(object sender, EventArgs e)
		{
			category = new List<NPCStats>();
			foreach (var npc in npcList)
			{
				if (npc.Friendly)
				{
					category.Add(npc);
				}
			}
			searchResults = category;
			BuildList();
			searchBox.Text = string.Empty;

			UIButton button = (UIButton)sender;
			WhiteAllButtons();
			button.SetTextColor(Color.Yellow);
		}

		private void bTownNPCs_onLeftClick(object sender, EventArgs e)
		{
			category = new List<NPCStats>();
			foreach (var npc in npcList)
			{
				if (npc.IsTownNPC)
				{
					category.Add(npc);
				}
			}
			searchResults = category;
			BuildList();
			searchBox.Text = string.Empty;

			UIButton button = (UIButton)sender;
			WhiteAllButtons();
			button.SetTextColor(Color.Yellow);
		}
		private void bCritters_onLeftClick(object sender, EventArgs e)
		{
			category = new List<NPCStats>();
			foreach (var npc in npcList)
			{
				if (npc.IsCritter)
				{
					category.Add(npc);
				}
			}
			searchResults = category;
			BuildList();
			searchBox.Text = string.Empty;

			UIButton button = (UIButton)sender;
			WhiteAllButtons();
			button.SetTextColor(Color.Yellow);
		}

		private void bAllNPCs_onLeftClick(object sender, EventArgs e)
		{
			category = npcList;
			searchResults = category;
			BuildList();
			searchBox.Text = string.Empty;

			UIButton button = (UIButton)sender;
			WhiteAllButtons();
			button.SetTextColor(Color.Yellow);
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}

		internal void BuildList()
		{
			scrollView.ClearContent();
			float yPos = Spacing;
			for (int i = 0; i < searchResults.Count; i++)
			{
				UILabel label = new UILabel(searchResults[i].Name);
				label.Tag = i;
				label.Scale = .35f;
				label.X = Spacing;
				label.Y = yPos;
				yPos += label.Height;
				label.onLeftClick += label_onLeftClick;
				label.onMouseEnter += label_onMouseEnter;
				label.onMouseLeave += label_onMouseLeave;
				scrollView.AddChild(label);
			}
			scrollView.ContentHeight = yPos + Spacing;
		}

		private void searchBox_KeyPressed(object sender, char key)
		{
			if (category != npcList)
			{
				category = npcList;
			}
			UITextbox textbox = (UITextbox)sender;
			if (textbox.Text.Length > 0)
			{
				List<NPCStats> matches = new List<NPCStats>();
				foreach (var npc in category)
				{
					if (npc.Name.ToLower().IndexOf(textbox.Text.ToLower(), System.StringComparison.Ordinal) != -1)
					{
						matches.Add(npc);
					}
				}
				if (matches.Count > 0)
				{
					searchResults = matches;
					BuildList();
				}
				else
				{
					textbox.Text = textbox.Text.Substring(0, textbox.Text.Length - 1);
				}
			}
			else
			{
				searchResults = category;
				BuildList();
			}
		}

		private void label_onMouseLeave(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			int npcType = (int)label.Tag;

			if (mobInfo.CurrentNPC == searchResults[npcType])
			{
				mobInfo.Visible = false;
			}
		}

		private void label_onMouseEnter(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			int npcType = (int)label.Tag;

			mobInfo.SetMobType(searchResults[npcType]);
			mobInfo.Visible = true;
		}

		private void WhiteAllButtons()
		{
			bAllNPCs.SetTextColor(Color.White);
			bTownNPCs.SetTextColor(Color.White);
			bCritters.SetTextColor(Color.White);
			bFriendly.SetTextColor(Color.White);
			bBoss.SetTextColor(Color.White);
			bMod.SetTextColor(Color.White);
			bMod.Tooltip = "";
		}

		public void Close()
		{
			mobInfo.Close();
		}

		private void label_onLeftClick(object sender, EventArgs e)
		{
			UILabel label = (UILabel)sender;
			int npcType = (int)label.Tag;

			if (ModUtils.NetworkMode == NetworkMode.None)
			{
				searchResults[npcType].Spawn(Main.myPlayer);
				//NPC.NewNPC((int)Main.player[Main.myPlayer].position.X, (int)Main.player[Main.myPlayer].position.Y, npcType);
			}
			else
			{
				HEROsModNetwork.GeneralMessages.RequestSpawnNPC(searchResults[npcType].NetID);
			}
		}

		//public override void Update()
		//{
		//	if (!_mobInfoAddToParent)
		//	{
		//		Parent.AddChild(mobInfo);
		//		_mobInfoAddToParent = true;
		//	}
		//	base.Update();
		//}

		private void RemoveNPCTypeFromList(int type)
		{
			for (int i = 0; i < npcList.Count; i++)
			{
				if (npcList[i].NetID == type)
				{
					npcList.RemoveAt(i);
					return;
				}
			}
		}

		private void RenameNPCFromList(string name, int type)
		{
			for (int i = 0; i < npcList.Count; i++)
			{
				if (npcList[i].NetID == type)
				{
					npcList[i].Name = name;
					return;
				}
			}
		}

		internal void GetNPCList()
		{
			npcList = new List<NPCStats>();
			NPC npc;
			npc = new NPC();
			for (int i = 0; i < TextureAssets.Npc.Length; i++)
			{
				npc.SetDefaults(i);
				//if (npc.name != string.Empty)
				{
					npcList.Add(new NPCStats(npc));
				}
			}
			for (int i = -1; i >= -MobSpawner.NumberOfNegativeNPCs; i--)
			{
				npc.SetDefaults(i);
				//if (npc.name != string.Empty)
				{
					npcList.Add(new NPCStats(npc));
				}
			}
			ModUtils.DebugText("GetNPCList Loaded " + npcList.Count + " npc. npcTexture: " + TextureAssets.Npc.Length);

			//Golem
			RemoveNPCTypeFromList(246);
			RemoveNPCTypeFromList(247);
			RemoveNPCTypeFromList(248);
			RemoveNPCTypeFromList(249);
			RenameNPCFromList("Eater of Worlds", 13);
			RemoveNPCTypeFromList(14);
			RemoveNPCTypeFromList(15);
			RenameNPCFromList("Bone Serpent", 39);
			RemoveNPCTypeFromList(40);
			RemoveNPCTypeFromList(41);
			RenameNPCFromList("Devourer", 7);
			RemoveNPCTypeFromList(8);
			RemoveNPCTypeFromList(9);
			RenameNPCFromList("Giant Worm", 10);
			RemoveNPCTypeFromList(11);
			RemoveNPCTypeFromList(12);
			RenameNPCFromList("Digger", 95);
			RemoveNPCTypeFromList(96);
			RemoveNPCTypeFromList(97);
			RenameNPCFromList("World Feeder", 98);
			RemoveNPCTypeFromList(99);
			RemoveNPCTypeFromList(100);
			RenameNPCFromList("Leech", 117);
			RemoveNPCTypeFromList(118);
			RemoveNPCTypeFromList(119);
			RenameNPCFromList("Wyvern", 87);
			RemoveNPCTypeFromList(88);
			RemoveNPCTypeFromList(89);
			RemoveNPCTypeFromList(90);
			RemoveNPCTypeFromList(91);
			RemoveNPCTypeFromList(92);
			RenameNPCFromList("Skeletron", 35);
			RemoveNPCTypeFromList(36);
			//prime
			RemoveNPCTypeFromList(128);
			RemoveNPCTypeFromList(129);
			RemoveNPCTypeFromList(130);
			RemoveNPCTypeFromList(131);
			//plantera
			RemoveNPCTypeFromList(263);
			RemoveNPCTypeFromList(264);
			//destoryer
			RemoveNPCTypeFromList(135);
			RemoveNPCTypeFromList(136);
			//pumpking
			RemoveNPCTypeFromList(328);
			//WoF
			RemoveNPCTypeFromList(113);
			RemoveNPCTypeFromList(114);

			NPC wof = new NPC();
			wof.SetDefaults(113);
			npcList.Add(new WallOfFlesh(wof));

			//npcList.Add(new EaterOfWorlds());
			npc = null;
			npcList = npcList.OrderBy(n => n.Name).ToList();

			category = npcList;
			searchResults = category;

			Loaded = true;
		}
	}

	internal class MobInfoPanel : UIWindow
	{
		private UIImage mobImage;
		public NPCStats CurrentNPC { get; set; }

		public MobInfoPanel()
		{
			CurrentNPC = null;
			this.OverridesMouse = false;
			this.UpdateWhenOutOfBounds = true;
			mobImage = new UIImage();
			mobImage.X = Spacing;
			mobImage.Y = Spacing;

			AddChild(mobImage);
		}

		public void SetMobType(NPCStats npc)
		{
			this.RemoveAllChildren();
			float yPos = Spacing;

			this.Width = 350;

			//if (npc.NetID < 0) return;
			CurrentNPC = npc;
			ModUtils.LoadNPC(npc.Type, immediate: true);
			mobImage.Texture = TextureAssets.Npc[npc.Type];
			mobImage.SourceRectangle = new Rectangle(0, 0, (int)mobImage.Texture.Value.Width, (int)mobImage.Texture.Value.Height / Main.npcFrameCount[npc.Type]);
			//mobImage.ForegroundColor = CurrentNPC.AlphaColor;
			AddChild(mobImage);

			UIImage mobImage2 = new UIImage();
			mobImage2.Texture = mobImage.Texture;
			mobImage2.SourceRectangle = mobImage.SourceRectangle;
			mobImage2.ForegroundColor = CurrentNPC.Color;
			AddChild(mobImage2);
			//AddChild(mobImage2);

			if (mobImage.Width > this.Width - Spacing * 2)
			{
				this.Width = mobImage.Width + Spacing * 2;
				mobImage.X = Spacing;
			}
			else
			{
				mobImage.X = Width / 2 - mobImage.Width / 2;
			}

			yPos += mobImage.Height;
			mobImage2.Position = mobImage.Position;

			//largestWidth =

			float statScale = .4f;

			UILabel lID = new UILabel("ID");
			lID.Scale = statScale;
			lID.X = Spacing;
			lID.Y = yPos;
			AddChild(lID);

			UILabel id = new UILabel(npc.NetID.ToString());
			id.Scale = statScale;
			id.Anchor = AnchorPosition.TopRight;
			id.X = Width - Spacing;
			id.Y = yPos;
			AddChild(id);

			yPos += lID.Height;

			if (npc.Health > 0)
			{
				UILabel lHealth = new UILabel("Health");
				lHealth.Scale = statScale;
				lHealth.X = Spacing;
				lHealth.Y = yPos;
				AddChild(lHealth);

				UILabel lValue = new UILabel(npc.Health.ToString());
				lValue.Scale = statScale;
				lValue.Anchor = AnchorPosition.TopRight;
				lValue.X = Width - Spacing;
				lValue.Y = yPos;
				AddChild(lValue);

				yPos += lHealth.Height;
			}

			if (npc.Damage > 0)
			{
				UILabel lDamage = new UILabel("Damage");
				lDamage.Scale = statScale;
				lDamage.X = Spacing;
				lDamage.Y = yPos;
				AddChild(lDamage);

				UILabel lValue = new UILabel(npc.Damage.ToString());
				lValue.Scale = statScale;
				lValue.Anchor = AnchorPosition.TopRight;
				lValue.X = Width - Spacing;
				lValue.Y = yPos;
				AddChild(lValue);

				yPos += lDamage.Height;
			}

			if (npc.Defense > 0)
			{
				UILabel lDefense = new UILabel("Defense");
				lDefense.Scale = statScale;
				lDefense.X = Spacing;
				lDefense.Y = yPos;
				AddChild(lDefense);

				UILabel lValue = new UILabel(npc.Defense.ToString());
				lValue.Scale = statScale;
				lValue.Anchor = AnchorPosition.TopRight;
				lValue.X = Width - Spacing;
				lValue.Y = yPos;
				AddChild(lValue);

				yPos += lDefense.Height;
			}

			if (npc.KncokbackResist > 0)
			{
				UILabel lKncokbackResist = new UILabel("Knockback Resistance");
				lKncokbackResist.Scale = statScale;
				lKncokbackResist.X = Spacing;
				lKncokbackResist.Y = yPos;
				AddChild(lKncokbackResist);

				UILabel lValue = new UILabel(npc.KncokbackResist.ToString());
				lValue.Scale = statScale;
				lValue.Anchor = AnchorPosition.TopRight;
				lValue.X = Width - Spacing;
				lValue.Y = yPos;
				AddChild(lValue);

				yPos += lKncokbackResist.Height;
			}

			//DropTableView tableView = new DropTableView(DropTableBuilder.DropTable.NPCDropTables[npc.Type], this.Width - Spacing * 2);
			//tableView.Y = yPos;
			//tableView.X = Spacing;
			//AddChild(tableView);
			//yPos += tableView.Height;

			//this.Width = mobImage.Width + Spacing * 2;
			this.Height = yPos + Spacing;
		}

		public override void Update()
		{
			//ErrorLogger.Log(Position.X+ " " + Position.Y);
			//ErrorLogger.Log(ModUtils.CursorPosition.X+ " " + ModUtils.CursorPosition.Y);
			//this.Position = ModUtils.CursorPosition + new Vector2(20, 0);
			this.Position = new Vector2(Parent.Width, 0);
			if (Y + Height > Main.screenHeight)
			{
				Y = Main.screenHeight - Height;
			}
			base.Update();
		}

		public void Close()
		{
			Parent.RemoveChild(this);
		}
	}

	internal class NPCStats
	{
		public string Name { get; set; }
		public int NetID { get; set; }
		public int Type { get; set; }
		public bool IsTownNPC { get; set; }
		public bool IsCritter { get; set; }
		public bool Friendly { get; set; }
		public bool Boss { get; set; }
		public int Damage { get; set; }
		public int Health { get; set; }
		public int Defense { get; set; }
		public int KncokbackResist { get; set; }
		public Color Color { get; set; }
		public Color AlphaColor { get; set; }
		public Mod Mod { get; set; }

		public NPCStats(NPC npc)
		{
			this.Name = Lang.GetNPCNameValue(npc.type);// npc.name;
			this.NetID = npc.netID;
			this.IsTownNPC = npc.townNPC;
			this.IsCritter = NPCID.Sets.CountsAsCritter[npc.type];
			this.Friendly = npc.friendly;
			this.Boss = npc.boss;
			this.Damage = npc.damage;
			this.Health = npc.lifeMax;
			this.Defense = npc.defense;
			this.KncokbackResist = (int)(npc.knockBackResist * 100);
			this.Color = npc.color;
			this.AlphaColor = npc.GetAlpha(Color.White);
			this.Type = npc.type;
			this.Mod = npc.ModNPC?.Mod;
		}

		public override string ToString()
		{
			return this.Name + " : " + this.NetID;
		}

		static public int[] boundNPC = new int[] { NPCID.BoundGoblin, NPCID.BoundMechanic, NPCID.BoundWizard };

		public virtual void Spawn(int playerIndex)
		{
			Player player = Main.player[playerIndex];
			if (IsTownNPC || boundNPC.Contains(Type))
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.type == NetID)
					{
						npc.position = player.position;
						return;
					}
				}
			}

			int index = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)player.position.X, (int)player.position.Y, Type);
			if (NetID < 0)
			{
				Main.npc[index].SetDefaults(NetID);
			}
		}
	}

	internal class WallOfFlesh : NPCStats
	{
		public WallOfFlesh(NPC npc) : base(npc)
		{
		}

		public override void Spawn(int playerIndex)
		{
			Player player = Main.player[playerIndex];

			Vector2 pos = player.position;

			if (pos.Y / 16f < (float)(Main.maxTilesY - 205))
			{
				Main.NewText(HEROsMod.HeroText("UnderworldToSpawnWoF"));
				return;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				return;
			}
			Player.FindClosest(pos, 16, 16);
			int num = 1;
			if (pos.X / 16f > (float)(Main.maxTilesX / 2))
			{
				num = -1;
			}
			bool flag = false;
			int num2 = (int)pos.X;
			while (!flag)
			{
				flag = true;
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && Main.player[i].position.X > (float)(num2 - 1200) && Main.player[i].position.X < (float)(num2 + 1200))
					{
						num2 -= num * 16;
						flag = false;
					}
				}
				if (num2 / 16 < 20 || num2 / 16 > Main.maxTilesX - 20)
				{
					flag = true;
				}
			}
			int num3 = (int)pos.Y;
			int num4 = num2 / 16;
			int num5 = num3 / 16;
			int num6 = 0;
			try
			{
				while (WorldGen.SolidTile(num4, num5 - num6) || Main.tile[num4, num5 - num6].LiquidAmount >= 100)
				{
					if (!WorldGen.SolidTile(num4, num5 + num6) && Main.tile[num4, num5 + num6].LiquidAmount < 100)
					{
						num5 += num6;
						goto IL_162;
					}
					num6++;
				}
				num5 -= num6;
			}
			catch
			{
			}
		IL_162:
			num3 = num5 * 16;
			int num7 = NPC.NewNPC(NPC.GetSource_NaturalSpawn(), num2, num3, 113, 0);
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				Main.NewText(Language.GetTextValue("Announcement.HasAwoken", Main.npc[num7].TypeName), 175, 75, 255);
				return;
			}
			if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", new object[]
						{
							Main.npc[num7].GetTypeNetName()
						}), new Color(175, 75, 255), -1);
			}
		}
	}
}
