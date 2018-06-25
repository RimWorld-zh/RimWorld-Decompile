using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200075B RID: 1883
	public class CompTargetable_AllAnimalsOnTheMap : CompTargetable_AllPawnsOnTheMap
	{
		// Token: 0x060029A1 RID: 10657 RVA: 0x00161EF8 File Offset: 0x001602F8
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
