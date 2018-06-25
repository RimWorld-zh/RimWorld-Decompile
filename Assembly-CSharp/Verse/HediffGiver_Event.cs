using System;

namespace Verse
{
	// Token: 0x02000D35 RID: 3381
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x04003258 RID: 12888
		private float chance = 1f;

		// Token: 0x06004A88 RID: 19080 RVA: 0x0026E140 File Offset: 0x0026C540
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}
	}
}
