using System.Collections.Generic;

namespace Verse.AI
{
	public class JobDriver_CastVerbOnce : JobDriver
	{
		public override string GetReport()
		{
			string text = (!base.TargetA.HasThing) ? "AreaLower".Translate() : base.TargetThingA.LabelCap;
			return "UsingVerb".Translate(base.CurJob.verbToUse.verbProps.label, text);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Combat.GotoCastPosition(TargetIndex.A, false);
			yield return Toils_Combat.CastVerb(TargetIndex.A, true);
		}
	}
}
