using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terraria;

namespace HEROsMod.HEROsModServices
{
	/// <summary>
	/// A serializable JSON class that stores login information locally.
	/// </summary>
	public class LoginStorage
	{
		/// <summary>
		/// Qualified file path for the LoginStorage JSON file.
		/// </summary>
		[JsonIgnore]
		public string FilePath { get; } = string.Concat(new object[]
			{
				Main.SavePath,
				Path.DirectorySeparatorChar,
				"HEROsModLogin",
				".json"
			});

		/// <summary>
		/// Locally stored logins saved by the user on a per-server basis.
		/// </summary>
		public List<LoginServer> Logins { get; set; }

		/// <summary>
		/// Retrieves the login based on the server address and player character name.
		/// </summary>
		/// <param name="serverAddress">IP address to find login by.</param>
		/// <param name="plrCharName">Individual player's character name.</param>
		/// <returns><para>Character-specific login, or default login if not found.</para>If no default login is found, returns an empty login.</returns>
		public LoginInfo GetLogin(string serverAddress, string plrCharName)
		{
			var loginServer = GetLoginServer(serverAddress);

			if (loginServer.PlayerLogins.Count() > 0)
			{
				var plrCharLogin = loginServer.PlayerLogins.Where(plrLogin => plrLogin.CharacterName == plrCharName).FirstOrDefault();

				if (plrCharLogin == null)
					plrCharLogin = new LoginInfo();

				if (!((plrCharLogin.Username == null || plrCharLogin.Username == "") & (plrCharLogin.Password == null || plrCharLogin.Password == "")))
				{
					ModUtils.DebugText("Player login found! Returning player login");
					return plrCharLogin;
				}
			}

			if (loginServer.DefaultLogin == null)
			{
				loginServer.DefaultLogin = new LoginInfo();
			}

			ModUtils.DebugText("Returning default login");
			return loginServer.DefaultLogin;
		}

		/// <summary>
		/// Assigns the default login for the designated server by server address.
		/// </summary>
		/// <param name="serverAddress">IP address to find login by.</param>
		public void AddLogin(string serverAddress, string username, string password)
			=> AddLogin(serverAddress, null, username, password);

		/// <summary>
		/// Adds a player character-specific login for the designated server by server address and player name respectively.
		/// </summary>
		/// <param name="serverAddress">IP address to find login by.</param>
		/// <param name="plrCharName">Individual player's character name.</param>
		public void AddLogin(string serverAddress, string plrCharName, string username, string password)
		{
			var loginServer = GetLoginServer(serverAddress);

			LoginInfo loginToAdd = new LoginInfo() {
					CharacterName = plrCharName,
					Username = username,
					Password = password
				};

			if(plrCharName == null)
			{
				loginServer.DefaultLogin = loginToAdd;
			}
			else
			{
				if (loginServer.PlayerLogins != null && loginServer.PlayerLogins.Count > 0)
				{
					var plrSearch = loginServer.PlayerLogins.Where(plrLogin => plrLogin.CharacterName == plrCharName);
					
					if(plrSearch.Count() > 0)
					{
						var player = plrSearch.First();
						player.Username = username;
						player.Password = password;
					}
					else
					{
						loginServer.PlayerLogins.Add(loginToAdd);
					}
				}
				else
				{
					loginServer.PlayerLogins.Add(loginToAdd);
				}
			}

			if (loginServer.ServerAddress == "")
			{
				loginServer.ServerAddress = serverAddress;
				Logins.Add(loginServer);
			}
		}

		/// <summary>
		/// <para>Removes a login entry using the designated serverAddress and plrCharName.</para>
		/// <para>If no entry is found using both parameters, the method will attempt to remove the default login entry using only the serverAddress.</para>
		/// </summary>
		/// <param name="serverAddress">IP address to find login by.</param>
		/// <param name="plrCharName">Individual player's character name.</param>
		/// <returns>True if successfully removed, otherwise false.</returns>
		public bool RemoveLogin(string serverAddress, string plrCharName)
		{
			var loginServer = GetLoginServer(serverAddress);

			if (loginServer.PlayerLogins.Count() > 0)
			{
				var plrLoginSearch = loginServer.PlayerLogins.Where(plrLogin => plrLogin.CharacterName == plrCharName);

				if (plrLoginSearch.Count() > 0)
				{
					return loginServer.PlayerLogins.Remove(plrLoginSearch.FirstOrDefault());
				}
			}

			if (loginServer.ServerAddress != "")
			{
				return Logins.Remove(loginServer);
			}
			
			return false;
		}

