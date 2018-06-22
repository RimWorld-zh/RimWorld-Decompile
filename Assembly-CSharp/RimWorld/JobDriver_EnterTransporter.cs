using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000067 RID: 103
	public class JobDriver_EnterTransporter : JobDriver
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x0001F03C File Offset: 0x0001D43C
		private CompTransporter Transporter
		{
			get
			{
				Thing thing = this.job.GetTarget(this.TransporterInd).Thing;
				CompTransporter result;
				if (thing == null)
				{
					result = null;
				}
				else
				{
					result = thing.TryGetComp<CompTransporter>();
				}
				return result;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0001F080 File Offset: 0x0001D480
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001F098 File Offset: 0x0001D498
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.TransporterInd);
			this.FailOn(() => !this.Transporter.LoadingInProgressOrReadyToLaunch);
			yield return Toils_Goto.GotoThing(this.TransporterInd, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					CompTransporter transporter = this.Transporter;
					this.pawn.DeSpawn(DestroyMode.Vanish);
					transporter.GetDirectlyHeldThings().TryAdd(this.pawn, true);
				}
			};
			yield break;
		}

		// Token: 0x04000207 RID: 519
		private TargetIndex TransporterInd = TargetIndex.A;
	}
}
