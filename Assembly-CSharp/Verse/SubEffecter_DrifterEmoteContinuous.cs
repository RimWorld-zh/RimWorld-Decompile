using System;

namespace Verse
{
	// Token: 0x02000974 RID: 2420
	public class SubEffecter_DrifterEmoteContinuous : SubEffecter_DrifterEmote
	{
		// Token: 0x0600367D RID: 13949 RVA: 0x001D121E File Offset: 0x001CF61E
		public SubEffecter_DrifterEmoteContinuous(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x001D1230 File Offset: 0x001CF630
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilMote--;
			if (this.ticksUntilMote <= 0)
			{
				base.MakeMote(A);
				this.ticksUntilMote = this.def.ticksBetweenMotes;
			}
		}

		// Token: 0x0400232B RID: 9003
		private int ticksUntilMote = 0;
	}
}
