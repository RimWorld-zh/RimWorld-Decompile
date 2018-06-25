using System;

namespace Verse
{
	// Token: 0x02000F28 RID: 3880
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06005CE9 RID: 23785 RVA: 0x002F1FDB File Offset: 0x002F03DB
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x002F1FE6 File Offset: 0x002F03E6
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A, B);
		}
	}
}
