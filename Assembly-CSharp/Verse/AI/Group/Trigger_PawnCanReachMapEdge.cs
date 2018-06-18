using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A21 RID: 2593
	public class Trigger_PawnCanReachMapEdge : Trigger
	{
		// Token: 0x060039C0 RID: 14784 RVA: 0x001E8608 File Offset: 0x001E6A08
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 193 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed)
					{
						if (!pawn.CanReachMapEdge())
						{
							return false;
						}
					}
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
