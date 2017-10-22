using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_GrowerSow : WorkGiver_Grower
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		protected override bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			if (!settable.CanAcceptSowNow())
			{
				return false;
			}
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
			if (WorkGiver_Grower.wantedPlantDef == null)
			{
				return false;
			}
			return true;
		}

		public override Job JobOnCell(Pawn pawn, IntVec3 c)
		{
			if (c.IsForbidden(pawn))
			{
				return null;
			}
			if (!GenPlant.GrowthSeasonNow(c, pawn.Map))
			{
				return null;
			}
			if (WorkGiver_Grower.wantedPlantDef == null)
			{
				WorkGiver_Grower.wantedPlantDef = WorkGiver_Grower.CalculateWantedPlantDef(c, pawn.Map);
				if (WorkGiver_Grower.wantedPlantDef == null)
				{
					return null;
				}
			}
			List<Thing> thingList = c.GetThingList(pawn.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def == WorkGiver_Grower.wantedPlantDef)
				{
					return null;
				}
				if ((thing is Blueprint || thing is Frame) && thing.Faction == pawn.Faction)
				{
					return null;
				}
			}
			Plant plant = c.GetPlant(pawn.Map);
			if (plant != null && plant.def.plant.blockAdjacentSow)
			{
				if (pawn.CanReserve((Thing)plant, 1, -1, null, false) && !plant.IsForbidden(pawn))
				{
					return new Job(JobDefOf.CutPlant, (Thing)plant);
				}
				return null;
			}
			Thing thing2 = GenPlant.AdjacentSowBlocker(WorkGiver_Grower.wantedPlantDef, c, pawn.Map);
			if (thing2 != null)
			{
				Plant plant2 = thing2 as Plant;
				if (plant2 != null && pawn.CanReserve((Thing)plant2, 1, -1, null, false) && !plant2.IsForbidden(pawn))
				{
					IPlantToGrowSettable plantToGrowSettable = plant2.Position.GetPlantToGrowSettable(plant2.Map);
					if (plantToGrowSettable != null && plantToGrowSettable.GetPlantDefToGrow() == plant2.def)
					{
						goto IL_0199;
					}
					return new Job(JobDefOf.CutPlant, (Thing)plant2);
				}
				goto IL_0199;
			}
			if (WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill > 0 && pawn.skills != null && pawn.skills.GetSkill(SkillDefOf.Growing).Level < WorkGiver_Grower.wantedPlantDef.plant.sowMinSkill)
			{
				return null;
			}
			for (int j = 0; j < thingList.Count; j++)
			{
				Thing thing3 = thingList[j];
				if (thing3.def.BlockPlanting)
				{
					if (!pawn.CanReserve(thing3, 1, -1, null, false))
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
					if (thing3.def.EverHaulable)
					{
						return HaulAIUtility.HaulAsideJobFor(pawn, thing3);
					}
					return null;
				}
			}
			if (WorkGiver_Grower.wantedPlantDef.CanEverPlantAt(c, pawn.Map) && GenPlant.GrowthSeasonNow(c, pawn.Map) && pawn.CanReserve(c, 1, -1, null, false))
			{
				Job job = new Job(JobDefOf.Sow, c);
				job.plantDefToSow = WorkGiver_Grower.wantedPlantDef;
				return job;
			}
			return null;
			IL_0199:
			return null;
		}
	}
}
