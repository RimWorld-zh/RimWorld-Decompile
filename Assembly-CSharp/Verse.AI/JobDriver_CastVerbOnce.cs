using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_CastVerbOnce : JobDriver
	{
		public override string GetReport()
		{
			string text = (!base.TargetA.HasThing) ? "AreaLower".Translate() : base.TargetThingA.LabelCap;
			return "UsingVerb".Translate(base.job.verbToUse.verbProps.label, text);
		}

		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
