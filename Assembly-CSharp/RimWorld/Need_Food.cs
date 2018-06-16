using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004FC RID: 1276
	public class Need_Food : Need
	{
		// Token: 0x060016E3 RID: 5859 RVA: 0x000CA358 File Offset: 0x000C8758
		public Need_Food(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x000CA370 File Offset: 0x000C8770
		public bool Starving
		{
			get
			{
				return this.CurCategory == HungerCategory.Starving;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x000CA390 File Offset: 0x000C8790
		public float PercentageThreshUrgentlyHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.4f;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x000CA3BC File Offset: 0x000C87BC
		public float PercentageThreshHungry
		{
			get
			{
				return this.pawn.RaceProps.FoodLevelPercentageWantEat * 0.8f;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060016E7 RID: 5863 RVA: 0x000CA3E8 File Offset: 0x000C87E8
		public float NutritionBetweenHungryAndFed
		{
			get
			{
				return (1f - this.PercentageThreshHungry) * this.MaxLevel;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x000CA410 File Offset: 0x000C8810
		public HungerCategory CurCategory
		{
			get
			{
				HungerCategory result;
				if (base.CurLevelPercentage <= 0f)
				{
					result = HungerCategory.Starving;
				}
				else if (base.CurLevelPercentage < this.PercentageThreshUrgentlyHungry)
				{
					result = HungerCategory.UrgentlyHungry;
				}
				else if (base.CurLevelPercentage < this.PercentageThreshHungry)
				{
					result = HungerCategory.Hungry;
				}
				else
				{
					result = HungerCategory.Fed;
				}
				return result;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060016E9 RID: 5865 RVA: 0x000CA470 File Offset: 0x000C8870
		public float FoodFallPerTick
		{
			get
			{
				return this.FoodFallPerTickAssumingCategory(this.CurCategory);
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060016EA RID: 5866 RVA: 0x000CA494 File Offset: 0x000C8894
		public int TicksUntilHungryWhenFed
		{
			get
			{
				return Mathf.CeilToInt(this.NutritionBetweenHungryAndFed / this.FoodFallPerTickAssumingCategory(HungerCategory.Fed));
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060016EB RID: 5867 RVA: 0x000CA4BC File Offset: 0x000C88BC
		public override int GUIChangeArrow
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060016EC RID: 5868 RVA: 0x000CA4D4 File Offset: 0x000C88D4
		public override float MaxLevel
		{
			get
			{
				return this.pawn.BodySize * this.pawn.ageTracker.CurLifeStage.foodMaxFactor;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x000CA50C File Offset: 0x000C890C
		public float NutritionWanted
		{
			get
			{
				return this.MaxLevel - this.CurLevel;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060016EE RID: 5870 RVA: 0x000CA530 File Offset: 0x000C8930
		private float HungerRate
		{
			get
			{
				return this.pawn.ageTracker.CurLifeStage.hungerRateFactor * this.pawn.RaceProps.baseHungerRate * this.pawn.health.hediffSet.HungerRateFactor * this.pawn.GetStatValue(StatDefOf.HungerRateMultiplier, true);
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060016EF RID: 5871 RVA: 0x000CA594 File Offset: 0x000C8994
		public int TicksStarving
		{
			get
			{
				return Mathf.Max(0, Find.TickManager.TicksGame - this.lastNonStarvingTick);
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x000CA5C0 File Offset: 0x000C89C0
		private float MalnutritionSeverityPerInterval
		{
			get
			{
				return 0.00113333331f * Mathf.Lerp(0.8f, 1.2f, Rand.ValueSeeded(this.pawn.thingIDNumber ^ 2551674));
			}
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x000CA600 File Offset: 0x000C8A00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastNonStarvingTick, "lastNonStarvingTick", -99999, false);
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x000CA620 File Offset: 0x000C8A20
		private float FoodFallPerTickAssumingCategory(HungerCategory cat)
		{
			float result;
			switch (cat)
			{
			case HungerCategory.Fed:
				result = 2.66666666E-05f * this.HungerRate;
				break;
			case HungerCategory.Hungry:
				result = 2.66666666E-05f * this.HungerRate * 0.5f;
				break;
			case HungerCategory.UrgentlyHungry:
				result = 2.66666666E-05f * this.HungerRate * 0.25f;
				break;
			case HungerCategory.Starving:
				result = 2.66666666E-05f * this.HungerRate * 0.15f;
				break;
			default:
				result = 999f;
				break;
			}
			return result;
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x000CA6B0 File Offset: 0x000C8AB0
		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				this.CurLevel -= this.FoodFallPerTick * 150f;
			}
			if (!this.Starving)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
			if (!base.IsFrozen)
			{
				if (this.Starving)
				{
					HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, this.MalnutritionSeverityPerInterval);
				}
				else
				{
					HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.Malnutrition, -this.MalnutritionSeverityPerInterval);
				}
			}
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x000CA748 File Offset: 0x000C8B48
		public override void SetInitialLevel()
		{
			if (this.pawn.RaceProps.Humanlike)
			{
				base.CurLevelPercentage = 0.8f;
			}
			else
			{
				base.CurLevelPercentage = Rand.Range(0.5f, 0.9f);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.lastNonStarvingTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x000CA7AC File Offset: 0x000C8BAC
		public override string GetTipString()
		{
			return string.Concat(new string[]
			{
				base.LabelCap,
				": ",
				base.CurLevelPercentage.ToStringPercent(),
				" (",
				this.CurLevel.ToString("0.##"),
				" / ",
				this.MaxLevel.ToString("0.##"),
				")\n",
				this.def.description
			});
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x000CA840 File Offset: 0x000C8C40
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true)
		{
			if (this.threshPercents == null)
			{
				this.threshPercents = new List<float>();
			}
			this.threshPercents.Clear();
			this.threshPercents.Add(this.PercentageThreshHungry);
			this.threshPercents.Add(this.PercentageThreshUrgentlyHungry);
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip);
		}

		// Token: 0x04000D71 RID: 3441
		private int lastNonStarvingTick = -99999;

		// Token: 0x04000D72 RID: 3442
		public const float BaseFoodFallPerTick = 2.66666666E-05f;

		// Token: 0x04000D73 RID: 3443
		private const float BaseMalnutritionSeverityPerDay = 0.17f;

		// Token: 0x04000D74 RID: 3444
		private const float BaseMalnutritionSeverityPerInterval = 0.00113333331f;
	}
}
