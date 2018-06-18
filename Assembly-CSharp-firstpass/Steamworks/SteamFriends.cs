using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000135 RID: 309
	public static class SteamFriends
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x0000456C File Offset: 0x0000276C
		public static string GetPersonaName()
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPersonaName());
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00004590 File Offset: 0x00002790
		public static SteamAPICall_t SetPersonaName(string pchPersonaName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPersonaName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamFriends_SetPersonaName(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000045DC File Offset: 0x000027DC
		public static EPersonaState GetPersonaState()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetPersonaState();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000045FC File Offset: 0x000027FC
		public static int GetFriendCount(EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCount(iFriendFlags);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000461C File Offset: 0x0000281C
		public static CSteamID GetFriendByIndex(int iFriend, EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetFriendByIndex(iFriend, iFriendFlags);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00004644 File Offset: 0x00002844
		public static EFriendRelationship GetFriendRelationship(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendRelationship(steamIDFriend);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00004664 File Offset: 0x00002864
		public static EPersonaState GetFriendPersonaState(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendPersonaState(steamIDFriend);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00004684 File Offset: 0x00002884
		public static string GetFriendPersonaName(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaName(steamIDFriend));
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000046AC File Offset: 0x000028AC
		public static bool GetFriendGamePlayed(CSteamID steamIDFriend, out FriendGameInfo_t pFriendGameInfo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendGamePlayed(steamIDFriend, out pFriendGameInfo);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000046D0 File Offset: 0x000028D0
		public static string GetFriendPersonaNameHistory(CSteamID steamIDFriend, int iPersonaName)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendPersonaNameHistory(steamIDFriend, iPersonaName));
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x000046F8 File Offset: 0x000028F8
		public static int GetFriendSteamLevel(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendSteamLevel(steamIDFriend);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00004718 File Offset: 0x00002918
		public static string GetPlayerNickname(CSteamID steamIDPlayer)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetPlayerNickname(steamIDPlayer));
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00004740 File Offset: 0x00002940
		public static int GetFriendsGroupCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendsGroupCount();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00004760 File Offset: 0x00002960
		public static FriendsGroupID_t GetFriendsGroupIDByIndex(int iFG)
		{
			InteropHelp.TestIfAvailableClient();
			return (FriendsGroupID_t)NativeMethods.ISteamFriends_GetFriendsGroupIDByIndex(iFG);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00004788 File Offset: 0x00002988
		public static string GetFriendsGroupName(FriendsGroupID_t friendsGroupID)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendsGroupName(friendsGroupID));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000047B0 File Offset: 0x000029B0
		public static int GetFriendsGroupMembersCount(FriendsGroupID_t friendsGroupID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendsGroupMembersCount(friendsGroupID);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000047D0 File Offset: 0x000029D0
		public static void GetFriendsGroupMembersList(FriendsGroupID_t friendsGroupID, CSteamID[] pOutSteamIDMembers, int nMembersCount)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_GetFriendsGroupMembersList(friendsGroupID, pOutSteamIDMembers, nMembersCount);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000047E0 File Offset: 0x000029E0
		public static bool HasFriend(CSteamID steamIDFriend, EFriendFlags iFriendFlags)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_HasFriend(steamIDFriend, iFriendFlags);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00004804 File Offset: 0x00002A04
		public static int GetClanCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanCount();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00004824 File Offset: 0x00002A24
		public static CSteamID GetClanByIndex(int iClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanByIndex(iClan);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000484C File Offset: 0x00002A4C
		public static string GetClanName(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanName(steamIDClan));
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00004874 File Offset: 0x00002A74
		public static string GetClanTag(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetClanTag(steamIDClan));
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000489C File Offset: 0x00002A9C
		public static bool GetClanActivityCounts(CSteamID steamIDClan, out int pnOnline, out int pnInGame, out int pnChatting)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanActivityCounts(steamIDClan, out pnOnline, out pnInGame, out pnChatting);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000048C0 File Offset: 0x00002AC0
		public static SteamAPICall_t DownloadClanActivityCounts(CSteamID[] psteamIDClans, int cClansToRequest)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_DownloadClanActivityCounts(psteamIDClans, cClansToRequest);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000048E8 File Offset: 0x00002AE8
		public static int GetFriendCountFromSource(CSteamID steamIDSource)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCountFromSource(steamIDSource);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00004908 File Offset: 0x00002B08
		public static CSteamID GetFriendFromSourceByIndex(CSteamID steamIDSource, int iFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetFriendFromSourceByIndex(steamIDSource, iFriend);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00004930 File Offset: 0x00002B30
		public static bool IsUserInSource(CSteamID steamIDUser, CSteamID steamIDSource)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsUserInSource(steamIDUser, steamIDSource);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00004951 File Offset: 0x00002B51
		public static void SetInGameVoiceSpeaking(CSteamID steamIDUser, bool bSpeaking)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_SetInGameVoiceSpeaking(steamIDUser, bSpeaking);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00004960 File Offset: 0x00002B60
		public static void ActivateGameOverlay(string pchDialog)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDialog))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlay(utf8StringHandle);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000049A4 File Offset: 0x00002BA4
		public static void ActivateGameOverlayToUser(string pchDialog, CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDialog))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlayToUser(utf8StringHandle, steamID);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000049EC File Offset: 0x00002BEC
		public static void ActivateGameOverlayToWebPage(string pchURL)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchURL))
			{
				NativeMethods.ISteamFriends_ActivateGameOverlayToWebPage(utf8StringHandle);
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00004A30 File Offset: 0x00002C30
		public static void ActivateGameOverlayToStore(AppId_t nAppID, EOverlayToStoreFlag eFlag)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ActivateGameOverlayToStore(nAppID, eFlag);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00004A3F File Offset: 0x00002C3F
		public static void SetPlayedWith(CSteamID steamIDUserPlayedWith)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_SetPlayedWith(steamIDUserPlayedWith);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00004A4D File Offset: 0x00002C4D
		public static void ActivateGameOverlayInviteDialog(CSteamID steamIDLobby)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ActivateGameOverlayInviteDialog(steamIDLobby);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00004A5C File Offset: 0x00002C5C
		public static int GetSmallFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetSmallFriendAvatar(steamIDFriend);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00004A7C File Offset: 0x00002C7C
		public static int GetMediumFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetMediumFriendAvatar(steamIDFriend);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00004A9C File Offset: 0x00002C9C
		public static int GetLargeFriendAvatar(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetLargeFriendAvatar(steamIDFriend);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00004ABC File Offset: 0x00002CBC
		public static bool RequestUserInformation(CSteamID steamIDUser, bool bRequireNameOnly)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_RequestUserInformation(steamIDUser, bRequireNameOnly);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00004AE0 File Offset: 0x00002CE0
		public static SteamAPICall_t RequestClanOfficerList(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_RequestClanOfficerList(steamIDClan);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00004B08 File Offset: 0x00002D08
		public static CSteamID GetClanOwner(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanOwner(steamIDClan);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00004B30 File Offset: 0x00002D30
		public static int GetClanOfficerCount(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanOfficerCount(steamIDClan);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00004B50 File Offset: 0x00002D50
		public static CSteamID GetClanOfficerByIndex(CSteamID steamIDClan, int iOfficer)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetClanOfficerByIndex(steamIDClan, iOfficer);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00004B78 File Offset: 0x00002D78
		public static uint GetUserRestrictions()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetUserRestrictions();
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00004B98 File Offset: 0x00002D98
		public static bool SetRichPresence(string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					result = NativeMethods.ISteamFriends_SetRichPresence(utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00004C00 File Offset: 0x00002E00
		public static void ClearRichPresence()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_ClearRichPresence();
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00004C10 File Offset: 0x00002E10
		public static string GetFriendRichPresence(CSteamID steamIDFriend, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresence(steamIDFriend, utf8StringHandle));
			}
			return result;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00004C5C File Offset: 0x00002E5C
		public static int GetFriendRichPresenceKeyCount(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendRichPresenceKeyCount(steamIDFriend);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00004C7C File Offset: 0x00002E7C
		public static string GetFriendRichPresenceKeyByIndex(CSteamID steamIDFriend, int iKey)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamFriends_GetFriendRichPresenceKeyByIndex(steamIDFriend, iKey));
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00004CA2 File Offset: 0x00002EA2
		public static void RequestFriendRichPresence(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamFriends_RequestFriendRichPresence(steamIDFriend);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00004CB0 File Offset: 0x00002EB0
		public static bool InviteUserToGame(CSteamID steamIDFriend, string pchConnectString)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchConnectString))
			{
				result = NativeMethods.ISteamFriends_InviteUserToGame(steamIDFriend, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public static int GetCoplayFriendCount()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetCoplayFriendCount();
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00004D18 File Offset: 0x00002F18
		public static CSteamID GetCoplayFriend(int iCoplayFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetCoplayFriend(iCoplayFriend);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00004D40 File Offset: 0x00002F40
		public static int GetFriendCoplayTime(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetFriendCoplayTime(steamIDFriend);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00004D60 File Offset: 0x00002F60
		public static AppId_t GetFriendCoplayGame(CSteamID steamIDFriend)
		{
			InteropHelp.TestIfAvailableClient();
			return (AppId_t)NativeMethods.ISteamFriends_GetFriendCoplayGame(steamIDFriend);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00004D88 File Offset: 0x00002F88
		public static SteamAPICall_t JoinClanChatRoom(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_JoinClanChatRoom(steamIDClan);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public static bool LeaveClanChatRoom(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_LeaveClanChatRoom(steamIDClan);
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00004DD0 File Offset: 0x00002FD0
		public static int GetClanChatMemberCount(CSteamID steamIDClan)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_GetClanChatMemberCount(steamIDClan);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00004DF0 File Offset: 0x00002FF0
		public static CSteamID GetChatMemberByIndex(CSteamID steamIDClan, int iUser)
		{
			InteropHelp.TestIfAvailableClient();
			return (CSteamID)NativeMethods.ISteamFriends_GetChatMemberByIndex(steamIDClan, iUser);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00004E18 File Offset: 0x00003018
		public static bool SendClanChatMessage(CSteamID steamIDClanChat, string pchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchText))
			{
				result = NativeMethods.ISteamFriends_SendClanChatMessage(steamIDClanChat, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00004E60 File Offset: 0x00003060
		public static int GetClanChatMessage(CSteamID steamIDClanChat, int iMessage, out string prgchText, int cchTextMax, out EChatEntryType peChatEntryType, out CSteamID psteamidChatter)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cchTextMax);
			int num = NativeMethods.ISteamFriends_GetClanChatMessage(steamIDClanChat, iMessage, intPtr, cchTextMax, out peChatEntryType, out psteamidChatter);
			prgchText = ((num == 0) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00004EAC File Offset: 0x000030AC
		public static bool IsClanChatAdmin(CSteamID steamIDClanChat, CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsClanChatAdmin(steamIDClanChat, steamIDUser);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00004ED0 File Offset: 0x000030D0
		public static bool IsClanChatWindowOpenInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_IsClanChatWindowOpenInSteam(steamIDClanChat);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00004EF0 File Offset: 0x000030F0
		public static bool OpenClanChatWindowInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_OpenClanChatWindowInSteam(steamIDClanChat);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00004F10 File Offset: 0x00003110
		public static bool CloseClanChatWindowInSteam(CSteamID steamIDClanChat)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_CloseClanChatWindowInSteam(steamIDClanChat);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00004F30 File Offset: 0x00003130
		public static bool SetListenForFriendsMessages(bool bInterceptEnabled)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamFriends_SetListenForFriendsMessages(bInterceptEnabled);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00004F50 File Offset: 0x00003150
		public static bool ReplyToFriendMessage(CSteamID steamIDFriend, string pchMsgToSend)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchMsgToSend))
			{
				result = NativeMethods.ISteamFriends_ReplyToFriendMessage(steamIDFriend, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00004F98 File Offset: 0x00003198
		public static int GetFriendMessage(CSteamID steamIDFriend, int iMessageID, out string pvData, int cubData, out EChatEntryType peChatEntryType)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubData);
			int num = NativeMethods.ISteamFriends_GetFriendMessage(steamIDFriend, iMessageID, intPtr, cubData, out peChatEntryType);
			pvData = ((num == 0) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00004FE0 File Offset: 0x000031E0
		public static SteamAPICall_t GetFollowerCount(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_GetFollowerCount(steamID);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00005008 File Offset: 0x00003208
		public static SteamAPICall_t IsFollowing(CSteamID steamID)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_IsFollowing(steamID);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00005030 File Offset: 0x00003230
		public static SteamAPICall_t EnumerateFollowingList(uint unStartIndex)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamFriends_EnumerateFollowingList(unStartIndex);
		}
	}
}
