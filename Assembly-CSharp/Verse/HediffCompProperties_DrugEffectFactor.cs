using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D0A RID: 3338
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x0600499B RID: 18843 RVA: 0x0026845A File Offset: 0x0026685A
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		// Token: 0x040031E8 RID: 12776
		public ChemicalDef chemical;
	}
}
