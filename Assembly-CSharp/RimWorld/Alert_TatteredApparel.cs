using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AE RID: 1966
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x06002B6E RID: 11118 RVA: 0x0016EEB2 File Offset: 0x0016D2B2
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002B6F RID: 11119 RVA: 0x0016EED8 File Offset: 0x0016D2D8
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}
	}
}
