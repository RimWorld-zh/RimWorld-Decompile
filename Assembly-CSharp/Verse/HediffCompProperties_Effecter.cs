using System;

namespace Verse
{
	// Token: 0x02000D0B RID: 3339
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x040031FA RID: 12794
		public EffecterDef stateEffecter = null;

		// Token: 0x040031FB RID: 12795
		public IntRange severityIndices = new IntRange(-1, -1);

		// Token: 0x060049B4 RID: 18868 RVA: 0x00269D41 File Offset: 0x00268141
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}
	}
}
