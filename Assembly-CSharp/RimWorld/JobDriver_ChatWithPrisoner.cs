using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ChatWithPrisoner : JobDriver
	{
		protected Pawn Talkee
		{
			get
			{
				return (Pawn)base.job.targetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnMentalState(TargetIndex.A);
			this.FailOnNotAwake(TargetIndex.A);
			this.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_009b: stateMachine*/)._0024this.Talkee.IsPrisonerOfColony || !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_009b: stateMachine*/)._0024this.Talkee.guest.PrisonerIsSecure));
			yield return Toils_Interpersonal.GotoPrisoner(base.pawn, this.Talkee, this.Talkee.guest.interactionMode);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
