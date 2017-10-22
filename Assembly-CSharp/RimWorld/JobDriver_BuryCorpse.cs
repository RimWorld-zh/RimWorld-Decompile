using System;
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
				return (Corpse)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)base.CurJob.GetTarget(TargetIndex.B).Thing;
			}
		}

		public JobDriver_BuryCorpse()
		{
			base.rotateToFace = TargetIndex.B;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			this.FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false);
			yield return Toils_Haul.CarryHauledThingToContainer();
			Toil prepare = Toils_General.Wait(250);
			prepare.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return prepare;
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.pawn.carryTracker.CarriedThing == null)
					{
						Log.Error(((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.pawn + " tried to place hauled corpse in grave but is not hauling anything.");
					}
					else if (((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.Grave.TryAcceptThing(((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.Corpse, true))
					{
						((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.pawn.carryTracker.innerContainer.Remove(((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.Corpse);
						((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.Grave.Notify_CorpseBuried(((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.pawn);
						((_003CMakeNewToils_003Ec__Iterator24)/*Error near IL_0125: stateMachine*/)._003C_003Ef__this.pawn.records.Increment(RecordDefOf.CorpsesBuried);
					}
				}
			};
		}
	}
}
