using System;

namespace Verse
{
	// Token: 0x02000976 RID: 2422
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x04002333 RID: 9011
		private int ticksUntilMote = 0;

		// Token: 0x06003681 RID: 13953 RVA: 0x001D1632 File Offset: 0x001CFA32
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x001D1644 File Offset: 0x001CFA44
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}
	}
}
