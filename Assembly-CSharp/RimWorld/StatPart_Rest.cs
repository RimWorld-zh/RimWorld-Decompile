using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B3 RID: 2483
	public class StatPart_Rest : StatPart
	{
		// Token: 0x040023B4 RID: 9140
		private float factorExhausted = 1f;

		// Token: 0x040023B5 RID: 9141
		private float factorVeryTired = 1f;

		// Token: 0x040023B6 RID: 9142
		private float factorTired = 1f;

		// Token: 0x040023B7 RID: 9143
		private float factorRested = 1f;

		// Token: 0x060037A7 RID: 14247 RVA: 0x001DA6BC File Offset: 0x001D8ABC
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					val *= this.RestMultiplier(pawn.needs.rest.CurCategory);
				}
			}
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x001DA718 File Offset: 0x001D8B18
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					return pawn.needs.rest.CurCategory.GetLabel() + ": x" + this.RestMultiplier(pawn.needs.rest.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x001DA7A0 File Offset: 0x001D8BA0
		private float RestMultiplier(RestCategory fatigue)
		{
			float result;
			switch (fatigue)
			{
			case RestCategory.Rested:
				result = this.factorRested;
				break;
			case RestCategory.Tired:
				result = this.factorTired;
				break;
			case RestCategory.VeryTired:
				result = this.factorVeryTired;
				break;
			case RestCategory.Exhausted:
				result = this.factorExhausted;
				break;
			default:
				throw new InvalidOperationException();
			}
			return result;
		}
	}
}
