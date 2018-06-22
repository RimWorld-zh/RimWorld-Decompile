using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000690 RID: 1680
	public class SlotGroup
	{
		// Token: 0x060023A6 RID: 9126 RVA: 0x00131F40 File Offset: 0x00130340
		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x060023A7 RID: 9127 RVA: 0x00131F50 File Offset: 0x00130350
		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060023A8 RID: 9128 RVA: 0x00131F70 File Offset: 0x00130370
		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x060023A9 RID: 9129 RVA: 0x00131F90 File Offset: 0x00130390
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
		// (get) Token: 0x060023AA RID: 9130 RVA: 0x00131FBC File Offset: 0x001303BC
		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x00131FDC File Offset: 0x001303DC
		public IEnumerator<IntVec3> GetEnumerator()
		{
			List<IntVec3> cellsList = this.CellsList;
			for (int i = 0; i < cellsList.Count; i++)
			{
				yield return cellsList[i];
			}
			yield break;
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x00131FFE File Offset: 0x001303FE
		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x00132035 File Offset: 0x00130435
		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0013206C File Offset: 0x0013046C
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

		// Token: 0x040013EB RID: 5099
		public ISlotGroupParent parent;
	}
}
