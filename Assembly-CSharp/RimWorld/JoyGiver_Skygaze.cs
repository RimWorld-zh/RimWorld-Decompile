using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_Skygaze : JoyGiver
	{
		public override Job TryGiveJob(Pawn pawn)
		{
			if (JoyUtility.EnjoyableOutsideNow(pawn, null) && !(pawn.Map.weatherManager.curWeather.rainRate > 0.10000000149011612))
			{
				IntVec3 c = default(IntVec3);
				if (!RCellFinder.TryFindSkygazeCell(pawn.Position, pawn, out c))
				{
					return null;
				}
				return new Job(base.def.jobDef, c);
			}
			return null;
		}
	}
}
