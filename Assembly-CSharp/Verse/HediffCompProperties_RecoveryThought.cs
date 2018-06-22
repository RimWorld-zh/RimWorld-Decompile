using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D15 RID: 3349
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x060049DF RID: 18911 RVA: 0x0026A881 File Offset: 0x00268C81
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x04003215 RID: 12821
		public ThoughtDef thought;
	}
}
