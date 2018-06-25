using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000205 RID: 517
	public class ThoughtWorker_Drunk : ThoughtWorker
	{
		// Token: 0x060009D5 RID: 2517 RVA: 0x00058468 File Offset: 0x00056868
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
