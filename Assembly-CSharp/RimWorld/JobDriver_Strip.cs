using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200007F RID: 127
	public class JobDriver_Strip : JobDriver
	{
		// Token: 0x04000238 RID: 568
		private const int StripTicks = 60;

		// Token: 0x0600035C RID: 860 RVA: 0x0002548C File Offset: 0x0002388C
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x000254C0 File Offset: 0x000238C0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOnAggroMentalState(TargetIndex.A);
			this.FailOn(() => !StrippableUtility.CanBeStrippedByColony(base.TargetThingA));
			Toil gotoThing = new Toil();
			gotoThing.initAction = delegate()
			{
				this.pawn.pather.StartPath(base.TargetThingA, PathEndMode.ClosestTouch);
			};
			gotoThing.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			gotoThing.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return gotoThing;
			yield return Toils_General.Wait(60).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate()
				{
					Thing thing = this.job.targetA.Thing;
					Designation designation = base.Map.designationManager.DesignationOn(thing, DesignationDefOf.Strip);
					if (designation != null)
					{
						designation.Delete();
					}
					IStrippable strippable = thing as IStrippable;
					if (strippable != null)
					{
						strippable.Strip();
					}
					this.pawn.records.Increment(RecordDefOf.BodiesStripped);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000254EC File Offset: 0x000238EC
		public override object[] TaleParameters()
		{
			Corpse corpse = base.TargetA.Thing as Corpse;
			return new object[]
			{
				this.pawn,
				(corpse == null) ? base.TargetA.Thing : corpse.InnerPawn
			};
		}
	}
}
