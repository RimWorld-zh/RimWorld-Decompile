using System;

namespace Verse
{
	// Token: 0x02000F22 RID: 3874
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x06005CB3 RID: 23731 RVA: 0x002EF85F File Offset: 0x002EDC5F
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x002EF871 File Offset: 0x002EDC71
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x04003D88 RID: 15752
		private int ticksUntilMote = 0;
	}
}
