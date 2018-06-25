using System;
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

		public Need_Beauty(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.35f);
			this.threshPercents.Add(0.65f);
			this.threshPercents.Add(0.85f);
		}

		public override float CurInstantLevel
		{
			get
			{
				float result;
				if (!this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
				{
					result = 0.5f;
				}
				else if (!this.pawn.Spawned)
				{
					result = 0.5f;
				}
				else
				{
					result = this.LevelFromBeauty(this.CurrentInstantBeauty());
				}
				return result;
			}
		}

		public BeautyCategory CurCategory
		{
			get
			{
				BeautyCategory result;
				if (this.CurLevel > 0.99f)
				{
					result = BeautyCategory.Beautiful;
				}
				else if (this.CurLevel > 0.85f)
				{
					result = BeautyCategory.VeryPretty;
				}
				else if (this.CurLevel > 0.65f)
				{
					result = BeautyCategory.Pretty;
				}
				else if (this.CurLevel > 0.35f)
				{
					result = BeautyCategory.Neutral;
				}
				else if (this.CurLevel > 0.15f)
				{
					result = BeautyCategory.Ugly;
				}
				else if (this.CurLevel > 0.01f)
				{
					result = BeautyCategory.VeryUgly;
				}
				else
				{
					result = BeautyCategory.Hideous;
				}
				return result;
			}
		}

		private float LevelFromBeauty(float beauty)
		{
			return Mathf.Clamp01(this.def.baseLevel + beauty * 0.1f);
		}

		public float CurrentInstantBeauty()
		{
			float result;
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				result = 0.5f;
			}
			else
			{
				result = BeautyUtility.AverageBeautyPerceptible(this.pawn.PositionHeld, this.pawn.MapHeld);
			}
			return result;
		}
	}
}
