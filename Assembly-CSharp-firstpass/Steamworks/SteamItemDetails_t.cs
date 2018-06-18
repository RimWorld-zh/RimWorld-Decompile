using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200012A RID: 298
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamItemDetails_t
	{
		// Token: 0x0400060D RID: 1549
		public SteamItemInstanceID_t m_itemId;

		// Token: 0x0400060E RID: 1550
		public SteamItemDef_t m_iDefinition;

		// Token: 0x0400060F RID: 1551
		public ushort m_unQuantity;

		// Token: 0x04000610 RID: 1552
		public ushort m_unFlags;
	}
}
