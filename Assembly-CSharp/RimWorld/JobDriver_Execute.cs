using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Execute : JobDriver
	{
		protected Pawn Victim
		{
			get
			{
				return (Pawn)base.job.targetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Victim, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			this.FailOnAggroMentalState(TargetIndex.A);
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Victim, PrisonerInteractionModeDefOf.Execution).FailOn(() => !_003CMakeNewToils_003Ec__Iterator._0024this.Victim.IsPrisonerOfColony || !_003CMakeNewToils_003Ec__Iterator._0024this.Victim.guest.PrisonerIsSecure);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
