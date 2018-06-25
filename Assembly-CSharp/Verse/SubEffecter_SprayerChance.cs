using System;

namespace Verse
{
	// Token: 0x02000F27 RID: 3879
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06005CE7 RID: 23783 RVA: 0x002F1F55 File Offset: 0x002F0355
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x002F1F60 File Offset: 0x002F0360
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float num = this.def.chancePerTick;
			if (this.def.spawnLocType == MoteSpawnLocType.RandomCellOnTarget && B.HasThing)
			{
				num *= (float)(B.Thing.def.size.x * B.Thing.def.size.z);
			}
			if (Rand.Value < num)
			{
				base.MakeMote(A, B);
			}
		}
	}
}
