using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Slaughter : JobDriver
	{
		private const int SlaughterDuration = 180;

		protected Pawn Victim
		{
			get
			{
				return (Pawn)base.job.targetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve((Thing)this.Victim, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Slaughter);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
