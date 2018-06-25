using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D09 RID: 3337
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x040031F8 RID: 12792
		public ChemicalDef chemical;

		// Token: 0x060049AD RID: 18861 RVA: 0x00269C22 File Offset: 0x00268022
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}
	}
}
