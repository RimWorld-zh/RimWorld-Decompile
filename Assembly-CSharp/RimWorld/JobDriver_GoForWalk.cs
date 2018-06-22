using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000054 RID: 84
	public class JobDriver_GoForWalk : JobDriver
	{
		// Token: 0x06000290 RID: 656 RVA: 0x0001B7C4 File Offset: 0x00019BC4
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0001B7DC File Offset: 0x00019BDC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			Toil goToil = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			goToil.tickAction = delegate()
			{
				if (Find.TickManager.TicksGame > this.startTick + this.job.def.joyDuration)
				{
					this.EndJobWith(JobCondition.Succeeded);
				}
				else
				{
					JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, 1f, null);
				}
			};
			yield return goToil;
			yield return new Toil
			{
				initAction = delegate()
				{
					if (this.job.targetQueueA.Count > 0)
					{
						LocalTargetInfo targetA = this.job.targetQueueA[0];
						this.job.targetQueueA.RemoveAt(0);
						this.job.targetA = targetA;
						this.JumpToToil(goToil);
					}
				}
			};
			yield break;
		}
	}
}
