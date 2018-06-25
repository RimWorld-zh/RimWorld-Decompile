using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A26 RID: 2598
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x060039CC RID: 14796 RVA: 0x001E8C30 File Offset: 0x001E7030
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && (signal.previousRelationKind == FactionRelationKind.Hostile && lord.faction != null) && !lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
