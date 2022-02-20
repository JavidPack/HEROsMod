using HEROsMod.HEROsModServices;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Terraria;

// TODO, save position of ui windows, save separate client json

namespace HEROsMod.HEROsModNetwork
{
	// Database should only be on the server or single. Data should be accessed from their respective member variables.
	public class HEROsModDatabase
	{
		// Global to the Server config
		public List<DatabasePlayer> players;

		public List<DatabaseGroup> groups;

		// Specific to world
		public List<DatabaseWorld> worlds;

		//public List<DatabaseRegion> regions;
		//public bool BanDestructiveExplosives;
	}

	public class DatabasePlayer
	{
		public int ID;
		public string username;
		public string password;
		public int group;
	}

	public class DatabaseGroup
	{
		public int ID;
		public string name;
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		[DefaultValue(typeof(Color), "255, 255, 255, 255")]
		public Color color;

		//public byte[] permissions;
		public string[] permissions;
	}

	public class DatabaseWorld
	{
		public int worldID;
		public string name;
		public bool BanDestructiveExplosives;
		public bool TimePaused;
		public double TimePausedTime;

		//public bool TimePausedIsDay;
		public bool GraveStonesDisabled;

		public bool NPCSpawnsDiabled;
		public List<DatabaseRegion> regions = new List<DatabaseRegion>();
		public List<DatabaseWaypoint> waypoints = new List<DatabaseWaypoint>();
	}

	public class DatabaseRegion
	{
		public int ID;
		public string name;
		public int x;
		public int y;
		public int width;
		public int height;
		public bool chestsprotected;
		public Color color;

		//public byte[] permissions;
		public int[] permissionsGroups;

		public int[] permissionsPlayers;
		//public int world;
	}

	public class DatabaseWaypoint
	{
		public string name;
		public float x;
		public float y;
	}

	internal class DatabaseController
	{
		//private static SqliteConnection connection;
		//static int latestDatabaseVersion = 0;
		private static string jsonDatabaseFilename = "HEROsModDatabase";

		private static HEROsModDatabase database;
		private static DatabaseWorld currentDatabaseWorld;

		public static void LoadSetting(string settingName)
		{
			ModUtils.DebugText("LoadSetting");
			Directory.CreateDirectory(Main.SavePath);
			string path = string.Concat(new object[]
				{
					Main.SavePath,
					Path.DirectorySeparatorChar,
					settingName,
					".json"
				});
			if (File.Exists(path))
			{
				ModUtils.DebugText("LoadSetting File Exists");
				using (StreamReader r = new StreamReader(path))
				{
					string json = r.ReadToEnd();
					database = JsonConvert.DeserializeObject<HEROsModDatabase>(json);
				}
			}
			else
			{
				ModUtils.DebugText("LoadSetting File Doesn't Exist");
			}
			if (database == null)
			{
				ModUtils.DebugText("Warning: Database null in LoadSetting");
				database = new HEROsModDatabase();
			}
			if (database.players == null)
			{
				ModUtils.DebugText("Warning: Database players null in LoadSetting");
				database.players = new List<DatabasePlayer>();
			}
			if (database.worlds == null)
			{
				ModUtils.DebugText("Warning: Database worlds null in LoadSetting");
				database.worlds = new List<DatabaseWorld>();
			}
			//foreach (var world in database.worlds)
			//{
			//	if(world.worldID == Main.worldID)
			//	{
			//		currentDatabaseWorld = world;
			//	}
			//}
			//if(currentDatabaseWorld == null)
			//{
			//	currentDatabaseWorld = new DatabaseWorld() { worldID = Main.worldID };
			//}
			//if (database.regions == null)
			//{
			//	ModUtils.DebugText("Warning: Database regions null in LoadSetting");
			//	database.regions = new List<DatabaseRegion>();
			//}
			if (database.groups == null)
			{
				ModUtils.DebugText("Warning: Database groups null in LoadSetting");
				database.groups = new List<DatabaseGroup>();
			}
			//Console.WriteLine("LoadSetting End");
			//if (Main.netMode != (int)NetworkMode.Server)
			//{
			//	database.groups.Clear();
			//	database.players.Clear();
			//}
		}

		internal static void SaveSetting()
		{
			SaveSetting(jsonDatabaseFilename);
		}

