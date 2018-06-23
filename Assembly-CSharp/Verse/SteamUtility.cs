using System;
using Steamworks;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000FBE RID: 4030
	public static class SteamUtility
	{
		// Token: 0x04003FB8 RID: 16312
		private static string cachedPersonaName = null;

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x0600616D RID: 24941 RVA: 0x00313204 File Offset: 0x00311604
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

		// Token: 0x0600616E RID: 24942 RVA: 0x00313250 File Offset: 0x00311650
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

		// Token: 0x0600616F RID: 24943 RVA: 0x0031326E File Offset: 0x0031166E
		public static void OpenWorkshopPage(PublishedFileId_t pfid)
		{
			SteamUtility.OpenUrl(SteamUtility.SteamWorkshopPageUrl(pfid));
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x0031327C File Offset: 0x0031167C
		public static void OpenSteamWorkshopPage()
		{
			SteamUtility.OpenUrl("http://steamcommunity.com/workshop/browse/?appid=" + SteamUtils.GetAppID());
		}

		// Token: 0x06006171 RID: 24945 RVA: 0x00313298 File Offset: 0x00311698
		public static string SteamWorkshopPageUrl(PublishedFileId_t pfid)
		{
			return "steam://url/CommunityFilePage/" + pfid;
		}
	}
}
