using System;

namespace Verse
{
	// Token: 0x02000F22 RID: 3874
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x06005CDB RID: 23771 RVA: 0x002F188B File Offset: 0x002EFC8B
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CDC RID: 23772 RVA: 0x002F189D File Offset: 0x002EFC9D
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x04003D9A RID: 15770
		private int ticksUntilMote = 0;
	}
}
