using System;

namespace RimWorld
{
	// Token: 0x02000680 RID: 1664
	[Flags]
	public enum OverlayTypes
	{
		// Token: 0x040013A6 RID: 5030
		NeedsPower = 1,
		// Token: 0x040013A7 RID: 5031
		PowerOff = 2,
		// Token: 0x040013A8 RID: 5032
		BurningWick = 4,
		// Token: 0x040013A9 RID: 5033
		Forbidden = 8,
		// Token: 0x040013AA RID: 5034
		ForbiddenBig = 16,
		// Token: 0x040013AB RID: 5035
		QuestionMark = 32,
		// Token: 0x040013AC RID: 5036
		BrokenDown = 64,
		// Token: 0x040013AD RID: 5037
		OutOfFuel = 128
	}
}
