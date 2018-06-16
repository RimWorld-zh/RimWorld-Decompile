using System;

namespace Verse
{
	// Token: 0x02000D17 RID: 3351
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x060049C6 RID: 18886 RVA: 0x00269062 File Offset: 0x00267462
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		// Token: 0x04003204 RID: 12804
		public float infectionChance = 0.5f;
	}
}
