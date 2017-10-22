using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_GrowerSow : WorkGiver_Grower
	{
		protected static string CantSowCavePlantBecauseOfLightTrans;

		protected static string CantSowCavePlantBecauseUnroofedTrans;

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		public static void Reset()
		{
			WorkGiver_GrowerSow.CantSowCavePlantBecauseOfLightTrans = "CantSowCavePlantBecauseOfLight".Translate();
			WorkGiver_GrowerSow.CantSowCavePlantBecauseUnroofedTrans = "CantSowCavePlantBecauseUnroofed".Translate();
		}

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
						result = false;
						goto IL_007b;
					}
					c = zone_Growing.Cells[0];
				}
				else
				{
					c = ((Thing)settable).Position;
				}
				WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, pawn.Map);
				result = ((byte)((WorkGiver_Grower.wantedPlantDef != null) ? 1 : 0) != 0);
			}
			goto IL_007b;
			IL_007b:
			return result;
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c)
		{
			Map map = pawn.Map;
			Job result;
			if (c.IsForbidden(pawn))
			{
				result = null;
				goto IL_03cd;
			}
			if (!GenPlant.GrowthSeasonNow(c, map))
			{
				result = null;
				goto IL_03cd;
			}
			if (WorkGiver_Grower.wantedPlantDef == null)
			{
				WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, map);
				if (WorkGiver_Grower.wantedPlantDef == null)
				{
					result = null;
					goto IL_03cd;
				}
			}
			List<Thing> thingList = c.GetThingList(map);
			bool flag = false;
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def == WorkGiver_Grower.wantedPlantDef)
					goto IL_0085;
				if ((thing is Blueprint || thing is Frame) && thing.Faction == pawn.Faction)
				{
					flag = true;
				}
			}
			if (flag)
			{
				Thing edifice = c.GetEdifice(map);
				if (edifice != null && !(edifice.def.fertility < 0.0))
				{
					goto IL_0101;
				}
				result = null;
				goto IL_03cd;
			}
			goto IL_0101;
			IL_03cd:
			return result;
			IL_0085:
			result = null;
			goto IL_03cd;
			IL_0259:
			result = null;
			goto IL_03cd;
			IL_0101:
			if (WorkGiver_Grower.wantedPlantDef.plant.cavePlant)
			{
				if (!c.Roofed(map))
				{
					JobFailReason.Is(WorkGiver_GrowerSow.CantSowCavePlantBecauseUnroofedTrans);
					result = null;
					goto IL_03cd;
				}
				if (map.glowGrid.GameGlowAt(c, true) > 0.0)
				{
					JobFailReason.Is(WorkGiver_GrowerSow.CantSowCavePlantBecauseOfLightTrans);
					result = null;
					goto IL_03cd;
				}
			}
			Plant plant = c.GetPlant(map);
			Thing thing3;
			if (plant != null && plant.def.plant.blockAdjacentSow)
			{
				result = ((pawn.CanReserve((Thing)plant, 1, -1, null, false) && !plant.IsForbidden(pawn)) ? new Job(JobDefOf.CutPlant, (Thing)plant) : null);
			}
			else
			{
				Thing thing2 = GenPlant.AdjacentSowBlocker(WorkGiver_Grower.wantedPlantDef, c, map);
				if (thing2 != null)
				{
					Plant plant2 = thing2 as Plant;
					if (plant2 != null && pawn.CanReserve((Thing)plant2, 1, -1, null, false) && !plant2.IsForbidden(pawn))
					{
						IPlantToGrowSettable plantToGrowSettable = plant2.Position.GetPlantToGrowSettable(plant2.Map);
						if (plantToGrowSettable != null && plantToGrowSettable.GetPlantDefToGrow() == plant2.def)
						{
							goto IL_0259;
						}
						result = new Job(JobDefOf.CutPlant, (Thing)plant2);
						goto IL_03cd;
					}
					goto IL_0259;
				}
				if (WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill > 0 && pawn.skills != null && pawn.skills.GetSkill(SkillDefOf.Growing).Level < WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill)
				{
					result = null;
				}
				else
				{
					for (int j = 0; j < thingList.Count; j++)
					{
						thing3 = thingList[j];
						if (thing3.def.BlockPlanting)
							goto IL_02d6;
					}
					if (!WorkGiver_Grower.wantedPlantDef.CanEverPlantAt(c, map) || !GenPlant.GrowthSeasonNow(c, map) || !pawn.CanReserve(c, 1, -1, null, false))
					{
						result = null;
					}
					else
					{
						Job job = new Job(JobDefOf.Sow, c);
						job.plantDefToSow = WorkGiver_Grower.wantedPlantDef;
						result = job;
					}
				}
			}
			goto IL_03cd;
			IL_02d6:
			result = (pawn.CanReserve(thing3, 1, -1, null, false) ? ((thing3.def.category != ThingCategory.Plant) ? ((!thing3.def.EverHaulable) ? null : HaulAIUtility.HaulAsideJobFor(pawn, thing3)) : (thing3.IsForbidden(pawn) ? null : new Job(JobDefOf.CutPlant, thing3))) : null);
			goto IL_03cd;
		}
	}
}
