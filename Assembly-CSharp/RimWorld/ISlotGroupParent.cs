using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200068F RID: 1679
	public interface ISlotGroupParent : IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x0600239F RID: 9119
		bool IgnoreStoredThingsBeauty { get; }

		// Token: 0x060023A0 RID: 9120
		IEnumerable<IntVec3> AllSlotCells();

		// Token: 0x060023A1 RID: 9121
		List<IntVec3> AllSlotCellsList();

		// Token: 0x060023A2 RID: 9122
		void Notify_ReceivedThing(Thing newItem);

		// Token: 0x060023A3 RID: 9123
		void Notify_LostThing(Thing newItem);

		// Token: 0x060023A4 RID: 9124
		string SlotYielderLabel();

		// Token: 0x060023A5 RID: 9125
		SlotGroup GetSlotGroup();
	}
}
