using System;

namespace Verse
{
	// Token: 0x02000E2E RID: 3630
	public struct DebugMenuOption
	{
		// Token: 0x040038DB RID: 14555
		public DebugMenuOptionMode mode;

		// Token: 0x040038DC RID: 14556
		public string label;

		// Token: 0x040038DD RID: 14557
		public Action method;

		// Token: 0x0600560C RID: 22028 RVA: 0x002C6036 File Offset: 0x002C4436
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}
	}
}
