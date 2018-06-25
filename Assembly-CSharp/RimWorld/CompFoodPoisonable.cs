using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000711 RID: 1809
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x040015DB RID: 5595
		private float poisonPct;

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060027B8 RID: 10168 RVA: 0x00154500 File Offset: 0x00152900
		// (set) Token: 0x060027B9 RID: 10169 RVA: 0x0015451B File Offset: 0x0015291B
		public float PoisonPercent
		{
			get
			{
				return this.poisonPct;
			}
			set
			{
				this.poisonPct = Mathf.Clamp01(value);
			}
		}

		// Token: 0x060027BA RID: 10170 RVA: 0x0015452A File Offset: 0x0015292A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
		}

		// Token: 0x060027BB RID: 10171 RVA: 0x0015454C File Offset: 0x0015294C
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
		}

		// Token: 0x060027BC RID: 10172 RVA: 0x00154574 File Offset: 0x00152974
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompFoodPoisonable compFoodPoisonable = otherStack.TryGetComp<CompFoodPoisonable>();
			this.poisonPct = GenMath.WeightedAverage(this.poisonPct, (float)this.parent.stackCount, compFoodPoisonable.poisonPct, (float)count);
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x001545B6 File Offset: 0x001529B6
		public override void PostIngested(Pawn ingester)
		{
			if (Rand.Chance(this.poisonPct * Find.Storyteller.difficulty.foodPoisonChanceFactor))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent);
			}
		}
	}
}
