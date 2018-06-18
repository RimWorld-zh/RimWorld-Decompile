using System;

namespace Steamworks
{
	// Token: 0x02000143 RID: 323
	public static class SteamMusicRemote
	{
		// Token: 0x060005FD RID: 1533 RVA: 0x00008AE8 File Offset: 0x00006CE8
		public static bool RegisterSteamMusicRemote(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamMusicRemote_RegisterSteamMusicRemote(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00008B30 File Offset: 0x00006D30
		public static bool DeregisterSteamMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_DeregisterSteamMusicRemote();
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00008B50 File Offset: 0x00006D50
		public static bool BIsCurrentMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BIsCurrentMusicRemote();
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00008B70 File Offset: 0x00006D70
		public static bool BActivationSuccess(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BActivationSuccess(bValue);
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00008B90 File Offset: 0x00006D90
		public static bool SetDisplayName(string pchDisplayName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDisplayName))
			{
				result = NativeMethods.ISteamMusicRemote_SetDisplayName(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00008BD8 File Offset: 0x00006DD8
		public static bool SetPNGIcon_64x64(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetPNGIcon_64x64(pvBuffer, cbBufferLength);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00008BFC File Offset: 0x00006DFC
		public static bool EnablePlayPrevious(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayPrevious(bValue);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00008C1C File Offset: 0x00006E1C
		public static bool EnablePlayNext(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayNext(bValue);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00008C3C File Offset: 0x00006E3C
		public static bool EnableShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableShuffled(bValue);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00008C5C File Offset: 0x00006E5C
		public static bool EnableLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableLooped(bValue);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00008C7C File Offset: 0x00006E7C
		public static bool EnableQueue(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableQueue(bValue);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00008C9C File Offset: 0x00006E9C
		public static bool EnablePlaylists(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlaylists(bValue);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00008CBC File Offset: 0x00006EBC
		public static bool UpdatePlaybackStatus(AudioPlayback_Status nStatus)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdatePlaybackStatus(nStatus);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00008CDC File Offset: 0x00006EDC
		public static bool UpdateShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateShuffled(bValue);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00008CFC File Offset: 0x00006EFC
		public static bool UpdateLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateLooped(bValue);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00008D1C File Offset: 0x00006F1C
		public static bool UpdateVolume(float flValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateVolume(flValue);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00008D3C File Offset: 0x00006F3C
		public static bool CurrentEntryWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryWillChange();
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00008D5C File Offset: 0x00006F5C
		public static bool CurrentEntryIsAvailable(bool bAvailable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryIsAvailable(bAvailable);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00008D7C File Offset: 0x00006F7C
		public static bool UpdateCurrentEntryText(string pchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchText))
			{
				result = NativeMethods.ISteamMusicRemote_UpdateCurrentEntryText(utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00008DC4 File Offset: 0x00006FC4
		public static bool UpdateCurrentEntryElapsedSeconds(int nValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(nValue);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00008DE4 File Offset: 0x00006FE4
		public static bool UpdateCurrentEntryCoverArt(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryCoverArt(pvBuffer, cbBufferLength);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00008E08 File Offset: 0x00007008
		public static bool CurrentEntryDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryDidChange();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00008E28 File Offset: 0x00007028
		public static bool QueueWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueWillChange();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00008E48 File Offset: 0x00007048
		public static bool ResetQueueEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetQueueEntries();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00008E68 File Offset: 0x00007068
		public static bool SetQueueEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = NativeMethods.ISteamMusicRemote_SetQueueEntry(nID, nPosition, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00008EB0 File Offset: 0x000070B0
		public static bool SetCurrentQueueEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentQueueEntry(nID);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00008ED0 File Offset: 0x000070D0
		public static bool QueueDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueDidChange();
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00008EF0 File Offset: 0x000070F0
		public static bool PlaylistWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistWillChange();
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00008F10 File Offset: 0x00007110
		public static bool ResetPlaylistEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetPlaylistEntries();
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00008F30 File Offset: 0x00007130
		public static bool SetPlaylistEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = NativeMethods.ISteamMusicRemote_SetPlaylistEntry(nID, nPosition, utf8StringHandle);
			}
			return result;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00008F78 File Offset: 0x00007178
		public static bool SetCurrentPlaylistEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentPlaylistEntry(nID);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00008F98 File Offset: 0x00007198
		public static bool PlaylistDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistDidChange();
		}
	}
}
