using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000037 RID: 55
	internal static class NativeMethods
	{
		// Token: 0x0400004A RID: 74
		internal const string NativeLibraryName = "CSteamworks";

		// Token: 0x060000D7 RID: 215
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Shutdown")]
		public static extern void SteamAPI_Shutdown();

		// Token: 0x060000D8 RID: 216
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "IsSteamRunning")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamAPI_IsSteamRunning();

		// Token: 0x060000D9 RID: 217
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "RestartAppIfNecessary")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamAPI_RestartAppIfNecessary(AppId_t unOwnAppID);

		// Token: 0x060000DA RID: 218
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "WriteMiniDump")]
		public static extern void SteamAPI_WriteMiniDump(uint uStructuredExceptionCode, IntPtr pvExceptionInfo, uint uBuildID);

		// Token: 0x060000DB RID: 219
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetMiniDumpComment")]
		public static extern void SteamAPI_SetMiniDumpComment(InteropHelp.UTF8StringHandle pchMsg);

		// Token: 0x060000DC RID: 220
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamClient_")]
		public static extern IntPtr SteamClient();

		// Token: 0x060000DD RID: 221
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "InitSafe")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamAPI_InitSafe();

		// Token: 0x060000DE RID: 222
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "RunCallbacks")]
		public static extern void SteamAPI_RunCallbacks();

		// Token: 0x060000DF RID: 223
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "RegisterCallback")]
		public static extern void SteamAPI_RegisterCallback(IntPtr pCallback, int iCallback);

		// Token: 0x060000E0 RID: 224
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "UnregisterCallback")]
		public static extern void SteamAPI_UnregisterCallback(IntPtr pCallback);

		// Token: 0x060000E1 RID: 225
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "RegisterCallResult")]
		public static extern void SteamAPI_RegisterCallResult(IntPtr pCallback, ulong hAPICall);

		// Token: 0x060000E2 RID: 226
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "UnregisterCallResult")]
		public static extern void SteamAPI_UnregisterCallResult(IntPtr pCallback, ulong hAPICall);

		// Token: 0x060000E3 RID: 227
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Steam_RunCallbacks_")]
		public static extern void Steam_RunCallbacks(HSteamPipe hSteamPipe, [MarshalAs(UnmanagedType.I1)] bool bGameServerCallbacks);

		// Token: 0x060000E4 RID: 228
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Steam_RegisterInterfaceFuncs_")]
		public static extern void Steam_RegisterInterfaceFuncs(IntPtr hModule);

		// Token: 0x060000E5 RID: 229
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Steam_GetHSteamUserCurrent_")]
		public static extern int Steam_GetHSteamUserCurrent();

		// Token: 0x060000E6 RID: 230
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetSteamInstallPath")]
		public static extern int SteamAPI_GetSteamInstallPath();

		// Token: 0x060000E7 RID: 231
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetHSteamPipe_")]
		public static extern int SteamAPI_GetHSteamPipe();

		// Token: 0x060000E8 RID: 232
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SetTryCatchCallbacks")]
		public static extern void SteamAPI_SetTryCatchCallbacks([MarshalAs(UnmanagedType.I1)] bool bTryCatchCallbacks);

		// Token: 0x060000E9 RID: 233
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetHSteamUser_")]
		public static extern int SteamAPI_GetHSteamUser();

		// Token: 0x060000EA RID: 234
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "UseBreakpadCrashHandler")]
		public static extern void SteamAPI_UseBreakpadCrashHandler(InteropHelp.UTF8StringHandle pchVersion, InteropHelp.UTF8StringHandle pchDate, InteropHelp.UTF8StringHandle pchTime, [MarshalAs(UnmanagedType.I1)] bool bFullMemoryDumps, IntPtr pvContext, IntPtr m_pfnPreMinidumpCallback);

		// Token: 0x060000EB RID: 235
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamUser();

		// Token: 0x060000EC RID: 236
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamFriends();

		// Token: 0x060000ED RID: 237
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamUtils();

		// Token: 0x060000EE RID: 238
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamMatchmaking();

		// Token: 0x060000EF RID: 239
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamUserStats();

		// Token: 0x060000F0 RID: 240
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamApps();

		// Token: 0x060000F1 RID: 241
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamNetworking();

		// Token: 0x060000F2 RID: 242
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamMatchmakingServers();

		// Token: 0x060000F3 RID: 243
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamRemoteStorage();

		// Token: 0x060000F4 RID: 244
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamScreenshots();

		// Token: 0x060000F5 RID: 245
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamHTTP();

		// Token: 0x060000F6 RID: 246
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamUnifiedMessages();

		// Token: 0x060000F7 RID: 247
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamController();

		// Token: 0x060000F8 RID: 248
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamUGC();

		// Token: 0x060000F9 RID: 249
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamAppList();

		// Token: 0x060000FA RID: 250
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamMusic();

		// Token: 0x060000FB RID: 251
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamMusicRemote();

		// Token: 0x060000FC RID: 252
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamHTMLSurface();

		// Token: 0x060000FD RID: 253
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamInventory();

		// Token: 0x060000FE RID: 254
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamVideo();

		// Token: 0x060000FF RID: 255
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_InitSafe")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamGameServer_InitSafe(uint unIP, ushort usSteamPort, ushort usGamePort, ushort usQueryPort, EServerMode eServerMode, InteropHelp.UTF8StringHandle pchVersionString);

		// Token: 0x06000100 RID: 256
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_Shutdown")]
		public static extern void SteamGameServer_Shutdown();

		// Token: 0x06000101 RID: 257
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_RunCallbacks")]
		public static extern void SteamGameServer_RunCallbacks();

		// Token: 0x06000102 RID: 258
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_BSecure")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SteamGameServer_BSecure();

		// Token: 0x06000103 RID: 259
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_GetSteamID")]
		public static extern ulong SteamGameServer_GetSteamID();

		// Token: 0x06000104 RID: 260
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_GetHSteamPipe")]
		public static extern int SteamGameServer_GetHSteamPipe();

		// Token: 0x06000105 RID: 261
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl, EntryPoint = "GameServer_GetHSteamUser")]
		public static extern int SteamGameServer_GetHSteamUser();

		// Token: 0x06000106 RID: 262
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamClientGameServer();

		// Token: 0x06000107 RID: 263
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamGameServer();

		// Token: 0x06000108 RID: 264
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamGameServerUtils();

		// Token: 0x06000109 RID: 265
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamGameServerNetworking();

		// Token: 0x0600010A RID: 266
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamGameServerStats();

		// Token: 0x0600010B RID: 267
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamGameServerHTTP();

		// Token: 0x0600010C RID: 268
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamGameServerInventory();

		// Token: 0x0600010D RID: 269
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SteamGameServerUGC();

		// Token: 0x0600010E RID: 270
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_BDecryptTicket")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BDecryptTicket([In] [Out] byte[] rgubTicketEncrypted, uint cubTicketEncrypted, [In] [Out] byte[] rgubTicketDecrypted, ref uint pcubTicketDecrypted, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] rgubKey, int cubKey);

		// Token: 0x0600010F RID: 271
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_BIsTicketForApp")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BIsTicketForApp([In] [Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID);

		// Token: 0x06000110 RID: 272
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_GetTicketIssueTime")]
		public static extern uint GetTicketIssueTime([In] [Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		// Token: 0x06000111 RID: 273
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_GetTicketSteamID")]
		public static extern void GetTicketSteamID([In] [Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out CSteamID psteamID);

		// Token: 0x06000112 RID: 274
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_GetTicketAppID")]
		public static extern uint GetTicketAppID([In] [Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		// Token: 0x06000113 RID: 275
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_BUserOwnsAppInTicket")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BUserOwnsAppInTicket([In] [Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, AppId_t nAppID);

		// Token: 0x06000114 RID: 276
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_BUserIsVacBanned")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BUserIsVacBanned([In] [Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted);

		// Token: 0x06000115 RID: 277
		[DllImport("sdkencryptedappticket", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SteamEncryptedAppTicket_GetUserVariableData")]
		public static extern IntPtr GetUserVariableData([In] [Out] byte[] rgubTicketDecrypted, uint cubTicketDecrypted, out uint pcubUserData);

		// Token: 0x06000116 RID: 278
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamAppList_GetNumInstalledApps();

		// Token: 0x06000117 RID: 279
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamAppList_GetInstalledApps([In] [Out] AppId_t[] pvecAppID, uint unMaxAppIDs);

		// Token: 0x06000118 RID: 280
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppName(AppId_t nAppID, IntPtr pchName, int cchNameMax);

		// Token: 0x06000119 RID: 281
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppInstallDir(AppId_t nAppID, IntPtr pchDirectory, int cchNameMax);

		// Token: 0x0600011A RID: 282
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamAppList_GetAppBuildId(AppId_t nAppID);

		// Token: 0x0600011B RID: 283
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribed();

		// Token: 0x0600011C RID: 284
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsLowViolence();

		// Token: 0x0600011D RID: 285
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsCybercafe();

		// Token: 0x0600011E RID: 286
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsVACBanned();

		// Token: 0x0600011F RID: 287
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamApps_GetCurrentGameLanguage();

		// Token: 0x06000120 RID: 288
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamApps_GetAvailableGameLanguages();

		// Token: 0x06000121 RID: 289
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribedApp(AppId_t appID);

		// Token: 0x06000122 RID: 290
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsDlcInstalled(AppId_t appID);

		// Token: 0x06000123 RID: 291
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetEarliestPurchaseUnixTime(AppId_t nAppID);

		// Token: 0x06000124 RID: 292
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsSubscribedFromFreeWeekend();

		// Token: 0x06000125 RID: 293
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamApps_GetDLCCount();

		// Token: 0x06000126 RID: 294
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BGetDLCDataByIndex(int iDLC, out AppId_t pAppID, out bool pbAvailable, IntPtr pchName, int cchNameBufferSize);

		// Token: 0x06000127 RID: 295
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamApps_InstallDLC(AppId_t nAppID);

		// Token: 0x06000128 RID: 296
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamApps_UninstallDLC(AppId_t nAppID);

		// Token: 0x06000129 RID: 297
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamApps_RequestAppProofOfPurchaseKey(AppId_t nAppID);

		// Token: 0x0600012A RID: 298
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_GetCurrentBetaName(IntPtr pchName, int cchNameBufferSize);

		// Token: 0x0600012B RID: 299
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_MarkContentCorrupt([MarshalAs(UnmanagedType.I1)] bool bMissingFilesOnly);

		// Token: 0x0600012C RID: 300
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetInstalledDepots(AppId_t appID, [In] [Out] DepotId_t[] pvecDepots, uint cMaxDepots);

		// Token: 0x0600012D RID: 301
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamApps_GetAppInstallDir(AppId_t appID, IntPtr pchFolder, uint cchFolderBufferSize);

		// Token: 0x0600012E RID: 302
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_BIsAppInstalled(AppId_t appID);

		// Token: 0x0600012F RID: 303
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamApps_GetAppOwner();

		// Token: 0x06000130 RID: 304
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamApps_GetLaunchQueryParam(InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000131 RID: 305
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamApps_GetDlcDownloadProgress(AppId_t nAppID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		// Token: 0x06000132 RID: 306
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamApps_GetAppBuildId();

		// Token: 0x06000133 RID: 307
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamClient_CreateSteamPipe();

		// Token: 0x06000134 RID: 308
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamClient_BReleaseSteamPipe(HSteamPipe hSteamPipe);

		// Token: 0x06000135 RID: 309
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamClient_ConnectToGlobalUser(HSteamPipe hSteamPipe);

		// Token: 0x06000136 RID: 310
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamClient_CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType);

		// Token: 0x06000137 RID: 311
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser);

		// Token: 0x06000138 RID: 312
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000139 RID: 313
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600013A RID: 314
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_SetLocalIPBinding(uint unIP, ushort usPort);

		// Token: 0x0600013B RID: 315
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600013C RID: 316
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUtils(HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600013D RID: 317
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600013E RID: 318
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600013F RID: 319
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000140 RID: 320
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000141 RID: 321
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000142 RID: 322
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000143 RID: 323
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000144 RID: 324
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000145 RID: 325
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000146 RID: 326
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_RunFrame();

		// Token: 0x06000147 RID: 327
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamClient_GetIPCCallCount();

		// Token: 0x06000148 RID: 328
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

		// Token: 0x06000149 RID: 329
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamClient_BShutdownIfAllPipesClosed();

		// Token: 0x0600014A RID: 330
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600014B RID: 331
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUnifiedMessages(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600014C RID: 332
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600014D RID: 333
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600014E RID: 334
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x0600014F RID: 335
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000150 RID: 336
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000151 RID: 337
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000152 RID: 338
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_Set_SteamAPI_CPostAPIResultInProcess(SteamAPI_PostAPIResultInProcess_t func);

		// Token: 0x06000153 RID: 339
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_Remove_SteamAPI_CPostAPIResultInProcess(SteamAPI_PostAPIResultInProcess_t func);

		// Token: 0x06000154 RID: 340
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamClient_Set_SteamAPI_CCheckCallbackRegisteredInProcess(SteamAPI_CheckCallbackRegistered_t func);

		// Token: 0x06000155 RID: 341
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000156 RID: 342
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamClient_GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, InteropHelp.UTF8StringHandle pchVersion);

		// Token: 0x06000157 RID: 343
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamController_Init(InteropHelp.UTF8StringHandle pchAbsolutePathToControllerConfigVDF);

		// Token: 0x06000158 RID: 344
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamController_Shutdown();

		// Token: 0x06000159 RID: 345
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_RunFrame();

		// Token: 0x0600015A RID: 346
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamController_GetControllerState(uint unControllerIndex, out SteamControllerState_t pState);

		// Token: 0x0600015B RID: 347
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_TriggerHapticPulse(uint unControllerIndex, ESteamControllerPad eTargetPad, ushort usDurationMicroSec);

		// Token: 0x0600015C RID: 348
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamController_SetOverrideMode(InteropHelp.UTF8StringHandle pchMode);

		// Token: 0x0600015D RID: 349
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetPersonaName();

		// Token: 0x0600015E RID: 350
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_SetPersonaName(InteropHelp.UTF8StringHandle pchPersonaName);

		// Token: 0x0600015F RID: 351
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EPersonaState ISteamFriends_GetPersonaState();

		// Token: 0x06000160 RID: 352
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCount(EFriendFlags iFriendFlags);

		// Token: 0x06000161 RID: 353
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags);

		// Token: 0x06000162 RID: 354
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EFriendRelationship ISteamFriends_GetFriendRelationship(CSteamID steamIDFriend);

		// Token: 0x06000163 RID: 355
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EPersonaState ISteamFriends_GetFriendPersonaState(CSteamID steamIDFriend);

		// Token: 0x06000164 RID: 356
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendPersonaName(CSteamID steamIDFriend);

		// Token: 0x06000165 RID: 357
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_GetFriendGamePlayed(CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo);

		// Token: 0x06000166 RID: 358
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName);

		// Token: 0x06000167 RID: 359
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendSteamLevel(CSteamID steamIDFriend);

		// Token: 0x06000168 RID: 360
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetPlayerNickname(CSteamID steamIDPlayer);

		// Token: 0x06000169 RID: 361
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendsGroupCount();

		// Token: 0x0600016A RID: 362
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern short ISteamFriends_GetFriendsGroupIDByIndex(int iFG);

		// Token: 0x0600016B RID: 363
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendsGroupName(FriendsGroupID_t friendsGroupID);

		// Token: 0x0600016C RID: 364
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID);

		// Token: 0x0600016D RID: 365
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_GetFriendsGroupMembersList(FriendsGroupID_t friendsGroupID, [In] [Out] CSteamID[] pOutSteamIDMembers, int nMembersCount);

		// Token: 0x0600016E RID: 366
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags);

		// Token: 0x0600016F RID: 367
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanCount();

		// Token: 0x06000170 RID: 368
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanByIndex(int iClan);

		// Token: 0x06000171 RID: 369
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetClanName(CSteamID steamIDClan);

		// Token: 0x06000172 RID: 370
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetClanTag(CSteamID steamIDClan);

		// Token: 0x06000173 RID: 371
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_GetClanActivityCounts(CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting);

		// Token: 0x06000174 RID: 372
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_DownloadClanActivityCounts([In] [Out] CSteamID[] psteamIDClans, int cClansToRequest);

		// Token: 0x06000175 RID: 373
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCountFromSource(CSteamID steamIDSource);

		// Token: 0x06000176 RID: 374
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend);

		// Token: 0x06000177 RID: 375
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource);

		// Token: 0x06000178 RID: 376
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_SetInGameVoiceSpeaking(CSteamID steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bSpeaking);

		// Token: 0x06000179 RID: 377
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlay(InteropHelp.UTF8StringHandle pchDialog);

		// Token: 0x0600017A RID: 378
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToUser(InteropHelp.UTF8StringHandle pchDialog, CSteamID steamID);

		// Token: 0x0600017B RID: 379
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToWebPage(InteropHelp.UTF8StringHandle pchURL);

		// Token: 0x0600017C RID: 380
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag);

		// Token: 0x0600017D RID: 381
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_SetPlayedWith(CSteamID steamIDUserPlayedWith);

		// Token: 0x0600017E RID: 382
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ActivateGameOverlayInviteDialog(CSteamID steamIDLobby);

		// Token: 0x0600017F RID: 383
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetSmallFriendAvatar(CSteamID steamIDFriend);

		// Token: 0x06000180 RID: 384
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetMediumFriendAvatar(CSteamID steamIDFriend);

		// Token: 0x06000181 RID: 385
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetLargeFriendAvatar(CSteamID steamIDFriend);

		// Token: 0x06000182 RID: 386
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_RequestUserInformation(CSteamID steamIDUser, [MarshalAs(UnmanagedType.I1)] bool bRequireNameOnly);

		// Token: 0x06000183 RID: 387
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_RequestClanOfficerList(CSteamID steamIDClan);

		// Token: 0x06000184 RID: 388
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanOwner(CSteamID steamIDClan);

		// Token: 0x06000185 RID: 389
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanOfficerCount(CSteamID steamIDClan);

		// Token: 0x06000186 RID: 390
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer);

		// Token: 0x06000187 RID: 391
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamFriends_GetUserRestrictions();

		// Token: 0x06000188 RID: 392
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_SetRichPresence(InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000189 RID: 393
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_ClearRichPresence();

		// Token: 0x0600018A RID: 394
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendRichPresence(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x0600018B RID: 395
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendRichPresenceKeyCount(CSteamID steamIDFriend);

		// Token: 0x0600018C RID: 396
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamFriends_GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey);

		// Token: 0x0600018D RID: 397
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamFriends_RequestFriendRichPresence(CSteamID steamIDFriend);

		// Token: 0x0600018E RID: 398
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_InviteUserToGame(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchConnectString);

		// Token: 0x0600018F RID: 399
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetCoplayFriendCount();

		// Token: 0x06000190 RID: 400
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetCoplayFriend(int iCoplayFriend);

		// Token: 0x06000191 RID: 401
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendCoplayTime(CSteamID steamIDFriend);

		// Token: 0x06000192 RID: 402
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamFriends_GetFriendCoplayGame(CSteamID steamIDFriend);

		// Token: 0x06000193 RID: 403
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_JoinClanChatRoom(CSteamID steamIDClan);

		// Token: 0x06000194 RID: 404
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_LeaveClanChatRoom(CSteamID steamIDClan);

		// Token: 0x06000195 RID: 405
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanChatMemberCount(CSteamID steamIDClan);

		// Token: 0x06000196 RID: 406
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetChatMemberByIndex(CSteamID steamIDClan, int iUser);

		// Token: 0x06000197 RID: 407
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_SendClanChatMessage(CSteamID steamIDClanChat, InteropHelp.UTF8StringHandle pchText);

		// Token: 0x06000198 RID: 408
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetClanChatMessage(CSteamID steamIDClanChat, int iMessage, IntPtr prgchText, int cchTextMax, out EChatEntryType peChatEntryType, out CSteamID psteamidChatter);

		// Token: 0x06000199 RID: 409
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser);

		// Token: 0x0600019A RID: 410
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat);

		// Token: 0x0600019B RID: 411
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_OpenClanChatWindowInSteam(CSteamID steamIDClanChat);

		// Token: 0x0600019C RID: 412
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_CloseClanChatWindowInSteam(CSteamID steamIDClanChat);

		// Token: 0x0600019D RID: 413
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_SetListenForFriendsMessages([MarshalAs(UnmanagedType.I1)] bool bInterceptEnabled);

		// Token: 0x0600019E RID: 414
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamFriends_ReplyToFriendMessage(CSteamID steamIDFriend, InteropHelp.UTF8StringHandle pchMsgToSend);

		// Token: 0x0600019F RID: 415
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamFriends_GetFriendMessage(CSteamID steamIDFriend, int iMessageID, IntPtr pvData, int cubData, out EChatEntryType peChatEntryType);

		// Token: 0x060001A0 RID: 416
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_GetFollowerCount(CSteamID steamID);

		// Token: 0x060001A1 RID: 417
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_IsFollowing(CSteamID steamID);

		// Token: 0x060001A2 RID: 418
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamFriends_EnumerateFollowingList(uint unStartIndex);

		// Token: 0x060001A3 RID: 419
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, AppId_t nGameAppId, InteropHelp.UTF8StringHandle pchVersionString);

		// Token: 0x060001A4 RID: 420
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetProduct(InteropHelp.UTF8StringHandle pszProduct);

		// Token: 0x060001A5 RID: 421
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameDescription(InteropHelp.UTF8StringHandle pszGameDescription);

		// Token: 0x060001A6 RID: 422
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetModDir(InteropHelp.UTF8StringHandle pszModDir);

		// Token: 0x060001A7 RID: 423
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetDedicatedServer([MarshalAs(UnmanagedType.I1)] bool bDedicated);

		// Token: 0x060001A8 RID: 424
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOn(InteropHelp.UTF8StringHandle pszToken);

		// Token: 0x060001A9 RID: 425
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOnAnonymous();

		// Token: 0x060001AA RID: 426
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_LogOff();

		// Token: 0x060001AB RID: 427
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BLoggedOn();

		// Token: 0x060001AC RID: 428
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BSecure();

		// Token: 0x060001AD RID: 429
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_GetSteamID();

		// Token: 0x060001AE RID: 430
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_WasRestartRequested();

		// Token: 0x060001AF RID: 431
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetMaxPlayerCount(int cPlayersMax);

		// Token: 0x060001B0 RID: 432
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetBotPlayerCount(int cBotplayers);

		// Token: 0x060001B1 RID: 433
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetServerName(InteropHelp.UTF8StringHandle pszServerName);

		// Token: 0x060001B2 RID: 434
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetMapName(InteropHelp.UTF8StringHandle pszMapName);

		// Token: 0x060001B3 RID: 435
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetPasswordProtected([MarshalAs(UnmanagedType.I1)] bool bPasswordProtected);

		// Token: 0x060001B4 RID: 436
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetSpectatorPort(ushort unSpectatorPort);

		// Token: 0x060001B5 RID: 437
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetSpectatorServerName(InteropHelp.UTF8StringHandle pszSpectatorServerName);

		// Token: 0x060001B6 RID: 438
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_ClearAllKeyValues();

		// Token: 0x060001B7 RID: 439
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetKeyValue(InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

		// Token: 0x060001B8 RID: 440
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameTags(InteropHelp.UTF8StringHandle pchGameTags);

		// Token: 0x060001B9 RID: 441
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetGameData(InteropHelp.UTF8StringHandle pchGameData);

		// Token: 0x060001BA RID: 442
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetRegion(InteropHelp.UTF8StringHandle pszRegion);

		// Token: 0x060001BB RID: 443
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_SendUserConnectAndAuthenticate(uint unIPClient, [In] [Out] byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser);

		// Token: 0x060001BC RID: 444
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_CreateUnauthenticatedUserConnection();

		// Token: 0x060001BD RID: 445
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SendUserDisconnect(CSteamID steamIDUser);

		// Token: 0x060001BE RID: 446
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_BUpdateUserData(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchPlayerName, uint uScore);

		// Token: 0x060001BF RID: 447
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServer_GetAuthSessionTicket([In] [Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		// Token: 0x060001C0 RID: 448
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EBeginAuthSessionResult ISteamGameServer_BeginAuthSession([In] [Out] byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID);

		// Token: 0x060001C1 RID: 449
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_EndAuthSession(CSteamID steamID);

		// Token: 0x060001C2 RID: 450
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_CancelAuthTicket(HAuthTicket hAuthTicket);

		// Token: 0x060001C3 RID: 451
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EUserHasLicenseForAppResult ISteamGameServer_UserHasLicenseForApp(CSteamID steamID, AppId_t appID);

		// Token: 0x060001C4 RID: 452
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_RequestUserGroupStatus(CSteamID steamIDUser, CSteamID steamIDGroup);

		// Token: 0x060001C5 RID: 453
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_GetGameplayStats();

		// Token: 0x060001C6 RID: 454
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_GetServerReputation();

		// Token: 0x060001C7 RID: 455
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServer_GetPublicIP();

		// Token: 0x060001C8 RID: 456
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServer_HandleIncomingPacket([In] [Out] byte[] pData, int cbData, uint srcIP, ushort srcPort);

		// Token: 0x060001C9 RID: 457
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamGameServer_GetNextOutgoingPacket([In] [Out] byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort);

		// Token: 0x060001CA RID: 458
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_EnableHeartbeats([MarshalAs(UnmanagedType.I1)] bool bActive);

		// Token: 0x060001CB RID: 459
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_SetHeartbeatInterval(int iHeartbeatInterval);

		// Token: 0x060001CC RID: 460
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServer_ForceHeartbeat();

		// Token: 0x060001CD RID: 461
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_AssociateWithClan(CSteamID steamIDClan);

		// Token: 0x060001CE RID: 462
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServer_ComputeNewPlayerCompatibility(CSteamID steamIDNewPlayer);

		// Token: 0x060001CF RID: 463
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerStats_RequestUserStats(CSteamID steamIDUser);

		// Token: 0x060001D0 RID: 464
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out int pData);

		// Token: 0x060001D1 RID: 465
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out float pData);

		// Token: 0x060001D2 RID: 466
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_GetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		// Token: 0x060001D3 RID: 467
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, int nData);

		// Token: 0x060001D4 RID: 468
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, float fData);

		// Token: 0x060001D5 RID: 469
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_UpdateUserAvgRateStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

		// Token: 0x060001D6 RID: 470
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_SetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName);

		// Token: 0x060001D7 RID: 471
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerStats_ClearUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName);

		// Token: 0x060001D8 RID: 472
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerStats_StoreUserStats(CSteamID steamIDUser);

		// Token: 0x060001D9 RID: 473
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTMLSurface_Init();

		// Token: 0x060001DA RID: 474
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTMLSurface_Shutdown();

		// Token: 0x060001DB RID: 475
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamHTMLSurface_CreateBrowser(InteropHelp.UTF8StringHandle pchUserAgent, InteropHelp.UTF8StringHandle pchUserCSS);

		// Token: 0x060001DC RID: 476
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_RemoveBrowser(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001DD RID: 477
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_LoadURL(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchURL, InteropHelp.UTF8StringHandle pchPostData);

		// Token: 0x060001DE RID: 478
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight);

		// Token: 0x060001DF RID: 479
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_StopLoad(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001E0 RID: 480
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_Reload(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001E1 RID: 481
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GoBack(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001E2 RID: 482
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GoForward(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001E3 RID: 483
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_AddHeader(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x060001E4 RID: 484
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_ExecuteJavascript(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchScript);

		// Token: 0x060001E5 RID: 485
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

		// Token: 0x060001E6 RID: 486
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

		// Token: 0x060001E7 RID: 487
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton);

		// Token: 0x060001E8 RID: 488
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseMove(HHTMLBrowser unBrowserHandle, int x, int y);

		// Token: 0x060001E9 RID: 489
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta);

		// Token: 0x060001EA RID: 490
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyDown(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers);

		// Token: 0x060001EB RID: 491
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyUp(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers);

		// Token: 0x060001EC RID: 492
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_KeyChar(HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers);

		// Token: 0x060001ED RID: 493
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

		// Token: 0x060001EE RID: 494
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll);

		// Token: 0x060001EF RID: 495
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetKeyFocus(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bHasKeyFocus);

		// Token: 0x060001F0 RID: 496
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_ViewSource(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001F1 RID: 497
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_CopyToClipboard(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001F2 RID: 498
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_PasteFromClipboard(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001F3 RID: 499
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_Find(HHTMLBrowser unBrowserHandle, InteropHelp.UTF8StringHandle pchSearchStr, [MarshalAs(UnmanagedType.I1)] bool bCurrentlyInFind, [MarshalAs(UnmanagedType.I1)] bool bReverse);

		// Token: 0x060001F4 RID: 500
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_StopFind(HHTMLBrowser unBrowserHandle);

		// Token: 0x060001F5 RID: 501
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y);

		// Token: 0x060001F6 RID: 502
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetCookie(InteropHelp.UTF8StringHandle pchHostname, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue, InteropHelp.UTF8StringHandle pchPath, uint nExpires, [MarshalAs(UnmanagedType.I1)] bool bSecure, [MarshalAs(UnmanagedType.I1)] bool bHTTPOnly);

		// Token: 0x060001F7 RID: 503
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetPageScaleFactor(HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY);

		// Token: 0x060001F8 RID: 504
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_SetBackgroundMode(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bBackgroundMode);

		// Token: 0x060001F9 RID: 505
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_AllowStartRequest(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bAllowed);

		// Token: 0x060001FA RID: 506
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_JSDialogResponse(HHTMLBrowser unBrowserHandle, [MarshalAs(UnmanagedType.I1)] bool bResult);

		// Token: 0x060001FB RID: 507
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamHTMLSurface_FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles);

		// Token: 0x060001FC RID: 508
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamHTTP_CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, InteropHelp.UTF8StringHandle pchAbsoluteURL);

		// Token: 0x060001FD RID: 509
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue);

		// Token: 0x060001FE RID: 510
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds);

		// Token: 0x060001FF RID: 511
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, InteropHelp.UTF8StringHandle pchHeaderValue);

		// Token: 0x06000200 RID: 512
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchParamName, InteropHelp.UTF8StringHandle pchParamValue);

		// Token: 0x06000201 RID: 513
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

		// Token: 0x06000202 RID: 514
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

		// Token: 0x06000203 RID: 515
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_DeferHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000204 RID: 516
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_PrioritizeHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000205 RID: 517
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

		// Token: 0x06000206 RID: 518
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, [In] [Out] byte[] pHeaderValueBuffer, uint unBufferSize);

		// Token: 0x06000207 RID: 519
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize);

		// Token: 0x06000208 RID: 520
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPResponseBodyData(HTTPRequestHandle hRequest, [In] [Out] byte[] pBodyDataBuffer, uint unBufferSize);

		// Token: 0x06000209 RID: 521
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, [In] [Out] byte[] pBodyDataBuffer, uint unBufferSize);

		// Token: 0x0600020A RID: 522
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_ReleaseHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x0600020B RID: 523
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut);

		// Token: 0x0600020C RID: 524
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchContentType, [In] [Out] byte[] pubBody, uint unBodyLen);

		// Token: 0x0600020D RID: 525
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamHTTP_CreateCookieContainer([MarshalAs(UnmanagedType.I1)] bool bAllowResponsesToModify);

		// Token: 0x0600020E RID: 526
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer);

		// Token: 0x0600020F RID: 527
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetCookie(HTTPCookieContainerHandle hCookieContainer, InteropHelp.UTF8StringHandle pchHost, InteropHelp.UTF8StringHandle pchUrl, InteropHelp.UTF8StringHandle pchCookie);

		// Token: 0x06000210 RID: 528
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer);

		// Token: 0x06000211 RID: 529
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchUserAgentInfo);

		// Token: 0x06000212 RID: 530
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, [MarshalAs(UnmanagedType.I1)] bool bRequireVerifiedCertificate);

		// Token: 0x06000213 RID: 531
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds);

		// Token: 0x06000214 RID: 532
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamHTTP_GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut);

		// Token: 0x06000215 RID: 533
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EResult ISteamInventory_GetResultStatus(SteamInventoryResult_t resultHandle);

		// Token: 0x06000216 RID: 534
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetResultItems(SteamInventoryResult_t resultHandle, [In] [Out] SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

		// Token: 0x06000217 RID: 535
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamInventory_GetResultTimestamp(SteamInventoryResult_t resultHandle);

		// Token: 0x06000218 RID: 536
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected);

		// Token: 0x06000219 RID: 537
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamInventory_DestroyResult(SteamInventoryResult_t resultHandle);

		// Token: 0x0600021A RID: 538
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetAllItems(out SteamInventoryResult_t pResultHandle);

		// Token: 0x0600021B RID: 539
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemsByID(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

		// Token: 0x0600021C RID: 540
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_SerializeResult(SteamInventoryResult_t resultHandle, [In] [Out] byte[] pOutBuffer, out uint punOutBufferSize);

		// Token: 0x0600021D RID: 541
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_DeserializeResult(out SteamInventoryResult_t pOutResultHandle, [In] [Out] byte[] pBuffer, uint unBufferSize, [MarshalAs(UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

		// Token: 0x0600021E RID: 542
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GenerateItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayItemDefs, [In] [Out] uint[] punArrayQuantity, uint unArrayLength);

		// Token: 0x0600021F RID: 543
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GrantPromoItems(out SteamInventoryResult_t pResultHandle);

		// Token: 0x06000220 RID: 544
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef);

		// Token: 0x06000221 RID: 545
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_AddPromoItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

		// Token: 0x06000222 RID: 546
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity);

		// Token: 0x06000223 RID: 547
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_ExchangeItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayGenerate, [In] [Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [In] [Out] SteamItemInstanceID_t[] pArrayDestroy, [In] [Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

		// Token: 0x06000224 RID: 548
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest);

		// Token: 0x06000225 RID: 549
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamInventory_SendItemDropHeartbeat();

		// Token: 0x06000226 RID: 550
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition);

		// Token: 0x06000227 RID: 551
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, [In] [Out] SteamItemInstanceID_t[] pArrayGive, [In] [Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [In] [Out] SteamItemInstanceID_t[] pArrayGet, [In] [Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

		// Token: 0x06000228 RID: 552
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_LoadItemDefinitions();

		// Token: 0x06000229 RID: 553
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemDefinitionIDs([In] [Out] SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize);

		// Token: 0x0600022A RID: 554
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamInventory_GetItemDefinitionProperty(SteamItemDef_t iDefinition, InteropHelp.UTF8StringHandle pchPropertyName, IntPtr pchValueBuffer, ref uint punValueBufferSize);

		// Token: 0x0600022B RID: 555
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetFavoriteGameCount();

		// Token: 0x0600022C RID: 556
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetFavoriteGame(int iGame, out AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer);

		// Token: 0x0600022D RID: 557
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_AddFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer);

		// Token: 0x0600022E RID: 558
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_RemoveFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags);

		// Token: 0x0600022F RID: 559
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_RequestLobbyList();

		// Token: 0x06000230 RID: 560
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListStringFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, InteropHelp.UTF8StringHandle pchValueToMatch, ELobbyComparison eComparisonType);

		// Token: 0x06000231 RID: 561
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListNumericalFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToMatch, ELobbyComparison eComparisonType);

		// Token: 0x06000232 RID: 562
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListNearValueFilter(InteropHelp.UTF8StringHandle pchKeyToMatch, int nValueToBeCloseTo);

		// Token: 0x06000233 RID: 563
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable);

		// Token: 0x06000234 RID: 564
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter);

		// Token: 0x06000235 RID: 565
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListResultCountFilter(int cMaxResults);

		// Token: 0x06000236 RID: 566
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(CSteamID steamIDLobby);

		// Token: 0x06000237 RID: 567
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyByIndex(int iLobby);

		// Token: 0x06000238 RID: 568
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_CreateLobby(ELobbyType eLobbyType, int cMaxMembers);

		// Token: 0x06000239 RID: 569
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_JoinLobby(CSteamID steamIDLobby);

		// Token: 0x0600023A RID: 570
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_LeaveLobby(CSteamID steamIDLobby);

		// Token: 0x0600023B RID: 571
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_InviteUserToLobby(CSteamID steamIDLobby, CSteamID steamIDInvitee);

		// Token: 0x0600023C RID: 572
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetNumLobbyMembers(CSteamID steamIDLobby);

		// Token: 0x0600023D RID: 573
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyMemberByIndex(CSteamID steamIDLobby, int iMember);

		// Token: 0x0600023E RID: 574
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmaking_GetLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x0600023F RID: 575
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000240 RID: 576
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyDataCount(CSteamID steamIDLobby);

		// Token: 0x06000241 RID: 577
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, IntPtr pchKey, int cchKeyBufferSize, IntPtr pchValue, int cchValueBufferSize);

		// Token: 0x06000242 RID: 578
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_DeleteLobbyData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000243 RID: 579
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmaking_GetLobbyMemberData(CSteamID steamIDLobby, CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000244 RID: 580
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_SetLobbyMemberData(CSteamID steamIDLobby, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000245 RID: 581
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SendLobbyChatMsg(CSteamID steamIDLobby, [In] [Out] byte[] pvMsgBody, int cubMsgBody);

		// Token: 0x06000246 RID: 582
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyChatEntry(CSteamID steamIDLobby, int iChatID, out CSteamID pSteamIDUser, [In] [Out] byte[] pvData, int cubData, out EChatEntryType peChatEntryType);

		// Token: 0x06000247 RID: 583
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_RequestLobbyData(CSteamID steamIDLobby);

		// Token: 0x06000248 RID: 584
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmaking_SetLobbyGameServer(CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, CSteamID steamIDGameServer);

		// Token: 0x06000249 RID: 585
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_GetLobbyGameServer(CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out CSteamID psteamIDGameServer);

		// Token: 0x0600024A RID: 586
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyMemberLimit(CSteamID steamIDLobby, int cMaxMembers);

		// Token: 0x0600024B RID: 587
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmaking_GetLobbyMemberLimit(CSteamID steamIDLobby);

		// Token: 0x0600024C RID: 588
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyType(CSteamID steamIDLobby, ELobbyType eLobbyType);

		// Token: 0x0600024D RID: 589
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyJoinable(CSteamID steamIDLobby, [MarshalAs(UnmanagedType.I1)] bool bLobbyJoinable);

		// Token: 0x0600024E RID: 590
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamMatchmaking_GetLobbyOwner(CSteamID steamIDLobby);

		// Token: 0x0600024F RID: 591
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLobbyOwner(CSteamID steamIDLobby, CSteamID steamIDNewOwner);

		// Token: 0x06000250 RID: 592
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmaking_SetLinkedLobby(CSteamID steamIDLobby, CSteamID steamIDLobbyDependent);

		// Token: 0x06000251 RID: 593
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestInternetServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000252 RID: 594
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestLANServerList(AppId_t iApp, IntPtr pRequestServersResponse);

		// Token: 0x06000253 RID: 595
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestFriendsServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000254 RID: 596
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestFavoritesServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000255 RID: 597
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestHistoryServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000256 RID: 598
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_RequestSpectatorServerList(AppId_t iApp, IntPtr ppchFilters, uint nFilters, IntPtr pRequestServersResponse);

		// Token: 0x06000257 RID: 599
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_ReleaseRequest(HServerListRequest hServerListRequest);

		// Token: 0x06000258 RID: 600
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamMatchmakingServers_GetServerDetails(HServerListRequest hRequest, int iServer);

		// Token: 0x06000259 RID: 601
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_CancelQuery(HServerListRequest hRequest);

		// Token: 0x0600025A RID: 602
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_RefreshQuery(HServerListRequest hRequest);

		// Token: 0x0600025B RID: 603
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMatchmakingServers_IsRefreshing(HServerListRequest hRequest);

		// Token: 0x0600025C RID: 604
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_GetServerCount(HServerListRequest hRequest);

		// Token: 0x0600025D RID: 605
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_RefreshServer(HServerListRequest hRequest, int iServer);

		// Token: 0x0600025E RID: 606
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_PingServer(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

		// Token: 0x0600025F RID: 607
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_PlayerDetails(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

		// Token: 0x06000260 RID: 608
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamMatchmakingServers_ServerRules(uint unIP, ushort usPort, IntPtr pRequestServersResponse);

		// Token: 0x06000261 RID: 609
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMatchmakingServers_CancelServerQuery(HServerQuery hServerQuery);

		// Token: 0x06000262 RID: 610
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusic_BIsEnabled();

		// Token: 0x06000263 RID: 611
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusic_BIsPlaying();

		// Token: 0x06000264 RID: 612
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern AudioPlayback_Status ISteamMusic_GetPlaybackStatus();

		// Token: 0x06000265 RID: 613
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_Play();

		// Token: 0x06000266 RID: 614
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_Pause();

		// Token: 0x06000267 RID: 615
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_PlayPrevious();

		// Token: 0x06000268 RID: 616
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_PlayNext();

		// Token: 0x06000269 RID: 617
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamMusic_SetVolume(float flVolume);

		// Token: 0x0600026A RID: 618
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern float ISteamMusic_GetVolume();

		// Token: 0x0600026B RID: 619
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_RegisterSteamMusicRemote(InteropHelp.UTF8StringHandle pchName);

		// Token: 0x0600026C RID: 620
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_DeregisterSteamMusicRemote();

		// Token: 0x0600026D RID: 621
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_BIsCurrentMusicRemote();

		// Token: 0x0600026E RID: 622
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_BActivationSuccess([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x0600026F RID: 623
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetDisplayName(InteropHelp.UTF8StringHandle pchDisplayName);

		// Token: 0x06000270 RID: 624
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetPNGIcon_64x64([In] [Out] byte[] pvBuffer, uint cbBufferLength);

		// Token: 0x06000271 RID: 625
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlayPrevious([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000272 RID: 626
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlayNext([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000273 RID: 627
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableShuffled([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000274 RID: 628
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableLooped([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000275 RID: 629
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnableQueue([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000276 RID: 630
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_EnablePlaylists([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000277 RID: 631
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdatePlaybackStatus(AudioPlayback_Status nStatus);

		// Token: 0x06000278 RID: 632
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateShuffled([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x06000279 RID: 633
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateLooped([MarshalAs(UnmanagedType.I1)] bool bValue);

		// Token: 0x0600027A RID: 634
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateVolume(float flValue);

		// Token: 0x0600027B RID: 635
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryWillChange();

		// Token: 0x0600027C RID: 636
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryIsAvailable([MarshalAs(UnmanagedType.I1)] bool bAvailable);

		// Token: 0x0600027D RID: 637
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryText(InteropHelp.UTF8StringHandle pchText);

		// Token: 0x0600027E RID: 638
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(int nValue);

		// Token: 0x0600027F RID: 639
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_UpdateCurrentEntryCoverArt([In] [Out] byte[] pvBuffer, uint cbBufferLength);

		// Token: 0x06000280 RID: 640
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_CurrentEntryDidChange();

		// Token: 0x06000281 RID: 641
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_QueueWillChange();

		// Token: 0x06000282 RID: 642
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_ResetQueueEntries();

		// Token: 0x06000283 RID: 643
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetQueueEntry(int nID, int nPosition, InteropHelp.UTF8StringHandle pchEntryText);

		// Token: 0x06000284 RID: 644
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetCurrentQueueEntry(int nID);

		// Token: 0x06000285 RID: 645
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_QueueDidChange();

		// Token: 0x06000286 RID: 646
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_PlaylistWillChange();

		// Token: 0x06000287 RID: 647
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_ResetPlaylistEntries();

		// Token: 0x06000288 RID: 648
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetPlaylistEntry(int nID, int nPosition, InteropHelp.UTF8StringHandle pchEntryText);

		// Token: 0x06000289 RID: 649
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_SetCurrentPlaylistEntry(int nID);

		// Token: 0x0600028A RID: 650
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamMusicRemote_PlaylistDidChange();

		// Token: 0x0600028B RID: 651
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_SendP2PPacket(CSteamID steamIDRemote, [In] [Out] byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel);

		// Token: 0x0600028C RID: 652
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel);

		// Token: 0x0600028D RID: 653
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_ReadP2PPacket([In] [Out] byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel);

		// Token: 0x0600028E RID: 654
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_AcceptP2PSessionWithUser(CSteamID steamIDRemote);

		// Token: 0x0600028F RID: 655
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_CloseP2PSessionWithUser(CSteamID steamIDRemote);

		// Token: 0x06000290 RID: 656
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel);

		// Token: 0x06000291 RID: 657
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState);

		// Token: 0x06000292 RID: 658
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_AllowP2PPacketRelay([MarshalAs(UnmanagedType.I1)] bool bAllow);

		// Token: 0x06000293 RID: 659
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		// Token: 0x06000294 RID: 660
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		// Token: 0x06000295 RID: 661
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamNetworking_CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec);

		// Token: 0x06000296 RID: 662
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_DestroySocket(SNetSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

		// Token: 0x06000297 RID: 663
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_DestroyListenSocket(SNetListenSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

		// Token: 0x06000298 RID: 664
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_SendDataOnSocket(SNetSocket_t hSocket, IntPtr pubData, uint cubData, [MarshalAs(UnmanagedType.I1)] bool bReliable);

		// Token: 0x06000299 RID: 665
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize);

		// Token: 0x0600029A RID: 666
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_RetrieveDataFromSocket(SNetSocket_t hSocket, IntPtr pubDest, uint cubDest, out uint pcubMsgSize);

		// Token: 0x0600029B RID: 667
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket);

		// Token: 0x0600029C RID: 668
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_RetrieveData(SNetListenSocket_t hListenSocket, IntPtr pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket);

		// Token: 0x0600029D RID: 669
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote);

		// Token: 0x0600029E RID: 670
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamNetworking_GetListenSocketInfo(SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort);

		// Token: 0x0600029F RID: 671
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ESNetSocketConnectionType ISteamNetworking_GetSocketConnectionType(SNetSocket_t hSocket);

		// Token: 0x060002A0 RID: 672
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamNetworking_GetMaxPacketSize(SNetSocket_t hSocket);

		// Token: 0x060002A1 RID: 673
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWrite(InteropHelp.UTF8StringHandle pchFile, [In] [Out] byte[] pvData, int cubData);

		// Token: 0x060002A2 RID: 674
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_FileRead(InteropHelp.UTF8StringHandle pchFile, [In] [Out] byte[] pvData, int cubDataToRead);

		// Token: 0x060002A3 RID: 675
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileForget(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002A4 RID: 676
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileDelete(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002A5 RID: 677
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileShare(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002A6 RID: 678
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_SetSyncPlatforms(InteropHelp.UTF8StringHandle pchFile, ERemoteStoragePlatform eRemoteStoragePlatform);

		// Token: 0x060002A7 RID: 679
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_FileWriteStreamOpen(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002A8 RID: 680
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamWriteChunk(UGCFileWriteStreamHandle_t writeHandle, [In] [Out] byte[] pvData, int cubData);

		// Token: 0x060002A9 RID: 681
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamClose(UGCFileWriteStreamHandle_t writeHandle);

		// Token: 0x060002AA RID: 682
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileWriteStreamCancel(UGCFileWriteStreamHandle_t writeHandle);

		// Token: 0x060002AB RID: 683
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FileExists(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002AC RID: 684
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_FilePersisted(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002AD RID: 685
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetFileSize(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002AE RID: 686
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern long ISteamRemoteStorage_GetFileTimestamp(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002AF RID: 687
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ERemoteStoragePlatform ISteamRemoteStorage_GetSyncPlatforms(InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002B0 RID: 688
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetFileCount();

		// Token: 0x060002B1 RID: 689
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamRemoteStorage_GetFileNameAndSize(int iFile, out int pnFileSizeInBytes);

		// Token: 0x060002B2 RID: 690
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetQuota(out int pnTotalBytes, out int puAvailableBytes);

		// Token: 0x060002B3 RID: 691
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_IsCloudEnabledForAccount();

		// Token: 0x060002B4 RID: 692
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_IsCloudEnabledForApp();

		// Token: 0x060002B5 RID: 693
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamRemoteStorage_SetCloudEnabledForApp([MarshalAs(UnmanagedType.I1)] bool bEnabled);

		// Token: 0x060002B6 RID: 694
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UGCDownload(UGCHandle_t hContent, uint unPriority);

		// Token: 0x060002B7 RID: 695
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetUGCDownloadProgress(UGCHandle_t hContent, out int pnBytesDownloaded, out int pnBytesExpected);

		// Token: 0x060002B8 RID: 696
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_GetUGCDetails(UGCHandle_t hContent, out AppId_t pnAppID, out IntPtr ppchName, out int pnFileSizeInBytes, out CSteamID pSteamIDOwner);

		// Token: 0x060002B9 RID: 697
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_UGCRead(UGCHandle_t hContent, [In] [Out] byte[] pvData, int cubDataToRead, uint cOffset, EUGCReadAction eAction);

		// Token: 0x060002BA RID: 698
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamRemoteStorage_GetCachedUGCCount();

		// Token: 0x060002BB RID: 699
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetCachedUGCHandle(int iCachedContent);

		// Token: 0x060002BC RID: 700
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_PublishWorkshopFile(InteropHelp.UTF8StringHandle pchFile, InteropHelp.UTF8StringHandle pchPreviewFile, AppId_t nConsumerAppId, InteropHelp.UTF8StringHandle pchTitle, InteropHelp.UTF8StringHandle pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IntPtr pTags, EWorkshopFileType eWorkshopFileType);

		// Token: 0x060002BD RID: 701
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_CreatePublishedFileUpdateRequest(PublishedFileId_t unPublishedFileId);

		// Token: 0x060002BE RID: 702
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileFile(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchFile);

		// Token: 0x060002BF RID: 703
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFilePreviewFile(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchPreviewFile);

		// Token: 0x060002C0 RID: 704
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileTitle(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchTitle);

		// Token: 0x060002C1 RID: 705
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileDescription(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchDescription);

		// Token: 0x060002C2 RID: 706
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileVisibility(PublishedFileUpdateHandle_t updateHandle, ERemoteStoragePublishedFileVisibility eVisibility);

		// Token: 0x060002C3 RID: 707
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileTags(PublishedFileUpdateHandle_t updateHandle, IntPtr pTags);

		// Token: 0x060002C4 RID: 708
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_CommitPublishedFileUpdate(PublishedFileUpdateHandle_t updateHandle);

		// Token: 0x060002C5 RID: 709
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetPublishedFileDetails(PublishedFileId_t unPublishedFileId, uint unMaxSecondsOld);

		// Token: 0x060002C6 RID: 710
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_DeletePublishedFile(PublishedFileId_t unPublishedFileId);

		// Token: 0x060002C7 RID: 711
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserPublishedFiles(uint unStartIndex);

		// Token: 0x060002C8 RID: 712
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_SubscribePublishedFile(PublishedFileId_t unPublishedFileId);

		// Token: 0x060002C9 RID: 713
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserSubscribedFiles(uint unStartIndex);

		// Token: 0x060002CA RID: 714
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UnsubscribePublishedFile(PublishedFileId_t unPublishedFileId);

		// Token: 0x060002CB RID: 715
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamRemoteStorage_UpdatePublishedFileSetChangeDescription(PublishedFileUpdateHandle_t updateHandle, InteropHelp.UTF8StringHandle pchChangeDescription);

		// Token: 0x060002CC RID: 716
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId);

		// Token: 0x060002CD RID: 717
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UpdateUserPublishedItemVote(PublishedFileId_t unPublishedFileId, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

		// Token: 0x060002CE RID: 718
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_GetUserPublishedItemVoteDetails(PublishedFileId_t unPublishedFileId);

		// Token: 0x060002CF RID: 719
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumerateUserSharedWorkshopFiles(CSteamID steamId, uint unStartIndex, IntPtr pRequiredTags, IntPtr pExcludedTags);

		// Token: 0x060002D0 RID: 720
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_PublishVideo(EWorkshopVideoProvider eVideoProvider, InteropHelp.UTF8StringHandle pchVideoAccount, InteropHelp.UTF8StringHandle pchVideoIdentifier, InteropHelp.UTF8StringHandle pchPreviewFile, AppId_t nConsumerAppId, InteropHelp.UTF8StringHandle pchTitle, InteropHelp.UTF8StringHandle pchDescription, ERemoteStoragePublishedFileVisibility eVisibility, IntPtr pTags);

		// Token: 0x060002D1 RID: 721
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_SetUserPublishedFileAction(PublishedFileId_t unPublishedFileId, EWorkshopFileAction eAction);

		// Token: 0x060002D2 RID: 722
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumeratePublishedFilesByUserAction(EWorkshopFileAction eAction, uint unStartIndex);

		// Token: 0x060002D3 RID: 723
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_EnumeratePublishedWorkshopFiles(EWorkshopEnumerationType eEnumerationType, uint unStartIndex, uint unCount, uint unDays, IntPtr pTags, IntPtr pUserTags);

		// Token: 0x060002D4 RID: 724
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamRemoteStorage_UGCDownloadToLocation(UGCHandle_t hContent, InteropHelp.UTF8StringHandle pchLocation, uint unPriority);

		// Token: 0x060002D5 RID: 725
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamScreenshots_WriteScreenshot([In] [Out] byte[] pubRGB, uint cubRGB, int nWidth, int nHeight);

		// Token: 0x060002D6 RID: 726
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamScreenshots_AddScreenshotToLibrary(InteropHelp.UTF8StringHandle pchFilename, InteropHelp.UTF8StringHandle pchThumbnailFilename, int nWidth, int nHeight);

		// Token: 0x060002D7 RID: 727
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamScreenshots_TriggerScreenshot();

		// Token: 0x060002D8 RID: 728
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamScreenshots_HookScreenshots([MarshalAs(UnmanagedType.I1)] bool bHook);

		// Token: 0x060002D9 RID: 729
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_SetLocation(ScreenshotHandle hScreenshot, InteropHelp.UTF8StringHandle pchLocation);

		// Token: 0x060002DA RID: 730
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_TagUser(ScreenshotHandle hScreenshot, CSteamID steamID);

		// Token: 0x060002DB RID: 731
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamScreenshots_TagPublishedFile(ScreenshotHandle hScreenshot, PublishedFileId_t unPublishedFileID);

		// Token: 0x060002DC RID: 732
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		// Token: 0x060002DD RID: 733
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		// Token: 0x060002DE RID: 734
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateQueryUGCDetailsRequest([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		// Token: 0x060002DF RID: 735
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SendQueryUGCRequest(UGCQueryHandle_t handle);

		// Token: 0x060002E0 RID: 736
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails);

		// Token: 0x060002E1 RID: 737
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, IntPtr pchURL, uint cchURLSize);

		// Token: 0x060002E2 RID: 738
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, IntPtr pchMetadata, uint cchMetadatasize);

		// Token: 0x060002E3 RID: 739
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, [In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		// Token: 0x060002E4 RID: 740
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out uint pStatValue);

		// Token: 0x060002E5 RID: 741
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index);

		// Token: 0x060002E6 RID: 742
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, IntPtr pchURLOrVideoID, uint cchURLSize, out bool pbIsImage);

		// Token: 0x060002E7 RID: 743
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index);

		// Token: 0x060002E8 RID: 744
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, IntPtr pchKey, uint cchKeySize, IntPtr pchValue, uint cchValueSize);

		// Token: 0x060002E9 RID: 745
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_ReleaseQueryUGCRequest(UGCQueryHandle_t handle);

		// Token: 0x060002EA RID: 746
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddRequiredTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

		// Token: 0x060002EB RID: 747
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddExcludedTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

		// Token: 0x060002EC RID: 748
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnKeyValueTags(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnKeyValueTags);

		// Token: 0x060002ED RID: 749
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnLongDescription(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnLongDescription);

		// Token: 0x060002EE RID: 750
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnMetadata(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnMetadata);

		// Token: 0x060002EF RID: 751
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnChildren(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnChildren);

		// Token: 0x060002F0 RID: 752
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnAdditionalPreviews(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnAdditionalPreviews);

		// Token: 0x060002F1 RID: 753
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetReturnTotalOnly(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnTotalOnly);

		// Token: 0x060002F2 RID: 754
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetLanguage(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

		// Token: 0x060002F3 RID: 755
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds);

		// Token: 0x060002F4 RID: 756
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetCloudFileNameFilter(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pMatchCloudFileName);

		// Token: 0x060002F5 RID: 757
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetMatchAnyTag(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bMatchAnyTag);

		// Token: 0x060002F6 RID: 758
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetSearchText(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pSearchText);

		// Token: 0x060002F7 RID: 759
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays);

		// Token: 0x060002F8 RID: 760
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddRequiredKeyValueTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

		// Token: 0x060002F9 RID: 761
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

		// Token: 0x060002FA RID: 762
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType);

		// Token: 0x060002FB RID: 763
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x060002FC RID: 764
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemTitle(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchTitle);

		// Token: 0x060002FD RID: 765
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemDescription(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchDescription);

		// Token: 0x060002FE RID: 766
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemUpdateLanguage(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

		// Token: 0x060002FF RID: 767
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemMetadata(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchMetaData);

		// Token: 0x06000300 RID: 768
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility);

		// Token: 0x06000301 RID: 769
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemTags(UGCUpdateHandle_t updateHandle, IntPtr pTags);

		// Token: 0x06000302 RID: 770
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemContent(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszContentFolder);

		// Token: 0x06000303 RID: 771
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_SetItemPreview(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile);

		// Token: 0x06000304 RID: 772
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_RemoveItemKeyValueTags(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x06000305 RID: 773
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_AddItemKeyValueTag(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x06000306 RID: 774
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SubmitItemUpdate(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchChangeNote);

		// Token: 0x06000307 RID: 775
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EItemUpdateStatus ISteamUGC_GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

		// Token: 0x06000308 RID: 776
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SetUserItemVote(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

		// Token: 0x06000309 RID: 777
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_GetUserItemVote(PublishedFileId_t nPublishedFileID);

		// Token: 0x0600030A RID: 778
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x0600030B RID: 779
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x0600030C RID: 780
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_SubscribeItem(PublishedFileId_t nPublishedFileID);

		// Token: 0x0600030D RID: 781
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUGC_UnsubscribeItem(PublishedFileId_t nPublishedFileID);

		// Token: 0x0600030E RID: 782
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetNumSubscribedItems();

		// Token: 0x0600030F RID: 783
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetSubscribedItems([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		// Token: 0x06000310 RID: 784
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUGC_GetItemState(PublishedFileId_t nPublishedFileID);

		// Token: 0x06000311 RID: 785
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, IntPtr pchFolder, uint cchFolderSize, out uint punTimeStamp);

		// Token: 0x06000312 RID: 786
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		// Token: 0x06000313 RID: 787
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUGC_DownloadItem(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bHighPriority);

		// Token: 0x06000314 RID: 788
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUnifiedMessages_SendMethod(InteropHelp.UTF8StringHandle pchServiceMethod, [In] [Out] byte[] pRequestBuffer, uint unRequestBufferSize, ulong unContext);

		// Token: 0x06000315 RID: 789
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_GetMethodResponseInfo(ClientUnifiedMessageHandle hHandle, out uint punResponseSize, out EResult peResult);

		// Token: 0x06000316 RID: 790
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_GetMethodResponseData(ClientUnifiedMessageHandle hHandle, [In] [Out] byte[] pResponseBuffer, uint unResponseBufferSize, [MarshalAs(UnmanagedType.I1)] bool bAutoRelease);

		// Token: 0x06000317 RID: 791
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_ReleaseMethod(ClientUnifiedMessageHandle hHandle);

		// Token: 0x06000318 RID: 792
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUnifiedMessages_SendNotification(InteropHelp.UTF8StringHandle pchServiceNotification, [In] [Out] byte[] pNotificationBuffer, uint unNotificationBufferSize);

		// Token: 0x06000319 RID: 793
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetHSteamUser();

		// Token: 0x0600031A RID: 794
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_BLoggedOn();

		// Token: 0x0600031B RID: 795
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_GetSteamID();

		// Token: 0x0600031C RID: 796
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUser_InitiateGameConnection([In] [Out] byte[] pAuthBlob, int cbMaxAuthBlob, CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer, [MarshalAs(UnmanagedType.I1)] bool bSecure);

		// Token: 0x0600031D RID: 797
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_TerminateGameConnection(uint unIPServer, ushort usPortServer);

		// Token: 0x0600031E RID: 798
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_TrackAppUsageEvent(CGameID gameID, int eAppUsageEvent, InteropHelp.UTF8StringHandle pchExtraInfo);

		// Token: 0x0600031F RID: 799
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_GetUserDataFolder(IntPtr pchBuffer, int cubBuffer);

		// Token: 0x06000320 RID: 800
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_StartVoiceRecording();

		// Token: 0x06000321 RID: 801
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_StopVoiceRecording();

		// Token: 0x06000322 RID: 802
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EVoiceResult ISteamUser_GetAvailableVoice(out uint pcbCompressed, out uint pcbUncompressed, uint nUncompressedVoiceDesiredSampleRate);

		// Token: 0x06000323 RID: 803
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EVoiceResult ISteamUser_GetVoice([MarshalAs(UnmanagedType.I1)] bool bWantCompressed, [In] [Out] byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, [MarshalAs(UnmanagedType.I1)] bool bWantUncompressed, [In] [Out] byte[] pUncompressedDestBuffer, uint cbUncompressedDestBufferSize, out uint nUncompressBytesWritten, uint nUncompressedVoiceDesiredSampleRate);

		// Token: 0x06000324 RID: 804
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EVoiceResult ISteamUser_DecompressVoice([In] [Out] byte[] pCompressed, uint cbCompressed, [In] [Out] byte[] pDestBuffer, uint cbDestBufferSize, out uint nBytesWritten, uint nDesiredSampleRate);

		// Token: 0x06000325 RID: 805
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUser_GetVoiceOptimalSampleRate();

		// Token: 0x06000326 RID: 806
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUser_GetAuthSessionTicket([In] [Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		// Token: 0x06000327 RID: 807
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EBeginAuthSessionResult ISteamUser_BeginAuthSession([In] [Out] byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID);

		// Token: 0x06000328 RID: 808
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_EndAuthSession(CSteamID steamID);

		// Token: 0x06000329 RID: 809
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_CancelAuthTicket(HAuthTicket hAuthTicket);

		// Token: 0x0600032A RID: 810
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EUserHasLicenseForAppResult ISteamUser_UserHasLicenseForApp(CSteamID steamID, AppId_t appID);

		// Token: 0x0600032B RID: 811
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_BIsBehindNAT();

		// Token: 0x0600032C RID: 812
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUser_AdvertiseGame(CSteamID steamIDGameServer, uint unIPServer, ushort usPortServer);

		// Token: 0x0600032D RID: 813
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_RequestEncryptedAppTicket([In] [Out] byte[] pDataToInclude, int cbDataToInclude);

		// Token: 0x0600032E RID: 814
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUser_GetEncryptedAppTicket([In] [Out] byte[] pTicket, int cbMaxTicket, out uint pcbTicket);

		// Token: 0x0600032F RID: 815
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetGameBadgeLevel(int nSeries, [MarshalAs(UnmanagedType.I1)] bool bFoil);

		// Token: 0x06000330 RID: 816
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUser_GetPlayerSteamLevel();

		// Token: 0x06000331 RID: 817
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUser_RequestStoreAuthURL(InteropHelp.UTF8StringHandle pchRedirectURL);

		// Token: 0x06000332 RID: 818
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_RequestCurrentStats();

		// Token: 0x06000333 RID: 819
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetStat(InteropHelp.UTF8StringHandle pchName, out int pData);

		// Token: 0x06000334 RID: 820
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetStat_(InteropHelp.UTF8StringHandle pchName, out float pData);

		// Token: 0x06000335 RID: 821
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetStat(InteropHelp.UTF8StringHandle pchName, int nData);

		// Token: 0x06000336 RID: 822
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetStat_(InteropHelp.UTF8StringHandle pchName, float fData);

		// Token: 0x06000337 RID: 823
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_UpdateAvgRateStat(InteropHelp.UTF8StringHandle pchName, float flCountThisSession, double dSessionLength);

		// Token: 0x06000338 RID: 824
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievement(InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		// Token: 0x06000339 RID: 825
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_SetAchievement(InteropHelp.UTF8StringHandle pchName);

		// Token: 0x0600033A RID: 826
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_ClearAchievement(InteropHelp.UTF8StringHandle pchName);

		// Token: 0x0600033B RID: 827
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievementAndUnlockTime(InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

		// Token: 0x0600033C RID: 828
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_StoreStats();

		// Token: 0x0600033D RID: 829
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetAchievementIcon(InteropHelp.UTF8StringHandle pchName);

		// Token: 0x0600033E RID: 830
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUserStats_GetAchievementDisplayAttribute(InteropHelp.UTF8StringHandle pchName, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x0600033F RID: 831
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_IndicateAchievementProgress(InteropHelp.UTF8StringHandle pchName, uint nCurProgress, uint nMaxProgress);

		// Token: 0x06000340 RID: 832
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUserStats_GetNumAchievements();

		// Token: 0x06000341 RID: 833
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUserStats_GetAchievementName(uint iAchievement);

		// Token: 0x06000342 RID: 834
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestUserStats(CSteamID steamIDUser);

		// Token: 0x06000343 RID: 835
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserStat(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out int pData);

		// Token: 0x06000344 RID: 836
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserStat_(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out float pData);

		// Token: 0x06000345 RID: 837
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserAchievement(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved);

		// Token: 0x06000346 RID: 838
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetUserAchievementAndUnlockTime(CSteamID steamIDUser, InteropHelp.UTF8StringHandle pchName, out bool pbAchieved, out uint punUnlockTime);

		// Token: 0x06000347 RID: 839
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_ResetAllStats([MarshalAs(UnmanagedType.I1)] bool bAchievementsToo);

		// Token: 0x06000348 RID: 840
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_FindOrCreateLeaderboard(InteropHelp.UTF8StringHandle pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType);

		// Token: 0x06000349 RID: 841
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_FindLeaderboard(InteropHelp.UTF8StringHandle pchLeaderboardName);

		// Token: 0x0600034A RID: 842
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUserStats_GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard);

		// Token: 0x0600034B RID: 843
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard);

		// Token: 0x0600034C RID: 844
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ELeaderboardSortMethod ISteamUserStats_GetLeaderboardSortMethod(SteamLeaderboard_t hSteamLeaderboard);

		// Token: 0x0600034D RID: 845
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ELeaderboardDisplayType ISteamUserStats_GetLeaderboardDisplayType(SteamLeaderboard_t hSteamLeaderboard);

		// Token: 0x0600034E RID: 846
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_DownloadLeaderboardEntries(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd);

		// Token: 0x0600034F RID: 847
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_DownloadLeaderboardEntriesForUsers(SteamLeaderboard_t hSteamLeaderboard, [In] [Out] CSteamID[] prgUsers, int cUsers);

		// Token: 0x06000350 RID: 848
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetDownloadedLeaderboardEntry(SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, [In] [Out] int[] pDetails, int cDetailsMax);

		// Token: 0x06000351 RID: 849
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_UploadLeaderboardScore(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, [In] [Out] int[] pScoreDetails, int cScoreDetailsCount);

		// Token: 0x06000352 RID: 850
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_AttachLeaderboardUGC(SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC);

		// Token: 0x06000353 RID: 851
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_GetNumberOfCurrentPlayers();

		// Token: 0x06000354 RID: 852
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestGlobalAchievementPercentages();

		// Token: 0x06000355 RID: 853
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetMostAchievedAchievementInfo(IntPtr pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

		// Token: 0x06000356 RID: 854
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetNextMostAchievedAchievementInfo(int iIteratorPrevious, IntPtr pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved);

		// Token: 0x06000357 RID: 855
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetAchievementAchievedPercent(InteropHelp.UTF8StringHandle pchName, out float pflPercent);

		// Token: 0x06000358 RID: 856
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUserStats_RequestGlobalStats(int nHistoryDays);

		// Token: 0x06000359 RID: 857
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetGlobalStat(InteropHelp.UTF8StringHandle pchStatName, out long pData);

		// Token: 0x0600035A RID: 858
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUserStats_GetGlobalStat_(InteropHelp.UTF8StringHandle pchStatName, out double pData);

		// Token: 0x0600035B RID: 859
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetGlobalStatHistory(InteropHelp.UTF8StringHandle pchStatName, [In] [Out] long[] pData, uint cubData);

		// Token: 0x0600035C RID: 860
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamUserStats_GetGlobalStatHistory_(InteropHelp.UTF8StringHandle pchStatName, [In] [Out] double[] pData, uint cubData);

		// Token: 0x0600035D RID: 861
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetSecondsSinceAppActive();

		// Token: 0x0600035E RID: 862
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetSecondsSinceComputerActive();

		// Token: 0x0600035F RID: 863
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EUniverse ISteamUtils_GetConnectedUniverse();

		// Token: 0x06000360 RID: 864
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetServerRealTime();

		// Token: 0x06000361 RID: 865
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUtils_GetIPCountry();

		// Token: 0x06000362 RID: 866
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetImageSize(int iImage, out uint pnWidth, out uint pnHeight);

		// Token: 0x06000363 RID: 867
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetImageRGBA(int iImage, [In] [Out] byte[] pubDest, int nDestBufferSize);

		// Token: 0x06000364 RID: 868
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetCSERIPPort(out uint unIP, out ushort usPort);

		// Token: 0x06000365 RID: 869
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte ISteamUtils_GetCurrentBatteryPower();

		// Token: 0x06000366 RID: 870
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetAppID();

		// Token: 0x06000367 RID: 871
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition);

		// Token: 0x06000368 RID: 872
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed);

		// Token: 0x06000369 RID: 873
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ESteamAPICallFailure ISteamUtils_GetAPICallFailureReason(SteamAPICall_t hSteamAPICall);

		// Token: 0x0600036A RID: 874
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

		// Token: 0x0600036B RID: 875
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_RunFrame();

		// Token: 0x0600036C RID: 876
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetIPCCallCount();

		// Token: 0x0600036D RID: 877
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

		// Token: 0x0600036E RID: 878
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsOverlayEnabled();

		// Token: 0x0600036F RID: 879
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_BOverlayNeedsPresent();

		// Token: 0x06000370 RID: 880
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamUtils_CheckFileSignature(InteropHelp.UTF8StringHandle szFileName);

		// Token: 0x06000371 RID: 881
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, InteropHelp.UTF8StringHandle pchExistingText);

		// Token: 0x06000372 RID: 882
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamUtils_GetEnteredGamepadTextLength();

		// Token: 0x06000373 RID: 883
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_GetEnteredGamepadTextInput(IntPtr pchText, uint cchText);

		// Token: 0x06000374 RID: 884
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamUtils_GetSteamUILanguage();

		// Token: 0x06000375 RID: 885
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamUtils_IsSteamRunningInVR();

		// Token: 0x06000376 RID: 886
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamUtils_SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset);

		// Token: 0x06000377 RID: 887
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamVideo_GetVideoURL(AppId_t unVideoAppID);

		// Token: 0x06000378 RID: 888
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamVideo_IsBroadcasting(out int pnNumViewers);

		// Token: 0x06000379 RID: 889
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerHTTP_CreateHTTPRequest(EHTTPMethod eHTTPRequestMethod, InteropHelp.UTF8StringHandle pchAbsoluteURL);

		// Token: 0x0600037A RID: 890
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestContextValue(HTTPRequestHandle hRequest, ulong ulContextValue);

		// Token: 0x0600037B RID: 891
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestNetworkActivityTimeout(HTTPRequestHandle hRequest, uint unTimeoutSeconds);

		// Token: 0x0600037C RID: 892
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, InteropHelp.UTF8StringHandle pchHeaderValue);

		// Token: 0x0600037D RID: 893
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestGetOrPostParameter(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchParamName, InteropHelp.UTF8StringHandle pchParamValue);

		// Token: 0x0600037E RID: 894
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SendHTTPRequest(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

		// Token: 0x0600037F RID: 895
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SendHTTPRequestAndStreamResponse(HTTPRequestHandle hRequest, out SteamAPICall_t pCallHandle);

		// Token: 0x06000380 RID: 896
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_DeferHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000381 RID: 897
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_PrioritizeHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000382 RID: 898
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseHeaderSize(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, out uint unResponseHeaderSize);

		// Token: 0x06000383 RID: 899
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseHeaderValue(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchHeaderName, [In] [Out] byte[] pHeaderValueBuffer, uint unBufferSize);

		// Token: 0x06000384 RID: 900
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseBodySize(HTTPRequestHandle hRequest, out uint unBodySize);

		// Token: 0x06000385 RID: 901
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPResponseBodyData(HTTPRequestHandle hRequest, [In] [Out] byte[] pBodyDataBuffer, uint unBufferSize);

		// Token: 0x06000386 RID: 902
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPStreamingResponseBodyData(HTTPRequestHandle hRequest, uint cOffset, [In] [Out] byte[] pBodyDataBuffer, uint unBufferSize);

		// Token: 0x06000387 RID: 903
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_ReleaseHTTPRequest(HTTPRequestHandle hRequest);

		// Token: 0x06000388 RID: 904
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPDownloadProgressPct(HTTPRequestHandle hRequest, out float pflPercentOut);

		// Token: 0x06000389 RID: 905
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestRawPostBody(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchContentType, [In] [Out] byte[] pubBody, uint unBodyLen);

		// Token: 0x0600038A RID: 906
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerHTTP_CreateCookieContainer([MarshalAs(UnmanagedType.I1)] bool bAllowResponsesToModify);

		// Token: 0x0600038B RID: 907
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_ReleaseCookieContainer(HTTPCookieContainerHandle hCookieContainer);

		// Token: 0x0600038C RID: 908
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetCookie(HTTPCookieContainerHandle hCookieContainer, InteropHelp.UTF8StringHandle pchHost, InteropHelp.UTF8StringHandle pchUrl, InteropHelp.UTF8StringHandle pchCookie);

		// Token: 0x0600038D RID: 909
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestCookieContainer(HTTPRequestHandle hRequest, HTTPCookieContainerHandle hCookieContainer);

		// Token: 0x0600038E RID: 910
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestUserAgentInfo(HTTPRequestHandle hRequest, InteropHelp.UTF8StringHandle pchUserAgentInfo);

		// Token: 0x0600038F RID: 911
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestRequiresVerifiedCertificate(HTTPRequestHandle hRequest, [MarshalAs(UnmanagedType.I1)] bool bRequireVerifiedCertificate);

		// Token: 0x06000390 RID: 912
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_SetHTTPRequestAbsoluteTimeoutMS(HTTPRequestHandle hRequest, uint unMilliseconds);

		// Token: 0x06000391 RID: 913
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerHTTP_GetHTTPRequestWasTimedOut(HTTPRequestHandle hRequest, out bool pbWasTimedOut);

		// Token: 0x06000392 RID: 914
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EResult ISteamGameServerInventory_GetResultStatus(SteamInventoryResult_t resultHandle);

		// Token: 0x06000393 RID: 915
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetResultItems(SteamInventoryResult_t resultHandle, [In] [Out] SteamItemDetails_t[] pOutItemsArray, ref uint punOutItemsArraySize);

		// Token: 0x06000394 RID: 916
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerInventory_GetResultTimestamp(SteamInventoryResult_t resultHandle);

		// Token: 0x06000395 RID: 917
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected);

		// Token: 0x06000396 RID: 918
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerInventory_DestroyResult(SteamInventoryResult_t resultHandle);

		// Token: 0x06000397 RID: 919
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetAllItems(out SteamInventoryResult_t pResultHandle);

		// Token: 0x06000398 RID: 920
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemsByID(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemInstanceID_t[] pInstanceIDs, uint unCountInstanceIDs);

		// Token: 0x06000399 RID: 921
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_SerializeResult(SteamInventoryResult_t resultHandle, [In] [Out] byte[] pOutBuffer, out uint punOutBufferSize);

		// Token: 0x0600039A RID: 922
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_DeserializeResult(out SteamInventoryResult_t pOutResultHandle, [In] [Out] byte[] pBuffer, uint unBufferSize, [MarshalAs(UnmanagedType.I1)] bool bRESERVED_MUST_BE_FALSE);

		// Token: 0x0600039B RID: 923
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GenerateItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayItemDefs, [In] [Out] uint[] punArrayQuantity, uint unArrayLength);

		// Token: 0x0600039C RID: 924
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GrantPromoItems(out SteamInventoryResult_t pResultHandle);

		// Token: 0x0600039D RID: 925
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_AddPromoItem(out SteamInventoryResult_t pResultHandle, SteamItemDef_t itemDef);

		// Token: 0x0600039E RID: 926
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_AddPromoItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayItemDefs, uint unArrayLength);

		// Token: 0x0600039F RID: 927
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_ConsumeItem(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemConsume, uint unQuantity);

		// Token: 0x060003A0 RID: 928
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_ExchangeItems(out SteamInventoryResult_t pResultHandle, [In] [Out] SteamItemDef_t[] pArrayGenerate, [In] [Out] uint[] punArrayGenerateQuantity, uint unArrayGenerateLength, [In] [Out] SteamItemInstanceID_t[] pArrayDestroy, [In] [Out] uint[] punArrayDestroyQuantity, uint unArrayDestroyLength);

		// Token: 0x060003A1 RID: 929
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TransferItemQuantity(out SteamInventoryResult_t pResultHandle, SteamItemInstanceID_t itemIdSource, uint unQuantity, SteamItemInstanceID_t itemIdDest);

		// Token: 0x060003A2 RID: 930
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerInventory_SendItemDropHeartbeat();

		// Token: 0x060003A3 RID: 931
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TriggerItemDrop(out SteamInventoryResult_t pResultHandle, SteamItemDef_t dropListDefinition);

		// Token: 0x060003A4 RID: 932
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_TradeItems(out SteamInventoryResult_t pResultHandle, CSteamID steamIDTradePartner, [In] [Out] SteamItemInstanceID_t[] pArrayGive, [In] [Out] uint[] pArrayGiveQuantity, uint nArrayGiveLength, [In] [Out] SteamItemInstanceID_t[] pArrayGet, [In] [Out] uint[] pArrayGetQuantity, uint nArrayGetLength);

		// Token: 0x060003A5 RID: 933
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_LoadItemDefinitions();

		// Token: 0x060003A6 RID: 934
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemDefinitionIDs([In] [Out] SteamItemDef_t[] pItemDefIDs, out uint punItemDefIDsArraySize);

		// Token: 0x060003A7 RID: 935
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerInventory_GetItemDefinitionProperty(SteamItemDef_t iDefinition, InteropHelp.UTF8StringHandle pchPropertyName, IntPtr pchValueBuffer, ref uint punValueBufferSize);

		// Token: 0x060003A8 RID: 936
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_SendP2PPacket(CSteamID steamIDRemote, [In] [Out] byte[] pubData, uint cubData, EP2PSend eP2PSendType, int nChannel);

		// Token: 0x060003A9 RID: 937
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsP2PPacketAvailable(out uint pcubMsgSize, int nChannel);

		// Token: 0x060003AA RID: 938
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_ReadP2PPacket([In] [Out] byte[] pubDest, uint cubDest, out uint pcubMsgSize, out CSteamID psteamIDRemote, int nChannel);

		// Token: 0x060003AB RID: 939
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_AcceptP2PSessionWithUser(CSteamID steamIDRemote);

		// Token: 0x060003AC RID: 940
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_CloseP2PSessionWithUser(CSteamID steamIDRemote);

		// Token: 0x060003AD RID: 941
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_CloseP2PChannelWithUser(CSteamID steamIDRemote, int nChannel);

		// Token: 0x060003AE RID: 942
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetP2PSessionState(CSteamID steamIDRemote, out P2PSessionState_t pConnectionState);

		// Token: 0x060003AF RID: 943
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_AllowP2PPacketRelay([MarshalAs(UnmanagedType.I1)] bool bAllow);

		// Token: 0x060003B0 RID: 944
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateListenSocket(int nVirtualP2PPort, uint nIP, ushort nPort, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		// Token: 0x060003B1 RID: 945
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateP2PConnectionSocket(CSteamID steamIDTarget, int nVirtualPort, int nTimeoutSec, [MarshalAs(UnmanagedType.I1)] bool bAllowUseOfPacketRelay);

		// Token: 0x060003B2 RID: 946
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerNetworking_CreateConnectionSocket(uint nIP, ushort nPort, int nTimeoutSec);

		// Token: 0x060003B3 RID: 947
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_DestroySocket(SNetSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

		// Token: 0x060003B4 RID: 948
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_DestroyListenSocket(SNetListenSocket_t hSocket, [MarshalAs(UnmanagedType.I1)] bool bNotifyRemoteEnd);

		// Token: 0x060003B5 RID: 949
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_SendDataOnSocket(SNetSocket_t hSocket, IntPtr pubData, uint cubData, [MarshalAs(UnmanagedType.I1)] bool bReliable);

		// Token: 0x060003B6 RID: 950
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsDataAvailableOnSocket(SNetSocket_t hSocket, out uint pcubMsgSize);

		// Token: 0x060003B7 RID: 951
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_RetrieveDataFromSocket(SNetSocket_t hSocket, IntPtr pubDest, uint cubDest, out uint pcubMsgSize);

		// Token: 0x060003B8 RID: 952
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_IsDataAvailable(SNetListenSocket_t hListenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket);

		// Token: 0x060003B9 RID: 953
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_RetrieveData(SNetListenSocket_t hListenSocket, IntPtr pubDest, uint cubDest, out uint pcubMsgSize, out SNetSocket_t phSocket);

		// Token: 0x060003BA RID: 954
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetSocketInfo(SNetSocket_t hSocket, out CSteamID pSteamIDRemote, out int peSocketStatus, out uint punIPRemote, out ushort punPortRemote);

		// Token: 0x060003BB RID: 955
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerNetworking_GetListenSocketInfo(SNetListenSocket_t hListenSocket, out uint pnIP, out ushort pnPort);

		// Token: 0x060003BC RID: 956
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ESNetSocketConnectionType ISteamGameServerNetworking_GetSocketConnectionType(SNetSocket_t hSocket);

		// Token: 0x060003BD RID: 957
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ISteamGameServerNetworking_GetMaxPacketSize(SNetSocket_t hSocket);

		// Token: 0x060003BE RID: 958
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryUserUGCRequest(AccountID_t unAccountID, EUserUGCList eListType, EUGCMatchingUGCType eMatchingUGCType, EUserUGCListSortOrder eSortOrder, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		// Token: 0x060003BF RID: 959
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryAllUGCRequest(EUGCQuery eQueryType, EUGCMatchingUGCType eMatchingeMatchingUGCTypeFileType, AppId_t nCreatorAppID, AppId_t nConsumerAppID, uint unPage);

		// Token: 0x060003C0 RID: 960
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateQueryUGCDetailsRequest([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint unNumPublishedFileIDs);

		// Token: 0x060003C1 RID: 961
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SendQueryUGCRequest(UGCQueryHandle_t handle);

		// Token: 0x060003C2 RID: 962
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCResult(UGCQueryHandle_t handle, uint index, out SteamUGCDetails_t pDetails);

		// Token: 0x060003C3 RID: 963
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCPreviewURL(UGCQueryHandle_t handle, uint index, IntPtr pchURL, uint cchURLSize);

		// Token: 0x060003C4 RID: 964
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCMetadata(UGCQueryHandle_t handle, uint index, IntPtr pchMetadata, uint cchMetadatasize);

		// Token: 0x060003C5 RID: 965
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCChildren(UGCQueryHandle_t handle, uint index, [In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		// Token: 0x060003C6 RID: 966
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCStatistic(UGCQueryHandle_t handle, uint index, EItemStatistic eStatType, out uint pStatValue);

		// Token: 0x060003C7 RID: 967
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetQueryUGCNumAdditionalPreviews(UGCQueryHandle_t handle, uint index);

		// Token: 0x060003C8 RID: 968
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCAdditionalPreview(UGCQueryHandle_t handle, uint index, uint previewIndex, IntPtr pchURLOrVideoID, uint cchURLSize, out bool pbIsImage);

		// Token: 0x060003C9 RID: 969
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetQueryUGCNumKeyValueTags(UGCQueryHandle_t handle, uint index);

		// Token: 0x060003CA RID: 970
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetQueryUGCKeyValueTag(UGCQueryHandle_t handle, uint index, uint keyValueTagIndex, IntPtr pchKey, uint cchKeySize, IntPtr pchValue, uint cchValueSize);

		// Token: 0x060003CB RID: 971
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_ReleaseQueryUGCRequest(UGCQueryHandle_t handle);

		// Token: 0x060003CC RID: 972
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddRequiredTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

		// Token: 0x060003CD RID: 973
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddExcludedTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pTagName);

		// Token: 0x060003CE RID: 974
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnKeyValueTags(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnKeyValueTags);

		// Token: 0x060003CF RID: 975
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnLongDescription(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnLongDescription);

		// Token: 0x060003D0 RID: 976
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnMetadata(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnMetadata);

		// Token: 0x060003D1 RID: 977
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnChildren(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnChildren);

		// Token: 0x060003D2 RID: 978
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnAdditionalPreviews(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnAdditionalPreviews);

		// Token: 0x060003D3 RID: 979
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetReturnTotalOnly(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bReturnTotalOnly);

		// Token: 0x060003D4 RID: 980
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetLanguage(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

		// Token: 0x060003D5 RID: 981
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetAllowCachedResponse(UGCQueryHandle_t handle, uint unMaxAgeSeconds);

		// Token: 0x060003D6 RID: 982
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetCloudFileNameFilter(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pMatchCloudFileName);

		// Token: 0x060003D7 RID: 983
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetMatchAnyTag(UGCQueryHandle_t handle, [MarshalAs(UnmanagedType.I1)] bool bMatchAnyTag);

		// Token: 0x060003D8 RID: 984
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetSearchText(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pSearchText);

		// Token: 0x060003D9 RID: 985
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetRankedByTrendDays(UGCQueryHandle_t handle, uint unDays);

		// Token: 0x060003DA RID: 986
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddRequiredKeyValueTag(UGCQueryHandle_t handle, InteropHelp.UTF8StringHandle pKey, InteropHelp.UTF8StringHandle pValue);

		// Token: 0x060003DB RID: 987
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_RequestUGCDetails(PublishedFileId_t nPublishedFileID, uint unMaxAgeSeconds);

		// Token: 0x060003DC RID: 988
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_CreateItem(AppId_t nConsumerAppId, EWorkshopFileType eFileType);

		// Token: 0x060003DD RID: 989
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_StartItemUpdate(AppId_t nConsumerAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x060003DE RID: 990
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemTitle(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchTitle);

		// Token: 0x060003DF RID: 991
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemDescription(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchDescription);

		// Token: 0x060003E0 RID: 992
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemUpdateLanguage(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchLanguage);

		// Token: 0x060003E1 RID: 993
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemMetadata(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchMetaData);

		// Token: 0x060003E2 RID: 994
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemVisibility(UGCUpdateHandle_t handle, ERemoteStoragePublishedFileVisibility eVisibility);

		// Token: 0x060003E3 RID: 995
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemTags(UGCUpdateHandle_t updateHandle, IntPtr pTags);

		// Token: 0x060003E4 RID: 996
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemContent(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszContentFolder);

		// Token: 0x060003E5 RID: 997
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_SetItemPreview(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pszPreviewFile);

		// Token: 0x060003E6 RID: 998
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_RemoveItemKeyValueTags(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey);

		// Token: 0x060003E7 RID: 999
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_AddItemKeyValueTag(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchKey, InteropHelp.UTF8StringHandle pchValue);

		// Token: 0x060003E8 RID: 1000
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SubmitItemUpdate(UGCUpdateHandle_t handle, InteropHelp.UTF8StringHandle pchChangeNote);

		// Token: 0x060003E9 RID: 1001
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EItemUpdateStatus ISteamGameServerUGC_GetItemUpdateProgress(UGCUpdateHandle_t handle, out ulong punBytesProcessed, out ulong punBytesTotal);

		// Token: 0x060003EA RID: 1002
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SetUserItemVote(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bVoteUp);

		// Token: 0x060003EB RID: 1003
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_GetUserItemVote(PublishedFileId_t nPublishedFileID);

		// Token: 0x060003EC RID: 1004
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_AddItemToFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x060003ED RID: 1005
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_RemoveItemFromFavorites(AppId_t nAppId, PublishedFileId_t nPublishedFileID);

		// Token: 0x060003EE RID: 1006
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_SubscribeItem(PublishedFileId_t nPublishedFileID);

		// Token: 0x060003EF RID: 1007
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUGC_UnsubscribeItem(PublishedFileId_t nPublishedFileID);

		// Token: 0x060003F0 RID: 1008
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetNumSubscribedItems();

		// Token: 0x060003F1 RID: 1009
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetSubscribedItems([In] [Out] PublishedFileId_t[] pvecPublishedFileID, uint cMaxEntries);

		// Token: 0x060003F2 RID: 1010
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUGC_GetItemState(PublishedFileId_t nPublishedFileID);

		// Token: 0x060003F3 RID: 1011
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetItemInstallInfo(PublishedFileId_t nPublishedFileID, out ulong punSizeOnDisk, IntPtr pchFolder, uint cchFolderSize, out uint punTimeStamp);

		// Token: 0x060003F4 RID: 1012
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_GetItemDownloadInfo(PublishedFileId_t nPublishedFileID, out ulong punBytesDownloaded, out ulong punBytesTotal);

		// Token: 0x060003F5 RID: 1013
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUGC_DownloadItem(PublishedFileId_t nPublishedFileID, [MarshalAs(UnmanagedType.I1)] bool bHighPriority);

		// Token: 0x060003F6 RID: 1014
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetSecondsSinceAppActive();

		// Token: 0x060003F7 RID: 1015
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetSecondsSinceComputerActive();

		// Token: 0x060003F8 RID: 1016
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern EUniverse ISteamGameServerUtils_GetConnectedUniverse();

		// Token: 0x060003F9 RID: 1017
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetServerRealTime();

		// Token: 0x060003FA RID: 1018
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamGameServerUtils_GetIPCountry();

		// Token: 0x060003FB RID: 1019
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetImageSize(int iImage, out uint pnWidth, out uint pnHeight);

		// Token: 0x060003FC RID: 1020
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetImageRGBA(int iImage, [In] [Out] byte[] pubDest, int nDestBufferSize);

		// Token: 0x060003FD RID: 1021
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetCSERIPPort(out uint unIP, out ushort usPort);

		// Token: 0x060003FE RID: 1022
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte ISteamGameServerUtils_GetCurrentBatteryPower();

		// Token: 0x060003FF RID: 1023
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetAppID();

		// Token: 0x06000400 RID: 1024
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetOverlayNotificationPosition(ENotificationPosition eNotificationPosition);

		// Token: 0x06000401 RID: 1025
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsAPICallCompleted(SteamAPICall_t hSteamAPICall, out bool pbFailed);

		// Token: 0x06000402 RID: 1026
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ESteamAPICallFailure ISteamGameServerUtils_GetAPICallFailureReason(SteamAPICall_t hSteamAPICall);

		// Token: 0x06000403 RID: 1027
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetAPICallResult(SteamAPICall_t hSteamAPICall, IntPtr pCallback, int cubCallback, int iCallbackExpected, out bool pbFailed);

		// Token: 0x06000404 RID: 1028
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_RunFrame();

		// Token: 0x06000405 RID: 1029
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetIPCCallCount();

		// Token: 0x06000406 RID: 1030
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetWarningMessageHook(SteamAPIWarningMessageHook_t pFunction);

		// Token: 0x06000407 RID: 1031
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsOverlayEnabled();

		// Token: 0x06000408 RID: 1032
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_BOverlayNeedsPresent();

		// Token: 0x06000409 RID: 1033
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern ulong ISteamGameServerUtils_CheckFileSignature(InteropHelp.UTF8StringHandle szFileName);

		// Token: 0x0600040A RID: 1034
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_ShowGamepadTextInput(EGamepadTextInputMode eInputMode, EGamepadTextInputLineMode eLineInputMode, InteropHelp.UTF8StringHandle pchDescription, uint unCharMax, InteropHelp.UTF8StringHandle pchExistingText);

		// Token: 0x0600040B RID: 1035
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern uint ISteamGameServerUtils_GetEnteredGamepadTextLength();

		// Token: 0x0600040C RID: 1036
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_GetEnteredGamepadTextInput(IntPtr pchText, uint cchText);

		// Token: 0x0600040D RID: 1037
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr ISteamGameServerUtils_GetSteamUILanguage();

		// Token: 0x0600040E RID: 1038
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool ISteamGameServerUtils_IsSteamRunningInVR();

		// Token: 0x0600040F RID: 1039
		[DllImport("CSteamworks", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ISteamGameServerUtils_SetOverlayNotificationInset(int nHorizontalInset, int nVerticalInset);
	}
}
