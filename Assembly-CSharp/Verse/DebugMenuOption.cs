using System;

namespace Verse
{
	// Token: 0x02000E2E RID: 3630
	public struct DebugMenuOption
	{
		// Token: 0x060055EC RID: 21996 RVA: 0x002C4162 File Offset: 0x002C2562
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		// Token: 0x040038C5 RID: 14533
		public DebugMenuOptionMode mode;

		// Token: 0x040038C6 RID: 14534
		public string label;

		// Token: 0x040038C7 RID: 14535
		public Action method;
	}
}
