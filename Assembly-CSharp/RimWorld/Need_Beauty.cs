using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Need_Beauty : Need_Seeker
	{
		private const float BeautyImpactFactor = 0.1f;

		private const float ThreshVeryUgly = 0.01f;

		private const float ThreshUgly = 0.15f;

		private const float ThreshNeutral = 0.35f;

		private const float ThreshPretty = 0.65f;

		private const float ThreshVeryPretty = 0.85f;

		private const float ThreshBeautiful = 0.99f;

		public override float CurInstantLevel
		{
			get
			{
				if (!base.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
				{
					return 0.5f;
				}
				if (!base.pawn.Spawned)
				{
					return 0.5f;
				}
				return this.LevelFromBeauty(this.CurrentInstantBeauty());
			}
		}

		public BeautyCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.99000000953674316)
				{
					return BeautyCategory.Beautiful;
				}
				if (this.CurLevel > 0.85000002384185791)
				{
					return BeautyCategory.VeryPretty;
				}
				if (this.CurLevel > 0.64999997615814209)
				{
					return BeautyCategory.Pretty;
				}
				if (this.CurLevel > 0.34999999403953552)
				{
					return BeautyCategory.Neutral;
				}
				if (this.CurLevel > 0.15000000596046448)
				{
					return BeautyCategory.Ugly;
				}
				if (this.CurLevel > 0.0099999997764825821)
				{
					return BeautyCategory.VeryUgly;
				}
				return BeautyCategory.Hideous;
			}
		}

		public Need_Beauty(Pawn pawn) : base(pawn)
		{
			base.threshPercents = new List<float>();
			base.threshPercents.Add(0.15f);
			base.threshPercents.Add(0.35f);
			base.threshPercents.Add(0.65f);
			base.threshPercents.Add(0.85f);
		}

		private float LevelFromBeauty(float beauty)
		{
			return Mathf.Clamp01((float)(base.def.baseLevel + beauty * 0.10000000149011612));
		}

		public float CurrentInstantBeauty()
		{
			if (!base.pawn.SpawnedOrAnyParentSpawned)
			{
				return 0.5f;
			}
			return BeautyUtility.AverageBeautyPerceptible(base.pawn.PositionHeld, base.pawn.MapHeld);
		}
	}
}
