using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000691 RID: 1681
	public interface ISlotGroupParent : IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x060023A2 RID: 9122
		bool IgnoreStoredThingsBeauty { get; }

		// Token: 0x060023A3 RID: 9123
		IEnumerable<IntVec3> AllSlotCells();

		// Token: 0x060023A4 RID: 9124
		List<IntVec3> AllSlotCellsList();

		// Token: 0x060023A5 RID: 9125
		void Notify_ReceivedThing(Thing newItem);

		// Token: 0x060023A6 RID: 9126
		void Notify_LostThing(Thing newItem);

		// Token: 0x060023A7 RID: 9127
		string SlotYielderLabel();

		// Token: 0x060023A8 RID: 9128
		SlotGroup GetSlotGroup();
	}
}
