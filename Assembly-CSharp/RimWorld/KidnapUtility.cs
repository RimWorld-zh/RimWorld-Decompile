using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000566 RID: 1382
	public static class KidnapUtility
	{
		// Token: 0x06001A19 RID: 6681 RVA: 0x000E2450 File Offset: 0x000E0850
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
