using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000149 RID: 329
	public class WorkGiver_HaulGeneral : WorkGiver_Haul
	{
		// Token: 0x060006D2 RID: 1746 RVA: 0x00046174 File Offset: 0x00044574
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (t is Corpse)
			{
				result = null;
			}
			else
			{
				result = base.JobOnThing(pawn, t, forced);
			}
			return result;
		}
	}
}
