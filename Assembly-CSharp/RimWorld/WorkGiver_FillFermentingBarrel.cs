using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000143 RID: 323
	public class WorkGiver_FillFermentingBarrel : WorkGiver_Scanner
	{
		// Token: 0x04000325 RID: 805
		private static string TemperatureTrans;

		// Token: 0x04000326 RID: 806
		private static string NoWortTrans;

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x00044F2C File Offset: 0x0004332C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x00044F4C File Offset: 0x0004334C
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00044F62 File Offset: 0x00043362
		public static void ResetStaticData()
		{
			WorkGiver_FillFermentingBarrel.TemperatureTrans = "BadTemperature".Translate().ToLower();
			WorkGiver_FillFermentingBarrel.NoWortTrans = "NoWort".Translate();
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00044F88 File Offset: 0x00043388
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			bool result;
			if (building_FermentingBarrel == null || building_FermentingBarrel.Fermented || building_FermentingBarrel.SpaceLeftForWort <= 0)
			{
				result = false;
			}
			else
			{
				float ambientTemperature = building_FermentingBarrel.AmbientTemperature;
				CompProperties_TemperatureRuinable compProperties = building_FermentingBarrel.def.GetCompProperties<CompProperties_TemperatureRuinable>();
				if (ambientTemperature < compProperties.minSafeTemperature + 2f || ambientTemperature > compProperties.maxSafeTemperature - 2f)
				{
					JobFailReason.Is(WorkGiver_FillFermentingBarrel.TemperatureTrans, null);
					result = false;
				}
				else
				{
					if (!t.IsForbidden(pawn))
					{
						LocalTargetInfo target = t;
						if (pawn.CanReserve(target, 1, -1, null, forced))
						{
							if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
							{
								return false;
							}
							if (this.FindWort(pawn, building_FermentingBarrel) == null)
							{
								JobFailReason.Is(WorkGiver_FillFermentingBarrel.NoWortTrans, null);
								return false;
							}
							return !t.IsBurning();
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000450A0 File Offset: 0x000434A0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel barrel = (Building_FermentingBarrel)t;
			Thing t2 = this.FindWort(pawn, barrel);
			return new Job(JobDefOf.FillFermentingBarrel, t, t2);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000450DC File Offset: 0x000434DC
		private Thing FindWort(Pawn pawn, Building_FermentingBarrel barrel)
		{
			Predicate<Thing> predicate = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			ThingRequest thingReq = ThingRequest.ForDef(ThingDefOf.Wort);
			PathEndMode peMode = PathEndMode.ClosestTouch;
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			return GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}
	}
}
