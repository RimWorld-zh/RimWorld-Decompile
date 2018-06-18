using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000441 RID: 1089
	public class RoomStatWorker_ResearchSpeedFactor : RoomStatWorker
	{
		// Token: 0x060012E5 RID: 4837 RVA: 0x000A30F8 File Offset: 0x000A14F8
		public override float GetScore(Room room)
		{
			float stat = room.GetStat(RoomStatDefOf.Cleanliness);
			return RoomStatWorker_ResearchSpeedFactor.CleanlinessFactorCurve.Evaluate(stat);
		}

		// Token: 0x04000B73 RID: 2931
		private static readonly SimpleCurve CleanlinessFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-5f, 0.75f),
				true
			},
			{
				new CurvePoint(-2.5f, 0.85f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.15f),
				true
			}
		};
	}
}
