using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F23 RID: 3875
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x04003D9C RID: 15772
		protected int ticksUntilSound;

		// Token: 0x06005CDF RID: 23775 RVA: 0x002F1A9F File Offset: 0x002EFE9F
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x002F1ABC File Offset: 0x002EFEBC
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
