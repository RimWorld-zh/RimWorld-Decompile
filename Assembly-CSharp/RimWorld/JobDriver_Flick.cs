using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200006B RID: 107
	public class JobDriver_Flick : JobDriver
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x0001FC24 File Offset: 0x0001E024
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0001FC58 File Offset: 0x0001E058
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(delegate()
			{
				Designation designation = this.Map.designationManager.DesignationOn(this.TargetThingA, DesignationDefOf.Flick);
				return designation == null;
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(15).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			Toil finalize = new Toil();
			finalize.initAction = delegate()
			{
				Pawn actor = finalize.actor;
				ThingWithComps thingWithComps = (ThingWithComps)actor.CurJob.targetA.Thing;
				for (int i = 0; i < thingWithComps.AllComps.Count; i++)
				{
					CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
					if (compFlickable != null && compFlickable.WantsFlick())
					{
						compFlickable.DoFlick();
					}
				}
				actor.records.Increment(RecordDefOf.SwitchesFlicked);
				Designation designation = this.Map.designationManager.DesignationOn(thingWithComps, DesignationDefOf.Flick);
				if (designation != null)
				{
					designation.Delete();
				}
			};
			finalize.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return finalize;
			yield break;
		}
	}
}
