using System;

namespace RimWorld
{
	// Token: 0x02000960 RID: 2400
	[DefOf]
	public static class IncidentTargetTypeDefOf
	{
		// Token: 0x06003668 RID: 13928 RVA: 0x001D0E0D File Offset: 0x001CF20D
		static IncidentTargetTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTypeDefOf));
		}

		// Token: 0x040022C7 RID: 8903
		public static IncidentTargetTypeDef World;

		// Token: 0x040022C8 RID: 8904
		public static IncidentTargetTypeDef Caravan;

		// Token: 0x040022C9 RID: 8905
		public static IncidentTargetTypeDef Map_RaidBeacon;

		// Token: 0x040022CA RID: 8906
		public static IncidentTargetTypeDef Map_PlayerHome;

		// Token: 0x040022CB RID: 8907
		public static IncidentTargetTypeDef Map_Misc;
	}
}
