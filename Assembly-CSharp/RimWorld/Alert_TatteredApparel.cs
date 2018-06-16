using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AE RID: 1966
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x06002B6C RID: 11116 RVA: 0x0016EE1E File Offset: 0x0016D21E
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x0016EE44 File Offset: 0x0016D244
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}
	}
}
