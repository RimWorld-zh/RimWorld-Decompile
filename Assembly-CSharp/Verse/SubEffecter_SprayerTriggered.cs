using System;

namespace Verse
{
	// Token: 0x02000F25 RID: 3877
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06005CB9 RID: 23737 RVA: 0x002EF853 File Offset: 0x002EDC53
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x002EF85E File Offset: 0x002EDC5E
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A, B);
		}
	}
}
