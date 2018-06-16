using System;

namespace Verse
{
	// Token: 0x02000978 RID: 2424
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x06003682 RID: 13954 RVA: 0x001D0F6E File Offset: 0x001CF36E
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x001D0F80 File Offset: 0x001CF380
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x0400232D RID: 9005
		private int ticksUntilMote = 0;
	}
}
