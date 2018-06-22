using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AD RID: 2477
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		// Token: 0x0600378A RID: 14218 RVA: 0x001D9DC3 File Offset: 0x001D81C3
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x001D9DDC File Offset: 0x001D81DC
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

		// Token: 0x0600378C RID: 14220 RVA: 0x001D9E24 File Offset: 0x001D8224
		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() != RotStage.Fresh;
		}
	}
}
