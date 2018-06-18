using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F25 RID: 3877
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x06005CB9 RID: 23737 RVA: 0x002EF945 File Offset: 0x002EDD45
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x002EF960 File Offset: 0x002EDD60
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

		// Token: 0x04003D89 RID: 15753
		private int age = 0;

		// Token: 0x04003D8A RID: 15754
		private Sustainer sustainer = null;
	}
}
