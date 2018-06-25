using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F24 RID: 3876
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x04003DA4 RID: 15780
		protected int ticksUntilSound;

		// Token: 0x06005CDF RID: 23775 RVA: 0x002F1CBF File Offset: 0x002F00BF
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x002F1CDC File Offset: 0x002F00DC
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}
	}
}
