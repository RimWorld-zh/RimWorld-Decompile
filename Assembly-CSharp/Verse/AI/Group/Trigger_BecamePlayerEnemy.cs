using System;
using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		public Trigger_BecamePlayerEnemy()
		{
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
