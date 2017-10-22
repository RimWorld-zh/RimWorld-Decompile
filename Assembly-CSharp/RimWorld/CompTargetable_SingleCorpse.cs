using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompTargetable_SingleCorpse : CompTargetable
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
			targetingParameters.canTargetPawns = false;
			targetingParameters.canTargetBuildings = false;
			targetingParameters.canTargetItems = true;
			targetingParameters.mapObjectTargetsMustBeAutoAttackable = false;
			targetingParameters.validator = (Predicate<TargetInfo>)((TargetInfo x) => x.Thing is Corpse && base.BaseTargetValidator(x.Thing));
			return targetingParameters;
		}

		public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
		{
			yield return targetChosenByPlayer;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
