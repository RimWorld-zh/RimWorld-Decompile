using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200009E RID: 158
	[CallbackIdentity(1307)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageFileShareResult_t
	{
		// Token: 0x040001AA RID: 426
		public const int k_iCallback = 1307;

		// Token: 0x040001AB RID: 427
		public EResult m_eResult;

		// Token: 0x040001AC RID: 428
		public UGCHandle_t m_hFile;

		// Token: 0x040001AD RID: 429
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string m_rgchFilename;
	}
}
