using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1D RID: 2589
	public class Trigger_PawnCanReachMapEdge : Trigger
	{
		// Token: 0x060039BA RID: 14778 RVA: 0x001E8848 File Offset: 0x001E6C48
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
