using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StatPart_Mood : StatPart
	{
		private SimpleCurve curve;

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
		}

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.mood != null)
				{
					val *= this.MoodMultiplier(pawn.needs.mood.CurLevel);
				}
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.mood != null)
				{
					return "StatsReport_MoodMultiplier".Translate(pawn.needs.mood.CurLevel.ToStringPercent()) + ": x" + this.MoodMultiplier(pawn.needs.mood.CurLevel).ToStringPercent();
				}
			}
			return (string)null;
		}

		private float MoodMultiplier(float mood)
		{
			return this.curve.Evaluate(mood);
		}
	}
}
