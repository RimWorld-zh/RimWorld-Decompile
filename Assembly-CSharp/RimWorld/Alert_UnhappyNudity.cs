using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020007AF RID: 1967
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x06002B6E RID: 11118 RVA: 0x0016EE5E File Offset: 0x0016D25E
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002B6F RID: 11119 RVA: 0x0016EE84 File Offset: 0x0016D284
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}
	}
}
