using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AB RID: 1963
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x06002B69 RID: 11113 RVA: 0x0016F0CA File Offset: 0x0016D4CA
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002B6A RID: 11114 RVA: 0x0016F0F0 File Offset: 0x0016D4F0
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}
	}
}
