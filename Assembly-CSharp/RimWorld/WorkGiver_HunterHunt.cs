using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_HunterHunt : WorkGiver_Scanner
	{
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation item in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Hunt))
			{
				yield return item.target.Thing;
			}
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			if (!WorkGiver_HunterHunt.HasHuntingWeapon(pawn))
			{
				return true;
			}
			if (WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn))
			{
				return true;
			}
			return false;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 != null && pawn2.RaceProps.Animal)
			{
				if (!pawn.CanReserve(t, 1, -1, null, forced))
				{
					return false;
				}
				if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Hunt) == null)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Hunt, t);
		}

		public static bool HasHuntingWeapon(Pawn p)
		{
			if (p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon)
			{
				return true;
			}
			return false;
		}

		public static bool HasShieldAndRangedWeapon(Pawn p)
		{
			if (p.equipment.Primary != null && !p.equipment.Primary.def.Verbs[0].MeleeRange)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i] is ShieldBelt)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
