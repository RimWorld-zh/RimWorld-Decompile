using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000069 RID: 105
	public class JobDriver_Flee : JobDriver
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x0001F6D0 File Offset: 0x0001DAD0
		public override bool TryMakePreToilReservations()
		{
			this.pawn.Map.pawnDestinationReservationManager.Reserve(this.pawn, this.job, this.job.GetTarget(TargetIndex.A).Cell);
			return true;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0001F71C File Offset: 0x0001DB1C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				atomicWithPrevious = true,
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate()
				{
					if (this.pawn.IsColonist)
					{
						MoteMaker.MakeColonistActionOverlay(this.pawn, ThingDefOf.Mote_ColonistFleeing);
					}
				}
			};
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield break;
		}

		// Token: 0x0400020B RID: 523
		protected const TargetIndex DestInd = TargetIndex.A;

		// Token: 0x0400020C RID: 524
		protected const TargetIndex DangerInd = TargetIndex.B;
	}
}
