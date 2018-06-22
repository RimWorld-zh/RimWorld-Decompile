using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A0 RID: 2464
	public class StatPart_Age : StatPart
	{
		// Token: 0x0600374C RID: 14156 RVA: 0x001D8CC0 File Offset: 0x001D70C0
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

		// Token: 0x0600374D RID: 14157 RVA: 0x001D8D08 File Offset: 0x001D7108
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					return "StatsReport_AgeMultiplier".Translate(new object[]
					{
						pawn.ageTracker.AgeBiologicalYears
					}) + ": x" + this.AgeMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x001D8D8C File Offset: 0x001D718C
		private float AgeMultiplier(Pawn pawn)
		{
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x001D8DC4 File Offset: 0x001D71C4
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x04002395 RID: 9109
		private SimpleCurve curve = null;
	}
}
