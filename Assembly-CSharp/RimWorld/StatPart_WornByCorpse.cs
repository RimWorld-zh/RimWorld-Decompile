using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B9 RID: 2489
	public class StatPart_WornByCorpse : StatPart
	{
		// Token: 0x040023BF RID: 9151
		private const float Factor = 0.1f;

		// Token: 0x060037BF RID: 14271 RVA: 0x001DAD48 File Offset: 0x001D9148
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

		// Token: 0x060037C0 RID: 14272 RVA: 0x001DAD8C File Offset: 0x001D918C
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
