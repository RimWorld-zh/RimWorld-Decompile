using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000713 RID: 1811
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x060027BC RID: 10172 RVA: 0x00154208 File Offset: 0x00152608
		// (set) Token: 0x060027BD RID: 10173 RVA: 0x00154223 File Offset: 0x00152623
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

		// Token: 0x060027BE RID: 10174 RVA: 0x00154232 File Offset: 0x00152632
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x00154254 File Offset: 0x00152654
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x0015427C File Offset: 0x0015267C
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompFoodPoisonable compFoodPoisonable = otherStack.TryGetComp<CompFoodPoisonable>();
			this.poisonPct = GenMath.WeightedAverage(this.poisonPct, (float)this.parent.stackCount, compFoodPoisonable.poisonPct, (float)count);
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x001542BE File Offset: 0x001526BE
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
