using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000007 RID: 7
	[StructLayout(LayoutKind.Sequential)]
	internal class CCallbackBase
	{
		// Token: 0x0400000F RID: 15
		public const byte k_ECallbackFlagsRegistered = 1;

		// Token: 0x04000010 RID: 16
		public const byte k_ECallbackFlagsGameServer = 2;

		// Token: 0x04000011 RID: 17
		public IntPtr m_vfptr;

		// Token: 0x04000012 RID: 18
		public byte m_nCallbackFlags;

		// Token: 0x04000013 RID: 19
		public int m_iCallback;
	}
}
