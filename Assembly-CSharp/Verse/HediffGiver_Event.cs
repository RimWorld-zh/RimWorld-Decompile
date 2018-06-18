using System;

namespace Verse
{
	// Token: 0x02000D35 RID: 3381
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x06004A70 RID: 19056 RVA: 0x0026C7A4 File Offset: 0x0026ABA4
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x04003246 RID: 12870
		private float chance = 1f;
	}
}
