using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;

//using System.Xml.Serialization;
//using System.Xml;
//using System.Reflection;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace HEROsMod.UIKit.UIComponents
{
	internal class ItemBrowser : UIWindow
	{
		internal static string HeroText(string key, string category = "ItemBrowser") => HEROsMod.HeroText($"{category}.{key}");

		private ItemCollectionView _itemView;
		private Slot _trashCan;
		private UIScrollView _categoryView;
		private Category _selectedCategory;
		private Item[] _currentItems;
		private UIImage _bCollapseCategories;
		private UIButton _bBack;
		private UIImage _bClose;
		private UIView _filterView;
		private UIImage _spacer;
		private UIView _sortView;

		private Asset<Texture2D> _collapseTexture;
		private Asset<Texture2D> _expandTexture;
		private Asset<Texture2D> _spacerTexture;

		private float _collapsePosition = 1f;
		private bool _collapsed = false;
		private float shownWidth;
		private float hiddenWidth;

		public UITextbox SearchBox;

		internal Category SelectedCategory
		{
			get { return _selectedCategory; }
			set
			{
				if (_selectedCategory != value)
				{
					SearchBox.Text = string.Empty;
				}
				_selectedCategory = value;
				if (CategoriesLoaded)
				{
					PopulateFilterView(); // mod/vanilla,
					PopulateSortView(); // damage, value, alpha, itemid
					CurrentItems = GetItems(); // takes care of filters and sorts
					PopulateCategoryView();
				}
			}
		}

		private Item[] CurrentItems
		{
			get { return _currentItems; }
			set
			{
				_currentItems = value;
				_itemView.Items = _currentItems;
			}
		}

		internal Sort SelectedSort;
		internal Sort[] AvailableSorts = new Sort[0];
		//internal Sort[] DefaultSorts;

		public ItemBrowser()
		{
			this.CanMove = true;
			//Height = 420;

			_expandTexture = HEROsMod.instance.Assets.Request<Texture2D>("Images/ExpandIcon", AssetRequestMode.ImmediateLoad);
			_collapseTexture = HEROsMod.instance.Assets.Request<Texture2D>("Images/CollapseIcon", AssetRequestMode.ImmediateLoad);
			_spacerTexture = HEROsMod.instance.Assets.Request<Texture2D>("Images/spacer", AssetRequestMode.ImmediateLoad);

			//UILabel lTitle = new UILabel("Item Browser");
			//lTitle.Scale = .6f;
			//lTitle.X = LargeSpacing;
			//lTitle.Y = LargeSpacing;
			//lTitle.OverridesMouse = false;
			//AddChild(lTitle);

			_itemView = new ItemCollectionView();
			Height = LargeSpacing * 2 + _itemView.Height + 32;
			_itemView.X = LargeSpacing;
			_itemView.Y = Height - LargeSpacing - _itemView.Height;
			AddChild(_itemView);

			_categoryView = new UIScrollView();
			_categoryView.X = _itemView.X + _itemView.Width + LargeSpacing;
			_categoryView.Y = _itemView.Y;
			_categoryView.Width = 45 + numberCategoryColumns * 100;
			_categoryView.Height = _itemView.Height;
			AddChild(_categoryView);

			Width = _categoryView.X + _categoryView.Width + LargeSpacing;

			_bClose = new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/closeButton", AssetRequestMode.ImmediateLoad));
			_bClose.Anchor = AnchorPosition.TopRight;
			_bClose.Position = new Vector2(Width - LargeSpacing, LargeSpacing);
			_bClose.onLeftClick += bClose_onLeftClick;
			AddChild(_bClose);

			SearchBox = new UITextbox();
			SearchBox.Width = 125;
			SearchBox.X = _itemView.X + _itemView.Width - SearchBox.Width;
			SearchBox.Y = _itemView.Y - SearchBox.Height - Spacing - 4;
			SearchBox.KeyPressed += textbox_KeyPressed;
			AddChild(SearchBox);

			_filterView = new UIView();
			_filterView.Position = new Vector2(LargeSpacing, Spacing);
			//_filterView.onLeftClick += _bViewAllItems_onLeftClick;
			//{
			//	UIImage test = new UIImage(HEROsMod.instance.GetTexture("Images/closeButton"));
			//	test.onLeftClick += bClose_onLeftClick;
			//	_filterView.AddChild(test);
			//	UIImage test2 = new UIImage(HEROsMod.instance.GetTexture("Images/closeButton"));
			//	test2.onLeftClick += bClose_onLeftClick;
			//	test2.Position = test.Position;
			//	test2.X += test.Width;
			//	test2.ForegroundColor = Color.Azure;
			//	_filterView.AddChild(test2);
			//}
			//	_filterView.ForegroundColor = Color.Red;
			//	_filterView.BackgroundColor = Color.Pink;
			//	_filterView.Width = 100;
			_filterView.Height = 40;
			AddChild(_filterView);

			_spacer = new UIImage(_spacerTexture);
			_spacer.Position = new Vector2(Spacing, LargeSpacing);
			_spacer.Height = 40;
			AddChild(_spacer);

			_sortView = new UIView();
			_sortView.Position = new Vector2(Spacing, Spacing);
			//_sortView.onLeftClick += _bViewAllItems_onLeftClick;
			//	_sortView.ForegroundColor = Color.Red;
			//	_sortView.BackgroundColor = Color.Red;
			//	_sortView.Width = 30;
			_sortView.Height = 40;
			AddChild(_sortView);

			//_trashCan = new Slot(0);
			//_trashCan.IsTrachCan = true;
			//_trashCan.X = _itemView.X;
			//_trashCan.Y = _itemView.Y - _trashCan.Height - SmallSpacing/2;
			//AddChild(_trashCan);

			_bBack = new UIButton(Language.GetTextValue("UI.Back"));
			_bBack.X = _categoryView.X;
			_bBack.Y = _categoryView.Y - _bBack.Height - Spacing;
			_bBack.onLeftClick += _bViewAllItems_onLeftClick;
			AddChild(_bBack);

			_bCollapseCategories = new UIImage(_collapseTexture);
			_bCollapseCategories.X = this.Width - _bCollapseCategories.Width - LargeSpacing;
			_bCollapseCategories.Y = _categoryView.Y - _bCollapseCategories.Height - Spacing;
			_bCollapseCategories.onLeftClick += _bCollapseCategories_onLeftClick;
			AddChild(_bCollapseCategories);

			shownWidth = _categoryView.X + _categoryView.Width + LargeSpacing;
			hiddenWidth = _itemView.X + _itemView.Width + LargeSpacing;

			//ParseList2();
			SelectedCategory = null;
		}

		private void _bViewAllItems_onLeftClick(object sender, EventArgs e)
		{
			if (SelectedCategory != null)
			{
				SelectedCategory = SelectedCategory.ParentCategory;
			}
			if (SelectedCategory == null)
			{
				((UIButton)sender).Visible = false;
			}
		}

		private void _bCollapseCategories_onLeftClick(object sender, EventArgs e)
		{
			_collapsed = !_collapsed;
			if (_collapsed)
			{
				_bCollapseCategories.Texture = _expandTexture;
			}
			else
			{
				_bCollapseCategories.Texture = _collapseTexture;
			}
		}

		private void WhiteAllCategoryButtons()
		{
			for (int i = 1; i < _categoryView.children.Count; i++)
			{
				((UIButton)_categoryView.children[i]).SetTextColor(Color.White);
			}
		}

		private static int numberCategoryColumns = 1;

		private void PopulateCategoryView()
		{
			Category[] categories = Categories;
			if (SelectedCategory != null)
			{
				categories = SelectedCategory.SubCategories.ToArray();
				if (categories.Length == 0) return;
			}

			_categoryView.ClearContent();
			float yPos = 0;
			for (int i = 0; i < categories.Length; i++)
			{
				UIButton button = new UIButton(categories[i].LocalizedName);
				button.Tag = categories[i];
				button.AutoSize = false;
				button.Width = 100;
				int x = i % numberCategoryColumns;
				int y = i / numberCategoryColumns;
				button.X = Spacing + x * (button.Width + Spacing);
				button.Y = Spacing + y * (button.Height + Spacing);
				button.onLeftClick += button_onLeftClick;
				yPos = button.Y + button.Height + Spacing;
				_categoryView.AddChild(button);
			}
			_categoryView.ContentHeight = yPos;
		}

		private void PopulateSortView()
		{
			if(DefaultSorts == null)
			{
				DefaultSorts = new Sort[]
				{
					new Sort( new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortItemID", AssetRequestMode.ImmediateLoad)){Tooltip = HeroText("SortName.ItemID")} , (x,y)=>x.type.CompareTo(y.type)),
					new Sort( new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortValue", AssetRequestMode.ImmediateLoad)){Tooltip = HeroText("SortName.Value")} , (x,y)=>x.value.CompareTo(y.value)),
					new Sort( new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortAZ", AssetRequestMode.ImmediateLoad)){Tooltip = HeroText("SortName.Alphabetical")} , (x,y)=>x.Name.CompareTo(y.Name)),
				};
			}
			List<Sort> sorts = DefaultSorts.ToList();

			if (SelectedCategory != null)
			{
				if (SelectedCategory.ParentCategory != null)
				{
					foreach (var item in SelectedCategory.ParentCategory.Sorts)
					{
						sorts.Add(item);
					}
				}
				foreach (var item in SelectedCategory.Sorts)
				{
					sorts.Add(item);
				}
			}

			AvailableSorts = sorts.ToArray();

			_sortView.RemoveAllChildren();
			_sortView.X = _spacer.X + _spacer.Width;
			float xPos = 0;
			for (int i = 0; i < AvailableSorts.Length; i++)
			{
				UIImage button = AvailableSorts[i].button;
				button.Width = 20;
				int x = i / 1;
				int y = i % 1;
				button.X = Spacing + x * (button.Width + Spacing);
				button.Y = Spacing + y * (button.Height + Spacing);
				xPos = button.X + button.Width + Spacing;
				_sortView.AddChild(button);
			}
			_sortView.Width = xPos;
		}

		private void PopulateFilterView()
		{
			if (Filters == null)
			{
				Filters = new Filter[]
				{
					new Filter( new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/filterMod", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("FilterName.ModFilter")}, x=>x.ModItem != null),
				};
			}
			//Category[] categories = Categories;
			//if (SelectedCategory != null)
			//{
			//	categories = SelectedCategory.SubCategories.ToArray();
			//	if (categories.Length == 0) return;
			//}

			_filterView.RemoveAllChildren();
			float xPos = 0;
			for (int i = 0; i < Filters.Length; i++)
			{
				Filter f = Filters[i];

				UIImage button = Filters[i].button;
				if (f.enabled)
				{
					button.ForegroundColor = Color.White;
				}
				else
				{
					button.ForegroundColor = Color.Gray;
				}

				//button.Tag = categories[i];
				//button.AutoSize = false;
				button.Width = 20;
				int x = i / 1;
				int y = i % 1;
				button.X = Spacing + x * (button.Width + Spacing);
				button.Y = Spacing + y * (button.Height + Spacing);
				//button.onLeftClick += (s, e) => {
				//	f.enabled = !f.enabled;
				//	Main.NewText(f.button.Tooltip + " " + f.enabled);
				//};
				//button.onLeftClick += (s, e) => Main.NewText(((UIImage)s).Tooltip);
				xPos = button.X + button.Width + Spacing;
				_filterView.AddChild(button);
			}
			_filterView.Width = xPos;
			_spacer.X = _filterView.X + _filterView.Width;
		}

		private void button_onLeftClick(object sender, EventArgs e)
		{
			UIButton button = (UIButton)sender;
			WhiteAllCategoryButtons();
			button.SetTextColor(Color.Yellow);
			SelectedCategory = (Category)button.Tag;
		}

		public override void Update()
		{
			if (!Main.playerInventory) this.Visible = false;

			float moveSpeed = 5f;
			_categoryView.Visible = true;
			_bBack.Visible = SelectedCategory != null;
			if (_collapsed)
			{
				_collapsePosition -= ModUtils.DeltaTime * moveSpeed;
				if (_collapsePosition < 0f)
				{
					_categoryView.Visible = false;
					_bBack.Visible = false;
					_collapsePosition = 0f;
				}
			}
			else
			{
				_collapsePosition += ModUtils.DeltaTime * moveSpeed;
				if (_collapsePosition > 1f) _collapsePosition = 1f;
			}
			this.Width = MathHelper.SmoothStep(hiddenWidth, shownWidth, _collapsePosition);
			_categoryView.Opacity = MathHelper.SmoothStep(0, 1f, _collapsePosition);
			_bBack.X = _categoryView.X;
			_bBack.Opacity = _categoryView.Opacity;
			_bCollapseCategories.X = this.Width - _bCollapseCategories.Width - LargeSpacing - 30;
			_categoryView.X = this.Width - _categoryView.Width - LargeSpacing;
			_bClose.X = this.Width - LargeSpacing;
			SearchBox.X = _itemView.X + _itemView.Width - SearchBox.Width;
			if (_bCollapseCategories.X - Spacing < SearchBox.X + SearchBox.Width)
			{
				SearchBox.X = _bCollapseCategories.X + -SearchBox.Width - Spacing;
			}
			base.Update();
		}

		private void bClose_onLeftClick(object sender, EventArgs e)
		{
			this.Visible = false;
		}

		private void textbox_KeyPressed(object sender, char key)
		{
			//if (_selectedCategory != null)
			//{
			//	_selectedCategory = null;
			//	CurrentItems = GetItems();
			//	PopulateCategoryView();
			//}

			//if (SearchBox.Text.Length > 0)
			//{
			//	List<Item> matches = new List<Item>();
			//	foreach (Item item in CurrentItems)
			//	{
			//		if (item.name.ToLower().IndexOf(SearchBox.Text.ToLower(), System.StringComparison.Ordinal) != -1)
			//		{
			//			matches.Add(item);
			//		}
			//	}
			//	if (matches.Count > 0)
			//	{
			//		_itemView.Items = matches.ToArray();
			//	}
			//	else
			//	{
			//		SearchBox.Text = SearchBox.Text.Substring(0, SearchBox.Text.Length - 1);
			//	}
			//}
			//else
			//{
			//	_itemView.Items = CurrentItems;
			//}
			if (SearchBox.Text.Length > 0)
			{
				bool match = false;
				foreach (Item item in CurrentItems)
				{
					if (item.Name.ToLower().IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) != -1)
					{
						match = true;
						break;
					}
				}
				if (!match)
				{
					SearchBox.Text = SearchBox.Text.Substring(0, SearchBox.Text.Length - 1);
				}
			}

			SelectedCategory = SelectedCategory;
		}

		// Caller of Category.GetItems
		public Item[] GetItems()
		{
			foreach (var item in AvailableSorts)
			{
				item.button.ForegroundColor = Color.Gray;
			}
			if (SelectedSort == null || !AvailableSorts.Contains(SelectedSort))
			{
				//ErrorLogger.Log("Default Sort Selected");
				SelectedSort = DefaultSorts[0];
			}
			SelectedSort.button.ForegroundColor = Color.White;

			List<Item> result = new List<Item>();
			if (SelectedCategory == null)
			{
				foreach (Category category in Categories)
				{
					Item[] items = category.GetItems();
					foreach (Item item in items)
					{
						result.Add(item);
					}
				}
			}
			else
			{
				result = SelectedCategory.GetItems().ToList();
			}
			result = result.Where(item => item.Name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) != -1).ToList();
			result = result.Distinct().Where(item => PassFilters(item)).ToList();
			result.Sort(new MyComparer(this));
			return result.ToArray();
		}

		//private int SortMethod(Item x, Item y)
		//{
		//	ErrorLogger.Log("SortMethod" + SelectedSort.button.Tooltip);

		//	if (!AvailableSorts.Contains(SelectedSort))
		//	{
		//		ErrorLogger.Log("Default Sort Selected");
		//		SelectedSort = DefaultSorts[0];
		//	}
		//	return SelectedSort.sort(x, y);
		//}

		//public override int CompareTo(object obj)
		//{
		//	switch (Interface.modBrowser.sortMode)
		//	{
		//		case SortModes.DisplayNameAtoZ:
		//			return this.displayname.CompareTo((obj as UIModDownloadItem).displayname);
		//		case SortModes.DisplayNameZtoA:
		//			return -1 * this.displayname.CompareTo((obj as UIModDownloadItem).displayname);
		//		case SortModes.DownloadsAscending:
		//			return this.downloads.CompareTo((obj as UIModDownloadItem).downloads);
		//		case SortModes.DownloadsDescending:
		//			return -1 * this.downloads.CompareTo((obj as UIModDownloadItem).downloads);
		//		case SortModes.RecentlyUpdated:
		//			return -1 * this.timeStamp.CompareTo((obj as UIModDownloadItem).timeStamp);
		//	}
		//	return base.CompareTo(obj);
		//}

		public bool PassFilters(Item item)
		{
			foreach (var filter in Filters)
			{
				if (filter.enabled && !filter.filter(item))
				{
					return false;
				}
			}
			return true;
		}

		public static bool CategoriesLoaded = false;
		public static Category[] Categories { get; set; }

		public static Filter[] Filters { get; set; }

		public static Sort[] DefaultSorts { get; set; }

		internal static void Unload()
		{
			//ErrorLogger.Log("Unloading Categories");
			if (CategoriesLoaded)
			{
				foreach (var category in Categories)
				{
					foreach (var subCategory in category.SubCategories)
					{
						subCategory.Items.Clear();
					}
					category.Items.Clear();
				}
			}
			CategoriesLoaded = false;
		}

		public static void ParseList2()
		{
			Category modCategory = new Category("Mod");
			modCategory.SubCategories = new List<Category>();
			foreach (Mod loadedMod in ModLoader.Mods.OrderBy(x => x.Name))
			{
				modCategory.SubCategories.Add(new Category(loadedMod.Name, x => x.ModItem != null && x.ModItem.Mod.Name == loadedMod.Name, skipLocalization: true));
			}

			Categories = new Category[] {
				new Category("Weapons"/*, x=>x.damage>0*/) {
					SubCategories = new List<Category>() {
						new Category("Melee", x=>x.DamageType == DamageClass.Melee),
						new Category("Magic", x=>x.DamageType == DamageClass.Magic),
						new Category("Ranged", x=>x.DamageType == DamageClass.Ranged && x.ammo == 0) // TODO and ammo no
						{
							Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortAmmo", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.UseAmmoType")}, (x,y)=>x.useAmmo.CompareTo(y.useAmmo)), }
						},
						//new Category("Throwing", x=>x.thrown),
						new Category("Summon", x=>x.DamageType == DamageClass.Summon && !x.sentry),
						new Category("Sentry", x=>x.DamageType == DamageClass.Summon && x.sentry),
					},
					Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortDamage", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.Damage")}, (x,y)=>x.damage.CompareTo(y.damage)), }
				},
				new Category("Tools"/*,x=>x.pick>0||x.axe>0||x.hammer>0*/) {
					SubCategories = new List<Category>() {
						new Category("Pickaxes", x=>x.pick>0) { Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortPick", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.PickPower")}, (x,y)=>x.pick.CompareTo(y.pick)), } },
						new Category("Axes", x=>x.axe>0){ Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortAxe", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.AxePower")}, (x,y)=>x.axe.CompareTo(y.axe)), } },
						new Category("Hammers", x=>x.hammer>0){ Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortHammer", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.HammerPower")}, (x,y)=>x.hammer.CompareTo(y.hammer)), } },
					},
				},
				new Category("Armor"/*,  x=>x.headSlot!=-1||x.bodySlot!=-1||x.legSlot!=-1*/) {
					SubCategories = new List<Category>() {
						new Category("Head", x=>x.headSlot!=-1),
						new Category("Body", x=>x.bodySlot!=-1),
						new Category("Legs", x=>x.legSlot!=-1),
					},
					Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortDefense", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.Defense")}, (x,y)=>x.defense.CompareTo(y.defense)), }
				},
				new Category("Placeables"/*,  x=>x.createTile!=-1||x.createWall!=-1*/) {
					SubCategories = new List<Category>() {
						new Category("Tiles", x=>x.createTile!=-1),
						new Category("Walls", x=>x.createWall!=-1),
					}
				},
				new Category("Accessories", x=>x.accessory),
				new Category("Ammo", x=>x.ammo!=0)
				{
					Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortAmmo", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.AmmoType")}, (x,y)=>x.ammo.CompareTo(y.ammo)), }
				},
				new Category("Potions", x=>(x.UseSound != null/* && x.UseSound.Style == 3*/)),
				new Category("Expert", x=>x.expert),
				new Category("Pets"/*, x=> x.buffType > 0 && (Main.vanityPet[x.buffType] || Main.lightPet[x.buffType])*/){
					SubCategories = new List<Category>() {
						new Category("Pets", x=>Main.vanityPet[x.buffType]),
						new Category("LightPets", x=>Main.lightPet[x.buffType]),
					}
				},
				new Category("Mounts", x=>x.mountType != -1),
				new Category("Dyes", x=>x.dye != 0),
				new Category("BossSummons", x=>ItemID.Sets.SortingPriorityBossSpawns[x.type] != -1 && x.type != ItemID.LifeCrystal && x.type != ItemID.ManaCrystal && x.type != ItemID.CellPhone && x.type != ItemID.IceMirror && x.type != ItemID.MagicMirror && x.type != ItemID.LifeFruit && x.netID != ItemID.TreasureMap || x.netID == ItemID.PirateMap) { // vanilla bug.
					Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortDamage", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.ProgressionOrder")}, (x,y)=>ItemID.Sets.SortingPriorityBossSpawns[x.type].CompareTo(ItemID.Sets.SortingPriorityBossSpawns[y.type])), }
				},
				new Category("Consumables", x=>x.consumable),
				new Category("Fishing"/*, x=> x.fishingPole > 0 || x.bait>0|| x.questItem*/){
					SubCategories = new List<Category>() {
						new Category("Poles", x=>x.fishingPole > 0) {Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortFish", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.PolePower")}, (x,y)=>x.fishingPole.CompareTo(y.fishingPole)), } },
						new Category("Bait", x=>x.bait>0) {Sorts = new Sort[] { new Sort(new UIImage(HEROsMod.instance.Assets.Request<Texture2D>("Images/sortBait", AssetRequestMode.ImmediateLoad)) {Tooltip = HeroText("SortName.BaitPower")}, (x,y)=>x.bait.CompareTo(y.bait)), } },
						new Category("QuestFish", x=>x.questItem),
					}
				},
				modCategory,
				new Category("Other", x=>false),
			};

			List<Category> categoryList = new List<Category>(Categories);
			foreach (var modCallCategory in HEROsMod.instance.modCategories)
			{
				if (string.IsNullOrEmpty(modCallCategory.Parent))
				{
					categoryList.Insert(categoryList.Count - 2, new Category(modCallCategory.Name, modCallCategory.belongs, true));
				}
				else
				{
					foreach (var item in categoryList)
					{
						if (item.Name == modCallCategory.Parent)
						{
							item.SubCategories.Add(new Category(modCallCategory.Name, modCallCategory.belongs, true));
						}
					}
				}
			}
			Categories = categoryList.ToArray();

			foreach (var parent in Categories)
			{
				foreach (var sub in parent.SubCategories)
				{
					sub.ParentCategory = parent;
				}
			}

			//var cats = Categories.ToList();
			//Category mod = new Category("Mod");
			//modCategory.SubCategories = new List<Category>();
			//foreach (Mod loadedMod in ModLoader.LoadedMods)
			//{
			//	if (loadedMod.Name != "ModLoader")
			//		modCategory.SubCategories.Add(new Category(loadedMod.Name, x => x.modItem != null && x.modItem.mod.Name == loadedMod.Name));
			//}
			//cats.Insert(cats.Count - 1, modCategory);
			//Categories = cats.ToArray();

			for (int i = 1; i < TextureAssets.Item.Length; i++)
			{
				if (!ItemID.Sets.Deprecated[i])
				{
					Item item = new Item();
					item.SetDefaults(i);
					bool other = true;
					foreach (var category in Categories)
					{
						if (category.belongs(item))
						{
							other = false;
							category.Items.Add(item);
						}
						foreach (var subCategory in category.SubCategories)
						{
							if (subCategory.belongs(item))
							{
								if (category != modCategory)
									other = false; // Don't count modded items as not Other just because of modCategory
								subCategory.Items.Add(item);
							}
						}
					}
					if (other)
					{
						Categories[Categories.Length - 1].Items.Add(item);
					}
				}
			}
			modCategory.SubCategories.RemoveAll(sub => sub.Items.Count == 0);
			foreach (var category in Categories)
			{
				// Sort? Value, damage, Tile/Placestyle, rare.
			}
			CategoriesLoaded = true;
			//foreach (var cat in Categories)
			//{
			//	ErrorLogger.Log("ParseList2 End");
			//	ErrorLogger.Log(cat.ToString());
			//}
			// Categories, subcats: what is it
			// Filters: string match, mod specific/vanilla/Anymod. Global?, multiple active
			// Sorts: default: ID, damage, alphabetical, add at each level?, 1 active at a time
		}
	}

	internal class MyComparer : IComparer<Item>
	{
		internal ItemBrowser br;

		public MyComparer(ItemBrowser br)
		{
			this.br = br;
		}

		public int Compare(Item a, Item b)
		{
			//if (br.SelectedSort.reversed)
			//{
			//	return br.SelectedSort.sort(b, a);
			//}
			//else
			//{
			//	return br.SelectedSort.sort(a, b);
			//}
			return br.SelectedSort.sort(a, b);
		}
	}

	internal class Filter
	{
		public Predicate<Item> filter;
		internal UIImage button;
		internal bool enabled = false;

		public Filter(UIImage button, Predicate<Item> filter)
		{
			this.filter = filter;
			this.button = button;
			button.onLeftClick += Button_onLeftClick;
		}

		private void Button_onLeftClick(object sender, EventArgs e)
		{
			enabled = !enabled;
			//Main.NewText(button.Tooltip + " " + enabled);
			(button.Parent.Parent as ItemBrowser).SelectedCategory = (button.Parent.Parent as ItemBrowser).SelectedCategory;
		}
	}

	internal class Sort
	{
		public Func<Item, Item, int> sort;
		internal UIImage button;

		//internal bool enabled = false;
		public Sort(UIImage button, Func<Item, Item, int> sort)
		{
			this.sort = sort;
			this.button = button;
			button.onLeftClick += Button_onLeftClick;
		}

		private void Button_onLeftClick(object sender, EventArgs e)
		{
			//enabled = !enabled;
			//Main.NewText(button.Tooltip + " " + enabled);
			(button.Parent.Parent as ItemBrowser).SelectedSort = this;
			(button.Parent.Parent as ItemBrowser).SelectedCategory = (button.Parent.Parent as ItemBrowser).SelectedCategory;
		}
	}

	//public class Sort
	//{
	//	public Predicate<Item> filter;
	//	internal UIImage button;
	//}

	// Represents a requested Category
	internal class ModCategory
	{
		internal Predicate<Item> belongs;

		internal string Name { get; private set; }
		internal string Parent { get; private set; }
		public ModCategory(string name, string parent, Predicate<Item> belongs)
		{
			Name = name;
			Parent = parent;
			this.belongs = belongs;
		}
	}
	
	public class Category
	{
		//private Category _parentCategory = null;
		public Category ParentCategory;

		//{
		//	get { return _parentCategory; }
		//}

		public Predicate<Item> belongs;

		public string Name { get; set; }
		public string LocalizedName { get; set; }
		public List<Item> Items { get; set; }
		public List<Category> SubCategories { get; set; }
		internal Sort[] Sorts { get; set; }

		public Category(string name)
		{
			this.Name = name;
			this.LocalizedName = HEROsMod.HeroText($"ItemBrowser.CategoryName.{name}");
			Items = new List<Item>();
			SubCategories = new List<Category>();
			Sorts = new Sort[0];
			this.belongs = x => false;
		}

		public Category(string name, Predicate<Item> belongs, bool skipLocalization = false)
		{
			this.Name = name;
			this.LocalizedName = skipLocalization ? name : HEROsMod.HeroText($"ItemBrowser.CategoryName.{name}");
			Items = new List<Item>();
			SubCategories = new List<Category>();
			Sorts = new Sort[0];
			this.belongs = belongs;
		}

		//public void AddCategory(Category category)
		//{
		//	category._parentCategory = this;
		//	SubCategories.Add(category);
		//	SubCategories = SubCategories.OrderBy(x => x.Name).ToList();
		//}

		public Item[] GetItems()
		{
			List<Item> result = new List<Item>();
			foreach (Item item in Items)
			{
				result.Add(item);
			}
			foreach (Category subCategory in SubCategories)
			{
				Item[] subItems = subCategory.GetItems();
				foreach (Item subItem in subItems)
				{
					result.Add(subItem);
				}
			}
			return result.ToArray();
		}

		public override string ToString()
		{
			return this.Name + " - Item Count: " + Items.Count;
		}
	}
}

