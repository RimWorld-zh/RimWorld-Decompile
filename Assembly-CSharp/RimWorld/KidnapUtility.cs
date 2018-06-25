using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000564 RID: 1380
	public static class KidnapUtility
	{
		// Token: 0x06001A14 RID: 6676 RVA: 0x000E25F4 File Offset: 0x000E09F4
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
