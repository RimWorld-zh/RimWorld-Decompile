using System;

namespace Verse
{
	// Token: 0x02000F23 RID: 3875
	public class SubEffecter_SprayerContinuous : SubEffecter_Sprayer
	{
		// Token: 0x06005CB5 RID: 23733 RVA: 0x002EF783 File Offset: 0x002EDB83
		public SubEffecter_SprayerContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x002EF795 File Offset: 0x002EDB95
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A, B);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x04003D89 RID: 15753
		private int ticksUntilMote = 0;
	}
}
