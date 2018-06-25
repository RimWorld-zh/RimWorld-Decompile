using System;

namespace Verse
{
	// Token: 0x02000D0A RID: 3338
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x040031F3 RID: 12787
		public EffecterDef stateEffecter = null;

		// Token: 0x040031F4 RID: 12788
		public IntRange severityIndices = new IntRange(-1, -1);

		// Token: 0x060049B4 RID: 18868 RVA: 0x00269A61 File Offset: 0x00267E61
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}
	}
}
