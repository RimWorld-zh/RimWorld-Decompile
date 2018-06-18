using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000B9 RID: 185
	[CallbackIdentity(3404)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SubmitItemUpdateResult_t
	{
		// Token: 0x0400022E RID: 558
		public const int k_iCallback = 3404;

		// Token: 0x0400022F RID: 559
		public EResult m_eResult;

		// Token: 0x04000230 RID: 560
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bUserNeedsToAcceptWorkshopLegalAgreement;
	}
}
