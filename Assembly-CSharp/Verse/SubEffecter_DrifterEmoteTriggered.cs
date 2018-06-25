using System;

namespace Verse
{
	// Token: 0x02000978 RID: 2424
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x06003685 RID: 13957 RVA: 0x001D13E2 File Offset: 0x001CF7E2
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x001D13ED File Offset: 0x001CF7ED
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A);
		}
	}
}