		public static void SaveSetting(string settingName)
		{
			if (!Main.dedServ && Main.netMode == 2)
			{
				ModUtils.DebugText("WARNING: non ded client saving");
			}
			ModUtils.DebugText("SaveSetting");
			if (currentDatabaseWorld != null)
			{
				currentDatabaseWorld.waypoints.Clear();
				foreach (var waypoint in Waypoints.points)
				{
					currentDatabaseWorld.waypoints.Add(new DatabaseWaypoint() { name = waypoint.name, x = waypoint.position.X, y = waypoint.position.Y });
				}
				currentDatabaseWorld.GraveStonesDisabled = !Network.GravestonesAllowed;
				currentDatabaseWorld.BanDestructiveExplosives = ItemBanner.ItemsBanned;
				currentDatabaseWorld.NPCSpawnsDiabled = !EnemyToggler.EnemiesAllowed;
				currentDatabaseWorld.TimePaused = TimeWeatherChanger.TimePaused;
				if (currentDatabaseWorld.TimePaused)
				{
					currentDatabaseWorld.TimePausedTime = TimeWeatherChanger.PausedTime;
					//	currentDatabaseWorld.TimePausedIsDay = TimeWeatherChanger.PausedTimeDayTime;
				}
			}

			Directory.CreateDirectory(Main.SavePath);
			//DataContractSerializer serializer = new DataContractSerializer(typeof(HEROsModDatabase));
			string path = string.Concat(new object[]
				{
					Main.SavePath,
					Path.DirectorySeparatorChar,
					settingName,
					//".xml"
					".json"
				});
			//XmlWriter writer = XmlWriter.Create(path);
			//serializer.WriteObject(writer, database);
			//writer.Close();
			string json = JsonConvert.SerializeObject(database, Newtonsoft.Json.Formatting.Indented);
			File.WriteAllText(path, json);
		}

		public static void InitializeWorld()
		{
			foreach (var world in database.worlds)
			{
				if (world.worldID == Main.worldID)
				{
					currentDatabaseWorld = world;
				}
			}
			if (currentDatabaseWorld == null)
			{
				currentDatabaseWorld = new DatabaseWorld() { worldID = Main.worldID, name = Main.worldName };
				database.worlds.Add(currentDatabaseWorld);
				SaveSetting();
			}
			Waypoints.ClearPoints();
			foreach (var waypoint in currentDatabaseWorld.waypoints)
			{
				Waypoints.AddWaypoint(waypoint.name, new Vector2(waypoint.x, waypoint.y));
			}
			Network.GravestonesAllowed = !currentDatabaseWorld.GraveStonesDisabled;
			ItemBanner.ItemsBanned = currentDatabaseWorld.BanDestructiveExplosives;
			EnemyToggler.EnemiesAllowed = !currentDatabaseWorld.NPCSpawnsDiabled;
			TimeWeatherChanger.TimePaused = currentDatabaseWorld.TimePaused;
			if (TimeWeatherChanger.TimePaused)
			{
				TimeWeatherChanger.PausedTime = currentDatabaseWorld.TimePausedTime;
				//	TimeWeatherChanger.PausedTimeDayTime = currentDatabaseWorld.TimePausedIsDay;
			}
			if (Main.netMode == 0)
			{
				GeneralMessages.ProcessCurrentTogglesSP(EnemyToggler.EnemiesAllowed, Network.GravestonesAllowed, ItemBanner.ItemsBanned, TimeWeatherChanger.TimePaused);
			}
		}

		public static void Init()
		{
			ResetDatabase();
			// load in xml
			LoadSetting(jsonDatabaseFilename);
			if (!HasDefaultGroup())
			{
				Console.WriteLine("No Default group");
				Group defaultGroup = new Group("Default");
				AddGroup(ref defaultGroup);
			}
			//ItemBanner.ItemsBanned = database.BanDestructiveExplosives;

			//connection = new SqliteConnection("Data Source=HEROsModDatabase.s3db");
			//if (!TablesExits("Players"))
			//{
			//	Console.WriteLine("Creating Player Table");
			//	CreatePlayersTable();
			//}
			//if (!TablesExits("Groups"))
			//{
			//	Console.WriteLine("Creating Groups Table");
			//	CreateGroupsTable();
			//}
			//if (!TablesExits("Regions"))
			//{
			//	Console.WriteLine("Creating Regions Table");
			//	CreateRegionsTable();
			//}
			//if (!HasDefaultGroup())
			//{
			//	Console.WriteLine("No Default group");
			//	Group defaultGroup = new Group("Default");
			//	AddGroup(ref defaultGroup);
			//}
		}

		private static void ResetDatabase()
		{
			database = new HEROsModDatabase();
			database.players = new List<DatabasePlayer>();
			database.groups = new List<DatabaseGroup>();
			database.worlds = new List<DatabaseWorld>();
			currentDatabaseWorld = null;
			//database.regions = new List<DatabaseRegion>();
		}

		//public static bool TablesExits(string tableName)
		//{
		//	bool result = false;
		//	try
		//	{
		//		SqliteCommand cmd = new SqliteCommand("SELECT sql FROM sqlite_master WHERE tbl_name = '" + tableName + "' AND type = 'table'", connection);
		//		connection.Open();
		//		var myReader = cmd.ExecuteReader();
		//		if (myReader.FieldCount > 0 && myReader[0].ToString().Length > 0) result = true;
		//		connection.Close();
		//	}
		//	finally { }
		//	return result;
		//}

