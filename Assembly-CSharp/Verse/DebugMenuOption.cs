using System;

namespace Verse
{
	// Token: 0x02000E2B RID: 3627
	public struct DebugMenuOption
	{
		// Token: 0x040038D3 RID: 14547
		public DebugMenuOptionMode mode;

		// Token: 0x040038D4 RID: 14548
		public string label;

		// Token: 0x040038D5 RID: 14549
		public Action method;

		// Token: 0x06005608 RID: 22024 RVA: 0x002C5D1E File Offset: 0x002C411E
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}
	}
}
