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
				return (Fire)base.job.targetA.Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0036: stateMachine*/;
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil beat = new Toil();
			Toil approach = new Toil();
			approach.initAction = (Action)delegate
			{
				if (_003CMakeNewToils_003Ec__Iterator._0024this.Map.reservationManager.CanReserve(_003CMakeNewToils_003Ec__Iterator._0024this.pawn, (Thing)_003CMakeNewToils_003Ec__Iterator._0024this.TargetFire, 1, -1, null, false))
				{
					_003CMakeNewToils_003Ec__Iterator._0024this.pawn.Reserve((Thing)_003CMakeNewToils_003Ec__Iterator._0024this.TargetFire, _003CMakeNewToils_003Ec__Iterator._0024this.job, 1, -1, null);
				}
				_003CMakeNewToils_003Ec__Iterator._0024this.pawn.pather.StartPath((Thing)_003CMakeNewToils_003Ec__Iterator._0024this.TargetFire, PathEndMode.Touch);
			};
			approach.tickAction = (Action)delegate
			{
				if (_003CMakeNewToils_003Ec__Iterator._0024this.pawn.pather.Moving && _003CMakeNewToils_003Ec__Iterator._0024this.pawn.pather.nextCell != _003CMakeNewToils_003Ec__Iterator._0024this.TargetFire.Position)
				{
					_003CMakeNewToils_003Ec__Iterator._0024this.StartBeatingFireIfAnyAt(_003CMakeNewToils_003Ec__Iterator._0024this.pawn.pather.nextCell, beat);
				}
				if (_003CMakeNewToils_003Ec__Iterator._0024this.pawn.Position != _003CMakeNewToils_003Ec__Iterator._0024this.TargetFire.Position)
				{
					_003CMakeNewToils_003Ec__Iterator._0024this.StartBeatingFireIfAnyAt(_003CMakeNewToils_003Ec__Iterator._0024this.pawn.Position, beat);
				}
			};
			approach.FailOnDespawnedOrNull(TargetIndex.A);
			approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			approach.atomicWithPrevious = true;
			yield return approach;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
		{
			List<Thing> thingList = cell.GetThingList(base.Map);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < thingList.Count)
				{
					Fire fire = thingList[num] as Fire;
					if (fire != null && fire.parent == null)
					{
						base.job.targetA = (Thing)fire;
						base.pawn.pather.StopDead();
						base.JumpToToil(nextToil);
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
