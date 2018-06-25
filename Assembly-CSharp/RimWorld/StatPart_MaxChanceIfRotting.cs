using System;
using Verse;

namespace RimWorld
{
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		public StatPart_MaxChanceIfRotting()
		{
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

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

		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() != RotStage.Fresh;
		}
	}
}
