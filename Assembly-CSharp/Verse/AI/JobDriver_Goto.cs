using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A37 RID: 2615
	public class JobDriver_Goto : JobDriver
	{
		// Token: 0x06003A04 RID: 14852 RVA: 0x001EA748 File Offset: 0x001E8B48
		public override bool TryMakePreToilReservations()
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x001EA790 File Offset: 0x001E8B90
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil gotoCell = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			gotoCell.AddPreTickAction(delegate
			{
				if (this.job.exitMapOnArrival && this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position))
				{
					this.TryExitMap();
				}
			});
			gotoCell.FailOn(() => this.job.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn));
			yield return gotoCell;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.pawn.mindState != null && this.pawn.mindState.forcedGotoPosition == base.TargetA.Cell)
					{
						this.pawn.mindState.forcedGotoPosition = IntVec3.Invalid;
					}
					if (this.job.exitMapOnArrival && (this.pawn.Position.OnEdge(this.pawn.Map) || this.pawn.Map.exitMapGrid.IsExitCell(this.pawn.Position)))
					{
						this.TryExitMap();
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x001EA7BC File Offset: 0x001E8BBC
		private void TryExitMap()
		{
			if (!this.job.failIfCantJoinOrCreateCaravan || CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn))
			{
				this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
			}
		}
	}
}
