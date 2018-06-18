using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009AB RID: 2475
	public class StatPart_Food : StatPart
	{
		// Token: 0x06003772 RID: 14194 RVA: 0x001D9428 File Offset: 0x001D7828
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

		// Token: 0x06003773 RID: 14195 RVA: 0x001D9484 File Offset: 0x001D7884
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

		// Token: 0x06003774 RID: 14196 RVA: 0x001D950C File Offset: 0x001D790C
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

		// Token: 0x040023A3 RID: 9123
		public float factorStarving = 1f;

		// Token: 0x040023A4 RID: 9124
		public float factorUrgentlyHungry = 1f;

		// Token: 0x040023A5 RID: 9125
		public float factorHungry = 1f;

		// Token: 0x040023A6 RID: 9126
		public float factorFed = 1f;
	}
}
