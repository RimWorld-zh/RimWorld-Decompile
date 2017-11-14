using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_BuryCorpse : JobDriver
	{
		private const TargetIndex CorpseIndex = TargetIndex.A;

		private const TargetIndex GraveIndex = TargetIndex.B;

		private Corpse Corpse
		{
			get
			{
				return (Corpse)base.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)base.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		public JobDriver_BuryCorpse()
		{
			base.rotateToFace = TargetIndex.B;
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(this.Corpse, base.job, 1, -1, null) && base.pawn.Reserve(this.Grave, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0051: stateMachine*/)._0024this.Grave.Accepts(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0051: stateMachine*/)._0024this.Corpse));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override object[] TaleParameters()
		{
			return new object[2]
			{
				base.pawn,
				(this.Grave.Corpse == null) ? null : this.Grave.Corpse.InnerPawn
			};
		}
	}
}
