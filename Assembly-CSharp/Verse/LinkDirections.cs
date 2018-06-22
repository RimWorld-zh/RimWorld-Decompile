using System;

namespace Verse
{
	// Token: 0x02000B15 RID: 2837
	[Flags]
	public enum LinkDirections : byte
	{
		// Token: 0x04002818 RID: 10264
		None = 0,
		// Token: 0x04002819 RID: 10265
		Up = 1,
		// Token: 0x0400281A RID: 10266
		Right = 2,
		// Token: 0x0400281B RID: 10267
		Down = 4,
		// Token: 0x0400281C RID: 10268
		Left = 8
	}
}
