using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200003B RID: 59
	[CallbackIdentity(1008)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RegisterActivationCodeResponse_t
	{
		// Token: 0x04000051 RID: 81
		public const int k_iCallback = 1008;

		// Token: 0x04000052 RID: 82
		public ERegisterActivationCodeResult m_eResult;

		// Token: 0x04000053 RID: 83
		public uint m_unPackageRegistered;
	}
}
