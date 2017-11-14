using Verse;

namespace RimWorld
{
	public static class BreakdownableUtility
	{
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			if (compBreakdownable != null)
			{
				return compBreakdownable.BrokenDown;
			}
			return false;
		}
	}
}
