using System;

namespace RimWorld
{
	// Token: 0x0200067E RID: 1662
	public interface IOpenable
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060022F6 RID: 8950
		bool CanOpen { get; }

		// Token: 0x060022F7 RID: 8951
		void Open();
	}
}
