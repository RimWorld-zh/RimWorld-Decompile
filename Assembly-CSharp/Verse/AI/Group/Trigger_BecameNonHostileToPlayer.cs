using System;
using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_BecameNonHostileToPlayer : Trigger
	{
		public Trigger_BecameNonHostileToPlayer()
		{
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && (signal.previousRelationKind == FactionRelationKind.Hostile && lord.faction != null) && !lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
