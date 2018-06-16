using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F21 RID: 3873
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x06005CB1 RID: 23729 RVA: 0x002EF38A File Offset: 0x002ED78A
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CB2 RID: 23730 RVA: 0x002EF395 File Offset: 0x002ED795
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
