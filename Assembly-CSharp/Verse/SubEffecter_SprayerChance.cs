namespace Verse
{
	public class SubEffecter_SprayerChance : SubEffecter_Sprayer
	{
		public SubEffecter_SprayerChance(SubEffecterDef def) : base(def)
		{
		}

		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float num = base.def.chancePerTick;
			if (base.def.spawnLocType == MoteSpawnLocType.RandomCellOnTarget && B.HasThing)
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
