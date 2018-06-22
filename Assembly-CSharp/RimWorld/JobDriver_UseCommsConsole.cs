using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000086 RID: 134
	public class JobDriver_UseCommsConsole : JobDriver
	{
		// Token: 0x0600037C RID: 892 RVA: 0x000271DC File Offset: 0x000255DC
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00027210 File Offset: 0x00025610
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell).FailOn(delegate(Toil to)
			{
				Building_CommsConsole building_CommsConsole = (Building_CommsConsole)to.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
				return !building_CommsConsole.CanUseCommsNow;
			});
			Toil openComms = new Toil();
			openComms.initAction = delegate()
			{
				Pawn actor = openComms.actor;
				Building_CommsConsole building_CommsConsole = (Building_CommsConsole)actor.jobs.curJob.GetTarget(TargetIndex.A).Thing;
				if (building_CommsConsole.CanUseCommsNow)
				{
					actor.jobs.curJob.commTarget.TryOpenComms(actor);
				}
			};
			yield return openComms;
			yield break;
		}
	}
}
