using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001BA RID: 442
	public class ThinkNode_ConditionalBodySize : ThinkNode_Conditional
	{
		// Token: 0x040003DB RID: 987
		public float min = 0f;

		// Token: 0x040003DC RID: 988
		public float max = 99999f;

		// Token: 0x06000929 RID: 2345 RVA: 0x00055E24 File Offset: 0x00054224
		protected override bool Satisfied(Pawn pawn)
		{
			float bodySize = pawn.BodySize;
			return bodySize >= this.min && bodySize <= this.max;
		}
	}
}
