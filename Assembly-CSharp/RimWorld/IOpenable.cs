using System;

namespace RimWorld
{
	// Token: 0x0200067C RID: 1660
	public interface IOpenable
	{
		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060022F4 RID: 8948
		bool CanOpen { get; }

		// Token: 0x060022F5 RID: 8949
		void Open();
	}
}
