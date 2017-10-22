using Verse.Sound;

namespace Verse
{
	public class SubEffecter_Sustainer : SubEffecter
	{
		private int age = 0;

		private Sustainer sustainer = null;

		public SubEffecter_Sustainer(SubEffecterDef def) : base(def)
		{
		}

		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.age++;
			if (this.age > base.def.ticksBeforeSustainerStart)
			{
				if (this.sustainer == null)
				{
					SoundInfo info = SoundInfo.InMap(A, MaintenanceType.PerTick);
					this.sustainer = base.def.soundDef.TrySpawnSustainer(info);
				}
				else
				{
					this.sustainer.Maintain();
				}
			}
		}
	}
}
