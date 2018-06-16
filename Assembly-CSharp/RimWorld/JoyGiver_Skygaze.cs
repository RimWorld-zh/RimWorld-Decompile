using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000100 RID: 256
	public class JoyGiver_Skygaze : JoyGiver
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x0003A764 File Offset: 0x00038B64
		public override float GetChance(Pawn pawn)
		{
			float num = pawn.Map.gameConditionManager.AggregateSkyGazeChanceFactor(pawn.Map);
			return base.GetChance(pawn) * num;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0003A79C File Offset: 0x00038B9C
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
