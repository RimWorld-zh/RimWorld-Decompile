using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000759 RID: 1881
	public class CompTargetable_AllAnimalsOnTheMap : CompTargetable_AllPawnsOnTheMap
	{
		// Token: 0x0600299D RID: 10653 RVA: 0x00161DA8 File Offset: 0x001601A8
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
	}
}
