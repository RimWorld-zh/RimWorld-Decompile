using System;

namespace Verse
{
	// Token: 0x02000E2F RID: 3631
	public struct DebugMenuOption
	{
		// Token: 0x060055EE RID: 21998 RVA: 0x002C4162 File Offset: 0x002C2562
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		// Token: 0x040038C7 RID: 14535
		public DebugMenuOptionMode mode;

		// Token: 0x040038C8 RID: 14536
		public string label;

		// Token: 0x040038C9 RID: 14537
		public Action method;
	}
}
