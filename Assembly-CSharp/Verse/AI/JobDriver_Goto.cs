using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A33 RID: 2611
	public class JobDriver_Goto : JobDriver
	{
		// Token: 0x06003A00 RID: 14848 RVA: 0x001EAA5C File Offset: 0x001E8E5C
		public override bool TryMakePreToilReservations()
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.targetA.Cell);
			return true;
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x001EAAA4 File Offset: 0x001E8EA4
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

		// Token: 0x06003A02 RID: 14850 RVA: 0x001EAAD0 File Offset: 0x001E8ED0
		private void TryExitMap()
		{
			if (!this.job.failIfCantJoinOrCreateCaravan || CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn))
			{
				this.pawn.ExitMap(true, CellRect.WholeMap(base.Map).GetClosestEdge(this.pawn.Position));
			}
		}
	}
}
