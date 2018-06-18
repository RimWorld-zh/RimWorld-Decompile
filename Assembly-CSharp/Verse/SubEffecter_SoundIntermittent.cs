using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F1F RID: 3871
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x06005CAD RID: 23725 RVA: 0x002EF3F3 File Offset: 0x002ED7F3
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x002EF410 File Offset: 0x002ED810
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilSound--;
			if (this.ticksUntilSound <= 0)
			{
				this.def.soundDef.PlayOneShot(A);
				this.ticksUntilSound = this.def.intermittentSoundInterval.RandomInRange;
			}
		}

		// Token: 0x04003D87 RID: 15751
		protected int ticksUntilSound;
	}
}