		//public static void CreatePlayersTable()
		//{
		//	try
		//	{
		//		SqliteCommand cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS [Players] (" +
		//			"[id] INTEGER  NOT NULL PRIMARY KEY," +
		//			"[username] VARCHAR(20)  NOT NULL," +
		//			"[password] VARCHAR(20)  NOT NULL," +
		//			"[group] INTEGER DEFAULT '0' NOT NULL" +
		//			")", connection);
		//		connection.Open();
		//		var myReader = cmd.ExecuteScalar();
		//	}
		//	finally { connection.Close(); }
		//}

		//public static void CreateGroupsTable()
		//{
		//	try
		//	{
		//		SqliteCommand cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS [Groups] (" +
		//			"[id] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL," +
		//			"[name] VARCHAR(20) NOT NULL," +
		//			"[permissions] VARCHAR)", connection);
		//		connection.Open();
		//		var myReader = cmd.ExecuteScalar();
		//	}
		//	finally { connection.Close(); }
		//}

		//public static void CreateRegionsTable()
		//{
		//	try
		//	{
		//		SqliteCommand cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS [Regions] (" +
		//			"[id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
		//			"[name] VARCHAR(20) NOT NULL," +
		//			"[x] INTEGER," +
		//			"[y] INTEGER," +
		//			"[width] INTEGER," +
		//			"[height] INTEGER," +
		//			"[color] INTEGER," +
		//			"[permissions] VARCHAR," +
		//			"[world] INTEGER NOT NULL)", connection);
		//		connection.Open();
		//		var myReader = cmd.ExecuteScalar();
		//	}
		//	finally { connection.Close(); }
		//}

		public static bool Login(ref string username, string password, ref int playerID, ref int groupID)
		{
			foreach (var user in database.players)
			{
				if (user.username.ToLower() == username.ToLower() && user.password == password)
				{
					username = user.username;
					playerID = user.ID;
					groupID = user.group;
					return true;
				}
			}
			return false;
		}

		public static RegistrationResult Register(string username, string password)
		{
			if (database.players.Any(x => x.username == username.ToLower()))
			{
				return RegistrationResult.UsernameTaken;
			}
			if (database.players.Count == 0)
			{
				database.players.Add(
					new DatabasePlayer() { username = username, password = password, ID = GetAvailablePlayerID(), group = -1 }
				);
			}
			else
			{
				database.players.Add(
					new DatabasePlayer() { username = username, password = password, ID = GetAvailablePlayerID() }
				);
			}
			SaveSetting(jsonDatabaseFilename);
			return RegistrationResult.Sucess;
		}

		private static int GetAvailablePlayerID()
		{
			int next = 0;
			foreach (var item in database.players)
			{
				if (item.ID >= next)
				{
					next = item.ID + 1;
				}
			}
			return next;
		}

		private static int GetAvailableGroupID()
		{
			int next = 0;
			foreach (var item in database.groups)
			{
				if (item.ID >= next)
				{
					next = item.ID + 1;
				}
			}
			return next;
		}

		private static int GetAvailableRegionID()
		{
			int next = 0;
			foreach (var item in currentDatabaseWorld.regions)
			{
				if (item.ID >= next)
				{
					next = item.ID + 1;
				}
			}
			return next;
		}

		public static UserWithID[] GetRegisteredUsers()
		{
			//List<UserWithID> result = new List<UserWithID>();
			return database.players.Select((x) => new UserWithID() { Username = x.username, ID = x.ID, groupID = x.group }).ToArray();
		}

		public static void SetPlayerGroup(int playerID, int groupID)
		{
			DatabasePlayer p = database.players.Where(x => x.ID == playerID).FirstOrDefault();
			if (p != null)
			{
				p.group = groupID;
			}
			SaveSetting(jsonDatabaseFilename);
		}

		private static bool HasDefaultGroup()
		{
			return database.groups.Any(x => x.name == "Default");
		}

		public static void AddGroup(ref Group group)
		{
			int newid = GetAvailableGroupID();
			DatabaseGroup newGroup = new DatabaseGroup() { name = group.Name, ID = newid, color = group.Color };
			database.groups.Add(newGroup);

			group.ID = newid;
			SetGroupPermissions(group);
			SaveSetting(jsonDatabaseFilename);
		}

		public static void DeleteGroup(Group group)
		{
			DatabaseGroup databaseGroup = database.groups.Where(x => x.ID == group.ID).FirstOrDefault();
			if (databaseGroup != null)
			{
				foreach (var player in database.players)
				{
					if (player.group == databaseGroup.ID)
					{
						player.group = Network.DefaultGroup.ID;
					}
				}
				database.groups.Remove(databaseGroup);
			}
			SaveSetting(jsonDatabaseFilename);
		}

		public static void SetGroupPermissions(Group group)
		{
			DatabaseGroup g = database.groups.Where(x => group.Name == x.name).FirstOrDefault();
			if (g != null)
			{
				g.permissions = group.Permissions.Where(x => x.Value).Select(x => x.Key).ToArray();//group.ExportPermissions();
				g.color = group.Color;
			}
			SaveSetting(jsonDatabaseFilename);
		}

