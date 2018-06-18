using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200014A RID: 330
	public static class SteamUserStats
	{
		// Token: 0x060006C4 RID: 1732 RVA: 0x0000AFA8 File Offset: 0x000091A8
		public static bool RequestCurrentStats()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_RequestCurrentStats();
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0000AFC8 File Offset: 0x000091C8
		public static bool GetStat(string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetStat(utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0000B010 File Offset: 0x00009210
		public static bool GetStat(string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetStat_(utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0000B058 File Offset: 0x00009258
		public static bool SetStat(string pchName, int nData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_SetStat(utf8StringHandle, nData);
			}
			return result;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0000B0A0 File Offset: 0x000092A0
		public static bool SetStat(string pchName, float fData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_SetStat_(utf8StringHandle, fData);
			}
			return result;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0000B0E8 File Offset: 0x000092E8
		public static bool UpdateAvgRateStat(string pchName, float flCountThisSession, double dSessionLength)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_UpdateAvgRateStat(utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return result;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0000B130 File Offset: 0x00009330
		public static bool GetAchievement(string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetAchievement(utf8StringHandle, out pbAchieved);
			}
			return result;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0000B178 File Offset: 0x00009378
		public static bool SetAchievement(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_SetAchievement(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0000B1C0 File Offset: 0x000093C0
		public static bool ClearAchievement(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_ClearAchievement(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0000B208 File Offset: 0x00009408
		public static bool GetAchievementAndUnlockTime(string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetAchievementAndUnlockTime(utf8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return result;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0000B250 File Offset: 0x00009450
		public static bool StoreStats()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_StoreStats();
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0000B270 File Offset: 0x00009470
		public static int GetAchievementIcon(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetAchievementIcon(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0000B2B8 File Offset: 0x000094B8
		public static string GetAchievementDisplayAttribute(string pchName, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchKey))
				{
					result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementDisplayAttribute(utf8StringHandle, utf8StringHandle2));
				}
			}
			return result;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0000B324 File Offset: 0x00009524
		public static bool IndicateAchievementProgress(string pchName, uint nCurProgress, uint nMaxProgress)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_IndicateAchievementProgress(utf8StringHandle, nCurProgress, nMaxProgress);
			}
			return result;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0000B36C File Offset: 0x0000956C
		public static uint GetNumAchievements()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetNumAchievements();
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0000B38C File Offset: 0x0000958C
		public static string GetAchievementName(uint iAchievement)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementName(iAchievement));
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0000B3B4 File Offset: 0x000095B4
		public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestUserStats(steamIDUser);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0000B3DC File Offset: 0x000095DC
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetUserStat(steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0000B424 File Offset: 0x00009624
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetUserStat_(steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0000B46C File Offset: 0x0000966C
		public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetUserAchievement(steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return result;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0000B4B4 File Offset: 0x000096B4
		public static bool GetUserAchievementAndUnlockTime(CSteamID steamIDUser, string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetUserAchievementAndUnlockTime(steamIDUser, utf8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return result;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0000B4FC File Offset: 0x000096FC
		public static bool ResetAllStats(bool bAchievementsToo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_ResetAllStats(bAchievementsToo);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0000B51C File Offset: 0x0000971C
		public static SteamAPICall_t FindOrCreateLeaderboard(string pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUserStats_FindOrCreateLeaderboard(utf8StringHandle, eLeaderboardSortMethod, eLeaderboardDisplayType);
			}
			return result;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0000B568 File Offset: 0x00009768
		public static SteamAPICall_t FindLeaderboard(string pchLeaderboardName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUserStats_FindLeaderboard(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0000B5B4 File Offset: 0x000097B4
		public static string GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetLeaderboardName(hSteamLeaderboard));
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0000B5DC File Offset: 0x000097DC
		public static int GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardEntryCount(hSteamLeaderboard);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0000B5FC File Offset: 0x000097FC
		public static ELeaderboardSortMethod GetLeaderboardSortMethod(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardSortMethod(hSteamLeaderboard);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0000B61C File Offset: 0x0000981C
		public static ELeaderboardDisplayType GetLeaderboardDisplayType(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardDisplayType(hSteamLeaderboard);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0000B63C File Offset: 0x0000983C
		public static SteamAPICall_t DownloadLeaderboardEntries(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_DownloadLeaderboardEntries(hSteamLeaderboard, eLeaderboardDataRequest, nRangeStart, nRangeEnd);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0000B664 File Offset: 0x00009864
		public static SteamAPICall_t DownloadLeaderboardEntriesForUsers(SteamLeaderboard_t hSteamLeaderboard, CSteamID[] prgUsers, int cUsers)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_DownloadLeaderboardEntriesForUsers(hSteamLeaderboard, prgUsers, cUsers);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0000B68C File Offset: 0x0000988C
		public static bool GetDownloadedLeaderboardEntry(SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, int[] pDetails, int cDetailsMax)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetDownloadedLeaderboardEntry(hSteamLeaderboardEntries, index, out pLeaderboardEntry, pDetails, cDetailsMax);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0000B6B4 File Offset: 0x000098B4
		public static SteamAPICall_t UploadLeaderboardScore(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, int[] pScoreDetails, int cScoreDetailsCount)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_UploadLeaderboardScore(hSteamLeaderboard, eLeaderboardUploadScoreMethod, nScore, pScoreDetails, cScoreDetailsCount);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0000B6E0 File Offset: 0x000098E0
		public static SteamAPICall_t AttachLeaderboardUGC(SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_AttachLeaderboardUGC(hSteamLeaderboard, hUGC);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0000B708 File Offset: 0x00009908
		public static SteamAPICall_t GetNumberOfCurrentPlayers()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_GetNumberOfCurrentPlayers();
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0000B72C File Offset: 0x0000992C
		public static SteamAPICall_t RequestGlobalAchievementPercentages()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestGlobalAchievementPercentages();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x0000B750 File Offset: 0x00009950
		public static int GetMostAchievedAchievementInfo(out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)unNameBufLen);
			int num = NativeMethods.ISteamUserStats_GetMostAchievedAchievementInfo(intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num == -1) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0000B798 File Offset: 0x00009998
		public static int GetNextMostAchievedAchievementInfo(int iIteratorPrevious, out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)unNameBufLen);
			int num = NativeMethods.ISteamUserStats_GetNextMostAchievedAchievementInfo(iIteratorPrevious, intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num == -1) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0000B7E4 File Offset: 0x000099E4
		public static bool GetAchievementAchievedPercent(string pchName, out float pflPercent)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetAchievementAchievedPercent(utf8StringHandle, out pflPercent);
			}
			return result;
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0000B82C File Offset: 0x00009A2C
		public static SteamAPICall_t RequestGlobalStats(int nHistoryDays)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestGlobalStats(nHistoryDays);
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0000B854 File Offset: 0x00009A54
		public static bool GetGlobalStat(string pchStatName, out long pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = NativeMethods.ISteamUserStats_GetGlobalStat(utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0000B89C File Offset: 0x00009A9C
		public static bool GetGlobalStat(string pchStatName, out double pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = NativeMethods.ISteamUserStats_GetGlobalStat_(utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		public static int GetGlobalStatHistory(string pchStatName, long[] pData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = NativeMethods.ISteamUserStats_GetGlobalStatHistory(utf8StringHandle, pData, cubData);
			}
			return result;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0000B92C File Offset: 0x00009B2C
		public static int GetGlobalStatHistory(string pchStatName, double[] pData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = NativeMethods.ISteamUserStats_GetGlobalStatHistory_(utf8StringHandle, pData, cubData);
			}
			return result;
		}
	}
}
