using System;

namespace Verse
{
	// Token: 0x02000E2D RID: 3629
	public struct DebugMenuOption
	{
		// Token: 0x040038D3 RID: 14547
		public DebugMenuOptionMode mode;

		// Token: 0x040038D4 RID: 14548
		public string label;

		// Token: 0x040038D5 RID: 14549
		public Action method;

		// Token: 0x0600560C RID: 22028 RVA: 0x002C5E4A File Offset: 0x002C424A
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}
	}
}