		public static List<Group> GetGroups()
		{
			List<Group> result = new List<Group>();
			foreach (DatabaseGroup dbGroup in database.groups)
			{
				Group group = new Group(dbGroup.name);
				group.ID = dbGroup.ID;
				group.Color = dbGroup.color;
				group.ImportPermissions(dbGroup.permissions);
				result.Add(group);
			}
			return result;
		}

		public static List<Region> GetRegions()
		{
			List<Region> result = new List<Region>();
			foreach (DatabaseRegion dbRegion in currentDatabaseWorld.regions)
			{
				//if (dbRegion.world != Main.worldID) continue;
				Region region = new Region(dbRegion.name, dbRegion.x, dbRegion.y, dbRegion.width, dbRegion.height, dbRegion.chestsprotected);
				region.ImportPermissions(dbRegion.permissionsGroups, dbRegion.permissionsPlayers);
				region.ID = dbRegion.ID;

				region.Color = dbRegion.color;
				result.Add(region);
			}
			return result;
		}

		public static void AddRegion(ref Region region)
		{
			int newid = GetAvailableRegionID();

			DatabaseRegion dbRegion = new DatabaseRegion();
			dbRegion.ID = newid;
			region.ID = dbRegion.ID;
			dbRegion.name = region.Name;
			dbRegion.x = region.X;
			dbRegion.y = region.Y;
			dbRegion.width = region.Width;
			dbRegion.height = region.Height;
			dbRegion.chestsprotected = region.ChestsProtected;
			//byte[] colorData = new byte[] { region.Color.R, region.Color.G, region.Color.B, region.Color.A };
			//int colorNum = BitConverter.ToInt32(colorData, 0);
			//dbRegion.color = colorNum;
			dbRegion.color = region.Color;
			//dbRegion.permissions = region.ExportPermissions();
			dbRegion.permissionsPlayers = region.AllowedPlayersIDs.ToArray();
			dbRegion.permissionsGroups = region.AllowedGroupsIDs.ToArray();
			//dbRegion.world = Main.worldID;
			currentDatabaseWorld.regions.Add(dbRegion);
			SaveSetting(jsonDatabaseFilename);
		}

		public static void WriteRegionPermissions(Region region)
		{
			DatabaseRegion r = currentDatabaseWorld.regions.Where(x => region.ID == x.ID).FirstOrDefault();
			if (r != null)
			{
				//r.permissions = region.ExportPermissions();
				r.permissionsPlayers = region.AllowedPlayersIDs.ToArray();
				r.permissionsGroups = region.AllowedGroupsIDs.ToArray();
			}
			SaveSetting(jsonDatabaseFilename);
		}

		public static void WriteRegionColor(Region region)
		{
			DatabaseRegion r = currentDatabaseWorld.regions.Where(x => region.ID == x.ID).FirstOrDefault();
			if (r != null)
			{
				r.color = region.Color;
			}
			SaveSetting(jsonDatabaseFilename);
		}

		public static void WriteRegionChestProtection(Region region)
		{
			DatabaseRegion r = currentDatabaseWorld.regions.Where(x => region.ID == x.ID).FirstOrDefault();
			if (r != null)
			{
				r.chestsprotected = region.ChestsProtected;
			}
			SaveSetting(jsonDatabaseFilename);
		}

		public static void RemoveRegion(Region region)
		{
			DatabaseRegion databaseRegion = currentDatabaseWorld.regions.Where(x => x.ID == region.ID).FirstOrDefault();
			if (databaseRegion != null)
			{
				currentDatabaseWorld.regions.Remove(databaseRegion);
			}
			SaveSetting(jsonDatabaseFilename);
		}

		public enum RegistrationResult
		{
			Error,
			UsernameTaken,
			Sucess
		}
	}
}

//try
//{
//	SqliteCommand cmd = new SqliteCommand("SELECT * from Players WHERE username='" + username.ToLower() + "'", connection);
//	connection.Open();
//	var myreader = cmd.ExecuteReader();
//	while (myreader.Read())
//	{
//		if ((string)myreader["password"] == password)
//		{
//			username = (string)myreader["username"];
//			playerID = (int)((long)myreader["id"]);
//			groupID = (int)((long)myreader["group"]);
//			return true;
//		}
//	}
//}
//finally { connection.Close(); }
//return false;

