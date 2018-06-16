using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200020C RID: 524
	public class ThoughtWorker_Tale : ThoughtWorker
	{
		// Token: 0x060009E6 RID: 2534 RVA: 0x000588F4 File Offset: 0x00056CF4
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
