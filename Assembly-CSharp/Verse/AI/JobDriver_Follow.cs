using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A31 RID: 2609
	public class JobDriver_Follow : JobDriver
	{
		// Token: 0x040024F9 RID: 9465
		private const TargetIndex FolloweeInd = TargetIndex.A;

		// Token: 0x040024FA RID: 9466
		private const int Distance = 4;

		// Token: 0x060039F2 RID: 14834 RVA: 0x001EA0A8 File Offset: 0x001E84A8
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x001EA0C0 File Offset: 0x001E84C0
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

		// Token: 0x060039F4 RID: 14836 RVA: 0x001EA0EC File Offset: 0x001E84EC
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}
	}
}
