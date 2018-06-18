using System;

namespace Steamworks
{
	// Token: 0x02000035 RID: 53
	public static class GameServer
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x000035E0 File Offset: 0x000017E0
		public static bool InitSafe(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString)
		{
			InteropHelp.TestIfPlatformSupported();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				result = NativeMethods.SteamGameServer_InitSafe(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000362C File Offset: 0x0000182C
		public static bool Init(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, string pchVersionString)
		{
			InteropHelp.TestIfPlatformSupported();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				result = NativeMethods.SteamGameServer_InitSafe(unIP, usSteamPort, usGamePort, usQueryPort, eServerMode, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003678 File Offset: 0x00001878
		public static void Shutdown()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_Shutdown();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003685 File Offset: 0x00001885
		public static void RunCallbacks()
		{
			InteropHelp.TestIfPlatformSupported();
			NativeMethods.SteamGameServer_RunCallbacks();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00003694 File Offset: 0x00001894
		public static bool BSecure()
		{
			InteropHelp.TestIfPlatformSupported();
			return NativeMethods.SteamGameServer_BSecure();
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000036B4 File Offset: 0x000018B4
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfPlatformSupported();
			return (CSteamID)NativeMethods.SteamGameServer_GetSteamID();
		}

		// Token: 0x060000CD RID: 205 RVA: 0x000036D8 File Offset: 0x000018D8
		public static HSteamPipe GetHSteamPipe()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamPipe)NativeMethods.SteamGameServer_GetHSteamPipe();
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000036FC File Offset: 0x000018FC
		public static HSteamUser GetHSteamUser()
		{
			InteropHelp.TestIfPlatformSupported();
			return (HSteamUser)NativeMethods.SteamGameServer_GetHSteamUser();
		}
	}
}
