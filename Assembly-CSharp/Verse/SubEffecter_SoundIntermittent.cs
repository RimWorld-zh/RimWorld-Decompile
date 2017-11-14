using Verse.Sound;

namespace Verse
{
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		protected int ticksUntilSound;

		public SubEffecter_SoundIntermittent(SubEffecterDef def)
			: base(def)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				base.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = base.def.intermittentSoundInterval.RandomInRange;
			}
		}
	}
}
