using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_WatchBuilding : JobDriver
	{
		public override bool TryMakePreToilReservations()
		{
			bool result;
			if (!base.pawn.Reserve(base.job.targetA, base.job, base.job.def.joyMaxParticipants, 0, null))
			{
				result = false;
			}
			else if (!base.pawn.Reserve(base.job.targetB, base.job, 1, -1, null))
			{
				result = false;
			}
			else
			{
				if (base.TargetC.HasThing)
				{
					if (base.TargetC.Thing is Building_Bed)
					{
						if (!base.pawn.Reserve(base.job.targetC, base.job, ((Building_Bed)base.TargetC.Thing).SleepingSlotsCount, 0, null))
						{
							result = false;
							goto IL_0111;
						}
					}
					else if (!base.pawn.Reserve(base.job.targetC, base.job, 1, -1, null))
					{
						result = false;
						goto IL_0111;
					}
				}
				result = true;
			}
			goto IL_0111;
			IL_0111:
			return result;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_003e: stateMachine*/;
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			if (base.TargetC.HasThing && base.TargetC.Thing is Building_Bed)
			{
				this.KeepLyingDown(TargetIndex.C);
				yield return Toils_Bed.ClaimBedIfNonMedical(TargetIndex.C, TargetIndex.None);
				/*Error: Unable to find new state assignment for yield return*/;
			}
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			/*Error: Unable to find new state assignment for yield return*/;
		}

		protected virtual void WatchTickAction()
		{
			base.pawn.rotationTracker.FaceCell(base.TargetA.Cell);
			base.pawn.GainComfortFromCellIfPossible();
			float statValue = base.TargetThingA.GetStatValue(StatDefOf.EntertainmentStrengthFactor, true);
			Pawn pawn = base.pawn;
			float extraJoyGainFactor = statValue;
			JoyUtility.JoyTickCheckEnd(pawn, JoyTickFullJoyAction.EndJob, extraJoyGainFactor);
		}

		public override object[] TaleParameters()
		{
			return new object[2]
			{
				base.pawn,
				base.TargetA.Thing.def
			};
		}
	}
}
