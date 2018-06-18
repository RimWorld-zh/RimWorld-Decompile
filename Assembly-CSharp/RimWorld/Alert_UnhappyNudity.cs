using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AF RID: 1967
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x06002B70 RID: 11120 RVA: 0x0016EEF2 File Offset: 0x0016D2F2
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002B71 RID: 11121 RVA: 0x0016EF18 File Offset: 0x0016D318
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}
	}
}
