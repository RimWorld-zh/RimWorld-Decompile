using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F2A RID: 3882
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x04003DA6 RID: 15782
		private int age = 0;

		// Token: 0x04003DA7 RID: 15783
		private Sustainer sustainer = null;

		// Token: 0x06005CEB RID: 23787 RVA: 0x002F2211 File Offset: 0x002F0611
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CEC RID: 23788 RVA: 0x002F222C File Offset: 0x002F062C
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
