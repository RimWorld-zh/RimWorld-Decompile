using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020E RID: 526
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		// Token: 0x060009E7 RID: 2535 RVA: 0x00058AB4 File Offset: 0x00056EB4
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			ThoughtState result;
			if (!other.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(p, other))
			{
				result = false;
			}
			else if (Find.TaleManager.GetLatestTale(this.def.taleDef, other) == null)
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
