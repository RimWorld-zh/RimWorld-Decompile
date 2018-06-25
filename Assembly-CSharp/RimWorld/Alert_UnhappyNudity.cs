using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AD RID: 1965
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x06002B6D RID: 11117 RVA: 0x0016F21A File Offset: 0x0016D61A
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002B6E RID: 11118 RVA: 0x0016F240 File Offset: 0x0016D640
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}
	}
}
