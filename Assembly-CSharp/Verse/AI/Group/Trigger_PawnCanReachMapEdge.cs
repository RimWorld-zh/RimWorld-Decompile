using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A20 RID: 2592
	public class Trigger_PawnCanReachMapEdge : Trigger
	{
		// Token: 0x060039BF RID: 14783 RVA: 0x001E8CA0 File Offset: 0x001E70A0
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
