using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200000E RID: 14
	public class ISteamMatchmakingServerListResponse
	{
		// Token: 0x04000019 RID: 25
		private ISteamMatchmakingServerListResponse.VTable m_VTable;

		// Token: 0x0400001A RID: 26
		private IntPtr m_pVTable;

		// Token: 0x0400001B RID: 27
		private GCHandle m_pGCHandle;

		// Token: 0x0400001C RID: 28
		private ISteamMatchmakingServerListResponse.ServerResponded m_ServerResponded;

		// Token: 0x0400001D RID: 29
		private ISteamMatchmakingServerListResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x0400001E RID: 30
		private ISteamMatchmakingServerListResponse.RefreshComplete m_RefreshComplete;

		// Token: 0x06000038 RID: 56 RVA: 0x000028D0 File Offset: 0x00000AD0
		public ISteamMatchmakingServerListResponse(ISteamMatchmakingServerListResponse.ServerResponded onServerResponded, ISteamMatchmakingServerListResponse.ServerFailedToRespond onServerFailedToRespond, ISteamMatchmakingServerListResponse.RefreshComplete onRefreshComplete)
		{
			if (onServerResponded == null || onServerFailedToRespond == null || onRefreshComplete == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_RefreshComplete = onRefreshComplete;
			this.m_VTable = new ISteamMatchmakingServerListResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingServerListResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingServerListResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond),
				m_VTRefreshComplete = new ISteamMatchmakingServerListResponse.InternalRefreshComplete(this.InternalOnRefreshComplete)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingServerListResponse.VTable)));
			Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002998 File Offset: 0x00000B98
		~ISteamMatchmakingServerListResponse()
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

		// Token: 0x0600003A RID: 58 RVA: 0x00002A04 File Offset: 0x00000C04
		private void InternalOnServerResponded(IntPtr thisptr, HServerListRequest hRequest, int iServer)
		{
			this.m_ServerResponded(hRequest, iServer);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002A14 File Offset: 0x00000C14
		private void InternalOnServerFailedToRespond(IntPtr thisptr, HServerListRequest hRequest, int iServer)
		{
			this.m_ServerFailedToRespond(hRequest, iServer);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002A24 File Offset: 0x00000C24
		private void InternalOnRefreshComplete(IntPtr thisptr, HServerListRequest hRequest, EMatchMakingServerResponse response)
		{
			this.m_RefreshComplete(hRequest, response);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002A34 File Offset: 0x00000C34
		public static explicit operator IntPtr(ISteamMatchmakingServerListResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x0600003F RID: 63
		public delegate void ServerResponded(HServerListRequest hRequest, int iServer);

		// Token: 0x02000010 RID: 16
		// (Invoke) Token: 0x06000043 RID: 67
		public delegate void ServerFailedToRespond(HServerListRequest hRequest, int iServer);

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x06000047 RID: 71
		public delegate void RefreshComplete(HServerListRequest hRequest, EMatchMakingServerResponse response);

		// Token: 0x02000012 RID: 18
		// (Invoke) Token: 0x0600004B RID: 75
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(IntPtr thisptr, HServerListRequest hRequest, int iServer);

		// Token: 0x02000013 RID: 19
		// (Invoke) Token: 0x0600004F RID: 79
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(IntPtr thisptr, HServerListRequest hRequest, int iServer);

		// Token: 0x02000014 RID: 20
		// (Invoke) Token: 0x06000053 RID: 83
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalRefreshComplete(IntPtr thisptr, HServerListRequest hRequest, EMatchMakingServerResponse response);

		// Token: 0x02000015 RID: 21
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x0400001F RID: 31
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000020 RID: 32
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;

			// Token: 0x04000021 RID: 33
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingServerListResponse.InternalRefreshComplete m_VTRefreshComplete;
		}
	}
}
