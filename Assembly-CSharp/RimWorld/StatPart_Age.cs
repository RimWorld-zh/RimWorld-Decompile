using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StatPart_Age : StatPart
	{
		private SimpleCurve curve = null;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					val *= this.AgeMultiplier(pawn);
				}
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					result = "StatsReport_AgeMultiplier".Translate(pawn.ageTracker.AgeBiologicalYears) + ": x" + this.AgeMultiplier(pawn).ToStringPercent();
					goto IL_0073;
				}
			}
			result = (string)null;
			goto IL_0073;
			IL_0073:
			return result;
		}

		private float AgeMultiplier(Pawn pawn)
		{
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
		}

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve != null)
				yield break;
			yield return "curve is null.";
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
