using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000147 RID: 327
	public class WorkGiver_GrowerSow : WorkGiver_Grower
	{
		// Token: 0x0400032A RID: 810
		protected static string CantSowCavePlantBecauseOfLightTrans;

		// Token: 0x0400032B RID: 811
		protected static string CantSowCavePlantBecauseUnroofedTrans;

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x00045BAC File Offset: 0x00043FAC
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x00045BC2 File Offset: 0x00043FC2
		public static void ResetStaticData()
		{
			WorkGiver_GrowerSow.CantSowCavePlantBecauseOfLightTrans = "CantSowCavePlantBecauseOfLight".Translate();
			WorkGiver_GrowerSow.CantSowCavePlantBecauseUnroofedTrans = "CantSowCavePlantBecauseUnroofed".Translate();
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00045BE4 File Offset: 0x00043FE4
		protected override bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			bool result;
			if (!settable.CanAcceptSowNow())
			{
				result = false;
			}
			else
			{
				Zone_Growing zone_Growing = settable as Zone_Growing;
				IntVec3 c;
				if (zone_Growing != null)
				{
					if (!zone_Growing.allowSow)
					{
						return false;
					}
					c = zone_Growing.Cells[0];
				}
				else
				{
					c = ((Thing)settable).Position;
				}
				WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, pawn.Map);
				result = (WorkGiver_Grower.wantedPlantDef != null);
			}
			return result;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00045C70 File Offset: 0x00044070
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			Map map = pawn.Map;
			Job result;
			if (c.IsForbidden(pawn))
			{
				result = null;
			}
			else if (!GenPlant.GrowthSeasonNow(c, map, true))
			{
				result = null;
			}
			else
			{
				if (WorkGiver_Grower.wantedPlantDef == null)
				{
					WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, map);
					if (WorkGiver_Grower.wantedPlantDef == null)
					{
						return null;
					}
				}
				List<Thing> thingList = c.GetThingList(map);
				bool flag = false;
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def == WorkGiver_Grower.wantedPlantDef)
					{
						return null;
					}
					if ((thing is Blueprint || thing is Frame) && thing.Faction == pawn.Faction)
					{
						flag = true;
					}
				}
				if (flag)
				{
					Thing edifice = c.GetEdifice(map);
					if (edifice == null || edifice.def.fertility < 0f)
					{
						return null;
					}
				}
				if (WorkGiver_Grower.wantedPlantDef.plant.cavePlant)
				{
					if (!c.Roofed(map))
					{
						JobFailReason.Is(WorkGiver_GrowerSow.CantSowCavePlantBecauseUnroofedTrans, null);
						return null;
					}
					if (map.glowGrid.GameGlowAt(c, true) > 0f)
					{
						JobFailReason.Is(WorkGiver_GrowerSow.CantSowCavePlantBecauseOfLightTrans, null);
						return null;
					}
				}
				if (WorkGiver_Grower.wantedPlantDef.plant.interferesWithRoof && c.Roofed(pawn.Map))
				{
					result = null;
				}
				else
				{
					Plant plant = c.GetPlant(map);
					if (plant != null)
					{
						if (plant.def.plant.blockAdjacentSow)
						{
							LocalTargetInfo target = plant;
							if (!pawn.CanReserve(target, 1, -1, null, forced) || plant.IsForbidden(pawn))
							{
								return null;
							}
							return new Job(JobDefOf.CutPlant, plant);
						}
					}
					Thing thing2 = GenPlant.AdjacentSowBlocker(WorkGiver_Grower.wantedPlantDef, c, map);
					if (thing2 != null)
					{
						Plant plant2 = thing2 as Plant;
						if (plant2 != null)
						{
							LocalTargetInfo target = plant2;
							if (pawn.CanReserve(target, 1, -1, null, forced) && !plant2.IsForbidden(pawn))
							{
								IPlantToGrowSettable plantToGrowSettable = plant2.Position.GetPlantToGrowSettable(plant2.Map);
								if (plantToGrowSettable == null || plantToGrowSettable.GetPlantDefToGrow() != plant2.def)
								{
									return new Job(JobDefOf.CutPlant, plant2);
								}
							}
						}
						result = null;
					}
					else
					{
						if (WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill > 0)
						{
							if (pawn.skills != null && pawn.skills.GetSkill(SkillDefOf.Growing).Level < WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill)
							{
								return null;
							}
						}
						int j = 0;
						while (j < thingList.Count)
						{
							Thing thing3 = thingList[j];
							if (thing3.def.BlockPlanting)
							{
								LocalTargetInfo target = thing3;
								if (!pawn.CanReserve(target, 1, -1, null, forced))
								{
									return null;
								}
								if (thing3.def.category == ThingCategory.Plant)
								{
									if (!thing3.IsForbidden(pawn))
									{
										return new Job(JobDefOf.CutPlant, thing3);
									}
									return null;
								}
								else
								{
									if (thing3.def.EverHaulable)
									{
										return HaulAIUtility.HaulAsideJobFor(pawn, thing3);
									}
									return null;
								}
							}
							else
							{
								j++;
							}
						}
						if (WorkGiver_Grower.wantedPlantDef.CanEverPlantAt(c, map) && GenPlant.GrowthSeasonNow(c, map, true))
						{
							LocalTargetInfo target = c;
							if (pawn.CanReserve(target, 1, -1, null, forced))
							{
								return new Job(JobDefOf.Sow, c)
								{
									plantDefToSow = WorkGiver_Grower.wantedPlantDef
								};
							}
						}
						result = null;
					}
				}
			}
			return result;
		}
	}
}
