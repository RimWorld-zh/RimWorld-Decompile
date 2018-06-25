using System;
using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_PawnHarmed : Trigger
	{
		public float chance = 1f;

		public bool requireInstigatorWithFaction = false;

		public Faction requireInstigatorWithSpecificFaction = null;

		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false, Faction requireInstigatorWithSpecificFaction = null)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
			this.requireInstigatorWithSpecificFaction = requireInstigatorWithSpecificFaction;
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && (this.requireInstigatorWithSpecificFaction == null || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == this.requireInstigatorWithSpecificFaction)) && Rand.Value < this.chance;
		}

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
