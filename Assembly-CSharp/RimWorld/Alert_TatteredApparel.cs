using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AC RID: 1964
	public class Alert_TatteredApparel : Alert_Thought
	{
		// Token: 0x06002B6A RID: 11114 RVA: 0x0016F43E File Offset: 0x0016D83E
		public Alert_TatteredApparel()
		{
			this.defaultLabel = "AlertTatteredApparel".Translate();
			this.explanationKey = "AlertTatteredApparelDesc";
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x0016F464 File Offset: 0x0016D864
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.ApparelDamaged;
			}
		}
	}
}
