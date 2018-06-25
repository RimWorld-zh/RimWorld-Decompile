using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A28 RID: 2600
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x060039CF RID: 14799 RVA: 0x001E8FD0 File Offset: 0x001E73D0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
