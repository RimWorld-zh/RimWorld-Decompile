using System;

namespace Verse.AI
{
	// Token: 0x02000AE2 RID: 2786
	public struct CastPositionRequest
	{
		// Token: 0x040026EA RID: 9962
		public Pawn caster;

		// Token: 0x040026EB RID: 9963
		public Thing target;

		// Token: 0x040026EC RID: 9964
		public Verb verb;

		// Token: 0x040026ED RID: 9965
		public float maxRangeFromCaster;

		// Token: 0x040026EE RID: 9966
		public float maxRangeFromTarget;

		// Token: 0x040026EF RID: 9967
		public IntVec3 locus;

		// Token: 0x040026F0 RID: 9968
		public float maxRangeFromLocus;

		// Token: 0x040026F1 RID: 9969
		public bool wantCoverFromTarget;

		// Token: 0x040026F2 RID: 9970
		public int maxRegions;
	}
}
