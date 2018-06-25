using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000929 RID: 2345
	[DefOf]
	public static class PawnCapacityDefOf
	{
		// Token: 0x04001FF4 RID: 8180
		public static PawnCapacityDef Consciousness;

		// Token: 0x04001FF5 RID: 8181
		public static PawnCapacityDef Sight;

		// Token: 0x04001FF6 RID: 8182
		public static PawnCapacityDef Hearing;

		// Token: 0x04001FF7 RID: 8183
		public static PawnCapacityDef Moving;

		// Token: 0x04001FF8 RID: 8184
		public static PawnCapacityDef Manipulation;

		// Token: 0x04001FF9 RID: 8185
		public static PawnCapacityDef Talking;

		// Token: 0x04001FFA RID: 8186
		public static PawnCapacityDef Eating;

		// Token: 0x04001FFB RID: 8187
		public static PawnCapacityDef Breathing;

		// Token: 0x04001FFC RID: 8188
		public static PawnCapacityDef BloodFiltration;

		// Token: 0x04001FFD RID: 8189
		public static PawnCapacityDef BloodPumping;

		// Token: 0x04001FFE RID: 8190
		public static PawnCapacityDef Metabolism;

		// Token: 0x06003633 RID: 13875 RVA: 0x001D0E1F File Offset: 0x001CF21F
		static PawnCapacityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnCapacityDefOf));
		}
	}
}
