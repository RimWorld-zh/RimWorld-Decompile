using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B1 RID: 2481
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		// Token: 0x06003791 RID: 14225 RVA: 0x001D9BFF File Offset: 0x001D7FFF
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x001D9C18 File Offset: 0x001D8018
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

		// Token: 0x06003793 RID: 14227 RVA: 0x001D9C60 File Offset: 0x001D8060
		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() != RotStage.Fresh;
		}
	}
}
