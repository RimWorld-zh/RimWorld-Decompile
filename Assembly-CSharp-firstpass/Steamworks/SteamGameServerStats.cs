using System;

namespace Steamworks
{
	// Token: 0x0200013A RID: 314
	public static class SteamGameServerStats
	{
		// Token: 0x0600050F RID: 1295 RVA: 0x00006278 File Offset: 0x00004478
		public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerStats_RequestUserStats(steamIDUser);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000062A0 File Offset: 0x000044A0
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_GetUserStat(steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000062E8 File Offset: 0x000044E8
		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_GetUserStat_(steamIDUser, utf8StringHandle, out pData);
			}
			return result;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00006330 File Offset: 0x00004530
		public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_GetUserAchievement(steamIDUser, utf8StringHandle, out pbAchieved);
			}
			return result;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00006378 File Offset: 0x00004578
		public static bool SetUserStat(CSteamID steamIDUser, string pchName, int nData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_SetUserStat(steamIDUser, utf8StringHandle, nData);
			}
			return result;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000063C0 File Offset: 0x000045C0
		public static bool SetUserStat(CSteamID steamIDUser, string pchName, float fData)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_SetUserStat_(steamIDUser, utf8StringHandle, fData);
			}
			return result;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00006408 File Offset: 0x00004608
		public static bool UpdateUserAvgRateStat(CSteamID steamIDUser, string pchName, float flCountThisSession, double dSessionLength)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_UpdateUserAvgRateStat(steamIDUser, utf8StringHandle, flCountThisSession, dSessionLength);
			}
			return result;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00006450 File Offset: 0x00004650
		public static bool SetUserAchievement(CSteamID steamIDUser, string pchName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_SetUserAchievement(steamIDUser, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00006498 File Offset: 0x00004698
		public static bool ClearUserAchievement(CSteamID steamIDUser, string pchName)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamGameServerStats_ClearUserAchievement(steamIDUser, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000064E0 File Offset: 0x000046E0
		public static SteamAPICall_t StoreUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServerStats_StoreUserStats(steamIDUser);
		}
	}
}
