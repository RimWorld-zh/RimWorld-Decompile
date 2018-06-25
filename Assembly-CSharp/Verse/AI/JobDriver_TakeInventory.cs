using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000A36 RID: 2614
	public class JobDriver_TakeInventory : JobDriver
	{
		// Token: 0x06003A08 RID: 14856 RVA: 0x001EAF80 File Offset: 0x001E9380
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x001EAFB4 File Offset: 0x001E93B4
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
