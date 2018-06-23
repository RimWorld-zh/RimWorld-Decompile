using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D06 RID: 3334
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x040031F1 RID: 12785
		public ChemicalDef chemical;

		// Token: 0x060049AA RID: 18858 RVA: 0x00269866 File Offset: 0x00267C66
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}
	}
}
