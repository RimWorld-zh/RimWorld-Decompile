using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000FBE RID: 4030
	public static class SteamUtility
	{
		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06006146 RID: 24902 RVA: 0x00311054 File Offset: 0x0030F454
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

		// Token: 0x06006147 RID: 24903 RVA: 0x003110A0 File Offset: 0x0030F4A0
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

		// Token: 0x06006148 RID: 24904 RVA: 0x003110BE File Offset: 0x0030F4BE
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x003110CC File Offset: 0x0030F4CC
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x003110E8 File Offset: 0x0030F4E8
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}

		// Token: 0x04003F9C RID: 16284
		private static string cachedPersonaName = null;
	}
}
