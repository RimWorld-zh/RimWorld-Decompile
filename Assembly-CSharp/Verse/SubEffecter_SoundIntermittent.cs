using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F1F RID: 3871
	public class SubEffecter_SoundIntermittent : SubEffecter
	{
		// Token: 0x04003D99 RID: 15769
		protected int ticksUntilSound;

		// Token: 0x06005CD5 RID: 23765 RVA: 0x002F141F File Offset: 0x002EF81F
		public SubEffecter_SoundIntermittent(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
			this.ticksUntilSound = def.intermittentSoundInterval.RandomInRange;
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x002F143C File Offset: 0x002EF83C
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
