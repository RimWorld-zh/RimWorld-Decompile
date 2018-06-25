using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020E RID: 526
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		// Token: 0x060009E8 RID: 2536 RVA: 0x00058AB8 File Offset: 0x00056EB8
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
