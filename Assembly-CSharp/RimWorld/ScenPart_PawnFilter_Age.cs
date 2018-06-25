using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000642 RID: 1602
	public class ScenPart_PawnFilter_Age : ScenPart
	{
		// Token: 0x040012ED RID: 4845
		public IntRange allowedAgeRange = new IntRange(0, 999999);

		// Token: 0x040012EE RID: 4846
		private const int RangeMin = 15;

		// Token: 0x040012EF RID: 4847
		private const int RangeMax = 120;

		// Token: 0x040012F0 RID: 4848
		private const int RangeMinMax = 19;

		// Token: 0x040012F1 RID: 4849
		private const int RangeMinWidth = 4;

		// Token: 0x06002136 RID: 8502 RVA: 0x0011A704 File Offset: 0x00118B04
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, 31f);
			Widgets.IntRange(scenPartRect, (int)listing.CurHeight, ref this.allowedAgeRange, 15, 120, null, 4);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0011A738 File Offset: 0x00118B38
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.allowedAgeRange, "allowedAgeRange", default(IntRange), false);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x0011A768 File Offset: 0x00118B68
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

		// Token: 0x06002139 RID: 8505 RVA: 0x0011A848 File Offset: 0x00118C48
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return this.allowedAgeRange.Includes(pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x0011A874 File Offset: 0x00118C74
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

		// Token: 0x0600213B RID: 8507 RVA: 0x0011A918 File Offset: 0x00118D18
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
	}
}
