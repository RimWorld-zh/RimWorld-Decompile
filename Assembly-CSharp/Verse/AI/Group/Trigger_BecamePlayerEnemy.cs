using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A29 RID: 2601
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x060039D0 RID: 14800 RVA: 0x001E8938 File Offset: 0x001E6D38
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
