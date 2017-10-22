namespace Verse
{
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		private int ticksUntilMote = 0;

		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def) : base(def)
		{
		}

		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A);
				this.ticksUntilMote = base.def.ticksBetweenMotes;
			}
		}
	}
}
