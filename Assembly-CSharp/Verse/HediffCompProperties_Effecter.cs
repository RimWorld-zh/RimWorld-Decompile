using System;

namespace Verse
{
	// Token: 0x02000D08 RID: 3336
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x060049B1 RID: 18865 RVA: 0x00269985 File Offset: 0x00267D85
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x040031F3 RID: 12787
		public EffecterDef stateEffecter = null;

		// Token: 0x040031F4 RID: 12788
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}
