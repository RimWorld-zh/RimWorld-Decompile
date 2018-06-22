using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F20 RID: 3872
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x06005CD7 RID: 23767 RVA: 0x002F1492 File Offset: 0x002EF892
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CD8 RID: 23768 RVA: 0x002F149D File Offset: 0x002EF89D
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
