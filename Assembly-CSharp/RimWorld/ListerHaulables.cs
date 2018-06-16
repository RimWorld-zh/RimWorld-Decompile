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
		// Token: 0x06000FB4 RID: 4020 RVA: 0x00084200 File Offset: 0x00082600
		public ListerHaulables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00084234 File Offset: 0x00082634
		public List<Thing> ThingsPotentiallyNeedingHauling()
		{
			return this.haulables;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x0008424F File Offset: 0x0008264F
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00084259 File Offset: 0x00082659
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00084263 File Offset: 0x00082663
		public void HaulDesignationAdded(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0008426D File Offset: 0x0008266D
		public void HaulDesignationRemoved(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00084277 File Offset: 0x00082677
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00084281 File Offset: 0x00082681
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x0008428C File Offset: 0x0008268C
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

		// Token: 0x06000FBD RID: 4029 RVA: 0x000842D0 File Offset: 0x000826D0
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

		// Token: 0x06000FBE RID: 4030 RVA: 0x0008444C File Offset: 0x0008284C
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00084490 File Offset: 0x00082890
		public void RecalcAllInCells(IEnumerable<IntVec3> cells)
		{
			foreach (IntVec3 c in cells)
			{
				this.RecalcAllInCell(c);
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000844E8 File Offset: 0x000828E8
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

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00084548 File Offset: 0x00082948
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

		// Token: 0x06000FC2 RID: 4034 RVA: 0x000845DD File Offset: 0x000829DD
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeHaulable(t) && !this.haulables.Contains(t))
			{
				this.haulables.Add(t);
			}
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00084609 File Offset: 0x00082A09
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item && this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0008463C File Offset: 0x00082A3C
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

		// Token: 0x04000998 RID: 2456
		private Map map;

		// Token: 0x04000999 RID: 2457
		private List<Thing> haulables = new List<Thing>();

		// Token: 0x0400099A RID: 2458
		private const int CellsPerTick = 4;

		// Token: 0x0400099B RID: 2459
		private static int groupCycleIndex = 0;

		// Token: 0x0400099C RID: 2460
		private List<int> cellCycleIndices = new List<int>();

		// Token: 0x0400099D RID: 2461
		private string debugOutput = "uninitialized";
	}
}
