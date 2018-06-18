using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000140 RID: 320
	public static class SteamMatchmaking
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x000080D4 File Offset: 0x000062D4
		public static int GetFavoriteGameCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetFavoriteGameCount();
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000080F4 File Offset: 0x000062F4
		public static bool GetFavoriteGame(int iGame, out AppId_t pnAppID, out uint pnIP, out ushort pnConnPort, out ushort pnQueryPort, out uint punFlags, out uint pRTime32LastPlayedOnServer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetFavoriteGame(iGame, out pnAppID, out pnIP, out pnConnPort, out pnQueryPort, out punFlags, out pRTime32LastPlayedOnServer);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00008120 File Offset: 0x00006320
		public static int AddFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags, uint rTime32LastPlayedOnServer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_AddFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags, rTime32LastPlayedOnServer);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00008148 File Offset: 0x00006348
		public static bool RemoveFavoriteGame(AppId_t nAppID, uint nIP, ushort nConnPort, ushort nQueryPort, uint unFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_RemoveFavoriteGame(nAppID, nIP, nConnPort, nQueryPort, unFlags);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00008170 File Offset: 0x00006370
		public static SteamAPICall_t RequestLobbyList()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamMatchmaking_RequestLobbyList();
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00008194 File Offset: 0x00006394
		public static void AddRequestLobbyListStringFilter(string pchKeyToMatch, string pchValueToMatch, ELobbyComparison eComparisonType)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValueToMatch))
				{
					NativeMethods.ISteamMatchmaking_AddRequestLobbyListStringFilter(utf8StringHandle, utf8StringHandle2, eComparisonType);
				}
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00008200 File Offset: 0x00006400
		public static void AddRequestLobbyListNumericalFilter(string pchKeyToMatch, int nValueToMatch, ELobbyComparison eComparisonType)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				NativeMethods.ISteamMatchmaking_AddRequestLobbyListNumericalFilter(utf8StringHandle, nValueToMatch, eComparisonType);
			}
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00008248 File Offset: 0x00006448
		public static void AddRequestLobbyListNearValueFilter(string pchKeyToMatch, int nValueToBeCloseTo)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKeyToMatch))
			{
				NativeMethods.ISteamMatchmaking_AddRequestLobbyListNearValueFilter(utf8StringHandle, nValueToBeCloseTo);
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00008290 File Offset: 0x00006490
		public static void AddRequestLobbyListFilterSlotsAvailable(int nSlotsAvailable)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_AddRequestLobbyListFilterSlotsAvailable(nSlotsAvailable);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x0000829E File Offset: 0x0000649E
		public static void AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter eLobbyDistanceFilter)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_AddRequestLobbyListDistanceFilter(eLobbyDistanceFilter);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000082AC File Offset: 0x000064AC
		public static void AddRequestLobbyListResultCountFilter(int cMaxResults)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_AddRequestLobbyListResultCountFilter(cMaxResults);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x000082BA File Offset: 0x000064BA
		public static void AddRequestLobbyListCompatibleMembersFilter(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_AddRequestLobbyListCompatibleMembersFilter(steamIDLobby);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000082C8 File Offset: 0x000064C8
		public static CSteamID GetLobbyByIndex(int iLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamMatchmaking_GetLobbyByIndex(iLobby);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x000082F0 File Offset: 0x000064F0
		public static SteamAPICall_t CreateLobby(ELobbyType eLobbyType, int cMaxMembers)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamMatchmaking_CreateLobby(eLobbyType, cMaxMembers);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00008318 File Offset: 0x00006518
		public static SteamAPICall_t JoinLobby(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamMatchmaking_JoinLobby(steamIDLobby);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0000833D File Offset: 0x0000653D
		public static void LeaveLobby(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_LeaveLobby(steamIDLobby);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0000834C File Offset: 0x0000654C
		public static bool InviteUserToLobby(CSteamID steamIDLobby, CSteamID steamIDInvitee)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_InviteUserToLobby(steamIDLobby, steamIDInvitee);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00008370 File Offset: 0x00006570
		public static int GetNumLobbyMembers(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetNumLobbyMembers(steamIDLobby);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00008390 File Offset: 0x00006590
		public static CSteamID GetLobbyMemberByIndex(CSteamID steamIDLobby, int iMember)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamMatchmaking_GetLobbyMemberByIndex(steamIDLobby, iMember);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000083B8 File Offset: 0x000065B8
		public static string GetLobbyData(CSteamID steamIDLobby, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamMatchmaking_GetLobbyData(steamIDLobby, utf8StringHandle));
			}
			return result;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00008404 File Offset: 0x00006604
		public static bool SetLobbyData(CSteamID steamIDLobby, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					result = NativeMethods.ISteamMatchmaking_SetLobbyData(steamIDLobby, utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0000846C File Offset: 0x0000666C
		public static int GetLobbyDataCount(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetLobbyDataCount(steamIDLobby);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0000848C File Offset: 0x0000668C
		public static bool GetLobbyDataByIndex(CSteamID steamIDLobby, int iLobbyData, out string pchKey, int cchKeyBufferSize, out string pchValue, int cchValueBufferSize)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchKeyBufferSize);
			IntPtr intPtr2 = Marshal.AllocHGlobal(cchValueBufferSize);
			bool flag = NativeMethods.ISteamMatchmaking_GetLobbyDataByIndex(steamIDLobby, iLobbyData, intPtr, cchKeyBufferSize, intPtr2, cchValueBufferSize);
			pchKey = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			pchValue = ((!flag) ? null : InteropHelp.PtrToStringUTF8(intPtr2));
			Marshal.FreeHGlobal(intPtr2);
			return flag;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000084F8 File Offset: 0x000066F8
		public static bool DeleteLobbyData(CSteamID steamIDLobby, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = NativeMethods.ISteamMatchmaking_DeleteLobbyData(steamIDLobby, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00008540 File Offset: 0x00006740
		public static string GetLobbyMemberData(CSteamID steamIDLobby, CSteamID steamIDUser, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamMatchmaking_GetLobbyMemberData(steamIDLobby, steamIDUser, utf8StringHandle));
			}
			return result;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x0000858C File Offset: 0x0000678C
		public static void SetLobbyMemberData(CSteamID steamIDLobby, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					NativeMethods.ISteamMatchmaking_SetLobbyMemberData(steamIDLobby, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000085F8 File Offset: 0x000067F8
		public static bool SendLobbyChatMsg(CSteamID steamIDLobby, byte[] pvMsgBody, int cubMsgBody)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SendLobbyChatMsg(steamIDLobby, pvMsgBody, cubMsgBody);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0000861C File Offset: 0x0000681C
		public static int GetLobbyChatEntry(CSteamID steamIDLobby, int iChatID, out CSteamID pSteamIDUser, byte[] pvData, int cubData, out EChatEntryType peChatEntryType)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetLobbyChatEntry(steamIDLobby, iChatID, out pSteamIDUser, pvData, cubData, out peChatEntryType);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00008644 File Offset: 0x00006844
		public static bool RequestLobbyData(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_RequestLobbyData(steamIDLobby);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00008664 File Offset: 0x00006864
		public static void SetLobbyGameServer(CSteamID steamIDLobby, uint unGameServerIP, ushort unGameServerPort, CSteamID steamIDGameServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmaking_SetLobbyGameServer(steamIDLobby, unGameServerIP, unGameServerPort, steamIDGameServer);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00008678 File Offset: 0x00006878
		public static bool GetLobbyGameServer(CSteamID steamIDLobby, out uint punGameServerIP, out ushort punGameServerPort, out CSteamID psteamIDGameServer)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetLobbyGameServer(steamIDLobby, out punGameServerIP, out punGameServerPort, out psteamIDGameServer);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0000869C File Offset: 0x0000689C
		public static bool SetLobbyMemberLimit(CSteamID steamIDLobby, int cMaxMembers)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLobbyMemberLimit(steamIDLobby, cMaxMembers);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000086C0 File Offset: 0x000068C0
		public static int GetLobbyMemberLimit(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_GetLobbyMemberLimit(steamIDLobby);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000086E0 File Offset: 0x000068E0
		public static bool SetLobbyType(CSteamID steamIDLobby, ELobbyType eLobbyType)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLobbyType(steamIDLobby, eLobbyType);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00008704 File Offset: 0x00006904
		public static bool SetLobbyJoinable(CSteamID steamIDLobby, bool bLobbyJoinable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLobbyJoinable(steamIDLobby, bLobbyJoinable);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00008728 File Offset: 0x00006928
		public static CSteamID GetLobbyOwner(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamMatchmaking_GetLobbyOwner(steamIDLobby);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00008750 File Offset: 0x00006950
		public static bool SetLobbyOwner(CSteamID steamIDLobby, CSteamID steamIDNewOwner)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLobbyOwner(steamIDLobby, steamIDNewOwner);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00008774 File Offset: 0x00006974
		public static bool SetLinkedLobby(CSteamID steamIDLobby, CSteamID steamIDLobbyDependent)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmaking_SetLinkedLobby(steamIDLobby, steamIDLobbyDependent);
		}
	}
}
