using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B7 RID: 2487
	public class StatPart_Rest : StatPart
	{
		// Token: 0x060037AC RID: 14252 RVA: 0x001DA424 File Offset: 0x001D8824
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

		// Token: 0x060037AD RID: 14253 RVA: 0x001DA480 File Offset: 0x001D8880
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

		// Token: 0x060037AE RID: 14254 RVA: 0x001DA508 File Offset: 0x001D8908
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

		// Token: 0x040023BA RID: 9146
		private float factorExhausted = 1f;

		// Token: 0x040023BB RID: 9147
		private float factorVeryTired = 1f;

		// Token: 0x040023BC RID: 9148
		private float factorTired = 1f;

		// Token: 0x040023BD RID: 9149
		private float factorRested = 1f;
	}
}
