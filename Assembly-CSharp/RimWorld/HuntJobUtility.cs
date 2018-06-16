using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200006E RID: 110
	public static class HuntJobUtility
	{
		// Token: 0x0600030A RID: 778 RVA: 0x000210C0 File Offset: 0x0001F4C0
		public static bool WasKilledByHunter(Pawn pawn, DamageInfo? dinfo)
		{
			bool result;
			if (dinfo == null)
			{
				result = false;
			}
			else
			{
				Pawn pawn2 = dinfo.Value.Instigator as Pawn;
				if (pawn2 == null || pawn2.CurJob == null)
				{
					result = false;
				}
				else
				{
					JobDriver_Hunt jobDriver_Hunt = pawn2.jobs.curDriver as JobDriver_Hunt;
					result = (jobDriver_Hunt != null && jobDriver_Hunt.Victim == pawn);
				}
			}
			return result;
		}
	}
}
