using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1E RID: 2590
	public class Trigger_PawnCannotReachMapEdge : Trigger
	{
		// Token: 0x060039BC RID: 14780 RVA: 0x001E88CC File Offset: 0x001E6CCC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed)
					{
						if (!pawn.CanReachMapEdge())
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
