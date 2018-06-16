using System;

namespace Verse
{
	// Token: 0x02000D36 RID: 3382
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x06004A72 RID: 19058 RVA: 0x0026C7CC File Offset: 0x0026ABCC
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x04003248 RID: 12872
		private float chance = 1f;
	}
}
