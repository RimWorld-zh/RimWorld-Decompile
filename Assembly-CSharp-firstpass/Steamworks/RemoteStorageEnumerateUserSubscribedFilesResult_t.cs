using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A3 RID: 163
	[CallbackIdentity(1314)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserSubscribedFilesResult_t
	{
		// Token: 0x040001BD RID: 445
		public const int k_iCallback = 1314;

		// Token: 0x040001BE RID: 446
		public EResult m_eResult;

		// Token: 0x040001BF RID: 447
		public int m_nResultsReturned;

		// Token: 0x040001C0 RID: 448
		public int m_nTotalResultCount;

		// Token: 0x040001C1 RID: 449
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x040001C2 RID: 450
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeSubscribed;
	}
}