//[Serializable()]
//public class XMLCategory
//{
//    [XmlAttribute]
//    public string Name { get; set; }

//    [XmlArray("Items", IsNullable = true)]
//    [XmlArrayItem("Item", typeof(string))]
//    public List<string> Items { get; set; }

//    [XmlArray("SubCategories", IsNullable= true)]
//    [XmlArrayItem("Category", typeof(XMLCategory))]
//    public List<XMLCategory> SubCategories { get; set; }

//    private XMLCategory ParentCategory { get; set; }

//    public XMLCategory()
//    {
//        SubCategories = new List<XMLCategory>();
//        Items = new List<string>();
//    }

//    public XMLCategory GetParent()
//    {
//        return ParentCategory;
//    }

//    public void SetParent(XMLCategory category)
//    {
//        ParentCategory = category;
//    }

//    public Category Convert()
//    {
//        Category result = new Category(this.Name);
//        foreach(string item in Items)
//        {
//            string[] s = item.Split(' ');
//            int itemNum = int.Parse(s[0]);
//            Item newItem = new Item();
//            newItem.SetDefaults(itemNum);
//            result.Items.Add(newItem);
//        }
//        foreach(XMLCategory category in SubCategories)
//        {
//            result.AddCategory(category.Convert());
//        }
//        return result;
//    }
//}

