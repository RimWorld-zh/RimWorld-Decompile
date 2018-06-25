using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1D RID: 2589
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x040024C0 RID: 9408
		public float chance = 1f;

		// Token: 0x040024C1 RID: 9409
		public bool requireInstigatorWithFaction = false;

		// Token: 0x040024C2 RID: 9410
		public Faction requireInstigatorWithSpecificFaction = null;

		// Token: 0x060039B7 RID: 14775 RVA: 0x001E89C6 File Offset: 0x001E6DC6
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x001E8A00 File Offset: 0x001E6E00
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && (this.requireInstigatorWithSpecificFaction == null || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == this.requireInstigatorWithSpecificFaction)) && Rand.Value < this.chance;
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x001E8AAC File Offset: 0x001E6EAC
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
	}
}
