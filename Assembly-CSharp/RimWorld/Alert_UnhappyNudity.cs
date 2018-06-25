using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AD RID: 1965
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x06002B6C RID: 11116 RVA: 0x0016F47E File Offset: 0x0016D87E
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x0016F4A4 File Offset: 0x0016D8A4
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}
	}
}
