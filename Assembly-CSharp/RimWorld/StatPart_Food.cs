using System;
using Verse;

namespace RimWorld
{
	public class StatPart_Food : StatPart
	{
		private float factorStarving = 1f;

		private float factorUrgentlyHungry = 1f;

		private float factorHungry = 1f;

		private float factorFed = 1f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					val *= this.FoodMultiplier(pawn.needs.food.CurCategory);
				}
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			string result;
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					result = pawn.needs.food.CurCategory.GetLabel() + ": x" + this.FoodMultiplier(pawn.needs.food.CurCategory).ToStringPercent();
					goto IL_007a;
				}
			}
			result = (string)null;
			goto IL_007a;
			IL_007a:
			return result;
		}

		private float FoodMultiplier(HungerCategory hunger)
		{
			float result;
			switch (hunger)
			{
			case HungerCategory.Starving:
			{
				result = this.factorStarving;
				break;
			}
			case HungerCategory.UrgentlyHungry:
			{
				result = this.factorUrgentlyHungry;
				break;
			}
			case HungerCategory.Hungry:
			{
				result = this.factorHungry;
				break;
			}
			case HungerCategory.Fed:
			{
				result = this.factorFed;
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
