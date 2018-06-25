using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B0 RID: 2480
	public class StatPart_Mood : StatPart
	{
		// Token: 0x040023AF RID: 9135
		private SimpleCurve factorFromMoodCurve = null;

		// Token: 0x06003792 RID: 14226 RVA: 0x001DA284 File Offset: 0x001D8684
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromMoodCurve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x001DA2B0 File Offset: 0x001D86B0
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

		// Token: 0x06003794 RID: 14228 RVA: 0x001DA2FC File Offset: 0x001D86FC
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

		// Token: 0x06003795 RID: 14229 RVA: 0x001DA384 File Offset: 0x001D8784
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.needs.mood != null;
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x001DA3AC File Offset: 0x001D87AC
		private float FactorFromMood(Pawn pawn)
		{
			return this.factorFromMoodCurve.Evaluate(pawn.needs.mood.CurLevel);
		}
	}
}
