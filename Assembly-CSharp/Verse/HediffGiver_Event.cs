using System;

namespace Verse
{
	// Token: 0x02000D34 RID: 3380
	public class HediffGiver_Event : HediffGiver
	{
		// Token: 0x04003251 RID: 12881
		private float chance = 1f;

		// Token: 0x06004A88 RID: 19080 RVA: 0x0026DE60 File Offset: 0x0026C260
		public bool EventOccurred(Pawn pawn)
		{
			return Rand.Value < this.chance && base.TryApply(pawn, null);
		}
	}
}
