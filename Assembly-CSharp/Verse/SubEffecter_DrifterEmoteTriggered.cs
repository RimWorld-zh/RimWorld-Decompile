using System;

namespace Verse
{
	// Token: 0x0200097A RID: 2426
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x06003688 RID: 13960 RVA: 0x001D10BA File Offset: 0x001CF4BA
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x001D10C5 File Offset: 0x001CF4C5
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A);
		}
	}
}
