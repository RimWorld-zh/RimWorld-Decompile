using System;

namespace Verse
{
	// Token: 0x02000F23 RID: 3875
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06005CDD RID: 23773 RVA: 0x002F18D5 File Offset: 0x002EFCD5
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x002F18E0 File Offset: 0x002EFCE0
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
