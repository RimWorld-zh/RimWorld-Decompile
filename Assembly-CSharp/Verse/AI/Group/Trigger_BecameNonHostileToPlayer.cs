using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A27 RID: 2599
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x060039CD RID: 14797 RVA: 0x001E8F5C File Offset: 0x001E735C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && (signal.previousRelationKind == FactionRelationKind.Hostile && lord.faction != null) && !lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
