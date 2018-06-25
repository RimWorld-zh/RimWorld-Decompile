using System;

namespace RimWorld
{
	// Token: 0x02000962 RID: 2402
	[DefOf]
	public static class IncidentTargetTypeDefOf
	{
		// Token: 0x040022CF RID: 8911
		public static IncidentTargetTypeDef World;

		// Token: 0x040022D0 RID: 8912
		public static IncidentTargetTypeDef Caravan;

		// Token: 0x040022D1 RID: 8913
		public static IncidentTargetTypeDef Map_RaidBeacon;

		// Token: 0x040022D2 RID: 8914
		public static IncidentTargetTypeDef Map_PlayerHome;

		// Token: 0x040022D3 RID: 8915
		public static IncidentTargetTypeDef Map_Misc;

		// Token: 0x0600366C RID: 13932 RVA: 0x001D1221 File Offset: 0x001CF621
		static IncidentTargetTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTypeDefOf));
		}
	}
}
