using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F29 RID: 3881
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x04003D9E RID: 15774
		private int age = 0;

		// Token: 0x04003D9F RID: 15775
		private Sustainer sustainer = null;

		// Token: 0x06005CEB RID: 23787 RVA: 0x002F1FF1 File Offset: 0x002F03F1
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CEC RID: 23788 RVA: 0x002F200C File Offset: 0x002F040C
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.age++;
			if (this.age > this.def.ticksBeforeSustainerStart)
			{
				if (this.sustainer == null)
				{
					SoundInfo info = SoundInfo.InMap(A, MaintenanceType.PerTick);
					this.sustainer = this.def.soundDef.TrySpawnSustainer(info);
				}
				else
				{
					this.sustainer.Maintain();
				}
			}
		}
	}
}
