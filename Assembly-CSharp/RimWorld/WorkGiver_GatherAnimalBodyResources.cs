using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class WorkGiver_GatherAnimalBodyResources : WorkGiver_Scanner
	{
		protected abstract JobDef JobDef
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

		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Pawn> pawns = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			int i = 0;
			if (i < pawns.Count)
			{
				yield return (Thing)pawns[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			bool result;
			if (pawn2 != null && pawn2.RaceProps.Animal)
			{
				CompHasGatherableBodyResource comp = this.GetComp(pawn2);
				if (comp != null && comp.ActiveAndFull && !pawn2.Downed && pawn2.CanCasuallyInteractNow(false))
				{
					LocalTargetInfo target = (Thing)pawn2;
					if (!pawn.CanReserve(target, 1, -1, null, forced))
						goto IL_0074;
					result = true;
					goto IL_0083;
				}
				goto IL_0074;
			}
			result = false;
			goto IL_0083;
			IL_0083:
			return result;
			IL_0074:
			result = false;
			goto IL_0083;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(this.JobDef, t);
		}
	}
}
