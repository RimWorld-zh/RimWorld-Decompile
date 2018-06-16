using System;

namespace Verse.AI
{
	// Token: 0x02000AE3 RID: 2787
	public struct CastPositionRequest
	{
		// Token: 0x040026E7 RID: 9959
		public Pawn caster;

		// Token: 0x040026E8 RID: 9960
		public Thing target;

		// Token: 0x040026E9 RID: 9961
		public Verb verb;

		// Token: 0x040026EA RID: 9962
		public float maxRangeFromCaster;

		// Token: 0x040026EB RID: 9963
		public float maxRangeFromTarget;

		// Token: 0x040026EC RID: 9964
		public IntVec3 locus;

		// Token: 0x040026ED RID: 9965
		public float maxRangeFromLocus;

		// Token: 0x040026EE RID: 9966
		public bool wantCoverFromTarget;

		// Token: 0x040026EF RID: 9967
		public int maxRegions;
	}
}
