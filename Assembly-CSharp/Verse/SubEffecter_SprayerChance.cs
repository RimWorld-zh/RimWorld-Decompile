using System;

namespace Verse
{
	// Token: 0x02000F23 RID: 3875
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		// Token: 0x06005CB5 RID: 23733 RVA: 0x002EF8A9 File Offset: 0x002EDCA9
		public SubEffecter_SprayerChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x002EF8B4 File Offset: 0x002EDCB4
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
