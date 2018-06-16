using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F26 RID: 3878
	public class SubEffecter_Sustainer : SubEffecter
	{
		// Token: 0x06005CBB RID: 23739 RVA: 0x002EF869 File Offset: 0x002EDC69
		public SubEffecter_Sustainer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x002EF884 File Offset: 0x002EDC84
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

		// Token: 0x04003D8A RID: 15754
		private int age = 0;

		// Token: 0x04003D8B RID: 15755
		private Sustainer sustainer = null;
	}
}
