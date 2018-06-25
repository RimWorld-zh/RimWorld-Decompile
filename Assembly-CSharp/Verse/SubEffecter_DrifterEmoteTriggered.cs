using System;

namespace Verse
{
	// Token: 0x02000978 RID: 2424
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x06003685 RID: 13957 RVA: 0x001D16B6 File Offset: 0x001CFAB6
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x001D16C1 File Offset: 0x001CFAC1
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A);
		}
	}
}
