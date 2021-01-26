using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

using Terraria;
using Terraria.ModLoader;

namespace HEROsMod.HEROsModServices
{
	// Gets a message everytime log in.
	internal class PSAService : HEROsModService
	{
		private static string psaUrl = "http://javid.ddns.net/tModLoader/herosmod/psa.php";
		private bool psaRequested = false;

		public PSAService()
		{
			//GetPSA();
		}

		public override void Update()
		{
			if (!psaRequested)
			{
				psaRequested = true;
				GetPSA();
			}
		}

		private static void GetPSA()
		{
			//WebRequest.DefaultWebProxy = null;
			try
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadStringCompleted += ProcessPSA;
					client.DownloadStringAsync(new Uri(psaUrl
						+ "?tmodversion=" + BuildInfo.tMLVersion.ToString()
						+ "&version=" + HEROsMod.instance.Version.ToString()
						+ "&platform=" + ModLoader.CompressedPlatformRepresentation
						+ "&steamid64=" + ModUtils.SteamID
					));
				}
			}
			catch { }
		}

		private static void ProcessPSA(Object sender, DownloadStringCompletedEventArgs e)
		{
			if (e.Error != null)
				return;
			ModUtils.DebugText("Received from PSA: " + e.Result);
			PSAResponse r = JsonConvert.DeserializeObject<PSAResponse>(e.Result);
			if (r.msgbox != null)
			{
				foreach (var msg in r.msgbox)
				{
					UIKit.MasterView.AddChildToMaster(new UIKit.UIMessageBox(msg));
				}
			}
			if (r.psa != null)
			{
				foreach (var psa in r.psa)
				{
					Main.NewText(psa, 255, 241, 85);
				}
			}
		}
	}

	internal class PSAResponse
	{
#pragma warning disable 0649
		public List<string> psa;
		public List<string> msgbox;
#pragma warning restore 0649
	}
}