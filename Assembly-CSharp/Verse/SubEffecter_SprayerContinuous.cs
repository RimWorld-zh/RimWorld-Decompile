using System;

namespace Verse
{
	// Token: 0x02000F27 RID: 3879
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x04003DA5 RID: 15781
		private int ticksUntilMote = 0;

		// Token: 0x06005CE5 RID: 23781 RVA: 0x002F212B File Offset: 0x002F052B
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CE6 RID: 23782 RVA: 0x002F213D File Offset: 0x002F053D
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
