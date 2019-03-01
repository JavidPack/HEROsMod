using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;

namespace HEROsMod.HEROsModNetwork
{
	public class Region
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public int X { get; set; }
		public int Y { get; set; }

		public Vector2 Position
		{
			get { return new Vector2(X, Y); }
			set
			{
				X = (int)value.X;
				Y = (int)value.Y;
			}
		}

		public int Width { get; set; }
		public int Height { get; set; }

		public Vector2 Size
		{
			get { return new Vector2(Width, Height); }
			set
			{
				Width = (int)value.X;
				Height = (int)value.Y;
			}
		}

		public bool ChestsProtected { get; set; }
		public List<int> AllowedPlayersIDs { get; set; }
		public List<int> AllowedGroupsIDs { get; set; }

		public Color Color { get; set; }

		public Region(string name, int x, int y, int width, int height, bool chestsprotected = false)
		{
			this.Name = name;
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
			this.ID = -1;
			this.ChestsProtected = chestsprotected;
			AllowedPlayersIDs = new List<int>();
			AllowedGroupsIDs = new List<int>();
			this.Color = GetRandomColor();
		}

		public Region(string name, Vector2 position, Vector2 size)
		{
			this.Name = name;
			this.Position = position;
			this.Size = size;
			this.ID = -1;
			this.ChestsProtected = false;
			AllowedPlayersIDs = new List<int>();
			AllowedGroupsIDs = new List<int>();
			this.Color = GetRandomColor();
		}

		private Color GetRandomColor()
		{
			int num = Main.rand.Next() % 1000;
			float hue = (float)num / 1000;
			return Main.hslToRgb(hue, 1f, .5f);
		}

		public bool ContainsTile(int x, int y)
		{
			return x >= this.X && x < this.X + this.Width && y >= this.Y && y < this.Y + this.Height;
		}

		public bool AddPlayer(int playerIndex)
		{
			for (int i = 0; i < AllowedPlayersIDs.Count; i++)
			{
				if (AllowedPlayersIDs[i] == playerIndex)
				{
					return false;
				}
			}
			AllowedPlayersIDs.Add(playerIndex);
			return true;
		}

		public bool RemovePlayer(int playerIndex)
		{
			bool result = false;
			for (int i = 0; i < AllowedPlayersIDs.Count; i++)
			{
				if (AllowedPlayersIDs[i] == playerIndex)
				{
					AllowedPlayersIDs.RemoveAt(i);
					i--;
					result = true;
				}
			}
			return result;
		}

		public bool AddGroup(int groupIndex)
		{
			for (int i = 0; i < AllowedGroupsIDs.Count; i++)
			{
				if (AllowedGroupsIDs[i] == groupIndex)
				{
					return false;
				}
			}
			AllowedGroupsIDs.Add(groupIndex);
			return true;
		}

		public bool RemoveGroup(int groupIndex)
		{
			bool result = false;
			for (int i = 0; i < AllowedGroupsIDs.Count; i++)
			{
				if (AllowedGroupsIDs[i] == groupIndex)
				{
					AllowedGroupsIDs.RemoveAt(i);
					i--;
					result = true;
				}
			}
			return result;
		}

		public byte[] Export()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(memoryStream))
				{
					writer.Write(Name);
					writer.Write(ID);
					writer.WriteVector2(Position);
					writer.WriteVector2(Size);

					writer.Write(AllowedPlayersIDs.Count);
					for (int j = 0; j < AllowedPlayersIDs.Count; j++)
					{
						writer.Write(AllowedPlayersIDs[j]);
					}
					writer.Write(AllowedGroupsIDs.Count);
					for (int j = 0; j < AllowedGroupsIDs.Count; j++)
					{
						writer.Write(AllowedGroupsIDs[j]);
					}
					writer.WriteRGB(Color);
					writer.Write(ChestsProtected);
					writer.Close();
					memoryStream.Close();
					return memoryStream.ToArray();
				}
			}
		}

		public static Region GetRegionFromBinaryReader(ref BinaryReader reader)
		{
			string name = reader.ReadString();
			int id = reader.ReadInt32();
			Vector2 position = reader.ReadVector2();
			Vector2 size = reader.ReadVector2();
			Region region = new Region(name, position, size);
			region.ID = id;

			int numberOfAllowPlayers = reader.ReadInt32();
			for (int j = 0; j < numberOfAllowPlayers; j++)
			{
				region.AllowedPlayersIDs.Add(reader.ReadInt32());
			}
			int numberOfAllowGroups = reader.ReadInt32();
			for (int j = 0; j < numberOfAllowGroups; j++)
			{
				region.AllowedGroupsIDs.Add(reader.ReadInt32());
			}
			region.Color = reader.ReadRGB();
			region.ChestsProtected = reader.ReadBoolean();
			return region;
		}

		public byte[] ExportPermissions()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(memoryStream))
				{
					writer.Write(AllowedPlayersIDs.Count);
					for (int j = 0; j < AllowedPlayersIDs.Count; j++)
					{
						writer.Write(AllowedPlayersIDs[j]);
					}
					writer.Write(AllowedGroupsIDs.Count);
					for (int j = 0; j < AllowedGroupsIDs.Count; j++)
					{
						writer.Write(AllowedGroupsIDs[j]);
					}
					writer.Close();
					memoryStream.Close();
					return memoryStream.ToArray();
				}
			}
		}

		//public void ImportPermissions(byte[] importData)
		//{
		//	using (MemoryStream memoryStream = new MemoryStream(importData))
		//	{
		//		using (BinaryReader reader = new BinaryReader(memoryStream))
		//		{
		//			int numberOfAllowPlayers = reader.ReadInt32();
		//			for (int j = 0; j < numberOfAllowPlayers; j++)
		//			{
		//				this.AllowedPlayersIDs.Add(reader.ReadInt32());
		//			}
		//			int numberOfAllowGroups = reader.ReadInt32();
		//			for (int j = 0; j < numberOfAllowGroups; j++)
		//			{
		//				this.AllowedGroupsIDs.Add(reader.ReadInt32());
		//			}
		//			reader.Close();
		//			memoryStream.Close();
		//		}
		//	}
		//}

		internal void ImportPermissions(int[] permissionsGroups, int[] permissionsPlayers)
		{
			for (int j = 0; j < permissionsPlayers.Length; j++)
			{
				this.AllowedPlayersIDs.Add(permissionsPlayers[j]);
			}
			for (int j = 0; j < permissionsGroups.Length; j++)
			{
				this.AllowedGroupsIDs.Add(permissionsGroups[j]);
			}
		}
	}
}