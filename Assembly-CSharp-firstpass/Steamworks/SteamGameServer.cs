using System;

namespace Steamworks
{
	// Token: 0x02000136 RID: 310
	public static class SteamGameServer
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00005058 File Offset: 0x00003258
		public static bool InitGameServer(uint unIP, ushort usGamePort, ushort usQueryPort, uint unFlags, AppId_t nGameAppId, string pchVersionString)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchVersionString))
			{
				result = NativeMethods.ISteamGameServer_InitGameServer(unIP, usGamePort, usQueryPort, unFlags, nGameAppId, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000050A4 File Offset: 0x000032A4
		public static void SetProduct(string pszProduct)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszProduct))
			{
				NativeMethods.ISteamGameServer_SetProduct(utf8StringHandle);
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000050E8 File Offset: 0x000032E8
		public static void SetGameDescription(string pszGameDescription)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszGameDescription))
			{
				NativeMethods.ISteamGameServer_SetGameDescription(utf8StringHandle);
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000512C File Offset: 0x0000332C
		public static void SetModDir(string pszModDir)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszModDir))
			{
				NativeMethods.ISteamGameServer_SetModDir(utf8StringHandle);
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00005170 File Offset: 0x00003370
		public static void SetDedicatedServer(bool bDedicated)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetDedicatedServer(bDedicated);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00005180 File Offset: 0x00003380
		public static void LogOn(string pszToken)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszToken))
			{
				NativeMethods.ISteamGameServer_LogOn(utf8StringHandle);
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000051C4 File Offset: 0x000033C4
		public static void LogOnAnonymous()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_LogOnAnonymous();
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000051D1 File Offset: 0x000033D1
		public static void LogOff()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_LogOff();
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000051E0 File Offset: 0x000033E0
		public static bool BLoggedOn()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BLoggedOn();
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00005200 File Offset: 0x00003400
		public static bool BSecure()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BSecure();
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00005220 File Offset: 0x00003420
		public static CSteamID GetSteamID()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (CSteamID)NativeMethods.ISteamGameServer_GetSteamID();
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00005244 File Offset: 0x00003444
		public static bool WasRestartRequested()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_WasRestartRequested();
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00005263 File Offset: 0x00003463
		public static void SetMaxPlayerCount(int cPlayersMax)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetMaxPlayerCount(cPlayersMax);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00005271 File Offset: 0x00003471
		public static void SetBotPlayerCount(int cBotplayers)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetBotPlayerCount(cBotplayers);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00005280 File Offset: 0x00003480
		public static void SetServerName(string pszServerName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszServerName))
			{
				NativeMethods.ISteamGameServer_SetServerName(utf8StringHandle);
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000052C4 File Offset: 0x000034C4
		public static void SetMapName(string pszMapName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszMapName))
			{
				NativeMethods.ISteamGameServer_SetMapName(utf8StringHandle);
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00005308 File Offset: 0x00003508
		public static void SetPasswordProtected(bool bPasswordProtected)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetPasswordProtected(bPasswordProtected);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00005316 File Offset: 0x00003516
		public static void SetSpectatorPort(ushort unSpectatorPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetSpectatorPort(unSpectatorPort);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00005324 File Offset: 0x00003524
		public static void SetSpectatorServerName(string pszSpectatorServerName)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszSpectatorServerName))
			{
				NativeMethods.ISteamGameServer_SetSpectatorServerName(utf8StringHandle);
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00005368 File Offset: 0x00003568
		public static void ClearAllKeyValues()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_ClearAllKeyValues();
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00005378 File Offset: 0x00003578
		public static void SetKeyValue(string pKey, string pValue)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pValue))
				{
					NativeMethods.ISteamGameServer_SetKeyValue(utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000053E4 File Offset: 0x000035E4
		public static void SetGameTags(string pchGameTags)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchGameTags))
			{
				NativeMethods.ISteamGameServer_SetGameTags(utf8StringHandle);
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00005428 File Offset: 0x00003628
		public static void SetGameData(string pchGameData)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchGameData))
			{
				NativeMethods.ISteamGameServer_SetGameData(utf8StringHandle);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000546C File Offset: 0x0000366C
		public static void SetRegion(string pszRegion)
		{
			InteropHelp.TestIfAvailableGameServer();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pszRegion))
			{
				NativeMethods.ISteamGameServer_SetRegion(utf8StringHandle);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x000054B0 File Offset: 0x000036B0
		public static bool SendUserConnectAndAuthenticate(uint unIPClient, byte[] pvAuthBlob, uint cubAuthBlobSize, out CSteamID pSteamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_SendUserConnectAndAuthenticate(unIPClient, pvAuthBlob, cubAuthBlobSize, out pSteamIDUser);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000054D4 File Offset: 0x000036D4
		public static CSteamID CreateUnauthenticatedUserConnection()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (CSteamID)NativeMethods.ISteamGameServer_CreateUnauthenticatedUserConnection();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000054F8 File Offset: 0x000036F8
		public static void SendUserDisconnect(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SendUserDisconnect(steamIDUser);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00005508 File Offset: 0x00003708
		public static bool BUpdateUserData(CSteamID steamIDUser, string pchPlayerName, uint uScore)
		{
			InteropHelp.TestIfAvailableGameServer();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPlayerName))
			{
				result = NativeMethods.ISteamGameServer_BUpdateUserData(steamIDUser, utf8StringHandle, uScore);
			}
			return result;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00005550 File Offset: 0x00003750
		public static HAuthTicket GetAuthSessionTicket(byte[] pTicket, int cbMaxTicket, out uint pcbTicket)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (HAuthTicket)NativeMethods.ISteamGameServer_GetAuthSessionTicket(pTicket, cbMaxTicket, out pcbTicket);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00005578 File Offset: 0x00003778
		public static EBeginAuthSessionResult BeginAuthSession(byte[] pAuthTicket, int cbAuthTicket, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_BeginAuthSession(pAuthTicket, cbAuthTicket, steamID);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0000559A File Offset: 0x0000379A
		public static void EndAuthSession(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_EndAuthSession(steamID);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000055A8 File Offset: 0x000037A8
		public static void CancelAuthTicket(HAuthTicket hAuthTicket)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_CancelAuthTicket(hAuthTicket);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000055B8 File Offset: 0x000037B8
		public static EUserHasLicenseForAppResult UserHasLicenseForApp(CSteamID steamID, AppId_t appID)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_UserHasLicenseForApp(steamID, appID);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x000055DC File Offset: 0x000037DC
		public static bool RequestUserGroupStatus(CSteamID steamIDUser, CSteamID steamIDGroup)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_RequestUserGroupStatus(steamIDUser, steamIDGroup);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000055FD File Offset: 0x000037FD
		public static void GetGameplayStats()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_GetGameplayStats();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000560C File Offset: 0x0000380C
		public static SteamAPICall_t GetServerReputation()
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_GetServerReputation();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00005630 File Offset: 0x00003830
		public static uint GetPublicIP()
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_GetPublicIP();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00005650 File Offset: 0x00003850
		public static bool HandleIncomingPacket(byte[] pData, int cbData, uint srcIP, ushort srcPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_HandleIncomingPacket(pData, cbData, srcIP, srcPort);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00005674 File Offset: 0x00003874
		public static int GetNextOutgoingPacket(byte[] pOut, int cbMaxOut, out uint pNetAdr, out ushort pPort)
		{
			InteropHelp.TestIfAvailableGameServer();
			return NativeMethods.ISteamGameServer_GetNextOutgoingPacket(pOut, cbMaxOut, out pNetAdr, out pPort);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00005697 File Offset: 0x00003897
		public static void EnableHeartbeats(bool bActive)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_EnableHeartbeats(bActive);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000056A5 File Offset: 0x000038A5
		public static void SetHeartbeatInterval(int iHeartbeatInterval)
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_SetHeartbeatInterval(iHeartbeatInterval);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000056B3 File Offset: 0x000038B3
		public static void ForceHeartbeat()
		{
			InteropHelp.TestIfAvailableGameServer();
			NativeMethods.ISteamGameServer_ForceHeartbeat();
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000056C0 File Offset: 0x000038C0
		public static SteamAPICall_t AssociateWithClan(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_AssociateWithClan(steamIDClan);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000056E8 File Offset: 0x000038E8
		public static SteamAPICall_t ComputeNewPlayerCompatibility(CSteamID steamIDNewPlayer)
		{
			InteropHelp.TestIfAvailableGameServer();
			return (SteamAPICall_t)NativeMethods.ISteamGameServer_ComputeNewPlayerCompatibility(steamIDNewPlayer);
		}
	}
}
