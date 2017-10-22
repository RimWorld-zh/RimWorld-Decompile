using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		private const TargetIndex OtherFianceInd = TargetIndex.A;

		private const int Duration = 2500;

		private int ticksLeftToMarry = 2500;

		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn((Func<bool>)(() => ((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0038: stateMachine*/)._003C_003Ef__this.OtherFiance.Drafted || !((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0038: stateMachine*/)._003C_003Ef__this.pawn.Position.AdjacentTo8WayOrInside(((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0038: stateMachine*/)._003C_003Ef__this.OtherFiance)));
			Toil marry = new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_005b: stateMachine*/)._003C_003Ef__this.ticksLeftToMarry = 2500;
				},
				tickAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.ticksLeftToMarry--;
					if (((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.ticksLeftToMarry <= 0)
					{
						((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.ticksLeftToMarry = 0;
						((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0072: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			marry.FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0095: stateMachine*/)._003C_003Ef__this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, ((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_0095: stateMachine*/)._003C_003Ef__this.OtherFiance)));
			yield return marry;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_00dc: stateMachine*/)._003C_003Ef__this.pawn.thingIDNumber < ((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_00dc: stateMachine*/)._003C_003Ef__this.OtherFiance.thingIDNumber)
					{
						MarriageCeremonyUtility.Married(((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_00dc: stateMachine*/)._003C_003Ef__this.pawn, ((_003CMakeNewToils_003Ec__Iterator16)/*Error near IL_00dc: stateMachine*/)._003C_003Ef__this.OtherFiance);
					}
				}
			};
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}
	}
}
