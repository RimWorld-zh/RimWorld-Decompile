using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			WorkGiver_GatherAnimalBodyResources.<PotentialWorkThingsGlobal>c__Iterator59 <PotentialWorkThingsGlobal>c__Iterator = new WorkGiver_GatherAnimalBodyResources.<PotentialWorkThingsGlobal>c__Iterator59();
			<PotentialWorkThingsGlobal>c__Iterator.pawn = pawn;
			<PotentialWorkThingsGlobal>c__Iterator.<$>pawn = pawn;
			WorkGiver_GatherAnimalBodyResources.<PotentialWorkThingsGlobal>c__Iterator59 expr_15 = <PotentialWorkThingsGlobal>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && pawn2.RaceProps.Animal && !pawn2.Downed && pawn2.CanCasuallyInteractNow(false) && this.GetComp(pawn2) != null && this.GetComp(pawn2).ActiveAndFull && pawn.CanReserve(pawn2, 1, -1, ReservationLayer.Default, forced);
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return new Job(this.JobDef, t);
		}
	}
}
