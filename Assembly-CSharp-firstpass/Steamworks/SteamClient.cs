using System;

namespace Steamworks
{
	// Token: 0x02000133 RID: 307
	public static class SteamClient
	{
		// Token: 0x0600042E RID: 1070 RVA: 0x00003CC8 File Offset: 0x00001EC8
		public static HSteamPipe CreateSteamPipe()
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamPipe)NativeMethods.ISteamClient_CreateSteamPipe();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00003CEC File Offset: 0x00001EEC
		public static bool BReleaseSteamPipe(HSteamPipe hSteamPipe)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_BReleaseSteamPipe(hSteamPipe);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00003D0C File Offset: 0x00001F0C
		public static HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamClient_ConnectToGlobalUser(hSteamPipe);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00003D34 File Offset: 0x00001F34
		public static HSteamUser CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType)
		{
			InteropHelp.TestIfAvailableClient();
			return (HSteamUser)NativeMethods.ISteamClient_CreateLocalUser(out phSteamPipe, eAccountType);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00003D5A File Offset: 0x00001F5A
		public static void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_ReleaseUser(hSteamPipe, hUser);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00003D6C File Offset: 0x00001F6C
		public static IntPtr GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUser(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00003DB4 File Offset: 0x00001FB4
		public static IntPtr GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamGameServer(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00003DFC File Offset: 0x00001FFC
		public static void SetLocalIPBinding(uint unIP, ushort usPort)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_SetLocalIPBinding(unIP, usPort);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00003E0C File Offset: 0x0000200C
		public static IntPtr GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamFriends(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00003E54 File Offset: 0x00002054
		public static IntPtr GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUtils(hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00003E9C File Offset: 0x0000209C
		public static IntPtr GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamMatchmaking(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00003EE4 File Offset: 0x000020E4
		public static IntPtr GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamMatchmakingServers(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00003F2C File Offset: 0x0000212C
		public static IntPtr GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamGenericInterface(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00003F74 File Offset: 0x00002174
		public static IntPtr GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUserStats(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00003FBC File Offset: 0x000021BC
		public static IntPtr GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamGameServerStats(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00004004 File Offset: 0x00002204
		public static IntPtr GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamApps(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000404C File Offset: 0x0000224C
		public static IntPtr GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamNetworking(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00004094 File Offset: 0x00002294
		public static IntPtr GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamRemoteStorage(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000040DC File Offset: 0x000022DC
		public static IntPtr GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamScreenshots(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00004124 File Offset: 0x00002324
		public static void RunFrame()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_RunFrame();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00004134 File Offset: 0x00002334
		public static uint GetIPCCallCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_GetIPCCallCount();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00004153 File Offset: 0x00002353
		public static void SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_SetWarningMessageHook(pFunction);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00004164 File Offset: 0x00002364
		public static bool BShutdownIfAllPipesClosed()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamClient_BShutdownIfAllPipesClosed();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00004184 File Offset: 0x00002384
		public static IntPtr GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamHTTP(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000041CC File Offset: 0x000023CC
		public static IntPtr GetISteamUnifiedMessages(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUnifiedMessages(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00004214 File Offset: 0x00002414
		public static IntPtr GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamController(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000425C File Offset: 0x0000245C
		public static IntPtr GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamUGC(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000042A4 File Offset: 0x000024A4
		public static IntPtr GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamAppList(hSteamUser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000042EC File Offset: 0x000024EC
		public static IntPtr GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamMusic(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00004334 File Offset: 0x00002534
		public static IntPtr GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamMusicRemote(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000437C File Offset: 0x0000257C
		public static IntPtr GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamHTMLSurface(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000043C4 File Offset: 0x000025C4
		public static void Set_SteamAPI_CPostAPIResultInProcess(SteamAPI_PostAPIResultInProcess_t func)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_Set_SteamAPI_CPostAPIResultInProcess(func);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000043D2 File Offset: 0x000025D2
		public static void Remove_SteamAPI_CPostAPIResultInProcess(SteamAPI_PostAPIResultInProcess_t func)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_Remove_SteamAPI_CPostAPIResultInProcess(func);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000043E0 File Offset: 0x000025E0
		public static void Set_SteamAPI_CCheckCallbackRegisteredInProcess(SteamAPI_CheckCallbackRegistered_t func)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamClient_Set_SteamAPI_CCheckCallbackRegisteredInProcess(func);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000043F0 File Offset: 0x000025F0
		public static IntPtr GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamInventory(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00004438 File Offset: 0x00002638
		public static IntPtr GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersion))
			{
				result = NativeMethods.ISteamClient_GetISteamVideo(hSteamuser, hSteamPipe, utf8StringHandle);
			}
			return result;
		}
	}
}
