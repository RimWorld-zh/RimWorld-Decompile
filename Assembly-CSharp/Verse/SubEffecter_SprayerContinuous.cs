using System;

namespace Verse
{
	// Token: 0x02000F26 RID: 3878
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x04003D9D RID: 15773
		private int ticksUntilMote = 0;

		// Token: 0x06005CE5 RID: 23781 RVA: 0x002F1F0B File Offset: 0x002F030B
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x002F1F1D File Offset: 0x002F031D
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}
	}
}
