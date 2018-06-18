using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000A1 RID: 161
	[CallbackIdentity(1312)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageEnumerateUserPublishedFilesResult_t
	{
		// Token: 0x040001B5 RID: 437
		public const int k_iCallback = 1312;

		// Token: 0x040001B6 RID: 438
		public EResult m_eResult;

		// Token: 0x040001B7 RID: 439
		public int m_nResultsReturned;

		// Token: 0x040001B8 RID: 440
		public int m_nTotalResultCount;

		// Token: 0x040001B9 RID: 441
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
		public PublishedFileId_t[] m_rgPublishedFileId;
	}
}
