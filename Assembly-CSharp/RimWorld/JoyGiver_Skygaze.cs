using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_Skygaze : JoyGiver
	{
		public JoyGiver_Skygaze()
		{
		}

		public override float GetChance(Pawn pawn)
		{
			float num = pawn.Map.gameConditionManager.AggregateSkyGazeChanceFactor(pawn.Map);
			return base.GetChance(pawn) * num;
		}

		public override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			IntVec3 c;
			if (!JoyUtility.EnjoyableOutsideNow(pawn, null) || pawn.Map.weatherManager.curWeather.rainRate > 0.1f)
			{
				result = null;
			}
			else if (!RCellFinder.TryFindSkygazeCell(pawn.Position, pawn, out c))
			{
				result = null;
			}
			else
			{
				result = new Job(this.def.jobDef, c);
			}
			return result;
		}
	}
}
