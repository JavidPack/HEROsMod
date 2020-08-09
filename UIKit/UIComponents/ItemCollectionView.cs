using Terraria;
using Terraria.GameContent;

namespace HEROsMod.UIKit.UIComponents
{
	internal class ItemCollectionView : UIScrollView
	{
		private Item[] _items;

		public Item[] Items
		{
			get { return _items; }
			set
			{
				_items = value;
				RepopulateSlots();
			}
		}

		private int slotSpace = 4;
		private int slotColumns = 8;
		private float slotSize = Slot.backgroundTexture.Width * .85f;
		private int slotRows = 4;
		private Slot[] slots = new Slot[TextureAssets.Item.Length];

		public ItemCollectionView()
		{
			Width = (slotSize + slotSpace) * slotColumns + slotSpace + 20;
			Height = (slotSize + slotSpace) * slotRows + slotSpace + 20;//300;

			int numOfSlots = slotRows * slotColumns;

			for (int i = 0; i < slots.Length; i++)
			{
				slots[i] = new Slot(0);
				Slot slot = slots[i];
				int x = i % slotColumns;
				int y = i / slotColumns;
				slot.X = slotSpace + x * (slot.Width + slotSpace);
				slot.Y = slotSpace + y * (slot.Height + slotSpace);
			}
		}

		public void RepopulateSlots()
		{
			ClearContent();
			for (int i = 0; i < slots.Length; i++)
			{
				Slot slot = slots[i];
				if (i < Items.Length)
				{
					slot.Visible = true;
					slot.item = Items[i];
					this.ContentHeight = slot.Y + slot.Height + Spacing;
					AddChild(slot);
				}
				else
				{
					slots[i].Visible = false;
				}
			}
		}
	}
}