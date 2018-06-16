using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D19 RID: 3353
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x060049D0 RID: 18896 RVA: 0x00269475 File Offset: 0x00267875
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x0400320C RID: 12812
		public ThoughtDef thought;
	}
}
