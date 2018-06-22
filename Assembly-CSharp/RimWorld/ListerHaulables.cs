using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000389 RID: 905
	public class ListerHaulables
	{
		// Token: 0x06000FB4 RID: 4020 RVA: 0x000843EC File Offset: 0x000827EC
		public ListerHaulables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00084420 File Offset: 0x00082820
		public List<Thing> ThingsPotentiallyNeedingHauling()
		{
			return this.haulables;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0008443B File Offset: 0x0008283B
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00084445 File Offset: 0x00082845
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x0008444F File Offset: 0x0008284F
		public void HaulDesignationAdded(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00084459 File Offset: 0x00082859
		public void HaulDesignationRemoved(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00084463 File Offset: 0x00082863
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x0008446D File Offset: 0x0008286D
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00084478 File Offset: 0x00082878
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			List<IntVec3> cellsList = sg.CellsList;
			if (cellsList != null)
			{
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.RecalcAllInCell(cellsList[i]);
				}
			}
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x000844BC File Offset: 0x000828BC
		public void ListerHaulablesTick()
		{
			ListerHaulables.groupCycleIndex++;
			if (ListerHaulables.groupCycleIndex >= 2147473647)
			{
				ListerHaulables.groupCycleIndex = 0;
			}
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			if (allGroupsListForReading.Count != 0)
			{
				int num = ListerHaulables.groupCycleIndex % allGroupsListForReading.Count;
				SlotGroup slotGroup = allGroupsListForReading[ListerHaulables.groupCycleIndex % allGroupsListForReading.Count];
				if (slotGroup.CellsList.Count != 0)
				{
					while (this.cellCycleIndices.Count <= num)
					{
						this.cellCycleIndices.Add(0);
					}
					if (this.cellCycleIndices[num] >= 2147473647)
					{
						this.cellCycleIndices[num] = 0;
					}
					for (int i = 0; i < 4; i++)
					{
						List<int> list;
						int index;
						(list = this.cellCycleIndices)[index = num] = list[index] + 1;
						IntVec3 c = slotGroup.CellsList[this.cellCycleIndices[num] % slotGroup.CellsList.Count];
						List<Thing> thingList = c.GetThingList(this.map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def.EverHaulable)
							{
								this.Check(thingList[j]);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00084638 File Offset: 0x00082A38
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0008467C File Offset: 0x00082A7C
		public void RecalcAllInCells(IEnumerable<IntVec3> cells)
		{
			foreach (IntVec3 c in cells)
			{
				this.RecalcAllInCell(c);
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000846D4 File Offset: 0x00082AD4
		private void Check(Thing t)
		{
			if (this.ShouldBeHaulable(t))
			{
				if (!this.haulables.Contains(t))
				{
					this.haulables.Add(t);
				}
			}
			else if (this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00084734 File Offset: 0x00082B34
		private bool ShouldBeHaulable(Thing t)
		{
			bool result;
			if (t.IsForbidden(Faction.OfPlayer))
			{
				result = false;
			}
			else
			{
				if (!t.def.alwaysHaulable)
				{
					if (!t.def.EverHaulable)
					{
						return false;
					}
					if (this.map.designationManager.DesignationOn(t, DesignationDefOf.Haul) == null && !t.IsInAnyStorage())
					{
						return false;
					}
				}
				result = !t.IsInValidBestStorage();
			}
			return result;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x000847C9 File Offset: 0x00082BC9
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeHaulable(t) && !this.haulables.Contains(t))
			{
				this.haulables.Add(t);
			}
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x000847F5 File Offset: 0x00082BF5
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item && this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00084828 File Offset: 0x00082C28
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All haulables (Count " + this.haulables.Count + ")");
				int num = 0;
				foreach (Thing thing in this.haulables)
				{
					stringBuilder.AppendLine(thing.ThingID);
					num++;
					if (num > 200)
					{
						break;
					}
				}
				this.debugOutput = stringBuilder.ToString();
			}
			return this.debugOutput;
		}

		// Token: 0x0400099A RID: 2458
		private Map map;

		// Token: 0x0400099B RID: 2459
		private List<Thing> haulables = new List<Thing>();

		// Token: 0x0400099C RID: 2460
		private const int CellsPerTick = 4;

		// Token: 0x0400099D RID: 2461
		private static int groupCycleIndex = 0;

		// Token: 0x0400099E RID: 2462
		private List<int> cellCycleIndices = new List<int>();

		// Token: 0x0400099F RID: 2463
		private string debugOutput = "uninitialized";
	}
}
