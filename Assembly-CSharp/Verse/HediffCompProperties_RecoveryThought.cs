using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D18 RID: 3352
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x060049CE RID: 18894 RVA: 0x0026944D File Offset: 0x0026784D
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x0400320A RID: 12810
		public ThoughtDef thought;
	}
}
