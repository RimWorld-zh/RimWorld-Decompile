using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A4 RID: 2468
	public class StatPart_Age : StatPart
	{
		// Token: 0x06003751 RID: 14161 RVA: 0x001D89F0 File Offset: 0x001D6DF0
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

		// Token: 0x06003752 RID: 14162 RVA: 0x001D8A38 File Offset: 0x001D6E38
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

		// Token: 0x06003753 RID: 14163 RVA: 0x001D8ABC File Offset: 0x001D6EBC
		private float AgeMultiplier(Pawn pawn)
		{
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x001D8AF4 File Offset: 0x001D6EF4
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x04002397 RID: 9111
		private SimpleCurve curve = null;
	}
}
