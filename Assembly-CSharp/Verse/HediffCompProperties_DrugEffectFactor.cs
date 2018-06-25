using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D08 RID: 3336
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x040031F1 RID: 12785
		public ChemicalDef chemical;

		// Token: 0x060049AD RID: 18861 RVA: 0x00269942 File Offset: 0x00267D42
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}
	}
}
