using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000201 RID: 513
	public class ThoughtWorker_TeetotalerVsAddict : ThoughtWorker
	{
		// Token: 0x060009D0 RID: 2512 RVA: 0x000581A8 File Offset: 0x000565A8
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
				List<Hediff> hediffs = other.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].def.IsAddiction)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}
	}
}
