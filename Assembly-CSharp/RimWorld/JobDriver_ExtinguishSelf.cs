using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ExtinguishSelf : JobDriver
	{
		protected Fire TargetFire
		{
			get
			{
				return (Fire)base.job.targetA.Thing;
			}
		}

		public override string GetReport()
		{
			return (this.TargetFire == null || this.TargetFire.parent == null) ? "ReportExtinguishingFire".Translate() : "ReportExtinguishingFireOn".Translate(this.TargetFire.parent.LabelCap);
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 150
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
