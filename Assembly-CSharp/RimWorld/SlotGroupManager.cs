using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public sealed class SlotGroupManager
	{
		private Map map;

		private List<SlotGroup> allGroups = new List<SlotGroup>();

		private SlotGroup[,,] groupGrid;

		[CompilerGenerated]
		private static Comparison<SlotGroup> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Comparison<SlotGroup> _003C_003Ef__mg_0024cache1;

		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroups;
			}
		}

		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroups;
			}
		}

		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroups;
			}
		}

		public IEnumerable<IntVec3> AllSlots
		{
			get
			{
				int j = 0;
				List<IntVec3> cellsList;
				int i;
				while (true)
				{
					if (j < this.allGroups.Count)
					{
						cellsList = this.allGroups[j].CellsList;
						i = 0;
						if (i < cellsList.Count)
							break;
						j++;
						continue;
					}
					yield break;
				}
				yield return cellsList[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public SlotGroupManager(Map map)
		{
			this.map = map;
			IntVec3 size = map.Size;
			int x = size.x;
			IntVec3 size2 = map.Size;
			int y = size2.y;
			IntVec3 size3 = map.Size;
			this.groupGrid = new SlotGroup[x, y, size3.z];
		}

		public void AddGroup(SlotGroup newGroup)
		{
			if (this.allGroups.Contains(newGroup))
			{
				Log.Error("Double-added slot group. SlotGroup parent is " + newGroup.parent);
			}
			else if ((from g in this.allGroups
			where g.parent == newGroup.parent
			select g).Any())
			{
				Log.Error("Added SlotGroup with a parent matching an existing one. Parent is " + newGroup.parent);
			}
			else
			{
				this.allGroups.Add(newGroup);
				this.allGroups.InsertionSort(new Comparison<SlotGroup>(SlotGroupManager.CompareSlotGroupPrioritiesDescending));
				List<IntVec3> cellsList = newGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.SetCellFor(cellsList[i], newGroup);
				}
				this.map.listerHaulables.Notify_SlotGroupChanged(newGroup);
			}
		}

		public void RemoveGroup(SlotGroup oldGroup)
		{
			if (!this.allGroups.Contains(oldGroup))
			{
				Log.Error("Removing SlotGroup that isn't registered.");
			}
			else
			{
				this.allGroups.Remove(oldGroup);
				List<IntVec3> cellsList = oldGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					IntVec3 intVec = cellsList[i];
					this.groupGrid[intVec.x, intVec.y, intVec.z] = null;
				}
				this.map.listerHaulables.Notify_SlotGroupChanged(oldGroup);
			}
		}

		public void Notify_GroupChangedPriority()
		{
			this.allGroups.InsertionSort(new Comparison<SlotGroup>(SlotGroupManager.CompareSlotGroupPrioritiesDescending));
		}

		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		public void SetCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != null)
			{
				Log.Error(group + " overwriting slot group square " + c + " of " + this.SlotGroupAt(c));
			}
			this.groupGrid[c.x, c.y, c.z] = group;
		}

		public void ClearCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != group)
			{
				Log.Error(group + " clearing group grid square " + c + " containing " + this.SlotGroupAt(c));
			}
			this.groupGrid[c.x, c.y, c.z] = null;
		}

		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}
	}
}
