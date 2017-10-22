using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_BeatFire : JobDriver
	{
		protected Fire TargetFire
		{
			get
			{
				return (Fire)base.CurJob.targetA.Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil beat = new Toil();
			Toil approach = new Toil
			{
				initAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.Map.reservationManager.CanReserve(((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.pawn, (Thing)((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.TargetFire, 1, -1, null, false))
					{
						((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.pawn.Reserve((Thing)((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.TargetFire, 1, -1, null);
					}
					((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.pawn.pather.StartPath((Thing)((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_004e: stateMachine*/)._003C_003Ef__this.TargetFire, PathEndMode.Touch);
				},
				tickAction = (Action)delegate
				{
					if (((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.pawn.pather.Moving && ((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.pawn.pather.nextCell != ((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.TargetFire.Position)
					{
						((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.StartBeatingFireIfAnyAt(((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.pawn.pather.nextCell, ((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003Cbeat_003E__0);
					}
					if (((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.pawn.Position != ((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.TargetFire.Position)
					{
						((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.StartBeatingFireIfAnyAt(((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003C_003Ef__this.pawn.Position, ((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_0065: stateMachine*/)._003Cbeat_003E__0);
					}
				}
			};
			approach.FailOnDespawnedOrNull(TargetIndex.A);
			approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			approach.atomicWithPrevious = true;
			yield return approach;
			beat.tickAction = (Action)delegate
			{
				if (!((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.pawn.CanReachImmediate((Thing)((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.TargetFire, PathEndMode.Touch))
				{
					((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.JumpToToil(((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003Capproach_003E__1);
				}
				else
				{
					if (((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.pawn.Position != ((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.TargetFire.Position && ((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.StartBeatingFireIfAnyAt(((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.pawn.Position, ((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003Cbeat_003E__0))
						return;
					((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.pawn.natives.TryBeatFire(((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.TargetFire);
					if (((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.TargetFire.Destroyed)
					{
						((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
						((_003CMakeNewToils_003Ec__Iterator14)/*Error near IL_00b9: stateMachine*/)._003C_003Ef__this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				}
			};
			beat.FailOnDespawnedOrNull(TargetIndex.A);
			beat.defaultCompleteMode = ToilCompleteMode.Never;
			yield return beat;
		}

		private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
		{
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Fire fire = thingList[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					base.pawn.CurJob.targetA = (Thing)fire;
					base.pawn.pather.StopDead();
					base.JumpToToil(nextToil);
					return true;
				}
			}
			return false;
		}
	}
}
