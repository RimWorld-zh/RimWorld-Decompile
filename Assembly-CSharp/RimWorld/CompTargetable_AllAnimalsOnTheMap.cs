using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class CompTargetable_AllAnimalsOnTheMap : CompTargetable_AllPawnsOnTheMap
	{
		public CompTargetable_AllAnimalsOnTheMap()
		{
		}

		protected override TargetingParameters GetTargetingParameters()
		{
			TargetingParameters targetingParameters = base.GetTargetingParameters();
			targetingParameters.validator = delegate(TargetInfo targ)
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

		[CompilerGenerated]
		private bool <GetTargetingParameters>m__0(TargetInfo targ)
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
		}
	}
}
