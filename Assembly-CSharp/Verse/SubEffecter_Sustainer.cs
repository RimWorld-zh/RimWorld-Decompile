using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F25 RID: 3877
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x06005CE1 RID: 23777 RVA: 0x002F1971 File Offset: 0x002EFD71
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x002F198C File Offset: 0x002EFD8C
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

		// Token: 0x04003D9B RID: 15771
		private int age = 0;

		// Token: 0x04003D9C RID: 15772
		private Sustainer sustainer = null;
	}
}
