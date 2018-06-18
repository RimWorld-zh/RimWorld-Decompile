using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A20 RID: 2592
	public class Trigger_PawnCannotReachMapEdge : Trigger
	{
		// Token: 0x060039BE RID: 14782 RVA: 0x001E8560 File Offset: 0x001E6960
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
