using System;

namespace Verse
{
	// Token: 0x02000976 RID: 2422
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x06003681 RID: 13953 RVA: 0x001D12A2 File Offset: 0x001CF6A2
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x001D12AD File Offset: 0x001CF6AD
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A);
		}
	}
}
