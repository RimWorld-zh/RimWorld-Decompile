using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Open : JobDriver
	{
		public const int OpenTicks = 300;

		private IOpenable Openable
		{
			get
			{
				return (IOpenable)base.job.targetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					if (!((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003f: stateMachine*/)._0024this.Openable.CanOpen)
					{
						Designation designation = ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003f: stateMachine*/)._0024this.Map.designationManager.DesignationOn(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003f: stateMachine*/)._0024this.job.targetA.Thing, DesignationDefOf.Open);
						if (designation != null)
						{
							designation.Delete();
						}
					}
				}
			}.FailOnDespawnedOrNull(TargetIndex.A);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
