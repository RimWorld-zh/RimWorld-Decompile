using System;

namespace Verse
{
	// Token: 0x02000F29 RID: 3881
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06005CE9 RID: 23785 RVA: 0x002F21FB File Offset: 0x002F05FB
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x002F2206 File Offset: 0x002F0606
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A, B);
		}
	}
}
