using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A25 RID: 2597
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x060039CA RID: 14794 RVA: 0x001E8B78 File Offset: 0x001E6F78
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
