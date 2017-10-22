using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_RemoveBuilding : WorkGiver_Scanner
	{
		protected abstract DesignationDef Designation
		{
			get;
		}

		protected abstract JobDef RemoveBuildingJob
		{
			get;
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			using (IEnumerator<Designation> enumerator = pawn.Map.designationManager.SpawnedDesignationsOfDef(this.Designation).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Designation des = enumerator.Current;
					yield return des.target.Thing;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_00dc:
			/*Error near IL_00dd: Unexpected return in MoveNext()*/;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			bool result;
			if (t.def.Claimable)
			{
				if (t.Faction != pawn.Faction)
				{
					result = false;
					goto IL_0095;
				}
			}
			else if (pawn.Faction != Faction.OfPlayer)
			{
				result = false;
				goto IL_0095;
			}
			LocalTargetInfo target = t;
			result = ((byte)(pawn.CanReserve(target, 1, -1, null, forced) ? ((pawn.Map.designationManager.DesignationOn(t, this.Designation) != null) ? 1 : 0) : 0) != 0);
			goto IL_0095;
			IL_0095:
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(this.RemoveBuildingJob, t);
		}
	}
}
