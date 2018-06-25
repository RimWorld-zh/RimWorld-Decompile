using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000312 RID: 786
	public class GameCondition_ToxicFallout : GameCondition
	{
		// Token: 0x04000882 RID: 2178
		private const int LerpTicks = 5000;

		// Token: 0x04000883 RID: 2179
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x04000884 RID: 2180
		private const float SkyGlow = 0.85f;

		// Token: 0x04000885 RID: 2181
		private SkyColorSet ToxicFalloutColors;

		// Token: 0x04000886 RID: 2182
		private List<SkyOverlay> overlays;

		// Token: 0x04000887 RID: 2183
		private const int CheckInterval = 3451;

		// Token: 0x04000888 RID: 2184
		private const float ToxicPerDay = 0.5f;

		// Token: 0x04000889 RID: 2185
		private const float PlantKillChance = 0.0065f;

		// Token: 0x0400088A RID: 2186
		private const float CorpseRotProgressAdd = 3000f;

		// Token: 0x06000D48 RID: 3400 RVA: 0x00072F0C File Offset: 0x0007130C
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

		// Token: 0x06000D49 RID: 3401 RVA: 0x00072F91 File Offset: 0x00071391
		public override void Init()
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Critical);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00072FAC File Offset: 0x000713AC
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

		// Token: 0x06000D4B RID: 3403 RVA: 0x00073050 File Offset: 0x00071450
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

		// Token: 0x06000D4C RID: 3404 RVA: 0x00073108 File Offset: 0x00071508
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

		// Token: 0x06000D4D RID: 3405 RVA: 0x000731C4 File Offset: 0x000715C4
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00073204 File Offset: 0x00071604
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, 5000f, 0.5f);
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0007322C File Offset: 0x0007162C
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.85f, this.ToxicFalloutColors, 1f, 1f));
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00073260 File Offset: 0x00071660
		public override float AnimalDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x0007327C File Offset: 0x0007167C
		public override float PlantDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00073298 File Offset: 0x00071698
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x000732B0 File Offset: 0x000716B0
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}
	}
}
