using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000078 RID: 120
	public class JobDriver_RearmTrap : JobDriver
	{
		// Token: 0x04000228 RID: 552
		private const int RearmTicks = 800;

		// Token: 0x0600033A RID: 826 RVA: 0x00023ABC File Offset: 0x00021EBC
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00023AF0 File Offset: 0x00021EF0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.RearmTrap);
			Toil gotoThing = new Toil();
			gotoThing.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.Touch);
			};
			gotoThing.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			yield return Toils_General.Wait(800).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.job.targetA.Thing;
					Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.RearmTrap);
					if (designation != null)
					{
						designation.Delete();
					}
					Building_TrapRearmable building_TrapRearmable = thing as Building_TrapRearmable;
					building_TrapRearmable.Rearm();
					this.pawn.records.Increment(RecordDefOf.TrapsRearmed);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}
	}
}
