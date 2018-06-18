using System;

namespace Verse
{
	// Token: 0x02000978 RID: 2424
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x06003684 RID: 13956 RVA: 0x001D1036 File Offset: 0x001CF436
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x001D1048 File Offset: 0x001CF448
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
