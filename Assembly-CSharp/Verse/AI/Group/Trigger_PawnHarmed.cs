using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1C RID: 2588
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x040024B0 RID: 9392
		public float chance = 1f;

		// Token: 0x040024B1 RID: 9393
		public bool requireInstigatorWithFaction = false;

		// Token: 0x040024B2 RID: 9394
		public Faction requireInstigatorWithSpecificFaction = null;

		// Token: 0x060039B6 RID: 14774 RVA: 0x001E869A File Offset: 0x001E6A9A
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x001E86D4 File Offset: 0x001E6AD4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && (this.requireInstigatorWithSpecificFaction == null || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == this.requireInstigatorWithSpecificFaction)) && Rand.Value < this.chance;
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x001E8780 File Offset: 0x001E6B80
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
