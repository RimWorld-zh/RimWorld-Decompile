using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A9 RID: 2473
	public class StatPart_Food : StatPart
	{
		// Token: 0x040023A5 RID: 9125
		public float factorStarving = 1f;

		// Token: 0x040023A6 RID: 9126
		public float factorUrgentlyHungry = 1f;

		// Token: 0x040023A7 RID: 9127
		public float factorHungry = 1f;

		// Token: 0x040023A8 RID: 9128
		public float factorFed = 1f;

		// Token: 0x0600376F RID: 14191 RVA: 0x001D9A00 File Offset: 0x001D7E00
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

		// Token: 0x06003770 RID: 14192 RVA: 0x001D9A5C File Offset: 0x001D7E5C
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

		// Token: 0x06003771 RID: 14193 RVA: 0x001D9AE4 File Offset: 0x001D7EE4
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
