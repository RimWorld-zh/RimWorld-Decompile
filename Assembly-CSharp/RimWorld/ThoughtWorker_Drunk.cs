using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000205 RID: 517
	public class ThoughtWorker_Drunk : ThoughtWorker
	{
		// Token: 0x060009D6 RID: 2518 RVA: 0x0005846C File Offset: 0x0005686C
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!p.IsTeetotaler())
			{
				result = false;
			}
			else if (!other.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				result = false;
			}
			else
			{
				Hediff firstHediffOfDef = other.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh, false);
				if (firstHediffOfDef == null || !firstHediffOfDef.Visible)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
	}
}
