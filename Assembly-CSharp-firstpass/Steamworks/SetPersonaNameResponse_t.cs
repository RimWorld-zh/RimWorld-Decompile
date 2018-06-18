using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200004F RID: 79
	[CallbackIdentity(347)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetPersonaNameResponse_t
	{
		// Token: 0x04000094 RID: 148
		public const int k_iCallback = 347;

		// Token: 0x04000095 RID: 149
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSuccess;

		// Token: 0x04000096 RID: 150
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocalSuccess;

		// Token: 0x04000097 RID: 151
		public EResult m_result;
	}
}
