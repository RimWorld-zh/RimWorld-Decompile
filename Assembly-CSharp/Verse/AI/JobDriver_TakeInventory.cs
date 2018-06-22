using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A34 RID: 2612
	public class JobDriver_TakeInventory : JobDriver
	{
		// Token: 0x06003A04 RID: 14852 RVA: 0x001EAE54 File Offset: 0x001E9254
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x001EAE88 File Offset: 0x001E9288
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
