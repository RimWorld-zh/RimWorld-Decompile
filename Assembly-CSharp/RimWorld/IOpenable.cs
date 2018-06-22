using System;

namespace RimWorld
{
	// Token: 0x0200067A RID: 1658
	public interface IOpenable
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060022F0 RID: 8944
		bool CanOpen { get; }

		// Token: 0x060022F1 RID: 8945
		void Open();
	}
}
