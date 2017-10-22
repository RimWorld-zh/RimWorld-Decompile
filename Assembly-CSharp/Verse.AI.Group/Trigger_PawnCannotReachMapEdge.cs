namespace Verse.AI.Group
{
	public class Trigger_PawnCannotReachMapEdge : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed && !pawn.CanReachMapEdge())
						goto IL_006a;
				}
			}
			bool result = false;
			goto IL_008f;
			IL_008f:
			return result;
			IL_006a:
			result = true;
			goto IL_008f;
		}
	}
}
