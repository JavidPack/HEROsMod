using System;
using System.Collections.Generic;

using Terraria;

namespace HEROsMod.HEROsModNetwork
{
	internal class TileChangeController
	{
		public static Dictionary<int, TileChange[,]> TileChanges { get; set; }

		public static void Init()
		{
			TileChanges = new Dictionary<int, TileChange[,]>();
		}

		public static void RecordChanges(HEROsModPlayer player, int x, int y)
		{
			if (player.ID < 0) return;
			if (!TileChanges.ContainsKey(player.ID))
			{
				TileChanges.Add(player.ID, new TileChange[Main.maxTilesX, Main.maxTilesY]);
			}
			if (TileChanges[player.ID][x, y] == null)
			{
				TileChanges[player.ID][x, y] = new TileChange(Main.tile[x, y]);
			}
		}

		public static void RestoreTileChangesMadeByPlayer(int playerID)
		{
			if (!TileChanges.ContainsKey(playerID)) return;
			TileChange[,] changes = TileChanges[playerID];
			for (int y = 0; y < changes.GetLength(1); y++)
			{
				for (int x = 0; x < changes.GetLength(0); x++)
				{
					if (changes[x, y] != null)
					{
						Tile tile = Main.tile[x, y];
						Tile backupTile = changes[x, y].TilePreviousToChange;

						// TODO: Make my own Tile class to store data, since current Tile instance will always point to current data.
						if (backupTile != null /*&& !backupTile.IsTheSameAs(tile)*/) 
						{
							tile.CopyFrom(backupTile);
							//NetMessage.SendData(20, -1, -1, "", 1, x, y, 0f, 0);
						}
					}
				}
			}
			changes = new TileChange[Main.maxTilesX, Main.maxTilesY];
			for (int i = 0; i < Main.player.Length; i++)
			{
				if (Main.player[i].active)
				{
					Network.ResendPlayerTileData(Network.Players[i]);
				}
			}
		}
	}

	internal class TileChange
	{
		public DateTime time { get; set; }

		public Tile TilePreviousToChange { get; set; }

		public TileChange(Tile tile)
		{
			time = DateTime.Now;
			TilePreviousToChange = new Tile();
			TilePreviousToChange.CopyFrom(tile);
		}
	}
}