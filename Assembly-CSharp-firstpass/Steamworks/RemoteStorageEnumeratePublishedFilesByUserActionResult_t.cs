using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B1 RID: 177
	[CallbackIdentity(1328)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumeratePublishedFilesByUserActionResult_t
	{
		// Token: 0x0400020F RID: 527
		public const int k_iCallback = 1328;

		// Token: 0x04000210 RID: 528
		public EResult m_eResult;

		// Token: 0x04000211 RID: 529
		public EWorkshopFileAction m_eAction;

		// Token: 0x04000212 RID: 530
		public int m_nResultsReturned;

		// Token: 0x04000213 RID: 531
		public int m_nTotalResultCount;

		// Token: 0x04000214 RID: 532
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;

		// Token: 0x04000215 RID: 533
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public uint[] m_rgRTimeUpdated;
	}
}
