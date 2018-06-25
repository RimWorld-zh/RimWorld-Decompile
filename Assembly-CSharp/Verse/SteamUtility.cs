using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000FC3 RID: 4035
	public static class SteamUtility
	{
		// Token: 0x04003FC8 RID: 16328
		private static string cachedPersonaName = null;

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x0600617D RID: 24957 RVA: 0x00313F28 File Offset: 0x00312328
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

		// Token: 0x0600617E RID: 24958 RVA: 0x00313F74 File Offset: 0x00312374
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

		// Token: 0x0600617F RID: 24959 RVA: 0x00313F92 File Offset: 0x00312392
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x00313FA0 File Offset: 0x003123A0
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x00313FBC File Offset: 0x003123BC
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}
	}
}
