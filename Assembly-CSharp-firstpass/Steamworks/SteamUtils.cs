using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200014B RID: 331
	public static class SteamUtils
	{
		// Token: 0x060006EF RID: 1775 RVA: 0x0000B974 File Offset: 0x00009B74
		public static uint GetSecondsSinceAppActive()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetSecondsSinceAppActive();
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0000B994 File Offset: 0x00009B94
		public static uint GetSecondsSinceComputerActive()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetSecondsSinceComputerActive();
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0000B9B4 File Offset: 0x00009BB4
		public static EUniverse GetConnectedUniverse()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetConnectedUniverse();
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0000B9D4 File Offset: 0x00009BD4
		public static uint GetServerRealTime()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetServerRealTime();
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0000B9F4 File Offset: 0x00009BF4
		public static string GetIPCountry()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetIPCountry());
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0000BA18 File Offset: 0x00009C18
		public static bool GetImageSize(int iImage, out uint pnWidth, out uint pnHeight)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetImageSize(iImage, out pnWidth, out pnHeight);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0000BA3C File Offset: 0x00009C3C
		public static bool GetImageRGBA(int iImage, byte[] pubDest, int nDestBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetImageRGBA(iImage, pubDest, nDestBufferSize);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0000BA60 File Offset: 0x00009C60
		public static bool GetCSERIPPort(out uint unIP, out ushort usPort)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetCSERIPPort(out unIP, out usPort);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0000BA84 File Offset: 0x00009C84
		public static byte GetCurrentBatteryPower()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetCurrentBatteryPower();
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0000BAA4 File Offset: 0x00009CA4
		public static AppId_t GetAppID()
		{
			InteropHelp.TestIfAvailableClient();
			return (AppId_t)NativeMethods.ISteamUtils_GetAppID();
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0000BAC8 File Offset: 0x00009CC8
		public static void SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetOverlayNotificationPosition(eNotificationPosition);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0000BAD8 File Offset: 0x00009CD8
		public static bool IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsAPICallCompleted(hSteamAPICall, out pbFailed);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0000BAFC File Offset: 0x00009CFC
		public static ESteamAPICallFailure GetAPICallFailureReason(SteamAPICall_t hSteamAPICall)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetAPICallFailureReason(hSteamAPICall);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0000BB1C File Offset: 0x00009D1C
		public static bool GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetAPICallResult(hSteamAPICall, pCallback, cubCallback, iCallbackExpected, out pbFailed);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0000BB41 File Offset: 0x00009D41
		public static void RunFrame()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_RunFrame();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0000BB50 File Offset: 0x00009D50
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetIPCCallCount();
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0000BB6F File Offset: 0x00009D6F
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetWarningMessageHook(pFunction);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0000BB80 File Offset: 0x00009D80
		public static bool IsOverlayEnabled()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsOverlayEnabled();
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0000BBA0 File Offset: 0x00009DA0
		public static bool BOverlayNeedsPresent()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_BOverlayNeedsPresent();
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		public static SteamAPICall_t CheckFileSignature(string szFileName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(szFileName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUtils_CheckFileSignature(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0000BC0C File Offset: 0x00009E0C
		public static bool ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchExistingText))
				{
					result = NativeMethods.ISteamUtils_ShowGamepadTextInput(eInputMode, eLineInputMode, utf8StringHandle, unCharMax, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0000BC78 File Offset: 0x00009E78
		public static uint GetEnteredGamepadTextLength()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_GetEnteredGamepadTextLength();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0000BC98 File Offset: 0x00009E98
		public static bool GetEnteredGamepadTextInput(out string pchText, uint cchText)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)cchText);
			bool flag = NativeMethods.ISteamUtils_GetEnteredGamepadTextInput(intPtr, cchText);
			pchText = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return flag;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0000BCDC File Offset: 0x00009EDC
		public static string GetSteamUILanguage()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUtils_GetSteamUILanguage());
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0000BD00 File Offset: 0x00009F00
		public static bool IsSteamRunningInVR()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUtils_IsSteamRunningInVR();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0000BD1F File Offset: 0x00009F1F
		public static void SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamUtils_SetOverlayNotificationInset(nHorizontalInset, nVerticalInset);
		}
	}
}
