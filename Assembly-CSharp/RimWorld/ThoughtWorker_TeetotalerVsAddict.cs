using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_TeetotalerVsAddict : ThoughtWorker
	{
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
						goto IL_0097;
				}
				result = false;
			}
			goto IL_00c0;
			IL_00c0:
			return result;
			IL_0097:
			result = true;
			goto IL_00c0;
		}
	}
}
