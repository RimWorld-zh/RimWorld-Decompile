using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public sealed class SlotGroupManager
	{
		private Map map;

		private List<SlotGroup> allGroups = new List<SlotGroup>();

		private SlotGroup[,,] groupGrid;

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
				for (int j = 0; j < this.allGroups.Count; j++)
				{
					List<IntVec3> cellsList = this.allGroups[j].CellsList;
					int i = 0;
					while (i < cellsList.Count)
					{
						yield return cellsList[i];
						j++;
					}
				}
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
