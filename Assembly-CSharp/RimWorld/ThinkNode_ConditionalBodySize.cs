using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BA RID: 442
	public class ThinkNode_ConditionalBodySize : ThinkNode_Conditional
	{
		// Token: 0x0600092B RID: 2347 RVA: 0x00055E10 File Offset: 0x00054210
		protected override bool Satisfied(Pawn pawn)
		{
			float bodySize = pawn.BodySize;
			return bodySize >= this.min && bodySize <= this.max;
		}

		// Token: 0x040003DD RID: 989
		public float min = 0f;

		// Token: 0x040003DE RID: 990
		public float max = 99999f;
	}
}
