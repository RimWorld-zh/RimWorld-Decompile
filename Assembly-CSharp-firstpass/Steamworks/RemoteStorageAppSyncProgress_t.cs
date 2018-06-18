using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009B RID: 155
	[CallbackIdentity(1303)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageAppSyncProgress_t
	{
		// Token: 0x0400019E RID: 414
		public const int k_iCallback = 1303;

		// Token: 0x0400019F RID: 415
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_rgchCurrentFile;

		// Token: 0x040001A0 RID: 416
		public AppId_t m_nAppID;

		// Token: 0x040001A1 RID: 417
		public uint m_uBytesTransferredThisChunk;

		// Token: 0x040001A2 RID: 418
		public double m_dAppPercentComplete;

		// Token: 0x040001A3 RID: 419
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUploading;
	}
}
