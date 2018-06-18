using System;

namespace RimWorld
{
	// Token: 0x02000964 RID: 2404
	[DefOf]
	public static class IncidentTargetTypeDefOf
	{
		// Token: 0x0600366F RID: 13935 RVA: 0x001D0C25 File Offset: 0x001CF025
		static IncidentTargetTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTypeDefOf));
		}

		// Token: 0x040022C9 RID: 8905
		public static IncidentTargetTypeDef World;

		// Token: 0x040022CA RID: 8906
		public static IncidentTargetTypeDef Caravan;

		// Token: 0x040022CB RID: 8907
		public static IncidentTargetTypeDef Map_RaidBeacon;

		// Token: 0x040022CC RID: 8908
		public static IncidentTargetTypeDef Map_PlayerHome;

		// Token: 0x040022CD RID: 8909
		public static IncidentTargetTypeDef Map_Misc;
	}
}
