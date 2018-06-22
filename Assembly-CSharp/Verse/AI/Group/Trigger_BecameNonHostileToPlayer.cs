using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A24 RID: 2596
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		// Token: 0x060039C8 RID: 14792 RVA: 0x001E8B04 File Offset: 0x001E6F04
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && (signal.previousRelationKind == FactionRelationKind.Hostile && lord.faction != null) && !lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
