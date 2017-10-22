using Verse;

namespace RimWorld
{
	public static class HuntJobUtility
	{
		public static bool WasKilledByHunter(Pawn pawn, DamageInfo? dinfo)
		{
			bool result;
			if (!dinfo.HasValue)
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
