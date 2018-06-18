using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A35 RID: 2613
	public class JobDriver_Follow : JobDriver
	{
		// Token: 0x060039F8 RID: 14840 RVA: 0x001E9E68 File Offset: 0x001E8268
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x001E9E80 File Offset: 0x001E8280
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				tickAction = delegate()
				{
					Pawn pawn = (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
					if (!this.pawn.Position.InHorDistOf(pawn.Position, 4f) || !this.pawn.Position.WithinRegions(pawn.Position, base.Map, 2, TraverseParms.For(this.pawn, Danger.Deadly, TraverseMode.ByPawn, false), RegionType.Set_Passable))
					{
						if (!this.pawn.CanReach(pawn, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
						{
							base.EndJobWith(JobCondition.Incompletable);
						}
						else if (!this.pawn.pather.Moving || this.pawn.pather.Destination != pawn)
						{
							this.pawn.pather.StartPath(pawn, PathEndMode.Touch);
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			yield break;
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x001E9EAC File Offset: 0x001E82AC
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x040024FE RID: 9470
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x040024FF RID: 9471
		private const int Distance = 4;
	}
}
