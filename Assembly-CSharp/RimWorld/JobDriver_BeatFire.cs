using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200004B RID: 75
	public class JobDriver_BeatFire : JobDriver
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00018FF8 File Offset: 0x000173F8
		protected Fire TargetFire
		{
			get
			{
				return (Fire)this.job.targetA.Thing;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00019024 File Offset: 0x00017424
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0001903C File Offset: 0x0001743C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Toil beat = new Toil();
			Toil approach = new Toil();
			approach.initAction = delegate()
			{
				if (this.Map.reservationManager.CanReserve(this.pawn, this.TargetFire, 1, -1, null, false))
				{
					this.pawn.Reserve(this.TargetFire, this.job, 1, -1, null);
				}
				this.pawn.pather.StartPath(this.TargetFire, PathEndMode.Touch);
			};
			approach.tickAction = delegate()
			{
				if (this.pawn.pather.Moving && this.pawn.pather.nextCell != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.pather.nextCell, beat);
				}
				if (this.pawn.Position != this.TargetFire.Position)
				{
					this.StartBeatingFireIfAnyAt(this.pawn.Position, beat);
				}
			};
			approach.FailOnDespawnedOrNull(TargetIndex.A);
			approach.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			approach.atomicWithPrevious = true;
			yield return approach;
			beat.tickAction = delegate()
			{
				if (!this.pawn.CanReachImmediate(this.TargetFire, PathEndMode.Touch))
				{
					this.JumpToToil(approach);
				}
				else if (!(this.pawn.Position != this.TargetFire.Position) || !this.StartBeatingFireIfAnyAt(this.pawn.Position, beat))
				{
					this.pawn.natives.TryBeatFire(this.TargetFire);
					if (this.TargetFire.Destroyed)
					{
						this.pawn.records.Increment(RecordDefOf.FiresExtinguished);
						this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true);
					}
				}
			};
			beat.FailOnDespawnedOrNull(TargetIndex.A);
			beat.defaultCompleteMode = ToilCompleteMode.Never;
			yield return beat;
			yield break;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00019068 File Offset: 0x00017468
		private bool StartBeatingFireIfAnyAt(IntVec3 cell, Toil nextToil)
		{
			List<Thing> thingList = cell.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Fire fire = thingList[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					this.job.targetA = fire;
					this.pawn.pather.StopDead();
					base.JumpToToil(nextToil);
					return true;
				}
			}
			return false;
		}
	}
}
