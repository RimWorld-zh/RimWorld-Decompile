using System;
using Verse;

namespace RimWorld
{
	public class CompTargetable_AllAnimalsOnTheMap : CompTargetable_AllPawnsOnTheMap
	{
		protected override TargetingParameters GetTargetingParameters()
		{
			TargetingParameters targetingParameters = base.GetTargetingParameters();
			targetingParameters.validator = (Predicate<TargetInfo>)delegate(TargetInfo targ)
			{
				bool result;
				if (!base.BaseTargetValidator(targ.Thing))
				{
					result = false;
				}
				else
				{
					Pawn pawn = targ.Thing as Pawn;
					result = (pawn != null && pawn.RaceProps.Animal);
				}
				return result;
			};
			return targetingParameters;
		}
	}
}
