using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000562 RID: 1378
	public static class KidnapUtility
	{
		// Token: 0x06001A10 RID: 6672 RVA: 0x000E24A4 File Offset: 0x000E08A4
		public static bool IsKidnapped(this Pawn pawn)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading.Contains(pawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
