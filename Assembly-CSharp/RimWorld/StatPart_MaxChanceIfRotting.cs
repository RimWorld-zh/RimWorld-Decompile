using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B1 RID: 2481
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		// Token: 0x0600378F RID: 14223 RVA: 0x001D9B2B File Offset: 0x001D7F2B
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x001D9B44 File Offset: 0x001D7F44
		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (this.IsRotting(req))
			{
				result = "StatsReport_NotFresh".Translate() + ": " + 1f.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x001D9B8C File Offset: 0x001D7F8C
		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() != RotStage.Fresh;
		}
	}
}
