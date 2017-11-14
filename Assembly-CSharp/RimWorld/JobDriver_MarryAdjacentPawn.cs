using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		private int ticksLeftToMarry = 2500;

		private const TargetIndex OtherFianceInd = TargetIndex.A;

		private const int Duration = 2500;

		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0038: stateMachine*/)._0024this.OtherFiance.Drafted || !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0038: stateMachine*/)._0024this.pawn.Position.AdjacentTo8WayOrInside(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0038: stateMachine*/)._0024this.OtherFiance));
			Toil marry = new Toil
			{
				initAction = delegate
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_005b: stateMachine*/)._0024this.ticksLeftToMarry = 2500;
				},
				tickAction = delegate
				{
					((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0072: stateMachine*/)._0024this.ticksLeftToMarry--;
					if (((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0072: stateMachine*/)._0024this.ticksLeftToMarry <= 0)
					{
						((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0072: stateMachine*/)._0024this.ticksLeftToMarry = 0;
						((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0072: stateMachine*/)._0024this.ReadyForNextToil();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			marry.FailOn(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0095: stateMachine*/)._0024this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0095: stateMachine*/)._0024this.OtherFiance));
			yield return marry;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}
	}
}
