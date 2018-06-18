using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200013C RID: 316
	public static class SteamGameServerUtils
	{
		// Token: 0x06000551 RID: 1361 RVA: 0x00007064 File Offset: 0x00005264
		public static uint GetSecondsSinceAppActive()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetSecondsSinceAppActive();
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00007084 File Offset: 0x00005284
		public static uint GetSecondsSinceComputerActive()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetSecondsSinceComputerActive();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000070A4 File Offset: 0x000052A4
		public static EUniverse GetConnectedUniverse()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetConnectedUniverse();
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000070C4 File Offset: 0x000052C4
		public static uint GetServerRealTime()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetServerRealTime();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000070E4 File Offset: 0x000052E4
		public static string GetIPCountry()
		{
			InteropHelp.TestIfAvailableGameServer();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetIPCountry());
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00007108 File Offset: 0x00005308
		public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0000712C File Offset: 0x0000532C
		public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00007150 File Offset: 0x00005350
		public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetCSERIPPort(out unIP, out usPort);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00007174 File Offset: 0x00005374
		public static byte GetCurrentBatteryPower()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetCurrentBatteryPower();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00007194 File Offset: 0x00005394
		public static AppId_t GetAppID()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (AppId_t)NativeMethods.ISteamGameServerUtils_GetAppID();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000071B8 File Offset: 0x000053B8
		public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetOverlayNotificationPosition(eNotificationPosition);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000071C8 File Offset: 0x000053C8
		public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000071EC File Offset: 0x000053EC
		public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetAPICallFailureReason(hSteamAPICall);
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0000720C File Offset: 0x0000540C
		public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x00007231 File Offset: 0x00005431
		public static void RunFrame()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_RunFrame();
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00007240 File Offset: 0x00005440
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetIPCCallCount();
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0000725F File Offset: 0x0000545F
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetWarningMessageHook(pFunction);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00007270 File Offset: 0x00005470
		public static bool IsOverlayEnabled()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsOverlayEnabled();
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x00007290 File Offset: 0x00005490
		public static bool BOverlayNeedsPresent()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_BOverlayNeedsPresent();
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000072B0 File Offset: 0x000054B0
		public static SteamAPICall_t CheckFileSignature(string szFileName)
		{
			InteropHelp.TestIfAvailableGameServer();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(szFileName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamGameServerUtils_CheckFileSignature(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000072FC File Offset: 0x000054FC
		public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
				{
					result = NativeMethods.ISteamGameServerUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, utf8StringHandle, unCharMax, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00007368 File Offset: 0x00005568
		public static uint GetEnteredGamepadTextLength()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextLength();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00007388 File Offset: 0x00005588
		public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
		{
			InteropHelp.TestIfAvailableGameServer();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchText);
			bool flag = NativeMethods.ISteamGameServerUtils_GetEnteredGamepadTextInput(intPtr, cchText);
			pchText = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000073CC File Offset: 0x000055CC
		public static string GetSteamUILanguage()
		{
			InteropHelp.TestIfAvailableGameServer();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamGameServerUtils_GetSteamUILanguage());
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000073F0 File Offset: 0x000055F0
		public static bool IsSteamRunningInVR()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServerUtils_IsSteamRunningInVR();
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0000740F File Offset: 0x0000560F
		public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServerUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
		}
	}
}