//[Serializable()]
//[XmlRoot("CategoryCollection")]
//public class CategoryCollection
//{
//    [XmlArray("Categories")]
//    [XmlArrayItem("Category", typeof(XMLCategory))]
//    public List<XMLCategory> Categories { get; set; }

//    public CategoryCollection()
//    {
//        Categories = new List<XMLCategory>();
//    }
//}

//public static int[] CategoriesToSortingArray()
//{
//	//if (Categories == null)
//	//{
//	//	ParseList2();
//	//}
//	if (!CategoriesLoaded)
//	{
//		ModUtils.DebugText("CategoriesToSortingArray calling ParseList2");
//		ParseList2();
//	}
//	List<Item> allItems = new List<Item>();
//	foreach (Category category in Categories)
//	{
//		Item[] items = category.GetItems();
//		foreach (Item item in items)
//		{
//			allItems.Add(item);
//		}
//	}
//	int[] sortArray = new int[Main.itemTexture.Length];
//	for (int i = 0; i < allItems.Count; i++)
//	{
//		if (i < allItems.Count)
//			sortArray[allItems[i].type] = i;
//	}
//	sortArray[0] = 5000;
//	return sortArray;
//}

//void ParseList()
//{
//    string list = File.ReadAllText("list.txt");
//    string pattern = @"(\n\d+\s[\(\)\.\w\d\s'_-]+,)|(<[\w+\s]+>|<\/[\w+\s]+>)";
//    MatchCollection matches = Regex.Matches(list, pattern);

