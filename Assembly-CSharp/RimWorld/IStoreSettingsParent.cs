using System;

namespace RimWorld
{
	// Token: 0x02000690 RID: 1680
	public interface IStoreSettingsParent
	{
		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x0600239F RID: 9119
		bool StorageTabVisible { get; }

		// Token: 0x060023A0 RID: 9120
		StorageSettings GetStoreSettings();

		// Token: 0x060023A1 RID: 9121
		StorageSettings GetParentStoreSettings();
	}
}
