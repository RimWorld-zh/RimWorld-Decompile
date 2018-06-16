using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000713 RID: 1811
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060027BA RID: 10170 RVA: 0x00154190 File Offset: 0x00152590
		// (set) Token: 0x060027BB RID: 10171 RVA: 0x001541AB File Offset: 0x001525AB
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

		// Token: 0x060027BC RID: 10172 RVA: 0x001541BA File Offset: 0x001525BA
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
		}

		// Token: 0x060027BD RID: 10173 RVA: 0x001541DC File Offset: 0x001525DC
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x00154204 File Offset: 0x00152604
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompFoodPoisonable compFoodPoisonable = otherStack.TryGetComp<CompFoodPoisonable>();
			this.poisonPct = GenMath.WeightedAverage(this.poisonPct, (float)this.parent.stackCount, compFoodPoisonable.poisonPct, (float)count);
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x00154246 File Offset: 0x00152646
		public override void PostIngested(Pawn ingester)
		{
			if (Rand.Value < this.poisonPct)
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent);
			}
		}

		// Token: 0x040015DD RID: 5597
		private float poisonPct;
	}
}
