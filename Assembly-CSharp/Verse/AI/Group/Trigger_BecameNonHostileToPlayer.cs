using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A28 RID: 2600
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x060039CE RID: 14798 RVA: 0x001E88C4 File Offset: 0x001E6CC4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && (signal.previousRelationKind == FactionRelationKind.Hostile && lord.faction != null) && !lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
