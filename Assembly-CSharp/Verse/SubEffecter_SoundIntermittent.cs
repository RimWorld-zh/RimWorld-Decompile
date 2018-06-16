using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F20 RID: 3872
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x06005CAF RID: 23727 RVA: 0x002EF317 File Offset: 0x002ED717
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06005CB0 RID: 23728 RVA: 0x002EF334 File Offset: 0x002ED734
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}

		// Token: 0x04003D88 RID: 15752
		protected int ticksUntilSound;
	}
}
