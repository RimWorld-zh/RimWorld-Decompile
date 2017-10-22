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
				return (float)(base.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) ? (base.pawn.Spawned ? this.LevelFromBeauty(this.CurrentInstantBeauty()) : 0.5) : 0.5);
			}
		}

		public BeautyCategory CurCategory
		{
			get
			{
				return (BeautyCategory)((!(this.CurLevel > 0.99000000953674316)) ? ((!(this.CurLevel > 0.85000002384185791)) ? ((!(this.CurLevel > 0.64999997615814209)) ? ((!(this.CurLevel > 0.34999999403953552)) ? ((!(this.CurLevel > 0.15000000596046448)) ? ((this.CurLevel > 0.0099999997764825821) ? 1 : 0) : 2) : 3) : 4) : 5) : 6);
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
			return (float)(base.pawn.SpawnedOrAnyParentSpawned ? BeautyUtility.AverageBeautyPerceptible(base.pawn.PositionHeld, base.pawn.MapHeld) : 0.5);
		}
	}
}
