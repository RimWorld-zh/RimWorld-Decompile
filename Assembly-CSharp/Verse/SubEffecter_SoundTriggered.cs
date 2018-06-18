using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F20 RID: 3872
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x06005CAF RID: 23727 RVA: 0x002EF466 File Offset: 0x002ED866
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CB0 RID: 23728 RVA: 0x002EF471 File Offset: 0x002ED871
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
