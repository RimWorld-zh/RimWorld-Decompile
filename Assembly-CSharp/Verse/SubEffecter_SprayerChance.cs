using System;

namespace Verse
{
	// Token: 0x02000F24 RID: 3876
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06005CB7 RID: 23735 RVA: 0x002EF7CD File Offset: 0x002EDBCD
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x002EF7D8 File Offset: 0x002EDBD8
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
