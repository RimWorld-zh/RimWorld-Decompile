using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AC RID: 1964
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x06002B6B RID: 11115 RVA: 0x0016F1DA File Offset: 0x0016D5DA
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002B6C RID: 11116 RVA: 0x0016F200 File Offset: 0x0016D600
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}
	}
}
