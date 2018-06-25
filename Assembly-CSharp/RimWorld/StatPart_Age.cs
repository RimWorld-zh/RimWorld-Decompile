using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A2 RID: 2466
	public class StatPart_Age : StatPart
	{
		// Token: 0x0400239D RID: 9117
		private SimpleCurve curve = null;

		// Token: 0x06003750 RID: 14160 RVA: 0x001D90D4 File Offset: 0x001D74D4
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

		// Token: 0x06003751 RID: 14161 RVA: 0x001D911C File Offset: 0x001D751C
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

		// Token: 0x06003752 RID: 14162 RVA: 0x001D91A0 File Offset: 0x001D75A0
		private float AgeMultiplier(Pawn pawn)
		{
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x001D91D8 File Offset: 0x001D75D8
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}
	}
}
