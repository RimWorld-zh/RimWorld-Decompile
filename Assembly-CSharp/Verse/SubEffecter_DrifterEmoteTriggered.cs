using System;

namespace Verse
{
	// Token: 0x0200097A RID: 2426
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x06003686 RID: 13958 RVA: 0x001D0FF2 File Offset: 0x001CF3F2
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x001D0FFD File Offset: 0x001CF3FD
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A);
		}
	}
}
