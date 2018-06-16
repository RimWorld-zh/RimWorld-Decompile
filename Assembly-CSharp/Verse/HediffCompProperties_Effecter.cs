using System;

namespace Verse
{
	// Token: 0x02000D0C RID: 3340
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x060049A2 RID: 18850 RVA: 0x00268579 File Offset: 0x00266979
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x040031EA RID: 12778
		public EffecterDef stateEffecter = null;

		// Token: 0x040031EB RID: 12779
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}
