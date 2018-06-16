using System;

namespace RimWorld
{
	// Token: 0x02000682 RID: 1666
	[Flags]
	public enum OverlayTypes
	{
		// Token: 0x040013A4 RID: 5028
		NeedsPower = 1,
		// Token: 0x040013A5 RID: 5029
		PowerOff = 2,
		// Token: 0x040013A6 RID: 5030
		BurningWick = 4,
		// Token: 0x040013A7 RID: 5031
		Forbidden = 8,
		// Token: 0x040013A8 RID: 5032
		ForbiddenBig = 16,
		// Token: 0x040013A9 RID: 5033
		QuestionMark = 32,
		// Token: 0x040013AA RID: 5034
		BrokenDown = 64,
		// Token: 0x040013AB RID: 5035
		OutOfFuel = 128
	}
}
