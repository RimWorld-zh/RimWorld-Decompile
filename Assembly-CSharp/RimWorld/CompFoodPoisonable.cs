using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070F RID: 1807
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x040015DB RID: 5595
		private float poisonPct;

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060027B4 RID: 10164 RVA: 0x001543B0 File Offset: 0x001527B0
		// (set) Token: 0x060027B5 RID: 10165 RVA: 0x001543CB File Offset: 0x001527CB
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

		// Token: 0x060027B6 RID: 10166 RVA: 0x001543DA File Offset: 0x001527DA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
		}

		// Token: 0x060027B7 RID: 10167 RVA: 0x001543FC File Offset: 0x001527FC
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
		}

		// Token: 0x060027B8 RID: 10168 RVA: 0x00154424 File Offset: 0x00152824
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompFoodPoisonable compFoodPoisonable = otherStack.TryGetComp<CompFoodPoisonable>();
			this.poisonPct = GenMath.WeightedAverage(this.poisonPct, (float)this.parent.stackCount, compFoodPoisonable.poisonPct, (float)count);
		}

		// Token: 0x060027B9 RID: 10169 RVA: 0x00154466 File Offset: 0x00152866
		public override void PostIngested(Pawn ingester)
		{
			if (Rand.Chance(this.poisonPct * Find.Storyteller.difficulty.foodPoisonChanceFactor))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent);
			}
		}
	}
}
