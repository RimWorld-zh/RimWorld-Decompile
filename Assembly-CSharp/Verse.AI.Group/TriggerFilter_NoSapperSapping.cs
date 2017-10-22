using RimWorld;

namespace Verse.AI.Group
{
	public class TriggerFilter_NoSapperSapping : TriggerFilter
	{
		public override bool AllowActivation(Lord lord, TriggerSignal signal)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < lord.ownedPawns.Count)
				{
					Pawn pawn = lord.ownedPawns[num];
					if (pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Sapper && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.Mine && pawn.CurJob.targetA.Cell.InHorDistOf(pawn.Position, 5f))
					{
						goto IL_00c5;
					}
					if (pawn.CurJob.def == JobDefOf.UseVerbOnThing && pawn.CurJob.targetA.Cell.InHorDistOf(pawn.Position, 20f))
						goto IL_00c5;
					num++;
					continue;
				}
				result = true;
				break;
				IL_00c5:
				result = false;
				break;
			}
			return result;
		}
	}
}
