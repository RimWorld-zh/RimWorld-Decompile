using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000FBD RID: 4029
	public static class SteamUtility
	{
		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06006144 RID: 24900 RVA: 0x00311130 File Offset: 0x0030F530
		public static string SteamPersonaName
		{
			get
			{
				if (SteamManager.Initialized && SteamUtility.cachedPersonaName == null)
				{
					SteamUtility.cachedPersonaName = SteamFriends.GetPersonaName();
				}
				return (SteamUtility.cachedPersonaName == null) ? "???" : SteamUtility.cachedPersonaName;
			}
		}

		// Token: 0x06006145 RID: 24901 RVA: 0x0031117C File Offset: 0x0030F57C
		public static void OpenUrl(string url)
		{
			if (SteamUtils.IsOverlayEnabled())
			{
				SteamFriends.ActivateGameOverlayToWebPage(url);
			}
			else
			{
				Application.OpenURL(url);
			}
		}

		// Token: 0x06006146 RID: 24902 RVA: 0x0031119A File Offset: 0x0030F59A
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x003111A8 File Offset: 0x0030F5A8
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x003111C4 File Offset: 0x0030F5C4
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}

		// Token: 0x04003F9B RID: 16283
		private static string cachedPersonaName = null;
	}
}
