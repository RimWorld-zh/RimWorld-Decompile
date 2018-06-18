using System;

namespace RimWorld
{
	// Token: 0x02000692 RID: 1682
	public interface IStoreSettingsParent
	{
		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x060023A4 RID: 9124
		bool StorageTabVisible { get; }

		// Token: 0x060023A5 RID: 9125
		StorageSettings GetStoreSettings();

		// Token: 0x060023A6 RID: 9126
		StorageSettings GetParentStoreSettings();
	}
}
