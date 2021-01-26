using System;
using System.Net;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	internal class StatsService : HEROsModService
	{
		private static Uri statUrl = new Uri("http://javid.ddns.net/tModLoader/herosmod/stats.php");
		private static float checkEventPassedTimer = 0;
		private static float checkEventPassedTime = 60; // Time between checks for time between in seconds
		private static DateTime eventTime;
		private static int playHours = -1;

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

		private static DateTime GetNextSendEvent()
		{
			playHours++;
			DateTime now = DateTime.Now;
			//	return new DateTime(now.Year, now.Month, now.Day, now.Hour, 5, 0).AddHours(1);
			return now.AddHours(1);
		}

		private static void CheckForEventPassed()
		{
			DateTime now = DateTime.Now;
			if (now > eventTime)
			{
				eventTime = GetNextSendEvent();
				SendData();
			}
		}

		private static void SendData()
		{
			try
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadStringAsync(new Uri(statUrl
						+ "?tmodversion=" + BuildInfo.tMLVersion.ToString()
						+ "&version=" + HEROsMod.instance.Version.ToString()
						+ "&platform=" + ModLoader.CompressedPlatformRepresentation
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