namespace Verse
{
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def) : base(def)
		{
		}

		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			float chancePerTick = base.def.chancePerTick;
			if (Rand.Value < chancePerTick)
			{
				base.MakeMote(A);
			}
		}
	}
}
