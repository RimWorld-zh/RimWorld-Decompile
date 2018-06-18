using System;

namespace Steamworks
{
	// Token: 0x02000034 RID: 52
	public static class SteamAPI
	{
		// Token: 0x060000BE RID: 190 RVA: 0x000034DC File Offset: 0x000016DC
		public static bool RestartAppIfNecessary(AppId_t unOwnAppID)
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_RestartAppIfNecessary(unOwnAppID);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000034FC File Offset: 0x000016FC
		public static bool InitSafe()
		{
			return SteamAPI.Init();
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003518 File Offset: 0x00001718
		public static bool Init()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_InitSafe();
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003537 File Offset: 0x00001737
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_Shutdown();
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003544 File Offset: 0x00001744
		public static void RunCallbacks()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamAPI_RunCallbacks();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003554 File Offset: 0x00001754
		public static bool IsSteamRunning()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamAPI_IsSteamRunning();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003574 File Offset: 0x00001774
		public static HSteamUser GetHSteamUserCurrent()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.Steam_GetHSteamUserCurrent();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003598 File Offset: 0x00001798
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamAPI_GetHSteamPipe();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000035BC File Offset: 0x000017BC
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamAPI_GetHSteamUser();
		}
	}
}
