using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000929 RID: 2345
	[DefOf]
	public static class PawnCapacityDefOf
	{
		// Token: 0x04001FED RID: 8173
		public static PawnCapacityDef Consciousness;

		// Token: 0x04001FEE RID: 8174
		public static PawnCapacityDef Sight;

		// Token: 0x04001FEF RID: 8175
		public static PawnCapacityDef Hearing;

		// Token: 0x04001FF0 RID: 8176
		public static PawnCapacityDef Moving;

		// Token: 0x04001FF1 RID: 8177
		public static PawnCapacityDef Manipulation;

		// Token: 0x04001FF2 RID: 8178
		public static PawnCapacityDef Talking;

		// Token: 0x04001FF3 RID: 8179
		public static PawnCapacityDef Eating;

		// Token: 0x04001FF4 RID: 8180
		public static PawnCapacityDef Breathing;

		// Token: 0x04001FF5 RID: 8181
		public static PawnCapacityDef BloodFiltration;

		// Token: 0x04001FF6 RID: 8182
		public static PawnCapacityDef BloodPumping;

		// Token: 0x04001FF7 RID: 8183
		public static PawnCapacityDef Metabolism;

		// Token: 0x06003633 RID: 13875 RVA: 0x001D0B4B File Offset: 0x001CEF4B
		static PawnCapacityDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnCapacityDefOf));
		}
	}
}
