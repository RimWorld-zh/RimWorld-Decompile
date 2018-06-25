using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D18 RID: 3352
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x0400321C RID: 12828
		public ThoughtDef thought;

		// Token: 0x060049E2 RID: 18914 RVA: 0x0026AC3D File Offset: 0x0026903D
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}
	}
}
