using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A29 RID: 2601
	public class Trigger_BecamePlayerEnemy : Trigger
	{
		// Token: 0x060039CE RID: 14798 RVA: 0x001E8864 File Offset: 0x001E6C64
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.FactionRelationsChanged && lord.faction != null && lord.faction.HostileTo(Faction.OfPlayer);
		}
	}
}
