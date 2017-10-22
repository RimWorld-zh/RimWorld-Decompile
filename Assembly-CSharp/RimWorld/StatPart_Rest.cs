using System;
using Verse;

namespace RimWorld
{
	public class StatPart_Rest : StatPart
	{
		private float factorExhausted = 1f;

		private float factorVeryTired = 1f;

		private float factorTired = 1f;

		private float factorRested = 1f;

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

		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.rest != null)
				{
					result = pawn.needs.rest.CurCategory.GetLabel() + ": x" + this.RestMultiplier(pawn.needs.rest.CurCategory).ToStringPercent();
					goto IL_007a;
				}
			}
			result = (string)null;
			goto IL_007a;
			IL_007a:
			return result;
		}

		private float RestMultiplier(RestCategory fatigue)
		{
			float result;
			switch (fatigue)
			{
			case RestCategory.Exhausted:
			{
				result = this.factorExhausted;
				break;
			}
			case RestCategory.VeryTired:
			{
				result = this.factorVeryTired;
				break;
			}
			case RestCategory.Tired:
			{
				result = this.factorTired;
				break;
			}
			case RestCategory.Rested:
			{
				result = this.factorRested;
				break;
			}
			default:
			{
				throw new InvalidOperationException();
			}
			}
			return result;
		}
	}
}
