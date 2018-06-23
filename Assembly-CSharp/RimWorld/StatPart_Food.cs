using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A7 RID: 2471
	public class StatPart_Food : StatPart
	{
		// Token: 0x0400239D RID: 9117
		public float factorStarving = 1f;

		// Token: 0x0400239E RID: 9118
		public float factorUrgentlyHungry = 1f;

		// Token: 0x0400239F RID: 9119
		public float factorHungry = 1f;

		// Token: 0x040023A0 RID: 9120
		public float factorFed = 1f;

		// Token: 0x0600376B RID: 14187 RVA: 0x001D95EC File Offset: 0x001D79EC
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

		// Token: 0x0600376C RID: 14188 RVA: 0x001D9648 File Offset: 0x001D7A48
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

		// Token: 0x0600376D RID: 14189 RVA: 0x001D96D0 File Offset: 0x001D7AD0
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
