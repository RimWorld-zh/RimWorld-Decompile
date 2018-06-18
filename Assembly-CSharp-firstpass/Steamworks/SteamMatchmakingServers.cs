using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000141 RID: 321
	public static class SteamMatchmakingServers
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x00008798 File Offset: 0x00006998
		public static HServerListRequest RequestInternetServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestInternetServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000087D0 File Offset: 0x000069D0
		public static HServerListRequest RequestLANServerList(AppId_t iApp, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestLANServerList(iApp, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000087FC File Offset: 0x000069FC
		public static HServerListRequest RequestFriendsServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestFriendsServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00008834 File Offset: 0x00006A34
		public static HServerListRequest RequestFavoritesServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestFavoritesServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0000886C File Offset: 0x00006A6C
		public static HServerListRequest RequestHistoryServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestHistoryServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000088A4 File Offset: 0x00006AA4
		public static HServerListRequest RequestSpectatorServerList(AppId_t iApp, MatchMakingKeyValuePair_t[] ppchFilters, uint nFilters, ISteamMatchmakingServerListResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerListRequest)NativeMethods.ISteamMatchmakingServers_RequestSpectatorServerList(iApp, new MMKVPMarshaller(ppchFilters), nFilters, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000088DB File Offset: 0x00006ADB
		public static void ReleaseRequest(HServerListRequest hServerListRequest)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_ReleaseRequest(hServerListRequest);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x000088EC File Offset: 0x00006AEC
		public static gameserveritem_t GetServerDetails(HServerListRequest hRequest, int iServer)
		{
			InteropHelp.TestIfAvailableClient();
			return (gameserveritem_t)Marshal.PtrToStructure(NativeMethods.ISteamMatchmakingServers_GetServerDetails(hRequest, iServer), typeof(gameserveritem_t));
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00008921 File Offset: 0x00006B21
		public static void CancelQuery(HServerListRequest hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_CancelQuery(hRequest);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0000892F File Offset: 0x00006B2F
		public static void RefreshQuery(HServerListRequest hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_RefreshQuery(hRequest);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00008940 File Offset: 0x00006B40
		public static bool IsRefreshing(HServerListRequest hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmakingServers_IsRefreshing(hRequest);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00008960 File Offset: 0x00006B60
		public static int GetServerCount(HServerListRequest hRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMatchmakingServers_GetServerCount(hRequest);
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00008980 File Offset: 0x00006B80
		public static void RefreshServer(HServerListRequest hRequest, int iServer)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_RefreshServer(hRequest, iServer);
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00008990 File Offset: 0x00006B90
		public static HServerQuery PingServer(uint unIP, ushort usPort, ISteamMatchmakingPingResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerQuery)NativeMethods.ISteamMatchmakingServers_PingServer(unIP, usPort, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x000089BC File Offset: 0x00006BBC
		public static HServerQuery PlayerDetails(uint unIP, ushort usPort, ISteamMatchmakingPlayersResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerQuery)NativeMethods.ISteamMatchmakingServers_PlayerDetails(unIP, usPort, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000089E8 File Offset: 0x00006BE8
		public static HServerQuery ServerRules(uint unIP, ushort usPort, ISteamMatchmakingRulesResponse pRequestServersResponse)
		{
			InteropHelp.TestIfAvailableClient();
			return (HServerQuery)NativeMethods.ISteamMatchmakingServers_ServerRules(unIP, usPort, (IntPtr)pRequestServersResponse);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00008A14 File Offset: 0x00006C14
		public static void CancelServerQuery(HServerQuery hServerQuery)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamMatchmakingServers_CancelServerQuery(hServerQuery);
		}
	}
}
