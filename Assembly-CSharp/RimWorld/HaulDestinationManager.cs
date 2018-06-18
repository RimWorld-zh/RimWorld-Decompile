using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000690 RID: 1680
	public sealed class HaulDestinationManager
	{
		// Token: 0x06002390 RID: 9104 RVA: 0x001316DC File Offset: 0x0012FADC
		public HaulDestinationManager(Map map)
		{
			this.map = map;
			this.groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06002391 RID: 9105 RVA: 0x00131744 File Offset: 0x0012FB44
		public IEnumerable<IHaulDestination> AllHaulDestinations
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06002392 RID: 9106 RVA: 0x00131760 File Offset: 0x0012FB60
		public List<IHaulDestination> AllHaulDestinationsListForReading
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06002393 RID: 9107 RVA: 0x0013177C File Offset: 0x0012FB7C
		public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06002394 RID: 9108 RVA: 0x00131798 File Offset: 0x0012FB98
		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06002395 RID: 9109 RVA: 0x001317B4 File Offset: 0x0012FBB4
		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06002396 RID: 9110 RVA: 0x001317D0 File Offset: 0x0012FBD0
		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06002397 RID: 9111 RVA: 0x001317EC File Offset: 0x0012FBEC
		public IEnumerable<IntVec3> AllSlots
		{
			get
			{
				for (int i = 0; i < this.allGroupsInOrder.Count; i++)
				{
					List<IntVec3> cellsList = this.allGroupsInOrder[i].CellsList;
					int j = 0;
					while (j < this.allGroupsInOrder.Count)
					{
						yield return cellsList[j];
						i++;
					}
				}
				yield break;
			}
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x00131818 File Offset: 0x0012FC18
		public void AddHaulDestination(IHaulDestination haulDestination)
		{
			if (this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Double-added haul destination " + haulDestination.ToStringSafe<IHaulDestination>(), false);
			}
			else
			{
				this.allHaulDestinationsInOrder.Add(haulDestination);
				IList<IHaulDestination> list = this.allHaulDestinationsInOrder;
				if (HaulDestinationManager.<>f__mg$cache0 == null)
				{
					HaulDestinationManager.<>f__mg$cache0 = new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending);
				}
				list.InsertionSort(HaulDestinationManager.<>f__mg$cache0);
				ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
				if (slotGroupParent != null)
				{
					SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
					if (slotGroup == null)
					{
						Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>(), false);
					}
					else
					{
						this.allGroupsInOrder.Add(slotGroup);
						IList<SlotGroup> list2 = this.allGroupsInOrder;
						if (HaulDestinationManager.<>f__mg$cache1 == null)
						{
							HaulDestinationManager.<>f__mg$cache1 = new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending);
						}
						list2.InsertionSort(HaulDestinationManager.<>f__mg$cache1);
						List<IntVec3> cellsList = slotGroup.CellsList;
						for (int i = 0; i < cellsList.Count; i++)
						{
							this.SetCellFor(cellsList[i], slotGroup);
						}
						this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
						this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
					}
				}
			}
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x00131944 File Offset: 0x0012FD44
		public void RemoveHaulDestination(IHaulDestination haulDestination)
		{
			if (!this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Removing haul destination that isn't registered " + haulDestination.ToStringSafe<IHaulDestination>(), false);
			}
			else
			{
				this.allHaulDestinationsInOrder.Remove(haulDestination);
				ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
				if (slotGroupParent != null)
				{
					SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
					if (slotGroup == null)
					{
						Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>(), false);
					}
					else
					{
						this.allGroupsInOrder.Remove(slotGroup);
						List<IntVec3> cellsList = slotGroup.CellsList;
						for (int i = 0; i < cellsList.Count; i++)
						{
							IntVec3 intVec = cellsList[i];
							this.groupGrid[intVec.x, intVec.y, intVec.z] = null;
						}
						this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
						this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
					}
				}
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x00131A40 File Offset: 0x0012FE40
		public void Notify_HaulDestinationChangedPriority()
		{
			IList<IHaulDestination> list = this.allHaulDestinationsInOrder;
			if (HaulDestinationManager.<>f__mg$cache2 == null)
			{
				HaulDestinationManager.<>f__mg$cache2 = new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending);
			}
			list.InsertionSort(HaulDestinationManager.<>f__mg$cache2);
			IList<SlotGroup> list2 = this.allGroupsInOrder;
			if (HaulDestinationManager.<>f__mg$cache3 == null)
			{
				HaulDestinationManager.<>f__mg$cache3 = new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending);
			}
			list2.InsertionSort(HaulDestinationManager.<>f__mg$cache3);
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x00131AA0 File Offset: 0x0012FEA0
		private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x00131AD4 File Offset: 0x0012FED4
		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x00131B08 File Offset: 0x0012FF08
		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x00131B40 File Offset: 0x0012FF40
		public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
		{
			SlotGroup slotGroup = this.SlotGroupAt(loc);
			return (slotGroup == null) ? null : slotGroup.parent;
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x00131B70 File Offset: 0x0012FF70
		public void SetCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != null)
			{
				Log.Error(string.Concat(new object[]
				{
					group,
					" overwriting slot group square ",
					c,
					" of ",
					this.SlotGroupAt(c)
				}), false);
			}
			this.groupGrid[c.x, c.y, c.z] = group;
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x00131BE4 File Offset: 0x0012FFE4
		public void ClearCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != group)
			{
				Log.Error(string.Concat(new object[]
				{
					group,
					" clearing group grid square ",
					c,
					" containing ",
					this.SlotGroupAt(c)
				}), false);
			}
			this.groupGrid[c.x, c.y, c.z] = null;
		}

		// Token: 0x040013E5 RID: 5093
		private Map map;

		// Token: 0x040013E6 RID: 5094
		private List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

		// Token: 0x040013E7 RID: 5095
		private List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

		// Token: 0x040013E8 RID: 5096
		private SlotGroup[,,] groupGrid;

		// Token: 0x040013E9 RID: 5097
		[CompilerGenerated]
		private static Comparison<IHaulDestination> <>f__mg$cache0;

		// Token: 0x040013EA RID: 5098
		[CompilerGenerated]
		private static Comparison<SlotGroup> <>f__mg$cache1;

		// Token: 0x040013EB RID: 5099
		[CompilerGenerated]
		private static Comparison<IHaulDestination> <>f__mg$cache2;

		// Token: 0x040013EC RID: 5100
		[CompilerGenerated]
		private static Comparison<SlotGroup> <>f__mg$cache3;
	}
}
