using System;

namespace RimWorld
{
	// Token: 0x0200068E RID: 1678
	public interface IStoreSettingsParent
	{
		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x0600239C RID: 9116
		bool StorageTabVisible { get; }

		// Token: 0x0600239D RID: 9117
		StorageSettings GetStoreSettings();

		// Token: 0x0600239E RID: 9118
		StorageSettings GetParentStoreSettings();
	}
}