//    List<string> openTags = new List<string>();

//    Category currentCategory = null;
//    List<Category> categories = new List<Category>();
//    int itemCount = 0;

//    CategoryCollection cc = new CategoryCollection();
//    XMLCategory currentXMLCategory = null;

//    foreach (Match match in matches)
//    {
//        foreach (Capture capture in match.Captures)
//        {
//            //Console.WriteLine(capture.Value);
//            pattern = @"<[\w+\s]+>";
//            Match m = Regex.Match(capture.Value, pattern);
//            if(m.Success)
//            {
//                string tagName = capture.Value.Substring(1, capture.Value.Length - 2);
//                openTags.Add(tagName);
//                Category newCategory = new Category(tagName);
//                XMLCategory newXmlCategory = new XMLCategory();
//                newXmlCategory.Name = tagName;
//                if(currentCategory == null)
//                {
//                    currentCategory = newCategory;
//                    categories.Add(currentCategory);

//                    newXmlCategory.SetParent(currentXMLCategory);
//                    currentXMLCategory = newXmlCategory;
//                    cc.Categories.Add(newXmlCategory);
//                }
//                else
//                {
//                    currentCategory.AddCategory(newCategory);
//                    currentCategory = newCategory;

//                    newXmlCategory.SetParent(currentXMLCategory);
//                    currentXMLCategory.SubCategories.Add(newXmlCategory);
//                    currentXMLCategory = newXmlCategory;
//                }
//            }
//            else
//            {
//                pattern = @"<[\/\w+\s]+>";
//                m = Regex.Match(capture.Value, pattern);
//                if(m.Success)
//                {
//                    string tagName = capture.Value.Substring(2, capture.Value.Length - 3);

