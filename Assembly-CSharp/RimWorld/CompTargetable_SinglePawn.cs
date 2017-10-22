using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompTargetable_SinglePawn : CompTargetable
	{
		protected override bool PlayerChoosesTarget
		{
			get
			{
				return true;
			}
		}

		protected override TargetingParameters GetTargetingParameters()
		{
			TargetingParameters targetingParameters = new TargetingParameters();
			targetingParameters.canTargetPawns = true;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.validator = (Predicate<TargetInfo>)((TargetInfo x) => base.BaseTargetValidator(x.Thing));
			return targetingParameters;
		}

		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
		}
	}
}
