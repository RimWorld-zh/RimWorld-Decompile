using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BD RID: 2493
	public class StatPart_WornByCorpse : StatPart
	{
		// Token: 0x060037C5 RID: 14277 RVA: 0x001DAB70 File Offset: 0x001D8F70
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

		// Token: 0x060037C6 RID: 14278 RVA: 0x001DABB4 File Offset: 0x001D8FB4
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

		// Token: 0x040023C4 RID: 9156
		private const float Factor = 0.1f;
	}
}
