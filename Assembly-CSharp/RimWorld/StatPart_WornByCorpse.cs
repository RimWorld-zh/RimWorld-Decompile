using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BB RID: 2491
	public class StatPart_WornByCorpse : StatPart
	{
		// Token: 0x040023C0 RID: 9152
		private const float Factor = 0.1f;

		// Token: 0x060037C3 RID: 14275 RVA: 0x001DAE88 File Offset: 0x001D9288
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					val *= 0.1f;
				}
			}
		}

		// Token: 0x060037C4 RID: 14276 RVA: 0x001DAECC File Offset: 0x001D92CC
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					return "StatsReport_WornByCorpse".Translate() + ": x" + 0.1f.ToStringPercent();
				}
			}
			return null;
		}
	}
}
