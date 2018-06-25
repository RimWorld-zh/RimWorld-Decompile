using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	public sealed class HaulDestinationManager
	{
		private Map map;

		private List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

		private List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

		private SlotGroup[,,] groupGrid;

		[CompilerGenerated]
		private static Comparison<IHaulDestination> <>f__mg$cache0;

		[CompilerGenerated]
		private static Comparison<SlotGroup> <>f__mg$cache1;

		[CompilerGenerated]
		private static Comparison<IHaulDestination> <>f__mg$cache2;

		[CompilerGenerated]
		private static Comparison<SlotGroup> <>f__mg$cache3;

		public HaulDestinationManager(Map map)
		{
			this.map = map;
			this.groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
		}

		public IEnumerable<IHaulDestination> AllHaulDestinations
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		public List<IHaulDestination> AllHaulDestinationsListForReading
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

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

		private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}

		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
		{
			SlotGroup slotGroup = this.SlotGroupAt(loc);
			return (slotGroup == null) ? null : slotGroup.parent;
		}

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

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal int <i>__1;

			internal List<IntVec3> <cellsList>__2;

			internal int <j>__3;

			internal HaulDestinationManager $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					i = 0;
					goto IL_C1;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				IL_97:
				if (j < this.allGroupsInOrder.Count)
				{
					this.$current = cellsList[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_C1:
				if (i < this.allGroupsInOrder.Count)
				{
					cellsList = this.allGroupsInOrder[i].CellsList;
					j = 0;
					goto IL_97;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				HaulDestinationManager.<>c__Iterator0 <>c__Iterator = new HaulDestinationManager.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
