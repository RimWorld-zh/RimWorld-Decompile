using System;

namespace Verse
{
	// Token: 0x02000F24 RID: 3876
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06005CDF RID: 23775 RVA: 0x002F195B File Offset: 0x002EFD5B
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x002F1966 File Offset: 0x002EFD66
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A, B);
		}
	}
}
