using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_BecameColonyAlly : Trigger
	{
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.FactionRelationsChanged)
			{
				return lord.faction == null || !lord.faction.HostileTo(Faction.OfPlayer);
			}
			return false;
		}
	}
}
