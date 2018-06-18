using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000149 RID: 329
	public static class SteamUser
	{
		// Token: 0x060006AB RID: 1707 RVA: 0x0000AC2C File Offset: 0x00008E2C
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamUser_GetHSteamUser();
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0000AC50 File Offset: 0x00008E50
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BLoggedOn();
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0000AC70 File Offset: 0x00008E70
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamUser_GetSteamID();
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0000AC94 File Offset: 0x00008E94
		public static int InitiateGameConnection(byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, bool bSecure)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_InitiateGameConnection(pAuthBlob, cbMaxAuthBlob, steamIDGameServer, unIPServer, usPortServer, bSecure);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0000ACBB File Offset: 0x00008EBB
		public static void TerminateGameConnection(uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_TerminateGameConnection(unIPServer, usPortServer);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0000ACCC File Offset: 0x00008ECC
		public static void TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, string pchExtraInfo = "")
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchExtraInfo))
			{
				NativeMethods.ISteamUser_TrackAppUsageEvent(gameID, eAppUsageEvent, utf8StringHandle);
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0000AD14 File Offset: 0x00008F14
		public static bool GetUserDataFolder(out string pchBuffer, int cubBuffer)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubBuffer);
			bool flag = NativeMethods.ISteamUser_GetUserDataFolder(intPtr, cubBuffer);
			pchBuffer = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0000AD58 File Offset: 0x00008F58
		public static void StartVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StartVoiceRecording();
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0000AD65 File Offset: 0x00008F65
		public static void StopVoiceRecording()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_StopVoiceRecording();
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0000AD74 File Offset: 0x00008F74
		public static EVoiceResult GetAvailableVoice(out uint pcbCompressed, out uint pcbUncompressed, uint nUncompressedVoiceDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetAvailableVoice(out pcbCompressed, out pcbUncompressed, nUncompressedVoiceDesiredSampleRate);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0000AD98 File Offset: 0x00008F98
		public static EVoiceResult GetVoice(bool bWantCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, bool bWantUncompressed, byte[] pUncompressedDestBuffer, uint cbUncompressedDestBufferSize, out uint nUncompressBytesWritten, uint nUncompressedVoiceDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoice(bWantCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, bWantUncompressed, pUncompressedDestBuffer, cbUncompressedDestBufferSize, out nUncompressBytesWritten, nUncompressedVoiceDesiredSampleRate);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0000ADC8 File Offset: 0x00008FC8
		public static EVoiceResult DecompressVoice(byte[] pCompressed, uint cbCompressed, byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_DecompressVoice(pCompressed, cbCompressed, pDestBuffer, cbDestBufferSize, out nBytesWritten, nDesiredSampleRate);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0000ADF0 File Offset: 0x00008FF0
		public static uint GetVoiceOptimalSampleRate()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetVoiceOptimalSampleRate();
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0000AE10 File Offset: 0x00009010
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return (HAuthTicket)NativeMethods.ISteamUser_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0000AE38 File Offset: 0x00009038
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0000AE5A File Offset: 0x0000905A
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_EndAuthSession(steamID);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0000AE68 File Offset: 0x00009068
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_CancelAuthTicket(hAuthTicket);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0000AE78 File Offset: 0x00009078
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_UserHasLicenseForApp(steamID, appID);
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0000AE9C File Offset: 0x0000909C
		public static bool BIsBehindNAT()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_BIsBehindNAT();
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0000AEBB File Offset: 0x000090BB
		public static void AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUser_AdvertiseGame(steamIDGameServer, unIPServer, usPortServer);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0000AECC File Offset: 0x000090CC
		public static SteamAPICall_t RequestEncryptedAppTicket(byte[] pDataToInclude, int cbDataToInclude)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUser_RequestEncryptedAppTicket(pDataToInclude, cbDataToInclude);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0000AEF4 File Offset: 0x000090F4
		public static bool GetEncryptedAppTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetEncryptedAppTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0000AF18 File Offset: 0x00009118
		public static int GetGameBadgeLevel(int nSeries, bool bFoil)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetGameBadgeLevel(nSeries, bFoil);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0000AF3C File Offset: 0x0000913C
		public static int GetPlayerSteamLevel()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUser_GetPlayerSteamLevel();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0000AF5C File Offset: 0x0000915C
		public static SteamAPICall_t RequestStoreAuthURL(string pchRedirectURL)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchRedirectURL))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUser_RequestStoreAuthURL(utf8StringHandle);
			}
			return result;
		}
	}
}
