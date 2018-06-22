using System;

namespace Verse
{
	// Token: 0x02000D32 RID: 3378
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x06004A84 RID: 19076 RVA: 0x0026DD34 File Offset: 0x0026C134
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}

		// Token: 0x04003251 RID: 12881
		private float chance = 1f;
	}
}
