using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000016 RID: 22
	public class ISteamMatchmakingPingResponse
	{
		// Token: 0x06000057 RID: 87 RVA: 0x00002A5C File Offset: 0x00000C5C
		public ISteamMatchmakingPingResponse(ISteamMatchmakingPingResponse.ServerResponded onServerResponded, ISteamMatchmakingPingResponse.ServerFailedToRespond onServerFailedToRespond)
		{
			if (onServerResponded == null || onServerFailedToRespond == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_VTable = new ISteamMatchmakingPingResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingPingResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingPingResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingPingResponse.VTable)));
			Marshal.StructureToPtr(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002B08 File Offset: 0x00000D08
		~ISteamMatchmakingPingResponse()
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

		// Token: 0x06000059 RID: 89 RVA: 0x00002B74 File Offset: 0x00000D74
		private void InternalOnServerResponded(IntPtr thisptr, gameserveritem_t server)
		{
			this.m_ServerResponded(server);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002B83 File Offset: 0x00000D83
		private void InternalOnServerFailedToRespond(IntPtr thisptr)
		{
			this.m_ServerFailedToRespond();
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002B94 File Offset: 0x00000D94
		public static explicit operator IntPtr(ISteamMatchmakingPingResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000022 RID: 34
		private ISteamMatchmakingPingResponse.VTable m_VTable;

		// Token: 0x04000023 RID: 35
		private IntPtr m_pVTable;

		// Token: 0x04000024 RID: 36
		private GCHandle m_pGCHandle;

		// Token: 0x04000025 RID: 37
		private ISteamMatchmakingPingResponse.ServerResponded m_ServerResponded;

		// Token: 0x04000026 RID: 38
		private ISteamMatchmakingPingResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x02000017 RID: 23
		// (Invoke) Token: 0x0600005D RID: 93
		public delegate void ServerResponded(gameserveritem_t server);

		// Token: 0x02000018 RID: 24
		// (Invoke) Token: 0x06000061 RID: 97
		public delegate void ServerFailedToRespond();

		// Token: 0x02000019 RID: 25
		// (Invoke) Token: 0x06000065 RID: 101
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(IntPtr thisptr, gameserveritem_t server);

		// Token: 0x0200001A RID: 26
		// (Invoke) Token: 0x06000069 RID: 105
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(IntPtr thisptr);

		// Token: 0x0200001B RID: 27
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000027 RID: 39
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000028 RID: 40
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
		}
	}
}
