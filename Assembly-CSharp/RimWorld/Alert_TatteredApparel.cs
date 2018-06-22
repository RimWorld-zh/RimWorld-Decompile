using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AA RID: 1962
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x06002B67 RID: 11111 RVA: 0x0016F08A File Offset: 0x0016D48A
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002B68 RID: 11112 RVA: 0x0016F0B0 File Offset: 0x0016D4B0
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}
	}
}
