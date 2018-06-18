using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000D9 RID: 217
	[CallbackIdentity(702)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LowBatteryPower_t
	{
		// Token: 0x04000299 RID: 665
		public const int k_iCallback = 702;

		// Token: 0x0400029A RID: 666
		public byte m_nMinutesBatteryLeft;
	}
}
