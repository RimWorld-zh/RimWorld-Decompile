using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004F2 RID: 1266
	public class Need_Beauty : Need_Seeker
	{
		// Token: 0x04000D49 RID: 3401
		private const float BeautyImpactFactor = 0.1f;

		// Token: 0x04000D4A RID: 3402
		private const float ThreshVeryUgly = 0.01f;

		// Token: 0x04000D4B RID: 3403
		private const float ThreshUgly = 0.15f;

		// Token: 0x04000D4C RID: 3404
		private const float ThreshNeutral = 0.35f;

		// Token: 0x04000D4D RID: 3405
		private const float ThreshPretty = 0.65f;

		// Token: 0x04000D4E RID: 3406
		private const float ThreshVeryPretty = 0.85f;

		// Token: 0x04000D4F RID: 3407
		private const float ThreshBeautiful = 0.99f;

		// Token: 0x060016C6 RID: 5830 RVA: 0x000C9B40 File Offset: 0x000C7F40
		public Need_Beauty(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.35f);
			this.threshPercents.Add(0.65f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060016C7 RID: 5831 RVA: 0x000C9BA0 File Offset: 0x000C7FA0
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

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x000C9C08 File Offset: 0x000C8008
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

		// Token: 0x060016C9 RID: 5833 RVA: 0x000C9CA8 File Offset: 0x000C80A8
		private float LevelFromBeauty(float beauty)
		{
			return Mathf.Clamp01(this.def.baseLevel + beauty * 0.1f);
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x000C9CD8 File Offset: 0x000C80D8
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
