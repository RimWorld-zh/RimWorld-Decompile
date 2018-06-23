using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1A RID: 2586
	public class Trigger_PawnHarmed : Trigger
	{
		// Token: 0x040024AF RID: 9391
		public float chance = 1f;

		// Token: 0x040024B0 RID: 9392
		public bool requireInstigatorWithFaction = false;

		// Token: 0x040024B1 RID: 9393
		public Faction requireInstigatorWithSpecificFaction = null;

		// Token: 0x060039B2 RID: 14770 RVA: 0x001E856E File Offset: 0x001E696E
		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x001E85A8 File Offset: 0x001E69A8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && (this.requireInstigatorWithSpecificFaction == null || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == this.requireInstigatorWithSpecificFaction)) && Rand.Value < this.chance;
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x001E8654 File Offset: 0x001E6A54
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
