using System;
using Verse;

namespace RimWorld
{
	public class StatPart_Food : StatPart
	{
		public float factorStarving = 1f;

		public float factorUrgentlyHungry = 1f;

		public float factorHungry = 1f;

		public float factorFed = 1f;

		public StatPart_Food()
		{
		}

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
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					return pawn.needs.food.CurCategory.GetLabel() + ": x" + this.FoodMultiplier(pawn.needs.food.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		private float FoodMultiplier(HungerCategory hunger)
		{
			float result;
			switch (hunger)
			{
			case HungerCategory.Fed:
				result = this.factorFed;
				break;
			case HungerCategory.Hungry:
				result = this.factorHungry;
				break;
			case HungerCategory.UrgentlyHungry:
				result = this.factorUrgentlyHungry;
				break;
			case HungerCategory.Starving:
				result = this.factorStarving;
				break;
			default:
				throw new InvalidOperationException();
			}
			return result;
		}
	}
}
