using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000080 RID: 128
	public class JobDriver_TakeAndExitMap : JobDriver
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000216A4 File Offset: 0x0001FAA4
		protected Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x000216D0 File Offset: 0x0001FAD0
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00021704 File Offset: 0x0001FB04
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			gotoCell.AddPreTickAction(delegate
			{
				if (base.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
				}
			});
			yield return gotoCell;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
					{
						this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x04000238 RID: 568
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04000239 RID: 569
		private const TargetIndex ExitCellInd = TargetIndex.B;
	}
}
