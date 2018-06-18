using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000DD RID: 221
	[CallbackIdentity(714)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GamepadTextInputDismissed_t
	{
		// Token: 0x040002A0 RID: 672
		public const int k_iCallback = 714;

		// Token: 0x040002A1 RID: 673
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSubmitted;

		// Token: 0x040002A2 RID: 674
		public uint m_unSubmittedText;
	}
}