		/// <summary>
		/// Attempts to populate this LoginStorage object with data stored locally on the machine using FilePath.
		/// </summary>
		/// <returns>True if successfully loaded, otherwise false.</returns>
		public bool LoadJSON()
		{
			if (File.Exists(FilePath))
			{
				ModUtils.DebugText("LoginStorage JSON File Exists");
				using (StreamReader r = new StreamReader(FilePath))
				{
					string json = r.ReadToEnd();
					try
					{
						var loginStorage = JsonConvert.DeserializeObject<LoginStorage>(json);
						Logins = loginStorage.Logins;
						ModUtils.DebugText("LoginStorage successfully loaded");
						return true;
					}
					catch
					{
						ModUtils.DebugText("Error deserializing LoginStorage JSON file. Is your file corrupted or malformed?");
						return false;
					}
				}
			}
			ModUtils.DebugText("LoginStorage JSON File Doesn't Exist. Expected location: " + FilePath);
			return false;
		}

		/// <summary>
		/// Attempts to store this LoginStorage object's data locally on the machine using FilePath.
		/// </summary>
		/// <returns>True if successfully saved, otherwise false.</returns>
		public bool SaveJSON()
		{
			string json = "";
			try
			{
				json = JsonConvert.SerializeObject(this, Formatting.Indented);
			}
			catch
			{
				ModUtils.DebugText("Error trying to serialize LoginStorage JSON.");
				return false;
			}

			try
			{
				File.WriteAllText(FilePath, json);
				ModUtils.DebugText("LoginStorage successfully saved. FilePath: " + FilePath);
				return true;
			}
			catch
			{
				ModUtils.DebugText("Error trying to write to LoginStorage file.");
				return false;
			}
		}

		public LoginStorage()
		{
			Logins = new List<LoginServer>();
		}

		/// <summary>
		/// Returns a login server that matches the provided server address.
		/// </summary>
		/// <param name="serverAddress">IP address to find login by.</param>
		/// <returns>LoginServer object of matching server address. Otherwise, returns an empty LoginServer object.</returns>
		private LoginServer GetLoginServer(string serverAddress)
		{
			LoginServer loginServer = Logins.Where(login => login.ServerAddress == serverAddress).FirstOrDefault();

			if (loginServer == null)
				loginServer = new LoginServer();

			return loginServer;
		}
	}

	/// <summary>
	/// All login data belonging to one server. Identified by IP address.
	/// </summary>
	public class LoginServer
	{
		/// <summary>
		/// <para>The IP address the login information belongs to.</para>Example: 127.0.0.1
		/// </summary>
		public string ServerAddress { get; set; } = "";

		/// <summary>
		/// Default login to use if no player-specific login is found.
		/// </summary>
		public LoginInfo DefaultLogin { get; set; } = new LoginInfo();

		/// <summary>
		/// A list containing all logins tied to one player character.
		/// </summary>
		public List<LoginInfo> PlayerLogins { get; set; } = new List<LoginInfo>();
	}

	public class LoginInfo
	{
		/// <summary>
		/// Name of the character this login information belongs to. This field is ignored when used as a DefaultLogin.
		/// </summary>
		public string CharacterName { get; set; } = "";

		/// <summary>
		/// <para>Player's username used for logging in.</para>Note: This data is NOT encrypted in any way.
		/// </summary>
		public string Username { get; set; } = "";

		/// <summary>
		/// <para>Player's password used for logging in.</para>Note: This data is NOT encrypted in any way.
		/// </summary>
		public string Password { get; set; } = "";

		/// <summary>
		/// Calculates the LoginSaveType and returns the appropriate value.
		/// </summary>
		/// <returns>LoginSaveType depending on property values.</returns>
		public LoginSaveType GetSaveType()
		{
			if ((CharacterName == "" || CharacterName == null) && (Username != "" || Password != ""))
			{
				return LoginSaveType.Default;
			}
			else if ((CharacterName != "" || CharacterName != null) && (Username != "" || Password != ""))
			{
				return LoginSaveType.Player;
			}
			else
			{
				return LoginSaveType.None;
			}
		}
	}

	public enum LoginSaveType
	{
		None,
		Default,
		Player
	}
}
