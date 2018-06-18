using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A38 RID: 2616
	public class JobDriver_TakeInventory : JobDriver
	{
		// Token: 0x06003A0A RID: 14858 RVA: 0x001EAC14 File Offset: 0x001E9014
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x001EAC48 File Offset: 0x001E9048
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil gotoThing = new Toil();
			gotoThing.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
			};
			gotoThing.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			yield return Toils_Haul.TakeToInventory(TargetIndex.A, this.job.count);
			yield break;
		}
	}
}
