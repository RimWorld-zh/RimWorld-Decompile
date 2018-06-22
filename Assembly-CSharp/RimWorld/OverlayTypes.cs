using System;

namespace RimWorld
{
	// Token: 0x0200067E RID: 1662
	[Flags]
	public enum OverlayTypes
	{
		// Token: 0x040013A2 RID: 5026
		NeedsPower = 1,
		// Token: 0x040013A3 RID: 5027
		PowerOff = 2,
		// Token: 0x040013A4 RID: 5028
		BurningWick = 4,
		// Token: 0x040013A5 RID: 5029
		Forbidden = 8,
		// Token: 0x040013A6 RID: 5030
		ForbiddenBig = 16,
		// Token: 0x040013A7 RID: 5031
		QuestionMark = 32,
		// Token: 0x040013A8 RID: 5032
		BrokenDown = 64,
		// Token: 0x040013A9 RID: 5033
		OutOfFuel = 128
	}
}