//RegistrationResult result = RegistrationResult.Sucess;
//try
//{
//	SqliteCommand cmd2 = new SqliteCommand("SELECT * from Players WHERE username='" + username.ToLower() + "'", connection);
//	SqliteCommand cmd = new SqliteCommand("INSERT INTO Players(username, password) " +
//									"values('" + username.ToLower() + "','" + password + "')", connection);
//	connection.Open();
//	var myreader = cmd2.ExecuteReader();
//	while (myreader.Read())
//	{
//		if ((string)myreader["username"] == username.ToLower())
//		{
//			connection.Close();
//			return RegistrationResult.UsernameTaken;
//		}
//	}
//	connection.Close();
//	connection.Open();
//	cmd.ExecuteNonQuery();
//}
//catch
//{
//	result = RegistrationResult.Error;
//}
//finally { connection.Close(); }
//return result;
//try
//{
//	SqliteCommand cmd = new SqliteCommand("SELECT * FROM Players", connection);

//	connection.Open();
//	var myreader = cmd.ExecuteReader();
//	while (myreader.Read())
//	{
//		string name = (string)myreader["username"];
//		int id = (int)((long)myreader["id"]);
//		UserWithID player = new UserWithID();
//		player.Username = name;
//		player.ID = id;
//		result.Add(player);
//	}
//}
//finally { connection.Close(); }
//return result.ToArray();
//byte[] colorData = new byte[] { region.Color.R, region.Color.G, region.Color.B, region.Color.A };
//int colorNum = BitConverter.ToInt32(colorData, 0);
//r.color = colorNum;
//try
//{
//	SqliteCommand cmd = new SqliteCommand("UPDATE Players SET [id]='" + Network.DefaultGroup.ID + "' WHERE [id]='" + group.ID + "'", connection);
//	connection.Open();

//	cmd.CommandText = "DELETE FROM Groups WHERE id='" + group.ID + "'";
//	cmd.ExecuteNonQuery();
//}
//finally { connection.Close(); }
//byte[] colorData = BitConverter.GetBytes(dbRegion.color);
//Color regionColor = new Color();
//regionColor.R = colorData[0];
//regionColor.G = colorData[1];
//regionColor.B = colorData[2];
//regionColor.A = colorData[3];
//region.Color = regionColor;
//try
//{
//	SqliteCommand cmd = new SqliteCommand("UPDATE Players SET [group]='" + groupID + "' WHERE [id]='" + playerID + "'"
//		, connection);
//	connection.Open();
//	cmd.ExecuteNonQuery();
//}
//finally { connection.Close(); }
//try
//{
//	SqliteCommand cmd = new SqliteCommand("SELECT * from Groups WHERE name='Default'", connection);
//	connection.Open();
//	var myreader = cmd.ExecuteReader();
//	while (myreader.Read())
//	{
//		if (myreader["name"] != DBNull.Value)
//		{
//			return true;
//		}
//	}
//}
//finally { connection.Close(); }
//return false;

//int result = 0;
//try
//{
//	SqliteCommand cmd = new SqliteCommand("INSERT INTO Groups(name) " +
//									"values('" + group.Name + "')", connection);
//	connection.Open();
//	result = cmd.ExecuteNonQuery();

//	cmd.CommandText = "SELECT * FROM Groups WHERE name = '" + group.Name + "'";
//	var reader = cmd.ExecuteReader();
//	while (reader.Read())
//	{
//		group.ID = (int)((long)reader["id"]);
//	}
//}
//catch (Exception)
//{
//	throw;
//}
//finally
//{
//	connection.Close();
//	SetGroupPermissions(group);
//}
//try
//{
//	SqliteCommand cmd = new SqliteCommand("UPDATE Groups SET [permissions]='" + Convert.ToBase64String(group.ExportPermissions()) + "' WHERE [name]='" + group.Name + "'", connection);

//	connection.Open();
//	cmd.ExecuteNonQuery();
//}
//finally { connection.Close(); }
//List<Group> result = new List<Group>();
//try
//{
//	connection.Open();
//	SqliteCommand cmd = new SqliteCommand("SELECT * FROM Groups", connection);

//	var myreader = cmd.ExecuteReader();
//	while (myreader.Read())
//	{
//		string name = (string)myreader["name"];
//		Group group = new Group(name);
//		group.ID = (int)((long)myreader["id"]);
//		string permissionsStr = (string)myreader["permissions"];
//		group.ImportPermissions(Convert.FromBase64String(permissionsStr));
//		result.Add(group);
//	}
//}
//finally { connection.Close(); }
//return result;
//try
//{
//	connection.Open();
//	SqliteCommand cmd = new SqliteCommand("SELECT * FROM Regions WHERE world = '" + Main.worldID + "'", connection);

//	var myreader = cmd.ExecuteReader();
//	while (myreader.Read())
//	{
//		string name = (string)myreader["name"];
//		int id = (int)((long)myreader["id"]);
//		int x = (int)((long)myreader["x"]);
//		int y = (int)((long)myreader["y"]);
//		int width = (int)((long)myreader["width"]);
//		int height = (int)((long)myreader["height"]);
//		string permissionsStr = string.Empty;
//		if (myreader["permissions"] != DBNull.Value)
//		{
//			permissionsStr = (string)myreader["permissions"];
//		}

