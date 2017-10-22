using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_PawnFilter_Age : ScenPart
	{
		public IntRange allowedAgeRange = new IntRange(0, 999999);

		private const int RangeMin = 15;

		private const int RangeMax = 120;

		private const int RangeMinMax = 19;

		private const int RangeMinWidth = 4;

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, 31f);
			Widgets.IntRange(scenPartRect, (int)listing.CurHeight, ref this.allowedAgeRange, 15, 120, (string)null, 4);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.allowedAgeRange, "allowedAgeRange", default(IntRange), false);
		}

		public override string Summary(Scenario scen)
		{
			string result;
			if (this.allowedAgeRange.min > 15)
			{
				result = ((this.allowedAgeRange.max >= 10000) ? "ScenPart_StartingPawnAgeMin".Translate(this.allowedAgeRange.min) : "ScenPart_StartingPawnAgeRange".Translate(this.allowedAgeRange.min, this.allowedAgeRange.max));
				goto IL_00d2;
			}
			if (this.allowedAgeRange.max < 10000)
			{
				result = "ScenPart_StartingPawnAgeMax".Translate(this.allowedAgeRange.max);
				goto IL_00d2;
			}
			throw new Exception();
			IL_00d2:
			return result;
		}

		public override bool AllowPlayerStartingPawn(Pawn pawn)
		{
			return this.allowedAgeRange.Includes(pawn.ageTracker.AgeBiologicalYears);
		}

		public override void Randomize()
		{
			this.allowedAgeRange = new IntRange(15, 120);
			switch (Rand.RangeInclusive(0, 2))
			{
			case 0:
			{
				this.allowedAgeRange.min = Rand.Range(20, 60);
				break;
			}
			case 1:
			{
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			}
			case 2:
			{
				this.allowedAgeRange.min = Rand.Range(20, 60);
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			}
			}
			this.MakeAllowedAgeRangeValid();
		}

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
