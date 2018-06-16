using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200003B RID: 59
	public class JobDriver_PrepareCaravan_GatherPawns : JobDriver
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00015A74 File Offset: 0x00013E74
		private Pawn AnimalOrSlave
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00015AA4 File Offset: 0x00013EA4
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.AnimalOrSlave, this.job, 1, -1, null);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00015AD8 File Offset: 0x00013ED8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
			this.FailOn(() => this.AnimalOrSlave == null || this.AnimalOrSlave.GetLord() != this.job.lord);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A).FailOn(() => GatherAnimalsAndSlavesForCaravanUtility.IsFollowingAnyone(this.AnimalOrSlave));
			yield return this.SetFollowerToil();
			yield break;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00015B04 File Offset: 0x00013F04
		private Toil SetFollowerToil()
		{
			return new Toil
			{
				initAction = delegate()
				{
					GatherAnimalsAndSlavesForCaravanUtility.SetFollower(this.AnimalOrSlave, this.pawn);
					RestUtility.WakeUp(this.pawn);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}

		// Token: 0x040001C9 RID: 457
		private const TargetIndex AnimalOrSlaveInd = TargetIndex.A;
	}
}