//		Region region = new Region(name, x, y, width, height);
//		region.ID = id;
//		if (permissionsStr.Length > 0)
//		{
//			byte[] permissionData = Convert.FromBase64String(permissionsStr);
//			region.ImportPermissions(permissionData);
//		}
//		if (myreader["color"] != DBNull.Value)
//		{
//			int colorNum = (int)((long)myreader["color"]);
//			byte[] colorData = BitConverter.GetBytes(colorNum);
//			Color regionColor = new Color();
//			regionColor.R = colorData[0];
//			regionColor.G = colorData[1];
//			regionColor.B = colorData[2];
//			regionColor.A = colorData[3];
//			region.Color = regionColor;
//		}

//		result.Add(region);
//	}
//}
//finally { connection.Close(); }
//return result;
//try
//{
//	byte[] colorData = new byte[] { region.Color.R, region.Color.G, region.Color.B, region.Color.A };
//	int colorNum = BitConverter.ToInt32(colorData, 0);
//	SqliteCommand cmd = new SqliteCommand("INSERT INTO Regions(name, x, y, width, height, color, world) " +
//									"values('" + region.Name + "'," +
//									region.X + "," +
//									region.Y + "," +
//									region.Width + "," +
//									region.Height + "," +
//									colorNum + "," +
//									Main.worldID +
//									")", connection);
//	connection.Open();
//	cmd.ExecuteNonQuery();

//	cmd.CommandText = "SELECT * FROM Regions WHERE name = '" + region.Name + "'";
//	var reader = cmd.ExecuteReader();
//	while (reader.Read())
//	{
//		region.ID = (int)((long)reader["id"]);
//	}
//}
//finally { connection.Close(); }

//try
//{
//	SqliteCommand cmd = new SqliteCommand("UPDATE Regions SET [permissions]='" + Convert.ToBase64String(region.ExportPermissions()) + "' WHERE [id]='" + region.ID + "'", connection);

//	connection.Open();
//	cmd.ExecuteNonQuery();
//}
//finally { connection.Close(); }
//try
//{
//	byte[] colorData = new byte[] { region.Color.R, region.Color.G, region.Color.B, region.Color.A };
//	int colorNum = BitConverter.ToInt32(colorData, 0);
//	SqliteCommand cmd = new SqliteCommand("UPDATE Regions SET [color]='" + colorNum + "' WHERE [id]='" + region.ID + "'", connection);

//	connection.Open();
//	cmd.ExecuteNonQuery();
//}
//finally { connection.Close(); }
//try
//{
//	SqliteCommand cmd = new SqliteCommand("DELETE FROM Regions WHERE id='" + region.ID + "'", connection);
//	connection.Open();
//	cmd.ExecuteNonQuery();
//}
//finally { connection.Close(); }

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using Mono.Data.Sqlite;
//using Microsoft.Xna.Framework;

//using Terraria;

//namespace HEROsModMod.HEROsModNetwork
//{
//    class DatabaseController
//    {
//        private static SqliteConnection connection;
//        static int latestDatabaseVersion = 0;

//        public static void Init()
//        {
//            connection = new SqliteConnection("Data Source=HEROsModDatabase.s3db");
//            if(!TablesExits("Players"))
//            {
//                Console.WriteLine("Creating Player Table");
//                CreatePlayersTable();
//            }
//            if (!TablesExits("Groups"))
//            {
//                Console.WriteLine("Creating Groups Table");
//                CreateGroupsTable();
//            }
//            if (!TablesExits("Regions"))
//            {
//                Console.WriteLine("Creating Regions Table");
//                CreateRegionsTable();
//            }
//            if(!HasDefaultGroup())
//            {
//                Console.WriteLine("No Default group");
//                Group defaultGroup = new Group("Default");
//                AddGroup(ref defaultGroup);

//            }
//        }

//        public static bool TablesExits(string tableName)
//        {
//            bool result = false;
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("SELECT sql FROM sqlite_master WHERE tbl_name = '" + tableName + "' AND type = 'table'", connection);
//                connection.Open();
//                var myReader = cmd.ExecuteReader();
//                if (myReader.FieldCount > 0 && myReader[0].ToString().Length > 0) result = true;
//                connection.Close();
//            }
//            finally {  }
//            return result;
//        }

