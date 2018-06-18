using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000132 RID: 306
	public static class SteamApps
	{
		// Token: 0x06000416 RID: 1046 RVA: 0x00003948 File Offset: 0x00001B48
		public static bool BIsSubscribed()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribed();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00003968 File Offset: 0x00001B68
		public static bool BIsLowViolence()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsLowViolence();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00003988 File Offset: 0x00001B88
		public static bool BIsCybercafe()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsCybercafe();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x000039A8 File Offset: 0x00001BA8
		public static bool BIsVACBanned()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsVACBanned();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000039C8 File Offset: 0x00001BC8
		public static string GetCurrentGameLanguage()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetCurrentGameLanguage());
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000039EC File Offset: 0x00001BEC
		public static string GetAvailableGameLanguages()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetAvailableGameLanguages());
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00003A10 File Offset: 0x00001C10
		public static bool BIsSubscribedApp(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribedApp(appID);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00003A30 File Offset: 0x00001C30
		public static bool BIsDlcInstalled(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsDlcInstalled(appID);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00003A50 File Offset: 0x00001C50
		public static uint GetEarliestPurchaseUnixTime(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetEarliestPurchaseUnixTime(nAppID);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00003A70 File Offset: 0x00001C70
		public static bool BIsSubscribedFromFreeWeekend()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsSubscribedFromFreeWeekend();
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00003A90 File Offset: 0x00001C90
		public static int GetDLCCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetDLCCount();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00003AB0 File Offset: 0x00001CB0
		public static bool BGetDLCDataByIndex(int iDLC, out AppId_t pAppID, out bool pbAvailable, out string pchName, int cchNameBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameBufferSize);
			bool flag = NativeMethods.ISteamApps_BGetDLCDataByIndex(iDLC, out pAppID, out pbAvailable, intPtr, cchNameBufferSize);
			pchName = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00003AF9 File Offset: 0x00001CF9
		public static void InstallDLC(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_InstallDLC(nAppID);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00003B07 File Offset: 0x00001D07
		public static void UninstallDLC(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_UninstallDLC(nAppID);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00003B15 File Offset: 0x00001D15
		public static void RequestAppProofOfPurchaseKey(AppId_t nAppID)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamApps_RequestAppProofOfPurchaseKey(nAppID);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00003B24 File Offset: 0x00001D24
		public static bool GetCurrentBetaName(out string pchName, int cchNameBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchNameBufferSize);
			bool flag = NativeMethods.ISteamApps_GetCurrentBetaName(intPtr, cchNameBufferSize);
			pchName = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00003B68 File Offset: 0x00001D68
		public static bool MarkContentCorrupt(bool bMissingFilesOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_MarkContentCorrupt(bMissingFilesOnly);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00003B88 File Offset: 0x00001D88
		public static uint GetInstalledDepots(AppId_t appID, DepotId_t[] pvecDepots, uint cMaxDepots)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetInstalledDepots(appID, pvecDepots, cMaxDepots);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00003BAC File Offset: 0x00001DAC
		public static uint GetAppInstallDir(AppId_t appID, out string pchFolder, uint cchFolderBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchFolderBufferSize);
			uint num = NativeMethods.ISteamApps_GetAppInstallDir(appID, intPtr, cchFolderBufferSize);
			pchFolder = ((num == 0u) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00003BF4 File Offset: 0x00001DF4
		public static bool BIsAppInstalled(AppId_t appID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_BIsAppInstalled(appID);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00003C14 File Offset: 0x00001E14
		public static CSteamID GetAppOwner()
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamApps_GetAppOwner();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00003C38 File Offset: 0x00001E38
		public static string GetLaunchQueryParam(string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamApps_GetLaunchQueryParam(utf8StringHandle));
			}
			return result;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00003C84 File Offset: 0x00001E84
		public static bool GetDlcDownloadProgress(AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetDlcDownloadProgress(nAppID, out punBytesDownloaded, out punBytesTotal);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00003CA8 File Offset: 0x00001EA8
		public static int GetAppBuildId()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamApps_GetAppBuildId();
		}
	}
}
