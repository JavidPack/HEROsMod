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
			new PermissionInfo("ModifyTerrain", HEROsMod.HeroText("PermissionInfo.ModifyTerrain")),
			new PermissionInfo("InfiniteReach", HEROsMod.HeroText("PermissionInfo.InfiniteReach")),
			new PermissionInfo("ItemBrowser", HEROsMod.HeroText("PermissionInfo.ItemBrowser")),
			new PermissionInfo("ChangeTimeWeather", HEROsMod.HeroText("PermissionInfo.ChangeTimeWeather")),
            //new PermissionInfo("ChangeTime", "Change Time"),
            new PermissionInfo("ToggleEnemies", HEROsMod.HeroText("PermissionInfo.ToggleEnemies")),
			new PermissionInfo("Flycam", HEROsMod.HeroText("PermissionInfo.Flycam")),
			new PermissionInfo("ClearItems", HEROsMod.HeroText("PermissionInfo.ClearItems")),
			new PermissionInfo("RevealMap", HEROsMod.HeroText("PermissionInfo.RevealMap")),
			new PermissionInfo("LightHack", HEROsMod.HeroText("PermissionInfo.LightHack")),
			new PermissionInfo("SpawnNPCs", HEROsMod.HeroText("PermissionInfo.SpawnNPCs")),
			new PermissionInfo("SpawnBosses", HEROsMod.HeroText("PermissionInfo.SpawnBosses")),
			//new PermissionInfo("SpawnBeatenBosses", HEROsMod.HeroText("PermissionInfo.SpawnBeatenBosses")),
			new PermissionInfo("Kick", HEROsMod.HeroText("PermissionInfo.Kick")),
			new PermissionInfo("Ban", HEROsMod.HeroText("PermissionInfo.Ban")),
			new PermissionInfo("TeleportToPlayers", HEROsMod.HeroText("PermissionInfo.TeleportToPlayers")),
			new PermissionInfo("Snoop", HEROsMod.HeroText("PermissionInfo.Snoop")),
            //new PermissionInfo("ControlWeather", "Control Weather"),
            new PermissionInfo("EditWaypoints", HEROsMod.HeroText("PermissionInfo.EditWaypoints")),
			new PermissionInfo("AccessWaypoints", HEROsMod.HeroText("PermissionInfo.AccessWaypoints")),
            new PermissionInfo("ToggleGravestones", HEROsMod.HeroText("PermissionInfo.ToggleGravestones")),
			new PermissionInfo("CanUseBuffs", HEROsMod.HeroText("PermissionInfo.CanUseBuffs")),
			//new PermissionInfo("ToggleHardmodeEnemies", "Toggle Hardmode Enemies"),
			new PermissionInfo("GodMode", HEROsMod.HeroText("PermissionInfo.GodMode")),
			new PermissionInfo("PrefixEditor", HEROsMod.HeroText("PermissionInfo.PrefixEditor")),
			//new PermissionInfo("StartEvents", "Start Events"),
			new PermissionInfo("ToggleBannedItems", HEROsMod.HeroText("PermissionInfo.ToggleBannedItems")),
			new PermissionInfo("Teleport", HEROsMod.HeroText("PermissionInfo.Teleport")),
			new PermissionInfo("CheckTiles", HEROsMod.HeroText("PermissionInfo.CheckTiles")),
			new PermissionInfo("ViewRegions", HEROsMod.HeroText("PermissionInfo.ViewRegions")),
			new PermissionInfo("EditRegions", HEROsMod.HeroText("PermissionInfo.EditRegions")),
			new PermissionInfo("EditServerConfigs", HEROsMod.HeroText("PermissionInfo.EditServerConfigs")), // of other mods.
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
			// Remember: add new default permissions to DoMigrations when added
			Permissions["SpawnBosses"] = true;
			// Permissions["SpawnBeatenBosses"] = true;

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