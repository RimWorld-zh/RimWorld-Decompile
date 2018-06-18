using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000694 RID: 1684
	public class SlotGroup
	{
		// Token: 0x060023AE RID: 9134 RVA: 0x00131DF8 File Offset: 0x001301F8
		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060023AF RID: 9135 RVA: 0x00131E08 File Offset: 0x00130208
		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x00131E28 File Offset: 0x00130228
		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060023B1 RID: 9137 RVA: 0x00131E48 File Offset: 0x00130248
		public IEnumerable<Thing> HeldThings
		{
			get
			{
				List<IntVec3> cellsList = this.CellsList;
				Map map = this.Map;
				for (int i = 0; i < cellsList.Count; i++)
				{
					List<Thing> thingList = map.thingGrid.ThingsListAt(cellsList[i]);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j].def.EverStorable(false))
						{
							yield return thingList[j];
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x00131E74 File Offset: 0x00130274
		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x00131E94 File Offset: 0x00130294
		public IEnumerator<IntVec3> GetEnumerator()
		{
			List<IntVec3> cellsList = this.CellsList;
			for (int i = 0; i < cellsList.Count; i++)
			{
				yield return cellsList[i];
			}
			yield break;
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x00131EB6 File Offset: 0x001302B6
		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x00131EED File Offset: 0x001302ED
		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x00131F24 File Offset: 0x00130324
		public override string ToString()
		{
			string result;
			if (this.parent != null)
			{
				result = this.parent.ToString();
			}
			else
			{
				result = "NullParent";
			}
			return result;
		}

		// Token: 0x040013ED RID: 5101
		public ISlotGroupParent parent;
	}
}
