using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000127 RID: 295
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SteamControllerState_t
	{
		// Token: 0x04000600 RID: 1536
		public uint unPacketNum;

		// Token: 0x04000601 RID: 1537
		public ulong ulButtons;

		// Token: 0x04000602 RID: 1538
		public short sLeftPadX;

		// Token: 0x04000603 RID: 1539
		public short sLeftPadY;

		// Token: 0x04000604 RID: 1540
		public short sRightPadX;

		// Token: 0x04000605 RID: 1541
		public short sRightPadY;
	}
}
