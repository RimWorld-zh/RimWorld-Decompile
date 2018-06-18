using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1E RID: 2590
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x060039B8 RID: 14776 RVA: 0x001E832E File Offset: 0x001E672E
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x001E8368 File Offset: 0x001E6768
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && (this.requireInstigatorWithSpecificFaction == null || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == this.requireInstigatorWithSpecificFaction)) && Rand.Value < this.chance;
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x001E8414 File Offset: 0x001E6814
		public static bool SignalIsHarm(TriggerSignal signal)
		{
			bool result;
			if (signal.type == TriggerSignalType.PawnDamaged)
			{
				result = signal.dinfo.Def.externalViolence;
			}
			else if (signal.type == TriggerSignalType.PawnLost)
			{
				result = (signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled);
			}
			else
			{
				result = (signal.type == TriggerSignalType.PawnArrestAttempted);
			}
			return result;
		}

		// Token: 0x040024B4 RID: 9396
		public float chance = 1f;

		// Token: 0x040024B5 RID: 9397
		public bool requireInstigatorWithFaction = false;

		// Token: 0x040024B6 RID: 9398
		public Faction requireInstigatorWithSpecificFaction = null;
	}
}