//                    if(currentCategory.Name != tagName)
//                    {
//                        Console.WriteLine("oops");
//                    }
//                    else
//                    {
//                        currentCategory = currentCategory.ParentCategory;
//                        currentXMLCategory = currentXMLCategory.GetParent();

//                    }

//                    if (openTags.Contains(tagName))
//                    {
//                        openTags.Remove(tagName);
//                    }
//                }
//                else
//                {
//                    pattern = @"\n\d+\s";
//                    m = Regex.Match(capture.Value, pattern);
//                    if (m.Success)
//                    {
//                        int itemNum = int.Parse(m.ToString());
//                        Item newItem = new Item();
//                        newItem.SetDefaults(itemNum);
//                        currentCategory.Items.Add(newItem);
//                        currentXMLCategory.Items.Add(itemNum + " " + newItem.name);
//                        itemCount++;
//                    }
//                }
//            }
//        }
//    }

//    XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(cc.GetType());
//    //x.Serialize(Console.Ou, cc);

//    XmlWriterSettings settings = new XmlWriterSettings();
//    settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
//    settings.Indent = true;
//    settings.OmitXmlDeclaration = false;

//    string xmlOut;
//    using (StringWriter textWriter = new StringWriter())
//    {
//        using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
//        {
//            serializer.Serialize(xmlWriter, cc);

