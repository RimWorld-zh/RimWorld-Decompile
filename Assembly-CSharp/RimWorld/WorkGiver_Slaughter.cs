using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Slaughter : WorkGiver_Scanner
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
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Slaughter).GetEnumerator())
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

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				result = false;
			}
			else if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Slaughter) == null)
			{
				result = false;
			}
			else if (pawn.Faction != t.Faction)
			{
				result = false;
			}
			else if (pawn2.InAggroMentalState)
			{
				result = false;
			}
			else
			{
				LocalTargetInfo target = t;
				if (!pawn.CanReserve(target, 1, -1, null, forced))
				{
					result = false;
				}
				else if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					JobFailReason.Is("IsIncapableOfViolenceShort".Translate());
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(JobDefOf.Slaughter, t);
		}
	}
}
