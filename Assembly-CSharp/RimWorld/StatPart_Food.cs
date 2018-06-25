using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A9 RID: 2473
	public class StatPart_Food : StatPart
	{
		// Token: 0x0400239E RID: 9118
		public float factorStarving = 1f;

		// Token: 0x0400239F RID: 9119
		public float factorUrgentlyHungry = 1f;

		// Token: 0x040023A0 RID: 9120
		public float factorHungry = 1f;

		// Token: 0x040023A1 RID: 9121
		public float factorFed = 1f;

		// Token: 0x0600376F RID: 14191 RVA: 0x001D972C File Offset: 0x001D7B2C
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

		// Token: 0x06003770 RID: 14192 RVA: 0x001D9788 File Offset: 0x001D7B88
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

		// Token: 0x06003771 RID: 14193 RVA: 0x001D9810 File Offset: 0x001D7C10
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
