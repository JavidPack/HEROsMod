using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HEROsMod.HEROsModNetwork
{
	public class Group
	{
		// TODO, move these to the Service?
		public static List<PermissionInfo> PermissionList = new List<PermissionInfo>();

		public static PermissionInfo[] DefaultPermissions = new PermissionInfo[]
		{
			new PermissionInfo("ModifyTerrain", "Modify Terrain"),
			new PermissionInfo("InfiniteReach", "Infinite Block Reach"),
			new PermissionInfo("ItemBrowser", "Item Browser"),
			new PermissionInfo("ChangeTimeWeather", "Change Time/Weather"),
            //new PermissionInfo("ChangeTime", "Change Time"),
            new PermissionInfo("ToggleEnemies", "Toggle Enemies"),
			new PermissionInfo("Flycam", "Fly Camera"),
			new PermissionInfo("ClearItems", "Clear Items on Ground"),
			new PermissionInfo("RevealMap", "Reveal Map"),
			new PermissionInfo("LightHack", "Light Hack"),
			new PermissionInfo("SpawnNPCs", "Spawn NPCs"),
			new PermissionInfo("Kick", "Kick Players"),
			new PermissionInfo("Ban", "Ban Players"),
			new PermissionInfo("TeleportToPlayers", "Teleport To Players"),
			new PermissionInfo("Snoop", "Snoop Player Inventories"),
            //new PermissionInfo("ControlWeather", "Control Weather"),
            new PermissionInfo("EditWaypoints", "Edit Waypoints"),
			new PermissionInfo("AccessWaypoints", "Access Wayppoints"),
            //new PermissionInfo("StartCTF", "Start CTF Match"),
            new PermissionInfo("ToggleGravestones", "Toggle Gravestones"),
			new PermissionInfo("CanUseBuffs", "Open Buff Window"),
			//new PermissionInfo("ToggleHardmodeEnemies", "Toggle Hardmode Enemies"),
			new PermissionInfo("GodMode", "God Mode"),
			new PermissionInfo("PrefixEditor", "Prefix Editor"),
			//new PermissionInfo("StartEvents", "Start Events"),
			new PermissionInfo("ToggleBannedItems", "Toggle Banned Items"),
			new PermissionInfo("Teleport", "Teleport"),
			new PermissionInfo("CheckTiles", "Check Tiles"),
			new PermissionInfo("ViewRegions", "View Regions"),
			new PermissionInfo("EditRegions", "Edit Regions"),
		};

		private bool _isAdmin = false;
		private string _name = String.Empty;
		public int ID { get; set; }
		public Dictionary<string, bool> Permissions { get; set; }

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public bool IsAdmin
		{
			get { return _isAdmin; }
			set { _isAdmin = value; }
		}

		public Color Color { get; set; }

		public Group(string name)
		{
			ID = -1;
			this._name = name;
			Permissions = new Dictionary<string, bool>();
			foreach (var p in PermissionList)
			{
				Permissions.Add(p.Key, false);
			}
			Permissions["ModifyTerrain"] = true;

			if (name == "Default")
			{
				Network.DefaultGroup = this;
			}
			this.Color = new Color(255, 255, 255);
		}

		public bool HasPermission(string permissionName)
		{
			if (Permissions.ContainsKey(permissionName))
			{
				if (Permissions[permissionName]) return true;
			}
			return false;
		}

		public byte[] ExportPermissions()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(memoryStream))
				{
					int numberOfPermissions = 0;
					List<string> permissions = new List<string>();
					foreach (KeyValuePair<string, bool> entry in Permissions)
					{
						if (entry.Value)
						{
							numberOfPermissions++;
							permissions.Add(entry.Key);
						}
					}
					writer.Write(numberOfPermissions);
					foreach (var p in permissions)
					{
						writer.Write(p);
					}
					writer.Close();
					memoryStream.Close();
					return memoryStream.ToArray();
				}
			}
		}

		public void RemoveAllPermissions()
		{
			for (int i = 0; i < Permissions.Count; i++)
			{
				var entry = Permissions.ElementAt(i);
				Permissions[entry.Key] = false;
			}
		}

		public void ImportPermissions(byte[] data)
		{
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				using (BinaryReader reader = new BinaryReader(memoryStream))
				{
					RemoveAllPermissions();
					int numberOfPermessions = reader.ReadInt32();
					for (int i = 0; i < numberOfPermessions; i++)
					{
						string permissionName = reader.ReadString();
						if (Permissions.ContainsKey(permissionName))
						{
							Permissions[permissionName] = true;
						}
					}
				}
			}
		}

		internal void ImportPermissions(string[] permissions)
		{
			RemoveAllPermissions();
			foreach (string p in permissions)
			{
				if (Permissions.ContainsKey(p))
				{
					Permissions[p] = true;
				}
				else
				{
					ModUtils.DebugText("Warning: Permission " + p + " not found during ImportPermissions");
				}
			}
		}

		public void MakeAdmin()
		{
			this.ID = -1;
			this._isAdmin = true;
			for (int i = 0; i < Permissions.Count; i++)
			{
				var entry = Permissions.ElementAt(i);
				Permissions[entry.Key] = true;
			}
		}
	}

	public class PermissionInfo
	{
		public string Key { get; set; }
		public string Description { get; set; }

		public PermissionInfo(string key, string description)
		{
			this.Key = key;
			this.Description = description;
		}
	}
}