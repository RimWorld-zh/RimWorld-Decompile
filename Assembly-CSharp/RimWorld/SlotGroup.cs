using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class SlotGroup
	{
		public ISlotGroupParent parent;

		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		public IEnumerable<Thing> HeldThings
		{
			get
			{
				List<IntVec3> cellsList = this.CellsList;
				for (int j = 0; j < cellsList.Count; j++)
				{
					List<Thing> thingList = this.Map.thingGrid.ThingsListAt(cellsList[j]);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].def.EverStoreable)
						{
							yield return thingList[i];
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
		}

		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
			this.Map.slotGroupManager.AddGroup(this);
		}

		public IEnumerator<IntVec3> GetEnumerator()
		{
			int i = 0;
			if (i < this.CellsList.Count)
			{
				yield return this.CellsList[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.slotGroupManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
		}

		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.slotGroupManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
		}

		public void Notify_ParentDestroying()
		{
			this.Map.slotGroupManager.RemoveGroup(this);
		}

		public override string ToString()
		{
			if (this.parent != null)
			{
				return this.parent.ToString();
			}
			return "NullParent";
		}
	}
}
