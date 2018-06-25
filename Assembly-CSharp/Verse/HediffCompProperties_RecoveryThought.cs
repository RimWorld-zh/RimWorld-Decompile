using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D17 RID: 3351
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x04003215 RID: 12821
		public ThoughtDef thought;

		// Token: 0x060049E2 RID: 18914 RVA: 0x0026A95D File Offset: 0x00268D5D
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}
	}
}
