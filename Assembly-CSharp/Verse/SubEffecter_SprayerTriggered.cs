using System;

namespace Verse
{
	// Token: 0x02000F24 RID: 3876
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06005CB7 RID: 23735 RVA: 0x002EF92F File Offset: 0x002EDD2F
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x002EF93A File Offset: 0x002EDD3A
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A, B);
		}
	}
}