//        }
//        xmlOut = textWriter.ToString();
//    }

//    Console.WriteLine();

//    Categories = categories.OrderBy(x => x.Name).ToArray();
//    /*
//    Console.WriteLine(count);
//    Console.WriteLine(categories.Count);

//    //itemView.category = itemView.allItemsSlots;
//    //itemView.category = new int[itemView.allItemsSlots.Length];
//    int[] items = new int[itemView.allItemsSlots.Length];
//    for (int i = 0; i < itemView.allItemsSlots.Length; i++) items[i] = i;
//    itemView.category = items;

//    itemView.activeSlots = itemView.category;

//    string output = "";
//    output += "static int[][] categoryNums = new int[][]{";
//    for (int i = 0; i < categories.Count; i++)
//    {
//        if (i != 0) output += ",\r\n";
//        output += "new int[]{";
//        for (int j = 0; j < categories[i].Count; j++)
//        {
//            if (j != 0) output += ",";
//            Slot slot = itemView.allItemsSlots[categories[i][j]];
//            output += slot.index;

//        }
//        output += "}";

//    }
//    output += "}\r\n\r\n";
//    Console.Write(output);

//    output += "static string[] categNames = new string[]{\r\n";
//    for (int i = 0; i < categoryNames.Count; i++)
//    {
//        if (i != 0) output += ",\r\n";
//        output += "\"" + categoryNames[i] + "\"";
//    }
//    output += "\r\n};";
//    Console.WriteLine(output);
//    File.WriteAllText("slotNums.txt", output);
//    */
//}