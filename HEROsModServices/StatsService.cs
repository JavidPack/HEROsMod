using System;
using System.Net;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	class StatsService : HEROsModService
	{
		static Uri statUrl = new Uri("http://javid.ddns.net/tModLoader/herosmod/stats.php");
		static float checkEventPassedTimer = 0;
		static float checkEventPassedTime = 60; // Time between checks for time between in seconds
		static DateTime eventTime;
		static int playHours = -1;

		public StatsService()
		{
			//eventTime = GetNextSendEvent();
			eventTime = DateTime.Now.AddSeconds(30);
		}

		public override void Update()
		{
			checkEventPassedTimer -= ModUtils.DeltaTime;
			if (checkEventPassedTimer <= 0)
			{
				CheckForEventPassed();
				checkEventPassedTimer = checkEventPassedTime;
			}
			base.Update();
		}

		static DateTime GetNextSendEvent()
		{
			playHours++;
			DateTime now = DateTime.Now;
			//	return new DateTime(now.Year, now.Month, now.Day, now.Hour, 5, 0).AddHours(1);
			return now.AddHours(1);
		}

		static void CheckForEventPassed()
		{
			DateTime now = DateTime.Now;
			if (now > eventTime)
			{
				eventTime = GetNextSendEvent();
				SendData();
			}
		}

		static void SendData()
		{
			try
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadStringAsync(new Uri(statUrl
						+ "?tmodversion=" + ModLoader.version.ToString()
						+ "&version=" + HEROsMod.instance.Version.ToString()
						+ "&platform=" + ModLoader.compressedPlatformRepresentation
						+ "&playhours=" + playHours
						+ "&steamid64=" + ModUtils.SteamID
					));
				}
			}
			catch
			{ }
		}
	}
}
