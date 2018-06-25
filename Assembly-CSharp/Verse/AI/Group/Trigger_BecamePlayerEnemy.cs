using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A27 RID: 2599
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x060039CE RID: 14798 RVA: 0x001E8CA4 File Offset: 0x001E70A4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
