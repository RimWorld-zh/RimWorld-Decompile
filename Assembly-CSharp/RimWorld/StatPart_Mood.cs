using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AE RID: 2478
	public class StatPart_Mood : StatPart
	{
		// Token: 0x040023A7 RID: 9127
		private SimpleCurve factorFromMoodCurve = null;

		// Token: 0x0600378E RID: 14222 RVA: 0x001D9E70 File Offset: 0x001D8270
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromMoodCurve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x001D9E9C File Offset: 0x001D829C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val *= this.FactorFromMood(pawn);
				}
			}
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x001D9EE8 File Offset: 0x001D82E8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_MoodMultiplier".Translate(new object[]
					{
						pawn.needs.mood.CurLevel.ToStringPercent()
					}) + ": x" + this.FactorFromMood(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x001D9F70 File Offset: 0x001D8370
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.needs.mood != null;
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x001D9F98 File Offset: 0x001D8398
		private float FactorFromMood(Pawn pawn)
		{
			return this.factorFromMoodCurve.Evaluate(pawn.needs.mood.CurLevel);
		}
	}
}
