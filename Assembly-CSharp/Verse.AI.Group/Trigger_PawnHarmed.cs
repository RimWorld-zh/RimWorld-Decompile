namespace Verse.AI.Group
{
	public class Trigger_PawnHarmed : Trigger
	{
		public float chance = 1f;

		public bool requireInstigatorWithFaction = false;

		public Trigger_PawnHarmed(float chance = 1f, bool requireInstigatorWithFaction = false)
		{
			this.chance = chance;
			this.requireInstigatorWithFaction = requireInstigatorWithFaction;
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return Trigger_PawnHarmed.SignalIsHarm(signal) && (!this.requireInstigatorWithFaction || (signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction != null)) && Rand.Value < this.chance;
		}

		public static bool SignalIsHarm(TriggerSignal signal)
		{
			return (byte)((signal.type != TriggerSignalType.PawnDamaged) ? ((signal.type != TriggerSignalType.PawnLost) ? ((signal.type == TriggerSignalType.PawnArrestAttempted) ? 1 : 0) : ((signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled) ? 1 : 0)) : (signal.dinfo.Def.externalViolence ? 1 : 0)) != 0;
		}
	}
}
