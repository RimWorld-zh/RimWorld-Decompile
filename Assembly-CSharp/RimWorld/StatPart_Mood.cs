using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class StatPart_Mood : StatPart
	{
		private SimpleCurve curve;

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			StatPart_Mood.<ConfigErrors>c__Iterator1AC <ConfigErrors>c__Iterator1AC = new StatPart_Mood.<ConfigErrors>c__Iterator1AC();
			<ConfigErrors>c__Iterator1AC.<>f__this = this;
			StatPart_Mood.<ConfigErrors>c__Iterator1AC expr_0E = <ConfigErrors>c__Iterator1AC;
			expr_0E.$PC = -2;
			return expr_0E;
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
					return "StatsReport_MoodMultiplier".Translate(new object[]
					{
						pawn.needs.mood.CurLevel.ToStringPercent()
					}) + ": x" + this.MoodMultiplier(pawn.needs.mood.CurLevel).ToStringPercent();
				}
			}
			return null;
		}

		private float MoodMultiplier(float mood)
		{
			return this.curve.Evaluate(mood);
		}
	}
}
