using System;

namespace Verse
{
	// Token: 0x02000D0B RID: 3339
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x060049A0 RID: 18848 RVA: 0x00268551 File Offset: 0x00266951
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x040031E8 RID: 12776
		public EffecterDef stateEffecter = null;

		// Token: 0x040031E9 RID: 12777
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}
