using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_FillFermentingBarrel : WorkGiver_Scanner
	{
		private static string TemperatureTrans;

		private static string NoWortTrans;

		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public static void Reset()
		{
			WorkGiver_FillFermentingBarrel.TemperatureTrans = "BadTemperature".Translate().ToLower();
			WorkGiver_FillFermentingBarrel.NoWortTrans = "NoWort".Translate();
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			if (building_FermentingBarrel != null && !building_FermentingBarrel.Fermented && building_FermentingBarrel.SpaceLeftForWort > 0)
			{
				float ambientTemperature = building_FermentingBarrel.AmbientTemperature;
				CompProperties_TemperatureRuinable compProperties = building_FermentingBarrel.def.GetCompProperties<CompProperties_TemperatureRuinable>();
				if (!(ambientTemperature < compProperties.minSafeTemperature + 2.0) && !(ambientTemperature > compProperties.maxSafeTemperature - 2.0))
				{
					if (!t.IsForbidden(pawn) && pawn.CanReserveAndReach(t, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, forced))
					{
						if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
						{
							return false;
						}
						Thing thing = this.FindWort(pawn, building_FermentingBarrel);
						if (thing == null)
						{
							JobFailReason.Is(WorkGiver_FillFermentingBarrel.NoWortTrans);
							return false;
						}
						if (t.IsBurning())
						{
							return false;
						}
						return true;
					}
					return false;
				}
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.TemperatureTrans);
				return false;
			}
			return false;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = (Building_FermentingBarrel)t;
			Thing t2 = this.FindWort(pawn, building_FermentingBarrel);
			Job job = new Job(JobDefOf.FillFermentingBarrel, t, t2);
			job.count = building_FermentingBarrel.SpaceLeftForWort;
			return job;
		}

		private Thing FindWort(Pawn pawn, Building_FermentingBarrel barrel)
		{
			Predicate<Thing> validator;
			Predicate<Thing> predicate = validator = (Predicate<Thing>)delegate(Thing x)
			{
				if (!x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false))
				{
					return true;
				}
				return false;
			};
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Wort), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}
	}
}
