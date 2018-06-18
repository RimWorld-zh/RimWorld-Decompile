using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D09 RID: 3337
	public class HediffCompProperties_DrugEffectFactor : HediffCompProperties
	{
		// Token: 0x06004999 RID: 18841 RVA: 0x00268432 File Offset: 0x00266832
		public HediffCompProperties_DrugEffectFactor()
		{
			this.compClass = typeof(HediffComp_DrugEffectFactor);
		}

		// Token: 0x040031E6 RID: 12774
		public ChemicalDef chemical;
	}
}
