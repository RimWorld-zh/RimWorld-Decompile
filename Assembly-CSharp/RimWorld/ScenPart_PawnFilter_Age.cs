using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000644 RID: 1604
	public class ScenPart_PawnFilter_Age : ScenPart
	{
		// Token: 0x0600213B RID: 8507 RVA: 0x0011A24C File Offset: 0x0011864C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, 31f);
			Widgets.IntRange(scenPartRect, (int)listing.CurHeight, ref this.allowedAgeRange, 15, 120, null, 4);
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x0011A280 File Offset: 0x00118680
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.allowedAgeRange, "allowedAgeRange", default(IntRange), false);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0011A2B0 File Offset: 0x001186B0
		public override string Summary(Scenario scen)
		{
			string result;
			if (this.allowedAgeRange.min > 15)
			{
				if (this.allowedAgeRange.max < 10000)
				{
					result = "ScenPart_StartingPawnAgeRange".Translate(new object[]
					{
						this.allowedAgeRange.min,
						this.allowedAgeRange.max
					});
				}
				else
				{
					result = "ScenPart_StartingPawnAgeMin".Translate(new object[]
					{
						this.allowedAgeRange.min
					});
				}
			}
			else
			{
				if (this.allowedAgeRange.max >= 10000)
				{
					throw new Exception();
				}
				result = "ScenPart_StartingPawnAgeMax".Translate(new object[]
				{
					this.allowedAgeRange.max
				});
			}
			return result;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0011A390 File Offset: 0x00118790
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return this.allowedAgeRange.Includes(pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x0011A3BC File Offset: 0x001187BC
		public override void Randomize()
		{
			this.allowedAgeRange = new IntRange(15, 120);
			int num = Rand.RangeInclusive(0, 2);
			if (num != 0)
			{
				if (num != 1)
				{
					if (num == 2)
					{
						this.allowedAgeRange.min = Rand.Range(20, 60);
						this.allowedAgeRange.max = Rand.Range(20, 60);
					}
				}
				else
				{
					this.allowedAgeRange.max = Rand.Range(20, 60);
				}
			}
			else
			{
				this.allowedAgeRange.min = Rand.Range(20, 60);
			}
			this.MakeAllowedAgeRangeValid();
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0011A460 File Offset: 0x00118860
		private void MakeAllowedAgeRangeValid()
		{
			if (this.allowedAgeRange.max < 19)
			{
				this.allowedAgeRange.max = 19;
			}
			if (this.allowedAgeRange.max - this.allowedAgeRange.min < 4)
			{
				this.allowedAgeRange.min = this.allowedAgeRange.max - 4;
			}
		}

		// Token: 0x040012EC RID: 4844
		public IntRange allowedAgeRange = new IntRange(0, 999999);

		// Token: 0x040012ED RID: 4845
		private const int RangeMin = 15;

		// Token: 0x040012EE RID: 4846
		private const int RangeMax = 120;

		// Token: 0x040012EF RID: 4847
		private const int RangeMinMax = 19;

		// Token: 0x040012F0 RID: 4848
		private const int RangeMinWidth = 4;
	}
}
