using System;

namespace RimWorld
{
	// Token: 0x020008FF RID: 2303
	[Flags]
	public enum SpectateRectSide
	{
		// Token: 0x04001CEC RID: 7404
		None = 0,
		// Token: 0x04001CED RID: 7405
		Up = 1,
		// Token: 0x04001CEE RID: 7406
		Right = 2,
		// Token: 0x04001CEF RID: 7407
		Down = 4,
		// Token: 0x04001CF0 RID: 7408
		Left = 8,
		// Token: 0x04001CF1 RID: 7409
		Vertical = 5,
		// Token: 0x04001CF2 RID: 7410
		Horizontal = 10,
		// Token: 0x04001CF3 RID: 7411
		All = 15
	}
}
