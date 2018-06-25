using System;

namespace RimWorld
{
	// Token: 0x02000962 RID: 2402
	[DefOf]
	public static class IncidentTargetTypeDefOf
	{
		// Token: 0x040022C8 RID: 8904
		public static IncidentTargetTypeDef World;

		// Token: 0x040022C9 RID: 8905
		public static IncidentTargetTypeDef Caravan;

		// Token: 0x040022CA RID: 8906
		public static IncidentTargetTypeDef Map_RaidBeacon;

		// Token: 0x040022CB RID: 8907
		public static IncidentTargetTypeDef Map_PlayerHome;

		// Token: 0x040022CC RID: 8908
		public static IncidentTargetTypeDef Map_Misc;

		// Token: 0x0600366C RID: 13932 RVA: 0x001D0F4D File Offset: 0x001CF34D
		static IncidentTargetTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTypeDefOf));
		}
	}
}
