using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_UseItem : JobDriver
	{
		private int useDuration = -1;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.useDuration, "useDuration", 0, false);
		}

		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.useDuration = base.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompUsable>().Props.useDuration;
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
