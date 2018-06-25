using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F24 RID: 3876
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x06005CE1 RID: 23777 RVA: 0x002F1B12 File Offset: 0x002EFF12
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x002F1B1D File Offset: 0x002EFF1D
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
