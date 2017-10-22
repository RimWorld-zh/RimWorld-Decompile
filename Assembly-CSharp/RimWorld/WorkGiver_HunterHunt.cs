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
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Hunt).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Designation des = enumerator.Current;
					yield return des.target.Thing;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00d6:
			/*Error near IL_00d7: Unexpected return in MoveNext()*/;
		}

		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		public override bool ShouldSkip(Pawn pawn)
		{
			return (byte)((!WorkGiver_HunterHunt.HasHuntingWeapon(pawn)) ? 1 : (WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn) ? 1 : 0)) != 0;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 == null || !pawn2.AnimalOrWildMan())
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				result = ((byte)(pawn.CanReserve(target, 1, -1, null, forced) ? ((pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Hunt) != null) ? 1 : 0) : 0) != 0);
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Hunt, t);
		}

		public static bool HasHuntingWeapon(Pawn p)
		{
			return (byte)((p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon && p.equipment.PrimaryEq.PrimaryVerb.HarmsHealth()) ? 1 : 0) != 0;
		}

		public static bool HasShieldAndRangedWeapon(Pawn p)
		{
			if (p.equipment.Primary != null && p.equipment.Primary.def.IsRangedWeapon)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (wornApparel[i] is ShieldBelt)
						goto IL_0051;
				}
			}
			bool result = false;
			goto IL_0071;
			IL_0071:
			return result;
			IL_0051:
			result = true;
			goto IL_0071;
		}
	}
}
