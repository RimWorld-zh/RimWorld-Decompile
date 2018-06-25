using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F25 RID: 3877
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x06005CE1 RID: 23777 RVA: 0x002F1D32 File Offset: 0x002F0132
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x002F1D3D File Offset: 0x002F013D
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