//        public static void CreatePlayersTable()
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS [Players] (" +
//                    "[id] INTEGER  NOT NULL PRIMARY KEY," +
//                    "[username] VARCHAR(20)  NOT NULL," +
//                    "[password] VARCHAR(20)  NOT NULL," +
//                    "[group] INTEGER DEFAULT '0' NOT NULL" +
//                    ")", connection);
//                connection.Open();
//                var myReader = cmd.ExecuteScalar();
//            }
//            finally{ connection.Close(); }
//        }

//        public static void CreateGroupsTable()
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS [Groups] (" +
//                    "[id] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL," +
//                    "[name] VARCHAR(20) NOT NULL," +
//                    "[permissions] VARCHAR)", connection);
//                connection.Open();
//                var myReader = cmd.ExecuteScalar();
//            }
//            finally { connection.Close(); }
//        }

//        public static void CreateRegionsTable()
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("CREATE TABLE IF NOT EXISTS [Regions] (" +
//                    "[id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
//                    "[name] VARCHAR(20) NOT NULL," +
//                    "[x] INTEGER," +
//                    "[y] INTEGER," +
//                    "[width] INTEGER," +
//                    "[height] INTEGER," +
//                    "[color] INTEGER," +
//                    "[permissions] VARCHAR," +
//                    "[world] INTEGER NOT NULL)", connection);
//                connection.Open();
//                var myReader = cmd.ExecuteScalar();
//            }
//            finally { connection.Close(); }
//        }

//        public static bool Login(ref string username, string password, ref int playerID, ref int groupID)
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("SELECT * from Players WHERE username='" + username.ToLower() + "'", connection);
//                connection.Open();
//                var myreader = cmd.ExecuteReader();
//                while (myreader.Read())
//                {
//                    if ((string)myreader["password"] == password)
//                    {
//                        username = (string)myreader["username"];
//                        playerID = (int)((long)myreader["id"]);
//                        groupID = (int)((long)myreader["group"]);
//                        return true;
//                    }
//                }
//            }
//            finally { connection.Close(); }
//            return false;
//        }

//        public static RegistrationResult Register(string username, string password)
//        {
//            RegistrationResult result = RegistrationResult.Sucess;
//            try
//            {
//                SqliteCommand cmd2 = new SqliteCommand("SELECT * from Players WHERE username='" + username.ToLower() + "'", connection);
//                SqliteCommand cmd = new SqliteCommand("INSERT INTO Players(username, password) " +
//                                                "values('" + username.ToLower() + "','" + password + "')", connection);
//                connection.Open();
//                var myreader = cmd2.ExecuteReader();
//                while (myreader.Read())
//                {
//                    if ((string)myreader["username"] == username.ToLower())
//                    {
//                        connection.Close();
//                        return RegistrationResult.UsernameTaken;
//                    }
//                }
//                connection.Close();
//                connection.Open();
//                cmd.ExecuteNonQuery();
//            }
//            catch
//            {
//                result = RegistrationResult.Error;
//            }
//            finally { connection.Close(); }
//            return result;
//        }

//        public static UserWithID[] GetRegisteredUsers()
//        {
//            List<UserWithID> result = new List<UserWithID>();
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("SELECT * FROM Players", connection);

//                connection.Open();
//                var myreader = cmd.ExecuteReader();
//                while (myreader.Read())
//                {
//                    string name = (string)myreader["username"];
//                    int id = (int)((long)myreader["id"]);
//                    UserWithID player = new UserWithID();
//                    player.Username = name;
//                    player.ID = id;
//                    result.Add(player);
//                }
//            }
//            finally { connection.Close(); }
//            return result.ToArray();
//        }

//        public static void SetPlayerGroup(int playerID, int groupID)
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("UPDATE Players SET [group]='" + groupID + "' WHERE [id]='" + playerID + "'"
//                    , connection);
//                connection.Open();
//                cmd.ExecuteNonQuery();
//            }
//            finally { connection.Close(); }
//        }

//        private static bool HasDefaultGroup()
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("SELECT * from Groups WHERE name='Default'", connection);
//                connection.Open();
//                var myreader = cmd.ExecuteReader();
//                while (myreader.Read())
//                {
//                    if (myreader["name"] != DBNull.Value)
//                    {
//                        return true;
//                    }
//                }
//            }
//            finally { connection.Close(); }
//            return false;
//        }

//        public static void AddGroup(ref Group group)
//        {
//            int result = 0;
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("INSERT INTO Groups(name) " +
//                                                "values('" + group.Name + "')", connection);
//                connection.Open();
//                result = cmd.ExecuteNonQuery();

//                cmd.CommandText = "SELECT * FROM Groups WHERE name = '" + group.Name + "'";
//                var reader = cmd.ExecuteReader();
//                while(reader.Read())
//                {
//                    group.ID = (int)((long)reader["id"]);
//                }
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//            finally
//            {
//                connection.Close();
//                SetGroupPermissions(group);
//            }
//        }

//        public static void DeleteGroup(Group group)
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("UPDATE Players SET [id]='" + Network.DefaultGroup.ID + "' WHERE [id]='" + group.ID + "'", connection);
//                connection.Open();

//                cmd.CommandText = "DELETE FROM Groups WHERE id='" + group.ID + "'";
//                cmd.ExecuteNonQuery();
//            }
//            finally { connection.Close(); }
//        }

//        public static void SetGroupPermissions(Group group)
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("UPDATE Groups SET [permissions]='" + Convert.ToBase64String(group.ExportPermissions()) + "' WHERE [name]='" + group.Name + "'", connection);

//                connection.Open();
//                cmd.ExecuteNonQuery();
//            }
//            finally { connection.Close(); }
//        }

//        public static List<Group> GetGroups()
//        {
//            List<Group> result = new List<Group>();
//            try
//            {
//                connection.Open();
//                SqliteCommand cmd = new SqliteCommand("SELECT * FROM Groups", connection);

//                var myreader = cmd.ExecuteReader();
//                while (myreader.Read())
//                {
//                    string name = (string)myreader["name"];
//                    Group group = new Group(name);
//                    group.ID = (int)((long)myreader["id"]);
//                    string permissionsStr = (string)myreader["permissions"];
//                    group.ImportPermissions(Convert.FromBase64String(permissionsStr));
//                    result.Add(group);
//                }
//            }
//            finally { connection.Close(); }
//            return result;
//        }

//        public static List<Region> GetRegions()
//        {
//            List<Region> result = new List<Region>();
//            try
//            {
//                connection.Open();
//                SqliteCommand cmd = new SqliteCommand("SELECT * FROM Regions WHERE world = '" + Main.worldID + "'", connection);

//                var myreader = cmd.ExecuteReader();
//                while (myreader.Read())
//                {
//                    string name = (string)myreader["name"];
//                    int id = (int)((long)myreader["id"]);
//                    int x = (int)((long)myreader["x"]);
//                    int y = (int)((long)myreader["y"]);
//                    int width = (int)((long)myreader["width"]);
//                    int height = (int)((long)myreader["height"]);
//                    string permissionsStr = string.Empty;
//                    if (myreader["permissions"] != DBNull.Value)
//                    {
//                        permissionsStr = (string)myreader["permissions"];
//                    }

//                    Region region = new Region(name, x, y, width, height);
//                    region.ID = id;
//                    if (permissionsStr.Length > 0)
//                    {
//                        byte[] permissionData = Convert.FromBase64String(permissionsStr);
//                        region.ImportPermissions(permissionData);
//                    }
//                    if (myreader["color"] != DBNull.Value)
//                    {
//                        int colorNum = (int)((long)myreader["color"]);
//                        byte[] colorData = BitConverter.GetBytes(colorNum);
//                        Color regionColor = new Color();
//                        regionColor.R = colorData[0];
//                        regionColor.G = colorData[1];
//                        regionColor.B = colorData[2];
//                        regionColor.A = colorData[3];
//                        region.Color = regionColor;
//                    }

//                    result.Add(region);
//                }
//            }
//            finally { connection.Close(); }
//            return result;
//        }

//        public static void AddRegion(ref Region region)
//        {
//            try
//            {
//                byte[] colorData = new byte[] { region.Color.R, region.Color.G, region.Color.B, region.Color.A };
//                int colorNum = BitConverter.ToInt32(colorData, 0);
//                SqliteCommand cmd = new SqliteCommand("INSERT INTO Regions(name, x, y, width, height, color, world) " +
//                                                "values('" + region.Name + "'," +
//                                                region.X + "," +
//                                                region.Y + "," +
//                                                region.Width + "," +
//                                                region.Height + "," +
//                                                colorNum + "," +
//                                                Main.worldID +
//                                                ")", connection);
//                connection.Open();
//                cmd.ExecuteNonQuery();

//                cmd.CommandText = "SELECT * FROM Regions WHERE name = '" + region.Name + "'";
//                var reader = cmd.ExecuteReader();
//                while (reader.Read())
//                {
//                    region.ID = (int)((long)reader["id"]);
//                }
//            }
//            finally { connection.Close(); }
//        }

//        public static void WriteRegionPermissions(Region region)
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("UPDATE Regions SET [permissions]='" + Convert.ToBase64String(region.ExportPermissions()) + "' WHERE [id]='" + region.ID + "'", connection);

//                connection.Open();
//                cmd.ExecuteNonQuery();
//            }
//            finally { connection.Close(); }
//        }

//        public static void WriteRegionColor(Region region)
//        {
//            try
//            {
//                byte[] colorData = new byte[] { region.Color.R, region.Color.G, region.Color.B, region.Color.A };
//                int colorNum = BitConverter.ToInt32(colorData, 0);
//                SqliteCommand cmd = new SqliteCommand("UPDATE Regions SET [color]='" + colorNum + "' WHERE [id]='" + region.ID + "'", connection);

//                connection.Open();
//                cmd.ExecuteNonQuery();
//            }
//            finally { connection.Close(); }
//        }

//        public static void RemoveRegion(Region region)
//        {
//            try
//            {
//                SqliteCommand cmd = new SqliteCommand("DELETE FROM Regions WHERE id='" + region.ID + "'", connection);
//                connection.Open();
//                cmd.ExecuteNonQuery();
//            }
//            finally { connection.Close(); }
//        }

//        public enum RegistrationResult
//        {
//            Error,
//            UsernameTaken,
//            Sucess
//        }
//    }
//}