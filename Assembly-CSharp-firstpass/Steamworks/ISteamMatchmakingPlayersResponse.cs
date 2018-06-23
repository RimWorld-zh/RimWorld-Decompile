using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200001C RID: 28
	public class ISteamMatchmakingPlayersResponse
	{
		// Token: 0x04000029 RID: 41
		private ISteamMatchmakingPlayersResponse.VTable m_VTable;

		// Token: 0x0400002A RID: 42
		private IntPtr m_pVTable;

		// Token: 0x0400002B RID: 43
		private GCHandle m_pGCHandle;

		// Token: 0x0400002C RID: 44
		private ISteamMatchmakingPlayersResponse.AddPlayerToList m_AddPlayerToList;

		// Token: 0x0400002D RID: 45
		private ISteamMatchmakingPlayersResponse.PlayersFailedToRespond m_PlayersFailedToRespond;

		// Token: 0x0400002E RID: 46
		private ISteamMatchmakingPlayersResponse.PlayersRefreshComplete m_PlayersRefreshComplete;

		// Token: 0x0600006D RID: 109 RVA: 0x00002BBC File Offset: 0x00000DBC
		public ISteamMatchmakingPlayersResponse(ISteamMatchmakingPlayersResponse.AddPlayerToList onAddPlayerToList, ISteamMatchmakingPlayersResponse.PlayersFailedToRespond onPlayersFailedToRespond, ISteamMatchmakingPlayersResponse.PlayersRefreshComplete onPlayersRefreshComplete)
		{
			if (onAddPlayerToList == null || onPlayersFailedToRespond == null || onPlayersRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_AddPlayerToList = onAddPlayerToList;
			this.m_PlayersFailedToRespond = onPlayersFailedToRespond;
			this.m_PlayersRefreshComplete = onPlayersRefreshComplete;
			this.m_VTable = new ISteamMatchmakingPlayersResponse.VTable
			{
				m_VTAddPlayerToList = new ISteamMatchmakingPlayersResponse.InternalAddPlayerToList(this.InternalOnAddPlayerToList),
				m_VTPlayersFailedToRespond = new ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond(this.InternalOnPlayersFailedToRespond),
				m_VTPlayersRefreshComplete = new ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete(this.InternalOnPlayersRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingPlayersResponse.VTable)));
			Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002C84 File Offset: 0x00000E84
		~ISteamMatchmakingPlayersResponse()
		{
			if (this.m_pVTable != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pGCHandle.IsAllocated)
			{
				this.m_pGCHandle.Free();
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002CF0 File Offset: 0x00000EF0
		private void InternalOnAddPlayerToList(IntPtr thisptr, IntPtr pchName, int nScore, float flTimePlayed)
		{
			this.m_AddPlayerToList(InteropHelp.PtrToStringUTF8(pchName), nScore, flTimePlayed);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002D07 File Offset: 0x00000F07
		private void InternalOnPlayersFailedToRespond(IntPtr thisptr)
		{
			this.m_PlayersFailedToRespond();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002D15 File Offset: 0x00000F15
		private void InternalOnPlayersRefreshComplete(IntPtr thisptr)
		{
			this.m_PlayersRefreshComplete();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002D24 File Offset: 0x00000F24
		public static explicit operator IntPtr(ISteamMatchmakingPlayersResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x0200001D RID: 29
		// (Invoke) Token: 0x06000074 RID: 116
		public delegate void AddPlayerToList(string pchName, int nScore, float flTimePlayed);

		// Token: 0x0200001E RID: 30
		// (Invoke) Token: 0x06000078 RID: 120
		public delegate void PlayersFailedToRespond();

		// Token: 0x0200001F RID: 31
		// (Invoke) Token: 0x0600007C RID: 124
		public delegate void PlayersRefreshComplete();

		// Token: 0x02000020 RID: 32
		// (Invoke) Token: 0x06000080 RID: 128
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalAddPlayerToList(IntPtr thisptr, IntPtr pchName, int nScore, float flTimePlayed);

		// Token: 0x02000021 RID: 33
		// (Invoke) Token: 0x06000084 RID: 132
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalPlayersFailedToRespond(IntPtr thisptr);

		// Token: 0x02000022 RID: 34
		// (Invoke) Token: 0x06000088 RID: 136
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		public delegate void InternalPlayersRefreshComplete(IntPtr thisptr);

		// Token: 0x02000023 RID: 35
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x0400002F RID: 47
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalAddPlayerToList m_VTAddPlayerToList;

			// Token: 0x04000030 RID: 48
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalPlayersFailedToRespond m_VTPlayersFailedToRespond;

			// Token: 0x04000031 RID: 49
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPlayersResponse.InternalPlayersRefreshComplete m_VTPlayersRefreshComplete;
		}
	}
}
