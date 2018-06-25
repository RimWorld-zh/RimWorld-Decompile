using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000312 RID: 786
	public class GameCondition_ToxicFallout : GameCondition
	{
		// Token: 0x0400087F RID: 2175
		private const int LerpTicks = 5000;

		// Token: 0x04000880 RID: 2176
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x04000881 RID: 2177
		private const float SkyGlow = 0.85f;

		// Token: 0x04000882 RID: 2178
		private SkyColorSet ToxicFalloutColors;

		// Token: 0x04000883 RID: 2179
		private List<SkyOverlay> overlays;

		// Token: 0x04000884 RID: 2180
		private const int CheckInterval = 3451;

		// Token: 0x04000885 RID: 2181
		private const float ToxicPerDay = 0.5f;

		// Token: 0x04000886 RID: 2182
		private const float PlantKillChance = 0.0065f;

		// Token: 0x04000887 RID: 2183
		private const float CorpseRotProgressAdd = 3000f;

		// Token: 0x06000D49 RID: 3401 RVA: 0x00072F04 File Offset: 0x00071304
		public GameCondition_ToxicFallout()
		{
			ColorInt colorInt = new ColorInt(216, 255, 0);
			Color toColor = colorInt.ToColor;
			ColorInt colorInt2 = new ColorInt(234, 200, 255);
			this.ToxicFalloutColors = new SkyColorSet(toColor, colorInt2.ToColor, new Color(0.6f, 0.8f, 0.5f), 0.85f);
			this.overlays = new List<SkyOverlay>
			{
				new WeatherOverlay_Fallout()
			};
			base..ctor();
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00072F89 File Offset: 0x00071389
		public override void Init()
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Critical);
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00072FA4 File Offset: 0x000713A4
		public override void GameConditionTick()
		{
			List<Map> affectedMaps = base.AffectedMaps;
			if (Find.TickManager.TicksGame % 3451 == 0)
			{
				for (int i = 0; i < affectedMaps.Count; i++)
				{
					this.DoPawnsToxicDamage(affectedMaps[i]);
				}
			}
			for (int j = 0; j < this.overlays.Count; j++)
			{
				for (int k = 0; k < affectedMaps.Count; k++)
				{
					this.overlays[j].TickOverlay(affectedMaps[k]);
				}
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00073048 File Offset: 0x00071448
		private void DoPawnsToxicDamage(Map map)
		{
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if (!pawn.Position.Roofed(map) && pawn.def.race.IsFlesh)
				{
					float num = 0.028758334f;
					num *= pawn.GetStatValue(StatDefOf.ToxicSensitivity, true);
					if (num != 0f)
					{
						float num2 = Mathf.Lerp(0.85f, 1.15f, Rand.ValueSeeded(pawn.thingIDNumber ^ 74374237));
						num *= num2;
						HealthUtility.AdjustSeverity(pawn, HediffDefOf.ToxicBuildup, num);
					}
				}
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00073100 File Offset: 0x00071500
		public override void DoCellSteadyEffects(IntVec3 c, Map map)
		{
			if (!c.Roofed(map))
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is Plant)
					{
						if (Rand.Value < 0.0065f)
						{
							thing.Kill(null, null);
						}
					}
					else if (thing.def.category == ThingCategory.Item)
					{
						CompRottable compRottable = thing.TryGetComp<CompRottable>();
						if (compRottable != null)
						{
							if (compRottable.Stage < RotStage.Dessicated)
							{
								compRottable.RotProgress += 3000f;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x000731BC File Offset: 0x000715BC
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x000731FC File Offset: 0x000715FC
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, 5000f, 0.5f);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00073224 File Offset: 0x00071624
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.85f, this.ToxicFalloutColors, 1f, 1f));
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00073258 File Offset: 0x00071658
		public override float AnimalDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00073274 File Offset: 0x00071674
		public override float PlantDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00073290 File Offset: 0x00071690
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000732A8 File Offset: 0x000716A8
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}
	}
}
