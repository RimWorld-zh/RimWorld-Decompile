using System;

namespace Verse.AI
{
	// Token: 0x02000AE1 RID: 2785
	public struct CastPositionRequest
	{
		// Token: 0x040026E3 RID: 9955
		public Pawn caster;

		// Token: 0x040026E4 RID: 9956
		public Thing target;

		// Token: 0x040026E5 RID: 9957
		public Verb verb;

		// Token: 0x040026E6 RID: 9958
		public float maxRangeFromCaster;

		// Token: 0x040026E7 RID: 9959
		public float maxRangeFromTarget;

		// Token: 0x040026E8 RID: 9960
		public IntVec3 locus;

		// Token: 0x040026E9 RID: 9961
		public float maxRangeFromLocus;

		// Token: 0x040026EA RID: 9962
		public bool wantCoverFromTarget;

		// Token: 0x040026EB RID: 9963
		public int maxRegions;
	}
}
